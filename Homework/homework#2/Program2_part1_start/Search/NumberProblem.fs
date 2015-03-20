//Ember Baker

module NumberProblem
open TreeSearch
open System

// states are integers
let startState = 1
let goalState = 127
let goalTest s = s = goalState

// actions
let incrementIt state = state + 1
let doubleIt state = state * 2
let timesTenIt state = state * 10

let actions = [|incrementIt; doubleIt; timesTenIt|]
let names = [|"increment"; "double"; "timesTen"|]
let costs = [| 1.0; 0.5; 100.0 |]

// heuristic evaluation of state's distance from goal - not too accurate, but admissible
let heuristic state =  
    match state with
    | x when x = goalState -> 0.0           //from here till the last comment is what we added in class
    | x when 2*x = goalState -> 0.5
    | x when x+1 = goalState -> 1.0
    | x when 4*x = goalState -> 1.0
    | x when 2*(x+1) = goalState -> 1.5
    | x when 2*x+1 = goalState -> 1.5
    | x when 8*x = goalState -> 1.5
    | _ -> 2.0                              // end of added code from class 

let sameState s1 s2 = s1 = s2

let problem = { Start = startState;
                IsGoal = goalTest;
                SameState = sameState;
                Actions = actions;
                Names = names;
                Costs = costs;
                Heuristic = heuristic }

//do bfs
let mutable startTime = System.DateTime.Now
let mutable goalNode = bfs problem |> snd
let mutable finishTime = System.DateTime.Now
let mutable elapsed = (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by bfs" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

// do depth-first search
startTime <- System.DateTime.Now
goalNode<-  dfs problem |> snd
finishTime <- System.DateTime.Now
elapsed<-  (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by dfs" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

//do UCS
startTime <- System.DateTime.Now
goalNode<- ucs problem |> snd
finishTime <- System.DateTime.Now
elapsed<-  (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by ucs" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

//do greedy best first search 
startTime<- System.DateTime.Now
goalNode<-  gbfs problem |> snd
finishTime <- System.DateTime.Now
elapsed<-  (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by Greedy Best First Search" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

//do astar
startTime <- System.DateTime.Now
goalNode<-  aStar problem |> snd
finishTime <- System.DateTime.Now
elapsed<- (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by A*" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

// do iterative deeping search
startTime <- System.DateTime.Now
goalNode<-  ids problem |> snd
finishTime <- System.DateTime.Now
elapsed<-  (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by ids" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

System.Console.ReadLine() |> ignore
