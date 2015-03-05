module Flags
open Minimax

// states are integers

let start = 21

let over state = state <= 0  // is game over?

let utility state player =   // only use if over is true
    match player with
    | Max -> -1.0
    | _ -> 1.0

// actions
let takeOne state = state - 1
let takeTwo state = state - 2
let takeThree state = state - 3
let actions = [|takeOne; takeTwo; takeThree|]
let names = [|"take-one"; "take-two"; "take-three"|]

let game = {    Start = start;
                IsOver = over;
                Actions = actions;
                Names = names;
                Utility = utility}

printfn "Initial state is %A" start
printfn "Do you want to go first? (y/n)"
let s = System.Console.ReadLine()
match s with 
    | "y" -> play game start Max
    | _ -> play game start Min

System.Console.ReadLine() |> ignore