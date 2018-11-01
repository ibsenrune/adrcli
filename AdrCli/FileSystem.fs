namespace AdrCli

open System.IO

module FileSystem =
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

    let init (currentDir : DirectoryInfo) (repositoryPath : string option) =
        currentDir.CreateSubdirectory(".adr") |> ignore
        if(repositoryPath |> Option.isSome) then
            System.IO.Directory.CreateDirectory(repositoryPath |> Option.get) |> ignore
