namespace AdrCli

open System.IO
open System.Text.RegularExpressions

module FileSystem =

    type Settings = { repositoryPath : string }

    let private isProjectRootDirectory (dir : DirectoryInfo) =
        dir.GetDirectories() |> Seq.exists (fun d -> d.Name = ".adr")

    let private parent (d : DirectoryInfo) = d.Parent

    let directoriesToRoot dir =
        let rec follow next x = seq {
             yield x; yield! follow next (next x)
        }
        dir |> follow parent |> Seq.takeWhile (not << isNull)

    let getSettingsDirectory (currentDir : DirectoryInfo) : DirectoryInfo option =
        currentDir |> directoriesToRoot |> Seq.tryFind isProjectRootDirectory 

    let cwd = System.Environment.CurrentDirectory |> DirectoryInfo

    let init (currentDir : DirectoryInfo) (settings : Settings) =
        currentDir.CreateSubdirectory(".adr") |> ignore
        System.IO.Directory.CreateDirectory(settings.repositoryPath) |> ignore

    let serialiseSettings (s : Settings) =
       sprintf "settings:\n    repositoryPath: %s\n"  s.repositoryPath

    let deserialiseSettings (s : string) =
        let m = System.Text.RegularExpressions.Regex.Match(s, "^    repositoryPath: (.+)$", RegexOptions.Multiline)
        if (m.Success) 
        then Some ({ repositoryPath = m.Groups.[1].Value })
        else None
