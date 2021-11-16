module PodVocasem.Client.SharedView

open Feliz
open Router

type prop
    with
        static member inline href (p:Page) = prop.href (p |> Page.toUrlSegments |> Router.formatPath)

type Html
    with
        static member inline a (text:string, p:Page) =
            Html.a [
                prop.href p
                prop.onClick Router.goToUrl
                prop.text text
            ]
        static member inline classed fn (cn:string) (elm:ReactElement list) =
            fn [
                prop.className cn
                prop.children elm
            ]
        static member inline divClassed (cn:string) (elm:ReactElement list) = Html.classed Html.div cn elm
        static member inline headerClassed (cn:string) (elm:ReactElement list) = Html.classed Html.header cn elm
