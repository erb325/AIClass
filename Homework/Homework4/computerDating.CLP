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
	(slot mother))

(deffacts people "all the people in the system"
	(Person (name Bill)
		(gender male)
		(age fifties)
		(income high)
		(hair grey)
		(eyes blue)
		(spouse Hillary))
	(Person (name Monica)
		(gender female)
		(age twenties)
		(income middle)
		(hair brown)
		(eyes brown))
	(Person (name Hillary)
		(gender female)
		(age fifties)
		(income high)
		(hair brown)
		(eyes green)
		(build shapely)
		(spouse Bill)
		(father William))
	(Person (name Chelsea)
		(gender female)
		(age teens)
		(income high)
		(hair brown)
		(eyes blue)
		(father Bill)
		(mother Hillary))
	(Person (name Fred)
		(gender male)
		(age forties)
		(income low)
		(hair bald)
		(eyes hazel)
		(build heavy)
		(spouse Wilma)
		(drinks moderate)
		(mother Beth))
	(Person (name Antonio)
		(gender male)
		(age twenties)
		(income low)
		(hair black)
		(eyes brown)
		(appearance handsome)
		(build slender)
		(smokes yes)
		(drinks heavy)
		(father David))
	(Person (name David)
		(gender male)
		(age forties)
		(income high)
		(hair blonde)
		(eyes blue)
		(appearance handsome)
		(build muscular) 
		(drinks moderate))
	(Person (name Hugo)
		(gender male)
		(age teens)
		(income middle)
		(hair black)
		(eyes blue)
		(appearance handsome)
		(build slender)
		(smokes yes)
		(father Fred))
	(Person (name Patricia)
		(gender female)
		(age thirties)
		(income middle)
		(hair blonde)
		(eyes brown)
		(build slender) 
		(spouse Mike))
	(Person (name Jennifer)
		(gender female)
		(age twenties)
		(income low)
		(hair brown)
		(eyes green)
		(appearance pretty)
		(build shapely)
		(drinks moderate)
		(father William))
	(Person (name Barbie)
		(gender female)
		(age twenties)
		(income middle)
		(hair brown)
		(eyes hazel)
		(appearance pretty)
		(build voluptuous))
	(Person (name Edgar)
		(gender male)
		(age twenties)
		(income high)
		(hair brown)
		(eyes hazel)
		(appearance handsome)
		(build slender)
		(drinks moderate))
	(Person (name William)
		(mother Mary))
	(Person (name Beth)
		(mother Mary)))

(deffacts has-interest "a person has interest of what"
	(has-interest Bill politics)
	(has-interest Monica politics)
	(has-interest Hillary politics)
	(has-interest Chelsea music)
	(has-interest Fred politics)
	(has-interest Antonio politics)
	(has-interest Antonio sports)
	(has-interest David politics)
	(has-interest David stockMarket)
	(has-interest Hugo music)
	(has-interest Hugo skateboards)
	(has-interest Patricia dancing)
	(has-interest Patricia outdoors)
	(has-interest Jennifer shopping)
	(has-interest Jennifer dancing)
	(has-interest Barbie music)
	(has-interest Barbie dancing)
	(has-interest Edgar dancing)
	(has-interest Edgar motorcycles))

(deftemplate family-pairs "name and name of family member"
	(slot name)
	(slot family))

(defrule family-tree "are two people related through ancestors"
	(or (or (Person (name ?n1) (father ?a&~nil))
		(Person (name ?n1) (mother ?a&~nil)))
	     (and (or (Person (name ?n1) (father ?p&~nil))
			(Person (name ?n1) (mother ?p&~nil)))
		(family-pairs (name ?a) (family ?p))))
=>
	(assert (family-pairs (name ?a)(family ?n1)))) 
	
(deftemplate match "matches in person -match pairs"
	(slot person)
	(slot match))

(defrule bill-match "anything in a skirt"
	(Person (gender female) (father ~Bill) (name ?n &~Bill))
=>
	(assert (match (person Bill) (match ?n))))

(defrule monica-match "married and politics"
	(and (Person (gender male) (spouse ~nil) (name ?n &~Monica))
		(has-interest ?n politics))
=>
	(assert (match (person Monica) (match ?n))))

(defrule hillary-match "politics and married and in their thirties or forties"
	(and (Person (gender male) (age thirties|forties) (spouse ~nil) (name ?n &~Hillary) (father ~William))
		(has-interest ?n politics))
=>
	(assert (match (person Hillary)(match ?n))))

(defrule chelsea-match "single and not interested in politics and in their teens or twenties"
	(and (Person (gender male) (age teens|twenties) (spouse nil) (name ?n &~Chelsea) 
			(father ~Bill) (mother ~Hillary))
		(not (has-interest ?n politics)))
=>
	(assert (match (person Chelsea)(match ?n))))
	
(defrule fred-match "brown hair and not a smoker"
	(Person (gender female) (name ?n &~Fred) (hair brown) (mother ~Beth))
=>
	(assert (match (person Fred)(match ?n))))
	
(defrule antonio-match "single and teens or shapely"
	(or (Person (gender female) (name ?n &~Antonio) (age teens) (spouse nil))
		(Person (gender female) (name ?n &~Antonio) (build shapely) (spouse nil)))
=>
	(assert(match (person Antonio)(match ?n))))
	

(defrule david-match "blonde or shapely"
	(or (Person (gender female) (name ?n &~David) (hair blonde))
		(Person (gender female) (name ?n &~David) (build shapely)))
=>
	(assert (match (person David)(match ?n))))
	
(defrule hugo-match "single and teens or twenties"
	(Person (gender female) (name ?n &~Hugo) (age teens|twenties) (spouse nil))
=> 	
	(assert (match (person Hugo)(match ?n))))

(defrule patricia-match "married and rich"
	(Person (gender male) (spouse ~nil) (name ?n &~Patricia) (income high))
=> 
	(assert (match (person Patricia) (match ?n))))

(defrule jenn-match "single and handsome OR married and rich"
	(or (Person (name ?n &~Jennifer) (spouse nil) (appearance handsome)(gender male))
		(Person (name ?n &~Jennifer) (spouse ~nil) (income high)(gender male)))
=>
	(assert (match (person Jennifer) (match ?n))))

(defrule barbie-match "non smoker and brown hair and not blue eyes"
	(Person (name ?n &~Barbie) (gender male)(hair brown) (eyes ~blue))
=>
	(assert (match (person Barbie) (match ?n))))

(defrule edgar-match "dancing or music AND teen or twenties AND not smoker"
	(and (has-interest ?n dancing|music)
		(Person (gender female)(name ?n &~Edgar)(age teens|twenties)))
=>
	(assert (match (person Edgar) (match ?n))))

(deftemplate is-related "these two are related somehow"
	(slot name)
	(slot family))

(defrule related "makes sure the matched pairs are not related"
	(and (family-pairs (name ?a)(family ?n1))
		(family-pairs (name ?a) (family ?n2)))
=>
	(assert (is-related (name ?n1) (family ?n2))))

(defrule print-matchs "prints matches based on not related and male-female pairs"
	(and (and (match (person ?n1) (match ?n2))
		   (Person (name ?n1) (gender male)))
	      (match (person ?n2) (match ?n1))
	      (not (is-related (name ?n1) (family ?n2))))
=> 
	(printout t ?n1 " is matched with " ?n2 crlf))

	
