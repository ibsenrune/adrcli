namespace AdrCli

open McMaster.Extensions.CommandLineUtils
open AdrCli
open McMaster.Extensions.CommandLineUtils

module Options =
    let ofOption (opt : CommandOption) =
        if opt.HasValue() then
            opt.Value() |> Some
        else
            None

    let withCommand name (action : CommandLineApplication -> unit) (application : CommandLineApplication) = 
        application.Command(name, action) |> ignore
        application

    let withHelp (app : CommandLineApplication) =
        app.HelpOption() |> ignore
        app

    let onExecute (invoke : unit -> unit) (cmd : CommandLineApplication) =
        cmd.OnExecute(System.Action(invoke))
        ()

module Cli =
    open Options

    let private initAction (cmd : CommandLineApplication) =
        let optionPath = cmd.Option("-p|--path <PATH>", "The path to the directory where you want to store your ADR files.", CommandOptionType.SingleValue)
        
        cmd
        |> withHelp
        |> onExecute (fun () -> FileSystem.init FileSystem.cwd (ofOption optionPath)) 

    let private addAction (cmd : CommandLineApplication) =
        let nameOption = cmd.Option("-n|--name", "The title of the ADR document you want to add", CommandOptionType.SingleValue)

        cmd
        |> withHelp
        |> onExecute (fun () -> ())

    let app = 
        CommandLineApplication()
        |> withHelp
        |> withCommand "init" initAction
        |> withCommand "add" addAction
