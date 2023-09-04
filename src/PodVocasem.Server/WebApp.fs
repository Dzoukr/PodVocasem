module PodVocasem.Server.WebApp

open Azure.Data.Tables
open Azure.Storage.Blobs
open FsToolkit.ErrorHandling
open Giraffe
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Giraffe.GoodRead
open Microsoft.Extensions.Logging
open PodVocasem.Shared.API
open PodVocasem.Libraries.Service

let private toEpisode (e:EpisodeRow) : Response.Episode =
    let nums = e.Episode.Name.Split("-").[0]
    let seas = (string nums.[1] + string nums.[2]) |> int
    {
        Season = seas
        Name = e.Episode.Name
        Description = e.Episode.Description
        SpotifyHash = e.Episode.Id
    }

let getService tableClient blobClient = {
    GetEpisodes = getEpisodes tableClient >> Task.map (List.map toEpisode) >> Async.AwaitTask
    UploadMessage = uploadMessage blobClient >> Async.AwaitTask
}

let webApp : HttpHandler =
    let remoting logger tableClient blobClient =
        Remoting.createApi()
        |> Remoting.withRouteBuilder Service.RouteBuilder
        |> Remoting.withErrorHandler (Remoting.errorHandler logger)
        |> Remoting.fromValue (getService tableClient blobClient)
        |> Remoting.buildHttpHandler
    choose [
        Require.services<ILogger<_>> (fun logger ->
            Require.services<TableClient,BlobContainerClient> (remoting logger)
        )
        htmlFile "public/index.html"
    ]