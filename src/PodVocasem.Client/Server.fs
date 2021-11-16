module PodVocasem.Client.Server

open Fable.Core
open Fable.Remoting.Client
open PodVocasem.Shared.API

[<Emit("config.baseUrl")>]
let baseUrl : string = jsNative

let service =
    Remoting.createApi()
    |> Remoting.withBaseUrl baseUrl
    |> Remoting.withRouteBuilder Service.RouteBuilder
    |> Remoting.buildProxy<Service>