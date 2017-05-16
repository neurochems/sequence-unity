using UnityEngine;
using System.Collections;

public class ParticleCoreManager : MonoBehaviour {

	private Animator anim;							// animator on core ref
	#pragma warning disable 0414
	private Mesh mesh;								// core mesh
	#pragma warning restore 0414
	public Mesh sphere, triangle, square;			// shape meshes
	private MeshRenderer rend;						// mesh renderer (for colour changes)
	private ParticleStatePattern psp;				// psp ref

	private int toState;							// to state indicator
	private bool resetScale = false;				// timer trigger for resetting scale after world switch
	public float resetScaleTimer;					// reset scale timer

    private Shader lightShader, darkShader;         // light/dark shaders

    void Awake () {
		anim = GetComponent<Animator>();							// init animator ref
		mesh = GetComponent<MeshFilter>().mesh;						// init mesh ref
		rend = GetComponent<MeshRenderer>();						// init mesh renderer ref
		psp = GetComponentInParent<ParticleStatePattern> ();        // init psp ref

        //lightShader = Shader.Find("light_nucleus");					// init light nucleus shader
        //darkShader = Shader.Find("dark_nucleus");						// init dark nucleus shader
    }

	void Update() {
		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;															// start timer
		if (resetScaleTimer >= 4.0f) {																				// when timer >= 4 sec
			//anim.ResetTrigger("colour");	
			if (toState == 0) ScaleTo (false, "hidden", "zero");														// if to zero, grow to zero
			if (toState == 1 || toState == 2 || toState == 5 || toState == 6) ScaleTo (false, "hidden", "first");		// if to first/second/fifth/sixth, grow to first
			if (toState == 3 || toState == 4) ScaleTo (false, "hidden", "third");										// if to third/fourth, grow to third
			if (toState == 7 || toState == 8) ScaleTo (false, "hidden", "seventh");										// if to seventh/eighth, grow to seventh
			if (toState == 9) ScaleTo (false, "hidden", "ninth");														// if to ninth, grow to ninth
			resetScale = false;																							// reset reset scale flag
			resetScaleTimer = 0f;																						// reset timer
		}
	}

    ///<summary>
    ///<para> method for world changing anims </para>
    ///</summary>
    public void ToOtherWorld (bool lw, int f, int t, bool l) 
	{
		toState = t;																				// set to state																		
		if (lw) {																					// if to light world
            // from changes
            if (f == 0) {
                ScaleTo(true, "zero", "hidden");                                                        // scale from zero
               // Debug.Log("ZERO TO HIDDEN");
            }
			else if (f == 1 || f == 2 || f == 5 || f == 6) ScaleTo(true, "first", "hidden");        	// scale from first
			else if (f == 3 || f == 4) ScaleTo(true, "third", "hidden");                                // scale from third
			else if (f == 7 || f == 8) ScaleTo(true, "seventh", "hidden");                              // scale from seventh
			else if (f == 9) ScaleTo(true, "ninth", "hidden");											// scale from ninth

			// to changes
			if (t == 0) SetLight(false, false);															// if to light world zero, change to black
			else if (t == 1 || t == 2 || t == 5 || t == 6) SetLight(false, false);						// if to light world first/second/fifth/sixth, change to black
			else if (t == 3 || t == 4) SetLight(false, false);											// if to light world third/fourth, change to black
			else if (t == 7 || t == 8) SetLight(false, false);											// if to light world seventh/eighth, change to black
			else if (t == 9) SetLight(false, false);													// if to light world ninth, change to black
		}

		else if (!lw) {																				// if to dark world
			if (f == 0) {																				// if from zero
				ScaleTo (true, "zero", "hidden");															// scale from zero
				SetLight(true, false);																		// change to white
				ScaleTo (false, "hidden", "zero");															// scale to zero
			}
		}
	}

	public void Core (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{
		// EVOLUTIONS \\

		///// zero \\\\\

		if (fromState == 0 && toState == 0 && fromLight && toLight) {			// to zero (init)
			ScaleTo (false, "hidden", "zero");										// scale to first
		}

		// to dark zero (0.5) (no core change)
		// to light zero (0.5) (no core change)

		// to first
		// from dark zero (0.5)
		if (fromState == 0 && toState == 1 && !fromLight && !toLight) {			// to dark first
			ScaleTo (false, "zero", "first");										// scale to first
		}
		else if (fromState == 0 && toState == 1 && !fromLight && toLight) {		// to light first
			ScaleTo (false, "zero", "first");										// scale to first
		}
		// from light zero (0.5)
		if (fromState == 0 && toState == 1 && fromLight && !toLight) {			// to dark first
			ScaleTo (false, "zero", "first");										// scale to first
		}
		else if (fromState == 0 && toState == 1 && fromLight && toLight) {		// to light first
			ScaleTo (false, "zero", "first");										// scale to first
		}

		///// first \\\\\

		// to second

		// from dark first
		// to dark second (no core change)
		// to light second (no core change)

		// from light first
		// to dark second (no core change)
		// to light second (no core change)

		///// second \\\\\

		// to third

		// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			ScaleTo (false, "first", "hidden");										// scale to hidden
		}
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third
			ScaleTo (false, "first", "third");										// scale to third
		}
		// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			ScaleTo (false, "first", "hidden");										// scale to hidden
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
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
		// to light circle sixth (no core change)
		// from light circle fifth
		// to dark circle sixth (no core change)
		// change shader to add faux nucleus/visual effect
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
			ScaleTo (false, "first", "seventh");													// scale to seventh
		}
		// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			ScaleTo (false, "first", "seventh");													// scale to seventh
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
		// to light circle eighth (no core change)

		// from light circle seventh
		// to dark circle eighth (no core change)
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

		///// dark zero (0.5) \\\\\

		// to zero (no core change)

		///// first \\\\\

		// from dark first
		// to zero
		if (fromState == 1 && toState == 0 && !fromLight && toLight) {							// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
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
			ScaleTo (true, "first", "zero");														// scale to zero
		}

		///// second \\\\\

		// from dark second
		// to zero
		if (fromState == 2 && toState == 0 && !fromLight && toLight) {							// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to first
		// to dark first (no core change)
		// to light first (no core change)

		// from light second
		// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to first
		// to dark first (no core change)
		// to light first (no core change)

		///// third \\\\\

		// from dark third	
		// to zero
		if (fromState == 3 && toState == 0 && !fromLight && toLight) {							// to zero
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {							// to dark first
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
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
			ScaleTo (true, "third", "zero");														// scale to zero
		}
		// to first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) {							// to dark first
			ScaleTo (true, "third", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (true, "third", "first");														// scale to first
		}
		// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			ScaleTo (true, "third", "first");														// scale to first
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			ScaleTo (true, "third", "first");														// scale to first
		}

		///// fourth \\\\\

		// from dark fourth	
		// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {							// to zero
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			ScaleTo (false, "hidden", "first");														// scale to first
		}
		// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {							// to dark second
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
			ScaleTo (true, "third", "zero");														// scale to zero
		}
		// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			ScaleTo (true, "third", "zero");														// scale to zero
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			ScaleTo (true, "third", "first");														// scale to first
		}
		// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			ScaleTo (true, "third", "first");														// scale to first
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

		// from dark circle fifth
		// to zero
		if (fromState == 5 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to first
		// to dark first (no core change)
		// to light first (no core change)
		// to second
		// to dark second (no core change)
		// to light second (no core change)
		// to third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (false, "false", "third");														// scale to third
		}
		// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}

		// from light circle fifth
		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to first
		// to dark first (no core change)
		// to light first (no core change)
		// to second
		// to dark second (no core change)
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
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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

		// from dark circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to first
		// to dark first (no core change)
		// to light first (no core change)
		// to second
		// to dark second (no core change)
		// to light second (no core change)
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (false, "first", "third");														// scale to third
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "first", "hidden");														// scale to hidden
		}
		// to fifth
		// to dark circle fifth (no core change)
		// to light circle fifth (no core change)

		// from light circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "first", "zero");														// scale to zero
		}
		// to first
		// to dark first (no core change)
		// to light first (no core change)
		// to second
		// to dark second (no core change)
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
		// to dark circle fifth (no core change)
		// to light circle fifth (no core change)

		// from triangle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "first", "hidden");														// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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

		// from dark circle seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			ScaleTo (true, "seventh", "zero");														// scale to zero
		}
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			ScaleTo (true, "seventh", "zero");														// scale to zero
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			ScaleTo (true, "seventh", "third");														// scale to third
		}
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
		}
		// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			ScaleTo (true, "seventh", "first");														// scale to first
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
			ScaleTo (true, "seventh", "zero");													// scale to zero
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			ScaleTo (true, "seventh", "first");														// scale to first
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
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			ScaleTo (true, "seventh", "first");														// scale to first
		}
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			ScaleTo (true, "seventh", "first");														// scale to first
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
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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
			ScaleTo (false, "hidden", "third");														// scale to third
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
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
			ScaleTo (false, "hidden", "zero");														// scale to zero
		}
		// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			ScaleTo (true, "seventh", "hidden");													// scale to hidden
			SetShape (0);																			// change to sphere
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
			SetShape (0);																			// change to sphere
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
	private void SetLight (bool light, bool shade)
	{
        if (light)
        {
            //rend.material.SetColor("_Color", Color.white);		        // change to white
            //anim.SetTrigger("colour");                                  // set colour change trigger
            anim.SetBool("black", false);                               // reset previously active state
            anim.SetBool("white", true);                                // set active state
        }
        else
        {
            //rend.material.SetColor("_Color", Color.black);              // change to black
            //anim.SetTrigger("colour");                                  // set colour change trigger
            anim.SetBool("white", false);                               // reset previously active state
            anim.SetBool("black", true);                                // set active state
        }
        //anim.ResetTrigger("colour");                                    // reset colour change trigger

        if (shade && light) rend.material.shader = lightShader;			// change to white shader
        else if (shade && !light) rend.material.shader = darkShader;    // change to white shader


		resetScale = true;

    }

	///<summary>
	///<para> devol = whether to scale up or scale down </para>
	///<para> resetState = state to set to false </para>
	///<para> setState = state to set to true </para>
	///</summary>
	private void ScaleTo (bool devol, string resetState, string setState)
	{
        //Debug.Log("ParticleCore ScaleTo");
        if (devol) {
			anim.ResetTrigger ("scaleup");								// reset last stage
			anim.SetTrigger ("scaledown");								// enable scaledown
		}
		else {
			anim.ResetTrigger ("scaledown");							// reset last stage
			anim.SetTrigger ("scaleup");								// enable scaleup

		}
		anim.SetBool(resetState, false);								// reset previously active state
		anim.SetBool(setState, true);									// set active state
	}
}
