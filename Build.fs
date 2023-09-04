open Fake
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

open BuildHelpers
open BuildTools

initializeContext()

let publishPath = Path.getFullName "publish"
let srcPath = Path.getFullName "src"
let clientSrcPath = srcPath </> "PodVocasem.Client"
let webSrcPath = srcPath </> "PodVocasem.Server"
let jobSrcPath = srcPath </> "PodVocasem.SpotifyChecker"
let webAppPublishPath = publishPath </> "app-web"
let jobAppPublishPath = publishPath </> "app-job"

// Targets
let clean proj = [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

Target.create "InstallClient" (fun _ ->
    printfn "Node version:"
    Tools.node "--version" clientSrcPath
    printfn "Yarn version:"
    Tools.yarn "--version" clientSrcPath
    Tools.yarn "install --frozen-lockfile" clientSrcPath
)

Target.create "PublishWeb" (fun _ ->
    [ webAppPublishPath ] |> Shell.cleanDirs
    let publishArgs = sprintf "publish -c Release -o \"%s\"" webAppPublishPath
    Tools.dotnet publishArgs webSrcPath
    [ webAppPublishPath </> "appsettings.Development.json" ] |> File.deleteAll
    Tools.yarn "build" ""
)

Target.create "PublishJob" (fun _ ->
    [ jobAppPublishPath ] |> Shell.cleanDirs
    let publishArgs = sprintf "publish -c Release -o \"%s\"" jobAppPublishPath
    Tools.dotnet publishArgs jobSrcPath
    [ jobAppPublishPath </> "appsettings.Development.json" ] |> File.deleteAll
)

Target.create "Run" (fun _ ->
    let server = async {
        Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "Development"
        Tools.dotnet "watch run" webSrcPath
    }
    let client = async {
        Tools.yarn "start" ""
    }
    [server;client]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

let dependencies = [
    "InstallClient"
        ==> "PublishWeb"

    "InstallClient"
        ==> "Run"
]

[<EntryPoint>]
let main args = runOrDefault "Run" args