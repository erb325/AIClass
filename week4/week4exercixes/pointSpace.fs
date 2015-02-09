// Ember Baker
module pointSpace

//states are ordered pairs (x,y)

//actions transform a point by moving north east ot northeast
let goNorth (x,y) = (x, y+1)
let goEast (x,y) = (x+1, y)
let goNorthEast (x,y) = (x+1, y+1)



