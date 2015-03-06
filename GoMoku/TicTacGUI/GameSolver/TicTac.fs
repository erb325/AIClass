module TicTac
open System

// states are (3x3) 2D arrays of integer
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
    
let rec minimaxValue game state player =
    let mutable value = 0.0
    if game.IsOver state then
        value <- game.Utility state player
    elif player = "Max" then
        let children = expand state game "Max"
        value <- [ for child in children -> minimaxValue game child "Min" ] |> List.max
    else
        let children = expand state game "Min"
        value <-[ for child in children -> minimaxValue game child "Max" ] |> List.min
    value

let minimaxDecision game (state:State) playerString =
    let mutable children = []
    if playerString = "Max" then
        for actNum in 0..game.Actions.Length-1 do
            for row in 0..game.Rows-1 do
                for col in 0..game.Cols-1 do
                    if state.[row,col] = 0 then
                        let child = game.Actions.[actNum] row col "Max" state        
                        children <- (actNum, row, col, minimaxValue game child "Min" ) :: children 
        children |> List.maxBy (fun (x,y,z,w) -> w)
    else
        for actNum in 0..game.Actions.Length-1 do
            for row in 0..game.Rows-1 do
                for col in 0..game.Cols-1 do
                    if state.[row,col] = 0 then
                        let child = game.Actions.[actNum] row col "Min" state        
                        children <- (actNum, row, col, minimaxValue game child "Max" ) :: children 
        children |> List.minBy (fun (x,y,z,w) -> w)
                    

let chooseMove game = 
    let state = game.CurrentState
    let (move,row,col) = minimaxDecision game state "Min" |> (fun (x,y,z,w) -> (x,y,z))
    let newState = game.Actions.[move] row col "Min" state
    let newgame = { game with CurrentState = newState }
    newgame



let sameState (s1:State) (s2:State) = 
    let mutable same = true
    for row in 0..2 do
        for col in 0..2 do
            if s1.[row,col] <> s2.[row,col] then
                same <- false
    same

let copyState (s:State) = 
    let newState = Array2D.zeroCreate<int> 19 19
    for row in 0..2 do
        for col in 0..2 do
            newState.[row,col] <- s.[row,col]
    newState

let start = Array2D.zeroCreate<int> 19 19

// helper function used by gameOver
// returns whether the game is such that player has already won
let wonBy player (state:State) =  // use 1 for max player, -1 for min player
    let mutable isWon = false
    //let mutable winner = "Neither"  
    let mutable playerCount = 0
                
    // check for 3 in a row, horizontally
    for row in 0..2 do
        playerCount <- 0
        for col in 0..2 do
            if state.[row, col] = player then
                playerCount <- playerCount + 1  
               
        if playerCount = 3 then
            isWon <- true

    // check for 3 in a row, vertically        
    for col in 0..2 do
        playerCount <- 0
        for row in 0..2 do
            if state.[row, col] = player then
                playerCount <- playerCount + 1  
               
        if playerCount = 3 then
            isWon <- true

    // check for 3 in a row, diagonally top-left to bot-right      
    playerCount <- 0
    for row in 0..2 do
        let col = row
        if state.[row, col] = player then
                playerCount <- playerCount + 1  
               
        if playerCount = 3 then
            isWon <- true
      
    // check for 3 in a row, diagonally top-right to bot-left  
    playerCount <- 0
    for row in 0..2 do
        let col = 2-row
        if state.[row, col] = player then
                playerCount <- playerCount + 1  
               
        if playerCount = 3 then
            isWon <- true    
   
    isWon
        
let gameOver (state:State) =
    if wonBy 1 state then
        true
    elif wonBy -1 state then
        true
    else
        let mutable spacesLeft = 0
        for row in 0..2 do
            for col in 0..2 do
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
                Rows = 3;
                Cols = 3}
