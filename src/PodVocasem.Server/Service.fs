module PodVocasem.Server.Service

open System
open Azure.Data.Tables
open Azure.Data.Tables.FSharp
open Azure.Storage.Blobs
open Newtonsoft.Json
open PodVocasem.Shared.API.Response
open SpotifyAPI.Web

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

let getEpisodes (client:TableClient) () =
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

let upsertEpisode (client:TableClient) (e:SimpleEpisode) =
    task {
        let entity = e |> EpisodeRow.fromSimpleEpisode |> EpisodeRow.toEntity
        let! _ = client.UpsertEntityAsync(entity, TableUpdateMode.Merge)
        return ()
    }

let uploadMessage (client:BlobContainerClient) (data:byte []) =
    task {
        let name = DateTimeOffset.UtcNow.ToString("yyyyMMdd-hhmmss.wav")
        let! _ = client.UploadBlobAsync(name, BinaryData.FromBytes data)
        return ()
    }