module PodVocasem.Shared.API

module Response =
    type Episode = {
        Season : int
        Episode : int
        SpotifyHash : string
    }

type Service = {
    GetEpisodes : unit -> Async<Response.Episode list>
}
with
    static member RouteBuilder s m = sprintf "/api/%s/%s" s m