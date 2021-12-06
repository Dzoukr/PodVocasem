namespace PodVocasem.Server

open System
open Azure.Data.Tables
open Fable.Remoting.Server
open Fable.Remoting.AzureFunctions.Worker
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Newtonsoft.Json
open PodVocasem.Shared.API
open PodVocasem.Shared.API.Response
open Azure.Data.Tables.FSharp
open SpotifyAPI.Web

module Service =

    type EpisodeRow = {
        Episode : SimpleEpisode
        Published : DateTimeOffset
    }

    module EpisodeRow =
        let fromSimpleEpisode (e:SimpleEpisode) = { Episode = e; Published = e.ReleaseDate |> DateTimeOffset.Parse }

        let toEntity (e:EpisodeRow) =
            let entity = TableEntity()
            entity.PartitionKey <- "PodVocasem"
            entity.RowKey <- e.Episode.Id
            entity.["Episode"] <- e.Episode |> JsonConvert.SerializeObject
            entity.["Published"] <- e.Published
            entity

        let fromEntity (e:TableEntity) =
            {
                Episode = e.GetString "Episode" |> JsonConvert.DeserializeObject<SimpleEpisode>
                Published = (e.GetDateTimeOffset "Published").Value
            }

    let private toEpisode (e:EpisodeRow) : Episode =
        let nums = e.Episode.Name.Split("-").[0]
        let seas = (nums.[1] + nums.[2]) |> int
        {
            Season = seas
            SpotifyHash = e.Episode.Id
        }

    let private getEpisodes (client:TableClient) () =
        task {
            let data =
                tableQuery {
                    filter (pk "PodVocasem")
                }
                |> client.Query<TableEntity>
                |> Seq.toList
                |> List.map EpisodeRow.fromEntity
                |> List.sortByDescending (fun x -> x.Published)

            return
                data
                |> List.map toEpisode
        }

    let get tableClient = {
        GetEpisodes = getEpisodes tableClient >> Async.AwaitTask
    }

    let upsertEpisode (client:TableClient) (e:SimpleEpisode) =
        task {
            let entity = e |> EpisodeRow.fromSimpleEpisode |> EpisodeRow.toEntity
            let! _ = client.UpsertEntityAsync(entity, TableUpdateMode.Merge)
            return ()
        }

type Functions(log:ILogger<Functions>, tableClient:TableClient, spotifyClient:SpotifyClient) =

    [<Function("CheckEpisodes")>]
    member _.CheckEpisodes([<TimerTrigger("0 30 5 * * *", RunOnStartup = false)>] myTimer:TimerInfo) =
        task {
            let req = ShowRequest()
            req.Market <- "CZ"

            let! episodes = spotifyClient.Shows.Get("280aceAx85AKZslVytXsrB", req)
            let toInsert = episodes.Episodes.Items
            for e in toInsert do
                do! e |> Service.upsertEpisode tableClient
            return ()
        }

    [<Function("Index")>]
    member _.Index ([<HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*any}")>] req: HttpRequestData, ctx: FunctionContext) =
        Remoting.createApi()
        |> Remoting.withRouteBuilder FunctionsRouteBuilder.apiPrefix
        |> Remoting.fromValue (Service.get tableClient)
        |> Remoting.buildRequestHandler
        |> HttpResponseData.fromRequestHandler req