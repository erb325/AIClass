; diagnosis2.clp  This is a very simple example of a CLIPS knowledge base,
; just using the pattern matching to create new knowledge.

; We have modified this to use binding to prompt users for input.

; In this example, instead of having separate facts, we will have
; a single Patient template which stores all information about
; the patient. We will then modify this fact as knowledge is added.

; Note that (unless we do some other things) this only allows one value
; per slot, which is why we will keep diganosis and treatment separate.

(deftemplate Patient (slot temperature) 
                     (slot spots)
                     (slot rash)
                     (slot sore_throat)
                     (slot innoculated)
                     (slot fever))

; One thing that we will need is an initial patient fact, since
; we will modify this instead of creating new facts.

(deffacts Initial
   (Patient ))

; Our first rules will be used to gather symptoms from the user.
; Note that there are no conditions, which meand that they will
; always fire. The action is to print a prompt, bind the (read)
; to a variable, and then assert a new fact using that value. Note
; that one of thes tests is for temperature to be nil, which means
; that temperature is not yet known (if this is left out, the shell
; will loop).

; For each of these rules, we bind the Patient fact to a variable
; and then modfiy it.

(defrule GetTemperature
   (declare (salience 500))
   ?p <- (Patient (temperature nil)) 
   =>
   (printout t "Enter patient temperature: ")
   (bind ?response (read))
   (modify ?p (temperature ?response)))

(defrule GetSpots
   (declare (salience 500))
   ?p <- (Patient (spots nil)) 
   =>
   (printout t "Does the patient have spots (yes or no): ")
   (bind ?response (read))
   (modify ?p (spots ?response)))

(defrule GetRash
   (declare (salience 500))
   ?p <- (Patient (rash nil)) 
   =>
   (printout t "Does the patient have a rash (yes or no): ")
   (bind ?response (read))
   (modify ?p (rash ?response)))

(defrule GetSoreThroat
   (declare (salience 500))
   ?p <- (Patient (sore_throat nil)) 
   =>
   (printout t "Does the patient have a sore throat (yes or no): ")
   (bind ?response (read))
   (modify ?p (sore_throat ?response)))

; We can also ask for certain information only if necessary. For example,
; it doesn't make sense to ask whether the patient has been innoculated
; unless there is a possiblity of measles.

(defrule GetInnoculated
   (declare (salience 500))
   ?p <- (Patient (fever high) (spots yes) (innoculated nil))
   =>
   (printout t "Has the patient been innoculated for measles (yes or no): ")
   (bind ?response (read))
   (modify ?p (innoculated ?response)))

; Rules for concluding fever from temperature.

; Note that these rules find the patient temperature, and then bind
; it to ?t. The next part uses the test keyword to evaluate the
; conditional expression as true or false.

; The numberp is a test to check whether the argument is a numeric value.
; This is placed before the numeric comparison to keep the compiler from
; complaining that the initial patient fact does not have a numeric
; temperature value.

(defrule Fever1
   ?p <- (Patient (temperature ?t) (fever nil))
   (test (numberp ?t))
   (test (>= ?t 101))
   =>
   (modify ?p (fever high))
   (printout t "High fever diagnosed" crlf))

(defrule Fever2
   ?p <- (Patient (temperature ?t) (fever nil))
   (test (numberp ?t))
   (test (and (< ?t 101) (> ?t 98.6)))
   =>
   (modify ?p (fever mild))
   (printout t "Mild fever diagnosed" crlf))

; Rules for determining diagnosis on the basis of patient symptoms
; Salience added to give this rule priority

(defrule Measles
   (declare (salience 100))
   (Patient (spots yes) (innoculated no) (fever high))
   =>
   (assert (diagnosis measles))
   (printout t "Measles diagnosed" crlf))

; Modified to only fire if no measles

(defrule Allergy1
   (declare (salience -100))
   (and (Patient (spots yes))
        (not (diagnosis measles)))      
   =>
   (assert (diagnosis allergy))
   (printout t "Allergy diagnosed from spots and lack of measles" crlf))   

(defrule Allergy2
   (Patient (rash yes))
   =>
   (assert (diagnosis allergy))
   (printout t "Allergy diagnosed from rash" crlf))

(defrule Flu
   (Patient (sore_throat yes) (fever mild|high))
   =>
   (assert (diagnosis flu))
   (printout t "Flu diagnosed" crlf))

; Rules for recommedaing treatments on the basis of
; Diagnosis facts created.

(defrule Penicillin
   (diagnosis measles)
   =>
   (assert (treatment pennicillin))
   (printout t "Penicillin prescribed" crlf))

(defrule Allergy_pills
   (diagnosis allergy)
   =>
   (assert (treatment allergy_shot))
   (printout t "Allergy shot prescribed" crlf))

(defrule Bed_rest
   (diagnosis flu)
   =>
   (assert (treatment bed_rest))
   (printout t "Bed rest prescribed" crlf))

; Finally, if there are no diagnosis facts, we print the 
; metaknowledge that the expert system does not apply to
; this problem, and another expert must be consulted. Note
; that the salience must be set to make this the last rule
; checked.

(defrule None
   (declare (salience -100))
   (not (diagnosis ?))
   =>
   (printout t "No diagnosis possible -- consult human expert" crlf))
