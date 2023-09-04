module PodVocasem.Client.View

open Feliz
open Router
open Feliz.UseElmish
open Elmish
open SharedView

type Msg =
    | UrlChanged of Page

type State = {
    Page : Page
}

let init () =
    let nextPage = Router.currentPath() |> Page.parseFromUrlSegments
    { Page = nextPage }, Cmd.navigatePage nextPage

let update (msg:Msg) (state:State) : State * Cmd<Msg> =
    match msg with
    | UrlChanged page -> { state with Page = page }, Cmd.none

[<ReactComponent>]
let AppView () =
    let state,dispatch = React.useElmish(init, update)
    let render =
        match state.Page with
        | Page.Index -> Pages.Index.IndexView ()
    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children [ render ]
    ]