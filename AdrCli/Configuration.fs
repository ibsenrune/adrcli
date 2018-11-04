namespace AdrCli

module Configuration =

    open System.Text.RegularExpressions

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern, RegexOptions.Multiline)
        if(m.Success)
        then Some (List.tail [ for g in m.Groups -> g.Value ])
        else None

    type Settings = { 
        editor : string
        template : string
        repositoryPath : string }

    let serialiseSettings (s : Settings) =
        sprintf "settings:\n    repositoryPath: %s\n    editor: %s\n    template: %s\n"  s.repositoryPath s.editor s.template

    let deserialiseSettings (s : string) =
        match s with
        | Regex @"^    repositoryPath: (.+)$.*^    editor: (.*)$.*^    template: (.*)$" [repositoryPath; editor; template] -> 
            Some { repositoryPath = repositoryPath; editor = editor; template = template }
        | _ -> None