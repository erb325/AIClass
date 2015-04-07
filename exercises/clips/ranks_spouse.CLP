; Ember Baker


(deftemplate has-rank "specifies a person and their rank"
  (slot person)
  (slot rank))

(deffacts has-ranks "some facts about people having a certain rank"
  (has-rank (person gomer) (rank private))
  (has-rank (person klinger) (rank corporal))
  (has-rank (person slaughter) (rank sergeant))
  (has-rank (person hoolihan) (rank lieutenant))
  (has-rank (person hunnicut) (rank captain))
  (has-rank (person pierce) (rank major))
  (has-rank (person klink) (rank colonel))
  (has-rank (person patton) (rank general)))

(deftemplate next-higher-rank "specifies what the next higher rank above above is"
  (slot above)
  (slot is))

(deffacts next-higher-ranks "some facts about the order of ranks"
  (next-higher-rank (above colonel) (is general))
  (next-higher-rank (above major) (is colonel))
  (next-higher-rank (above captain) (is major))
  (next-higher-rank (above lieutenant) (is captain))
  (next-higher-rank (above sergeant) (is lieutenant))
  (next-higher-rank (above corporal) (is sergeant))
  (next-higher-rank (above private) (is corporal)))

(deffacts wife-husband-pairs "spouses in wife husband order"
	(spouses jayne erik)
	(spouses judy peter)
	(spouses olivia dale))

(defrule print-spouses
	(spouses ?w ?m)
=>
	(printout t ?w " is married to " ?m crlf)
	(printout t ?m " is married to " ?w crlf))

(deftemplate is-a-higher-rank "specifies that a rank is higher than another"
	(slot higher)
	(slot lower))

(defrule higher-ranks-rule "generates all facts about order 'recursivly' "
	(or (next-higher-rank (above ?lower) (is ?higher))	; basecase for this rule
		(and (is-a-higher-rank (lower ?lower) (higher ?high))
			(next-higher-rank (above ?high) (is ?higher))))
=>
	(assert (is-a-higher-rank (lower ?lower) (higher ?higher))))

(defrule outranks-rule
	(has-rank (person ?p1) (rank ?r1))
	(has-rank (person ?p2) (rank ?r2))
	(is-a-higher-rank (higher ?r1) (lower ?r2)) 
=> 
	(printout t ?p1 " outranks " ?p2 crlf))

(defrule top-dog "who outranks everyone"
	(has-rank (person ?p1) (rank ?r1))
	(forall (has-rank (person ?p2&~?p1) (rank ?r2))
		(is-a-higher-rank (higher ?r1) (lower ?r2)))
=>
	(printout t ?p1 " outranks every other known person " crlf))
	
