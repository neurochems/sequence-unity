using UnityEngine;
using System.Collections;

public class ParticleNucleusManager : MonoBehaviour {

	private Animator anim;																										// animator on core ref
	public Mesh sphere, triangle, square;																						// shape meshes
	private MeshRenderer rend;																									// mesh renderer (for colour changes)
	private ParticleStatePattern psp;																							// psp ref

	private float zeroPos, firstPos, thirdPos, seventhPos, ninthPos;															// y positions

	private int fromState, toState, fromShape, toShape;																					// to state, from shape, to shape indicator
	private bool toLight, colour, shader; 																						// to light, colour + shader indicator
	private bool changeColour = false, changeShape = false, resetScale = false;													// timer trigger for resetting scale after world switch
	private float changeColourTimer, changeShapeTimer, resetScaleTimer;															// reset scale timer

	private Shader lightShader, darkShader;																						// light/dark shaders

	void Start () {
		anim = GetComponent<Animator>();																						// init animator ref
		rend = GetComponent<MeshRenderer>();																					// init mesh renderer ref
		psp = GetComponentInParent<ParticleStatePattern> ();																	// init psp ref
		lightShader = Shader.Find("Unlit/light_nucleus");																		// init light nucleus shader
		darkShader = Shader.Find("Unlit/dark_nucleus");																			// init dark nucleus shader

		zeroPos = 0.15f;																										// set zero y position
		firstPos = 0.50f;																										// set first/second/fifth/sixth y position
		thirdPos = 1.0f;																										// set third/fourth y position
		seventhPos = 1.5f;																										// set seventh/eighth y position
		ninthPos = 2.0f;																										// set ninth y position
	}

	void Update() {

		// !inLightworld WORLD CHANGES \\  	- swapping nuclei colours for particles remaining in dark world, no state changes
			// to light world
		if (!psp.inLightworld && psp.changeParticles) {																			// if change particles and light world
			//Debug.Log (psp.gameObject.name + ": change nucleus to opposite world");
			toState = psp.state;																									// set toState to current state
			if (psp.isLight) ToOtherWorld (psp.lightworld, toState, toState, true, fromShape, toShape);							// if light: to hidden, change to white
			else if (!psp.isLight) ToOtherWorld (psp.lightworld, toState, toState, false, fromShape, toShape);						// if dark: to hidden, change to black
		}


		// change colour timer
		if (changeColour) changeColourTimer += Time.deltaTime;																	// start timer
		if (changeColourTimer >= 2.0f) {																						// when timer >= 4 sec
			SetLight(toLight);																										// set colour
			changeColour = false;																									// reset reset scale flag
			changeColourTimer = 0f;																									// reset timer
		}

		// change shape timer
		if (changeShape) changeShapeTimer += Time.deltaTime;																	// start timer
		if (changeShapeTimer >= 2.0f) {																							// when timer >= 2 sec
			SetShape(toShape);																										// set shape
			changeShape = false;																									// reset reset scale flag
			changeShapeTimer = 0f;																									// reset timer
		}
		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;																		// start timer
		if (resetScaleTimer >= 2.75f) {																							// when timer >= 2.75 sec

			// ADJUST NUCLEUS HEIGHT FOR VISIBILITY for devolutions
			if (toState < fromState) {
				if (toState == 0)	 																									// to zero
					transform.localPosition = new Vector3 (0f, zeroPos, 0f);																// adjust position
				else if (toState == 1 || toState == 2)																					// to first/second
					transform.localPosition = new Vector3 (0f, firstPos, 0f);																// adjust position
				else if (toState == 3 || toState == 4)																					// to third/fourth
					transform.localPosition = new Vector3 (0f, thirdPos, 0f);																// adjust position
				else if (toState == 5 || toState == 6)																					// to fifth/sixth
					transform.localPosition = new Vector3 (0f, firstPos, 0f);																// adjust position
				else if (toState == 7 || toState == 8)																					// to seventh/eighth
					transform.localPosition = new Vector3 (0f, seventhPos, 0f);																// adjust position
				else if (toState == 9)																									// to ninth
					transform.localPosition = new Vector3 (0f, ninthPos, 0f);																// adjust position
			}

			// reset scale
			if (toState == 0) 																										// if to zero
				ScaleTo (false, "hidden", "zero");																					// grow to zero
			if (!toLight && (toState == 1 || toState == 2 || toState == 5 || toState == 6)) 										// if to dark first/fourth/fifth/sixth
				ScaleTo (false, "hidden", "first");																						// grow to first
			if (toShape == 0 && toLight && (toState == 2 || toState == 4 || toState == 6)) 											// if to light circle second/fourth/sixth
				ScaleTo (false, "hidden", "zero");																						// grow to zero
			if (!toLight && toState == 4)
				ScaleTo (false, "hidden", "zero");																						// grow to zero
			if (!toLight && (toState == 7 || toState == 8)) 																		// if to dark seventh/eighth
				ScaleTo (false, "hidden", "seventh");																					// grow to seventh
			if (toShape == 0 && toLight && toState == 8) 																				// if to light circle eighth 
				ScaleTo (false, "hidden", "first");																						// grow to first
			if (toState == 9) 																										// if to ninth
				ScaleTo (false, "hidden", "ninth");																						// grow to ninth
			resetScale = false;																										// reset reset scale flag
			resetScaleTimer = 0f;																									// reset timer
		}
	}

	///<summary>
	///<para>set nucleus world</para>
	///<para>lw = to light world = T, to dark world = F</para>
	///<para>f = from state</para>
	///<para>t = to state</para>
	///<para>tl = to light</para>
	///<para>fs = from shape</para>
	///<para>ts = to shape</para>
	///</summary>
	public void ToOtherWorld (bool lw, int f, int t, bool tl, int fs, int ts) 
	{
		fromState = f;																											// set to state
		toState = t;																											// set to state
		toLight = tl;																											// set to light
		toShape = ts;																											// set to light

		// ADJUST NUCLEUS HEIGHT FOR VISIBILITY on evolutions
		if (t > f) {
			if (toState == 0)	 																									// to zero
				transform.localPosition = new Vector3 (0f, zeroPos, 0f);																// adjust position
			else if (toState == 1 || toState == 2)																					// to first/second
				transform.localPosition = new Vector3 (0f, firstPos, 0f);																// adjust position
			else if (toState == 3 || toState == 4)																					// to third/fourth
				transform.localPosition = new Vector3 (0f, thirdPos, 0f);																// adjust position
			else if (toState == 5 || toState == 6)																					// to fifth/sixth
				transform.localPosition = new Vector3 (0f, firstPos, 0f);																// adjust position
			else if (toState == 7 || toState == 8)																					// to seventh/eighth
				transform.localPosition = new Vector3 (0f, seventhPos, 0f);																// adjust position
			else if (toState == 9)																									// to ninth
				transform.localPosition = new Vector3 (0f, ninthPos, 0f);																// adjust position
		}

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

			// shape changes
			if ((ts == 0) && (fs != 0)) changeShape = true;																		// change to circle
			else if ((ts == 1) && (fs != 1)) changeShape = true;																// change to triangle
			else if ((ts == 2) && (fs != 2)) changeShape = true;																// change to square

			// to changes
			changeColour = true; 																								// start change timer

			if (tl && (t == 2 || t == 4 || t == 6 || t == 8)) resetScale = true;												// to light even states: set reset scale flag
			if (!tl && (t != 3)) resetScale = true;																				// to dark not third, set reset scale flag
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

			// shape changes
			if ((ts == 0) && (fs != 0)) changeShape = true;															// change to circle
			else if ((ts == 1) && (fs != 1)) changeShape = true;													// change to triangle
			else if ((ts == 2) && (fs != 2)) changeShape = true;													// change to square

			// to changes
			changeColour = true; 																									// start change timer

			if (tl && (t == 2 || t == 4 || t == 6 || t == 8)) resetScale = true;													// to light even states, set reset scale flag
			if (!tl && (t != 3)) resetScale = true;																					// to dark not third, set reset scale flag
		}
	}

	///<summary>
	///<para>set nucleus state</para>
	///<para>f = from state</para>
	///<para>t = to state</para>
	///<para>fl = from light</para>
	///<para>tl = to light</para>
	///<para>fs = from shape</para>
	///<para>ts = to shape</para>
	///</summary>
	public void Nucleus (int f, int t, bool fl, bool tl, int fs, int ts) 
	{
		// set up
		toState = t;																											// set to state
		toLight = tl;																											// set to light
		fromShape = fs;																											// set from shape
		toShape = ts;																											// set to shape

		// ADJUST NUCLEUS HEIGHT FOR VISIBILITY on evolutions
		if (t > f) {
			if (toState == 0)	 																									// to zero
				transform.localPosition = new Vector3 (0f, zeroPos, 0f);																// adjust position
			else if (toState == 1 || toState == 2)																					// to first/second
				transform.localPosition = new Vector3 (0f, firstPos, 0f);																// adjust position
			else if (toState == 3 || toState == 4)																					// to third/fourth
				transform.localPosition = new Vector3 (0f, thirdPos, 0f);																// adjust position
			else if (toState == 5 || toState == 6)																					// to fifth/sixth
				transform.localPosition = new Vector3 (0f, firstPos, 0f);																// adjust position
			else if (toState == 7 || toState == 8)																					// to seventh/eighth
				transform.localPosition = new Vector3 (0f, seventhPos, 0f);																// adjust position
			else if (toState == 9)																									// to ninth
				transform.localPosition = new Vector3 (0f, ninthPos, 0f);																// adjust position
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
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 3 && !fl && !tl) ScaleTo (true, "zero", "hidden");												// scale to third
		else if (f == 0 && t == 4 && !fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 5 && !fl && !tl) ScaleTo (false, "zero", "first");												// scale to first
		else if (f == 0 && t == 6 && !fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 7 && !fl && !tl) ScaleTo (false, "zero", "seventh");											// scale to seventh
		else if (f == 0 && t == 8 && !fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 9 && !fl && !tl) ScaleTo (false, "zero", "ninth");												// scale to ninth
			// to light
		if (f == 0 && t == 0 && !fl && tl) ScaleTo (true, "zero", "hidden");													// scale to hidden
		else if (f == 0 && t == 1 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to first
		else if (f == 0 && t == 2 && !fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 3 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to hidden
		else if (f == 0 && t == 5 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to hidden
		else if (f == 0 && t == 6 && !fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 7 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to seventh
		else if (f == 0 && t == 8 && !fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 9 && !fl && tl) ScaleTo (true, "zero", "hidden");												// scale to ninth

		// from light zero (0.5)
			// to dark
		if (f == 0 && t == 1 && fl && !tl) ScaleTo (false, "hidden", "first");													// scale to first
		else if (f == 0 && t == 2 && fl && !tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 4 && fl && !tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 5 && fl && !tl) ScaleTo (false, "hidden", "first");												// scale to first
		else if (f == 0 && t == 6 && fl && !tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 7 && fl && !tl) ScaleTo (false, "hidden", "seventh");											// scale to seventh
		else if (f == 0 && t == 8 && fl && !tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 9 && fl && !tl) ScaleTo (false, "hidden", "ninth");												// scale to ninth
			// to light
		else if (f == 0 && t == 2 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 4 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 6 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 0 && t == 8 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}


	///// first \\\\\


		// from dark first
			// to dark
		if (f == 1 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");													// scale to zero
		else if (f == 1 && t == 2 && !fl && !tl) changeColour = true;															// set change colour flag
		else if (f == 1 && t == 3 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 1 && t == 4 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 7 && !fl && !tl) ScaleTo (false, "first", "seventh");											// scale to seventh
		else if (f == 1 && t == 8 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 9 && !fl && !tl) ScaleTo (false, "first", "ninth");												// scale to ninth
			// to light
		if (f == 1 && t == 0 && !fl && tl) ScaleTo (true, "first", "hidden");													// scale to hidden
		else if (f == 1 && t == 1 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden
		else if (f == 1 && t == 2 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			resetScale = true;																									// set reset scale flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 1 && t == 3 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 1 && t == 4 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 5 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden
		else if (f == 1 && t == 6 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 7 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden
		else if (f == 1 && t == 8 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to zero
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 9 && !fl && tl) ScaleTo (true, "first", "hidden");												// scale to hidden

		// from light first
			// to dark
		if (f == 1 && t == 0 && fl && !tl) ScaleTo (true, "first", "zero");														// scale to zero
		else if (f == 1 && t == 2 && fl && !tl) {			
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 4 && fl && !tl) {			
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 5 && fl && !tl) ScaleTo (false, "hidden", "first");												// scale to first
		else if (f == 1 && t == 6 && fl && !tl) {			
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 7 && fl && !tl) ScaleTo (false, "hidden", "seventh");											// scale to seventh
		else if (f == 1 && t == 8 && fl && !tl) {			
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 9 && fl && !tl) ScaleTo (false, "hidden", "ninth");												// scale to ninth
			// to light
		if (f == 1 && t == 2 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 4 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 6 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 1 && t == 8 && fl && tl) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}

	///// second \\\\\


		// from dark second
			// to dark
		if (f == 2 && t == 0 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 1 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 3 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 4 && !fl && !tl) ScaleTo (true, "first", "zero");												// scale to zero
		else if (f == 2 && t == 5 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 7 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 8 && !fl && !tl) ScaleTo (false, "first", "seventh");											// scale to seventh
		else if (f == 2 && t == 9 && !fl && !tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light
		if (f == 2 && t == 0 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 1 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 2 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
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
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 5 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 6 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 7 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 8 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 9 && !fl && tl) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}

		// from light second
			// to dark
		if (f == 2 && t == 0 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 1 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 2 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 3 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 4 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 5 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 6 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 7 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 8 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 2 && t == 9 && fl && !tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light
		if (f == 2 && t == 0 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 1 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 3 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 5 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 7 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 2 && t == 8 && fl && tl) ScaleTo (false, "zero", "first");												// scale to first
		else if (f == 2 && t == 9 && fl && tl) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		

	///// third \\\\\


		// from dark third
			// to dark circle
		if (f == 3 && t == 0 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "zero");										// scale to zero
		else if (f == 3 && t == 1 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 2 && !fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && !fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 5 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 6 && !fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 7 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 3 && t == 8 && !fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 9 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 3 && t == 2 && !fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && !fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 6 && !fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 8 && !fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light triangle
		if (f == 3 && t == 5 && !fl && tl && ts == 1) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light square
		if (f == 3 && t == 5 && !fl && tl && ts == 2) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}

		// from light third
			// to dark circle
		if (f == 3 && t == 0 && fl && !tl && ts == 0) ScaleTo (false, "hidden", "zero");										// scale to zero
		else if (f == 3 && t == 1 && fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 2 && fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 5 && fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");									// scale to first
		else if (f == 3 && t == 6 && fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 7 && fl && !tl && ts == 0) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 3 && t == 8 && fl && !tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 9 && fl && !tl && ts == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 3 && t == 2 && fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 4 && fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 6 && fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 8 && fl && tl && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light triangle
		if (f == 3 && t == 5 && fl && tl && ts == 1) changeShape = true;														// set change shape flag
			// to light square
		if (f == 3 && t == 5 && fl && tl && ts == 2) changeShape = true;														// set change shape flag


	///// fourth \\\\\


		// from dark fourth
			// to dark circle
		if (f == 4 && t == 0 && !fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 1 && !fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 2 && !fl && !tl && ts == 0) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 4 && t == 3 && !fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 5 && !fl && !tl && ts == 0) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 6 && !fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && !fl && !tl && ts == 0) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 8 && !fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && !fl && !tl && ts == 0) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 4 && t == 0 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 1 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 2 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 3 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 4 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 5 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 6 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 8 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && !fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}

		// from light fourth
			// to dark circle
		if (f == 4 && t == 0 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 1 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 2 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 3 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 4 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 5 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 6 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 8 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && !tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 4 && t == 0 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 1 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 3 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 5 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 6 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 8 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && tl && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to dark triangle
		if (f == 4 && t == 6 && fl && !tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && !tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 8 && fl && !tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && !tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
			// to dark square
		if (f == 4 && t == 6 && fl && !tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && !tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 8 && fl && !tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && !tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
			// to light triangle
		if (f == 4 && t == 5 && fl && tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 7 && fl && tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 8 && fl && tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && tl && ts == 1) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
			// to light square
		if (f == 4 && t == 5 && fl && tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 7 && fl && tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 4 && t == 8 && fl && tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && tl && ts == 2) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}


	///// fifth \\\\\


		// from dark circle fifth
			// to dark circle
		if (f == 5 && t == 0 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 5 && t == 2 && !fl && !tl && fs == 0 && ts == 0) changeColour = true;										// set change colour flag
		else if (f == 5 && t == 3 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 4 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 6 && !fl && !tl && fs == 0 && ts == 0) changeColour = true;										// set change colour flag
		else if (f == 5 && t == 7 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 8 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (false, "first", "seventh");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 5 && t == 9 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to light circle
		if (f == 5 && t == 0 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");								// scale to hidden
		else if (f == 5 && t == 1 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 2 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 3 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 4 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 5 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 6 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 7 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 8 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden

		// from light circle fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "zero");								// scale to zero
		else if (f == 5 && t == 1 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "zero");						// scale to zero
		else if (f == 5 && t == 2 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 4 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 5 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 5 && t == 6 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 7 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "seventh");						// scale to seventh
		else if (f == 5 && t == 8 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "ninth");						// scale to ninth
			// to light circle
		if (f == 5 && t == 2 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 4 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 6 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 7 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 8 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");							// scale to ninth

		// from light triangle/square fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 1 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {											
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 2 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 3 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 5 && t == 4 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 5 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 6 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 7 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 8 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light circle
		if (f == 5 && t == 0 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;								// set change shape flag
		else if (f == 5 && t == 1 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 5 && t == 2 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 3 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 5 && t == 4 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 5 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		} 
		else if (f == 5 && t == 6 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 7 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 5 && t == 8 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 5 && t == 9 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 

		// from light triangle fifth
			// to dark triangle	
		if (f == 5 && t == 6 && fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "first");							// scale to first
			// to seventh = no change

		// from light square fifth
			// to dark square
		if (f == 5 && t == 6 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "first");							// scale to first
			// to seventh = no change


	//// sixth \\\\\


		// from dark circle sixth
			// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 1 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 3 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		} 
		else if (f == 6 && t == 4 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 7 && !fl && !tl && fs == 0 && ts == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 8 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && !fl && !tl && fs == 0 && ts == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 1 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 2 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 4 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 6 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 7 && !fl && tl && fs == 0 && ts == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 8 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 9 && !fl && tl && fs == 0 && ts == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}

		// from light circle sixth
			// to dark circle
		if (f == 6 && t == 0 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 1 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 2 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 4 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 6 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 7 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 8 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 9 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 6 && t == 0 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 1 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 3 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
			// fourth = no change
		else if (f == 6 && t == 5 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 7 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 6 && t == 8 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}

		// from dark triangle/square sixth
			// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 1 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 2 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 3 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 4 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 5 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 6 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 7 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 8 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 9 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 1 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 2 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 6 && t == 4 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 5 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		} 
		else if (f == 6 && t == 6 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 7 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 6 && t == 8 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 6 && t == 9 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
	
		// from dark triangle sixth
			// to dark triangle
		if (f == 6 && t == 7 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "first", "hidden");							// scale to hidden
		else if (f == 6 && t == 8 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");						// scale to seventh
			// to light triangle
		if (f == 6 && t == 5 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "first", "hidden");								// scale to hidden
		else if (f == 6 && t == 7 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 6 && t == 8 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");						// scale to seventh

		// from dark square sixth
			// to dark square
		if (f == 6 && t == 7 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "first", "hidden");							// scale to hidden
		else if (f == 6 && t == 8 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");						// scale to seventh
			// to light square
		if (f == 6 && t == 5 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "first", "hidden");								// scale to hidden
		else if (f == 6 && t == 7 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 6 && t == 8 && !fl && tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");						// scale to seventh


	///// seventh \\\\\


		// from dark circle seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "zero");							// scale to zero
		else if (f == 7 && t == 1 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");						// scale to first
		else if (f == 7 && t == 2 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 7 && t == 4 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");						// scale to first
		else if (f == 7 && t == 6 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 8 && !fl && !tl && fs == 0 && ts == 0) changeColour = true;										// set change colour flag
		else if (f == 7 && t == 9 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "seventh", "ninth");						// scale to ninth
			// to light circle
		if (f == 7 && t == 0 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		else if (f == 7 && t == 1 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 7 && t == 2 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 7 && t == 4 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 7 && t == 6 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 7 && t == 8 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");						// scale to hidden

		// from light circle seventh
			// to dark circle
		if (f == 7 && t == 0 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "hidden", "zero");								// scale to zero
		else if (f == 7 && t == 1 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "hidden", "first");						// scale to first
		else if (f == 7 && t == 2 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 4 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "hidden", "first");						// scale to first
		else if (f == 7 && t == 6 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "seventh");						// scale to seventh
		else if (f == 7 && t == 8 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "ninth");						// scale to ninth
			// to light circle
		if (f == 7 && t == 2 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 4 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 6 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 8 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}

		// from dark triangle/square seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 1 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 2 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 4 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 6 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 8 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 7 && t == 0 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;								// set change shape flag
		else if (f == 7 && t == 1 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 2 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 4 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 7 && t == 6 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 7 && t == 8 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}

		// from light triangle/square seventh
			// to dark circle
		if (f == 7 && t == 0 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 1 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 2 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 4 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 6 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 8 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 7 && t == 0 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;								// set change shape flag
		else if (f == 7 && t == 1 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 2 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 3 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 4 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 5 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 6 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 7 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 7 && t == 8 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 7 && t == 9 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag

		// from dark triangle seventh
			// to dark triangle
		if (f == 7 && t == 6 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "first");							// scale to first
		else if (f == 7 && t == 8 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");					// scale to seventh
			// ninth = no change
			// to light triangle
		// fifth = no change
		if (f == 7 && t == 8 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");							// scale to seventh
			// ninth = no change

		// from light triangle seventh
			// to dark triangle
		if (f == 7 && t == 6 && fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "first");							// scale to first
		else if (f == 7 && t == 8 && fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");						// scale to seventh
			// to light triangle
		if (f == 7 && t == 8 && fl && tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");							// scale to seventh

		// from dark square seventh
			// to dark square
		if (f == 7 && t == 6 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "first");							// scale to first
		else if (f == 7 && t == 8 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");					// scale to seventh
			// ninth = no change
			// to light square
			// fifth = no change
		if (f == 7 && t == 8 && !fl && tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");							// scale to seventh
			// ninth = no change

		// from light square seventh
			// to dark square
		if (f == 7 && t == 6 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "first");							// scale to first
		else if (f == 7 && t == 8 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");						// scale to seventh
			// ninth = no change
			// to light square
		if (f == 7 && t == 8 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");							// scale to seventh
			// ninth = no change


	//// eighth \\\\\


		// from dark circle eighth
			// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 8 && t == 2 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");						// scale to first
		else if (f == 8 && t == 3 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "zero");						// scale to first
		else if (f == 8 && t == 5 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 6 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");						// scale to first
		else if (f == 8 && t == 7 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && !fl && !tl && fs == 0 && ts == 0) {			
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 1 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 2 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 6 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 8 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && !fl && tl && fs == 0 && ts == 0) {		
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}

		// from light circle eighth
			// to dark circle
		if (f == 8 && t == 0 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 6 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 8 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 1 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 2 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");							// scale to zero
		else if (f == 8 && t == 3 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 4 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 6 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");							// scale to zero
		else if (f == 8 && t == 7 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 9 && fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeColour = true;																								// set change colour flag
		}

		// from dark triangle/square eighth
			// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 8 && t == 3 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 6 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 8 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 1 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 2 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 6 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}
		else if (f == 8 && t == 8 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
		}

		// from light triangle/square eighth
			// to dark circle
		if (f == 8 && t == 0 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 1 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 2 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 6 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 8 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 8 && t == 0 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 1 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 2 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 3 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 4 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 5 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 6 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 7 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}
		else if (f == 8 && t == 8 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 8 && t == 9 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																				// scale to hidden
			changeShape = true;																									// set change shape flag
		}

		// from dark triangle eighth
			// to dark triangle
		if (f == 8 && t == 6 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "first");							// scale to first
		else if (f == 8 && t == 7 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		// to light triangle
		if (f == 8 && t == 5 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		else if (f == 8 && t == 7 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden

		// from light triangle eighth
			// to dark triangle
		if (f == 8 && t == 6 && fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "first");							// scale to first
		else if (f == 8 && t == 7 && fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
			// to light triangle
		if (f == 8 && t == 5 && fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		else if (f == 8 && t == 7 && fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "hidden");						// scale to hidden

		// from dark square eighth
			// to dark square
		if (f == 8 && t == 6 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "first");							// scale to first
		else if (f == 8 && t == 7 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
			// to light square
		if (f == 8 && t == 5 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		else if (f == 8 && t == 7 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden

		// from light square eighth
			// to dark square
		if (f == 8 && t == 6 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "first");							// scale to first
		else if (f == 8 && t == 7 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
			// to light square
		if (f == 8 && t == 5 && fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");							// scale to hidden
		else if (f == 8 && t == 7 && fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden
		else if (f == 8 && t == 9 && fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "hidden");						// scale to hidden


	///// ninth \\\\\


		// from dark circle ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "zero");								// scale to hidden
		else if (f == 9 && t == 1 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");						// scale to hidden
		else if (f == 9 && t == 2 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden
		else if (f == 9 && t == 4 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");						// scale to hidden
		else if (f == 9 && t == 6 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");						// scale to hidden
		else if (f == 9 && t == 8 && !fl && !tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 10 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "tenth");						// scale to tenth
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");								// scale to hidden
		else if (f == 9 && t == 1 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden
		else if (f == 9 && t == 2 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden
		else if (f == 9 && t == 4 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to hidden
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden
		else if (f == 9 && t == 6 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden
		else if (f == 9 && t == 8 && !fl && tl && fs == 0 && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																					// scale to first
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 10 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden

		// from light circle ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "zero");								// scale to zero
		else if (f == 9 && t == 1 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 9 && t == 2 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 4 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 9 && t == 6 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 10 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "hidden", "tenth");						// scale to tenth
			// to light circle 
		if (f == 9 && t == 2 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 4 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 6 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 8 && fl && tl && fs == 0 && ts == 0) {
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
			// tenth (no change)

		// from dark triangle/square ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 1 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 2 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 4 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 6 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 8 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 9 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;								// set change shape flag
		else if (f == 9 && t == 1 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 2 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 9 && t == 3 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 4 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 9 && t == 5 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 6 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 8 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 9 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag

		// from light triangle/square ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 1 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 2 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 3 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 4 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 6 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 8 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 9 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;								// set change shape flag
		else if (f == 9 && t == 1 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 2 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		} 
		else if (f == 9 && t == 3 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 4 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 5 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 6 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 7 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag
		else if (f == 9 && t == 8 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			changeShape = true;																									// set change shape flag
			changeColour = true;																								// set change colour flag
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 9 && t == 9 && fl && tl && (fs == 1 || fs == 2) && ts == 0) changeShape = true;							// set change shape flag

		// from dark triangle ninth
			// to dark triangle
		if (f == 9 && t == 6 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 9 && t == 8 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 9 && t == 10 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light triangle 
		if (f == 9 && t == 8 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh	
			// tenth (no nucleus change)

		// from light triangle ninth
			// to dark triangle
		if (f == 9 && t == 6 && fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 9 && t == 8 && fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light triangle
		if (f == 9 && t == 8 && fl && tl && fs == 1 && ts == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh	
			// tenth (no nucleus change)

		// from dark square ninth
			// to dark square
		if (f == 9 && t == 6 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "first");										// scale to first
		else if (f == 9 && t == 8 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 9 && t == 10 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light square
		if (f == 9 && t == 8 && !fl && tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");										// scale to seventh	
			// tenth (no nucleus change)

		// from light square ninth
			// to dark square
		if (f == 9 && t == 6 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "hidden", "first");											// scale to first
		else if (f == 9 && t == 8 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "ninth", "tenth");									// scale to tenth
			// to light square
		else if (f == 9 && t == 8 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "hidden", "seventh");									// scale to seventh	
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
		else if (f == 5 && t == 10 && !fl && fs == 0) ScaleTo (true, "first", "hidden");										// scale to hidden
		// from sixth
		else if (f == 6 && t == 10 && !fl && fs == 0) ScaleTo (true, "first", "hidden");										// scale to hidden
		else if (f == 6 && t == 10 && fl && fs == 0) ScaleTo (true, "zero", "hidden");											// scale to hidden
		else if (f == 6 && t == 10 && fs != 0) ScaleTo (true, "first", "hidden");												// scale to hidden
		// from seventh
		else if (f == 7 && t == 10 && !fl && fs == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		// from eighth
		else if (f == 8 && t == 10 && !fl && fs == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 10 && fl && fs == 0) ScaleTo (true, "first", "hidden");											// scale to hidden
		else if (f == 8 && t == 10 && fs != 0) ScaleTo (true, "seventh", "hidden");												// scale to hidden
		// from ninth
		else if (f == 9 && t == 10 && !fl && fs == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
	
	}

	///<summary>
	///<para>0 = circle</para>
	///<para>1 = triangle</para>
	///<para>2 = square</para>
	///<para>3 = ring</para>
	///</summary>
	private void SetShape(int ts)
	{
		if (ts == 0) GetComponent<MeshFilter>().mesh = sphere;																	// change mesh to sphere
		else if (ts == 1) GetComponent<MeshFilter>().mesh = triangle;															// change mesh to triangle
		else if (ts == 2) GetComponent<MeshFilter>().mesh = square;																// change mesh to square
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
		// in/to dark world
			// to dark
		if (!l && (!psp.inLightworld || psp.toDarkworld)) {
			// while dark world
			if (!psp.psp.lightworld) {
				if (toState != 0 && (toState % 2 == 0))
					rend.material.shader = lightShader;																			// if even # state (not zero), change to white shader
				else if ((toState == 6) && (toShape != 0)) {																	// else if, sixth	 state triangle/square
					rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
					anim.SetBool ("white", false);																					// reset white
					anim.SetBool ("black", true);																					// set black
				}
				else {																											// else, odd # states
					rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
					anim.SetBool ("white", false);																					// reset white
					anim.SetBool ("black", true);																					// set black
				}
			}
			// while light world
			else if (psp.psp.lightworld) {
				rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
				anim.SetBool ("black", false);																					// reset black
				anim.SetBool ("white", true);																					// set white
			}
		}
			// to light
		else if (l && (!psp.inLightworld || psp.toDarkworld)) {
			// while dark world
			if (!psp.psp.lightworld) {
				if (toState != 0 && (toState % 2 == 0)) rend.material.shader = darkShader;										// if even # state, change to black shader
				else if ((toState == 8) && (toShape != 0)) {																	// else if, eighth state triangle/square
					rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
					anim.SetBool ("white", false);																					// reset white
					anim.SetBool ("black", true);																					// set black
				}
			}
			// while light world
			else if (psp.psp.lightworld) {	
				rend.material.shader = Shader.Find("Unlit/Color");																// reset shader
				anim.SetBool("black", false);																					// reset black
				anim.SetBool("white", true);																					// set white (keep hidden)
			}
		}

		// in/to light world
			// to dark
		if (!l && (psp.inLightworld || psp.toLightworld)) {
			// while dark world
			if (!psp.psp.lightworld) {
				rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
				anim.SetBool ("white", false);																					// reset white
				anim.SetBool ("black", true);																					// set black
			}
			// while light world
			else if (psp.psp.lightworld) {
				if (toState != 0 && (toState % 2 == 0))
					rend.material.shader = darkShader;																			// if even # state, change to black shader
				else {																											// else, odd # state
					rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
					anim.SetBool ("white", false);																					// reset white
					anim.SetBool ("black", true);																					// set black
				}
			}
		}
			// to light
		else if (l && (psp.inLightworld || psp.toLightworld)) {
			// while dark world
			if (!psp.psp.lightworld) {
				rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
				anim.SetBool ("white", false);																					// reset black
				anim.SetBool ("black", true);																					// set white
			}
			// while light world
			else if (psp.psp.lightworld) {
				if (toState != 0 && (toState % 2 == 0))
					rend.material.shader = lightShader;																			// if even # state, change to white shader
				else {																											// else, odd # state
					rend.material.shader = Shader.Find ("Unlit/Color");																// reset shader
					anim.SetBool ("black", false);																					// reset previously active state
					anim.SetBool ("white", true);																					// set active state
				}
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
