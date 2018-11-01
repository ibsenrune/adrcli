// Learn more about F# at http://fsharp.org

open System
open AdrCli

[<EntryPoint>]
let main argv =
    Cli.app.Execute(argv) |> ignore
    0 // return an integer exit code
