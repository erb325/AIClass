﻿//Ember Baker

module EightPuzzle
open GraphSearch
open System
open System.IO

// states are (3x3) 2D arrays of integer
type State = int [,]

let sameState (s1:State) (s2:State) = 
    let mutable same = true
    for row in 0..2 do
        for col in 0..2 do
            if s1.[row,col] <> s2.[row,col] then
                same <- false
    same

let load2DarrayFromFile filename (a:int [,]) =
    let infile = File.OpenText filename
    for row in 0 .. 2 do
        for col in 0 .. 2 do
            a.[row,col] <- infile.ReadLine() |> int
    infile.Close()

let goalState = Array2D.zeroCreate<int> 3 3
load2DarrayFromFile "goal.txt" goalState  
let goalTest s = sameState s goalState
  
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
let holeSpot  = findSpot hole // currying - uses findspot with first arg = 0
  
// actions
//type Action = State -> State

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

let h2 (state:State) = 
    let mutable differences = 0
    let value = 0
    for row in 0..2 do
        for col in 0..2 do
            if state.[row,col] <> goalState.[row,col] then
                match state.[row, col] with 
                | 1 -> (differences <- differences + abs(row-0) + abs(col-0))
                | 2 -> (differences <- differences + abs(row-0) + abs(col-1))
                | 3 -> (differences <- differences + abs(row-0) + abs(col-2))
                | 4 -> (differences <- differences + abs(row-1) + abs(col-0))
                | 5 -> (differences <- differences + abs(row-1) + abs(col-1))
                | 6 -> (differences <- differences + abs(row-1) + abs(col-2))
                | 7 -> (differences <- differences + abs(row-2) + abs(col-0))
                | 8 -> (differences <- differences + abs(row-2) + abs(col-1))
                | 0 -> (differences <- differences + abs(row-2) + abs(col-2))
                
    differences |> double
                       
let startState = Array2D.zeroCreate<int> 3 3
load2DarrayFromFile "start20.txt" startState


let problem = { Start = startState;
                IsGoal = goalTest;
                SameState = sameState;
                Heuristic = h1;
                Actions = actions;
                Names = names;
                Costs = costs }


let mutable startTime = System.DateTime.Now
let mutable goalNode = aStar problem |> snd
let mutable finishTime = System.DateTime.Now
let mutable elapsed = (finishTime - startTime).TotalSeconds

(*//do bfs
startTime <- System.DateTime.Now
goalNode<-  bfs problem |> snd
finishTime <- System.DateTime.Now
elapsed<-  (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by bfs" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""*)

//do astar
startTime <- System.DateTime.Now
goalNode<-  aStar problem |> snd
finishTime <- System.DateTime.Now
elapsed<-  (finishTime - startTime).TotalSeconds
printfn "%A" goalNode
printfn "%d nodes were expanded by a*" nodesExpanded
printfn "Max frontier size = %d" nodesInMemory
printfn "Took time: %A" elapsed
printfn ""



System.Console.ReadLine() |> ignore


