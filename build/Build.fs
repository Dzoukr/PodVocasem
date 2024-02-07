open Fake
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

open BuildHelpers
open BuildTools
open SpotifyChecker

initializeContext()

let publishPath = Path.getFullName "publish"
let srcPath = Path.getFullName "src"
let clientSrcPath = srcPath </> "PodVocasem.Client"
let appPublishPath = publishPath </> "app"


// Targets
let clean proj = [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

Target.create "InstallClient" (fun _ ->
    printfn "Node version:"
    Tools.node "--version" clientSrcPath
    printfn "Yarn version:"
    Tools.yarn "--version" clientSrcPath
    Tools.yarn "install --frozen-lockfile" clientSrcPath
)

Target.create "Publish" (fun _ ->
    Tools.yarn "build" ""
)

Target.create "GenerateJson" (fun x ->
    let clientId = x.Context.Arguments.[0]
    let clientSecret = x.Context.Arguments.[1]
    generateJson { ClientId = clientId; ClientSecret = clientSecret } (clientSrcPath </> "public/data/episodes.json")
    |> Async.AwaitTask
    |> Async.RunSynchronously
)

Target.create "Run" (fun _ ->
    Tools.yarn "start" ""
)

let dependencies = [
    "InstallClient"
        ==> "Publish"

    "InstallClient"
        ==> "Run"
]

[<EntryPoint>]
let main args = runOrDefault "Run" args