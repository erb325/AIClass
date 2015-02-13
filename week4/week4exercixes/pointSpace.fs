// Ember Baker
module pointSpace
open treeSearch

//states are ordered pairs (x,y)

//actions transform a point by moving north east ot northeast
let goNorth (x,y) = (x, y+1)
let goEast (x,y) = (x+1, y)
let goNorthEast (x,y) = (x+1, y+1)

let actions = [goNorth; goEast; goNorthEast]
printfn "%A" (dfs (3,4) actions [[0,0]])
System.Console.ReadLine() |>ignore 
