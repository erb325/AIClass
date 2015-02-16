module NumberProblem
open TreeSearch

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

printfn "%A" (dfs problem)
let s = System.Console.ReadLine()
