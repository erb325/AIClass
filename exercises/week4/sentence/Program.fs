// Ember Baker
// Week 4: Monday Febuary 9, 2015
// sentence.lsp converted to equilvalent F# code

open System     //so that you do not have to say System.random
open System.Speech.Synthesis    //so that later we can make it talk

let articles = [ "a"; "the"; "this"; "my"; "that"]
let nouns = ["dog"; "cat"; "man"; "woman"; "bike"; "ball"; "computer"]
let verbs = ["ate"; "hit"; "rode"; "kissed"; "chased"; "broke"]

let rand = new Random() 
let randomElement l = List.nth l (rand.Next(l.Length))
let selectFrom l = [ randomElement l]

let nounPhrase () = selectFrom articles @ selectFrom nouns
let verbPhrase () = selectFrom verbs @ nounPhrase() 
//let sentence () = nounPhrase () @ verbPhrase()

let talk = new SpeechSynthesizer()

let rec speakUntilQuit input = 
    match input with 
    | "quit" -> ()
    | _      -> let sentence = nounPhrase () @ verbPhrase()
                let saying = String.concat " " sentence     //concat put a space
                Console.WriteLine(saying)
                talk.Speak(saying)
                speakUntilQuit (Console.ReadLine()) 

speakUntilQuit (Console.ReadLine()) 