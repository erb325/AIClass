module TreeSearch

type Problem<'State> = 
    { Start : 'State; 
      IsGoal : 'State -> bool; 
      SameState : 'State -> 'State -> bool
      Heuristic : 'State -> double
      Actions : array<'State -> 'State>; 
      Names : array<string>
      Costs : array<double>}

type Node<'State> =
    { State : 'State;
      Parent : 'State;
      Action : int;
      Depth : int;
      Cost : double
      Value : double
    }

let mutable nodesExpanded = 0
let mutable nodesInMemory = 0

let expand problem node =
    nodesExpanded <- nodesExpanded + 1
    [ for i = 0 to problem.Actions.Length-1 do 
        let action = problem.Actions.[i]
        yield { State = action node.State;
                Parent = node.State;
                Action = i;
                Depth = node.Depth + 1
                Cost = node.Cost + problem.Costs.[i];
                Value = 0.0 }
    ]

let treeSearch problem combiner =
    let start = { State = problem.Start;
                  Parent = problem.Start;
                  Action = -1;  // not a valid array index - no action
                  Depth = 0;
                  Cost = 0.0;
                  Value = 0.0 }

    let mutable frontier = [start]
    let mutable goalSatisfied = false
    let mutable currentNode = start

    while (not (List.isEmpty frontier)) && (not goalSatisfied) do
        currentNode <- frontier.Head
        //printfn "%A" currentNode.State  // can use for debugging but comment out when timing searches
        if problem.IsGoal currentNode.State then
            goalSatisfied <- true
        else
            frontier <-  combiner  (expand problem frontier.Head) frontier.Tail
            if frontier.Length > nodesInMemory then
                nodesInMemory <- frontier.Length
    (goalSatisfied, currentNode)

let dfs problem = 
    nodesExpanded <- 0
    nodesInMemory <- 0
    treeSearch problem List.append

let bfs problem = 
    let prepend children old = old @ children 
    nodesExpanded <- 0
    nodesInMemory <- 0
    treeSearch problem prepend

let ucs problem = 
    let ucCombine l1 l2 = (l1 @ l2) |> List.sortBy (fun n -> n.Cost )
    nodesExpanded <- 0
    nodesInMemory <- 0
    treeSearch problem ucCombine

let gbfs problem =
    nodesExpanded <- 0
    nodesInMemory <- 0
    let gbfCombine children old =
        let newChildren = [ for n in children -> { n with Value = problem.Heuristic n.State} ]
        (newChildren @ old) |> List.sortBy (fun n -> n.Value )
    treeSearch problem gbfCombine

let aStar problem = 
    nodesExpanded <- 0
    nodesInMemory <- 0
    let aCombine children old = 
        let newChildren = [ for n in children -> { n with Value = problem.Heuristic n.State + n.Cost} ]
        (newChildren @ old) |> List.sortBy (fun n -> n.Value ) 
    treeSearch problem aCombine

let dls problem maxDepth = 
    nodesExpanded <- 0
    nodesInMemory <- 0
    let dlsCombine children old = 
        (children @ old) |> List.filter(fun n -> n.Depth <= maxDepth)
    treeSearch problem dlsCombine
    
let ids problem = 
     let mutable limit = 1
     let mutable solve = dls problem limit
     while not (fst solve) do
        limit <- limit + 1
        solve <- dls problem limit
     dls problem limit 
         