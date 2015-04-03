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

