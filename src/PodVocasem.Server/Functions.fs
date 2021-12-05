namespace PodVocasem.Server

open Fable.Remoting.Server
open Fable.Remoting.AzureFunctions.Worker
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open PodVocasem.Shared.API
open PodVocasem.Shared.API.Response
open Azure.Data.Tables
open Azure.Data.Tables.FSharp
open SpotifyAPI.Web

module Service =
    let private asInt (s:string) =
        let v = s.[1..(s.Length - 1)]
        v |> int

    let getEpisodes (client:TableClient) () =
        task {
            let data =
                tableQuery {
                    maxPerPage 5
                }
                |> client.Query<TableEntity> |> Seq.toList
            return
                data |> List.map (fun x -> { Season = asInt x.PartitionKey; Episode = asInt x.RowKey; SpotifyHash = x.GetString("SpotifyHash") })
        }

    let upsertEpisode (client:TableClient) (name:string,url:string) =
        task {
            let parts = name.Split("-")
            let nums = parts.[0].Trim()
            let episodeName = parts.[1].Trim()
            let hash = url.Replace("spotify:episode:","")

            return ()
        }

type Functions(log:ILogger<Functions>, tableClient:TableClient, spotifyClient:SpotifyClient) =

    let service = {
        GetEpisodes = Service.getEpisodes tableClient >> Async.AwaitTask
    }

    [<Function("CheckEpisodes")>]
    member _.CheckEpisodes([<TimerTrigger("0 30 5 * * *", RunOnStartup = true)>] myTimer:TimerInfo) =
        task {
            let req = ShowRequest()
            req.Market <- "CZ"

            let! episodes = spotifyClient.Shows.Get("280aceAx85AKZslVytXsrB", req)
            let eps =
                episodes.Episodes.Items
                |> Seq.map (fun x -> x.Name, x.Uri)

            for e in eps do
                do! e |> Service.upsertEpisode tableClient
            return ()
        }

    [<Function("Index")>]
    member _.Index ([<HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*any}")>] req: HttpRequestData, ctx: FunctionContext) =
        Remoting.createApi()
        |> Remoting.withRouteBuilder FunctionsRouteBuilder.apiPrefix
        |> Remoting.withErrorHandler (fun exn _ ->
            log.LogError(exn, "An error occured: {Exception}")
            ErrorResult.Propagate(exn)
        )
        |> Remoting.fromValue service
        |> Remoting.buildRequestHandler
        |> HttpResponseData.fromRequestHandler req