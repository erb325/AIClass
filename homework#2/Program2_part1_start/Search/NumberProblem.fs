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
    (match goalState - state with
                       | 0 -> 0.0
                       | 1 -> 1.0
                       | x when ((double x) / (double state)) = 1.0 -> 0.5
                       | _ -> 500000.0) 

let sameState s1 s2 = s1 = s2

let problem = { Start = startState;
                IsGoal = goalTest;
                SameState = sameState;
                Actions = actions;
                Names = names;
                Costs = costs;
                Heuristic = heuristic }

// do depth-first search
let mutable startTime = System.DateTime.Now
let mutable goalNode = dfs problem |> snd
let mutable finishTime = System.DateTime.Now
let mutable elapsed = (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by dfs" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

//do greedy best first search 
let mutable startTime1 = System.DateTime.Now
let mutable goalNode1 = gbfs problem |> snd
let mutable finishTime1 = System.DateTime.Now
let mutable elapsed1 = (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by Greedy Best First Search" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

//do astar
let mutable startTime2 = System.DateTime.Now
let mutable goalNode2 = aStar problem |> snd
let mutable finishTime2 = System.DateTime.Now
let mutable elapsed2 = (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by A*" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""

System.Console.ReadLine() |> ignore
