using UnityEngine;
using System.Collections;

public class ParticleNucleusManager : MonoBehaviour {

	private Animator anim;																										// animator on core ref
	public Mesh sphere, triangle, square;																						// shape meshes
	private MeshRenderer rend;																									// mesh renderer (for colour changes)
	private ParticleStatePattern psp;																							// psp ref

	private int toState, shape;																									// to state indicator
	private bool toLight, colour; 																								// to light, colour indicator
	private bool changeColour = false, changeShape = false, resetScale = false;													// timer trigger for resetting scale after world switch
	private float changeColourTimer, changeShapeTimer, resetScaleTimer;															// reset scale timer

	private Shader lightShader, darkShader;																						// light/dark shaders

	void Start () {
		anim = GetComponent<Animator>();																						// init animator ref
		rend = GetComponent<MeshRenderer>();																					// init mesh renderer ref
		psp = GetComponentInParent<ParticleStatePattern> ();																	// init psp ref
		lightShader = Shader.Find("Unlit/light_nucleus");																		// init light nucleus shader
		darkShader = Shader.Find("Unlit/light_nucleus");																		// init light nucleus shader
	}

	void Update() {

		if (psp.changeParticles && psp.lightworld && !psp.isLight) {															// if change particles and light world
			//Debug.Log ("change particle nucleus");
			toState = psp.state;																									// set toState to current state
			ToOtherWorld (true, toState, toState, false);																			// to hidden, change to white
			resetScale = true;																										// set reset scale flag
			//psp.psp.changeParticles = false;																						// reset change particles in playerstatepattern
		} 
		else if (psp.changeParticles && !psp.lightworld && !psp.isLight) {																		// if change particles and dark world
			toState = psp.state;																									// set toState to current state
			ToOtherWorld (false, toState, toState, true);																			// to hidden, change to black
			resetScale = true;																										// set reset scale flag
			//psp.psp.changeParticles = false;																						// reset change particles in playerstatepattern
		}

		// change colour timer
		if (changeColour) changeColourTimer += Time.deltaTime;																	// start timer
		if (changeColourTimer >= 2.0f) {																						// when timer >= 4 sec
			//Debug.Log("set colour: " + colour);
			SetLight(colour);																										// set colour
			changeColour = false;																									// reset reset scale flag
			changeColourTimer = 0f;																									// reset timer
		}
		// change shape timer
		if (changeShape) changeShapeTimer += Time.deltaTime;																	// start timer
		if (changeShapeTimer >= 2.0f) {																							// when timer >= 4 sec
			Debug.Log("particle nucleus set shape: " + shape);
			SetShape(shape);																										// set shape
			changeShape = false;																									// reset reset scale flag
			changeShapeTimer = 0f;																									// reset timer
		}
		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;																		// start timer
		if (resetScaleTimer >= 4.0f) {																							// when timer >= 4 sec
			//anim.ResetTrigger("colour");	
			if (toState == 0) ScaleTo (false, "hidden", "zero");																	// if to zero, grow to zero
			if (!toLight && (toState == 1 || toState == 2 ||toState == 4 ||  toState == 5 || toState == 6)) 						// if to dark first/second/fourth/fifth/sixth
				ScaleTo (false, "hidden", "first");																						// grow to first
			if (shape == 0 && toLight && (toState == 2 || toState == 4 || toState == 6)) 											// if to light circle second/fourth/sixth
				ScaleTo (false, "hidden", "zero");																						// grow to zero
			if (!toLight && (toState == 7 || toState == 8)) ScaleTo (false, "hidden", "seventh");									// if to dark seventh/eighth, grow to seventh
			if (shape == 0 && toLight && toState == 8) ScaleTo (false, "hidden", "first");											// if to light circle eighth, grow to first
			if (toState == 9) ScaleTo (false, "hidden", "ninth");																	// if to ninth, grow to ninth
			resetScale = false;																										// reset reset scale flag
			resetScaleTimer = 0f;																									// reset timer
		}
	}

	public void ToOtherWorld (bool lw, int f, int t, bool l) 
	{
		toState = t;																											// set to state
		if (lw) {																												// to light world
			// from changes
			if (f == 0) ScaleTo (true, "zero", "hidden");																			// scale from zero to hidden
			else if (f == 1 || f == 2 || f == 4 || f == 5 || f == 6) ScaleTo (true, "first", "hidden");								// scale from second/first to hidden
			else if (f == 7 || f == 8) ScaleTo (true, "seventh", "hidden");															// scale from seventh to hidden
			else if (f == 9) ScaleTo (true, "ninth", "hidden");																		// scale from ninth to hidden

			// to changes
			if (t == 0) SetLight (false, true);																						// if to zero, change to white
			else if (t == 1 || t == 5) SetLight (false, true);																		// if to first/fifth, change to white
			else if (t == 2 || t == 4 || t == 6) SetLight (true, true);																// if to second/fourth/sixth, change to black dot shader
			else if (t == 3) SetLight (false, true);																				// to third, change to white
			else if (t == 7) SetLight (false, true);																				// if to seventh, change to white
			else if (t == 8) SetLight (true, true);																					// if to eighth, change to white + black shader
			else if (t == 9) SetLight (false, true);																				// if to ninth, change to white
		}
		else if (!lw) {																												// to dark world
			Debug.Log("particle nucleus to dark world");
			// from changes
			if (f == 0) ScaleTo (true, "zero", "hidden");																			// scale from zero to hidden
			else if (f == 1 || f == 2 || f == 4 || f == 5 || f == 6) ScaleTo (true, "first", "hidden");								// scale from second/first to hidden
			else if (f == 7 || f == 8) ScaleTo (true, "seventh", "hidden");															// scale from seventh to hidden
			else if (f == 9) ScaleTo (true, "ninth", "hidden");

			// to changes
			if (t == 0) SetLight (false, false);																					// if to zero, change to black
			else if (t == 1 || t == 5) SetLight (false, false);																		// if to first/fifth, change to black
			else if (t == 2 || t == 4 || t == 6) SetLight (true, false);															// if to second/fourth/sixth, change to white dot shader
			else if (t == 3) SetLight (false, false);																				// to third, change to black
			else if (t == 7) SetLight (false, false);																				// if to seventh, change to black
			else if (t == 8) SetLight (true, false);																				// if to eighth, change to black + white shader
			else if (t == 9) SetLight (false, false);																				// if to ninth, change to black

			/*if (f == 0) {																												// from light world zero		
				ScaleTo (true, "zero", "hidden");																						// scale from zero
				colour = false;																											// change to black
				changeColour = true;																									// set change colour flag
				resetScale = true;																										// set reset scale flag
			}
			else if (f == 1) {																												// from light world zero		
				ScaleTo (true, "first", "hidden");																						// scale from zero
				colour = false;																											// change to black
				changeColour = true;																									// set change colour flag
				resetScale = true;																										// set reset scale flag
			}*/
		}
	}

	public void Nucleus (int f, int t, bool fl, bool tl, int s) 
	{
		// set up
		toState = t;																											// set to state
		toLight = tl;																											// set to light
		shape = s;																												// set s


		///////////////////// EVOLUTIONS \\\\\\\\\\\\\\\\\\\\\\


///// zero \\\\\


		// to dark zero
		if (f == 0 && t == 0 && !fl && !tl) {
			ScaleTo (false, "hidden", "zero");													// scale to zero
			Debug.Log(transform.parent.name + " nucleus init to dark zero - ParticleNucleusManager");
		}

		// to dark first (init)
		if (f == 0 && t == 1 && !fl && !tl) ScaleTo (false, "hidden", "first");													// scale to first


///// half zero \\\\\


	// from dark zero
		// to dark first 
		if (f == 0 && t == 1 && !fl && !tl) ScaleTo (false, "zero", "first");													// scale to first
		// to light first (no nucleus change)

	// from light zero (0.5)
		// to dark first
		if (f == 0 && t == 1 && fl && !tl) ScaleTo (false, "hidden", "first");													// scale to first
		// to light first (no nucleus change)


///// first \\\\\


	// from dark first
		// to dark second
		if (f == 1 && t == 2 && !fl && !tl) {
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
		}
		// to light second
		else if (f == 1 && t == 2 && !fl && tl) ScaleTo (false, "hidden", "zero");												// scale to zero

	// from light first
		// to dark second
		if (f == 1 && t == 2 && fl && !tl) {			
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		// to light second
		else if (f == 1 && t == 2 && fl && tl) ScaleTo (false, "hidden", "zero");												// scale to first


///// second \\\\\


	// from dark second
		// to dark third
		if (f == 2 && t == 3 && !fl && !tl) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		} 
		// to light third
		else if (f == 2 && t == 3 && !fl && tl) {		 
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}

	// from light second
		// to dark third
		else if (f == 2 && t == 3 && fl && !tl) ScaleTo (true, "zero", "hidden");												// scale to hidden
		// to light third
		else if (f == 2 && t == 3 && fl && tl) ScaleTo (true, "zero", "hidden");												// scale to hidden


///// third \\\\\


	// from dark third
		// to dark fourth
		if (f == 3 && t == 4 && !fl && !tl) {							
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		// to light fourth
		else if (f == 3 && t == 4 && !fl && tl) ScaleTo (false, "hidden", "zero");												// scale to zero

	// from light third
		// to dark fourth
		if (f == 3 && t == 4 && fl && !tl) {							
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		// to light fourth
		else if (f == 3 && t == 4 && fl && tl) ScaleTo (false, "hidden", "zero");												// scale to first


///// fourth \\\\\


	// from dark fourth
		// to dark circle fifth
		if (f == 4 && t == 5 && !fl && !tl && s == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to light circle fifth
		else if (f == 4 && t == 5 && !fl && tl && s == 0) {		
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}

		// from light fourth
		// to triangle fifth
		if (f == 4 && t == 5 && fl && tl && s == 1) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 1;																											// change to triangle
			changeShape = true;																									// set change shape flag
		}
		// to square fifth
		else if (f == 4 && t == 5 && fl && tl && s == 2) {		
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 2;																											// change to square
			changeShape = true;																									// set change shape flag
		}


///// fifth \\\\\


	// from dark circle fifth
		// to dark circle sixth
		if (f == 5 && t == 6 && !fl && !tl && s == 0) {
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
		}
		// to light circle sixth
		else if (f == 5 && t == 6 && !fl && tl && s == 0) ScaleTo (false, "first", "zero");										// scale to first

	// from light circle fifth
		// to dark circle sixth
		if (f == 5 && t == 6 && fl && !tl && s == 0) {			
			SetLight(true);																										// change to white
			resetScale = true;																									// set reset scale flag
		}
		// to light circle sixth
		else if (f == 5 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "zero");										// scale to zero

	// from triangle fifth
		// to dark triangle sixth
		if (f == 5 && t == 6 && fl && s == 1) ScaleTo (false, "hidden", "first");												// scale to first

	// from square fifth
		// to dark square sixth
		if (f == 5 && t == 6 && fl && s == 2) ScaleTo (false, "hidden", "first");												// scale to first


//// sixth \\\\\


	// from dark circle sixth
		// to dark circle seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to light circle seventh
		else if (f == 6 && t == 7 && !fl && tl && s == 0) {		
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}

	// from light circle sixth
		// to dark circle seventh
		if (f == 6 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "zero", "seventh");										// scale to seventh
		// to light circle seventh
		else if (f == 6 && t == 7 && fl && tl && s == 0) ScaleTo (true, "zero", "hidden");										// scale to hidden

	// from dark triangle sixth
		// to dark triangle seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 1) ScaleTo (false, "first", "hidden");										// scale to hidden
		// to light triangle seventh
		else if (f == 6 && t == 7 && !fl && tl && s == 1) ScaleTo (false, "first", "hidden");									// scale to hidden

	// from dark square sixth
		// to dark square seventh
		if (f == 6 && t == 7 && !fl && !tl && s == 2) ScaleTo (false, "first", "hidden");										// scale to hidden
		// to light square seventh
		else if (f == 6 && t == 7 && !fl && tl && s == 2) ScaleTo (false, "first", "hidden");									// scale to hidden


///// seventh \\\\\


	// from dark circle seventh
		// to dark circle eighth
		if (f == 7 && t == 8 && !fl && !tl && s == 0) {
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
		}
		// to light circle eighth
		else if (f == 7 && t == 8 && !fl && tl && s == 0) ScaleTo (true, "seventh", "first");									// scale to first

	// from light circle seventh
		// to dark circle eighth
		if (f == 7 && t == 8 && fl && !tl && s == 0) {			
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		// to light circle eighth
		else if (f == 7 && t == 8 && fl && tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first

	// from dark triangle seventh
		// to dark triangle eighth
		if (f == 7 && t == 8 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		// to light triangle eighth
		else if (f == 7 && t == 8 && !fl && tl && s == 1) ScaleTo (false, "hidden", "seventh");									// scale to seventh

	// from light triangle seventh
		// to dark triangle eighth
		if (f == 7 && t == 8 && fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		// to light triangle eighth
		else if (f == 7 && t == 8 && fl && tl && s == 1) ScaleTo (false, "hidden", "seventh");									// scale to seventh

	// from dark square seventh
		// to dark square eighth
		if (f == 7 && t == 8 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		// to light square eighth
		else if (f == 7 && t == 8 && !fl && tl && s == 2) ScaleTo (false, "hidden", "seventh");									// scale to seventh

	// from light square seventh
		// to dark square eighth
		if (f == 7 && t == 8 && fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		// to light square eighth
		else if (f == 7 && t == 8 && fl && tl && s == 2) ScaleTo (false, "hidden", "seventh");									// scale to seventh


//// eighth \\\\\


	// from dark circle eighth
		// to dark circle ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 0) {			
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to light circle ninth
		else if (f == 6 && t == 7 && !fl && tl && s == 0) {		
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

	// from light circle eighth
		// to dark circle ninth
		if (f == 8 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "first", "ninth");											// scale to ninth
		// to light circle ninth
		else if (f == 8 && t == 9 && fl && tl && s == 0) ScaleTo (true, "first", "hidden");										// scale to hidden

	// from dark triangle eighth
		// to dark triangle ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to light triangle ninth
		else if (f == 8 && t == 9 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden

	// from light triangle eighth
		// to dark triangle ninth
		if (f == 8 && t == 9 && fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to light triangle ninth
		else if (f == 8 && t == 9 && fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden

	// from dark square eighth
		// to dark square ninth
		if (f == 8 && t == 9 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to light square ninth
		else if (f == 8 && t == 9 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden

	// from light square eighth
		// to dark square ninth
		if (f == 8 && t == 9 && fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to light square ninth
		else if (f == 8 && t == 9 && fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden


///// ninth \\\\\
		// no tenth state in particle

///// player tenth \\\\\

		// from zero/half zero
		if (f == 0 && t == 10 && !fl) ScaleTo (true, "zero", "hidden");															// scale to hidden
		// from first
		else if (f == 1 && t == 10 && !fl) ScaleTo (true, "first", "hidden");													// scale to hidden
		// from second
		else if (f == 2 && t == 10 && !fl) ScaleTo (true, "first", "hidden");													// scale to hidden
		// from fourth
		else if (f == 4 && t == 10 && !fl) ScaleTo (true, "first", "hidden");													// scale to hidden
		else if (f == 4 && t == 10 && fl) ScaleTo (true, "zero", "hidden");														// scale to hidden
		// from fifth
		else if (f == 5 && t == 10 && !fl && s == 0) ScaleTo (true, "first", "hidden");											// scale to hidden
		// from sixth
		else if (f == 6 && t == 10 && !fl && s == 0) ScaleTo (true, "first", "hidden");											// scale to hidden
		else if (f == 6 && t == 10 && fl && s == 0) ScaleTo (true, "zero", "hidden");											// scale to hidden
		else if (f == 6 && t == 10 && s != 0) ScaleTo (true, "first", "hidden");												// scale to hidden
		// from seventh
		else if (f == 7 && t == 10 && !fl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// from eighth
		else if (f == 8 && t == 10 && !fl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 10 && fl && s == 0) ScaleTo (true, "first", "hidden");											// scale to hidden
		else if (f == 8 && t == 10 && s != 0) ScaleTo (true, "seventh", "hidden");												// scale to hidden
		// from ninth
		else if (f == 9 && t == 10 && !fl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
	

		////////////////////// DEVOLUTIONS \\\\\\\\\\\\\\\\\\\\\\\



///// zero \\\\\


///// dark zero (0.5) \\\\\


		// to zero
		if (f == 0 && t == 0 && !fl && tl) ScaleTo (true, "zero", "hidden");													// scale to hidden


///// first \\\\\


	// from dark first
		// to zero
		if (f == 1 && t == 0 && !fl && tl) ScaleTo (true, "first", "hidden");													// scale to hidden
		// to dark zero
		if (f == 1 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");													// scale to zero

	// from light first
		// to zero (no nucleus change)
		// to dark zero
		if (f == 1 && t == 0 && fl && !tl) ScaleTo (true, "first", "zero");														// scale to zero


///// second \\\\\


	// from dark second
		// to zero
		if (f == 2 && t == 0 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}
		// to dark zero
		else if (f == 2 && t == 0 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 2 && t == 1 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 2 && t == 1 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}

	// from light second
		// to zero
		if (f == 2 && t == 0 && fl && tl) ScaleTo (true, "zero", "hidden");														// scale to hidden
		// to dark zero (no nucleus change)
		// to first
			// to dark first
		if (f == 2 && t == 1 && fl && !tl) ScaleTo (true, "zero", "first");														// scale to first
			// to light first
		else if (f == 2 && t == 1 && fl && tl) ScaleTo (true, "zero", "hidden");												// scale to hidden


///// third \\\\\


	// from dark third	
		// to zero (no nucleus change)
		// to dark zero
		if (f == 3 && t == 0 && !fl && !tl) ScaleTo (false, "hidden", "zero");													// scale to zero
		// to first 
			// to dark first
		if (f == 3 && t == 1 && !fl && !tl) ScaleTo (false, "hidden", "first");													// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 3 && t == 2 && !fl && !tl) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 3 && t == 2 && !fl && tl) ScaleTo (false, "hidden", "zero");												// scale to zero

	// from light third	
		// to zero ((no nucleus change)
		// to dark zero
		if (f == 3 && t == 0 && fl && !tl) ScaleTo (false, "hidden", "zero");													// scale to zero
		// to first
			// to dark first
		if (f == 3 && t == 1 && fl && !tl) ScaleTo (false, "hidden", "first");													// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 3 && t == 2 && fl && !tl) {
			SetLight(true);																										// change to white
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 3 && t == 2 && !fl && tl) ScaleTo (false, "hidden", "zero");												// scale to zero


///// fourth \\\\\


	// from dark fourth	
		// to zero
		if (f == 4 && t == 0 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}
		// to dark zero
		if (f == 4 && t == 0 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 4 && t == 1 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 4 && t == 1 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}
		// to second
			// to dark second (no nucleus change)
			// to light second
		if (f == 4 && t == 2 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 4 && t == 3 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}
			// to light third
		else if (f == 4 && t == 3 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}

	// from light fourth	
		// to zero
		if (f == 4 && t == 0 && fl && tl) ScaleTo (true, "zero", "hidden");														// scale to hidden
		// to dark zero (no nucleus change)
		// to first
			// to dark first
		if (f == 4 && t == 1 && fl && !tl) ScaleTo (false, "zero", "first");													// scale to first
			// to light first
		else if (f == 4 && t == 1 && fl && tl) ScaleTo (true, "zero", "hidden");												// scale to hidden
		// to second
			// to dark second
		if (f == 4 && t == 2 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second (no nucleus change)
		// to third
			// to dark third
		if (f == 4 && t == 3 && fl && !tl) ScaleTo (true, "first", "hidden");													// scale to hidden
			// to light third
		else if (f == 4 && t == 3 && fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden


///// fifth \\\\\


	// from dark circle fifth
		// to zero
		if (f == 5 && t == 0 && !fl && tl) ScaleTo (true, "first", "hidden");													// scale to hidden
		// to dark zero
		if (f == 5 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");													// scale to zero
		// to first
			// to dark first (no nucleus change)
			// to light first
		if (f == 5 && t == 1 && !fl && tl) ScaleTo (true, "first", "hidden");													// scale to hidden
		// to second
			// to dark second
		if (f == 5 && t == 2 && !fl && !tl && s == 0) SetLight(true);															// change to white shader
			// to light second
		else if (f == 5 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "first", "zero");										// scale to zero
		// to third
			// to dark third
		if (f == 5 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light third
		else if (f == 5 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 5 && t == 4 && !fl && !tl && s == 0) SetLight(true);															// change to white shader

	// from light circle fifth
		// to zero (no nucleus change)
		// to dark zero
		if (f == 5 && t == 0 && fl && !tl) ScaleTo (false, "hidden", "zero");													// scale to zero
		// to first
			// to dark first (no nucleus change)
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 5 && t == 2 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader	
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 5 && t == 2 && fl && tl && s == 0) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
			// to dark fourth
		if (f == 5 && t == 4 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white
			resetScale = true;																									// set reset scale flag
		}

	// from triangle fifth
		// to zero 
		// to light zero
		if (f == 5 && t == 0 && fl && tl && s == 1) SetShape(0);																// change to sphere
		// to dark zero
		else if (f == 5 && t == 0 && fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 5 && t == 1 && fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 5 && t == 1 && fl && tl && s == 1) SetShape(0);															// change to sphere
		// to second
			// to dark second
		if (f == 5 && t == 2 && fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 5 && t == 2 && fl && tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 5 && t == 3 && fl && !tl && s == 1) SetShape(0);																// change to sphere
			// to light third
		else if (f == 5 && t == 3 && fl && tl && s == 1) SetShape(0);															// change to sphere
		// to fourth
			// to light fourth
		if (f == 5 && t == 4 && fl && tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}

	// from square fifth
		// to zero
		if (f == 5 && t == 0 && fl && tl && s == 2) SetShape(0);																// change to sphere
		// to dark zero
		else if (f == 5 && t == 0 && fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 5 && t == 1 && fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
			// to light first 
		else if (f == 5 && t == 1 && fl && tl && s == 2) SetShape(0);															// change to sphere
		// to second
			// to dark second
		if (f == 5 && t == 2 && fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 5 && t == 2 && fl && tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 5 && t == 3 && fl && !tl && s == 2) SetShape(0);																// change to sphere
			// to light third
		else if (f == 5 && t == 3 && fl && tl && s == 2) SetShape(0);															// change to sphere
		// to fourth
			// to light fourth
		if (f == 5 && t == 4 && fl && tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}


///// sixth \\\\\


	// from dark circle sixth
		// to zero
		if (f == 6 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}
		// to dark zero
		if (f == 6 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 6 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light first
		else if (f == 6 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}
		// to second
			// to dark second (no nucleus change)
			// to light second
		if (f == 6 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		} 
			// to light third
		else if (f == 6 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}
		// to fourth
			// to dark fourth (no nucleus change)
		// to fifth
			// to dark circle fifth
		if (f == 6 && t == 5 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle fifth
		else if (f == 6 && t == 5 && !fl && tl && s == 0) {										
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// change to black
			changeColour = true;																								// set change colour flag
		}

	// from light circle sixth
		// to zero
		if (f == 6 && t == 0 && fl && tl && s == 0) ScaleTo (true, "zero", "hidden");											// scale to hidden
		// to dark zero (no nucleus change)
		// to first
			// to dark first
		if (f == 6 && t == 1 && fl && !tl && s == 0) ScaleTo (false, "zero", "first");											// scale to first
			// to light first
		else if (f == 6 && t == 1 && fl && tl && s == 0) ScaleTo (true, "zero", "hidden");										// scale to hidden
		// to second
			// to dark second
		if (f == 6 && t == 2 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second (no nucleus change)
		// to third
			// to dark third
		if (f == 6 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "zero", "hidden");											// scale to hidden
			// to light third
		else if (f == 6 && t == 3 && fl && tl && s == 0) ScaleTo (true, "zero", "hidden");										// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 6 && t == 4 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to dark circle fifth
		if (f == 6 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "zero", "first");											// scale to first
			// to light circle fifth
		else if (f == 6 && t == 5 && fl && tl && s == 0) ScaleTo (true, "zero", "hidden");										// scale to hidden

	// from triangle sixth
		// to zero
		if (f == 6 && t == 0 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to dark zero
		if (f == 6 && t == 0 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 6 && t == 1 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 6 && t == 1 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to second
			// to dark second
		if (f == 6 && t == 2 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 6 && t == 2 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
			// to light third
		else if (f == 6 && t == 3 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to fourth
			// to light fourth
		if (f == 6 && t == 4 && !fl && tl && s == 1) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to triangle fifth
		if (f == 6 && t == 5 && !fl && !tl && s == 1) ScaleTo (true, "first", "hidden");										// scale to hidden

	// from square sixth
		// to zero
		if (f == 6 && t == 0 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to dark zero
		if (f == 6 && t == 0 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 6 && t == 1 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 6 && t == 1 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to second
			// to dark second
		if (f == 6 && t == 2 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 6 && t == 2 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 6 && t == 3 && !fl && !tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
			// to light third
		else if (f == 6 && t == 3 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to fourth
			// to light fourth
		if (f == 6 && t == 4 && !fl && tl && s == 2) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to square fifth
		if (f == 6 && t == 5 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}


///// seventh \\\\\


	// from dark circle seventh
		// to zero
		if (f == 7 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to dark zero
		if (f == 7 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "zero");										// scale to zero
		// to first
			// to dark first
		if (f == 7 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");										// scale to first
			// to light first
		else if (f == 7 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		// to second
			// to dark second
		if (f == 7 && t == 2 && !fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 7 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "zero");									// scale to zero
		// to third
			// to dark third
		if (f == 7 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
			// to light third
		else if (f == 7 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 7 && t == 4 && !fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to dark circle fifth
		if (f == 7 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");										// scale to first
			// to light circle fifth
		if (f == 7 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to sixth
			// to dark circle sixth
		if (f == 7 && t == 6 && !fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
			// to light circle sixth
		else if (f == 7 && t == 6 && !fl && tl && s == 0) ScaleTo (false, "seventh", "zero");									// scale to first

	// from light circle seventh
		// to zero (no nucleus change)
		// to dark zero (no nucleus change)
		if (f == 7 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "hidden", "zero");											// scale to zero
		// to first
			// to dark first (no nucleus change)
		if (f == 7 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "hidden", "first");											// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 7 && t == 2 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 7 && t == 2 && fl && tl && s == 0) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
			// to dark fourth
		if (f == 7 && t == 4 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to dark circle fifth
		if (f == 7 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "hidden", "first");											// scale to first
			// to light circle fifth (no nucleus change)
		// to sixth
			// to dark circle sixth
		if (f == 7 && t == 6 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
			// to light circle sixth
		else if (f == 7 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "zero");										// scale to zero

	// from dark triangle seventh
		// to zero
		if (f == 7 && t == 0 && !fl && tl && s == 1) SetShape(0);																// change to sphere
		// to dark zero
		if (f == 7 && t == 0 && !fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 7 && t == 1 && !fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 7 && t == 1 && !fl && tl && s == 1) SetShape(0);															// change to sphere
		// to second
			// to dark second
		if (f == 7 && t == 2 && !fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 7 && t == 2 && !fl && tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 7 && t == 3 && !fl && !tl && s == 1) SetShape(0);																// change to sphere
			// to light third
		else if (f == 7 && t == 3 && !fl && tl && s == 1) SetShape(0);															// change to sphere
		// to fourth
			// to light fourth
		if (f == 7 && t == 4 && !fl && tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to triangle fifth (no nucleus change)
		// to sixth
			// to dark triangle sixth
		if (f == 7 && t == 6 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first

	// from light triangle seventh
		// to zero
		if (f == 7 && t == 0 && fl && tl && s == 1) SetShape(0);																// change to sphere
		// to dark zero
		if (f == 7 && t == 0 && fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 7 && t == 1 && fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 7 && t == 1 && fl && tl && s == 1) SetShape(0);															// change to sphere
		// to second
			// to dark second
		if (f == 7 && t == 2 && fl && !tl && s == 1) {
			SetShape(0);																										// change to sphere
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 7 && t == 2 && fl && tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 7 && t == 3 && fl && !tl && s == 1) SetShape(0);																// change to sphere
			// to light third
		else if (f == 7 && t == 3 && fl && tl && s == 1) SetShape(0);															// change to sphere
		// to fourth
			// to light fourth
		if (f == 7 && t == 4 && fl && tl && s == 1) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to triangle fifth (no nucleus change)
		// to sixth
			// to dark triangle sixth
		if (f == 7 && t == 6 && fl && !tl && s == 1)ScaleTo (false, "hidden", "first");											// scale to first

	// from dark square seventh
		// to zero
		if (f == 7 && t == 0 && !fl && tl && s == 2) SetShape(0);																// change to sphere
		// to dark zero
		if (f == 7 && t == 0 && !fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 7 && t == 1 && !fl && tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 7 && t == 1 && !fl && tl && s == 2) SetShape(0);															// change to sphere
		// to second
			// to dark second
		if (f == 7 && t == 2 && !fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 7 && t == 2 && !fl && tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 7 && t == 3 && !fl && !tl && s == 2) SetShape(0);																// change to sphere
			// to light third
		else if (f == 7 && t == 3 && !fl && tl && s == 2) SetShape(0);															// change to sphere
		// to fourth
			// to light fourth
		if (f == 7 && t == 4 && !fl && tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to square fifth (no nucleus change)
		// to sixth
			// to dark square sixth
		if (f == 7 && t == 6 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first

	// from light square seventh
		// to zero
		if (f == 7 && t == 0 && fl && tl && s == 2) SetShape(0);																// change to sphere
		// to dark zero
		if (f == 7 && t == 0 && fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 7 && t == 1 && fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 7 && t == 1 && fl && tl && s == 2) SetShape(0);															// change to sphere
		// to second
			// to dark second
		if (f == 7 && t == 2 && fl && !tl && s == 2) {
			SetShape(0);																										// change to sphere
			colour = true;																										// change to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		else if (f == 7 && t == 2 && fl && tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 7 && t == 3 && fl && !tl && s == 2) SetShape(0);																// change to sphere
			// to light third
		else if (f == 7 && t == 3 && fl && tl && s == 2) SetShape(0);															// change to sphere
		// to fourth
			// to light fourth
		if (f == 7 && t == 4 && fl && tl && s == 2) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to square fifth (no nucleus change)
		// to sixth
			// to dark square sixth
		if (f == 7 && t == 6 && fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first


///// eighth \\\\\


	// from dark circle eighth
		// to zero
		if (f == 8 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		// to dark zero
		if (f == 8 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 8 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light first
		else if (f == 8 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		// to second
			// to dark second 
		if (f == 8 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");										// scale to first
			// to light second
		if (f == 8 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 8 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		} 
			// to light third
		else if (f == 8 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		// to fourth
			// to dark fourth
		if (f == 8 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");										// scale to first
		// to fifth
			// to dark circle fifth
		if (f == 8 && t == 5 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle fifth
		else if (f == 8 && t == 5 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		// to sixth
			// to dark circle sixth
		if (f == 8 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");										// scale to first
			// to light circle sixth
		else if (f == 8 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to seventh
			// to dark circle seventh
		if (f == 8 && t == 7 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle seventh
		else if (f == 8 && t == 7 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

	// from light circle eighth
		// to zero
		if (f == 8 && t == 0 && fl && tl && s == 0) ScaleTo (true, "first", "hidden");											// scale to hidden
		// to dark zero
		if (f == 8 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "first", "zero");											// scale to zero
		// to first
			// to dark first (no nucleus change)
			// to light first
		if (f == 8 && t == 1 && fl && tl && s == 0) ScaleTo (true, "first", "hidden");										// scale to hidden
		// to second
			// to dark second
		if (f == 8 && t == 2 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																				// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		if (f == 8 && t == 2 && fl && tl && s == 0) ScaleTo (true, "first", "zero");											// scale to zero
		// to third
			// to dark third
		if (f == 8 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to light third
		else if (f == 8 && t == 3 && fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 8 && t == 4 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																				// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to dark circle fifth (no nucleus change)
			// to light circle fifth
		if (f == 8 && t == 5 && fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		// to sixth
			// to dark circle sixth
		if (f == 8 && t == 6 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																											// colour to white shader
			changeColour = true;																									// set change colour flag
			resetScale = true;																										// set reset scale flag
		}
			// to light circle sixth
		else if (f == 8 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "first", "zero");										// scale to zero
		// to seventh
			// to dark circle seventh
		if (f == 8 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "first", "seventh");										// scale to seventh
			// to light circle seventh
		else if (f == 8 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden

	// from dark triangle eighth
		// to zero
		if (f == 8 && t == 0 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to dark zero
		if (f == 8 && t == 0 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 8 && t == 1 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 8 && t == 1 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to second
			// to dark second
		if (f == 8 && t == 2 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 8 && t == 2 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 8 && t == 3 && !fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
			// to light third
		else if (f == 8 && t == 3 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to fourth
			// to light fourth
		if (f == 8 && t == 4 && !fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to triangle fifth
		if (f == 8 && t == 5 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to sixth
			// to triangle sixth
		if (f == 8 && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "first");										// scale to first
		// to seventh
			// to dark triangle seventh
		if (f == 8 && t == 7 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
			// to light triangle seventh
		if (f == 8 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden

	// from light triangle eighth
		// to zero
		if (f == 8 && t == 0 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to dark zero
		if (f == 8 && t == 0 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 8 && t == 1 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 8 && t == 1 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to second
			// to dark second
		if (f == 8 && t == 2 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 8 && t == 2 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 8 && t == 3 && fl && !tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
			// to light third
		else if (f == 8 && t == 3 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to fourth
			// to light fourth
		if (f == 8 && t == 4 && fl && tl && s == 1) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to triangle fifth
		if (f == 8 && t == 5 && fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to sixth
			// to triangle sixth
		if (f == 8 && t == 6 && fl && !tl && s == 1) ScaleTo (true, "seventh", "first");										// scale to first
		// to seventh
			// to dark triangle seventh
		if (f == 8 && t == 7 && fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
			// to light triangle seventh
		if (f == 8 && t == 7 && fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden

	// from dark square eighth
		// to zero
		if (f == 8 && t == 0 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to dark zero
		if (f == 8 && t == 0 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 8 && t == 1 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 8 && t == 1 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to second
			// to dark second
		if (f == 8 && t == 2 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 8 && t == 2 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 8 && t == 3 && !fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
			// to light third
		else if (f == 8 && t == 3 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to fourth
			// to light fourth
		if (f == 8 && t == 4 && !fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to square fifth
		if (f == 8 && t == 5 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to sixth
			// to square sixth
		if (f == 8 && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "first");										// scale to first
		// to seventh
			// to dark square seventh
		if (f == 8 && t == 7 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
			// to light square seventh
		if (f == 8 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden

	// from light square eighth
		// to zero
		if (f == 8 && t == 0 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to dark zero
		if (f == 8 && t == 0 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to first
			// to dark first
		if (f == 8 && t == 1 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light first
		else if (f == 8 && t == 1 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to second
			// to dark second
		if (f == 8 && t == 2 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 8 && t == 2 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to third
			// to dark third
		if (f == 8 && t == 3 && fl && !tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
			// to light third
		else if (f == 8 && t == 3 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		// to fourth
			// to light fourth
		if (f == 8 && t == 4 && fl && tl && s == 2) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to square fifth
		if (f == 8 && t == 5 && fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// to sixth
			// to square sixth
		if (f == 8 && t == 6 && fl && !tl && s == 2) ScaleTo (true, "seventh", "first");										// scale to first
		// to seventh
			// to dark square seventh
		if (f == 8 && t == 7 && fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
			// to light square seventh
		if (f == 8 && t == 7 && fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden


///// ninth \\\\\


	// from dark circle ninth
		// to zero
		if (f == 9 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "zero");											// scale to hidden
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");											// scale to hidden
			// to light first
		else if (f == 9 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second
		if (f == 9 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "ninth", "zero");											// scale to hidden
		// to third
			// to dark third
		if (f == 9 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
			// to light third
		else if (f == 9 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to fourth
			// to dark fourth
		if (f == 9 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to dark circle fifth
		if (f == 9 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");											// scale to hidden
			// to light circle fifth
		else if (f == 9 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to sixth
			// to dark circle sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle sixth
		else if (f == 9 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "ninth", "zero");										// scale to hidden
		// to seventh
			// to dark circle seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");										// scale to hidden
			// to light circle seventh
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		// to eighth
			// to dark circle eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first

	// from light circle ninth
		// to zero (no nucleus change)
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 0) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to first
			// to dark first 
		if (f == 9 && t == 1 && fl && !tl && s == 0) ScaleTo (false, "hidden", "first");										// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 9 && t == 2 && fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light second 
		if (f == 9 && t == 2 && fl && tl && s == 0) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
			// to dark fourth
		if (f == 9 && t == 4 && fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		// to fifth
			// to dark circle fifth
		if (f == 9 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "first");										// scale to first
			// to light circle fifth (no nucleus change)
		// to sixth
			// to dark circle sixth
		if (f == 9 && t == 6 && fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle sixth 
		else if (f == 9 && t == 6 && !fl && tl && s == 0) ScaleTo (false, "hidden", "zero");									// scale to zero
		// to seventh
			// to dark circle seventh
		if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");										// scale to seventh
			// to light circle seventh (no nucleus change)
		// to eighth
			// to dark circle eighth
		if (f == 9 && t == 8 && fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle eighth
		else if (f == 9 && t == 8 && fl && tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first

	// from dark triangle ninth
		// to zero (no nucleus change)
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 1) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 1) ScaleTo (false, "hidden", "zero");									// scale to zero
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && !fl && tl && s == 1) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to fifth
			// to triangle fifth (no nucleus change)
		// to sixth
			// to triangle sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
		// to seventh
			// to dark triangle seventh (no nucleus change)
			// to light triangle seventh (no nucleus change)
		// to eighth
			// to dark triangle eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 1) {
			shape = 1;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 1) {
			shape = 1;																											// change to sphere
			changeShape = true;																									// set change shape flag	
			resetScale = true;																									// set reset scale flag
		}

	// from light triangle ninth
		// to zero (no nucleus change)
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 1) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to first
			// to dark first
		if (f == 9 && t == 1 && fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 9 && t == 2 && fl && !tl && s == 1) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 9 && t == 2 && fl && tl && s == 1) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && fl && tl && s == 1) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to fifth
			// to triangle fifth (no nucleus change)
		// to sixth
			// to triangle sixth
		if (f == 9 && t == 6 && fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
		// to seventh
			// to dark triangle seventh (no nucleus change)
			// to light triangle seventh (no nucleus change)
		// to eighth
			// to dark triangle eighth
		if (f == 9 && t == 8 && fl && !tl && s == 1) {
			shape = 1;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle eighth
		else if (f == 9 && t == 8 && fl && tl && s == 1) {
			shape = 1;																											// change to sphere
			changeShape = true;																									// set change shape flag	
			resetScale = true;																									// set reset scale flag
		}

	// from dark square ninth
		// to zero (no nucleus change)
		// to dark zero
		if (f == 9 && t == 0 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to first
			// to dark first
		if (f == 9 && t == 1 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 9 && t == 2 && !fl && !tl && s == 2) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 9 && t == 2 && !fl && tl && s == 2) ScaleTo (false, "hidden", "zero");									// scale to zero
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && !fl && tl && s == 2) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to fifth
			// to square fifth (no nucleus change)
		// to sixth
			// to square sixth
		if (f == 9 && t == 6 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first
		// to seventh
			// to dark square seventh (no nucleus change)
			// to light square seventh (no nucleus change)
		// to eighth
			// to dark square eighth
		if (f == 9 && t == 8 && !fl && !tl && s == 2) {
			shape = 2;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light square eighth
		else if (f == 9 && t == 8 && !fl && tl && s == 2) {
			shape = 2;																											// change to sphere
			changeShape = true;																									// set change shape flag	
			resetScale = true;																									// set reset scale flag
		}

	// from light square ninth
		// to zero (no nucleus change)
		// to dark zero
		if (f == 9 && t == 0 && fl && !tl && s == 2) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to first
			// to dark first
		if (f == 9 && t == 1 && fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first
			// to light first (no nucleus change)
		// to second
			// to dark second
		if (f == 9 && t == 2 && fl && !tl && s == 2) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light second
		else if (f == 9 && t == 2 && fl && tl && s == 2) ScaleTo (false, "hidden", "zero");										// scale to zero
		// to third
			// to dark third (no nucleus change)
			// to light third (no nucleus change)
		// to fourth
			// to light fourth
		if (f == 9 && t == 4 && fl && tl && s == 2) ScaleTo (false, "hidden", "zero");											// scale to zero
		// to fifth
			// to square fifth (no nucleus change)
		// to sixth
			// to square sixth
		if (f == 9 && t == 6 && fl && !tl && s == 2) ScaleTo (true, "hidden", "first");										// scale to first
		// to seventh
			// to dark square seventh (no nucleus change)
			// to light square seventh (no nucleus change)
		// to eighth
			// to dark square eighth
		if (f == 9 && t == 8 && fl && !tl && s == 2) {
			shape = 2;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light square eighth
		else if (f == 9 && t == 8 && fl && tl && s == 2) {
			shape = 2;																											// change to sphere
			changeShape = true;																									// set change shape flag	
			resetScale = true;																									// set reset scale flag
		}

	}

	///<summary>
	///<para>0 = circle</para>
	///<para>1 = triangle</para>
	///<para>2 = square</para>
	///<para>3 = ring</para>
	///</summary>
	private void SetShape(int s)
	{
		if (s == 0) GetComponent<MeshFilter>().mesh = sphere;								// change mesh to sphere
		else if (s == 1) GetComponent<MeshFilter>().mesh = triangle;						// change mesh to triangle
		else if (s == 2) GetComponent<MeshFilter>().mesh = square;							// change mesh to square
	}

	///<summary>
	///<para>set nucleus light into dark world</para>
	///<para>white = dark, black = light</para>
	///<para>true, true = black dot shader</para>
	///<para>false, true = solid white</para>
	///<para>true, false = white dot shader</para>
	///<para>false, false = solid black</para>
	///</summary>
	private void SetLight (bool lite, bool toLW)
	{
		if (lite && toLW) {												// if to light world and light (as in second, fourth, etc)
			rend.material.shader = darkShader;								// change to black shader
			//light = true;													// set is light flag
		} 
		else if (!lite && toLW) {										// if to light world and dark (as in dark zero, first, etc)
			anim.SetBool("black", false);									// reset previously active state
			anim.SetBool("white", true);									// set active state
			//light = false;													// reset is light flag
		}
		else if (lite && !toLW) {										// if to dark world and light (as in second, fourth, etc)
			rend.material.shader = lightShader;								// change to white shader
			//light = true;													// set is light flag
		}
		else if (!lite && !toLW) {										// if to dark world and dark (as in dark zero, first, etc)
			anim.SetBool("white", false);									// reset previously active state
			anim.SetBool("black", true);									// set active state
			//light = false;													// reset is light flag
		}
	}

	///<summary>
	///<para>set core as light</para>
	///<para>true (in dark world) = white dot shader</para>
	///<para>false (in dark world) = solid black</para>
	///<para>true (in light world) = black dot shader</para>
	///<para>false (in light world) = solid white</para>
	///</summary>
	private void SetLight (bool lite)
	{
		if (lite && !psp.lightworld) {
			rend.material.shader = lightShader;							// change to white shader
			//light = true;													// set is light flag
		} 
		else if (!lite && !psp.lightworld) {
			rend.material.shader = Shader.Find ("Unlit/Color");			// change to colour shader
			rend.material.SetColor("_Color", Color.black);				// change to black
			//light = false;												// reset is light flag
		}
		else if (lite && psp.lightworld) {
			rend.material.shader = darkShader;							// change to black shader
			//light = true;													// set is light flag
		}
		else if (!lite && psp.lightworld) {
			rend.material.shader = Shader.Find ("Unlit/Color");			// change to colour shader
			rend.material.SetColor("_Color", Color.white);				// change to white
			//light = false;												// reset is light flag
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
			anim.ResetTrigger ("scaleup");							// reset last stage
			anim.SetTrigger ("scaledown");							// enable scaledown
		}
		else {
			anim.ResetTrigger ("scaledown");						// reset last stage
			anim.SetTrigger ("scaleup");							// enable scaleup

		}
		anim.SetBool(resetState, false);							// reset previously active state
		anim.SetBool(setState, true);								// set active state
	}
}
