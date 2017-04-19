using UnityEngine;
using System.Collections;

public class PlayerNucleusManager : MonoBehaviour {

	private Animator anim;							// animator on core ref
	#pragma warning disable 0414
	private Mesh mesh;								// core mesh
	#pragma warning restore 0414
	public Mesh sphere, triangle, square;			// shape meshes
	private MeshRenderer rend;						// mesh renderer (for colour changes)
	private PlayerStatePattern psp;					// psp ref
	private bool light; 							// is light flag

	void Awake () {
		anim = GetComponent<Animator>();							// init animator ref
		mesh = GetComponent<MeshFilter>().mesh;						// init mesh ref
		rend = GetComponent<MeshRenderer>();						// init mesh renderer ref
		psp = GameObject.Find ("Player")
			.gameObject.GetComponent<PlayerStatePattern> ();		// init psp ref
	}

	void Update() {
		if (light && psp.changeParticles) rend.material.SetColor("_Color", Color.black);			// if light && light world, change to black
		else if (!light && psp.changeParticles) rend.material.SetColor("_Color", Color.white);		// if not light && light world, change to white
	}

	public void Nucleus (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{
		// EVOLUTIONS \\

		///// zero \\\\\

		// to dark zero (0.5)

		// from zero
			// to dark zero (no nucleus change)
			
		// to first

		// from dark zero (0.5)
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
			
		// from light zero (0.5)
			// to dark first (no nucleus change)
			// to light first (no nucleus change)

		///// first \\\\\

		// to second

		// from dark first
		if (fromState == 1 && toState == 2 && !fromLight && !toLight) {			// to dark second
			SetLight(true);															// change to white
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		else if (fromState == 1 && toState == 2 && !fromLight && toLight) {		// to light second
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		// from light first
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			// to dark second
			SetLight(true);															// change to white
			ScaleTo (false, "hidden", "first");										// scale to first
		}
		else if (fromState == 1 && toState == 2 && fromLight && toLight) {		// to light first
			ScaleTo (false, "hidden", "first");										// scale to first
		}

		///// second \\\\\

		// to third

		// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			ScaleTo (true, "first", "hidden");										// scale to hidden
			SetLight(false);														// change to black
		} 
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third 
			ScaleTo (true, "first", "hidden");										// scale to hidden
			SetLight(false);														// change to black
		}
			// from light second
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to dark third
			// scale to zero
			ScaleTo (true, "first", "hidden");										// scale to hidden
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
			ScaleTo (true, "first", "hidden");										// scale to hidden
		}

		///// third \\\\\

		// to fourth

		// from dark third
		if (fromState == 3 && toState == 4 && !fromLight && !toLight) {							// to dark fourth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 4 && !fromLight && toLight) {						// to light fourth
			// scale to first
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {							// to dark fourth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 4 && fromLight && toLight) {						// to light fourth
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		///// fourth \\\\\

		// to fifth

		// from dark fourth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// from light fourth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(1);																			// change to triangle
		}
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		// to square fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(2);																			// change to square
		}

		///// fifth \\\\\

		// to sixth

		// from dark circle fifth
		if (fromState == 5 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from light circle fifth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 5 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from triangle fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 1) {						// to dark triangle sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from square fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 2) {						// to dark square sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		//// sixth \\\\\

		// to seventh

		// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			ScaleTo (false, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			ScaleTo (false, "first", "hidden");														// scale to hidden
		}
		// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			ScaleTo (false, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) {		// to light triangle seventh
			ScaleTo (false, "first", "hidden");														// scale to hidden
		}
		// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			ScaleTo (false, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) {		// to light square seventh
			ScaleTo (false, "first", "hidden");														// scale to hidden
		}

		///// seventh \\\\\

		// to eighth

		// from dark circle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 0) {			// to dark circle eighth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) {		// to light circle eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		// from light circle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			// to dark circle eighth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 0) {		// to light circle eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		// from dark triangle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 1) {		// to light triangle eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		// from light triangle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 1) {		// to light triangle eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		// from dark square seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 2) {			// to dark square eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 2) {		// to light square eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		// from light square seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 2) {			// to dark square eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 2) {		// to light square eighth
			ScaleTo (false, "hidden", "seventh");													// scale to seventh
		}

		// DEVOLUTIONS \\

		///// zero \\\\\

		// to dead
		if (fromState == 0 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		///// dark zero (0.5) \\\\\

			// to zero (no nucleus change)

		///// first \\\\\

		// to dead
		if (fromState == 1 && toState == -1) {													// to dead
			anim.ResetTrigger ("scaleup");										// reset next stage
			anim.SetTrigger("scaledown");										// enable core to black animation
			anim.SetBool("photon", true);										// enable black core animation state
		}

		// from dark first
			// to zero (no nucleus change)
			// to dark zero (0.5) (no nucleus change)
	
		// from light first
			// to zero (no nucleus change)
			// to dark zero (0.5) (no nucleus change)

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
			SetLight(false);																		// change to black
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to first
		if (fromState == 2 && toState == 1 && !fromLight && !toLight) {							// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		else if (fromState == 2 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// from light second
		// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) {							// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 2 && toState == 1 && fromLight && toLight) {						// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}

		///// third \\\\\

		// to dead
		if (fromState == 3 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark third	
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first 
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from light third	
			// to zero ((no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
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
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to second
			// to dark second (no nucleus change)
		if (fromState == 4 && toState == 2 && !fromLight && toLight) {						// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
		if (fromState == 4 && toState == 3 && !fromLight && !toLight) {							// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		else if (fromState == 4 && toState == 3 && !fromLight && toLight) {						// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// from light fourth	
		// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) {							// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to light second (no nucleus change)
		// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {							// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 4 && toState == 3 && fromLight && toLight) {						// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}

		///// fifth \\\\\

		// to dead
		if (fromState == 5 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger("scaledown");						// enable core to black animation
			anim.SetBool("photon", true);						// enable black core animation state
		}

		// from dark circle fifth
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 5 && toState == 2 && !fromLight && !toLight && shape == 0) {			// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// from light circle fifth
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		} 
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
	
		// from triangle fifth
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		} 
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {				// to light fourth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
	
		// from square fifth
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		} 
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			ScaleTo (false, "hidden", "first");														// scale to first
		}

		///// sixth \\\\\
	
		// to dead
		if (fromState == 6 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger ("scaledown");						// enable core to black animation
			anim.SetBool ("photon", true);						// enable black core animation state
		}
	
		// from dark circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		} 
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to second
			// to dark second (no nucleus change)
		if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {			// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		} 
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// to fourth
			// to dark fourth (no nucleus change)
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		else if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(false);																		// change to black
		}
		// from light circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
		} 
		else if (fromState == 6 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
			// to light second (no nucleus change)
		// to third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		} 
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
		} 
		else if (fromState == 6 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// from triangle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		} 
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 1) {			// to triangle fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// from square sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {				// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {			// to square fifth
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape(0);																			// change to sphere	
		}

		///// seventh \\\\\
	
		// to dead
		if (fromState == 7 && toState == -1) {									// to dead
			anim.ResetTrigger ("scaleup");						// reset next stage
			anim.SetTrigger ("scaledown");						// enable core to black animation
			anim.SetBool ("photon", true);						// enable black core animation state
		}
	
		// from dark circle seventh
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
			// to dark circle fifth (no nucleus change)
			// to light circle fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
	
		// from light circle seventh
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
			// to dark circle fifth (no nucleus change)
			// to light circle fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
	
		// from dark triangle seventh
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			SetShape(0);																			// change to sphere
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
			// to triangle fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
	
		// from light triangle seventh
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			SetShape(0);																			// change to sphere
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {			// to light fourth
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
			// to triangle fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}	
		// from dark square seventh
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			SetShape(0);																			// change to sphere
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		} 
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
			// to square fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {			// to dark square sixth
			ScaleTo (false, "hidden", "first");														// scale to first
		}
	
		// from light square seventh
			// to zero (no nucleus change)
			// to dark zero (no nucleus change)
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			SetShape(0);																			// change to sphere
			SetLight(true);																			// change to white
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			SetShape(0);																			// change to sphere
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to fifth
			// to square fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 2) {			// to dark square sixth
			ScaleTo (false, "hidden", "first");														// scale to first
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
	}

	///<summary>
	///<para>set core as light</para>
	///<para>true = white</para>
	///<para>false = black</para>
	///</summary>
	private void SetLight (bool light)
	{
		if (light && !psp.lightworld) {
			rend.material.SetColor("_Color", Color.white);			// change to white
			light = true;											// set is light flag
		} 
		else if (!light && !psp.lightworld) {
			rend.material.SetColor("_Color", Color.black);			// change to white
			light = false;											// reset is light flag
		}
		else if (light && psp.lightworld) {
			rend.material.SetColor("_Color", Color.black);			// change to white
			light = true;											// set is light flag
		}
		else if (!light && psp.lightworld) {
			rend.material.SetColor("_Color", Color.white);			// change to white
			light = false;											// reset is light flag
		}
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