namespace AdrCli

open System.IO
open System.Text.RegularExpressions
open AdrCli.Configuration

module FileSystem =

    let private filename = "settings.yaml"
    let private writeAllText path text = File.WriteAllText(path, text, System.Text.Encoding.UTF8)
    let private readAllText path = File.ReadAllText(path, System.Text.Encoding.UTF8)
    
    let writeSettings (s : Settings) (settingsDirectory : DirectoryInfo) =
        let path = Path.Combine(settingsDirectory.FullName, filename)
        s |> serialiseSettings |> writeAllText path

    let readSettings (settingsDirectory : DirectoryInfo) =
        let path = Path.Combine(settingsDirectory.FullName, filename)
        readAllText path |> deserialiseSettings
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
        let settingsDirectory = currentDir.CreateSubdirectory(".adr")
        System.IO.Directory.CreateDirectory(settings.repositoryPath) |> ignore
        writeSettings settings settingsDirectory
        ()

