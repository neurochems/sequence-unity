using UnityEngine;
using System.Collections;

public class ParticleCoreManager : MonoBehaviour {

	private Animator anim;																							// animator on core ref
	private MeshRenderer rend;																						// mesh renderer (for colour changes)
	public Mesh sphere, triangle, square;																			// shape meshes
	//private ParticleStatePattern psp;																				// psp ref

	private int toState, fromShape, toShape;																		// to state indicator, from shape/to shape index
	private bool toLight, colour;																					// to light, colour indicator
	private bool resetScale = false, changeColour = true, changeShape = false;										// timer trigger for changing shape, resetting scale after world switch
	private float resetScaleTimer, changeColourTimer, changeShapeTimer;												// change shape timer, reset scale timer

    private Shader lightShader, darkShader;         // light/dark shaders

    void Start () {
		anim = GetComponent<Animator>();																			// init animator ref
		rend = GetComponent<MeshRenderer>();																		// init mesh renderer ref
		//psp = GetComponentInParent<ParticleStatePattern> ();        												// init psp ref

        //lightShader = Shader.Find("light_nucleus");																// init light nucleus shader
        //darkShader = Shader.Find("dark_nucleus");																	// init dark nucleus shader
    }

	void Update() {

		// change colour timer
		if (changeColour) changeColourTimer += Time.deltaTime;														// start timer
		if (changeColourTimer >= 2.0f) {																			// when timer >= 4 sec
			SetLight(toLight);																							// set colour
			changeColour = false;																						// reset reset scale flag
			changeColourTimer = 0f;																						// reset timer
		}

		// change shape timer
		if (changeShape) changeShapeTimer += Time.deltaTime;														// start timer
		if (changeShapeTimer >= 2.0f) {																				// when timer >= 4 sec
			Debug.Log(transform.parent.name + " core set shape: " + toShape);
			SetShape(toShape);																							// set shape
			changeShape = false;																						// reset reset scale flag
			changeShapeTimer = 0f;																						// reset timer
		}

		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;															// start timer
		if (resetScaleTimer >= 2.25f) {																				// when timer >= 4 sec
			if (toState == 0) 																						// if to zero
				ScaleTo (false, "hidden", "zero");																		// grow to zero
			if (toState == 1 || toState == 2 || toState == 5 || toState == 6) 										// if to first/second/fifth/sixth, 
				ScaleTo (false, "hidden", "first");																		// grow to first
			if (toState == 3 || toState == 4) 																		// if to third/fourth
				ScaleTo (false, "hidden", "third");																		// grow to third
			if (toState == 7 || toState == 8) 																		// if to seventh/eighth
				ScaleTo (false, "hidden", "seventh");																	// grow to seventh
			if (toState == 9) 																						// if to ninth
				ScaleTo (false, "hidden", "ninth");																		// grow to ninth
			resetScale = false;																						// reset reset scale flag
			resetScaleTimer = 0f;																					// reset timer
		}
	}

    ///<summary>
    ///<para> method for world changing anims </para>
	///<para> lw = to lightworld (T), dark world (F) </para>
	///<para> f = from state </para>
	///<para> t = to state </para>
	///<para> l = to light (T), to dark (F)</para>
    ///</summary>
	public void ToOtherWorld (bool lw, int f, int t, bool l, int fs, int ts) 
	{
		toState = t;																								// set to state
		toShape = ts;																								// set to shape
		toLight = l;																								// set to light

		if (lw) {																									// if to light world
			// from changes
			if (f == 0) ScaleTo (true, "zero", "hidden");                                                      		// scale from zero
			else if (f == 1 || f == 2 || f == 5 || f == 6) ScaleTo (true, "first", "hidden");    			    	// scale from first
			else if (f == 3 || f == 4) ScaleTo (true, "third", "hidden");            			                    // scale from third
			else if (f == 7 || f == 8) ScaleTo (true, "seventh", "hidden");											// scale from seventh
			else if (f == 9) ScaleTo (true, "ninth", "hidden");														// scale from ninth

			// shape changes
			if ((ts == 0) && (fs != 0)) changeShape = true;															// change to circle
			else if ((ts == 1) && (fs != 1)) changeShape = true;													// change to triangle
			else if ((ts == 2) && (fs != 2)) changeShape = true;													// change to square

			// to changes
			changeColour = true; 																					// start change timer
			if (!l && (t == 3 || t == 4)) resetScale = false;														// don't trigger rescale timer
			else resetScale = true;																					// trigger rescale timer
																	
		}

		else if (!lw) {																								// if to dark world
			// from changes
			if (f == 0) ScaleTo (true, "zero", "hidden");															// scale from zero
			else if (f == 1 || f == 2 || f == 5 || f == 6) ScaleTo (true, "first", "hidden");        				// scale from first
			else if (f == 3 || f == 4) ScaleTo (true, "third", "hidden");											// scale from third
			else if (f == 7 || f == 8) ScaleTo (true, "seventh", "hidden");											// scale from seventh
			else if (f == 9) ScaleTo (true, "ninth", "hidden");														// scale from ninth

			// shape changes
			if ((ts == 0) && (fs != 0)) changeShape = true;															// change to circle
			else if ((ts == 1) && (fs != 1)) changeShape = true;													// change to triangle
			else if ((ts == 2) && (fs != 2)) changeShape = true;													// change to square

			// to changes
			changeColour = true; 																					// start change timer
			if (!l && (t == 3 || t == 4)) resetScale = false;														// don't trigger rescale timer
			else resetScale = true;																					// trigger rescale timer
		}
	}

	public void Core (int f, int t, bool fl, bool tl, int fs, int ts) 
	{
		// set up
		toState = t;																								// set to state
		fromShape = fs;																								// set from shape
		toShape = ts;																								// set to shape


	///// zero \\\\\


		// to zero (init)
		if (f == 0 && t == 0 && fl && tl) ScaleTo (false, "hidden", "zero");										// scale to first
		else if (f == 0 && t == 0 && !fl && !tl) ScaleTo (false, "hidden", "zero");									// scale to first

		// from dark zero (0.5)
			// to dark
		if (f == 0 && t == 1 && !fl && !tl) ScaleTo (false, "zero", "first");										// scale to first
		else if (f == 0 && t == 2 && !fl && !tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 3 && !fl && !tl) ScaleTo (true, "zero", "hidden");									// scale to hidden
		else if (f == 0 && t == 4 && !fl && !tl) ScaleTo (true, "zero", "hidden");									// scale to hidden
		else if (f == 0 && t == 5 && !fl && !tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 6 && !fl && !tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 7 && !fl && !tl) ScaleTo (false, "zero", "seventh");								// scale to seventh
		else if (f == 0 && t == 8 && !fl && !tl) ScaleTo (false, "zero", "seventh");								// scale to seventh
		else if (f == 0 && t == 9 && !fl && !tl) ScaleTo (false, "zero", "ninth");									// scale to ninth
			// to light
		if (f == 0 && t == 1 && !fl && tl) ScaleTo (false, "zero", "first");										// scale to first
		else if (f == 0 && t == 2 && !fl && tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 3 && !fl && tl) ScaleTo (false, "zero", "third");									// scale to third
		else if (f == 0 && t == 4 && !fl && tl) ScaleTo (false, "zero", "third");									// scale to third
		else if (f == 0 && t == 5 && !fl && tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 6 && !fl && tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 7 && !fl && tl) ScaleTo (false, "zero", "seventh");									// scale to seventh
		else if (f == 0 && t == 8 && !fl && tl) ScaleTo (false, "zero", "seventh");									// scale to seventh
		else if (f == 0 && t == 9 && !fl && tl) ScaleTo (false, "zero", "ninth");									// scale to ninth

		// from light zero (0.5)
			// to dark
		if (f == 0 && t == 1 && fl && !tl) ScaleTo (false, "zero", "first");										// scale to first
		else if (f == 0 && t == 2 && fl && !tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 3 && fl && !tl) ScaleTo (true, "zero", "hidden");									// scale to hidden
		else if (f == 0 && t == 4 && fl && !tl) ScaleTo (true, "zero", "hidden");									// scale to hidden
		else if (f == 0 && t == 5 && fl && !tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 6 && fl && !tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 7 && fl && !tl) ScaleTo (false, "zero", "seventh");									// scale to seventh
		else if (f == 0 && t == 8 && fl && !tl) ScaleTo (false, "zero", "seventh");									// scale to seventh
		else if (f == 0 && t == 9 && fl && !tl) ScaleTo (false, "zero", "ninth");									// scale to ninth
			// to light
		if (f == 0 && t == 1 && fl && tl) ScaleTo (false, "zero", "first");											// scale to first
		else if (f == 0 && t == 2 && fl && tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 3 && fl && tl) ScaleTo (false, "zero", "third");									// scale to third
		else if (f == 0 && t == 4 && fl && tl) ScaleTo (false, "zero", "third");									// scale to third
		else if (f == 0 && t == 5 && fl && tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 6 && fl && tl) ScaleTo (false, "zero", "first");									// scale to first
		else if (f == 0 && t == 7 && fl && tl) ScaleTo (false, "zero", "seventh");									// scale to seventh
		else if (f == 0 && t == 8 && fl && tl) ScaleTo (false, "zero", "seventh");									// scale to seventh
		else if (f == 0 && t == 9 && fl && tl) ScaleTo (false, "zero", "ninth");									// scale to ninth


	///// first \\\\\


		// from dark first
			// to dark
		if (f == 1 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");										// scale to zero
		else if (f == 1 && t == 3 && !fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 1 && t == 4 && !fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 1 && t == 7 && !fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 1 && t == 8 && !fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 1 && t == 9 && !fl && !tl) ScaleTo (false, "first", "ninth");									// scale to ninth
			// to light
		if (f == 1 && t == 0 && !fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		else if (f == 1 && t == 3 && !fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 1 && t == 4 && !fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 1 && t == 7 && !fl && tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 1 && t == 8 && !fl && tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 1 && t == 9 && !fl && tl) ScaleTo (false, "first", "ninth");									// scale to ninth

		// from light first
			// to dark
		if (f == 1 && t == 0 && fl && !tl) ScaleTo (true, "first", "zero");											// scale to zero
		else if (f == 1 && t == 3 && fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 1 && t == 4 && fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 1 && t == 7 && fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 1 && t == 8 && fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 1 && t == 9 && fl && !tl) ScaleTo (false, "first", "ninth");									// scale to ninth
			// to light
		if (f == 1 && t == 0 && fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		else if (f == 1 && t == 3 && fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 1 && t == 4 && fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 1 && t == 7 && fl && tl) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 1 && t == 8 && fl && tl) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 1 && t == 9 && fl && tl) ScaleTo (false, "first", "ninth");									// scale to ninth


	///// second \\\\\


		// from dark second
			// to dark
		if (f == 2 && t == 0 && !fl && !tl) ScaleTo (true, "first", "zero");										// scale to zero
		else if (f == 2 && t == 3 && !fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 2 && t == 4 && !fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 2 && t == 7 && !fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 2 && t == 8 && !fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 2 && t == 9 && !fl && !tl) ScaleTo (false, "first", "ninth");									// scale to ninth
			// to light
		if (f == 2 && t == 0 && !fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		else if (f == 2 && t == 3 && !fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 2 && t == 4 && !fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 2 && t == 7 && !fl && tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 2 && t == 8 && !fl && tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 2 && t == 9 && !fl && tl) ScaleTo (false, "first", "ninth");									// scale to ninth

		// from light second
			// to dark
		if (f == 2 && t == 0 && fl && !tl) ScaleTo (true, "first", "zero");											// scale to zero
		else if (f == 2 && t == 3 && fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 2 && t == 4 && fl && !tl) ScaleTo (true, "first", "hidden");									// scale to hidden
		else if (f == 2 && t == 7 && fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 2 && t == 8 && fl && !tl) ScaleTo (false, "first", "seventh");								// scale to seventh
		else if (f == 2 && t == 9 && fl && !tl) ScaleTo (false, "first", "ninth");									// scale to ninth
			// to light
		if (f == 2 && t == 0 && fl && tl) ScaleTo (true, "first", "zero");											// scale to zero
		else if (f == 2 && t == 3 && fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 2 && t == 4 && fl && tl) ScaleTo (false, "first", "third");									// scale to third
		else if (f == 2 && t == 7 && fl && tl) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 2 && t == 8 && fl && tl) ScaleTo (false, "first", "seventh");									// scale to seventh
		else if (f == 2 && t == 9 && fl && tl) ScaleTo (false, "first", "ninth");									// scale to ninth


	///// third \\\\\


		// from dark third
			// to dark circle
		if (f == 3 && t == 0 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "zero");							// scale to zero
		else if (f == 3 && t == 1 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 2 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 5 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 6 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 7 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "seventh");					// scale to seventh
		else if (f == 3 && t == 8 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "seventh");					// scale to seventh
		else if (f == 3 && t == 9 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "ninth");						// scale to ninth
			// to light circle
		if (f == 3 && t == 0 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "zero");								// scale to zero
		else if (f == 3 && t == 1 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 2 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 4 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "third");						// scale to third
		else if (f == 3 && t == 5 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 6 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 3 && t == 7 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "seventh");						// scale to seventh
		else if (f == 3 && t == 8 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "seventh");						// scale to seventh
		else if (f == 3 && t == 9 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "ninth");						// scale to ninth
			// to light triangle
		if (f == 3 && t == 5 && !fl && tl && ts == 1) {
			SetShape(1);																							// change to triangle
			resetScale = true;																						// set reset scale flag
		}
			// to light square
		if (f == 3 && t == 5 && !fl && tl && ts == 2) {
			SetShape(2);																							// change to square
			resetScale = true;																						// set reset scale flag
		}

		// from light third
			// to dark circle
		if (f == 3 && t == 0 && fl && !tl && ts == 0) ScaleTo (true, "third", "zero");								// scale to zero
		else if (f == 3 && t == 1 && fl && !tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 2 && fl && !tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 4 && fl && !tl && ts == 0) ScaleTo (true, "third", "hidden");						// scale to hidden
		else if (f == 3 && t == 5 && fl && !tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 6 && fl && !tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 7 && fl && !tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 3 && t == 8 && fl && !tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 3 && t == 9 && fl && !tl && ts == 0) ScaleTo (false, "third", "ninth");						// scale to ninth
			// to light circle
		if (f == 3 && t == 0 && fl && tl && ts == 0) ScaleTo (true, "third", "zero");								// scale to zero
		else if (f == 3 && t == 1 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 2 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 5 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 6 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 3 && t == 7 && fl && tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 3 && t == 8 && fl && tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 3 && t == 9 && fl && tl && ts == 0) ScaleTo (false, "third", "ninth");							// scale to ninth
			// to light triangle
		if (f == 3 && t == 5 && fl && tl && ts == 1) {
			ScaleTo (false, "third", "ninth");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light square
		if (f == 3 && t == 5 && fl && tl && ts == 2) {
			ScaleTo (false, "third", "ninth");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}


	///// fourth \\\\\

		// from dark fourth
			// to dark circle
		if (f == 4 && t == 0 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "zero");							// scale to zero
		else if (f == 4 && t == 1 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 2 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 5 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 6 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 7 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "seventh");					// scale to seventh
		else if (f == 4 && t == 8 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "seventh");					// scale to seventh
		else if (f == 4 && t == 9 && !fl && !tl && ts == 0) ScaleTo (false, "hidden", "ninth");						// scale to ninth
			// to light circle
		if (f == 4 && t == 0 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "zero");								// scale to zero
		else if (f == 4 && t == 1 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 2 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 3 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "third");						// scale to third
		else if (f == 4 && t == 4 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "third");						// scale to third
		else if (f == 4 && t == 5 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 6 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "first");						// scale to first
		else if (f == 4 && t == 7 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "seventh");						// scale to seventh
		else if (f == 4 && t == 8 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "seventh");						// scale to seventh
		else if (f == 4 && t == 9 && !fl && tl && ts == 0) ScaleTo (false, "hidden", "ninth");						// scale to ninth

		// from light fourth
			// to dark circle
		if (f == 4 && t == 0 && fl && !tl && ts == 0) ScaleTo (true, "third", "zero");								// scale to zero
		else if (f == 4 && t == 1 && fl && !tl && ts == 0) ScaleTo (true, "third", "zero");							// scale to zero
		else if (f == 4 && t == 2 && fl && !tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 4 && t == 3 && fl && !tl && ts == 0) ScaleTo (true, "third", "hidden");						// scale to hidden
		else if (f == 4 && t == 4 && fl && !tl && ts == 0) ScaleTo (true, "third", "hidden");						// scale to hidden
		else if (f == 4 && t == 5 && fl && !tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 4 && t == 6 && fl && !tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 4 && t == 7 && fl && !tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 4 && t == 8 && fl && !tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 4 && t == 9 && fl && !tl && ts == 0) ScaleTo (false, "third", "ninth");						// scale to ninth
			// to light circle
		if (f == 4 && t == 0 && fl && tl && ts == 0) ScaleTo (true, "third", "zero");								// scale to zero
		else if (f == 4 && t == 1 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 4 && t == 2 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 4 && t == 5 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 4 && t == 6 && fl && tl && ts == 0) ScaleTo (true, "third", "first");							// scale to first
		else if (f == 4 && t == 7 && fl && tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 4 && t == 8 && fl && tl && ts == 0) ScaleTo (false, "third", "seventh");						// scale to seventh
		else if (f == 4 && t == 9 && fl && tl && ts == 0) ScaleTo (false, "third", "ninth");							// scale to ninth
			// to light triangle
		if (f == 4 && t == 5 && fl && !tl && ts == 1) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 6 && fl && !tl && ts == 1) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && !tl && ts == 1) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 8 && fl && !tl && ts == 1) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && !tl && ts == 1) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light square
		if (f == 4 && t == 5 && fl && tl && ts == 2) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 6 && fl && tl && ts == 2) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 7 && fl && tl && ts == 2) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 8 && fl && tl && ts == 2) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 4 && t == 9 && fl && tl && ts == 2) {
			ScaleTo (true, "third", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}


	///// fifth \\\\\


		// from dark circle fifth
			// to dark circle
		if (f == 5 && t == 0 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 5 && t == 3 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 4 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 7 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 8 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to light circle
		if (f == 5 && t == 0 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 5 && t == 3 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "third");						// scale to third
		else if (f == 5 && t == 4 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "third");						// scale to third
		else if (f == 5 && t == 7 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 8 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");						// scale to ninth

		// from light circle fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 5 && t == 3 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 4 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 5 && t == 7 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 8 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to light circle
		if (f == 5 && t == 0 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 5 && t == 3 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "third");							// scale to third
		else if (f == 5 && t == 4 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "third");							// scale to third
		else if (f == 5 && t == 7 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 8 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");							// scale to ninth

		// from light triangle/square fifth
			// to dark circle
		if (f == 5 && t == 0 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 5 && t == 1 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 5 && t == 2 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 5 && t == 3 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 5 && t == 4 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 5 && t >= 5 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light circle
		if (f == 5 && t == 0 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 5 && t == 1 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 5 && t == 2 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 5 && t == 3 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 5 && t == 4 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 5 && t >= 5 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}

		// from light triangle fifth
			// to dark triangle
		if (f == 5 && t == 7 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 5 && t == 8 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to dark triangle
		if (f == 5 && t == 7 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 5 && t == 8 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "first", "ninth");						// scale to ninth

		// from light square fifth
			// to dark square
		if (f == 5 && t == 7 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 5 && t == 8 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to dark square
		if (f == 5 && t == 7 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 5 && t == 8 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 5 && t == 9 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "first", "ninth");							// scale to ninth


	///// sixth \\\\\


		// from dark circle sixth
			// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 6 && t == 3 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 6 && t == 4 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 6 && t == 7 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 8 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 6 && t == 3 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "third");						// scale to third
		else if (f == 6 && t == 7 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 8 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");						// scale to ninth

		// from light circle sixth
			// to dark circle
		if (f == 6 && t == 0 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 6 && t == 3 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 6 && t == 4 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "first", "hidden");						// scale to hidden
		else if (f == 6 && t == 7 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 8 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to light circle
		if (f == 6 && t == 0 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "first", "zero");								// scale to zero
		else if (f == 6 && t == 3 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "third");							// scale to third
		else if (f == 6 && t == 7 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 8 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "first", "ninth");							// scale to ninth

		// from dark triangle/square sixth
			// to dark circle
		if (f == 6 && t == 0 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 6 && t == 1 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 6 && t == 2 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 6 && t == 4 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 6 && t >= 5 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light circle
		if (f == 6 && t == 0 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 6 && t == 1 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 6 && t == 2 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 6 && t == 3 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 6 && t == 4 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 5 && t >= 5 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "first", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			
		// from dark triangle sixth
			// to dark triangle
		if (f == 6 && t == 7 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 6 && t == 8 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to light triangle
		if (f == 6 && t == 7 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 6 && t == 8 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "first", "ninth");						// scale to ninth

		// from dark square sixth
			// to dark square
		if (f == 6 && t == 7 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 6 && t == 8 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "first", "ninth");						// scale to ninth
			// to light square
		if (f == 6 && t == 7 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");							// scale to seventh
		else if (f == 6 && t == 8 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "first", "seventh");						// scale to seventh
		else if (f == 6 && t == 9 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "first", "ninth");							// scale to ninth


	///// seventh + eighth \\\\\


		// from dark circle seventh/eighth
			// to dark circle
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "zero");				// scale to zero
		else if ((f == 7 || f == 8) && t == 1 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 2 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 3 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");			// scale to hidden
		else if ((f == 7 || f == 8) && t == 4 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");			// scale to hidden
		else if ((f == 7 || f == 8) && t == 5 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 6 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 9 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "seventh", "ninth");			// scale to ninth
			// to light circle
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "zero");					// scale to zero
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "third");			// scale to third
		else if ((f == 7 || f == 8) && t == 5 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 6 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 9 && !fl && tl && fs == 0 && ts == 0) ScaleTo (false, "seventh", "ninth");			// scale to ninth

		// from light circle seventh/eighth
			// to dark circle
		if ((f == 7 || f == 8) && t == 0 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "zero");					// scale to zero
		else if ((f == 7 || f == 8) && t == 1 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 2 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 3 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");			// scale to hidden
		else if ((f == 7 || f == 8) && t == 4 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "hidden");			// scale to hidden
		else if ((f == 7 || f == 8) && t == 5 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 6 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 9 && fl && !tl && fs == 0 && ts == 0) ScaleTo (false, "seventh", "ninth");			// scale to ninth
			// to light circle
		if ((f == 7 || f == 8) && t == 0 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "zero");					// scale to zero
		else if ((f == 7 || f == 8) && t == 1 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 2 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 3 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "third");			// scale to third
		else if ((f == 7 || f == 8) && t == 5 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 6 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "seventh", "first");			// scale to first
		else if ((f == 7 || f == 8) && t == 9 && fl && tl && fs == 0 && ts == 0) ScaleTo (false, "seventh", "ninth");			// scale to ninth

		// from dark triangle/square seventh/eighth
			// to dark circle	
		if ((f == 7 || f == 8) && t == 0 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 1 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 2 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 3 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if ((f == 7 || f == 8) && t == 4 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if ((f == 7 || f == 8) && t >= 5 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light circle
		if ((f == 7 || f == 8) && t == 0 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 1 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 2 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 3 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 4 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if ((f == 7 || f == 8) && t >= 5 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}

		// from light triangle/square seventh/eighth
			// to dark circle
		if ((f == 7 || f == 8) && t == 0 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 1 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 2 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 3 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if ((f == 7 || f == 8) && t == 4 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if ((f == 7 || f == 8) && t >= 5 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light circle
		if ((f == 7 || f == 8) && t == 0 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 1 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 2 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 3 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t == 4 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if ((f == 7 || f == 8) && t >= 5 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "seventh", "hidden");																	// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			
		// from dark triangle seventh/eighth
			// to dark triangle
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "first");				// scale to first
		else if ((f == 7 || f == 8) && t == 9 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "seventh", "ninth");			// scale to ninth
			// to light triangle
		if ((f == 7 || f == 8) && t == 5 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "first");				// scale to first
		else if ((f == 7 || f == 8) && t == 9 && !fl && tl && fs == 1 && ts == 1) ScaleTo (false, "seventh", "ninth");			// scale to ninth

		// from light triangle seventh/eighth
			// to dark triangle
		if ((f == 7 || f == 8) && t == 6 && fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "first");				// scale to first
		else if ((f == 7 || f == 8) && t == 9 && fl && !tl && fs == 1 && ts == 1) ScaleTo (false, "seventh", "ninth");			// scale to ninth
			// to light triangle
		if ((f == 7 || f == 8) && t == 5 && fl && tl && fs == 1 && ts == 1) ScaleTo (true, "seventh", "first");					// scale to first
		else if ((f == 7 || f == 8) && t == 9 && fl && tl && fs == 1 && ts == 1) ScaleTo (false, "seventh", "ninth");			// scale to ninth

		// from dark square seventh/eighth
			// to dark square
		if ((f == 7 || f == 8) && t == 6 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "first");				// scale to first
		else if ((f == 7 || f == 8) && t == 9 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "seventh", "ninth");			// scale to ninth
			// to light square
		if ((f == 7 || f == 8) && t == 5 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "first");				// scale to first
		else if ((f == 7 || f == 8) && t == 9 && !fl && tl && fs == 2 && ts == 2) ScaleTo (false, "seventh", "ninth");			// scale to ninth

		// from light square seventh/eighth
			// to dark square
		if ((f == 7 || f == 8) && t == 6 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "first");				// scale to first
		else if ((f == 7 || f == 8) && t == 9 && fl && !tl && fs == 2 && ts == 2) ScaleTo (false, "seventh", "ninth");			// scale to ninth
			// to light square
		if ((f == 7 || f == 8) && t == 5 && fl && tl && fs == 2 && ts == 2) ScaleTo (true, "seventh", "first");					// scale to first
		else if ((f == 7 || f == 8) && t == 9 && fl && tl && fs == 2 && ts == 2) ScaleTo (false, "seventh", "ninth");			// scale to ninth


	///// ninth \\\\\


		// from dark circle ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		else if (f == 9 && t == 1 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");						// scale to first
		else if (f == 9 && t == 2 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");						// scale to first
		else if (f == 9 && t == 3 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden
		else if (f == 9 && t == 4 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");						// scale to hidden
		else if (f == 9 && t == 5 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");						// scale to first
		else if (f == 9 && t == 6 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");						// scale to first
		else if (f == 9 && t == 7 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		else if (f == 9 && t == 1 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 2 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 3 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "third");							// scale to third
		else if (f == 9 && t == 5 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 6 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 7 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 7 && !fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh

		// from light circle ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		if (f == 9 && t == 1 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");								// scale to first
		if (f == 9 && t == 2 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");								// scale to first
		if (f == 9 && t == 3 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");								// scale to hidden
		if (f == 9 && t == 4 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "hidden");								// scale to hidden
		if (f == 9 && t == 5 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");								// scale to first
		if (f == 9 && t == 6 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");								// scale to first
		if (f == 9 && t == 7 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");							// scale to seventh
		if (f == 9 && t == 8 && fl && !tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");							// scale to seventh
			// to light circle
		if (f == 9 && t == 0 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "zero");								// scale to zero
		else if (f == 9 && t == 1 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 2 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 3 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "third");							// scale to hidden
		else if (f == 9 && t == 4 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "third");							// scale to hidden
		else if (f == 9 && t == 5 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 6 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "first");							// scale to first
		else if (f == 9 && t == 7 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && fl && tl && fs == 0 && ts == 0) ScaleTo (true, "ninth", "seventh");						// scale to seventh

		// from dark triangle/square ninth
			// to dark circle
		if (f == 9 && t == 0 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 1 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 2 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 9 && t == 4 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 9 && t >= 5 && !fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 1 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 2 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 3 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 4 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 9 && t >= 5 && !fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
	
		// from light triangle/square ninth
			// to dark circle
		if (f == 9 && t == 0 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 1 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 2 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 3 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 9 && t == 4 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
		}
		else if (f == 9 && t >= 5 && fl && !tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
			// to light circle
		if (f == 9 && t == 0 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 1 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 2 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 3 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t == 4 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}
		else if (f == 9 && t >= 5 && fl && tl && (fs == 1 || fs == 2) && ts == 0) {
			ScaleTo (true, "ninth", "hidden");																		// scale to hidden
			changeShape = true;																						// set change shape flag
			resetScale = true;																						// set reset scale flag
		}

		// from dark triangle ninth
			// to dark triangle
		if (f == 9 && t == 6 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
			// to light triangle
		if (f == 9 && t == 5 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && !fl && tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh

		// from light triangle ninth
			// to dark triangle
		if (f == 9 && t == 6 && fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
			// to light triangle
		if (f == 9 && t == 5 && fl && tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && fl && tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && fl && tl && fs == 1 && ts == 1) ScaleTo (true, "ninth", "seventh");						// scale to seventh

		// from dark square ninth
			// to dark square
		if (f == 9 && t == 6 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && !fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
			// to light square
		if (f == 9 && t == 5 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && !fl && tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh

		// from light square ninth
			// to dark square
		if (f == 9 && t == 6 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && fl && !tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
			// to light square
		if (f == 9 && t == 5 && fl && tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "first");								// scale to first
		else if (f == 9 && t == 7 && fl && tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh
		else if (f == 9 && t == 8 && fl && tl && fs == 2 && ts == 2) ScaleTo (true, "ninth", "seventh");						// scale to seventh


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

	}
		
	///<summary>
	///<para>0 = circle</para>
	///<para>1 = triangle</para>
	///<para>2 = square</para>
	///</summary>
	private void SetShape(int ts)
	{
		if (ts == 0) GetComponent<MeshFilter>().mesh = sphere;														// change mesh to sphere
		else if (ts == 1) GetComponent<MeshFilter>().mesh = triangle;												// change mesh to triangle
		else if (ts == 2) GetComponent<MeshFilter>().mesh = square;													// change mesh to square
	}

	///<summary>
	///<para>set core light</para>
	///<para>true = white</para>
	///<para>false = black</para>
	///</summary>
	private void SetLight (bool l)
	{
        if (l) {
            anim.SetBool("black", false);																			// reset previously active state
            anim.SetBool("white", true);																			// set active state
        }
		else if (!l) {
            anim.SetBool("white", false);																			// reset previously active state
            anim.SetBool("black", true);																			// set active state
        }
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
			anim.ResetTrigger ("scaleup");																			// reset last stage
			anim.SetTrigger ("scaledown");																			// enable scaledown
		}
		else if (!devol) {
			anim.ResetTrigger ("scaledown");																		// reset last stage
			anim.SetTrigger ("scaleup");																			// enable scaleup

		}
		anim.SetBool(resetState, false);																			// reset previously active state
		anim.SetBool(setState, true);																				// set active state
	}
}
