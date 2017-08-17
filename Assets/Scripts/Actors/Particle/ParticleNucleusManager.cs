using UnityEngine;
using System.Collections;

public class ParticleNucleusManager : MonoBehaviour {

	private Animator anim;																										// animator on core ref
	public Mesh sphere, triangle, square;																						// shape meshes
	private MeshRenderer rend;																									// mesh renderer (for colour changes)
	private ParticleStatePattern psp;																							// psp ref

	private float zeroPos, firstPos, thirdPos, seventhPos, ninthPos;															// y positions

	private int toState, shape;																									// to state indicator
	private bool toLight, colour, shader; 																						// to light, colour + shader indicator
	private bool changeColour = false, changeShape = false, resetScale = false;													// timer trigger for resetting scale after world switch
	private float changeColourTimer, changeShapeTimer, resetScaleTimer;															// reset scale timer

	private Shader lightShader, darkShader;																						// light/dark shaders

	void Start () {
		anim = GetComponent<Animator>();																						// init animator ref
		rend = GetComponent<MeshRenderer>();																					// init mesh renderer ref
		psp = GetComponentInParent<ParticleStatePattern> ();																	// init psp ref
		lightShader = Shader.Find("Unlit/light_nucleus");																		// init light nucleus shader
		darkShader = Shader.Find("Unlit/light_nucleus");																		// init light nucleus shader

		zeroPos =  0.175f;																										// set zero y position
		firstPos =  0.5f;																										// set first/second/fifth/sixth y position
		thirdPos =  1.0f;																										// set third/fourth y position
		seventhPos =  1.5f;																										// set seventh/eighth y position
		ninthPos =  2.0f;																										// set ninth y position
	}

	void Update() {

		// !inLightworld WORLD CHANGES \\  	- swapping nuclei colours for particles remaining in dark world, no state changes
			// to light world
		if (!psp.inLightworld && psp.changeParticles) {																			// if change particles and light world
			Debug.Log (psp.gameObject.name + ": change nucleus to opposite world");
			toState = psp.state;																									// set toState to current state
			ToOtherWorld (psp.lightworld, toState, toState, psp.isLight);																			// to hidden, change to white
		}

		// change colour timer
		if (changeColour) changeColourTimer += Time.deltaTime;																	// start timer
		if (changeColourTimer >= 2.0f) {																						// when timer >= 4 sec
			//Debug.Log("set colour: " + colour);
			SetLight(colour, shader);																										// set colour
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
		if (resetScaleTimer >= 2.75f) {																							// when timer >= 4 sec
			//anim.ResetTrigger("colour");	
			if (toState == 0) 																										// if to zero
				ScaleTo (false, "hidden", "zero");																					// grow to zero
			if (!toLight && (toState == 1 || toState == 2 || toState == 4 || toState == 5 || toState == 6)) 						// if to dark first/second/fourth/fifth/sixth
				ScaleTo (false, "hidden", "first");																						// grow to first
			if (shape == 0 && toLight && (toState == 2 || toState == 4 || toState == 6)) 											// if to light circle second/fourth/sixth
				ScaleTo (false, "hidden", "zero");																						// grow to zero
			if (!toLight && (toState == 7 || toState == 8)) 																		// if to dark seventh/eighth
				ScaleTo (false, "hidden", "seventh");																					// grow to seventh
			if (shape == 0 && toLight && toState == 8) 																				// if to light circle eighth 
				ScaleTo (false, "hidden", "first");																						// grow to first
			if (toState == 9) 																										// if to ninth
				ScaleTo (false, "hidden", "ninth");																						// grow to ninth
			resetScale = false;																										// reset reset scale flag
			resetScaleTimer = 0f;																									// reset timer
		}
	}

	public void ToOtherWorld (bool lw, int f, int t, bool tl) 
	{
		toState = t;																											// set to state
		if (lw) {																												// to light world
			// from changes
			if (!psp.isLight && f == 0) 																						// from dark zero
				ScaleTo (true, "zero", "hidden");																					// scale to hidden
			else if (!psp.isLight && (f == 1 || f == 2 || f == 5 || f == 6)) 													// from dark first/second/fifth/sixth
				ScaleTo (true, "first", "hidden");																					// scale to hidden
			else if (psp.isLight && (f == 2 || f == 6)) 																		// from light second/sixth
				ScaleTo (true, "zero", "hidden");																					// scale to hidden
			else if (f == 4) 																									// from fourth
				ScaleTo (true, "zero", "hidden");																					// scale to hidden
			else if (!psp.isLight && (f == 7 || f == 8)) 																		// from dark seventh/eighth
				ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			else if (psp.isLight && f == 8) 																					// from light eighth
				ScaleTo (true, "first", "hidden");																					// scale to hidden
			else if (!psp.isLight && f == 9) 																					// from dark ninth
				ScaleTo (true, "ninth", "hidden");																					// scale to hidden

			// to changes
			if (tl) {																												// if to light
				if (t == 0 || t == 1 || t == 3 || t == 5 || t == 7 || t == 9) {															// odd states
					colour = false;																											// change to white
					shader = true;																											// change to white
					changeColour = true; 																									// start change timer
				}
				else if (t == 2 || t == 4 || t == 6 || t == 8) {																		// even states
					colour = true;																											// change to black shader
					shader = true;																											// change to black shader
					changeColour = true;	 																								// start change timer
					resetScale = true;																										// set reset scale flag
				}
			}
			if (!tl) {																												// if to dark
				if (t == 0 || t == 1 || t == 3 || t == 5 || t == 7 || t == 9) {															// odd states
					colour = false;																											// change to white
					shader = true;																											// change to white
					changeColour = true;	 																								// start change timer
					resetScale = true;																										// set reset scale flag
				}
				else if (t == 2 || t == 4 || t == 6 || t == 8) {																		// even states
					colour = true;																											// change to white shader
					shader = false;																											// change to white shader
					changeColour = true;	 																								// start change timer
					resetScale = true;																										// set reset scale flag
				}
			}
		}
		else if (!lw) {																											// to dark world
			// from changes
			if (!psp.isLight && f == 0) 																							// from dark zero
				ScaleTo (true, "zero", "hidden");																						// scale to hidden
			else if (!psp.isLight && (f == 1 || f == 2 || f == 5 || f == 6)) 														// from dark first/second/fifth/sixth
				ScaleTo (true, "first", "hidden");																						// scale to hidden
			else if (psp.isLight && (f == 2 || f == 6)) 																			// from light second/sixth
				ScaleTo (true, "zero", "hidden");																						// scale to hidden
			else if (f == 4) 																										// from fourth
				ScaleTo (true, "zero", "hidden");																						// scale to hidden
			else if (!psp.isLight && (f == 7 || f == 8))																			// from dark seventh/eighth
				ScaleTo (true, "seventh", "hidden");																					// scale to hidden
			else if (psp.isLight && f == 8) 																						// from light eighth
				ScaleTo (true, "first", "hidden");																						// scale to hidden
			else if (!psp.isLight && f == 9) 																						// from dark ninth
				ScaleTo (true, "ninth", "hidden");																						// scale to hidden

			// to changes
			if (tl) {																												// if to light
				if (t == 0 || t == 1 || t == 3 || t == 5 || t == 7 || t == 9) {															// odd states
					colour = false;																											// change to black
					shader = false;																											// change to black
					changeColour = true; 																									// start change timer
				}
				else if (t == 2 || t == 4 || t == 6 || t == 8) {																		// even states
					colour = true;																											// change to black shader
					shader = true;																											// change to black shader
					changeColour = true;	 																								// start change timer
					resetScale = true;																										// set reset scale flag
				}
			}
			if (!tl) {																												// if to dark
				if (t == 0 || t == 1 || t == 3 || t == 5 || t == 7 || t == 9) {															// odd states
					colour = false;																											// change to black
					shader = false;																											// change to black
					changeColour = true;	 																								// start change timer
					resetScale = true;																										// set reset scale flag
				}
				else if (t == 2 || t == 4 || t == 6 || t == 8) {																		// even states
					colour = true;																											// change to white shader
					shader = false;																											// change to white shader
					changeColour = true;	 																								// start change timer
					resetScale = true;																										// set reset scale flag
				}
			}
		}
	}

	public void Nucleus (int f, int t, bool fl, bool tl, int s) 
	{
		// set up
		toState = t;																											// set to state
		toLight = tl;																											// set to light
		shape = s;																												// set s

		// ADJUST NUCLEUS HEIGHT FOR VISIBILITY
		if (toState == 0)	 																									// to zero
			transform.localPosition = new Vector3 (0f, zeroPos, 0f);																// adjust position
		else if (toState == 1 || toState == 2)																					// to first/second
			transform.localPosition = new Vector3 (0f, firstPos, 0f);																// adjust position
		else if (toState == 3 || toState == 4)																					// to third/fourth
			transform.localPosition = new Vector3 (0f, thirdPos, 0f);																// adjust position
		else if (toState == 5 || toState == 6) {																				// to fifth/sixth
			if (shape == 1) transform.localPosition = new Vector3 (0f, firstPos, 0.075f);											// adjust position
			else transform.localPosition = new Vector3 (0f, firstPos, 0f);															// adjust position
		}
		else if (toState == 7 || toState == 8) {																				// to seventh/eighth
			if (shape == 1) transform.localPosition = new Vector3 (0f, seventhPos, 0.075f);											// adjust position
			else transform.localPosition = new Vector3 (0f, seventhPos, 0f);														// adjust position
		}
		else if (toState == 9) {																								// to ninth
			if (shape == 1) transform.localPosition = new Vector3 (0f, ninthPos, 0.075f);											// adjust position
			else transform.localPosition = new Vector3 (0f, ninthPos, 0f);															// adjust position
		}


		///////////////////// EVOLUTIONS \\\\\\\\\\\\\\\\\\\\\\


	///// zero \\\\\


		// to dark zero
		if (f == 0 && t == 0 && !fl && !tl) ScaleTo (false, "hidden", "zero");													// scale to zero

		// to dark first (init)
		if (f == 0 && t == 1 && !fl && !tl) ScaleTo (false, "hidden", "first");													// scale to first


	///// half zero \\\\\


		// from dark zero (0.5)
				// to dark
		if (f == 0 && t == 1 && !fl && !tl) ScaleTo (false, "zero", "first");													// scale to first
		else if (f == 0 && t == 2 && !fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 3 && !fl && !tl) ScaleTo (true, "zero", "hidden");												// scale to third
		else if (f == 0 && t == 4 && !fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 5 && !fl && !tl) ScaleTo (false, "zero", "first");												// scale to first
		else if (f == 0 && t == 6 && !fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 7 && !fl && !tl) ScaleTo (false, "zero", "seventh");											// scale to seventh
		else if (f == 0 && t == 8 && !fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 9 && !fl && !tl) ScaleTo (false, "zero", "ninth");												// scale to ninth
			// to light
		if (f == 0 && t == 0 && !fl && tl) ScaleTo (true, "zero", "hidden");													// scale to hidden
		else if (f == 0 && t == 1 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to first
		else if (f == 0 && t == 2 && !fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 3 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to hidden
		else if (f == 0 && t == 5 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to hidden
		else if (f == 0 && t == 6 && !fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 7 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to seventh
		else if (f == 0 && t == 8 && !fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 9 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to ninth

		// from light zero (0.5)
			// to dark
		if (f == 0 && t == 1 && fl && !tl) ScaleTo (false, "hidden", "first");													// scale to first
		else if (f == 0 && t == 2 && fl && !tl) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 4 && fl && !tl) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 5 && fl && !tl) ScaleTo (false, "hidden", "first");												// scale to first
		else if (f == 0 && t == 6 && fl && !tl) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 7 && fl && !tl) ScaleTo (false, "hidden", "seventh");											// scale to seventh
		else if (f == 0 && t == 8 && fl && !tl) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 9 && fl && !tl) ScaleTo (false, "hidden", "ninth");												// scale to ninth
			// to light
		else if (f == 0 && t == 2 && fl && tl) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 4 && fl && tl) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 6 && fl && tl) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 8 && fl && tl) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}


	///// first \\\\\


		// from dark first
			// to dark
		if (f == 1 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");													// scale to zero
		else if (f == 1 && t == 2 && !fl && !tl) {
			//ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			//resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 3 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 1 && t == 4 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 7 && !fl && !tl) ScaleTo (false, "first", "seventh");											// scale to seventh
		else if (f == 1 && t == 8 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 9 && !fl && !tl) ScaleTo (false, "first", "ninth");												// scale to ninth
			// to light
		if (f == 1 && t == 0 && !fl && tl) ScaleTo (true, "first", "hidden");													// scale to hidden
		else if (f == 1 && t == 1 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden
		else if (f == 1 && t == 2 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			colour = false;																										// colour to black shader
			resetScale = true;																									// set reset scale flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 1 && t == 3 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 1 && t == 4 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 5 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden
		else if (f == 1 && t == 6 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 7 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden
		else if (f == 1 && t == 8 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 9 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden

		// from light first
			// to dark
		if (f == 1 && t == 0 && fl && !tl) ScaleTo (true, "first", "zero");														// scale to zero
		else if (f == 1 && t == 2 && fl && !tl) {			
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 4 && fl && !tl) {			
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 5 && fl && !tl) ScaleTo (false, "hidden", "first");												// scale to first
		else if (f == 1 && t == 6 && fl && !tl) {			
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 7 && fl && !tl) ScaleTo (false, "hidden", "seventh");											// scale to seventh
		else if (f == 1 && t == 8 && fl && !tl) {			
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 9 && fl && !tl) ScaleTo (false, "hidden", "ninth");												// scale to ninth
			// to light
		if (f == 1 && t == 2 && fl && tl) {
			SetLight (false);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 4 && fl && tl) {
			SetLight (false);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 6 && fl && tl) {
			SetLight (false);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 8 && fl && tl) {
			SetLight (false);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}

	///// second \\\\\


		// from dark second
			// to dark
		if (f == 2 && t == 0 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 1 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 3 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 4 && !fl && !tl) ScaleTo (true, "first", "zero");												// scale to zero
		else if (f == 2 && t == 5 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 7 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 8 && !fl && !tl) ScaleTo (false, "first", "seventh");											// scale to seventh
		else if (f == 2 && t == 9 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light
		if (f == 2 && t == 0 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 1 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 2 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 3 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 4 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 5 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 6 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 7 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 8 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 9 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

		// from light second
			// to dark
		if (f == 2 && t == 0 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 1 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 2 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 3 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 4 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 5 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 6 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 7 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 8 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 9 && fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light
		if (f == 2 && t == 0 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 1 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 3 && fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 5 && fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 7 && fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 8 && fl && tl) ScaleTo (false, "zero", "first");												// scale to first
		else if (f == 2 && t == 9 && fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		

	///// third \\\\\


		// from dark third
			// to dark circle
		if (f == 3 && t == 0 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "zero");										// scale to zero
		else if (f == 3 && t == 1 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 2 && !fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && !fl && !tl && s == 0) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 5 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 6 && !fl && !tl && s == 0) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 3 && t == 8 && !fl && !tl && s == 0) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 3 && t == 2 && !fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && !fl && tl && s == 0) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 6 && !fl && tl && s == 0) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 8 && !fl && tl && s == 0) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
			// to light triangle
		if (f == 3 && t == 5 && !fl && tl && s == 1) {
			SetShape(1);																										// change to triangle
			resetScale = true;																									// set reset scale flag
		}
			// to light square
		if (f == 3 && t == 5 && !fl && tl && s == 2) {
			SetShape(2);																										// change to square
			resetScale = true;																									// set reset scale flag
		}

		// from light third
			// to dark circle
		if (f == 3 && t == 0 && fl && !tl && s == 0) ScaleTo (false, "hidden", "zero");											// scale to zero
		else if (f == 3 && t == 1 && fl && !tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 2 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && fl && !tl && s == 0) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 6 && fl && !tl && s == 0) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 3 && t == 8 && fl && !tl && s == 0) {
			SetLight (true);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 3 && t == 2 && fl && tl && s == 0) {
			SetLight(false);																									// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && fl && tl && s == 0) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 6 && fl && tl && s == 0) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 8 && fl && tl && s == 0) {
			SetLight (false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
			// to light triangle
		if (f == 3 && t == 5 && fl && tl && s == 1) {
			SetShape(1);																										// change to triangle
			resetScale = true;																									// set reset scale flag
		}
		// to light square
		if (f == 3 && t == 5 && fl && tl && s == 2) {
			SetShape(2);																										// change to square
			resetScale = true;																									// set reset scale flag
		}


	///// fourth \\\\\


		// from dark fourth
			// to dark circle
		if (f == 4 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 2 && !fl && !tl && s == 0) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 4 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 5 && !fl && !tl && s == 0) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 6 && !fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && !fl && !tl && s == 0) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 8 && !fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && !fl && !tl && s == 0) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 4 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 4 && !fl && tl && s == 0) {
			//ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			//resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 5 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 8 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && !fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

		// from light fourth
			// to dark circle
		if (f == 4 && t == 0 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		if (f == 4 && t == 1 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		if (f == 4 && t == 2 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		if (f == 4 && t == 3 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
		}
		if (f == 4 && t == 4 && fl && !tl && s == 0) {
			//ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
			// to light circle
		if (f == 4 && t == 0 && fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 1 && fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 3 && fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
		}
			// to light triangle
		if (f == 4 && t == 5 && fl && tl && s == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 1;																											// change to triangle
			changeShape = true;																									// set change shape flag
		}
		else if (f == 4 && t == 6 && fl && tl && s == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 1;																											// change to triangle
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && tl && s == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 1;																											// change to triangle
			changeShape = true;																									// set change shape flag
		}
		else if (f == 4 && t == 8 && fl && tl && s == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 1;																											// change to triangle
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && tl && s == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 1;																											// change to triangle
			changeShape = true;																									// set change shape flag
		}
			// to light square
		if (f == 4 && t == 5 && fl && tl && s == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 2;																											// change to triangle
			changeShape = true;																									// set change shape flag
		}
		else if (f == 4 && t == 6 && fl && tl && s == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 2;																											// change to triangle
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && tl && s == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 2;																											// change to triangle
			changeShape = true;																									// set change shape flag
		}
		else if (f == 4 && t == 8 && fl && tl && s == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 2;																											// change to triangle
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && tl && s == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			shape = 2;																											// change to triangle
			changeShape = true;																									// set change shape flag
		}


	///// fifth \\\\\


		// from dark circle fifth
			// to dark circle
		if (f == 5 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "first", "zero");											// scale to zero
		else if (f == 5 && t == 2 && !fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
		}
		else if (f == 5 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 5 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 6 && !fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
		}
		else if (f == 5 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 5 && t == 8 && !fl && !tl && s == 0) {
			ScaleTo (false, "first", "seventh");																				// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
		}
		else if (f == 5 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "first", "ninth");									// scale to ninth
			// to light circle
		if (f == 5 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");											// scale to hidden
		else if (f == 5 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 5 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 5 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 5 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 5 && t == 8 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden

		// from light circle fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && s == 0) ScaleTo (false, "hidden", "zero");											// scale to zero
		else if (f == 5 && t == 1 && fl && !tl && s == 0) ScaleTo (false, "hidden", "zero");									// scale to zero
		else if (f == 5 && t == 2 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader	
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 4 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 5 && t == 6 && fl && !tl && s == 0) {
			SetLight(true);																										// set to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 5 && t == 8 && fl && !tl && s == 0) {
			SetLight(true);																										// set to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 5 && t == 2 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader	
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 4 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 6 && fl && tl && s == 0) {
			SetLight(false);																									// set to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 7 && fl && tl && s == 0) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 5 && t == 8 && fl && tl && s == 0) {
			SetLight(false);																									// set to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && fl && tl && s == 0) ScaleTo (false, "first", "ninth");										// scale to ninth

		// from light triangle fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 1 && fl && !tl && s == 0) {											
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 2 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 3 && fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 5 && t == 4 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light circle
		if (f == 5 && t == 0 && fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 5 && t == 1 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 5 && t == 2 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 3 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 5 && t == 4 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark triangle	
		if (f == 5 && t == 6 && fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
			// to seventh = no change

		// from light square fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 1 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 2 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 3 && fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 5 && t == 4 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light circle
		if (f == 5 && t == 0 && fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 5 && t == 1 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 5 && t == 2 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 3 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 5 && t == 4 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark square
		if (f == 5 && t == 6 && fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first
			// to seventh = no change


	//// sixth \\\\\


		// from dark circle sixth
			// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		} 
		else if (f == 6 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 7 && !fl && !tl && s == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 8 && !fl && !tl && s == 0) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 6 && t == 9 && !fl && !tl && s == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 7 && !fl && tl && s == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 8 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 9 && !fl && tl && s == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

		// from light circle sixth
			// to dark circle
		if (f == 6 && t == 0 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 1 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 2 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 4 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && fl && !tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 6 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 7 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 8 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 9 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 6 && t == 0 && fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 1 && fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 3 && fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
			// fourth = no change
		else if (f == 6 && t == 5 && fl && tl && s == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 7 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 8 && fl && tl && s == 0) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 6 && t == 9 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

		// from dark triangle sixth
			// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 2 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "first", "hidden");									// scale to hidden
			// to dark triangle
		if (f == 6 && t == 7 && !fl && !tl && s == 1) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to eighth = no change
			// to light triangle
		if (f == 6 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "first", "hidden");											// scale to hidden
			// to eighth = no change

		// from dark square sixth
		// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 2 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && !fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
			// to dark square
		if (f == 6 && t == 7 && !fl && !tl && s == 2) ScaleTo (true, "first", "hidden");										// scale to hidden
			// to eighth = no change
			// to light square
		if (f == 6 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "first", "hidden");											// scale to hidden
			// to eighth = no change


	///// seventh \\\\\


		// from dark circle seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "zero");										// scale to zero
		else if (f == 7 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");									// scale to first
		else if (f == 7 && t == 2 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");									// scale to first
		else if (f == 7 && t == 6 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 8 && !fl && !tl && s == 0) {
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
		}
		else if (f == 7 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle
		if (f == 7 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 8 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden

		// from light circle seventh
			// to dark circle
		if (f == 7 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "hidden", "zero");											// scale to zero
		else if (f == 7 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "hidden", "first");									// scale to first
		else if (f == 7 && t == 2 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 4 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "hidden", "first");									// scale to first
		else if (f == 7 && t == 6 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 8 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 7 && t == 2 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 4 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 6 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 8 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}

		// from dark triangle seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 1 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 2 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 7 && t == 0 && !fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 7 && t == 1 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 2 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark triangle
		if (f == 7 && t == 6 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 7 && t == 8 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");								// scale to seventh
			// ninth = no change
			// to light triangle
		else if (f == 7 && t == 8 && !fl && tl && s == 1) ScaleTo (false, "first", "seventh");									// scale to seventh
			// ninth = no change

		// from light triangle seventh
			// to dark circle
		if (f == 7 && t == 0 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 1 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 2 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 7 && t == 0 && fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 7 && t == 1 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 2 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark triangle
		if (f == 7 && t == 6 && fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 7 && t == 8 && fl && !tl && s == 1) ScaleTo (false, "first", "seventh");									// scale to seventh
			// to light triangle
		if (f == 7 && t == 8 && fl && tl && s == 1) ScaleTo (false, "first", "seventh");										// scale to seventh

		// from dark square seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 1 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 2 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 7 && t == 3 && !fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 7 && t == 0 && !fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 7 && t == 1 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 2 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}	
			// to dark square
		if (f == 7 && t == 6 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 7 && t == 8 && !fl && !tl && s == 2) ScaleTo (false, "first", "seventh");									// scale to seventh
			// ninth = no change
			// to light square
		if (f == 7 && t == 8 && !fl && tl && s == 2) ScaleTo (false, "first", "seventh");										// scale to seventh
			// ninth = no change

		// from light square seventh
			// to dark circle
		if (f == 7 && t == 0 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 1 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 2 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 7 && t == 0 && fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 7 && t == 1 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 2 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 7 && t == 4 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark square
		if (f == 7 && t == 6 && fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 7 && t == 8 && fl && !tl && s == 2) ScaleTo (false, "first", "seventh");									// scale to seventh
			// ninth = no change
			// to light square
		if (f == 7 && t == 8 && fl && tl && s == 2) ScaleTo (false, "first", "seventh");										// scale to seventh
			// ninth = no change


	//// eighth \\\\\


		// from dark circle eighth
			// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 8 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");									// scale to first
		else if (f == 8 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "zero");									// scale to first
		else if (f == 8 && t == 5 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "first");									// scale to first
		else if (f == 8 && t == 7 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && !fl && !tl && s == 0) {			
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 8 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && !fl && tl && s == 0) {		
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

		// from light circle eighth
			// to dark circle
		if (f == 8 && t == 0 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 6 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && fl && !tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 8 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 1 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 2 && fl && tl && s == 0) ScaleTo (true, "first", "zero");										// scale to zero
		else if (f == 8 && t == 3 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																				// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 6 && fl && tl && s == 0) ScaleTo (true, "first", "zero");										// scale to zero
		else if (f == 8 && t == 7 && fl && tl && s == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 9 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			colour = false;																										// colour to black
			changeColour = true;																								// set change colour flag
		}

		// from dark triangle eighth
			// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 8 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark triangle
		if (f == 8 && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "first");										// scale to first
		else if (f == 8 && t == 7 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
			// to light triangle
		if (f == 8 && t == 5 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden

		// from light triangle eighth
			// to dark circle
		if (f == 8 && t == 0 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 1 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 2 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark triangle
		if (f == 8 && t == 6 && fl && !tl && s == 1) ScaleTo (true, "seventh", "first");										// scale to first
		else if (f == 8 && t == 7 && fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
			// to light triangle
		if (f == 8 && t == 5 && fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 7 && fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden

		// from dark square eighth
		// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 1 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark square
		if (f == 8 && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "first");										// scale to first
		else if (f == 8 && t == 7 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden
			// to light square
		if (f == 8 && t == 5 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden

		// from light square eighth
			// to dark circle
		if (f == 8 && t == 0 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 8 && t == 3 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && fl && !tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 1 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 2 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && fl && tl && s == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			shape = 0;																											// change to sphere
			changeShape = true;																									// set change shape flag
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark square
		if (f == 8 && t == 6 && fl && !tl && s == 2) ScaleTo (true, "seventh", "first");										// scale to first
		else if (f == 8 && t == 7 && fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden
			// to light square
		if (f == 8 && t == 5 && fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 7 && fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden


	///// ninth \\\\\


		// from dark circle ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "zero");											// scale to hidden
		else if (f == 9 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && !tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 4 && !fl && !tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "first");									// scale to hidden
		else if (f == 9 && t == 6 && !fl && !tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to hidden
		else if (f == 9 && t == 8 && !fl && !tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 10 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "tenth");									// scale to tenth
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 4 && !fl && tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to hidden
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 8 && !fl && tl && s == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 10 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden

		// from light circle ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && s == 0) ScaleTo (false, "hidden", "zero");											// scale to zero
		else if (f == 9 && t == 1 && fl && !tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 9 && t == 2 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 4 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 9 && t == 6 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && s == 0) {
			SetLight(true);																										// change to white shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 10 && fl && !tl && s == 0) ScaleTo (false, "hidden", "tenth");									// scale to tenth
			// to light circle 
		if (f == 9 && t == 2 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 4 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 6 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 8 && fl && tl && s == 0) {
			SetLight(false);																									// change to black shader
			resetScale = true;																									// set reset scale flag
		}
			// tenth (no change)

		// from dark triangle ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 1 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 2 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 9 && t == 1 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 2 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 9 && t == 3 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to dark triangle
		if (f == 9 && t == 6 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 9 && t == 8 && !fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 9 && t == 10 && !fl && !tl && s == 1) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light triangle 
		if (f == 9 && t == 8 && !fl && tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh	
			// tenth (no nucleus change)

		// from light triangle ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 1 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 2 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 9 && t == 1 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 2 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 9 && t == 3 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark triangle
		if (f == 9 && t == 6 && fl && !tl && s == 1) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 9 && t == 8 && fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && fl && !tl && s == 1) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light triangle
		if (f == 9 && t == 8 && fl && tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh	
			// tenth (no nucleus change)

		// from dark square ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 1 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 2 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && !fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 9 && t == 1 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 2 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && !fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark square
		if (f == 9 && t == 6 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 9 && t == 8 && !fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 9 && t == 10 && !fl && !tl && s == 2) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light square
		if (f == 9 && t == 8 && !fl && tl && s == 2) ScaleTo (false, "hidden", "seventh");										// scale to seventh	
			// tenth (no nucleus change)

		// from light square ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 1 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 2 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && fl && !tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && fl && !tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = true;																										// colour to white shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && fl && tl && s == 0) SetShape(0);																// change to sphere
		else if (f == 9 && t == 1 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 2 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && fl && tl && s == 0) SetShape(0);															// change to sphere
		else if (f == 9 && t == 4 && fl && tl && s == 0) {
			SetShape(0);																										// change to sphere
			colour = false;																										// colour to black shader
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark square
		if (f == 9 && t == 6 && fl && !tl && s == 2) ScaleTo (true, "hidden", "first");											// scale to first
		else if (f == 9 && t == 8 && fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && fl && !tl && s == 2) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light square
		else if (f == 9 && t == 8 && fl && tl && s == 2) ScaleTo (false, "hidden", "seventh");									// scale to seventh	
			// tenth (no nucleus change)


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
	///<para>set nucleus light/colour for world switches</para>
	///<para>white = dark, black = light</para>
	///<para>true, true = black shader</para>
	///<para>false, true = solid white</para>
	///<para>true, false = white shader</para>
	///<para>false, false = solid black</para>
	///</summary>
	private void SetLight (bool l, bool toLW)
	{
		if (l && toLW) {												// if to light world and light (as in second, fourth, etc)
			rend.material.shader = darkShader;								// change to black shader
		} 
		else if (!l && toLW) {											// if to light world and dark (as in dark zero, first, etc)
			anim.SetBool("black", false);									// reset previously active state
			anim.SetBool("white", true);									// set active state
		}
		else if (l && !toLW) {											// if to dark world and light (as in second, fourth, etc)
			rend.material.shader = lightShader;								// change to white shader
		}
		else if (!l && !toLW) {											// if to dark world and dark (as in dark zero, first, etc)
			anim.SetBool("white", false);									// reset previously active state
			anim.SetBool("black", true);									// set active state
		}
	}

	///<summary>
	///<para>set nucleus light/colour for normal state-to-state evolution</para>
	///<para>true (in dark world) = white shader</para>
	///<para>false (in dark world) = solid black</para>
	///<para>true (in light world) = black shader</para>
	///<para>false (in light world) = solid white</para>
	///</summary>
	private void SetLight (bool l)
	{
		if (l && !psp.lightworld) {
			rend.material.shader = lightShader;							// change to white shader
		} 
		else if (!l && !psp.lightworld) {
			if (toState != 0 && toState % 2 == 0) rend.material.shader = darkShader;											// if even # state, change to black shader
			else {
				anim.SetBool("white", false);									// reset previously active state
				anim.SetBool("black", true);									// set active state
				//rend.material.shader = Shader.Find ("Unlit/Color");			// change shader
				//rend.material.SetColor("_Color", Color.black);				// change to black
			}
		}
		else if (l && psp.lightworld) {
			rend.material.shader = darkShader;							// change to black shader
		}
		else if (!l && psp.lightworld) {
			if (toState != 0 && toState % 2 == 0) rend.material.shader = lightShader;											// if even # state, change to white shader
			else {
				anim.SetBool("black", false);									// reset previously active state
				anim.SetBool("white", true);									// set active state
				//rend.material.shader = Shader.Find ("Unlit/Color");			// change shader
				//rend.material.SetColor("_Color", Color.white);				// change to white
			}
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
