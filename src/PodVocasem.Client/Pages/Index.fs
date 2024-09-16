module PodVocasem.Client.Pages.Index

open Fable.SimpleJson
open Feliz
open Elmish
open Feliz.UseElmish
open PodVocasem.Client
open SharedView
open Fable.Core.JsInterop
open Fable.SimpleHttp

type Episode = {
    Season : int
    Name : string
    Description : string
    SpotifyHash : string
}

type State = {
    Episodes: Episode list
    IsLoading : bool
}

type Msg =
    | GetEpisodes
    | EpisodesDownloaded of Episode list

let init () = { Episodes = []; IsLoading = false }, Cmd.ofMsg GetEpisodes

let private upload url =
    async {
        return ()
    }

let private getEpisodes () =
    async {
        let! (_, responseText) = Http.get "/data/episodes.json"
        return responseText |> Json.parseNativeAs<Episode []> |> Array.toList
    }

let update (msg:Msg) (model:State) : State * Cmd<Msg> =
    match msg with
    | GetEpisodes -> { model with IsLoading = true }, Cmd.OfAsync.perform getEpisodes () EpisodesDownloaded
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

let mcs : string = importDefault "../assets/img/mcs.jpg"
let loader : string = importDefault "../assets/img/loader.png"

let playBox (e:Episode) =
    Html.divClassed "flex flex-col overflow-hidden rounded-lg shadow-lg bg-gray-700" [
        Html.divClassed "flex-shrink-0 text-gray-50 p-4" [

            Html.divClassed "flex flex-row gap-4" [
                Html.divClassed "basis-5/12 flex flex-col gap-2" [
                    Html.img [ prop.className "bg-gray-50 rounded-lg"; prop.src "/img/default.png" ]

                    Html.a [
                        prop.className "text-center rounded-lg border border-gray-50 p-3 hover:bg-gray-50 hover:text-gray-700"
                        prop.text "▶ Přehrát epizodu"
                        prop.href $"https://open.spotify.com/episode/{e.SpotifyHash}?go=1"
                    ]
                ]
                Html.divClassed "basis-7/12 flex flex-col gap-4" [
                    Html.divClassed "tex-slate-50 font-bold" [
                        Html.text e.Name
                    ]
                    Html.divClassed "line-clamp-5" [
                        Html.text e.Description
                    ]
                ]
            ]
        ]
    ]

let partnerBox (logo:string) (desc:string) (linkHref:string) (linkName:string) =
    Html.divClassed "py-8 sm:py-16 px-8 md:px-16 lg:px-32 text-center text-gray-700" [
        Html.divClassed "text-2xl font-semibold" [ Html.text "Partnerem této série je" ]
        Html.a [
            prop.href linkHref
            prop.children [
                Html.img [ prop.src logo; prop.className "mx-auto w-64" ]
            ]
        ]
        Html.divClassed "mx-auto w-96 text-lg" [ Html.text desc ]

        Html.divClassed "mx-auto w-96 mt-2 underline text-lg font-medium" [
            Html.a [
                prop.href linkHref
                prop.text linkName
            ]
        ]
    ]

[<ReactComponent>]
let IndexView () =
    let state, dispatch = React.useElmish(init, update, [| |])

    let maxSerie =
        state.Episodes
        |> List.map (fun x -> x.Season)
        |> List.sortDescending
        |> List.tryHead
        |> Option.defaultValue 1

    Html.divClassed "flex flex-col min-h-screen" [

        Html.headerClassed "relative bg-hero py-8 sm:py-16" [
            Html.divClassed "flex flex-col items-start justify-center h-full text-default max-w-screen-xl px-8 md:px-16 lg:px-32" [
                Html.divClassed "mb-8" [ Html.img [ prop.className "w-24 sm:w-32 md:w-40"; prop.src "/img/logo.png" ] ]
                Html.classed Html.h1 "text-4xl font-semibold leading-none mb-4 sm:text-7xl md:text-8xl" [ Html.text "IT podcast, který rozhodně neplave po povrchu." ]
                Html.divClassed "text-xl leading-tight mb-12 sm:text-2xl md:text-3xl" [
                    Html.text "Zajímaví hosté v hodinovém pořadu vysílaném "
                    Html.a [ prop.className "text-blue-800 hover:underline"; prop.href "https://twitter.com/podvocasem"; prop.text "#PodVocasem" ]
                ]
                Html.divClassed "mb-2 text-lg md:text-xl" [ Html.text "Poslouchej nás na své oblíbené platformě:" ]
                Html.divClassed "sm:flex items-center" [
                    podcastBtn "https://open.spotify.com/show/280aceAx85AKZslVytXsrB?si=50b87e50890746b7" "spotify" "Spotify"
                    podcastBtn "https://podcasts.apple.com/us/podcast/podvocasem/id1590431276" "apple-podcast" "Apple Podcasts"
                ]
            ]
        ]

        partnerBox "/img/partners/vivio.png" "PŘINÁŠÍME ŽIVOT DO VAŠÍ ONLINE REKLAMY. Pomůžeme vám s PPC kampaněmi i celkovou marketingovou strategií." "https://www.vivio.cz/" "www.vivio.cz"

        Html.divClassed "flex-grow max-w-full" [
            Html.classed Html.main "block py-12" [

                if state.IsLoading then
                    Html.divClassed "flex justify-center h-24" [
                        Html.img [ prop.src loader; prop.className "animate-spin" ]
                    ]
                else
                    for s in ([1..maxSerie] |> List.rev) do
                        Html.classed Html.article "px-8 md:px-16 lg:px-32 pb-12 lg:pb-16" [
                            Html.classed Html.h1 "text-gray-700" [ Html.text $"PodVocasem - {s}. série" ]
                            Html.divClassed "relative" [
                                Html.divClassed "relative mx-auto" [
                                    Html.divClassed "grid gap-8 mx-auto mt-12 md:grid-cols-2 xl:grid-cols-3 lg:max-w-none" [
                                        for e in (state.Episodes |> List.filter (fun x -> x.Season = s)) do
                                            yield playBox e
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
                                prop.href "https://www.ciklum.com/we"
                                prop.children [
                                    Html.img [
                                        prop.src "/svg/ciklum-logo.svg"
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