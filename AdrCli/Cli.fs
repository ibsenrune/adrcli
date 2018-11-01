namespace AdrCli

open McMaster.Extensions.CommandLineUtils
open AdrCli

module Options =
    let ofOption (opt : CommandOption) =
        if opt.HasValue() then
            opt.Value() |> Some
        else
            None

module Cli =
    open Options

    let private initAction (cmd : CommandLineApplication) =
        cmd.HelpOption() |> ignore
        let optionPath = cmd.Option("-p|--path <PATH>", "The path to the directory where you want to store your ADR files.", CommandOptionType.SingleValue)
        cmd.OnExecute(fun () -> FileSystem.init FileSystem.cwd (ofOption optionPath)) |> ignore

    let private addAction (cmd : CommandLineApplication) =
        cmd.HelpOption() |> ignore
        let nameOption = cmd.Option("-n|--name", "The title of the ADR document you want to add", CommandOptionType.SingleValue)
        cmd.OnExecute(fun () -> ()) |> ignore

    let app = CommandLineApplication()
    app.HelpOption() |> ignore
    app.Command("init", initAction) |> ignore
    app.Command("add", addAction) |> ignore