using UnityEngine;
using System.Collections;

public class PlayerShellManager : MonoBehaviour {

	private Animator anim;						// animator on core ref

	void Awake () {
		anim = GetComponent<Animator>();		// init animator ref
	}

	public void Shell (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{

// EVOLUTIONS \\

	///// zero \\\\\

		// to dark zero (0.5)

		// from zero
			// to dark zero (no shell change)

		// to first

		// from dark zero (0.5)
			// to dark first (no shell change)
			// to light first (no shell change)
			
		// from light zero (0.5)
			// to dark first (no shell change)
			// to light first (no shell change)

	///// first \\\\\

		// to second

		// from dark first
			// to dark second (no shell change)
			// to light second (no shell change)
			
		// from light first
			// to dark second (no shell change)
			// to light second (no shell change)

		// to third

		// from dark first
		if (fromState == 1 && toState == 3 && !fromLight && !toLight) {			// to dark third
			Debug.Log("player shell first to third");
			ScaleTo (false, "hidden", "third");										// scale to third
		}
		// to light third (no shell change)

		// from light first
		if (fromState == 1 && toState == 3 && fromLight && !toLight) {			// to dark third
			ScaleTo (false, "hidden", "third");										// scale to third
		}
		// to light third (no shell change)

	///// second \\\\\

		// to third

		// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			ScaleTo (false, "hidden", "third");										// scale to third
		}
			// to light third (no shell change)
			
		// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			ScaleTo (false, "hidden", "third");										// scale to third
		}
			// to light third (no shell change)

	///// third \\\\\

		// to fourth

		// from dark third
			// to dark fourth (no shell change)
		if (fromState == 3 && toState == 4 && !fromLight && toLight) {			// to light fourth
			ScaleTo (false, "third", "hidden");										// scale to hidden
		}
		// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {			// to dark fourth
			ScaleTo (false, "hidden", "third");										// scale to third
		}
			// to light fourth (no shell change)
			
	///// fourth \\\\\

		// to fifth

		// from dark fourth
			// to dark circle fifth (no shell change)
			// to light circle fifth (no shell change)

		// from light fourth
			// to triangle fifth (no shell change)
			// to square fifth (no shell change)

	///// fifth \\\\\

		// to sixth

		// from dark circle fifth
			// to dark circle sixth (no shell change)
			// to light circle sixth (no shell change)
			
		// from light circle fifth
			// to dark circle sixth (no shell change)
			// to light circle sixth (no shell change)
			
		// from triangle fifth
			// to dark triangle sixth (no shell change)
			
		// from square fifth
			// to dark square sixth (no shell change)

	///// sixth \\\\\

		// to seventh

		// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			ScaleTo (false, "third", "seventh");													// scale to seventh
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			ScaleTo (false, "third", "seventh");													// scale to seventh
		}
		// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			ScaleTo (false, "third", "seventh");													// scale to seventh
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			ScaleTo (false, "third", "seventh");													// scale to seventh
		}
		// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
			// to light triangle seventh (no shell change)
			
		// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
			// to light square seventh (no shell change)

	///// seventh \\\\\

		// to eighth

		// from dark circle seventh
			// to dark circle eighth (no shell change)
			// to light circle eighth (no shell change)
			
		// from light circle seventh
			// to dark circle eighth (no shell change)
			// to light circle eighth (no shell change)
			
		// from dark triangle seventh
			// to dark triangle eighth (no shell change)
		if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 1) {			// to light triangle eighth
			ScaleTo (false, "seventh", "hidden");													// scale to hidden
		}

		// from light triangle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
			// to light triangle eighth (no shell change)
			
		// from dark square seventh
			// to dark square eighth (no shell change)
		if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 2) {			// to light square eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}

		// from light square seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 2) {			// to dark square eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		// to light square eighth (no shell change)

// DEVOLUTIONS \\

	///// zero \\\\\

		// to dead
		if (fromState == 0 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

	///// half zero (0.5) \\\\\

		// to zero (no shell change)

	///// first \\\\\

		// to dead
		if (fromState == 1 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from dark first
			// to zero (no shell change)
			// to dark zero (0.5) (no shell change)
	
		// from light first
			// to zero (no shell change)
			// to dark zero (0.5) (no shell change)

	///// second \\\\\

		// to dead
		if (fromState == 2 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from dark second
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first 
			// to dark first (no shell change)
			// to light first (no shell change)
			
		// from light second
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)

	///// third \\\\\

		// to dead
		if (fromState == 3 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark third	
		// to zero
		if (fromState == 3 && toState == 0 && !fromLight && toLight) {					// to zero
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {					// to dark zero
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {					// to dark first
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {				// to light first
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {					// to dark second
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {				// to light second
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}

		// from light third	
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
			
	///// fourth \\\\\

		// to dead
		if (fromState == 4 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark fourth	
		// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {					// to zero
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {					// to dark zero
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {					// to dark first
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {				// to light first
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {					// to dark second
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		else if (fromState == 4 && toState == 2 && !fromLight && toLight) {				// to light second
			ScaleTo (true, "third", "hidden");												// scale to hidden
		}
		// to third
			// to dark third (no shell change)
			// to light third (no shell change)

		// from light fourth	
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {					// to dark third
			ScaleTo (false, "hidden", "third");												// scale to third
		}
		// to light third (no shell change)

	///// fifth \\\\\

		// to dead
		if (fromState == 5 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark circle fifth
		// to zero
		if (fromState == 5 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to first
		if (fromState == 5 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 5 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to second
		if (fromState == 5 && toState == 2 && !fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to third
			// to dark third (no shell change)
		if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {			// to light third
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to fourth
			// to dark fourth (no shell change)
			
		// from light circle fifth
		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to third
			// to dark third (no shell change)
		if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 0) {				// to light third
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to fourth
			// to dark fourth (no shell change)
			
		// from triangle fifth
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to light third (no shell change)
		// to fourth
		// to light fourth (no shell change)
			
		// from square fifth
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)

	///// sixth \\\\\

		// to dead
		if (fromState == 6 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to third
			// to dark third (no shell change)
		if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to fourth
			// to dark fourth (no shell change)

		// to fifth
			// to dark circle fifth (no shell change)
			// to light circle fifth (no shell change)
			
		// from light circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to third
			// to dark third (no shell change)
			// to light third (no shell change)
		// to fourth
			// to dark fourth (no shell change)
		// to fifth
			// to dark circle fifth (no shell change)
			// to light circle fifth (no shell change)
			
		// from triangle sixth
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to triangle fifth (no shell change)
	
		// from square sixth
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to square fifth (no shell change)
			
	///// seventh \\\\\

		// to dead
		if (fromState == 7 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark circle seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (true, "seventh", "third");														// scale to third
		}

		// from light circle seventh
		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		// to fifth
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (true, "seventh", "third");														// scale to third
		}

		// from dark triangle seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 1) {			// to triangle fifth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}

		// from light triangle seventh
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (true, "seventh", "third");														// scale to third
		}
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to triangle fifth (no shell change)
		// to sixth
			// to dark triangle sixth (no shell change)

		// from dark square seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 2) {			// to square fifth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {			// to dark square sixth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}

		// from light square seventh
			// to zero (no shell change)
			// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (true, "seventh", "third");														// scale to third
		}
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to square fifth (no shell change)
		// to sixth
			// to dark square sixth (no shell change)
	}

	///<summary>
	///<para> devol = whether to scale up or scale down </para>
	///<para> resetState = state to set to false </para>
	///<para> setState = state to set to true </para>
	///</summary>
	private void ScaleTo (bool devol, string resetState, string setState)
	{
		if (devol) {
			anim.ResetTrigger ("scaleup");											// reset last stage
			anim.SetTrigger ("scaledown");											// enable scaledown
		}
		else {
			anim.ResetTrigger ("scaledown");										// reset last stage
			anim.SetTrigger ("scaleup");											// enable scaleup

		}
		anim.SetBool(resetState, false);											// reset previously active state
		anim.SetBool(setState, true);												// set active state
	}

}
