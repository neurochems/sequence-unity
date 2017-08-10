using UnityEngine;
using System.Collections;

public class ParticleCoreManager : MonoBehaviour {

	private Animator anim;																							// animator on core ref
	private MeshRenderer rend;																						// mesh renderer (for colour changes)
	public Mesh sphere, triangle, square;																			// shape meshes
	//private ParticleStatePattern psp;																					// psp ref

	private int toState, shape;																						// to state indicator, shape index
	private bool changeShape = false, resetScale = false;															// timer trigger for changing shape, resetting scale after world switch
	private float changeShapeTimer, resetScaleTimer;																// change shape timer, reset scale timer

    private Shader lightShader, darkShader;         // light/dark shaders

    void Start () {
		anim = GetComponent<Animator>();																			// init animator ref
		rend = GetComponent<MeshRenderer>();																		// init mesh renderer ref
		//psp = GetComponentInParent<ParticleStatePattern> ();        													// init psp ref

        //lightShader = Shader.Find("light_nucleus");																	// init light nucleus shader
        //darkShader = Shader.Find("dark_nucleus");																		// init dark nucleus shader
    }

	void Update() {
		// change shape timer
		if (changeShape) changeShapeTimer += Time.deltaTime;														// start timer
		if (changeShapeTimer >= 2.0f) {																				// when timer >= 4 sec
			Debug.Log(transform.parent.name + " core set shape: " + shape);
			SetShape(shape);																							// set shape
			changeShape = false;																						// reset reset scale flag
			changeShapeTimer = 0f;																						// reset timer
		}
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
			if (t == 0) SetLight(true, false);															// if to light world zero, change to black
			else if (t == 1 || t == 2 || t == 5 || t == 6) SetLight(true, false);						// if to light world first/second/fifth/sixth, change to black
			else if (t == 3 || t == 4) SetLight(true, false);											// if to light world third/fourth, change to black
			else if (t == 7 || t == 8) SetLight(true, false);											// if to light world seventh/eighth, change to black
			else if (t == 9) SetLight(true, false);													// if to light world ninth, change to black
		}
	}

	public void Core (int f, int t, bool fl, bool tl, int s) 
	{
		// set up
		toState = t;																// set to state
		shape = s;																	// set shape

// EVOLUTIONS \\

	///// zero \\\\\

		// to zero (init)
		if (f == 0 && t == 0 && fl && tl) {
			ScaleTo (false, "hidden", "zero");										// scale to first
			//Debug.Log (transform.parent.name + " core init to light zero");
		}
		// to dark zero (init)
		else if (f == 0 && t == 0 && !fl && !tl) ScaleTo (false, "hidden", "zero");									// scale to first
		// to light zero (0.5) (no core change)

		// to first

		// from dark zero (0.5)
			// to dark first
		if (f == 0 && t == 1 && !fl && !tl) ScaleTo (false, "zero", "first");										// scale to first
			// to light first
		else if (f == 0 && t == 1 && !fl && tl) ScaleTo (false, "zero", "first");									// scale to first

		// from light zero (0.5)
			// to dark first
		if (f == 0 && t == 1 && fl && !tl) ScaleTo (false, "zero", "first");										// scale to first
			// to light first
		else if (f == 0 && t == 1 && fl && tl) {
			ScaleTo (false, "zero", "first");																		// scale to first
			anim.SetBool("hidden", false);																			// init: reset hidden
		}

	///// first \\\\\

		// to second

		// from dark first
			// to dark second (no core change)
			// to light second (no core change)

		// from light first
			// to dark second (no core change)
			// to light second (no core change)

		// to third

		// from dark first
			// to dark third
		if (f == 1 && t == 3 && !fl && !tl) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light third
		else if (f == 1 && t == 3 && !fl && tl) ScaleTo (false, "first", "third");									// scale to third
		// from dark first
			// to dark third
		if (f == 1 && t == 3 && fl && !tl) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light third
		else if (f == 1 && t == 3 && fl && tl) ScaleTo (false, "first", "third");									// scale to third

	///// second \\\\\

		// to third

		// from dark second
			// to dark third
		if (f == 2 && t == 3 && !fl && !tl) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light third
		else if (f == 2 && t == 3 && !fl && tl) ScaleTo (false, "first", "third");									// scale to third
		// from light second
			// to dark third
		if (f == 2 && t == 3 && fl && !tl) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light third
		else if (f == 2 && t == 3 && fl && tl) ScaleTo (false, "first", "third");									// scale to third

		// to fourth

		// from dark second
			// to dark fourth
		if (f == 2 && t == 4 && !fl && !tl) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light fourth
		else if (f == 2 && t == 4 && !fl && tl) ScaleTo (false, "first", "third");									// scale to third
		// from light second
			// to dark fourth
		if (f == 2 && t == 4 && fl && !tl) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light fourth
		else if (f == 2 && t == 4 && fl && tl) ScaleTo (false, "first", "third");									// scale to third

	///// third \\\\\

		// to fourth

		// from dark third
			// to dark fourth (no core change)
			// to light fourth
		if (f == 3 && t == 4 && !fl && tl) ScaleTo (false, "hidden", "third");										// scale to third
		// from light third
			// to dark fourth
		if (f == 3 && t == 4 && fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
			// to light fourth (no core change)

	///// fourth \\\\\

		// to fifth

		// from dark fourth
			// to dark circle fifth
		if (f == 4 && t == 5 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "first");							// scale to first
			// to light circle fifth
		else if (f == 4 && t == 5 && !fl && tl && s == 0) ScaleTo (false, "hidden", "first");						// scale to first
		// from light fourth
			// to triangle fifth
		if (f == 4 && t == 5 && fl && tl && s == 1) {
			ScaleTo (true, "zero", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to square fifth
		else if (f == 4 && t == 5 && fl && tl && s == 2) {
			ScaleTo (true, "zero", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
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
			// to dark circle seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "first", "seventh");							// scale to seventh
			// to light circle seventh
		else if (f == 6 && t == 7 && !fl && tl && s == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		// from light circle sixth
			// to dark circle seventh
		if (f == 6 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "first", "seventh");							// scale to seventh
			// to light circle seventh
		else if (f == 6 && t == 7 && fl && tl && s == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		// from dark triangle sixth
			// to dark triangle seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 1) ScaleTo (false, "first", "seventh");							// scale to seventh
			// to light triangle seventh
		else if (f == 6 && t == 7 && !fl && tl && s == 1) ScaleTo (false, "first", "seventh");						// scale to seventh
		// from dark square sixth
			// to dark square seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 2) ScaleTo (false, "first", "seventh");							// scale to seventh
			// to light square seventh
		else if (f == 6 && t == 7 && !fl && tl && s == 2) ScaleTo (false, "first", "seventh");						// scale to seventh

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

		// to ninth

		// from dark circle eighth
			// to dark circle ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light circle ninth
		else if (f == 8 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");						// scale to ninth
		// from light circle eighth
			// to dark circle ninth
		if (f == 8 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light circle ninth
		else if (f == 8 && t == 9 && fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");						// scale to ninth
		// from dark triangle eighth
			// to dark triangle ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 1) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light triangle seventh
		else if (f == 8 && t == 9 && !fl && tl && s == 1) ScaleTo (false, "seventh", "ninth");						// scale to ninth
		// from dark square eighth
			// to dark square ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 2) ScaleTo (false, "seventh", "ninth");							// scale to ninth
			// to light square ninth
		else if (f == 8 && t == 9 && !fl && tl && s == 2) ScaleTo (false, "seventh", "ninth");

	///// ninth \\\\\
		// no tenth state in particle

	///// player tenth \\\\\

		// from zero/half zero
		if (f == 0 && t == 10) ScaleTo (true, "zero", "hidden");													// scale to hidden
		// from first
		else if (f == 1 && t == 10) ScaleTo (true, "first", "hidden");												// scale to hidden
		// from second
		else if (f == 2 && t == 10) ScaleTo (true, "first", "hidden");												// scale to hidden
		// from third
		else if (f == 3 && t == 10 && fl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// from fourth
		else if (f == 4 && t == 10 && fl) ScaleTo (true, "third", "hidden");										// scale to hidden
		// from fifth
		else if (f == 5 && t == 10) ScaleTo (true, "first", "hidden");												// scale to hidden
		// from sixth
		else if (f == 6 && t == 10) ScaleTo (true, "first", "hidden");												// scale to hidden
		// from seventh
		else if (f == 7 && t == 10) ScaleTo (true, "seventh", "hidden");											// scale to hidden
		// from eighth
		else if (f == 8 && t == 10) ScaleTo (true, "seventh", "hidden");											// scale to hidden
		// from ninth
		else if (f == 9 && t == 10) ScaleTo (true, "ninth", "hidden");												// scale to hidden


// DEVOLUTIONS \\


	///// zero \\\\\

	///// half zero (0.5) \\\\\

		// to zero (no core change)

	///// first \\\\\

		// from dark first

		// to zero
		if (f == 1 && t == 0 && !fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		// to dark zero (0.5)
		if (f == 1 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");										// scale to zero

		// from light first

		// to zero
		if (f == 1 && t == 0 && fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		// to dark zero (0.5)
		if (f == 1 && t == 0 && fl && !tl) ScaleTo (true, "first", "zero");											// scale to zero

	///// second \\\\\

		// from dark second

		// to zero
		if (f == 2 && t == 0 && !fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		// to dark zero
		if (f == 2 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");										// scale to zero
		// to first
			// to dark first (no core change)
			// to light first (no core change)

		// from light second

		// to zero
		if (f == 2 && t == 0 && fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		// to dark zero
		if (f == 2 && t == 0 && fl && !tl) ScaleTo (true, "first", "zero");											// scale to zero
		// to first
			// to dark first (no core change)
			// to light first (no core change)

	///// third \\\\\

		// from dark third	
	
		// to zero
		if (f == 3 && t == 0 && !fl && tl) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to dark zero
		if (f == 3 && t == 0 && !fl && !tl) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to first
			// to dark first
		if (f == 3 && t == 1 && !fl && !tl) ScaleTo (false, "hidden", "first");										// scale to first
			// to light first
		else if (f == 3 && t == 1 && !fl && tl) ScaleTo (false, "hidden", "first");									// scale to first
		// to second
			// to dark second
		if (f == 3 && t == 2 && !fl && !tl) ScaleTo (false, "hidden", "first");										// scale to first
			// to light second
		else if (f == 3 && t == 2 && !fl && tl) ScaleTo (false, "hidden", "first");									// scale to first

		// from light third	

		// to zero
		if (f == 3 && t == 0 && fl && tl) ScaleTo (true, "third", "zero");											// scale to zero
		// to dark zero
		if (f == 3 && t == 0 && fl && !tl) ScaleTo (true, "third", "zero");											// scale to zero
		// to first
			// to dark first
		if (f == 3 && t == 1 && fl && !tl) ScaleTo (true, "third", "first");										// scale to first
			// to light first
		else if (f == 3 && t == 1 && !fl && tl) ScaleTo (true, "third", "first");									// scale to first
		// to second
			// to dark second
		if (f == 3 && t == 2 && fl && !tl) ScaleTo (true, "third", "first");										// scale to first
			// to light second
		else if (f == 3 && t == 2 && !fl && tl) ScaleTo (true, "third", "first");									// scale to first

	///// fourth \\\\\

		// from dark fourth

		// to zero
		if (f == 4 && t == 0 && !fl && tl) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to dark zero
		if (f == 4 && t == 0 && !fl && !tl) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to first
			// to dark first
		if (f == 4 && t == 1 && !fl && !tl) ScaleTo (false, "hidden", "first");										// scale to first
			// to light first
		else if (f == 4 && t == 1 && !fl && tl) ScaleTo (false, "hidden", "first");									// scale to first
		// to second
			// to dark second
		if (f == 4 && t == 2 && !fl && !tl) ScaleTo (false, "hidden", "first");										// scale to first
			// to light second
		else if (f == 4 && t == 2 && !fl && tl) ScaleTo (false, "hidden", "first");									// scale to first
		// to third
			// to dark third (no core change)
			// to light third
		if (f == 4 && t == 3 && !fl && tl) ScaleTo (false, "hidden", "third");										// scale to third

		// from light fourth	

		// to zero
		if (f == 4 && t == 0 && fl && tl) ScaleTo (true, "third", "zero");											// scale to zero
		// to dark zero
		if (f == 4 && t == 0 && fl && !tl) ScaleTo (true, "third", "zero");											// scale to zero
		// to first
			// to dark first
		if (f == 4 && t == 1 && fl && !tl) ScaleTo (true, "third", "zero");											// scale to zero
			// to light first
		else if (f == 4 && t == 1 && fl && tl) ScaleTo (true, "third", "first");									// scale to first
		// to second
			// to dark second
		if (f == 4 && t == 2 && fl && !tl) ScaleTo (true, "third", "first");										// scale to first
			// to light second
		else if (f == 4 && t == 2 && fl && tl) ScaleTo (true, "third", "first");									// scale to first
		// to third
			// to dark third
		if (f == 4 && t == 3 && fl && !tl) ScaleTo (true, "third", "hidden");										// scale to hidden
			// to light third (no core change)

	///// fifth \\\\\

		// from dark circle fifth
	
		// to zero
		if (f == 5 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to dark zero
		if (f == 5 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to first
			// to dark first (no core change)
			// to light first (no core change)
		// to second
			// to dark second (no core change)
			// to light second (no core change)
		// to third
			// to dark third
		if (f == 5 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "first", "hidden");							// scale to hidden
			// to light third
		else if (f == 5 && t == 3 && !fl && tl && s == 0) ScaleTo (false, "false", "third");						// scale to third
		// to fourth
			// to dark fourth
		if (f == 5 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "first", "hidden");							// scale to hidden

		// from light circle fifth

		// to zero
		if (f == 5 && t == 0 && fl && tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to dark zero
		if (f == 5 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to first
			// to dark first (no core change)
			// to light first (no core change)
		// to second
			// to dark second (no core change)
			// to light second (no core change)
		// to third
			// to dark third
		if (f == 5 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "first", "hidden");								// scale to hidden
			// to light third
		else if (f == 5 && t == 3 && fl && tl && s == 0) ScaleTo (false, "first", "third");							// scale to zero

		// to fourth
			// to dark fourth
		if (f == 5 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "first", "hidden");								// scale to hidden

	// from triangle fifth

		// to zero
		if (f == 5 && t == 0 && fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 5 && t == 0 && fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 5 && t == 1 && fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 5 && t == 1 && fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 5 && t == 2 && fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 5 && t == 2 && fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 5 && t == 3 && fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 5 && t == 3 && fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 5 && t == 4 && fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}

	// from square fifth

		// to zero
		if (f == 5 && t == 0 && fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 5 && t == 0 && fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 5 && t == 1 && fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 5 && t == 1 && fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 5 && t == 2 && fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 5 && t == 2 && fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 5 && t == 3 && fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 5 && t == 3 && fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 5 && t == 4 && fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}

	///// sixth \\\\\

		// from dark circle sixth
	
		// to zero
		if (f == 6 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to dark zero
		if (f == 6 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to first
			// to dark first (no core change)
			// to light first (no core change)
		// to second
			// to dark second (no core change)
			// to light second (no core change)
		// to third
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "first", "hidden");							// scale to hidden
			// to light third
		else if (f == 6 && t == 3 && !fl && tl && s == 0) ScaleTo (false, "first", "third");						// scale to third
		// to fourth
			// to dark fourth
		if (f == 6 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "first", "hidden");							// scale to hidden
		// to fifth
			// to dark circle fifth (no core change)
			// to light circle fifth (no core change)

		// from light circle sixth

		// to zero
		if (f == 6 && t == 0 && fl && tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to dark zero
		if (f == 6 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "first", "zero");								// scale to zero
		// to first
			// to dark first (no core change)
			// to light first (no core change)
		// to second
			// to dark second (no core change)
			// to light second (no core change)
		// to third
			// to dark third
		if (f == 6 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "first", "hidden");								// scale to hidden
			// to light third
		else if (f == 6 && t == 3 && fl && tl && s == 0) ScaleTo (false, "first", "third");							// scale to third
		// to fourth
			// to dark fourth
		if (f == 6 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "first", "hidden");								// scale to hidden
		// to fifth
			// to dark circle fifth (no core change)
			// to light circle fifth (no core change)

	// from triangle sixth

		// to zero
		if (f == 6 && t == 0 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 6 && t == 0 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 6 && t == 1 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 6 && t == 1 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 6 && t == 2 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 6 && t == 2 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 6 && t == 3 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 6 && t == 4 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
		// to fifth
			// to triangle fifth (no core change)

	// from square sixth

		// to zero
		if (f == 6 && t == 0 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 6 && t == 0 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 6 && t == 1 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 6 && t == 1 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 6 && t == 2 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 6 && t == 2 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 6 && t == 3 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 6 && t == 4 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
		// to fifth
			// to square fifth (no core change)

	///// seventh/eighth \\\\\

		// from dark circle seventh/eighth

		// to zero
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "zero");								// scale to zero
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "zero");							// scale to zero
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");						// scale to third
		// to fourth
			// to dark fourth
		if ((f == 7 || f == 8) && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to fifth
			// to dark circle fifth
		if ((f == 7 || f == 8) && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light circle fifth
		else if ((f == 7 || f == 8) && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first
		// to sixth
			// to dark circle sixth
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light circle sixth
		else if ((f == 7 || f == 8) && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first

		// from light circle seventh/eighth

		// to zero
		if ((f == 7 || f == 8) && t == 0 && fl && tl && s == 0) ScaleTo (true, "seventh", "zero");								// scale to zero
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && fl && !tl && s == 0) ScaleTo (true, "seventh", "zero");								// scale to zero
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && fl && tl && s == 0) ScaleTo (true, "seventh", "third");						// scale to hidden
		// to fourth
			// to dark fourth
		if ((f == 7 || f == 8) && t == 4 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		// to fifth
			// to dark circle fifth
		if ((f == 7 || f == 8) && t == 5 && fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light circle fifth
		else if ((f == 7 || f == 8) && t == 5 && fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first
		// to sixth
			// to dark circle sixth
		if ((f == 7 || f == 8) && t == 6 && fl && !tl && s == 0) ScaleTo (true, "seventh", "first");							// scale to first
			// to light circle sixth
		else if ((f == 7 || f == 8) && t == 6 && fl && tl && s == 0) ScaleTo (true, "seventh", "first");						// scale to first

	// from dark triangle seventh/eighth

		// to zero
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if ((f == 7 || f == 8) && t == 4 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fifth
			// to triangle fifth
		if ((f == 7 || f == 8) && t == 5 && !fl && tl && s == 1) ScaleTo (true, "seventh", "first");							// scale to first
		// to sixth
			// to dark triangle sixth
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "first");							// scale to first

	// from light triangle seventh/eighth

		// to zero
		if ((f == 7 || f == 8) && t == 0 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if ((f == 7 || f == 8) && t == 4 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fifth
			// to triangle fifth
		if ((f == 7 || f == 8) && t == 5 && fl && tl && s == 1) ScaleTo (true, "seventh", "first");								// scale to first
		// to sixth
			// to dark triangle sixth
		if ((f == 7 || f == 8) && t == 6 && fl && !tl && s == 1) ScaleTo (true, "seventh", "first");							// scale to first

	// from dark square seventh/eighth

		// to zero
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second	
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if ((f == 7 || f == 8) && t == 4 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fifth
			// to square fifth
		if ((f == 7 || f == 8) && t == 5 && !fl && tl && s == 2) ScaleTo (true, "seventh", "first");							// scale to first
		// to sixth
			// to dark square sixth
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "first");							// scale to first

	// from light square seventh/eighth

		// to zero
		if ((f == 7 || f == 8) && t == 0 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if ((f == 7 || f == 8) && t == 0 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if ((f == 7 || f == 8) && t == 1 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if ((f == 7 || f == 8) && t == 1 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if ((f == 7 || f == 8) && t == 2 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if ((f == 7 || f == 8) && t == 2 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if ((f == 7 || f == 8) && t == 3 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if ((f == 7 || f == 8) && t == 3 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if ((f == 7 || f == 8) && t == 4 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
		// to fifth
			// to square fifth
		if ((f == 7 || f == 8) && t == 5 && fl && tl && s == 0) ScaleTo (true, "seventh", "first");					// scale to first
		// to sixth
			// to dark square sixth
		if ((f == 7 || f == 8) && t == 6 && fl && !tl && s == 0) ScaleTo (true, "seventh", "first");				// scale to first

///// ninth \\\\\

	// from dark circle ninth

		// to zero
		if (f == 9 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light first
		else if (f == 9 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to third
			// to dark third
		if (f == 9 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");							// scale to hidden
			// to light third
		else if (f == 9 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");							// scale to third
		// to fourth
			// to dark fourth
		if (f == 9 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");							// scale to hidden
		// to fifth
			// to dark circle fifth
		if (f == 9 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light circle fifth
		else if (f == 9 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to sixth
			// to dark circle sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light circle sixth
		else if (f == 9 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to seventh
			// to dark circle seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light circle seventh
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		// to eighth
			// to dark circle eighth
		if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light circle eighth
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh

	// from light circle ninth

		// to zero
		if (f == 9 && t == 0 && fl && tl && s == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		// to first
			// to dark first
		if (f == 9 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light first
		else if (f == 9 && t == 1 && fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to second
			// to dark second
		if (f == 9 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light second
		else if (f == 9 && t == 2 && fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to third
			// to dark third
		if (f == 9 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");								// scale to hidden
		// to light third
		else if (f == 9 && t == 3 && fl && tl && s == 0) ScaleTo (true, "ninth", "third");							// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 9 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");								// scale to hidden
		// to fifth
			// to dark circle fifth
		if (f == 9 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light circle fifth
		else if (f == 9 && t == 5 && fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to sixth
			// to dark circle sixth
		if (f == 9 && t == 6 && fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
			// to light circle sixth
		else if (f == 9 && t == 6 && fl && tl && s == 0) ScaleTo (true, "ninth", "first");							// scale to first
		// to seventh
			// to dark circle seventh
		if (f == 9 && t == 7 && fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light circle seventh
		else if (f == 9 && t == 7 && fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		// to eighth
			// to dark circle eighth
		if (f == 9 && t == 7 && fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light circle eighth
		else if (f == 9 && t == 7 && fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh

	// from dark triangle ninth

		// to zero
		if (f == 9 && t == 0 && !fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 9 && t == 1 && !fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 9 && t == 3 && !fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 9 && t == 3 && !fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && !fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
		// to fifth
			// to triangle fifth
		if (f == 9 && t == 5 && !fl && tl && s == 1) ScaleTo (true, "ninth", "first");								// scale to first
		// to sixth
			// to dark triangle sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "first");								// scale to first
		// to seventh
			// to dark triangle seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light triangle seventh
		else if (f == 9 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		// to eighth
			// to dark triangle eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light triangle eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh

	// from light triangle ninth

		// to zero
		if (f == 9 && t == 0 && fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 9 && t == 1 && fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 9 && t == 1 && fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 9 && t == 2 && fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 9 && t == 2 && fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 9 && t == 3 && fl && !tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 9 && t == 3 && fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && fl && tl && s == 1) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fifth
			// to triangle fifth
		if (f == 9 && t == 5 && fl && tl && s == 1) ScaleTo (true, "ninth", "first");								// scale to first
		// to sixth
			// to dark triangle sixth
		if (f == 9 && t == 6 && fl && !tl && s == 1) ScaleTo (true, "ninth", "first");								// scale to first
		// to seventh
			// to dark triangle seventh
		if (f == 9 && t == 7 && fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light triangle seventh
		else if (f == 9 && t == 7 && fl && tl && s == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		// to eighth
			// to dark triangle eighth
		if (f == 9 && t == 8 && fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light triangle eighth
		else if (f == 9 && t == 8 && fl && tl && s == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh

	// from dark square ninth

		// to zero
		if (f == 9 && t == 0 && !fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 9 && t == 1 && !fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 9 && t == 3 && !fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 9 && t == 3 && !fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && !fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
		// to fifth
			// to square fifth
		if (f == 9 && t == 5 && !fl && tl && s == 2) ScaleTo (true, "ninth", "first");								// scale to first
		// to sixth
			// to dark square sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "first");								// scale to first
		// to seventh
			// to dark square seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light square seventh
		else if (f == 9 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		// to eighth
			// to dark square eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light square eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh

	// from light square ninth

		// to zero
		if (f == 9 && t == 0 && fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 9 && t == 1 && fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light first
		else if (f == 9 && t == 1 && fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to second
			// to dark second
		if (f == 9 && t == 2 && fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light second
		else if (f == 9 && t == 2 && fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 9 && t == 3 && fl && !tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
			// to light third
		else if (f == 9 && t == 3 && fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && fl && tl && s == 2) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			shape = 0;																								// change to circle
			changeShape = true;																						// set change shape flag
		}
		// to fifth
			// to square fifth
		if (f == 9 && t == 5 && fl && tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
		// to sixth
			// to dark square sixth
		if (f == 9 && t == 6 && fl && !tl && s == 0) ScaleTo (true, "ninth", "first");								// scale to first
		// to seventh
			// to dark square seventh
		if (f == 9 && t == 7 && fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light square seventh
		else if (f == 9 && t == 7 && fl && tl && s == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		// to eighth
			// to dark square eighth
		if (f == 9 && t == 8 && fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light square eighth
		else if (f == 9 && t == 8 && fl && tl && s == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
	}
		
	///<summary>
	///<para>0 = circle</para>
	///<para>1 = triangle</para>
	///<para>2 = square</para>
	///</summary>
	private void SetShape(int s)
	{
		if (s == 0) GetComponent<MeshFilter>().mesh = sphere;									// change mesh to sphere
		else if (s == 1) GetComponent<MeshFilter>().mesh = triangle;							// change mesh to triangle
		else if (s == 2) GetComponent<MeshFilter>().mesh = square;								// change mesh to square
	}

	///<summary>
	///<para>set core as light</para>
	///<para>true = white</para>
	///<para>false = black</para>
	///</summary>
	private void SetLight (bool l, bool shade)
	{
        if (l)
        {
            //rend.material.SetColor("_Color", Color.white);				// change to white
            //anim.SetTrigger("colour");									// set colour change trigger
            anim.SetBool("black", false);								// reset previously active state
            anim.SetBool("white", true);								// set active state
        }
		else if (!l)
        {
            //rend.material.SetColor("_Color", Color.black);				// change to black
            //anim.SetTrigger("colour");									// set colour change trigger
            anim.SetBool("white", false);								// reset previously active state
            anim.SetBool("black", true);								// set active state
        }
        //anim.ResetTrigger("colour");										// reset colour change trigger

        //if (shade && light) rend.material.shader = lightShader;			// change to white shader
        //else if (shade && !light) rend.material.shader = darkShader;    // change to white shader

		resetScale = true;												// trigger rescale timer

    }

	///<summary>
	///<para> devol = whether to scale up or scale down </para>
	///<para> resetState = state to set to false </para>
	///<para> setState = state to set to true </para>
	///</summary>
	private void ScaleTo (bool devol, string resetState, string setState)
	{
		//Debug.Log("devol: " + devol);
        if (devol) {
			anim.ResetTrigger ("scaleup");								// reset last stage
			anim.SetTrigger ("scaledown");								// enable scaledown
		}
		else if (!devol) {
			anim.ResetTrigger ("scaledown");							// reset last stage
			anim.SetTrigger ("scaleup");								// enable scaleup

		}
		anim.SetBool(resetState, false);								// reset previously active state
		anim.SetBool(setState, true);									// set active state
	}
}
