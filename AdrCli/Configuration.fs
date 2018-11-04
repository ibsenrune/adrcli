namespace AdrCli

module Configuration =

    open System.Text.RegularExpressions

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern, RegexOptions.Multiline)
        if(m.Success)
        then Some (m.Groups.[1].Value)
        else None

    type Settings = { 
        editor : string
        template : string
        repositoryPath : string }

    let serialiseSettings (s : Settings) =
        sprintf "settings:\n    repositoryPath: %s\n    editor: %s\n    template: %s\n"  s.repositoryPath s.editor s.template

    let deserialiseSettings (s : string) =
        optional {
            let! repositoryPath = (|Regex|_|) "^    repositoryPath: (.+)$" s
            let! editor = (|Regex|_|) "^    editor: (.+)$" s
            let! template = (|Regex|_|) "^    template: (.+)$" s
            return { 
                repositoryPath = repositoryPath
                editor = editor
                template = template
            }
        }

