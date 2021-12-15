module PodVocasem.Server.WebApp

open Azure.Data.Tables
open Giraffe
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Giraffe.GoodRead
open PodVocasem.Shared.API
open PodVocasem.Server.Service

let getService tableClient = {
    GetEpisodes = getEpisodes tableClient >> Async.AwaitTask
}

let webApp : HttpHandler =
    let remoting tableClient =
        Remoting.createApi()
        |> Remoting.withRouteBuilder Service.RouteBuilder
        |> Remoting.fromValue (getService tableClient)
        |> Remoting.buildHttpHandler
    choose [
        Require.services<TableClient> remoting
        htmlFile "public/index.html"
    ]