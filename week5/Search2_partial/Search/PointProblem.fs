﻿module PointProblem
open TreeSearch

// states are (x,y) tuples
let startState = (0,0)
let goalState = (3,3)
let goalTest s = s = goalState

// actions
let goNorth (x,y) = (x, y+1)
let goEast (x,y) = (x+1, y)
let goNorthEast (x,y) = (x+1, y+1)
let actions = [|goNorth; goEast; goNorthEast|]
let names = [|"goNorth"; "goEast"; "goNorthEast"|]
let costs = [| 1.0; 1.0; 1.414214 |]


let problem = { Start = startState;
                IsGoal = goalTest;
                Actions = actions;
                Names = names;
                Costs = costs; }

let s = dfs problem
printfn "%A" s
printfn "%A" nodesExpanded
System.Console.ReadLine() |> ignore
