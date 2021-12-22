module PodVocasem.Server.WebApp

open Azure.Data.Tables
open Azure.Storage.Blobs
open Giraffe
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Giraffe.GoodRead
open Microsoft.Extensions.Logging
open PodVocasem.Shared.API
open PodVocasem.Server.Service

let getService tableClient blobClient = {
    GetEpisodes = getEpisodes tableClient >> Async.AwaitTask
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