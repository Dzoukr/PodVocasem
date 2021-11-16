﻿module PodVocasem.Shared.API

type Service = {
    GetMessage : unit -> Async<string>
}
with
    static member RouteBuilder s m = sprintf "/api/%s/%s" s m