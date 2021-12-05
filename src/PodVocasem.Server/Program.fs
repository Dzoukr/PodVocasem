module PodVocasem.Server.Program

open System
open System.Threading.Tasks
open Azure.Data.Tables
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Azure.Functions.Worker.Configuration
open Microsoft.Extensions.DependencyInjection
open SpotifyAPI.Web

let configureServices (ctx:HostBuilderContext) (svcs:IServiceCollection) =
    svcs.AddSingleton<TableClient>(TableClient(ctx.Configuration.["StorageAccount"], "Episodes")) |> ignore

    let config =
        SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(ClientCredentialsAuthenticator(ctx.Configuration.["SpotifyClientId"], ctx.Configuration.["SpotifyClientSecret"]))
    svcs.AddSingleton<SpotifyClient>(SpotifyClient(config)) |> ignore
    ()

[<EntryPoint>]
(HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices (configureServices))
    .Build()
    .Run()