using UnityEngine;
using System.Collections;

public class PlayerNucleusManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// nucleus
	public void Nucleus (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{
		// evolutions \\

		// zero
		// to dark zero (0.5)
		// from zero
		if (fromState == 0 && toState == 0 && fromLight && !toLight) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// to first
		// from dark zero (0.5)
		if (fromState == 0 && toState == 1 && !fromLight && !toLight) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && !fromLight && toLight) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from light zero (0.5)
		if (fromState == 0 && toState == 1 && fromLight && !toLight) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && fromLight && toLight) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// first
		// to second
		// from dark first
		if (fromState == 1 && toState == 2 && !fromLight && !toLight) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && !fromLight && toLight) {	// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from light first
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && fromLight && toLight) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// second
		// to third
		// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// third
		// to fourth
		// from dark third
		if (fromState == 3 && toState == 4 && !fromLight && !toLight) {							// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && !fromLight && toLight) {						// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {							// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && fromLight && toLight) {						// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
		// to fifth
		// from dark fourth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light fourth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
		// to sixth
		// from dark circle fifth
		if (fromState == 5 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light circle fifth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from triangle fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 1) {						// to dark triangle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from square fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 2) {						// to dark square sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
		// to seventh
		// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) {		// to light triangle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) {		// to light square seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
		// to eighth
		// from dark circle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 0) {			// to dark circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) {		// to light circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light circle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			// to dark circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 0) {		// to light circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from dark triangle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 1) {		// to light triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light triangle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 1) {		// to light triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from dark square seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 2) {			// to dark square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 2) {		// to light square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light square seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 2) {			// to dark square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 2) {		// to light square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// devolutions \\

		// zero
		// to dead
		if (fromState == 0 && toState == -1) {													// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// dark zero (0.5)
		// to zero
		if (fromState == 0 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// first
		// to dead
		if (fromState == 1 && toState == -1) {													// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from dark first
		// to zero
		if (fromState == 1 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light first
		// to zero
		if (fromState == 1 && toState == 0 && fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// second
		// to dead
		if (fromState == 2 && toState == -1) {													// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from dark second
		// to zero
		if (fromState == 2 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 2 && toState == 1 && !fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light second
		// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// third
		// to dead
		if (fromState == 3 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from dark third	
		// to zero
		if (fromState == 3 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light third	
		// to zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
		// to dead
		if (fromState == 4 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from dark fourth	
		// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && !fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 4 && toState == 3 && !fromLight && !toLight) {							// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && !fromLight && toLight) {						// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light fourth	
		// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {							// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && fromLight && toLight) {						// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
		// to dead
		if (fromState == 5 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from dark circle fifth
		// to zero
		if (fromState == 5 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 5 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 5 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light circle fifth
		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from triangle fifth
		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {				// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from square fifth
		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
		// to dead
		if (fromState == 6 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from dark circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from triangle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from square sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {			// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
		// to dead
		if (fromState == 7 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		// from dark circle seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light circle seventh
		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from dark triangle seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light triangle seventh
		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}	
		// from dark square seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 2) {			// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {			// to dark square sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// from light square seventh
		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 2) {				// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 2) {			// to dark square sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

	}

}
