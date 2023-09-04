module PodVocasem.SpotifyChecker.Program

open System
open Azure.Data.Tables
open Azure.Storage.Blobs
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open PodVocasem.SpotifyChecker.SpotifyChecker
open SpotifyAPI.Web

let private addDependencies (builder:HostApplicationBuilder) =
    let config =
        SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(ClientCredentialsAuthenticator(builder.Configuration.["SpotifyClientId"], builder.Configuration.["SpotifyClientSecret"]))

    let blobClient = BlobContainerClient(builder.Configuration.["StorageAccount"], "messages")
    let _ = blobClient.CreateIfNotExists()

    let tableClient = TableClient(builder.Configuration.["StorageAccount"], "EpisodesSpotify")
    let _ = tableClient.CreateIfNotExists()

    builder.Services
        .AddSingleton<SpotifyClient>(SpotifyClient(config))
        .AddSingleton<BlobContainerClient>(blobClient)
        .AddSingleton<TableClient>(tableClient)
        .AddHostedService<SpotifyChecker.SpotifyChecker>()
        |> ignore
    builder


let builder =
    Host.CreateApplicationBuilder()
    |> addDependencies

let app = builder.Build()
app.Run()
