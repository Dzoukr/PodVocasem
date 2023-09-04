module PodVocasem.SpotifyChecker.SpotifyChecker

open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open SpotifyAPI.Web
open Azure.Data.Tables

type SpotifyChecker(host:IHostApplicationLifetime, logger:ILogger<_>, spotifyClient:SpotifyClient, tableClient:TableClient) =
    interface IHostedService with
        member this.StartAsync _ =
            task {
                try
                    logger.LogInformation("Checking Spotify API for new episodes")

                    let req = ShowRequest()
                    req.Market <- "CZ"

                    let! episodes = spotifyClient.Shows.Get("280aceAx85AKZslVytXsrB", req)
                    let toInsert = episodes.Episodes.Items

                    logger.LogInformation("Found {EpisodesCount} episodes", toInsert.Count)

                    for e in toInsert do
                        do! e |> PodVocasem.Libraries.Service.upsertEpisode tableClient
                    ()
                    host.StopApplication()
                with ex ->
                    logger.LogError(ex, "Spotify update failed")
            }

        member this.StopAsync _ = Task.CompletedTask