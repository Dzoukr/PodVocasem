module PodVocasem.Client.Server

open Fable.Core
open Fable.Remoting.Client
open PodVocasem.Shared.API

let service =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Service.RouteBuilder
    |> Remoting.buildProxy<Service>