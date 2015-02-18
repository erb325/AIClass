module TreeSearch

type Problem<'State> = 
    { Start : 'State; 
      IsGoal : 'State -> bool; 
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
    nodesExpanded <- 0
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
        printfn "%A" currentNode.State
        if problem.IsGoal currentNode.State then
            goalSatisfied <- true
        else
            frontier <-  combiner  (expand problem frontier.Head) frontier.Tail
    (goalSatisfied, currentNode)

let dfs problem = treeSearch problem List.append

let bfs problem = 
    let prepend children old = old @ children
    treeSearch problem prepend
    //could also say: let bfs problem= treeSearch (fun l1 l2 -> l2 @ l1)

let ucs problem = 
    let ucCombine children old = ( children @ old ) |> List.sortBy (fun n -> n.Cost)
    treeSearch problem ucCombine