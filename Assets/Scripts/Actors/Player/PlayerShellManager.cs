using UnityEngine;
using System.Collections;

public class PlayerShellManager : MonoBehaviour {

	private Animator anim;						// animator on core

	//private MeshRenderer rend;																									// mesh renderer (for colour changes)
	//private PlayerStatePattern psp;

	private int toState;																										// to state indicator, shape index
	//private bool toLight, colour; 																								// to light indicator, colour indicator
	private bool resetScale = false; //, changeColour = false;																	// change shape timer, reset scale timer	// timer trigger for changing shape, resetting scale after world switch
	private float resetScaleTimer; //, changeColourTimer;

	//private Shader lightShader, darkShader;																						// light/dark shaders

	void Start () {
		anim = GetComponent<Animator>();																						// init animator ref
		//rend = GetComponent<MeshRenderer>();																					// init mesh renderer ref
		//psp = GameObject.Find ("Player")
		//	.gameObject.GetComponent<PlayerStatePattern> ();																	// init psp ref
		//lightShader = Shader.Find("Unlit/light_nucleus");																		// init light nucleus shader
		//darkShader = Shader.Find("Unlit/light_nucleus");																		// init dark nucleus shader
	}

	void Update() {
		// change colour timer
		/*if (changeColour) changeColourTimer += Time.deltaTime;																	// start timer
		if (changeColourTimer >= 2.0f) {																						// when timer >= 2 sec
			Debug.Log("set colour: " + colour);
			SetLight(colour);																										// set colour to dark
			changeColour = false;																									// reset change colour flag
			changeColourTimer = 0f;																									// reset timer
		}*/
		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;																		// start timer
		if (resetScaleTimer >= 4.0f) {																							// when timer >= 4 sec
			//anim.ResetTrigger("colour");	
			if (toState == 3 || toState == 4 || toState == 5 || toState == 6) 													// if to third/fourth/fifth/sixth
				ScaleTo (false, "hidden", "third");																					// grow to first
			if (toState == 7 || toState == 8) 																					// if to seventh/eighth
				ScaleTo (false, "hidden", "seventh");																				// grow to seventh
			if (toState == 9) 																									// if to ninth
				ScaleTo (false, "hidden", "ninth");																					// grow to ninth
			resetScale = false;																									// reset reset scale flag
			resetScaleTimer = 0f;																								// reset timer
		}
	}

	public void Shell (int f, int t, bool fl, bool tl, int s) 
	{
		// set up
		toState = t;

// EVOLUTIONS \\


	///// zero \\\\\


		// from dark zero (0.5)
			// to dark
		if (f == 0 && t == 3 && !fl && !tl) ScaleTo (false, "hidden", "third");													// scale to third
		else if (f == 0 && t == 4 && !fl && !tl) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 0 && t == 5 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 6 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 7 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 8 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 9 && !fl && !tl) resetScale = true;																// set reset scale flag
			// to light
		else if (f == 0 && t == 5 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 6 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 7 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 8 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 9 && !fl && tl) resetScale = true;																// set reset scale flag
		// from light zero (0.5)
			// to dark
		if (f == 0 && t == 3 && fl && !tl) ScaleTo (false, "hidden", "third");													// scale to third
		else if (f == 0 && t == 4 && fl && !tl) ScaleTo (false, "hidden", "third");												// scale to third
		else if (f == 0 && t == 5 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 6 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 7 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 8 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 9 && fl && !tl) resetScale = true;																// set reset scale flag
		// to light
		else if (f == 0 && t == 5 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 6 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 7 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 8 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 0 && t == 9 && fl && tl) resetScale = true;																// set reset scale flag


	///// first \\\\\


		// from dark first
			// to dark
		if (f == 1 && t == 3 && !fl && !tl) ScaleTo (false, "hidden", "third");													// scale to third
		else if (f == 1 && t == 4 && !fl && !tl) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 1 && t == 5 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 6 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 7 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 8 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 9 && !fl && !tl) resetScale = true;																// set reset scale flag
			// to light
		if (f == 1 && t == 5 && !fl && tl) resetScale = true;																	// set reset scale flag
		else if (f == 1 && t == 6 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 7 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 8 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 9 && !fl && tl) resetScale = true;																// set reset scale flag
		// from light first
			// to dark
		if (f == 1 && t == 3 && fl && !tl) ScaleTo (false, "hidden", "third");													// scale to third
		else if (f == 1 && t == 4 && fl && !tl) ScaleTo (false, "hidden", "third");												// scale to third
		else if (f == 1 && t == 5 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 6 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 7 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 8 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 9 && fl && !tl) resetScale = true;																// set reset scale flag
			// to light
		if (f == 1 && t == 5 && fl && tl) resetScale = true;																	// set reset scale flag
		else if (f == 1 && t == 6 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 7 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 8 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 1 && t == 9 && fl && tl) resetScale = true;																// set reset scale flag


	///// second \\\\\


		// from dark second
			// to dark
		if (f == 2 && t == 3 && !fl && !tl) ScaleTo (false, "hidden", "third");													// scale to third
		else if (f == 2 && t == 4 && !fl && !tl) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 2 && t == 5 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 6 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 7 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 8 && !fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 9 && !fl && !tl) resetScale = true;																// set reset scale flag
			// to light
		if (f == 2 && t == 5 && !fl && tl) resetScale = true;																	// set reset scale flag
		else if (f == 2 && t == 6 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 7 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 8 && !fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 9 && !fl && tl) resetScale = true;																// set reset scale flag
		// from light second
			// to dark
		if (f == 2 && t == 3 && fl && !tl) ScaleTo (false, "hidden", "third");													// scale to third
		else if (f == 2 && t == 4 && fl && !tl) ScaleTo (false, "hidden", "third");												// scale to third
		else if (f == 2 && t == 5 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 6 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 7 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 8 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 9 && fl && !tl) resetScale = true;																// set reset scale flag
			// to light
		if (f == 2 && t == 5 && fl && tl) resetScale = true;																	// set reset scale flag
		else if (f == 2 && t == 6 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 7 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 8 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 2 && t == 9 && fl && tl) resetScale = true;																// set reset scale flag


	///// third \\\\\

		// from dark third
			// to dark
		if (f == 3 && t == 0 && !fl && !tl) ScaleTo (true, "third", "hidden");													// scale to hidden
		else if (f == 3 && t == 1 && !fl && !tl) ScaleTo (true, "third", "hidden");												// scale to hidden
		else if (f == 3 && t == 2 && !fl && !tl) ScaleTo (true, "third", "hidden");												// scale to hidden
		else if (f == 3 && t == 5 && !fl && !tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 6 && !fl && !tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 7 && !fl && !tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 8 && !fl && !tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 9 && !fl && !tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
			// to light
		if (f == 3 && t == 0 && !fl && tl) ScaleTo (true, "third", "hidden");													// scale to hidden
		else if (f == 3 && t == 1 && !fl && tl) ScaleTo (true, "third", "hidden");												// scale to hidden
		else if (f == 3 && t == 2 && !fl && tl) ScaleTo (true, "third", "hidden");												// scale to hidden
		else if (f == 3 && t == 3 && !fl && tl) ScaleTo (true, "third", "hidden");												// scale to hidden
		else if (f == 3 && t == 4 && !fl && tl) ScaleTo (true, "third", "hidden");												// scale to hidden
		else if (f == 3 && t == 5 && !fl && tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 6 && !fl && tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 7 && !fl && tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 8 && !fl && tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 3 && t == 9 && !fl && tl) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}

		// from light third
			// to dark
		if (f == 3 && t == 3 && fl && !tl) ScaleTo (false, "hidden", "third");													// scale to third
		else if (f == 3 && t == 4 && fl && !tl) ScaleTo (false, "hidden", "third");												// scale to third
		else if (f == 3 && t == 5 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 3 && t == 6 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 3 && t == 7 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 3 && t == 8 && fl && !tl) resetScale = true;																// set reset scale flag
		else if (f == 3 && t == 9 && fl && !tl) resetScale = true;																// set reset scale flag
			// to light
		if (f == 3 && t == 5 && fl && tl) resetScale = true;																	// set reset scale flag
		else if (f == 3 && t == 6 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 3 && t == 7 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 3 && t == 8 && fl && tl) resetScale = true;																// set reset scale flag
		else if (f == 3 && t == 9 && fl && tl) resetScale = true;																// set reset scale flag


	///// fourth \\\\\


		// from dark fourth
			// to dark circle
		if (f == 4 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 4 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 4 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 4 && t == 5 && !fl && !tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 6 && !fl && !tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && !fl && !tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 8 && !fl && !tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && !fl && !tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
			// to light circle
		if (f == 4 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");											// scale to hidden
		else if (f == 4 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 4 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 4 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 4 && t == 5 && !fl && tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 6 && !fl && tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 7 && !fl && tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 8 && !fl && tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}
		else if (f == 4 && t == 9 && !fl && tl && s == 0) {
			ScaleTo (true, "third", "hidden");																					// scale to hidden
			resetScale = true;																									// set reset scale flag
		}

		// from light fourth
			// to dark circle
		if (f == 4 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 4 && t == 4 && fl && !tl && s == 0) resetScale = true;													// set reset scale flag
		else if (f == 4 && t == 5 && fl && !tl && s == 0) resetScale = true;													// set reset scale flag
		else if (f == 4 && t == 6 && fl && !tl && s == 0) resetScale = true;													// set reset scale flag
		// to light circle
		if (f == 4 && t == 5 && fl && tl && s == 0) resetScale = true;															// set reset scale flag
		else if (f == 4 && t == 6 && fl && tl && s == 0) resetScale = true;														// set reset scale flag
			// to dark triangle
		//if (f == 4 && t == 7 && fl && !tl && s == 1) resetScale = true;															// set reset scale flag
		//else if (f == 4 && t == 8 && fl && !tl && s == 1) resetScale = true;														// set reset scale flag
		//else if (f == 4 && t == 9 && fl && !tl && s == 1) resetScale = true;														// set reset scale flag
			// to dark square
		//if (f == 4 && t == 7 && fl && !tl && s == 2) resetScale = true;															// set reset scale flag
		//else if (f == 4 && t == 8 && fl && !tl && s == 2) resetScale = true;														// set reset scale flag
		//else if (f == 4 && t == 9 && fl && !tl && s == 2) resetScale = true;														// set reset scale flag


	///// fifth \\\\\


		// from dark fifth
			// to dark circle
		if (f == 5 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 5 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 8 && !fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "third", "ninth");									// scale to ninth
			// to light circle
		if (f == 5 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");											// scale to hidden
		else if (f == 5 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 7 && !fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 8 && !fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "third", "ninth");									// scale to ninth

		// from light fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");											// scale to hidden
		else if (f == 5 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 5 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 8 && fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "third", "ninth");									// scale to ninth
			// to light circle
		if (f == 5 && t == 0 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");											// scale to hidden
		else if (f == 5 && t == 1 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 5 && t == 2 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 5 && t == 3 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 5 && t == 4 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 5 && t == 7 && fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 8 && fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 5 && t == 9 && fl && tl && s == 0) ScaleTo (false, "third", "ninth");										// scale to ninth

		// from light triangle
			// to dark circle
		if (f == 5 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 5 && t == 4 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 6 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
			// to light circle
		if (f == 5 && t == 5 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 5 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 7 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
			// to dark triangle
		if (f == 5 && t == 7 && fl && !tl && s == 1) resetScale = true;															// set reset scale flag
		//else if (f == 5 && t == 8 && fl && !tl && s == 1) resetScale = true;														// set reset scale flag
		//else if (f == 5 && t == 9 && fl && !tl && s == 1) resetScale = true;														// set reset scale flag

		// from light square
			// to dark circle
		if (f == 5 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 5 && t == 4 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 6 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
			// to light circle
		if (f == 5 && t == 5 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 5 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 5 && t == 7 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
			// to dark square
		if (f == 5 && t == 7 && fl && !tl && s == 2) resetScale = true;															// set reset scale flag
		//else if (f == 5 && t == 8 && fl && !tl && s == 2) resetScale = true;														// set reset scale flag
		//else if (f == 5 && t == 9 && fl && !tl && s == 2) resetScale = true;														// set reset scale flag


	///// sixth \\\\\


		// from dark circle sixth
			// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 6 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 8 && !fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "third", "ninth");									// scale to ninth
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");											// scale to hidden
		else if (f == 6 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 7 && !fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 8 && !fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "third", "ninth");									// scale to ninth

		// from light circle sixth
			// to dark circle
		if (f == 6 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");											// scale to hidden
		else if (f == 6 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "third", "hidden");									// scale to hidden
		else if (f == 6 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 8 && fl && !tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "third", "ninth");									// scale to ninth
			// to light circle
		if (f == 6 && t == 0 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");											// scale to hidden
		else if (f == 6 && t == 1 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 6 && t == 2 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 6 && t == 3 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 6 && t == 4 && fl && tl && s == 0) ScaleTo (true, "third", "hidden");										// scale to hidden
		else if (f == 6 && t == 7 && fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 8 && fl && tl && s == 0) ScaleTo (false, "third", "seventh");									// scale to seventh
		else if (f == 6 && t == 9 && fl && tl && s == 0) ScaleTo (false, "third", "ninth");										// scale to ninth

		// from dark triangle sixth
			// to dark circle
		if (f == 6 && t == 3 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 6 && t == 4 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 5 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 6 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 6 && t == 8 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");								// scale to seventh
			// to light circle
		if (f == 6 && t == 5 && !fl && tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 6 && t == 6 && !fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 7 && !fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 6 && t == 8 && !fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
			// to dark triangle
		if (f == 6 && t == 7 && fl && !tl && s == 1) resetScale = true;															// set reset scale flag
		else if (f == 6 && t == 8 && fl && !tl && s == 1) resetScale = true;													// set reset scale flag
		//else if (f == 6 && t == 9 && fl && !tl && s == 1) resetScale = true;													// set reset scale flag

		// from dark square sixth
			// to dark circle
		if (f == 6 && t == 3 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 6 && t == 4 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 5 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 6 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 7 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");								// scale to seventh
		else if (f == 6 && t == 8 && !fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");								// scale to seventh
			// to light circle
		if (f == 6 && t == 5 && !fl && tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 6 && t == 6 && !fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 6 && t == 7 && !fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 6 && t == 8 && !fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
			// to dark square
		if (f == 6 && t == 7 && fl && !tl && s == 2) resetScale = true;															// set reset scale flag
		else if (f == 6 && t == 8 && fl && !tl && s == 2) resetScale = true;													// set reset scale flag
		//else if (f == 6 && t == 9 && fl && !tl && s == 2) resetScale = true;														// set reset scale flag


	///// seventh \\\\\


		// from dark circle seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle
		if (f == 7  && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth

		// from light circle seventh
			// to dark circle
		if (f == 7 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle
		if (f == 7 && t == 0 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 4 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 5 && fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 9 && fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth

		// from dark triangle seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
			// seventh/eighth = no change
		else if (f == 7 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
		// to light circle
		if (f == 7 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
			// seventh/eighth = no change
		else if (f == 7 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to dark triangle
		if (f == 7 && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 9 && !fl && !tl && s == 1) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light triangle
		if (f == 7 && t == 5 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 8 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 9 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
			
		// from light triangle seventh
			// to dark circle
		if (f == 7 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 7 && t == 4 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 6 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 8 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 7 && t == 5 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 7 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 7 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 8 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 9 && fl && tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to dark triangle
		if (f == 7 && t == 8 && fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		else if (f == 7 && t == 9 && fl && !tl && s == 1) ScaleTo (false, "hidden", "ninth");									// scale to ninth

		// from dark square seventh
			// to dark circle
		if (f == 7 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
			// seventh/eighth = no change
		else if (f == 7 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle
		if (f == 7 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 7 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
			// seventh/eighth = no change
		else if (f == 7 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to dark square
		if (f == 7 && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 9 && !fl && !tl && s == 2) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light square
		if (f == 7 && t == 5 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 7 && t == 8 && !fl && tl && s == 2) ScaleTo (false, "seventh", "hidden");									// scale to hidden
		else if (f == 7 && t == 9 && !fl && tl && s == 2) ScaleTo (false, "seventh", "hidden");									// scale to hidden

		// from light square seventh
			// to dark circle
		if (f == 7 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 7 && t == 4 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 6 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 8 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 7 && t == 5 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 7 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 7 && t == 7 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 8 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 7 && t == 9 && fl && tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to dark square
		if (f == 7 && t == 8 && fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		else if (f == 7 && t == 9 && fl && !tl && s == 2) ScaleTo (false, "hidden", "ninth");									// scale to ninth


	///// eighth \\\\\


		// from dark circle eighth
			// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth

		// from light circle eighth
			// to dark circle ninth
		if (f == 8 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle ninth
		if (f == 8 && t == 0 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 4 && fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 5 && fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 9 && fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth

		// from dark triangle eighth
			// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to nint
			// to dark triangle
		if (f == 8 && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 9 && !fl && !tl && s == 1) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light triangle
		if (f == 8 && t == 5 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && !fl && tl && s == 1) ScaleTo (true, "seventh", "hidden");									// scale to hidden

		// from light triangle eighth
			// to dark circle
		if (f == 8 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 8 && t == 4 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 6 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 8 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 8 && t == 5 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 8 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 7 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 8 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 9 && fl && tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to dark triangle
		if (f == 8 && t == 7 && fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		else if (f == 8 && t == 9 && fl && !tl && s == 1) ScaleTo (false, "hidden", "ninth");									// scale to ninth

		// from dark square eighth
			// to dark circle
		if (f == 8 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
			// seventh/eighth = no change
		else if (f == 8 && t == 9 && !fl && !tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light circle
		if (f == 8 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "seventh", "third");									// scale to third
		else if (f == 8 && t == 9 && !fl && tl && s == 0) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to dark square
		if (f == 8 && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 9 && !fl && !tl && s == 2) ScaleTo (false, "seventh", "ninth");									// scale to ninth
			// to light square
		if (f == 8 && t == 5 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");										// scale to hidden
		else if (f == 8 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden
		else if (f == 8 && t == 9 && !fl && tl && s == 2) ScaleTo (true, "seventh", "hidden");									// scale to hidden

		// from light square eighth
			// to dark circle
		if (f == 8 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 8 && t == 4 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 6 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 8 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 8 && t == 5 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 8 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 8 && t == 7 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 8 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 8 && t == 9 && fl && tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to dark square
		if (f == 8 && t == 7 && fl && !tl && s == 2) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		else if (f == 8 && t == 9 && fl && !tl && s == 2) ScaleTo (false, "hidden", "ninth");									// scale to ninth


	///// ninth \\\\\


		// from dark circle ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden

		// from light circle ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 1 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 6 && fl && !tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 7 && fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
			// to light circle
		if (f == 9 && t == 0 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 1 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 2 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 3 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 4 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 5 && fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 6 && fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 7 && fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden

		// from dark triangle ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
			// to dark triangle
		if (f == 9 && t == 6 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 7 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && !fl && !tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden
			// to light triangle
		if (f == 9 && t == 5 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 7 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 8 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 10 && !fl && tl && s == 1) ScaleTo (true, "ninth", "hidden");									// scale to hidden

		// from light triangle ninth
			// to dark circle
		if (f == 9 && t == 3 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");										// scale to third
		else if (f == 9 && t == 4 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 9 && t == 5 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 9 && t == 6 && fl && !tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 9 && t == 7 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 9 && fl && !tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 9 && t == 5 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");											// scale to third
		else if (f == 9 && t == 6 && fl && tl && s == 0) ScaleTo (false, "hidden", "third");									// scale to third
		else if (f == 9 && t == 7 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && fl && tl && s == 0) ScaleTo (false, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 9 && fl && tl && s == 0) ScaleTo (false, "hidden", "ninth");									// scale to ninth
			// to dark triangle
		if (f == 9 && t == 7 && fl && !tl && s == 1) ScaleTo (false, "hidden", "seventh");										// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && s == 1) ScaleTo (true, "hidden", "seventh");									// scale to seventh

		// from dark square ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 1 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 3 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 4 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 5 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 6 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "third");									// scale to third
		else if (f == 9 && t == 7 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 1 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 2 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 3 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 4 && !fl && tl && s == 0) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 5 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 6 && !fl && tl && s == 0) ScaleTo (true, "ninth", "third");										// scale to third
		else if (f == 9 && t == 7 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && tl && s == 0) ScaleTo (true, "ninth", "seventh");									// scale to seventh
			// to dark square
		if (f == 9 && t == 6 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "hidden");										// scale to hidden
		else if (f == 9 && t == 7 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");									// scale to seventh
		else if (f == 9 && t == 10 && !fl && !tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden
			// to light square
		if (f == 9 && t == 5 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");											// scale to hidden
		else if (f == 9 && t == 7 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 8 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden
		else if (f == 9 && t == 10 && !fl && tl && s == 2) ScaleTo (true, "ninth", "hidden");									// scale to hidden

		// from light square ninth
			// to dark circle
		if (f == 9 && t == 3 && fl && !tl && s == 0) ScaleTo (true, "hidden", "third");											// scale to third
		else if (f == 9 && t == 4 && fl && !tl && s == 0) ScaleTo (true, "hidden", "third");									// scale to third
		else if (f == 9 && t == 5 && fl && !tl && s == 0) ScaleTo (true, "hidden", "third");									// scale to third
		else if (f == 9 && t == 6 && fl && !tl && s == 0) ScaleTo (true, "hidden", "third");									// scale to third
		else if (f == 9 && t == 7 && fl && !tl && s == 0) ScaleTo (true, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && s == 0) ScaleTo (true, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 9 && fl && !tl && s == 0) ScaleTo (true, "hidden", "ninth");									// scale to ninth
			// to light circle
		if (f == 9 && t == 5 && fl && tl && s == 0) ScaleTo (true, "hidden", "third");											// scale to third
		else if (f == 9 && t == 6 && fl && tl && s == 0) ScaleTo (true, "hidden", "third");										// scale to third
		else if (f == 9 && t == 7 && fl && tl && s == 0) ScaleTo (true, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 8 && fl && tl && s == 0) ScaleTo (true, "hidden", "seventh");									// scale to seventh
		else if (f == 9 && t == 9 && fl && tl && s == 0) ScaleTo (true, "hidden", "ninth");										// scale to ninth
			// to dark square
		if (f == 9 && t == 7 && fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");										// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && s == 2) ScaleTo (true, "ninth", "seventh");									// scale to seventh

	}

	///<summary>
	///<para>set core as light</para>
	///<para>light: true = white, false = black</para>
	///</summary>
	/*private void SetLight (bool l)
	{
		if (l && !psp.lightworld) {
			rend.material.shader = lightShader;																					// change to white shader
		} 
		else if (!l && !psp.lightworld) {
			if (toState != 0 && toState % 2 == 0) rend.material.shader = darkShader;											// if even # state, change to black shader
			else {
				rend.material.shader = Shader.Find("Unlit/Color");																// change to unlit colour shader
				rend.material.SetColor("_Color", Color.black);																	// change to black
			}
		}
		else if (l && psp.lightworld) {
			rend.material.shader = darkShader;																					// change to black shader
		}
		else if (!l && psp.lightworld) {
			if (toState != 0 && toState % 2 == 0) rend.material.shader = lightShader;											// if even # state, change to white shader
			rend.material.shader = Shader.Find("Unlit/Color");																	// change to unlit colour shader
			rend.material.SetColor("_Color", Color.white);																		// change to white
		}
	}*/

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
