module EightPuzzleSearch 

    open System.IO

    type Problem<'State> = 
          { Start : 'State; 
            IsGoal : 'State -> bool; 
            Heuristic : 'State -> double
            SameState : 'State -> 'State -> bool
            Actions : array<'State -> 'State>; 
            Names : array<string>
            Costs : array<double> }

    type Node<'State> =
          { State : 'State;
            Ancestors : List<Node<'State>>;
            Action : int;
            Depth : int;
            Cost : double;
            Value : double }

    let printPath node =
        let path = [ for nd in node::node.Ancestors -> nd.State ] |> List.rev
        printfn "%A" path

    let printActions node problem =
        let steps = [ for nd in (List.rev (node::node.Ancestors)).Tail -> problem.Names.[nd.Action] ]
        printfn "%A" steps

    let extractSolution node = 
        let steps = [ for nd in (List.rev (node::node.Ancestors)).Tail -> nd.Action ]
        //printfn "%A" path
        List.toArray steps
           
    let mutable nodesExpanded = 0
    //let expanded = Set.empty

    let expand problem node expanded =
        nodesExpanded <- nodesExpanded + 1
        [ for i = 0 to problem.Actions.Length-1 do 
            let action = problem.Actions.[i]
            let child = action node.State
            yield {     State = action node.State;
                        Ancestors = node::node.Ancestors
                        Action = i
                        Depth = node.Depth + 1
                        Cost = node.Cost + problem.Costs.[i];
                        Value = 0.0 }
        ]

    let graphSearch problem combiner =
        nodesExpanded <- 0 // won't be right for ids
        let start = {   State = problem.Start;
                        Ancestors = [];
                        Action = -1;  // not a valid array index - no action
                        Depth = 0;
                        Cost = 0.0;
                        Value = 0.0}

        let mutable frontier = [start]
        let mutable expanded  = []
        let mutable goalSatisfied = false
        let mutable currentNode = start

        while (not (List.isEmpty frontier)) && (not goalSatisfied) do
            currentNode <- frontier.Head
            if problem.IsGoal currentNode.State then
                goalSatisfied <- true
                else
                frontier <-  combiner (expand problem frontier.Head expanded) frontier.Tail
        currentNode

    let dfs problem = 
        let myappend (children: Node<'State> list) old = children @ old
        graphSearch problem myappend

    let bfs problem = 
        let prepend children old = (old @ children)
        graphSearch problem prepend
    // or let bfs problem = graphSearch (fun l1 l2 -> l2 @ l1) problem

    let ucs problem = 
        let ucCombine l1 l2 = (l1 @ l2) |> List.sortBy (fun n -> n.Cost )
        graphSearch problem ucCombine

    let gbfs problem =
        let gbfCombine children old =
            let newChildren = [ for n in children -> { n with Value = problem.Heuristic n.State} ]
            (newChildren @ old) |> List.sortBy (fun n -> n.Value )
        graphSearch problem gbfCombine

    
   
    // states are (3x3) 2D arrays of integer
    type State = int [,]

    let startState = Array2D.zeroCreate<int> 3 3

    let goalState = Array2D.zeroCreate<int> 3 3

  
    let sameState (s1:State) (s2:State) = 
        let mutable same = true
        for row in 0..2 do
            for col in 0..2 do
                if s1.[row,col] <> s2.[row,col] then
                    same <- false
        same

    let copyState (s:State) = 
        let newState = Array2D.zeroCreate<int> 3 3
        for row in 0..2 do
            for col in 0..2 do
                newState.[row,col] <- s.[row,col]
        newState

    let findSpot num (s:State) =
        let mutable spot = (0,0)
        for row in 0..2 do
            for col in 0..2 do
                if s.[row,col] = num then
                    spot <- (row,col)
        spot

    let hole = 0

    let holeSpot  = findSpot 0 // currying - uses findspot with first arg = 0
      

    // actions
    
    let moveHoleUp (s:State) = 
        let result = copyState s
        let (hr,hc) = holeSpot s
        if hr > 0 then
            result.[hr,hc] <- s.[hr-1,hc] // copy item above into hole
            result.[hr-1,hc] <- hole      // copy hole into spot above
        result
    
    let moveHoleDown (s:State) = 
        let result = copyState s
        let (hr,hc) = holeSpot s
        if hr < 2 then
            result.[hr,hc] <- s.[hr+1,hc] // copy item below into hole
            result.[hr+1,hc] <- hole      // copy hole into spot below
        result
    
    let moveHoleLeft (s:State) = 
        let result = copyState s
        let (hr,hc) = holeSpot s
        if hc > 0 then
            result.[hr,hc] <- s.[hr,hc-1] // copy item on left into hole
            result.[hr,hc-1] <- hole      // copy hole into spot on left
        result
    
    let moveHoleRight (s:State) = 
        let result = copyState s
        let (hr,hc) = holeSpot s
        if hc < 2 then
            result.[hr,hc] <- s.[hr,hc+1] // copy item on right into hole
            result.[hr,hc+1] <- hole      // copy hole into spot on right
        result
       
    let actions = [| moveHoleUp; moveHoleDown; moveHoleLeft; moveHoleRight |]
    let names = [| "up"; "down"; "left"; "right" |]
    let costs = [| 1.0; 1.0; 1.0; 1.0 |]

    // heuristic evaluation of state's distance from goal
    let h1 (state:State) = 
        let mutable differences = 0
        for row in 0..2 do
            for col in 0..2 do
                if state.[row,col] <> goalState.[row,col] then
                    differences <- differences + 1
        differences |> double

    
    
    let ss =        [| [| 0; 7; 8 |]; 
                       [| 3; 1; 2 |];
                       [| 6; 4; 5 |] |]

    let gs =        [| [| 1; 2; 3 |]; 
                       [| 4; 5; 6 |];
                       [| 7; 8; 0 |] |]
    
    let init2D (state:State) (array: int [][]) =
        for row in 0 .. 2 do
            for col in 0 .. 2 do
                state.[row,col] <- array.[row].[col]

    init2D startState ss
    init2D goalState gs
                                         
    let goalTest s = sameState s goalState

    
    let theproblem = {  Start = startState;
                        IsGoal = goalTest;
                        Heuristic = h1;
                        SameState = sameState;
                        Actions = actions;
                        Names = names;
                        Costs = costs }

    

