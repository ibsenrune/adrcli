namespace AdrCli

module Configuration =

    open System.Text.RegularExpressions

    type Settings = { repositoryPath : string }

    let serialiseSettings (s : Settings) =
        sprintf "settings:\n    repositoryPath: %s\n"  s.repositoryPath

    let deserialiseSettings (s : string) =
        let m = System.Text.RegularExpressions.Regex.Match(s, "^    repositoryPath: (.+)$", RegexOptions.Multiline)
        if (m.Success) 
        then Some ({ repositoryPath = m.Groups.[1].Value })
        else None