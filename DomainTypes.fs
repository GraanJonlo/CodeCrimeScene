module Git

type Sha = Sha of string

type Signature = {
    name:string
    email:string
    time:System.DateTimeOffset
}

type Blob = {
    id:Sha
    isBinary:bool
}

type Tree = {
    id:Sha
    // treeEntries:TreeEntry seq
}

and TreeEntry =
    | Blob of Blob
    | Tree of Tree

type Commit = {
    id:Sha
    signature:Signature
    message:string
    messageShort:string
    tree:Tree
}
type GitObject =
    | Blob of Blob
    | Commit of Commit
    | Tree of Tree

type Repository = Repository of string

type Branch = {
    repository:Repository
}
