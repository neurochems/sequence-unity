using UnityEngine;
using System.Collections;

public class ParticleShellManager : MonoBehaviour {

	private Animator anim;						// animator on core ref
	private MeshRenderer rend;					// mesh renderer (for colour changes)
	private ParticleStatePattern psp;				// psp ref

	private int toState;							// to state indicator
	private bool resetScale = false;				// timer trigger for resetting scale after world switch
	public float resetScaleTimer;					// reset scale timer

	void Awake () {
		anim = GetComponent<Animator>();							// init animator ref
		rend = GetComponent<MeshRenderer>();						// init mesh renderer ref
		psp = GetComponentInParent<ParticleStatePattern> ();		// init psp ref
	}

	void Update() {
		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;													// start timer
		if (resetScaleTimer >= 4.0f) {
			//anim.ResetTrigger("colour");
			if (toState == 3 || toState == 4 || toState == 5 || toState == 6) ScaleTo (false, "hidden", "third");
			if (toState == 7 || toState == 8) ScaleTo (false, "hidden", "seventh");
			if (toState == 9) ScaleTo (false, "hidden", "ninth");
			resetScale = false;
			resetScaleTimer = 0f;
		}
	}

	public void ToOtherWorld (bool lw, int f, int t, bool l)
	{
		toState = t;																			// set to state
		if (lw) {																				// if to light world
			// from changes
			if (f == 3 || f == 4 || f == 5 || f == 6) ScaleTo (true, "third", "hidden");			// scale from third
			else if (f == 7 || f == 8) ScaleTo (true, "seventh", "hidden");							// scale from seventh	
			else if (f == 9) ScaleTo (true, "ninth", "hidden");										// scale from ninth
			// to changes
			if (f == 3 || f == 4 || f == 5 || f == 6) SetLight(false);								// change to black
			else if (f == 7 || f == 8) SetLight(false);												// change to black
			else if (f == 9) SetLight(false);														// change to black
		}
		else if (!lw) SetLight(true);															// if to dark world, change to white
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

		///// dark zero (0.5) \\\\\

		// to zero (no shell change)

		///// first \\\\\

		// from dark first
		// to zero (no shell change)
		// to dark zero (0.5) (no shell change)

		// from light first
		// to zero (no shell change)
		// to dark zero (0.5) (no shell change)

		///// second \\\\\

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
	private void ScaleTo (bool devol, string setState, string resetState)
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
