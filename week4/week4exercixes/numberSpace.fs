﻿// Ember Baker
// NumberSpace.fs
// States are numbers

// Actions transform a number into another number

let increment state = state + 1
let double state = 2 * state

let actions = [increment; double]  
    