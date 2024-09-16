module SpotifyChecker

open System.Text.Json
open System.IO
open SpotifyAPI.Web

type SpotifyConfiguration = {
    ClientId : string
    ClientSecret : string
}

type Episode = {
    Season : int
    Name : string
    Description : string
    SpotifyHash : string
}

module Episode =
    let ofSimpleEpisode (e:SimpleEpisode) =
        let nums = e.Name.Split("-").[0]
        let seas = (string nums.[1] + string nums.[2]) |> int
        {
            Season = seas
            Name = e.Name
            Description = e.Description
            SpotifyHash = e.Id
        }

let generateJson (cfg:SpotifyConfiguration) (jsonFilename:string) =
    let config =
        SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(ClientCredentialsAuthenticator(cfg.ClientId, cfg.ClientSecret))
    let client = SpotifyClient(config)

    task {
        let req = ShowRequest()
        req.Market <- "CZ"

        let! episodes = client.Shows.Get("280aceAx85AKZslVytXsrB", req)
        let rows = ResizeArray<Episode>()
        episodes.Episodes.Limit <- 500

        episodes.Episodes.Items
        |> Seq.iter (Episode.ofSimpleEpisode >> rows.Add)

        use stream = File.Create jsonFilename
        do! JsonSerializer.SerializeAsync(stream, rows)
    }
