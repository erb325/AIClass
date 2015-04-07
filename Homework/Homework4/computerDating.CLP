; Ember Baker
; Computer Dating Problem 

; define what a 'Person' is for this expert system
(deftemplate Person
	(slot name)
	(slot gender)
	(slot age)
	(slot income)
	(slot hair)
	(slot eyes)
	(slot appearance)
	(slot build)
	(slot spouse)
	(slot smokes)
	(slot drinks)
	(slot father)
	(slot mother)
	(slot interest))

(deftemplate Requirements
	(slot name)
	(slot requirements))

(Person (name Bill)
	(gender male)
	(age fifties)
	(income high)
	(hair grey)
	(eyes blue)
	(spouse Hillary)
	(interest politics))
(Requirements (name Bill)
	(requirements (gender female)))

(Person (name Monica)
	(gender female)
	(age twenties)
	(income middle)
	(hair brown)
	(eyes brown) 
	(interest politics))
(Requirements (name Monica)
	(requirements (and (exists (spouse))
			(interest politics))))

