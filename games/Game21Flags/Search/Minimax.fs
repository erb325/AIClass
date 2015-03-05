module Minimax
open System

type Player = 
    | Max 
    | Min

type Game<'State> = {   Start : 'State; 
                        IsOver : 'State -> bool; 
                        Actions : array<'State -> 'State>; 
                        Names : array<string>;
                        Utility : 'State -> Player -> double; }

let expand state game = [ for action in game.Actions -> action state ]
    
let rec minimaxValue game state player =
    if game.IsOver state then
        game.Utility state player
    elif player = Max then
        let children = expand state game
        [ for child in children -> minimaxValue game child Min ] |> List.max
    else
        let children = expand state game
        [ for child in children -> minimaxValue game child Max ] |> List.min

let minimaxDecision game state player =
    let children = expand state game |> List.toArray
    if player = Max then
        [ for i in 0..children.Length-1 -> (i, minimaxValue game children.[i] Min) ] |> List.maxBy snd
    else
        [ for i in 0..children.Length-1 -> (i, minimaxValue game children.[i] Max) ] |> List.minBy snd

let rec play game state player =
    if game.IsOver state then
        match game.Utility state player with
        | x when x > 0.0 -> printfn "Game over - you won! :)"
        | x when x < 0.0 -> printfn "Game over - you lost. :("
        | _ -> printfn "Game over - it's a draw."
    elif player = Max then
        Console.Write("Moves: ")
        let mutable i = 0
        while i < game.Actions.Length do
            printf "%d)%s " i game.Names.[i]
            i <- i+1
        let choice = Console.ReadLine()
        let mutable action = game.Actions.[0] // subject to change
        let num = ref 0                       // subject to change
        if Int32.TryParse(choice, num) then
            action <- game.Actions.[!num]     // user entered action number
        else
            num := Array.findIndex (fun x -> x = choice) game.Names // user entered action name
            action <- game.Actions.[!num]
        let newState = action state
        printfn "New state is %A\n" newState
        play game newState Min
    else
        let move = minimaxDecision game state Min |> fst
        let newState = game.Actions.[move] state
        printfn "Computer made move %A" game.Names.[move]
        printfn "New state is %A\n" newState
        play game newState Max