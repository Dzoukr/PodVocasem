module PodVocasem.Server.Program

open Microsoft.Extensions.Hosting

[<EntryPoint>]
(HostBuilder()
    .ConfigureFunctionsWorkerDefaults())
    .Build()
    .Run()