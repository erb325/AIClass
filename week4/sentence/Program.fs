// Ember Baker
// Week 4: Monday Febuary 9, 2015
// sentence.lsp converted to equilvalent F# code

open System     //so that you do not have to say System.random

let articles = [ "a"; "the"; "this"; "my"; "that"]
let nouns = ["dog"; "cat"; "man"; "woman"; "bike"; "ball"; "computer"]
let verbs = ["ate"; "hit"; "rode"; "kissed"; "chased"; "broke"]

let rand = new Random() 
let randomElement l = List.nth l (rand.Next(l.Length))
let selectFrom l = [ randomElement l]

let nounPhrase () = selectFrom articles @ selectFrom nouns
let verbPhrase () = selectFrom verbs @ nounPhrase() 
let sentence () = nounPhrase () @ verbPhrase()

