using UnityEngine;
using System.Collections;

public class ParticleShellManager : MonoBehaviour {

	private Animator anim;						// animator on core ref
	private MeshRenderer rend;					// mesh renderer (for colour changes)
	private ParticleStatePattern psp;				// psp ref

	private int toState;							// to state indicator
	private bool resetScale = false;				// timer trigger for resetting scale after world switch
	private float resetScaleTimer;					// reset scale timer

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
			if (t == 3 || t == 4 || t == 5 || t == 6) SetLight(false);								// change to black
			else if (t == 7 || t == 8) SetLight(false);												// change to black
			else if (t == 9) SetLight(false);														// change to black
			else if (t == 0 || t == 1 || t == 2) SetLight(false);									// change to black
		}
		else if (!lw) {																			// if to dark world
			// from changes
			if (f == 3 || f == 4 || f == 5 || f == 6) ScaleTo (true, "third", "hidden");			// scale from third
			else if (f == 7 || f == 8) ScaleTo (true, "seventh", "hidden");							// scale from seventh	
			else if (f == 9) ScaleTo (true, "ninth", "hidden");										// scale from ninth
			// to changes
			if (t == 3 || t == 4 || t == 5 || t == 6) SetLight(true);								// change to white
			else if (t == 7 || t == 8) SetLight(true);												// change to white
			else if (t == 9) SetLight(true);														// change to white
			else if (t == 0 || t == 1 || t == 2) SetLight(true);									// change to white
		}
	}

	public void Shell (int f, int t, bool fl, bool tl, int s) 
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
			// to dark third
		if (f == 2 && t == 3 && !fl && !tl) {
			ScaleTo (false, "hidden", "third");										// scale to third
			Debug.Log (transform.parent.name + " shell dark second to dark third");
		}
			// to light third (no shell change)

		// from light second
			// to dark third
		if (f == 2 && t == 3 && fl && !tl) {
			Debug.Log (transform.parent.name + " shell light second to dark third");
			ScaleTo (false, "hidden", "third");										// scale to third
		}
			// to light third (no shell change)

	///// third \\\\\

		// to fourth

		// from dark third
			// to dark fourth (no shell change)
			// to light fourth
		if (f == 3 && t == 4 && !fl && tl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// from light third
			// to dark fourth
		if (f == 3 && t == 4 && fl && !tl) ScaleTo (false, "hidden", "third");										// scale to third
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
			// to dark circle seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "third", "seventh");							// scale to seventh
			// to light circle seventh
		else if (f == 6 && t == 7 && !fl && tl && s == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		// from light circle sixth
			// to dark circle seventh
		if (f == 6 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "third", "seventh");							// scale to seventh
			// to light circle seventh
		else if (f == 6 && t == 7 && fl && tl && s == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		// from dark triangle sixth
			// to dark triangle seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");							// scale to seventh
			// to light triangle seventh (no shell change)

		// from dark square sixth
			// to dark square seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");							// scale to seventh
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
			// to light triangle eighth
		if (f == 7 && t == 8 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden

		// from light triangle seventh
			// to dark triangle eighth
		if (f == 7 && t == 8 && fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");							// scale to seventh
			// to light triangle eighth (no shell change)

		// from dark square seventh
			// to dark square eighth (no shell change)
			// to light square eighth
		if (f == 7 && t == 8 && !fl && tl && s == 2) ScaleTo (false, "hidden", "seventh");							// scale to seventh

		// from light square seventh
			// to dark square eighth
		if (f == 7 && t == 8 && fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");							// scale to seventh
			// to light square eighth (no shell change)

	///// eighth \\\\\

		// to ninth

		// from dark circle eighth
			// to dark circle ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light circle seventh
		else if (f == 8 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");						// scale to ninth
		// from light circle eighth
			// to dark circle ninth
		if (f == 8 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light circle ninth
		else if (f == 8 && t == 9 && fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");						// scale to ninth
		// from dark triangle eighth
			// to dark triangle ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 1) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light triangle ninth
		else if (f == 8 && t == 9 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// from dark square eighth
			// to dark square ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 2) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light square ninth
		else if (f == 8 && t == 9 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden

	///// ninth \\\\\
		// no tenth state in particle

	///// player tenth \\\\\

		// from third
		else if (f == 3 && t == 10 && !fl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// from fourth
		else if (f == 4 && t == 10 && !fl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// from fifth
		else if (f == 5 && t == 10 && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		// from sixth
		else if (f == 6 && t == 10 && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		// from seventh
		else if (f == 7 && t == 10 && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 10 && !fl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		else if (f == 7 && t == 10 && !fl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// from eighth
		else if (f == 8 && t == 10) ScaleTo (true, "seventh", "hidden");											// scale to hidden
		else if (f == 8 && t == 10 && !fl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		else if (f == 8 && t == 10 && !fl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// from ninth
		else if (f == 9 && t == 10) ScaleTo (true, "ninth", "hidden");												// scale to hidden
		else if (f == 9 && t == 10 && !fl && s == 1) ScaleTo (true, "ninth", "hidden");								// scale to hidden
		else if (f == 9 && t == 10 && !fl && s == 2) ScaleTo (true, "ninth", "hidden");								// scale to hidden

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
		if (f == 3 && t == 0 && !fl && tl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// to dark zero
		if (f == 3 && t == 0 && !fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// to first
			// to dark first
		if (f == 3 && t == 1 && !fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
			// to light first
		else if (f == 3 && t == 1 && !fl && tl) ScaleTo (true, "third", "hidden");									// scale to hidden
		// to second
			// to dark second
		if (f == 3 && t == 2 && !fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
			// to light second
		else if (f == 3 && t == 2 && !fl && tl) ScaleTo (true, "third", "hidden");									// scale to hidden

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
		if (f == 4 && t == 0 && !fl && tl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// to dark zero
		if (f == 4 && t == 0 && !fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// to first
			// to dark first
		if (f == 4 && t == 1 && !fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
			// to light first
		else if (f == 4 && t == 1 && !fl && tl) ScaleTo (true, "third", "hidden");									// scale to hidden
		// to second
			// to dark second
		if (f == 4 && t == 2 && !fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
			// to light second
		else if (f == 4 && t == 2 && !fl && tl) ScaleTo (true, "third", "hidden");									// scale to hidden
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
			// to dark third
		if (f == 4 && t == 3 && fl && !tl) ScaleTo (false, "hidden", "third");										// scale to third
			// to light third (no shell change)

///// fifth \\\\\

	// from dark circle fifth
		// to zero
		if (f == 5 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to dark zero
		if (f == 5 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
		// to first
			// to dark first
		if (f == 5 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
			// to light first
		else if (f == 5 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");						// scale to hidden
		// to second
			// to dark second
		if (f == 5 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
			// to light second
		else if (f == 5 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");						// scale to hidden
		// to third
			// to dark third (no shell change)
			// to light third
		if (f == 5 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to fourth
			// to dark fourth (no shell change)

	// from light circle fifth
		// to zero
		if (f == 5 && t == 0 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to dark zero
		if (f == 5 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to first
			// to dark first
		if (f == 5 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
			// to light first
		else if (f == 5 && t == 1 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
		// to second
			// to dark second
		if (f == 5 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
			// to light second
		else if (f == 5 && t == 2 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
		// to third
			// to dark third (no shell change)
			// to light third
		if (f == 5 && t == 3 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
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
			// to dark third
		if (f == 5 && t == 3 && fl && !tl && s == 1) ScaleTo (false, "hidden", "third");							// scale to third
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
			// to dark third
		if (f == 5 && t == 3 && fl && !tl && s == 2) ScaleTo (false, "hidden", "third");							// scale to third
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)

///// sixth \\\\\

	// from dark circle sixth
		// to zero
		if (f == 6 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to dark zero
		if (f == 6 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
		// to first
			// to dark first
		if (f == 6 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
			// to light first
		else if (f == 6 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");						// scale to hidden
		// to second
			// to dark second
		if (f == 6 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
			// to light second
		else if (f == 6 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");						// scale to hidden
		// to third
			// to dark third (no shell change)
			// to light third
		if (f == 6 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to fourth
			// to dark fourth (no shell change)
		// to fifth
			// to dark circle fifth (no shell change)
			// to light circle fifth (no shell change)

	// from light circle sixth
		// to zero
		if (f == 6 && t == 0 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to dark zero
		if (f == 6 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
		// to first
			// to dark first
		if (f == 6 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
			// to light first
		else if (f == 6 && t == 1 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
		// to second
			// to dark second
		if (f == 6 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");								// scale to hidden
			// to light second
		else if (f == 6 && t == 2 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");							// scale to hidden
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
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "third");							// scale to third
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
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "third");							// scale to third
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to square fifth (no shell change)

///// seventh/eighth \\\\\

	// from dark circle seventh
		// to zero
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to fourth
			// to dark fourth
		if ((f == 7 || f == 8) && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
		// to fifth
			// to dark circle fifth
		if ((f == 7 || f == 8) && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
			// to light circle fifth
		else if ((f == 7 || f == 8) && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");						// scale to third
		// to sixth
			// to dark circle sixth
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
			// to light circle sixth
		else if ((f == 7 || f == 8) && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");						// scale to third

	// from light circle seventh
		// to zero
		if ((f == 7 || f == 8) && t == 0 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to fourth
			// to dark fourth
		if ((f == 7 || f == 8) && t == 4 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
		// to fifth
			// to dark circle fifth
		if ((f == 7 || f == 8) && t == 5 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
			// to light circle fifth
		else if ((f == 7 || f == 8) && t == 5 && fl && tl && s == 0) ScaleTo (true, "seventh", "third");						// scale to third
		// to sixth
			// to dark circle sixth
		if ((f == 7 || f == 8) && t == 6 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");							// scale to third
			// to light circle sixth
		else if ((f == 7 || f == 8) && t == 6 && fl && tl && s == 0) ScaleTo (true, "seventh", "third");						// scale to third

	// from dark triangle seventh
		// to zero
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "third");							// scale to third
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to fourth
			// to light fourth
		if ((f == 7 || f == 8) && t == 4 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to fifth
			// to triangle fifth
		if ((f == 7 || f == 8) && t == 5 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to sixth
			// to dark triangle sixth
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden

	// from dark triangle eighth
		// to seventh
			// to dark triangle seventh (no shell change)
			// to light triangle seventh
		if (f == 8 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden

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
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && fl && !tl && s == 1) ScaleTo (true, "seventh", "third");							// scale to third
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to triangle fifth (no shell change)
		// to sixth
			// to dark triangle sixth (no shell change)

	// from dark square seventh
		// to zero
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "third");							// scale to third
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to fourth
			// to light fourth
		if ((f == 7 || f == 8) && t == 4 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to fifth
			// to square fifth
		if ((f == 7 || f == 8) && t == 5 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to sixth
			// to dark square sixth
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden

	// from dark square eighth
		// to seventh
			// to dark square seventh (no shell change)
			// to light square seventh
		if (f == 8 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden

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
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && fl && !tl && s == 2) ScaleTo (true, "seventh", "third");							// scale to third
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to square fifth (no shell change)
		// to sixth
			// to dark square sixth (no shell change)

///// ninth \\\\\

	// from dark circle ninth
		// to zero
		if (f == 9 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
			// to light first
		else if (f == 9 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to third
			// to dark third
		if (f == 9 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
			// to light third
		else if (f == 9 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 9 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
		// to fifth
			// to dark circle fifth
		if (f == 9 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
			// to light circle fifth
		else if (f == 9 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		// to sixth
			// to dark circle sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
			// to light circle sixth
		else if (f == 9 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		// to seventh
			// to dark circle seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light circle seventh
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		// to eighth
			// to dark circle eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light circle eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh

	// from light circle ninth
		// to zero
		if (f == 9 && t == 0 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to first
			// to dark first
		if (f == 9 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
			// to light first
		else if (f == 9 && t == 1 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		// to second
			// to dark second
		if (f == 9 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
			// to light second
		else if (f == 9 && t == 2 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		// to third
			// to dark third
		if (f == 9 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
			// to light third
		else if (f == 9 && t == 3 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 9 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
		// to fifth
			// to dark circle fifth
		if (f == 9 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
			// to light circle fifth
		else if (f == 9 && t == 5 && fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		// to sixth
			// to dark circle sixth
		if (f == 9 && t == 6 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");											// scale to third
			// to light circle sixth
		else if (f == 9 && t == 6 && fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		// to seventh
			// to dark circle seventh
		if (f == 9 && t == 7 && fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light circle seventh
		else if (f == 9 && t == 7 && fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		// to eighth
			// to dark circle eighth
		if (f == 9 && t == 8 && fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light circle eighth
		else if (f == 9 && t == 8 && fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh

	// from dark triangle ninth
		// to zero
		if (f == 9 && t == 0 && fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 1) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "hidden");										// scale to hidden
			// to light first
		else if (f == 9 && t == 1 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "hidden");										// scale to hidden
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to third
			// to dark third
		if (f == 9 && t == 3 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "third");											// scale to third
			// to light third
		else if (f == 9 && t == 3 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to fifth
			// to triangle fifth
		if (f == 9 && t == 5 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to sixth
			// to dark triangle sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		// to seventh
			// to dark triangle seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light triangle seventh
		else if (f == 9 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to eighth
			// to dark tiangle eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light triangle eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden

	// from light triangle ninth
		// to zero (no shell change)
		// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
			// to dark third
		if (f == 9 && t == 3 && fl && !tl && s == 1) ScaleTo (false, "hidden", "third");										// scale to third
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to triangle fifth (no shell change)
		// to sixth
			// to dark triangle sixth (no shell change)
		// to seventh
			// to dark triangle seventh
		if (f == 9 && t == 7 && fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh
			// to light triangle seventh (no shell change)
		// to eighth
			// to dark triangle eighth
		if (f == 9 && t == 8 && fl && !tl && s == 1) ScaleTo (true, "hidden", "seventh");										// scale to seventh
			// to light triangle eighth (no shell change)

	// from dark square ninth
		// to zero
		if (f == 9 && t == 0 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "hidden");										// scale to hidden
			// to light first
		else if (f == 9 && t == 1 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "hidden");										// scale to hidden
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to third
			// to dark third
		if (f == 9 && t == 3 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "third");											// scale to third
			// to light third
		else if (f == 9 && t == 3 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to fifth
			// to square fifth
		if (f == 9 && t == 5 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to sixth
			// to dark square sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		// to seventh
			// to dark square seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light square seventh
		else if (f == 9 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to eighth
			// to dark square eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light square eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden

	// from light square seventh/eighth
		// to zero (no shell change)
		// to dark zero (no shell change)
		// to first
			// to dark first (no shell change)
			// to light first (no shell change)
		// to second
			// to dark second (no shell change)
			// to light second (no shell change)
		// to third
			// to dark third
		if (f == 9 && t == 3 && fl && !tl && s == 2) ScaleTo (true, "hidden", "third");											// scale to third
			// to light third (no shell change)
		// to fourth
			// to light fourth (no shell change)
		// to fifth
			// to square fifth (no shell change)
		// to sixth
			// to dark square sixth (no shell change)
		// to seventh
			// to dark square seventh
		if (f == 9 && t == 7 && fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light square seventh (no shell change)
		// to eighth
			// to dark square eighth
		if (f == 9 && t == 8 && fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");										// scale to seventh
			// to light square eighth (no shell change)
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
			anim.ResetTrigger ("scaleup");											// reset last stage
			anim.SetTrigger ("scaledown");											// enable scaledown
		}
		else {
			anim.ResetTrigger ("scaledown");										// reset last stage
			anim.SetTrigger ("scaleup");											// enable scaleup

		}
		anim.SetBool(resetState, false);											// reset previously active state
		anim.SetBool(setState, true);												// set active state
		Debug.Log (transform.parent.name + " shell second to dark third");
	}
}
