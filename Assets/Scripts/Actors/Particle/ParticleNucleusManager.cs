using UnityEngine;
using System.Collections;

public class ParticleNucleusManager : MonoBehaviour {

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

	private bool light; 							// is light flag
	private Shader lightShader, darkShader;			// light/dark shaders

	void Awake () {
		anim = GetComponent<Animator>();							// init animator ref
		mesh = GetComponent<MeshFilter>().mesh;						// init mesh ref
		rend = GetComponent<MeshRenderer>();						// init mesh renderer ref
		psp = GetComponentInParent<ParticleStatePattern> ();		// init psp ref
		//lightShader = Shader.Find("light_nucleus");					// init light nucleus shader
		//darkShader = Shader.Find("dark_nucleus");						// init dark nucleus shader
	}

	void Update() {
		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;																			// start timer
		if (resetScaleTimer >= 4.0f) {																								// when timer >= 4 sec
			//anim.ResetTrigger("colour");	
			if (toState == 0) ScaleTo (false, "hidden", "zero");																		// if to zero, grow to zero
			if (toState == 1 || toState == 2 || toState == 4 || toState == 5 || toState == 6) ScaleTo (false, "hidden", "first");		// if to first/second/fifth/sixth, grow to first
			if (toState == 7 || toState == 8) ScaleTo (false, "hidden", "seventh");														// if to seventh/eighth, grow to seventh
			if (toState == 9) ScaleTo (false, "hidden", "ninth");																		// if to ninth, grow to ninth
			resetScale = false;																											// reset reset scale flag
			resetScaleTimer = 0f;																										// reset timer
		}
	}

	public void ToOtherWorld (bool lw, int f, int t, bool l) 
	{
		toState = t;																					// set to state
		if (lw) {																						// to light world
			// from changes
			if (f == 0) ScaleTo (true, "zero", "hidden");													// scale from zero to hidden
			else if (f == 1 || f == 2 || f == 4 || f == 5 || f == 6) ScaleTo (true, "first", "hidden");		// scale from second/first to hidden
			else if (f == 7 || f == 8) ScaleTo (true, "seventh", "hidden");									// scale from seventh to hidden
			else if (f == 9) ScaleTo (true, "ninth", "hidden");												// scale from ninth to hidden

			// to changes
			if (t == 0) SetLight (false, true);																// if to zero, change to white
			else if (t == 1 || t == 5) SetLight (false, true);												// if to first/fifth, change to white
			else if (t == 2 || t == 4 || t == 6) SetLight (true, true);										// if to second/fourth/sixth, change to black dot shader
			else if (t == 3) SetLight (false, true);														// to third, change to white
			else if (t == 7) SetLight (false, true);														// if to seventh, change to white
			else if (t == 8) SetLight (true, true);															// if to eighth, change to white + black shader
			else if (t == 9) SetLight (false, true);														// if to ninth, change to white
		}
		else if (!lw) {																					// to dark world
			if (f == 0) {																					// from light world zero		
				ScaleTo (true, "zero", "hidden");															// scale from zero
				SetLight (false, false);																	// change to black
				ScaleTo (false, "hidden", "zero");															// scale to zero
			}
		}
	}

	public void Nucleus (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{


		///////////////////// EVOLUTIONS \\\\\\\\\\\\\\\\\\\\\\


		///// zero \\\\\


		// to dark zero
		if (fromState == 0 && toState == 0 && fromLight && !toLight) ScaleTo (false, "hidden", "zero");							// scale to zero


		///// half zero \\\\\


		// from dark zero
		// to dark first
		if (fromState == 0 && toState == 1 && !fromLight && !toLight) ScaleTo (false, "zero", "first");							// scale to first
		// to light first (no nucleus change)

		// from light zero (0.5)
		// to dark first
		if (fromState == 0 && toState == 1 && fromLight && !toLight) ScaleTo (false, "hidden", "first");						// scale to first
		// to light first (no nucleus change)


		///// first \\\\\


		// from dark first
		// to dark second
		if (fromState == 1 && toState == 2 && !fromLight && !toLight) SetLight(true);											// change to white shader
		// to light second
		else if (fromState == 1 && toState == 2 && !fromLight && toLight) ScaleTo (false, "hidden", "zero");					// scale to zero

		// from light first
		// to dark second
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			
			SetLight(true);																										// change to white shader
			ScaleTo (false, "hidden", "first");																					// scale to first
		}
		// to light second
		else if (fromState == 1 && toState == 2 && fromLight && toLight) ScaleTo (false, "hidden", "zero");						// scale to first


		///// second \\\\\


		// from dark second
		// to dark third
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
		} 
		// to light third
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		 
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
		}

		// from light second
		// to dark third
		else if (fromState == 2 && toState == 3 && fromLight && !toLight) ScaleTo (true, "zero", "hidden");						// scale to hidden
		// to light third
		else if (fromState == 2 && toState == 3 && fromLight && toLight) ScaleTo (true, "zero", "hidden");						// scale to hidden


		///// third \\\\\


		// from dark third
		// to dark fourth
		if (fromState == 3 && toState == 4 && !fromLight && !toLight) {							
			SetLight(true);																										// change to white shader
			ScaleTo (false, "hidden", "first");																					// scale to first
		}
		// to light fourth
		else if (fromState == 3 && toState == 4 && !fromLight && toLight) ScaleTo (false, "hidden", "zero");					// scale to zero

		// from light third
		// to dark fourth
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {							
			SetLight(true);																										// change to white shader
			ScaleTo (false, "hidden", "first");																					// scale to first
		}
		// to light fourth
		else if (fromState == 3 && toState == 4 && fromLight && toLight) ScaleTo (false, "hidden", "zero");						// scale to first


		///// fourth \\\\\


		// from dark fourth
		// to dark circle fifth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
			ScaleTo (false, "hidden", "first");																					// scale to first
		}
		// to light circle fifth
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
		}

		// from light fourth
		// to triangle fifth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {			
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			SetShape(1);																										// change to triangle
		}
		// to square fifth
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		
			ScaleTo (true, "zero", "hidden");																					// scale to hidden
			SetShape(2);																										// change to square
		}


		///// fifth \\\\\


		// from dark circle fifth
		// to dark circle sixth
		if (fromState == 5 && toState == 6 && !fromLight && !toLight && shape == 0) SetLight(true);								// change to white shader
		// to light circle sixth
		else if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) ScaleTo (false, "first", "zero");		// scale to first

		// from light circle fifth
		// to dark circle sixth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			
			SetLight(true);																										// change to white
			ScaleTo (false, "hidden", "first");																					// scale to first
		}
		// to light circle sixth
		else if (fromState == 5 && toState == 6 && fromLight && toLight && shape == 0) ScaleTo (false, "hidden", "zero");		// scale to zero

		// from triangle fifth
		// to dark triangle sixth
		if (fromState == 5 && toState == 6 && fromLight && shape == 1) ScaleTo (false, "hidden", "first");						// scale to first

		// from square fifth
		// to dark square sixth
		if (fromState == 5 && toState == 6 && fromLight && shape == 2) ScaleTo (false, "hidden", "first");						// scale to first


		//// sixth \\\\\


		// from dark circle sixth
		// to dark circle seventh
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
			ScaleTo (false, "hidden", "seventh");																				// scale to seventh
		}
		// to light circle seventh
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
		}

		// from light circle sixth
		// to dark circle seventh
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) ScaleTo (false, "zero", "seventh");			// scale to seventh
		// to light circle seventh
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) ScaleTo (true, "zero", "hidden");		// scale to hidden

		// from dark triangle sixth
		// to dark triangle seventh
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) ScaleTo (false, "first", "hidden");			// scale to hidden
		// to light triangle seventh
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) ScaleTo (false, "first", "hidden");		// scale to hidden

		// from dark square sixth
		// to dark square seventh
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) ScaleTo (false, "first", "hidden");			// scale to hidden
		// to light square seventh
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) ScaleTo (false, "first", "hidden");		// scale to hidden


		///// seventh \\\\\


		// from dark circle seventh
		// to dark circle eighth
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 0) SetLight(true);								// change to white shader
		// to light circle eighth
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) ScaleTo (true, "seventh", "first");		// scale to first

		// from light circle seventh
		// to dark circle eighth
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			
			SetLight(true);																										// change to white shader
			ScaleTo (false, "hidden", "seventh");																				// scale to seventh
		}
		// to light circle eighth
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 0) ScaleTo (false, "hidden", "first");		// scale to first

		// from dark triangle seventh
		// to dark triangle eighth
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 1) ScaleTo (false, "hidden", "seventh");		// scale to seventh
		// to light triangle eighth
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 1) ScaleTo (false, "hidden", "seventh");	// scale to seventh

		// from light triangle seventh
		// to dark triangle eighth
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 1) ScaleTo (false, "hidden", "seventh");		// scale to seventh
		// to light triangle eighth
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 1) ScaleTo (false, "hidden", "seventh");	// scale to seventh

		// from dark square seventh
		// to dark square eighth
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 2) ScaleTo (false, "hidden", "seventh");		// scale to seventh
		// to light square eighth
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 2) ScaleTo (false, "hidden", "seventh");	// scale to seventh

		// from light square seventh
		// to dark square eighth
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 2) ScaleTo (false, "hidden", "seventh");		// scale to seventh
		// to light square eighth
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 2) ScaleTo (false, "hidden", "seventh");	// scale to seventh



		////////////////////// DEVOLUTIONS \\\\\\\\\\\\\\\\\\\\\\\



		///// zero \\\\\


		///// dark zero (0.5) \\\\\


		// to zero
		if (fromState == 0 && toState == 0 && !fromLight && toLight) ScaleTo (true, "zero", "hidden");							// scale to hidden


		///// first \\\\\


		// from dark first
		// to zero
		if (fromState == 1 && toState == 0 && !fromLight && toLight) ScaleTo (true, "first", "hidden");							// scale to hidden
		// to dark zero
		if (fromState == 1 && toState == 0 && !fromLight && !toLight) ScaleTo (true, "first", "zero");							// scale to zero

		// from light first
		// to zero (no nucleus change)
		// to dark zero
		if (fromState == 1 && toState == 0 && fromLight && !toLight) ScaleTo (true, "first", "zero");							// scale to zero


		///// second \\\\\


		// from dark second
		// to zero
		if (fromState == 2 && toState == 0 && !fromLight && toLight) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
		}
		// to dark zero
		else if (fromState == 2 && toState == 0 && !fromLight && !toLight) {
			ScaleTo (true, "first", "hidden");																					// scale to hidden
			SetLight(false);																									// change to black
			ScaleTo (false, "hidden", "zero");																					// scale to zero
		}
		// to first
		if (fromState == 2 && toState == 1 && !fromLight && !toLight) {															// to dark first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "first");																						// scale to hidden
		}
		else if (fromState == 2 && toState == 1 && !fromLight && toLight) {														// to light first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}

		// from light second
		// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) ScaleTo (true, "zero", "hidden");							// scale to hidden
		// to dark zero (no nucleus change)
		// to first
		// to dark first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) ScaleTo (true, "zero", "first");							// scale to hidden
		// to light first
		else if (fromState == 2 && toState == 1 && fromLight && toLight) ScaleTo (true, "zero", "hidden");						// scale to hidden


		///// third \\\\\


		// from dark third	
		// to zero (no nucleus change)
		// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) ScaleTo (false, "hidden", "zero");						// scale to zero
		// to first 
		// to dark first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) ScaleTo (false, "hidden", "first");						// scale to first
		// to light first (no nucleus change)
		// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {															// to dark second
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light second
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) ScaleTo (false, "hidden", "zero");					// scale to zero

		// from light third	
		// to zero ((no nucleus change)
		// to dark zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) ScaleTo (false, "hidden", "zero");							// scale to zero
		// to first
		// to dark first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) ScaleTo (false, "hidden", "first");						// scale to first
		// to light first (no nucleus change)
		// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {															// to dark second					
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {														// to light second
			ScaleTo (false, "hidden", "zero");																						// scale to first
		}


		///// fourth \\\\\


		// from dark fourth	
		// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {															// to light zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}
		// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {															// to dark zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {															// to dark first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {														// to light first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}
		// to second
		// to dark second (no nucleus change)
		if (fromState == 4 && toState == 2 && !fromLight && toLight) {															// to light second
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		if (fromState == 4 && toState == 3 && !fromLight && !toLight) {															// to dark third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}
		else if (fromState == 4 && toState == 3 && !fromLight && toLight) {														// to light third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}

		// from light fourth	
		// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) ScaleTo (true, "zero", "hidden");							// scale to hidden
		// to dark zero (no nucleus change)
		// to first
		// to dark first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) ScaleTo (false, "zero", "first");							// scale to first
		// to light first
		else if (fromState == 4 && toState == 1 && fromLight && toLight) ScaleTo (true, "zero", "hidden");						// scale to hidden
		// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {															// to dark second
			ScaleTo (true, "zero", "hidden");																						// scale to hidden
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light second (no nucleus change)
		// to third
		// to dark third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) ScaleTo (true, "first", "hidden");							// scale to hidden
		// to light third
		else if (fromState == 4 && toState == 3 && fromLight && toLight) ScaleTo (true, "first", "hidden");						// scale to hidden


		///// fifth \\\\\


		// from dark circle fifth
		// to zero
		if (fromState == 5 && toState == 0 && !fromLight && toLight) ScaleTo (true, "first", "hidden");							// scale to hidden
		// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight) ScaleTo (true, "first", "zero");							// scale to zero
		// to first
		// to dark first (no nucleus change)
		// to light first (no nucleus change)
		if (fromState == 5 && toState == 1 && !fromLight && toLight) ScaleTo (true, "first", "hidden");							// scale to hidden
		// to second
		// to dark second
		if (fromState == 5 && toState == 2 && !fromLight && !toLight && shape == 0) SetLight(true);								// change to white shader
		// to light second
		else if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) ScaleTo (true, "first", "zero");		// scale to zero
		// to third
		// to dark third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) ScaleTo (true, "first", "hidden");			// scale to hidden
		// to light third
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) ScaleTo (true, "first", "hidden");		// scale to hidden
		// to fourth
		// to dark fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) SetLight(true);								// change to white shader

		// from light circle fifth
		// to zero (no nucleus change)
		// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight) ScaleTo (false, "hidden", "zero");							// scale to zero
		// to first
		// to dark first (no nucleus change)
		// to light first (no nucleus change)
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {											// to dark second
			SetLight(true);																											// change to white shader	
			ScaleTo (false, "hidden", "first");																						// scale to zero
		}
		// to light second
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 0) ScaleTo (false, "hidden", "zero");		// scale to zero
		// to third
		// to dark third (no nucleus change)
		// to light third (no nucleus change)
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {											// to dark fourth
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		}

		// from triangle fifth
		// to zero 
		// to light zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 1) SetShape(0);									// change to sphere
		// to dark zero
		else if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 1) {										// to dark zero
			SetShape(0);																											// change to sphere
			ScaleTo (true, "hidden", "zero");																						// scale to zero
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {											// to dark first
			SetShape(0);																											// change to sphere
			ScaleTo (true, "hidden", "first");																						// scale to zero
		}
		// to light first
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 1) SetShape(0);								// change to sphere
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {											// to dark second
			SetShape(0);																											// change to sphere
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		} 
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {										// to light second
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		// to dark third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) SetShape(0);									// change to sphere
		// to light third
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 1) SetShape(0);								// change to sphere
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {												// to light fourth
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}

		// from square fifth
		// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 2) SetShape(0);									// change to sphere
		// to dark zero (no nucleus change)
		else if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 2) {										// to dark zero
			SetShape(0);																											// change to sphere
			ScaleTo (true, "hidden", "zero");																						// scale to zero
		}
		// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {											// to dark first
			SetShape(0);																											// change to sphere
			ScaleTo (true, "hidden", "first");																						// scale to zero
		}
		// to light first 
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 2) SetShape(0);								// change to sphere
		// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {											// to dark second
			SetShape(0);																											// change to sphere
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		} 
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {										// to light second
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		// to dark third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) SetShape(0);									// change to sphere
		// to light third
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 2) SetShape(0);								// change to sphere
		// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {												// to light fourth
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}


		///// sixth \\\\\


		// from dark circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {											// to zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {											// to dark zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 0) {											// to dark first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "first");																						// scale to first
		} 
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {										// to light first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}
		// to second
		// to dark second (no nucleus change)
		if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {											// to light second
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {											// to dark third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		} 
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {										// to light third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}
		// to fourth
		// to dark fourth (no nucleus change)
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 0) {											// to dark circle fifth
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
			ScaleTo (false, "hidden", "first");																						// scale to hidden
		}
		else if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {										// to light circle fifth
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetLight(false);																										// change to black
		}

		// from light circle sixth
		// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) ScaleTo (true, "zero", "hidden");				// scale to hidden
		// to dark zero (no nucleus change)
		// to first
		// to dark first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) ScaleTo (false, "zero", "first");			// scale to first
		// to light first
		else if (fromState == 6 && toState == 1 && fromLight && toLight && shape == 0) ScaleTo (true, "zero", "hidden");		// scale to hidden
		// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {											// to dark second
			ScaleTo (true, "zero", "hidden");																						// scale to hidden
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light second (no nucleus change)
		// to third
		// to dark third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) ScaleTo (true, "zero", "hidden");			// scale to hidden
		// to light third
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) ScaleTo (true, "zero", "hidden");		// scale to hidden
		// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {											// to dark fourth
			ScaleTo (true, "zero", "hidden");																						// scale to hidden
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to fifth
		// to dark circle fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) ScaleTo (false, "zero", "first");			// scale to first
		// to light circle fifth
		else if (fromState == 6 && toState == 5 && fromLight && toLight && shape == 0) ScaleTo (true, "zero", "hidden");		// scale to hidden

		// from triangle sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {											// to zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {											// to dark zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {											// to dark first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			ScaleTo (true, "hidden", "first");																						// scale to hidden
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {										// to light first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {											// to dark second
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		} 
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {										// to light second
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {											// to dark third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {										// to light third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {											// to light fourth
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to fifth
		// to triangle fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 1) ScaleTo (true, "first", "hidden");			// scale to hidden

		// from square sixth
		// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {											// to zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {											// to dark zero
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {											// to dark first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			ScaleTo (false, "hidden", "first");																						// scale to hidden
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {										// to light first
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {											// to dark second
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {										// to light second
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {											// to dark third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {										// to light third
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}
		// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {											// to light fourth
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {											// to square fifth
			ScaleTo (true, "first", "hidden");																						// scale to hidden
			SetShape(0);																											// change to sphere	
		}


		///// seventh \\\\\


		// from dark circle seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 0) ScaleTo (true, "seventh", "hidden");			// scale to hidden
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) ScaleTo (true, "seventh", "zero");			// scale to zero
		// to first
		// to dark first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) ScaleTo (true, "seventh", "first");			// scale to first
		// to light first
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) ScaleTo (true, "seventh", "hidden");	// scale to hidden
		// to second
		// to dark second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {											// to dark second
			SetLight(true);																											// change to white shader
			ScaleTo (true, "seventh", "first");																						// scale to first
		}
		// to light second
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) ScaleTo (true, "seventh", "zero");		// scale to zero
		// to third
		// to dark third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) ScaleTo (true, "seventh", "hidden");		// scale to hidden
		// to light third
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) ScaleTo (true, "seventh", "hidden");	// scale to hidden
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {											// to dark fourth
			SetLight(true);																											// change to white shader
			ScaleTo (false, "seventh", "first");																					// scale to first
		}
		// to fifth
		// to dark circle fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) ScaleTo (true, "seventh", "first");			// scale to first
		// to light circle fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) ScaleTo (true, "seventh", "hidden");			// scale to hidden
		// to sixth
		// to dark circle sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {											// to dark circle sixth
			SetLight(true);																											// change to white shader
			ScaleTo (false, "seventh", "first");																					// scale to first
		}
		// to light circle sixth
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) ScaleTo (false, "seventh", "zero");		// scale to first

		// from light circle seventh
		// to zero (no nucleus change)
		// to dark zero (no nucleus change)
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) ScaleTo (true, "hidden", "zero");			// scale to zero
		// to first
		// to dark first (no nucleus change)
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) ScaleTo (true, "hidden", "first");			// scale to first
		// to light first (no nucleus change)
		// to second
		// to dark second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {											// to dark second
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light second
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) ScaleTo (false, "hidden", "zero");		// scale to zero
		// to third
		// to dark third (no nucleus change)
		// to light third (no nucleus change)
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {											// to dark fourth
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to fifth
		// to dark circle fifth (no nucleus change)
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) ScaleTo (true, "hidden", "first");			// scale to first
		// to light circle fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {											// to dark circle sixth
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {										// to light circle sixth
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}

		// from dark triangle seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) SetShape(0);									// change to sphere
		// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {											// to dark zero
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		// to dark first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {											// to dark first
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light first
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) SetShape(0);							// change to sphere
		// to second
		// to dark second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {											// to dark second
			SetShape(0);																											// change to sphere
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light second
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {										// to light second
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		// to dark third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) SetShape(0);								// change to sphere
		// to light third
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) SetShape(0);							// change to sphere
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {											// to light fourth
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to fifth
		// to triangle fifth (no nucleus change)
		// to sixth
		// to dark triangle sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) ScaleTo (false, "hidden", "first");			// scale to first

		// from light triangle seventh
		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 1) SetShape(0);									// change to sphere
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 1) {											// to dark zero
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		// to dark first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {											// to dark first
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light first
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 1) SetShape(0);								// change to sphere
		// to second
		// to dark second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {											// to dark second
			SetShape(0);																											// change to sphere
			SetLight(true);																											// change to white shader
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light second
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {										// to light second
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		// to dark third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) SetShape(0);									// change to sphere
		// to light third
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 1) SetShape(0);								// change to sphere
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {												// to light fourth
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to fifth
		// to triangle fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {											// to dark triangle sixth
			ScaleTo (false, "hidden", "first");																						// scale to first
		}	
		// from dark square seventh
		// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) SetShape(0);									// change to sphere
		// to dark zero (no nucleus change)
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {											// to dark zero
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		// to dark first
		if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {											// to dark first
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light first
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) SetShape(0);							// change to sphere
		// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {											// to dark second
			SetShape(0);																											// change to sphere
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		} 
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {										// to light second
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		// to dark third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) SetShape(0);								// change to sphere
		// to light third
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) SetShape(0);							// change to sphere
		// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {											// to light fourth
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to fifth
		// to square fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {											// to dark square sixth
			ScaleTo (false, "hidden", "first");																						// scale to first
		}

		// from light square seventh
		// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 2) SetShape(0);									// change to sphere
		// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 2) {											// to dark zero
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to first
		// to dark first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {											// to dark first
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		// to light first
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 2) SetShape(0);								// change to sphere
		// to second
		// to dark second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {											// to dark second
			SetShape(0);																											// change to sphere
			SetLight(true);																											// change to white
			ScaleTo (false, "hidden", "first");																						// scale to first
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {										// to light second
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to third
		// to dark third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) SetShape(0);									// change to sphere
		// to light third
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 2) SetShape(0);								// change to sphere
		// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {												// to light fourth
			SetShape(0);																											// change to sphere
			ScaleTo (false, "hidden", "zero");																						// scale to zero
		}
		// to fifth
		// to square fifth (no nucleus change)
		// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 2) {											// to dark square sixth
			ScaleTo (false, "hidden", "first");																						// scale to first
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
		if (shape == 0) mesh = sphere;								// change mesh to sphere
		else if (shape == 1) mesh = triangle;						// change mesh to triangle
		else if (shape == 2) mesh = square;							// change mesh to square
	}

	///<summary>
	///<para>set nucleus light into dark world</para>
	///<para>white = dark, black = light</para>
	///<para>true, true = black dot shader</para>
	///<para>true, false = solid white</para>
	///<para>false, true = white dot shader</para>
	///<para>false, false = solid black</para>
	///</summary>
	private void SetLight (bool lite, bool toLW)
	{
		if (lite && toLW) {												// if to light world and light (as in second, fourth, etc)
			rend.material.shader = darkShader;								// change to black shader
			light = true;													// set is light flag
		} 
		else if (!lite && toLW) {										// if to light world and dark (as in dark zero, first, etc)
			rend.material.SetColor("_Color", Color.white);					// change to white
			light = false;													// reset is light flag
		}
		else if (lite && !toLW) {										// if to dark world and light (as in second, fourth, etc)
			rend.material.shader = lightShader;								// change to white shader
			light = true;													// set is light flag
		}
		else if (!lite && !toLW) {										// if to dark world and dark (as in dark zero, first, etc)
			rend.material.SetColor("_Color", Color.black);					// change to black
			light = false;													// reset is light flag
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
			light = true;												// set is light flag
		} 
		else if (!lite && !psp.lightworld) {
			rend.material.SetColor("_Color", Color.black);				// change to black
			light = false;												// reset is light flag
		}
		else if (lite && psp.lightworld) {
			rend.material.shader = darkShader;							// change to black shader
			light = true;												// set is light flag
		}
		else if (!lite && psp.lightworld) {
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
