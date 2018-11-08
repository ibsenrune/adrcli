namespace AdrCli

[<AutoOpen>]
module Optional =
   type OptionalBuilder() =
       member __.Bind(opt, binder) =
         match opt with
         | Some value -> binder value
         | None -> None
       member __.Return(value) = Some value

    let optional = OptionalBuilder()