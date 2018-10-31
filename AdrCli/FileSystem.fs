namespace AdrCli

open System.IO

module FileSystem =
    let private isProjectRootDirectory (dir : DirectoryInfo) =
        dir.GetDirectories() |> Seq.exists (fun d -> d.Name = ".adr")

    let rec directoriesToRoot =
        Seq.unfold (fun (dir : DirectoryInfo) -> match dir with | null -> None | _ -> Some (dir, dir.Parent))

    let getSettingsDirectory (currentDir : DirectoryInfo) : DirectoryInfo option =
        currentDir |> directoriesToRoot |> Seq.tryFind isProjectRootDirectory 

    let init (currentDir : DirectoryInfo) =
        currentDir.CreateSubdirectory(".adr")