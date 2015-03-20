module NumberProblem
open TreeSearch
open System

// states are integers
//type State = int
let startState = 1
let goalState = 21
let goalTest s = s = goalState

// actions
//type Action = State -> State
let incrementIt state = state + 1
let doubleIt state = state * 2
let actions = [|incrementIt; doubleIt|]
let names = [|"increment"; "double"|]
let costs = [| 1.0; 5.0 |]


let problem = { Start = startState;
                IsGoal = goalTest;
                Actions = actions;
                Names = names;
                Costs = costs; }

let startTime= DateTime.Now
let (satisfied, g) = ucs problem
let finishTime = DateTime.Now
let elapsed = finishTime - startTime 

printfn "%A" g
printfn "with %d steps" g.Depth
printfn "%d nodes were expanded" nodesExpanded
printfn "took time: %A" elapsed 

let s = System.Console.ReadLine()
