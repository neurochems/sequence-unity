using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Animator anim;																				// animator on camera ref
	private PlayerStatePattern psp;																		// psp ref

	private int fromState, toState;																		// from state/to state

	private bool resetZoom = false;																		// reset zoom flag
	private float resetZoomTimer;																		// reset zoom timer

	void Update() 
	{
		// reset scale timer
		if (resetZoom) resetZoomTimer += Time.deltaTime;												// start timer
		if (resetZoomTimer >= 2.5f) {																	// when timer >= 2 sec
			Debug.Log ("reset camera zoom");
			anim.ResetTrigger ("zoomin");																	// reset scale down trigger
			anim.SetTrigger ("zoomout");																	// set scale up trigger
			anim.SetBool ("switchworld", false);															// reset switchworld
			resetZoom = false;																				// reset reset scale flag
			resetZoomTimer = 0f;																			// reset timer
		}
	}

	void Start () {
		anim = GetComponent<Animator>();																// init animator ref
		psp = GetComponentInParent<PlayerStatePattern>();												// init psp ref
	}

	public void ToOtherWorld (int f, int t)
	{
		fromState = f;																					// store from state
		toState = t;																					// store to state

		///// zero \\\\\

		if (f == 0 && t == 0) SwitchWorld ("zero", "zero");												// zoom out
		else if (f == 0 && t == 1) SwitchWorld ("zero", "first");										// zoom out
		else if (f == 0 && t == 2) SwitchWorld ("zero", "first");										// zoom out
		else if (f == 0 && t == 3) SwitchWorld ("zero", "third");										// zoom out
		else if (f == 0 && t == 4) SwitchWorld ("zero", "third");										// zoom out
		else if (f == 0 && t == 5) SwitchWorld ("zero", "fifth");										// zoom out
		else if (f == 0 && t == 6) SwitchWorld ("zero", "fifth");										// zoom out
		else if (f == 0 && t == 7) SwitchWorld ("zero", "seventh");										// zoom out
		else if (f == 0 && t == 8) SwitchWorld ("zero", "seventh");										// zoom out
		else if (f == 0 && t == 9) SwitchWorld ("zero", "ninth");										// zoom out

		///// first \\\\\

		if (f == 1 && t == 0) SwitchWorld ("first", "zero");											// zoom out
		else if (f == 1 && t == 1) SwitchWorld ("first", "first");										// zoom out
		else if (f == 1 && t == 2) SwitchWorld ("first", "first");										// zoom out
		else if (f == 1 && t == 3) SwitchWorld ("first", "third");										// zoom out
		else if (f == 1 && t == 4) SwitchWorld ("first", "third");										// zoom out
		else if (f == 1 && t == 5) SwitchWorld ("first", "fifth");										// zoom out	
		else if (f == 1 && t == 6) SwitchWorld ("first", "fifth");										// zoom out	
		else if (f == 1 && t == 7) SwitchWorld ("first", "seventh");									// zoom out
		else if (f == 1 && t == 8) SwitchWorld ("first", "seventh");									// zoom out
		else if (f == 1 && t == 9) SwitchWorld ("first", "ninth");										// zoom out

		///// second \\\\\

		if (f == 2 && t == 0) SwitchWorld ("first", "zero");											// zoom in
		else if (f == 2 && t == 1) SwitchWorld ("first", "first");										// zoom out
		else if (f == 2 && t == 2) SwitchWorld ("first", "first");										// zoom out
		else if (f == 2 && t == 3) SwitchWorld ("first", "third");										// zoom out
		else if (f == 2 && t == 4) SwitchWorld ("first", "third");										// zoom out
		else if (f == 2 && t == 5) SwitchWorld ("first", "fifth");										// zoom out	
		else if (f == 2 && t == 6) SwitchWorld ("first", "fifth");										// zoom out	
		else if (f == 2 && t == 7) SwitchWorld ("first", "seventh");									// zoom out
		else if (f == 2 && t == 8) SwitchWorld ("first", "seventh");									// zoom out
		else if (f == 2 && t == 9) SwitchWorld ("first", "ninth");										// zoom out

		///// third \\\\\

		if (f == 3 && t == 0) SwitchWorld ("third", "zero");											// zoom in
		else if (f == 3 && t == 1) SwitchWorld ("third", "first");										// zoom in
		else if (f == 3 && t == 2) SwitchWorld ("third", "first");										// zoom in	
		else if (f == 3 && t == 3) SwitchWorld ("third", "third");										// zoom in	
		else if (f == 3 && t == 4) SwitchWorld ("third", "third");										// zoom in	
		else if (f == 3 && t == 5) SwitchWorld ("third", "fifth");										// zoom out	
		else if (f == 3 && t == 6) SwitchWorld ("third", "fifth");										// zoom out	
		else if (f == 3 && t == 7) SwitchWorld ("third", "seventh");									// zoom out
		else if (f == 3 && t == 8) SwitchWorld ("third", "seventh");									// zoom out
		else if (f == 3 && t == 9) SwitchWorld ("third", "ninth");										// zoom out

		///// fourth \\\\\

		if (f == 4 && t == 0) SwitchWorld ("third", "zero");											// zoom in
		else if (f == 4 && t == 1) SwitchWorld ("third", "first");										// zoom in
		else if (f == 4 && t == 2) SwitchWorld ("third", "first");										// zoom in
		else if (f == 4 && t == 3) SwitchWorld ("third", "third");										// zoom in
		else if (f == 4 && t == 4) SwitchWorld ("third", "third");										// zoom in
		else if (f == 4 && t == 5) SwitchWorld ("third", "fifth");										// zoom out	
		else if (f == 4 && t == 6) SwitchWorld ("third", "fifth");										// zoom out	
		else if (f == 4 && t == 7) SwitchWorld ("third", "seventh");									// zoom out
		else if (f == 4 && t == 8) SwitchWorld ("third", "seventh");									// zoom out
		else if (f == 4 && t == 9) SwitchWorld ("third", "ninth");										// zoom out

		///// fifth \\\\\

		if (f == 5 && t == 0) SwitchWorld ("fifth", "zero");											// zoom in
		else if (f == 5 && t == 1) SwitchWorld ("fifth", "first");										// zoom in
		else if (f == 5 && t == 2) SwitchWorld ("fifth", "first");										// zoom in	
		else if (f == 5 && t == 3) SwitchWorld ("fifth", "third");										// zoom in
		else if (f == 5 && t == 4) SwitchWorld ("fifth", "third");										// zoom in
		else if (f == 5 && t == 5) SwitchWorld ("fifth", "fifth");										// zoom in
		else if (f == 5 && t == 6) SwitchWorld ("fifth", "fifth");										// zoom in
		else if (f == 5 && t == 7) SwitchWorld ("fifth", "seventh");									// zoom out
		else if (f == 5 && t == 8) SwitchWorld ("fifth", "seventh");									// zoom out
		else if (f == 5 && t == 9) SwitchWorld ("fifth", "ninth");										// zoom out

		///// sixth \\\\\

		if (f == 6 && t == 0) SwitchWorld ("fifth", "zero");											// zoom in
		else if (f == 6 && t == 1) SwitchWorld ("fifth", "first");										// zoom in
		else if (f == 6 && t == 2) SwitchWorld ("fifth", "first");										// zoom in
		else if (f == 6 && t == 3) SwitchWorld ("fifth", "third");										// zoom in
		else if (f == 6 && t == 4) SwitchWorld ("fifth", "third");										// zoom in
		else if (f == 6 && t == 5) SwitchWorld ("fifth", "fifth");										// zoom in
		else if (f == 6 && t == 6) SwitchWorld ("fifth", "fifth");										// zoom in
		else if (f == 6 && t == 7) SwitchWorld ("fifth", "seventh");									// zoom out
		else if (f == 6 && t == 8) SwitchWorld ("fifth", "seventh");									// zoom out
		else if (f == 6 && t == 9) SwitchWorld ("fifth", "ninth");										// zoom out

		///// seventh \\\\\

		if (f == 7 && t == 0) SwitchWorld ("seventh", "zero");											// zoom in
		else if (f == 7 && t == 1) SwitchWorld ("seventh", "first");									// zoom in
		else if (f == 7 && t == 2) SwitchWorld ("seventh", "first");									// zoom in
		else if (f == 7 && t == 3) SwitchWorld ("seventh", "third");									// zoom in
		else if (f == 7 && t == 4) SwitchWorld ("seventh", "third");									// zoom in
		else if (f == 7 && t == 5) SwitchWorld ("seventh", "fifth");									// zoom in	
		else if (f == 7 && t == 6) SwitchWorld ("seventh", "fifth");									// zoom in	
		else if (f == 7 && t == 7) SwitchWorld ("seventh", "seventh");									// zoom in	
		else if (f == 7 && t == 8) SwitchWorld ("seventh", "seventh");									// zoom in	
		else if (f == 7 && t == 9) SwitchWorld ("seventh", "ninth");									// zoom out

		///// eighth \\\\\

		if (f == 8 && t == 0) SwitchWorld ("seventh", "zero");											// zoom in
		else if (f == 8 && t == 1) SwitchWorld ("seventh", "first");									// zoom in
		else if (f == 8 && t == 2) SwitchWorld ("seventh", "first");									// zoom in
		else if (f == 8 && t == 3) SwitchWorld ("seventh", "third");									// zoom in
		else if (f == 8 && t == 4) SwitchWorld ("seventh", "third");									// zoom in
		else if (f == 8 && t == 5) SwitchWorld ("seventh", "fifth");									// zoom in	
		else if (f == 8 && t == 6) SwitchWorld ("seventh", "fifth");									// zoom in	
		else if (f == 8 && t == 7) SwitchWorld ("seventh", "seventh");									// zoom in	
		else if (f == 8 && t == 8) SwitchWorld ("seventh", "seventh");									// zoom in	
		else if (f == 8 && t == 9) SwitchWorld ("seventh", "ninth");									// zoom out

		///// ninth \\\\\

		if (f == 9 && t == 0) SwitchWorld ("ninth", "zero");											// zoom in
		else if (f == 9 && t == 1) SwitchWorld ("ninth", "first");										// zoom in
		else if (f == 9 && t == 2) SwitchWorld ("ninth", "first");										// zoom in
		else if (f == 9 && t == 3) SwitchWorld ("ninth", "third");										// zoom in
		else if (f == 9 && t == 4) SwitchWorld ("ninth", "third");										// zoom in
		else if (f == 9 && t == 5) SwitchWorld ("ninth", "fifth");										// zoom in	
		else if (f == 9 && t == 6) SwitchWorld ("ninth", "fifth");										// zoom in	
		else if (f == 9 && t == 7) SwitchWorld ("ninth", "seventh");									// zoom in
		else if (f == 9 && t == 8) SwitchWorld ("ninth", "seventh");									// zoom in
		else if (f == 9 && t == 9) SwitchWorld ("ninth", "ninth");										// zoom in

	}

	public void ZoomCamera (int f, int t)
	{


	///// zero \\\\\

		//if (f == 0 && t == 0) Zoom (false, "zero", "zero");											// zoom out

		if (f == 0 && t == 1) Zoom (false, "zero", "first");											// zoom out
		else if (f == 0 && t == 2) Zoom (false, "zero", "first");										// zoom out
		else if (f == 0 && t == 3) Zoom (false, "zero", "third");										// zoom out
		else if (f == 0 && t == 4) Zoom (false, "zero", "third");										// zoom out
		else if (f == 0 && t == 5) Zoom (false, "zero", "fifth");										// zoom out
		else if (f == 0 && t == 6) Zoom (false, "zero", "fifth");										// zoom out
		else if (f == 0 && t == 7) Zoom (false, "zero", "seventh");										// zoom out
		else if (f == 0 && t == 8) Zoom (false, "zero", "seventh");										// zoom out
		else if (f == 0 && t == 9) Zoom (false, "zero", "ninth");										// zoom out

	///// first \\\\\
			
		if (f == 1 && t == 0) Zoom (true, "first", "zero");												// zoom out
		else if (f == 1 && t == 3) Zoom (false, "first", "third");										// zoom out
		else if (f == 1 && t == 4) Zoom (false, "first", "third");										// zoom out
		else if (f == 1 && t == 5) Zoom (false, "first", "fifth");										// zoom out	
		else if (f == 1 && t == 6) Zoom (false, "first", "fifth");										// zoom out	
		else if (f == 1 && t == 7) Zoom (false, "first", "seventh");									// zoom out
		else if (f == 1 && t == 8) Zoom (false, "first", "seventh");									// zoom out
		else if (f == 1 && t == 9) Zoom (false, "first", "ninth");										// zoom out

	///// second \\\\\

		if (f == 2 && t == 0) Zoom (true, "first", "zero");												// zoom in
		else if (f == 2 && t == 3) Zoom (false, "first", "third");										// zoom out
		else if (f == 2 && t == 4) Zoom (false, "first", "third");										// zoom out
		else if (f == 2 && t == 5) Zoom (false, "first", "fifth");										// zoom out	
		else if (f == 2 && t == 6) Zoom (false, "first", "fifth");										// zoom out	
		else if (f == 2 && t == 7) Zoom (false, "first", "seventh");									// zoom out
		else if (f == 2 && t == 8) Zoom (false, "first", "seventh");									// zoom out
		else if (f == 2 && t == 9) Zoom (false, "first", "ninth");										// zoom out

	///// third \\\\\
			
		if (f == 3 && t == 0) Zoom (true, "third", "zero");												// zoom in
		else if (f == 3 && t == 1) Zoom (true, "third", "first");										// zoom in
		else if (f == 3 && t == 2) Zoom (true, "third", "first");										// zoom in	
		else if (f == 3 && t == 5) Zoom (false, "third", "fifth");										// zoom out	
		else if (f == 3 && t == 6) Zoom (false, "third", "fifth");										// zoom out	
		else if (f == 3 && t == 7) Zoom (false, "third", "seventh");									// zoom out
		else if (f == 3 && t == 8) Zoom (false, "third", "seventh");									// zoom out
		else if (f == 3 && t == 9) Zoom (false, "third", "ninth");										// zoom out

	///// fourth \\\\\

		if (f == 4 && t == 0) Zoom (true, "third", "zero");												// zoom in
		else if (f == 4 && t == 1) Zoom (true, "third", "first");										// zoom in
		else if (f == 4 && t == 2) Zoom (true, "third", "first");										// zoom in
		else if (f == 4 && t == 5) Zoom (false, "third", "fifth");										// zoom out	
		else if (f == 4 && t == 6) Zoom (false, "third", "fifth");										// zoom out	
		else if (f == 4 && t == 7) Zoom (false, "third", "seventh");									// zoom out
		else if (f == 4 && t == 8) Zoom (false, "third", "seventh");									// zoom out
		else if (f == 4 && t == 9) Zoom (false, "third", "ninth");										// zoom out

	///// fifth \\\\\

		if (f == 5 && t == 0) Zoom (true, "fifth", "zero");												// zoom in
		else if (f == 5 && t == 1) Zoom (true, "fifth", "first");										// zoom in
		else if (f == 5 && t == 2) Zoom (true, "fifth", "first");										// zoom in	
		else if (f == 5 && t == 3) Zoom (true, "fifth", "third");										// zoom in
		else if (f == 5 && t == 4) Zoom (true, "fifth", "third");										// zoom in
		else if (f == 5 && t == 7) Zoom (false, "fifth", "seventh");									// zoom out
		else if (f == 5 && t == 8) Zoom (false, "fifth", "seventh");									// zoom out
		else if (f == 5 && t == 9) Zoom (false, "fifth", "ninth");										// zoom out

	///// sixth \\\\\

		if (f == 6 && t == 0) Zoom (true, "fifth", "zero");												// zoom in
		else if (f == 6 && t == 1) Zoom (true, "fifth", "first");										// zoom in
		else if (f == 6 && t == 2) Zoom (true, "fifth", "first");										// zoom in
		else if (f == 6 && t == 3) Zoom (true, "fifth", "third");										// zoom in
		else if (f == 6 && t == 4) Zoom (true, "fifth", "third");										// zoom in
		else if (f == 6 && t == 7) Zoom (false, "fifth", "seventh");									// zoom out
		else if (f == 6 && t == 8) Zoom (false, "fifth", "seventh");									// zoom out
		else if (f == 6 && t == 9) Zoom (false, "fifth", "ninth");										// zoom out

	///// seventh \\\\\

		if (f == 7 && t == 0) Zoom (true, "seventh", "zero");											// zoom in
		else if (f == 7 && t == 1) Zoom (true, "seventh", "first");										// zoom in
		else if (f == 7 && t == 2) Zoom (true, "seventh", "first");										// zoom in
		else if (f == 7 && t == 3) Zoom (true, "seventh", "third");										// zoom in
		else if (f == 7 && t == 4) Zoom (true, "seventh", "third");										// zoom in
		else if (f == 7 && t == 5) Zoom (true, "seventh", "fifth");										// zoom in	
		else if (f == 7 && t == 6) Zoom (true, "seventh", "fifth");										// zoom in	
		else if (f == 7 && t == 9) Zoom (false, "seventh", "ninth");									// zoom out

	///// eighth \\\\\

		if (f == 8 && t == 0) Zoom (true, "seventh", "zero");											// zoom in
		else if (f == 8 && t == 1) Zoom (true, "seventh", "first");										// zoom in
		else if (f == 8 && t == 2) Zoom (true, "seventh", "first");										// zoom in
		else if (f == 8 && t == 3) Zoom (true, "seventh", "third");										// zoom in
		else if (f == 8 && t == 4) Zoom (true, "seventh", "third");										// zoom in
		else if (f == 8 && t == 5) Zoom (true, "seventh", "fifth");										// zoom in	
		else if (f == 8 && t == 6) Zoom (true, "seventh", "fifth");										// zoom in	
		else if (f == 8 && t == 9) Zoom (false, "seventh", "ninth");									// zoom out

	///// ninth \\\\\

		if (f == 9 && t == 0) Zoom (true, "ninth", "zero");												// zoom in
		else if (f == 9 && t == 1) Zoom (true, "ninth", "first");										// zoom in
		else if (f == 9 && t == 2) Zoom (true, "ninth", "first");										// zoom in
		else if (f == 9 && t == 3) Zoom (true, "ninth", "third");										// zoom in
		else if (f == 9 && t == 4) Zoom (true, "ninth", "third");										// zoom in
		else if (f == 9 && t == 5) Zoom (true, "ninth", "fifth");										// zoom in	
		else if (f == 9 && t == 6) Zoom (true, "ninth", "fifth");										// zoom in	
		else if (f == 9 && t == 7) Zoom (true, "ninth", "seventh");										// zoom in
		else if (f == 9 && t == 8) Zoom (true, "ninth", "seventh");										// zoom in

		else if (f == 9 && t == 10) Zoom (false, "ninth", "tenth");										// zoom out
	
	}

	public void SwitchWorld (string resetState, string setState) {

			anim.SetBool ("switchworld", true);															// set switchworld

			anim.SetBool (resetState, false);															// reset previously active state
			anim.SetBool (setState, true);																// set new active state
			
			anim.ResetTrigger ("zoomout");																// reset scale up trigger
			anim.SetTrigger ("zoomin");																	// set scale down trigger

			resetZoom = true;																			// set reset zoom flag
	}

	public void Zoom (bool devol, string resetState, string setState) {

			if (devol) {																				// if devol true
				anim.ResetTrigger ("zoomout");																// reset scale up trigger
				anim.SetTrigger ("zoomin");																	// set scale down trigger
			}																								
			else {																						// else
				anim.ResetTrigger ("zoomin");																// reset scale down trigger
				anim.SetTrigger ("zoomout");																// set scale up trigger
			}																											

			anim.SetBool (resetState, false);															// reset previously active state
			anim.SetBool (setState, true);																// set new active state

	}

}
