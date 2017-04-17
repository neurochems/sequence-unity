using UnityEngine;
using System.Collections;

public class PlayerCoreManager : MonoBehaviour {

	private Animator anim;							// animator on core ref
	#pragma warning disable 0414
	private Mesh mesh;								// core mesh
	#pragma warning restore 0414
	public Mesh sphere, triangle, square, ring;		// shape meshes
	private MeshRenderer rend;						// mesh renderer (for colour changes)

	void Awake () {
		anim = GetComponent<Animator>();			// init animator ref
		mesh = GetComponent<MeshFilter>().mesh;		// init mesh ref
		rend = GetComponent<MeshRenderer>();		// init mesh renderer ref
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
			ScaleTo (true, "first", "hidden");										// scale to hidden
			SetShape (0);															// change to sphere
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		// from light first
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			// to dark second
			ScaleTo (true, "first", "hidden");										// scale to hidden
			SetShape (3);															// change to ring
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		// to light second (no core change)

		///// second \\\\\

		// to third

		// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			ScaleTo (true, "first", "hidden");										// scale to hidden
			SetShape (0);															// change to sphere
		}
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third
			ScaleTo (true, "first", "hidden");										// scale to hidden
			SetShape (0);															// change to sphere
			ScaleTo (false, "hidden", "third");										// scale to third
		}
		// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			ScaleTo (true, "first", "hidden");										// scale to hidden
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
			// scale to third
			ScaleTo (false, "first", "third");										// scale to third
		}

		///// third \\\\\

		// to fourth

		// from dark third
			// to dark fourth (no core change)
		if (fromState == 3 && toState == 4 && !fromLight && toLight) {			// to light fourth
			ScaleTo (false, "hidden", "third");										// scale to third
		}
		// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {			// to dark fourth
			ScaleTo (true, "third", "hidden");										// scale to hidden
		}
		// to light fourth (no core change)

		///// fourth \\\\\

		// to fifth

		// from dark fourth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from light fourth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {				// to triangle fifth
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (1);																			// change to triangle
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		// to square fifth
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (2);																			// change to square
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		///// fifth \\\\\

		// to sixth

		// from dark circle fifth
		// to dark circle sixth (no core change)
		if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) {			// to light circle sixth
			// change to sphere
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from light circle fifth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to light circle sixth (no core change)

		// from triangle fifth
		// to dark triangle sixth (no core change)
			
		// from square fifth
		// to dark square sixth (no core change)

		///// sixth \\\\\

		// to seventh

		// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			ScaleTo (false, "first", "seventh");														// scale to seventh
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (0);																			// change to ring
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			ScaleTo (false, "first", "seventh");													// scale to seventh
		}
		// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			ScaleTo (false, "first", "seventh");													// scale to seventh
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) {		// to light triangle seventh
			ScaleTo (false, "first", "seventh");													// scale to seventh
		}
		// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			ScaleTo (false, "first", "seventh");													// scale to seventh
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) {		// to light square seventh
			ScaleTo (false, "first", "seventh");													// scale to seventh
		}

		///// seventh \\\\\

		// to eighth

		// from dark circle seventh
		// to dark circle eighth (no core change)
		if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) {		// to light circle eighth
			// change to sphere
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (0);																			// change to ring
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}

		// from light circle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			// to dark circle eighth
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
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

		///// eighth \\\\\
		///// ninth \\\\\
		///// tenth \\\\\

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
			ScaleTo (true, "zero", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
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
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}

		// from light first
			// to zero
		if (fromState == 1 && toState == 0 && fromLight && toLight) {							// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
			// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
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
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to first
		}
			// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
			// to first
				// to dark first (no core change)
		if (fromState == 2 && toState == 1 && !fromLight && toLight) {							// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		// from light second
			// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) {							// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
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
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {							// to dark first
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		// from light third	
			// to zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to zero
			ScaleTo (true, "third", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) {							// to dark first
			ScaleTo (true, "third", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (true, "third", "first");														// scale to first
		}
			// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			ScaleTo (true, "third", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			ScaleTo (true, "third", "first");														// scale to first
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
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {							// to dark second
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 4 && toState == 2 && !fromLight && toLight) {						// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
			// to dark third (no core change)
		if (fromState == 4 && toState == 3 && !fromLight && toLight) {							// to light third
			ScaleTo (false, "hidden", "third");														// scale to third
		}

		// from light fourth	
			// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) {							// to zero
			ScaleTo (true, "third", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "third", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			ScaleTo (true, "third", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			ScaleTo (true, "third", "hidden");														// scale to first
		}
			// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			ScaleTo (true, "third", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 4 && toState == 2 && fromLight && toLight) {						// to light second
			ScaleTo (true, "third", "first");														// scale to first
		}
			// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {							// to dark third
			ScaleTo (true, "third", "hidden");														// scale to hidden
		}
		// to light third (no core change)

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
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
			// to first
				// to dark first (no core change)
		if (fromState == 5 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
				// to dark second (no core change)
		if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}

		// from light circle fifth
			// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to light first (no core change)

			// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to light second (no core change)
			
			// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (false, "first", "third");														// scale to zero
		}
	
			// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}

		// from triangle fifth
			// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {				// to light fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}

		// from square fifth
			// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
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
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
			// to first
				// to dark first (no core change)
		if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {			// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
				// to dark second (no core change)
		if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
			// to fifth
				// to dark circle fifth (no core change)
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		// from light circle sixth
			// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
				// to light first (no core change)
			// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
				// to light second (no core change)
			// to third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (false, "first", "third");														// scale to third
		}
			// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
			// to fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
				// to light circle fifth (no core change)

		// from triangle sixth
			// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {				// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
			// to fifth
				// to triangle fifth (no core change)

		// from square sixth
			// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {				// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
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
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "seventh", "zero");														// scale to zero
		}
			// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "seventh", "first");													// scale to first
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
			// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		// from light circle seventh
			// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "seventh", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "seventh", "first");														// scale to first
		}
			// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "seventh", "first");														// scale to first
		}
			// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "seventh", "third");													// scale to hidden
		}
			// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
			// to fifth
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
			// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (true, "seventh", "first");													// scale to first
		}

		// from dark triangle seventh
			// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to first
		}
			// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
			// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 1) {			// to triangle fifth
			ScaleTo (true, "seventh", "first");													// scale to first
		}
			// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			ScaleTo (true, "seventh", "first");													// scale to first
		}

		// from light triangle seventh
			// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {			// to light fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			ScaleTo (true, "seventh", "first");													// scale to first
		}
			// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			ScaleTo (true, "seventh", "first");													// scale to first
		}	

		// from dark square seventh
			// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			// change to sphere
			// scale to third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
			// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 2) {			// to square fifth
			ScaleTo (true, "seventh", "first");													// scale to first
		}
			// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {			// to dark square sixth
			ScaleTo (true, "seventh", "first");													// scale to first
		}

		// from light square seventh
			// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
			// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (3);																			// change to ring
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "third");														// scale to third
		}
			// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
		}
			// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {				// to square fifth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark square sixth
			ScaleTo (true, "seventh", "first");														// scale to first
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
		if (shape == 0) mesh = sphere;									// change mesh to sphere
		else if (shape == 1) mesh = triangle;							// change mesh to triangle
		else if (shape == 2) mesh = square;								// change mesh to square
		else if (shape == 3) mesh = ring;								// change mesh to ring
	}

	///<summary>
	///<para>set core as light</para>
	///<para>true = white</para>
	///<para>false = black</para>
	///</summary>
	private void SetLight (bool light)
	{
		if (light) rend.material.SetColor("_Color", Color.white);		// change to white
		else rend.material.SetColor("_Color", Color.black);				// change to black
	}

	///<summary>
	///<para> devol = whether to scale up or scale down </para>
	///<para> resetState = state to set to false </para>
	///<para> setState = state to set to true </para>
	///</summary>
	private void ScaleTo (bool devol, string resetState, string setState)
	{
		if (devol) {
			anim.ResetTrigger ("scaleup");								// reset last stage
			anim.SetTrigger ("scaledown");								// enable scaledown
		}
		else {
			anim.ResetTrigger ("scaledown");							// reset last stage
			anim.SetTrigger ("scaleup");								// enable scaleup
			
		}
		anim.SetBool(resetState, false);								// reset previously active state
		anim.SetBool(setState, true);									// set new active state
	}

}
