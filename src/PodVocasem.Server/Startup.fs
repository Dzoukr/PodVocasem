module PodVocasem.Server.Startup

open Azure.Data.Tables
open Azure.Storage.Blobs
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Giraffe
open SpotifyAPI.Web

type Startup(cfg:IConfiguration, env:IWebHostEnvironment) =
    let config =
        SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(ClientCredentialsAuthenticator(cfg.["SpotifyClientId"], cfg.["SpotifyClientSecret"]))

    member _.ConfigureServices (services:IServiceCollection) =
        services
            .AddApplicationInsightsTelemetry(cfg.["APPINSIGHTS_INSTRUMENTATIONKEY"])
            .AddSingleton<BlobContainerClient>(BlobContainerClient(cfg.["StorageAccount"], "messages"))
            .AddSingleton<TableClient>(TableClient(cfg.["StorageAccount"], "Episodes"))
            .AddSingleton<SpotifyClient>(SpotifyClient(config))
            .AddHostedService<SpotifyChecker.SpotifyChecker>()
            .AddGiraffe() |> ignore
    member _.Configure(app:IApplicationBuilder) =
        app
            .UseStaticFiles()
            .UseGiraffe WebApp.webApp