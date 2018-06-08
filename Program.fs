open Git

module Convert =
    let blob (x:LibGit2Sharp.Blob) =
        { id = Sha x.Sha; isBinary = x.IsBinary }
    let signature (x:LibGit2Sharp.Signature) =
        { name = x.Name; email = x.Email; time = x.When }

module Repository =
    open LibGit2Sharp

    let lookup (Sha id) (Repository path) =
        use repo = new LibGit2Sharp.Repository(path)
        match repo.Lookup(id) with
        | :? LibGit2Sharp.Blob as blob -> GitObject.Blob <| Convert.blob blob
        | :? LibGit2Sharp.Commit as commit -> 
            let tree = { id = Sha commit.Tree.Sha }
            GitObject.Commit { id = Sha commit.Sha; signature = Convert.signature commit.Author; message = commit.Message; messageShort = commit.MessageShort; tree = tree }
        | :? LibGit2Sharp.Tree as tree ->
            GitObject.Tree { id = Sha tree.Sha }

[<EntryPoint>]
let main _ =
    let repo = Repository @"C:\projects\src\github.com\GraanJonlo\CodeCrimeScene"
    let id = Sha "a74df61ac53788464a585abfae4aee4cf6e65c8f"
    let gitObject = Repository.lookup id repo
    match gitObject with
    | Blob _ ->
        printfn "Blob"
    | Commit commit ->
        printfn "Author: %s" commit.signature.name
        printfn "Message: %s" commit.messageShort
    | Tree _ ->
        printfn "Tree"
    0
