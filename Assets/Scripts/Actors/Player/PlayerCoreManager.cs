using UnityEngine;
using System.Collections;

public class PlayerCoreManager : MonoBehaviour {

	private Animator anim;							// animator on core ref
	private Mesh mesh;								// core mesh
	public Mesh sphere, triangle, square, ring;		// shape meshes

	void Awake () {
		anim = GetComponent<Animator>();			// init animator ref
		mesh = GetComponent<MeshFilter> ().mesh;	// init mesh ref
	}
		
	public void Core (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{
		// EVOLUTIONS \\

		///// zero \\\\\

		// to dark zero (0.5)
			// from zero
		if (fromState == 0 && toState == 0 && fromLight && !toLight) {			// to dark zero
			ScaleTo (true, "zero", "hidden");										// scale to hidden
			SetShape (3);															// change to ring
			ScaleTo (false, "hidden", "zero");										// scale to zero
		}
		// to light zero (0.5) no change

		// to first
			// from dark zero (0.5)
		if (fromState == 0 && toState == 1 && !fromLight && !toLight) {			// to dark first
			ScaleTo (false, "zero", "first");										// scale to first
		}
		else if (fromState == 0 && toState == 1 && !fromLight && toLight) {		// to light first
			ScaleTo (true, "zero", "hidden");										// scale to hidden
			SetShape (0);															// change to ring
			ScaleTo (false, "hidden", "first");										// scale to first
		}
			// from light zero (0.5)
		if (fromState == 0 && toState == 1 && fromLight && !toLight) {			// to dark first
			ScaleTo (true, "zero", "hidden");										// scale to hidden
			SetShape (3);															// change to ring
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		else if (fromState == 0 && toState == 1 && fromLight && toLight) {		// to light first
			ScaleTo (true, "zero", "first");										// scale to first
		}

		///// first \\\\\

		// to second

		// from dark first
			// to dark second (no core change)
		if (fromState == 1 && toState == 2 && !fromLight && toLight) {			// to light second
			ScaleTo (true, "zero", "hidden");										// scale to hidden
			SetShape (0);															// change to ring
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		// from light first
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			// to dark second
			ScaleTo (true, "zero", "hidden");										// scale to hidden
			SetShape (3);															// change to ring
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		// to light second (no core change)

		///// second \\\\\

		// to third

		// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			// change to black
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}
		// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			// change to black
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
			// scale to third
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		///// third \\\\\

		// to fourth

		// from dark third
		// to dark fourth (no core change
		if (fromState == 3 && toState == 4 && !fromLight && toLight) {		// to light fourth
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {							// to dark fourth
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light fourth (no core change)

		///// fourth \\\\\

		// to fifth

		// from dark fourth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			// change to ring
			// change to white
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			// change to sphere
			// change to white
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// from light fourth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			// change to triangle
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		// to square fifth
			// change to square
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		///// fifth \\\\\

		// to sixth

		// from dark circle fifth
		// to dark circle sixth (no core change)
		if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// from light circle fifth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light circle sixth (no core change)

		// from triangle fifth
		// to dark triangle sixth (no core change)
			
		// from square fifth
		// to dark square sixth (no core change)

		// sixth

		// to seventh

		// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			// change to sphere
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			// change to ring
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) {		// to light triangle seventh
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) {		// to light square seventh
			// scale to seventh
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		///// seventh \\\\\

		// to eighth

		// from dark circle seventh
		// to dark circle eighth (no core change)
		if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) {		// to light circle eighth
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light circle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			// to dark circle eighth
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light circle eighth (no core change)

		// from dark triangle seventh
		// to dark triangle eighth (no core change)
		// to light triangle eighth (no core change)

		// from light triangle seventh
		// to dark triangle eighth (no core change)
		// to light triangle eighth (no core change)

		// from dark square seventh
		// to dark square eighth (no core change)
		// to light square eighth (no core change)

		// from light square seventh
		// to dark square eighth (no core change)
		// to light square eighth (no core change)

		// eighth
		// ninth
		// tenth

		// DEVOLUTIONS \\

		///// zero \\\\\

		// to dead
		if (fromState == 0 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		///// dark zero (0.5) \\\\\

		// to zero
		if (fromState == 0 && toState == 0 && !fromLight && toLight) {							// to zero
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		///// first \\\\\

		// to dead
		if (fromState == 1 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from dark first

		// to zero
		if (fromState == 1 && toState == 0 && !fromLight && toLight) {							// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light first

		// to zero
		if (fromState == 1 && toState == 0 && fromLight && toLight) {							// to zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && fromLight && !toLight) {							// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		///// second \\\\\

		// to dead
		if (fromState == 2 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from dark second

		// to zero
		if (fromState == 2 && toState == 0 && !fromLight && toLight) {							// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		// to dark first (no core change)
		if (fromState == 2 && toState == 1 && !fromLight && toLight) {						// to light first
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light second

		// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) {							// to dark first
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light first (no core change)

		///// third \\\\\

		// to dead
		if (fromState == 3 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark third	

		// to zero
		if (fromState == 3 && toState == 0 && !fromLight && toLight) {							// to zero
			// change to sphere
			// change to white
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			// change to white
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {							// to dark first
			// change to ring
			// change to white
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			// change to sphere
			// change to white
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
			// change to ring
			// change to white
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			// change to sphere
			// change to white
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light third	

		// to zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) {							// to dark first
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		///// fourth \\\\\

		// to dead
		if (fromState == 4 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark fourth	

		// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {							// to zero
			// change to sphere
			// change to white
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			// change to ring
			// change to white
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			// change to ring
			// change to white
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			// change to sphere
			// change to white
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {							// to dark second
			// change to ring
			// change to white
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && !fromLight && toLight) {						// to light second
			// change to shpere
			// change to white
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		// to dark third (no core change)
		if (fromState == 4 && toState == 3 && !fromLight && toLight) {						// to light third
			// change to shpere
			// change to white
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light fourth	

		// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) {							// to zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && fromLight && !toLight) {							// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && fromLight && toLight) {						// to light second
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {							// to dark third
			// change to ring
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light third ( no core change)

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
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
			// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
			// to first
			// to dark first (no core change)
		if (fromState == 5 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
			// to second
			// to dark second (no core change)
		if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
			// to third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
			// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light circle fifth

		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light first (no core change)
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light second (no core change)
			
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from triangle fifth

		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {				// to light fourth
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from square fifth

		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

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
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
			// to dark first (no core change)
		if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
			// to dark second (no core change)
		if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
			// to dark circle fifth (no core change)
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light circle sixth

		// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
			// to light first (no core change)
		// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light second (no core change)
		// to third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to light circle fifth (no core change)

		// from triangle sixth

		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {				// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		// to triangle fifth (no core change)

		// from square sixth

		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {				// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			// change to ring
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			// change to sphere
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			// change to ring
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		// to square fifth (no core change)

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
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			// // change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light circle seventh

		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			// change to ring
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			// change to ring
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from dark triangle seventh

		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			// change to ring
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 1) {			// to triangle fifth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light triangle seventh

		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			// change to ring
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {			// to light fourth
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}	

		// from dark square seventh

		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			// change to ring
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 2) {			// to square fifth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {			// to dark square sixth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from light square seventh

		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			// change to sphere
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			// change to ring
			// scale to zero
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			// change to ring
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			// change to sphere
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			// change to ring
			// scale to first
			// change to black
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			// change to sphere
			// scale to third
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {				// to square fifth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark square sixth
			// scale to first
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state

		}

	}

	///<summary>
	///<para>0 = circle</para>
	///<para>1 = triangle</para>
	///<para>2 = square</para>
	///<para>3 = ring</para>
	///</summary>
	private void SetShape(int shape)
	{
		if (shape == 0) {
			// change mesh to sphere
			mesh = sphere;
		}
		else if (shape == 1) {
			// change mesh to triangle
			mesh = triangle;
		}
		else if (shape == 2) {
			// change mesh to square
			mesh = square;
		}
		else if (shape == 3) {
			// change mesh to ring
			mesh = ring;
		}
	}

	///<summary>
	///<para>set core as light</para>
	///<para>true = white</para>
	///<para>false = black</para>
	///</summary>
	private void SetLight (bool light)
	{
		if (light) {
			// change to white
			GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
		}
		else {
			// change to black
			GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
		}
	}

	///<summary>
	///<para> devol = whether to scale up or scale down </para>
	///<para> resetState = state to set to false </para>
	///<para> setState = state to set to true </para>
	///</summary>
	private void ScaleTo (bool devol, string resetState, string setState)
	{
		// scale to zero
		// scale to first
		// scale to third
		// scale to fifth
		// scale to seventh
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
