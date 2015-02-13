module treeSearch

let neighbors state actions= 
    [for action in actions -> action state]

let extend path actions =
    printfn "%A" (List.rev path)
    let children = actions |> neighbors (path |> List.head)
    [for child in children -> child :: path ]

let rec bfs goal actions paths =
    match paths with
    |[] -> []
    | first::rest when first |> List.head = goal -> List.rev first
    | first::rest -> (rest @(extend first actions)) |> bfs goal actions

let rec dfs goal actions paths =
    match paths with
    |[] -> []
    | first::rest when first |> List.head = goal -> List.rev first
    | first::rest -> (rest @(extend first actions)) |> dfs goal actions
