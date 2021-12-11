module PodVocasem.Client.Pages.Index

open Feliz
open Elmish
open Feliz.UseElmish
open PodVocasem.Client
open PodVocasem.Shared.API
open SharedView

type State = {
    Episodes: Response.Episode list
    IsLoading : bool
}

type Msg =
    | GetEpisodes
    | EpisodesDownloaded of Response.Episode list

let init () = { Episodes = []; IsLoading = false }, Cmd.ofMsg GetEpisodes

let update (msg:Msg) (model:State) : State * Cmd<Msg> =
    match msg with
    | GetEpisodes -> { model with IsLoading = true }, Cmd.OfAsync.perform Server.service.GetEpisodes () EpisodesDownloaded
    | EpisodesDownloaded episodes -> { model with Episodes = episodes; IsLoading = false }, Cmd.none

let podcastBtn link (svg:string) (name:string) =
    Html.a [
        prop.className $"btn btn-{svg}"
        prop.href link
        prop.children [
            Html.divClassed "w-8 h-8" [
                Html.img [
                    prop.src $"/svg/{svg}.svg"
                ]
            ]
            Html.classed Html.span "ml-2" [ Html.text name ]
        ]
    ]

open Fable.Core.JsInterop

let mcs : string = importDefault "../assets/img/mcs.jpg"
let loader : string = importDefault "../assets/img/loader.png"

[<ReactComponent>]
let IndexView () =
    let state, dispatch = React.useElmish(init, update, [| |])

    let playBox (e:Response.Episode) =
        Html.divClassed "flex flex-col overflow-hidden rounded-lg shadow-lg" [
            Html.divClassed "flex-shrink-0" [
                Html.iframe [
                    prop.src $"https://open.spotify.com/embed/episode/{e.SpotifyHash}?theme=0"
                    prop.width (length.percent 100)
                    prop.height 232
                    prop.allow.autoplay
                    prop.allow.encryptedMedia
                    prop.allow.fullscreen
                    prop.allow.pictureInPicture
                ]
            ]
        ]

    Html.divClassed "flex flex-col min-h-screen" [
        Html.headerClassed "relative bg-hero py-8 sm:py-16" [
            Html.divClassed "flex flex-col items-start justify-center h-full text-default max-w-screen-xl px-8 md:px-16 lg:px-32" [
                Html.divClassed "mb-8" [ Html.img [ prop.className "w-24 sm:w-32 md:w-40"; prop.src "/svg/logo.svg" ] ]
                Html.classed Html.h1 "text-4xl font-semibold leading-none mb-4 sm:text-7xl md:text-8xl" [ Html.text "IT podcast, který rozhodně neplave po povrchu." ]
                Html.divClassed "text-xl leading-tight mb-12 sm:text-2xl md:text-3xl" [
                    Html.text "Zajímaví hosté v hodinovém pořadu vysílaném "
                    Html.a [ prop.className "text-blue-800 hover:underline"; prop.href "https://twitter.com/podvocasem"; prop.text "#PodVocasem" ]
                ]
                Html.divClassed "mb-2 text-lg md:text-xl" [ Html.text "Poslouchej nás na své oblíbené platformě:" ]
                Html.divClassed "sm:flex items-center" [
                    podcastBtn "https://open.spotify.com/show/280aceAx85AKZslVytXsrB?si=50b87e50890746b7" "spotify" "Spotify"
                    podcastBtn "https://podcasts.apple.com/us/podcast/podvocasem/id1590431276" "apple-podcast" "Apple Podcasts"
                    podcastBtn "https://podcasts.google.com/feed/aHR0cHM6Ly9mZWVkLnBvZHZvY2FzZW0uY3ovcnNz" "google-podcast" "Google Podcasts"
                ]
            ]
        ]

        Html.divClassed "flex-grow max-w-full" [
            Html.classed Html.main "block py-12" [
                Html.classed Html.article "px-8 md:px-16 lg:px-32" [
                    Html.classed Html.h1 "text-gray-700" [ Html.text "PodVocasem - 1. série" ]
                    Html.classed Html.section "mt-12" [
                        Html.divClassed "relative pb-20 lg:pb-28" [
                            Html.divClassed "relative mx-auto" [
                                if state.IsLoading then
                                    Html.divClassed "flex justify-center h-24" [
                                        Html.img [ prop.src loader; prop.className "animate-spin" ]
                                    ]
                                else
                                    Html.divClassed "grid gap-8 mx-auto mt-12 md:grid-cols-2 xl:grid-cols-3 lg:max-w-none" [
                                        for e in state.Episodes do
                                            yield playBox e
                                    ]
                            ]
                        ]
                    ]
                ]
            ]
        ]

        Html.footer [
            Html.classed Html.section "bg-gray-100" [
                Html.divClassed "max-w-screen-xl px-4 py-12 mx-auto sm:px-6 md:flex md:items-center lg:px-8" [
                    Html.img [
                        prop.className "mb-2 md:mr-4"
                        prop.src mcs
                    ]
                    Html.p [
                        Html.text "Roman \"Džoukr\" Provazník a Petr \"Poli\" Polák v novém IT podcastu, který rozhodně neplave po povrchu. Zajímaví hosté a neotřelá témata do hloubky v hodinovém pořadu vysílaném přímo "
                        Html.a [ prop.className "text-blue-800 hover:underline"; prop.href "https://twitter.com/podvocasem"; prop.text "#PodVocasem" ]
                        Html.text "!"
                    ]
                ]
            ]
            Html.divClassed "bg-gray-900" [
                Html.divClassed "max-w-screen-xl px-4 py-12 mx-auto sm:px-6 md:flex md:items-center md:justify-between lg:px-8" [
                    Html.divClassed "flex justify-center items-center md:order-2 text-gray-100" [
                        Html.a [
                            prop.href "https://open.spotify.com/show/280aceAx85AKZslVytXsrB?si=50b87e50890746b7"
                            prop.className "hover:text-gray-200"
                            prop.children [ Html.img [ prop.className "mx-2 h-8 w-8"; prop.src "/svg/spotify-bw.svg" ] ]
                        ]
                        Html.a [
                            prop.href "https://podcasts.apple.com/us/podcast/podvocasem/id1590431276"
                            prop.className "hover:text-gray-200"
                            prop.children [ Html.img [ prop.className "mx-2 h-8 w-8"; prop.src "/svg/apple-podcast-bw.svg" ] ]
                        ]
                        Html.a [
                            prop.href "https://podcasts.google.com/feed/aHR0cHM6Ly9mZWVkLnBvZHZvY2FzZW0uY3ovcnNz"
                            prop.className "hover:text-gray-200"
                            prop.children [ Html.img [ prop.className "mx-2 h-8 w-8"; prop.src "/svg/google-podcast-bw.svg" ] ]
                        ]
                        Html.a [
                            prop.href "https://twitter.com/podvocasem"
                            prop.className "hover:text-gray-200"
                            prop.children [ Html.img [ prop.className "mx-2 h-6 w-6 invert"; prop.src "/svg/twitter.svg" ] ]
                        ]
                    ]
                    Html.divClassed "mt-8 md:mt-0 md:order-1" [
                        Html.classed Html.p "text-center md:flex md:items-center text-base leading-6 text-gray-100" [
                            Html.text "Supported by "
                            Html.a [
                                prop.className "mx-auto mt-2 md:mt-0 md:ml-2"
                                prop.href "https://www.cngroup.dk"
                                prop.children [
                                    Html.img [
                                        prop.src "/svg/cn-logo-w.svg"
                                        prop.className "mx-auto mt-2 md:mt-0 md:ml-2 h-10"
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]