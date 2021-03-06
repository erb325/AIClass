﻿//Ember Baker

module TicTac
open System

// states are (19x19) 2D arrays of integer
type State = int [,]

type MatrixGame = { Start : State;
                    CurrentState: State; 
                    IsOver : State -> bool; 
                    Actions : array<int -> int-> string -> State -> State>;
                    Names : array<string>;
                    Utility : State -> string -> double; 
                    Rows : int;
                    Cols : int }

let makeMove row col player actionnum game =
    let newgame = { game with CurrentState = game.Actions.[actionnum] row col player game.CurrentState }
    newgame

let expand (state:State) game player = [ for action in game.Actions do
                                            for row in 0..game.Rows-1 do
                                                for col in 0..game.Cols-1 do
                                                    if state.[row,col]=0 then
                                                        yield action row col player state ]

//assign a weight based on the count 
let weight count =
    let mutable temp = 0.0 
    match count with 
    | -5 -> temp <- -500.0
    | -4 -> temp <- -100.0
    | -3 -> temp <- -30.0
    | -2 -> temp <- -10.0
    | -1 -> temp <- -1.0
    | 0 -> temp <- 0.0
    | 1 -> temp <- 2.0
    | 2 -> temp <- 20.0
    | 3 -> temp <- 60.0
    | 4 -> temp <- 200.0
    | 5 -> temp <- 500.0
    | _ -> temp <- 0.0 
    temp

let checkVertical (state:State) row col = 
    let mutable value = 0.0 
    let mutable count = 0 
    
    if (row+5)<= 18 then 
        for x in 0..4 do
           count <- count + state.[(row + x), col]
    value <- value + (weight count)        
    value 

let checkHorizonal (state:State) row col = 
    let mutable value = 0.0 
    let mutable count = 0 
   
    if (col+5)<= 18 then 
        for x in 0..4 do
           count <- count + state.[row, (col+x)]
    value <- value + (weight count)        
    value 

let checkLeftDiagonal (state:State) row col =
    let mutable value = 0.0 
    let mutable count = 0 
   
    if (row+5<= 18 && col+5<= 18) then 
        for x in 0..4 do
           count <- count + state.[(row+x), (col+x)]
    value <- value + (weight count)        
    value

let checkRightDiagonal (state:State) row col =
    let mutable value = 0.0 
    let mutable count = 0 
    
    if (row+5<= 18 && col-5>= 0) then 
        for x in 0..4 do
           count <- count + state.[(row+x), (col-x)]
    value <- value + (weight count)        
    value

//check every group of 5
let h1 player (state:State) = 
    let mutable value = 0.0
    for row in 0..18 do
        for col in 0..18 do
            value <- value + (checkVertical state row col)
            value <- value + (checkHorizonal state row col)
            value <- value + (checkLeftDiagonal state row col)
            value <- value + (checkRightDiagonal state row col)
    value
    
let rec minimaxValue game state player ply =
    let mutable value = 0.0
    if game.IsOver state then
        value <- game.Utility state player
    elif ply = 0 && player = "Max" then
        value <- h1 1 state
    elif ply = 0 && player = "Min" then
        value <- h1 -1 state
    elif player = "Max" then
        let children = expand state game "Max"
        value <- [ for child in children -> minimaxValue game child "Min" (ply-1)] |> List.max
    else
        let children = expand state game "Min"
        value <-[ for child in children -> minimaxValue game child "Max" (ply-1)] |> List.min
    value

let minimaxDecision game (state:State) playerString  ply =
    let mutable children = []
    if playerString = "Max" then
        for actNum in 0..game.Actions.Length-1 do
            for row in 0..game.Rows-1 do
                for col in 0..game.Cols-1 do
                    if state.[row,col] = 0 then
                        let child = game.Actions.[actNum] row col "Max" state        
                        children <- (actNum, row, col, minimaxValue game child "Min" ply) :: children 
        children |> List.maxBy (fun (x,y,z,w) -> w)
    else
        for actNum in 0..game.Actions.Length-1 do
            for row in 0..game.Rows-1 do
                for col in 0..game.Cols-1 do
                    if state.[row,col] = 0 then
                        let child = game.Actions.[actNum] row col "Min" state        
                        children <- (actNum, row, col, minimaxValue game child "Max" ply) :: children 
        children |> List.minBy (fun (x,y,z,w) -> w)
                    

let chooseMove game ply = 
    let state = game.CurrentState
    let (move,row,col) = minimaxDecision game state "Min" (ply-1)|> (fun (x,y,z,w) -> (x,y,z))
    let newState = game.Actions.[move] row col "Min" state
    let newgame = { game with CurrentState = newState }
    newgame



let sameState (s1:State) (s2:State) = 
    let mutable same = true
    for row in 0..18 do
        for col in 0..18 do
            if s1.[row,col] <> s2.[row,col] 
            then
                same <- false
    same

let copyState (s:State) = 
    let newState = Array2D.zeroCreate<int> 19 19
    for row in 0..18 do
        for col in 0..18 do
            newState.[row,col] <- s.[row,col]
    newState

let start = Array2D.zeroCreate<int> 19 19

// helper function used by gameOver
// returns whether the game is such that player has already won
let wonBy player (state:State) =  // use 1 for max player, -1 for min player
    let mutable isWon = false
    //let mutable winner = "Neither"  
    let mutable playerCount = 0
                
    // check for 5 in a row, horizontally
    for row in 0..18 do
        playerCount <- 0
        for col in 0..18 do
            if state.[row, col] = player then
                playerCount <- playerCount + 1 
            else  
                if playerCount = 5 then
                    isWon <- true
                playerCount <- 0
        if playerCount = 5 then
                isWon <- true

    // check for 5 in a row, vertically        
    for col in 0..18 do
        playerCount <- 0
        for row in 0..18 do
            if state.[row, col] = player then
                playerCount <- playerCount + 1  
             else  
                if playerCount = 5 then
                    isWon <- true
                playerCount <- 0
        if playerCount = 5 then
            isWon <- true

    // check for 5 in a row, diagonally top-left to bot-right      
    let mutable i = 0
    playerCount <- 0
    for row in 0..14 do 
        i <- 0
        for col in 0..(18-row) do
            if state.[row+i, col] = player then
               playerCount <- playerCount + 1
            else playerCount <- 0
            i <- i + 1
            if playerCount = 5 then 
                isWon <- true 
        if playerCount = 5 then 
            isWon <- true
    playerCount <- 0
    for col in 0..14 do 
        i <- 0
        for row in 0..(18-col) do
            if state.[row, col+i] = player then
               playerCount <- playerCount + 1
            else playerCount <- 0
            i <- i + 1
            if playerCount = 5 then 
                isWon <- true 
        if playerCount = 5 then 
            isWon <- true

    // check for 5 in a row, diagonally top-right to bot-left  
    playerCount <- 0
    for row in 0..18 do 
        i <- 0
        for col in 0..(18-row) do
            if state.[(18-(row+i)), col] = player then
               playerCount <- playerCount + 1
            else playerCount <- 0
            i <- i + 1
            if playerCount = 5 then 
                isWon <- true 
        if playerCount = 5 then 
            isWon <- true
    playerCount <- 0
    for col in 0..18 do 
        i <- 0
        for row in 0..(18-col) do
            if state.[(18-row), col+i] = player then
               playerCount <- playerCount + 1
            else playerCount <- 0
            i <- i + 1
            if playerCount = 5 then 
                isWon <- true 
        if playerCount = 5 then 
            isWon <- true
    isWon 
        
let gameOver (state:State) =
    if wonBy 1 state then
        true
    elif wonBy -1 state then
        true
    else
        let mutable spacesLeft = 0
        for row in 0..18 do
            for col in 0..18 do
                if state.[row,col] = 0 then
                    spacesLeft <- spacesLeft + 1
        spacesLeft = 0

let utility state player =   // only use if gameOver is true
    if wonBy 1 state then
        1.0
    elif wonBy -1 state then
        -1.0
    else
        0.0

// actions
let action row col player state = 
    let newState = copyState state
    if player = "Max" then
        newState.[row,col] <- 1
    else
        newState.[row,col] <- -1
    newState

let actions = [|action|]
let names = [|"action"|]

let game = {    Start = start;
                CurrentState = start;
                IsOver = gameOver;
                Actions = actions;
                Names = names;
                Utility = utility;
                Rows = 19;
                Cols = 19}
