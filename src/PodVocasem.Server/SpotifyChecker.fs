module PodVocasem.Server.SpotifyChecker

open System
open System.Threading
open Azure.Data.Tables
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks
open SpotifyAPI.Web
open PodVocasem.Server.Service

type SpotifyChecker (logger:ILogger<SpotifyChecker>, spotifyClient:SpotifyClient, tableClient:TableClient) =
    inherit BackgroundService()

    override this.ExecuteAsync(stoppingToken) =
        let timer = new PeriodicTimer(TimeSpan.FromHours(1))
        task {
            while not stoppingToken.IsCancellationRequested do

                logger.LogInformation("Checking Spotify API for new episodes")

                let req = ShowRequest()
                req.Market <- "CZ"

                let! episodes = spotifyClient.Shows.Get("280aceAx85AKZslVytXsrB", req)
                let toInsert = episodes.Episodes.Items

                logger.LogInformation("Found {EpisodesCount} episodes", toInsert.Count)

                for e in toInsert do
                    do! e |> upsertEpisode tableClient

                let! _ = timer.WaitForNextTickAsync(stoppingToken)
                ()
        }