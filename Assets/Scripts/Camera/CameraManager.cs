using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Animator anim;																				// animator on camera ref
	private PlayerStatePattern psp;																		// psp ref

	private bool switchWorld;																			// switch world flag

	void Start () {
		anim = GetComponent<Animator>();																// init animator ref
		psp = GetComponentInParent<PlayerStatePattern>();												// init psp ref
	}
	
	public void ZoomCamera (bool switchworld, int fromState, int toState)
	{

		switchWorld = switchworld;																		// set switch world flag

		// EVOLUTIONS \\

		// from light world zero
			// to dark world zero
		if (switchworld && fromState == 0 && toState == 0) {
			Debug.Log ("camera zero to zero");
			ZoomTo (true, "zero", "zero");		// zoom out
		}
		if (!switchworld && fromState == 0 && toState == 0) {
			Debug.Log ("camera zero reset to zero");
			ZoomTo (false, "zero", "zero");		// zoom out
		}
			// to light world first
		if (switchworld && fromState == 0 && toState == 1) {
			Debug.Log ("camera zero to light world first");
			ZoomTo (true, "zero", "first");		// zoom out
		}
			// to first
		if (!switchworld && fromState == 0 && toState == 1) {
			Debug.Log ("camera zero to first");
			ZoomTo (false, "zero", "first");		// zoom out
		}
			// to third
		else if (fromState == 0 && toState == 3) ZoomTo (false, "zero", "third");						// zoom out
			// to fifth
		else if (fromState == 0 && toState == 5) ZoomTo (false, "zero", "fifth");						// zoom out
			// to seventh
		else if (fromState == 0 && toState == 7) ZoomTo (false, "zero", "seventh");						// zoom out
			// to ninth
		else if (fromState == 0 && toState == 9) ZoomTo (false, "zero", "ninth");						// zoom out

		// from first
			// to third
		if (fromState == 1 && toState == 3)	ZoomTo (false, "first", "third");							// zoom out
			// to fifth
		else if (fromState == 1 && toState == 5) ZoomTo (false, "first", "fifth");						// zoom out	
			// to seventh
		else if (fromState == 1 && toState == 7) ZoomTo (false, "first", "seventh");					// zoom out
			// to ninth
		else if (fromState == 1 && toState == 9) ZoomTo (false, "first", "ninth");						// zoom out

		// from second
			// to third
		if (fromState == 2 && toState == 3)	ZoomTo (false, "first", "third");							// zoom out
			// to fifth
		else if (fromState == 2 && toState == 5) ZoomTo (false, "first", "fifth");						// zoom out	
			// to seventh
		else if (fromState == 2 && toState == 7) ZoomTo (false, "first", "seventh");					// zoom out
			// to ninth
		else if (fromState == 2 && toState == 9) ZoomTo (false, "first", "ninth");						// zoom out

		// from third
			// to fifth
		if (fromState == 3 && toState == 5) ZoomTo (false, "third", "fifth");							// zoom out	
			// to seventh
		else if (fromState == 3 && toState == 7) ZoomTo (false, "third", "seventh");					// zoom out
			// to ninth
		else if (fromState == 3 && toState == 9) ZoomTo (false, "third", "ninth");						// zoom out

		// from fourth
			// to fifth
		if (fromState == 4 && toState == 5) ZoomTo (false, "third", "fifth");							// zoom out	
			// to seventh
		else if (fromState == 4 && toState == 7) ZoomTo (false, "third", "seventh");					// zoom out
			// to ninth
		else if (fromState == 4 && toState == 9) ZoomTo (false, "third", "ninth");						// zoom out

		// from fifth
			// to seventh
		if (fromState == 5 && toState == 7) ZoomTo (false, "fifth", "seventh");							// zoom out
			// to ninth
		else if (fromState == 5 && toState == 9) ZoomTo (false, "fifth", "ninth");						// zoom out

		// from sixth
			// to seventh
		if (fromState == 6 && toState == 7) ZoomTo (false, "fifth", "seventh");							// zoom out
			// to ninth
		else if (fromState == 6 && toState == 9) ZoomTo (false, "fifth", "ninth");						// zoom out

		// from seventh
			// to ninth
		if (fromState == 7 && toState == 9)	ZoomTo (false, "seventh", "ninth");							// zoom out

		// from eighth
			// to ninth
		if (fromState == 8 && toState == 9)	ZoomTo (false, "seventh", "ninth");							// zoom out

		// from ninth
			// to tenth
		if (fromState == 9 && toState == 10) ZoomTo (false, "ninth", "tenth");							// zoom out


		// DEVOLUTIONS \\


		// from first
			// to zero
		if (fromState == 1 && toState == 0) ZoomTo (true, "first", "zero");								// zoom in
			
		// from light world first
			// to dark world zero
		if (psp.resetScale && fromState == 1 && toState == 0) ZoomTo (false, "first", "zero");			// zoom out

		// from third
			// to second
		if (fromState == 3 && toState == 2)	ZoomTo (true, "third", "first");							// zoom in	
			// to first
		else if (fromState == 3 && toState == 1) ZoomTo (true, "third", "first");						// zoom in
			// to zero
		else if (fromState == 3 && toState == 0) ZoomTo (true, "third", "zero");						// zoom in

		// from light world third
			// to dark world zero
		if (psp.resetScale && fromState == 3 && toState == 0) ZoomTo (false, "third", "zero");			// zoom out

		// from fifth
			// to fourth
		if (fromState == 5 && toState == 4) ZoomTo (true, "fifth", "third");							// zoom in
			// to third
		else if (fromState == 5 && toState == 3) ZoomTo (true, "fifth", "third");						// zoom in
			// to second
		else if (fromState == 3 && toState == 2) ZoomTo (true, "third", "first");						// zoom in	
			// to first
		else if (fromState == 5 && toState == 1) ZoomTo (true, "fifth", "first");						// zoom in
			// to zero
		else if (fromState == 5 && toState == 0) ZoomTo (true, "fifth", "zero");						// zoom in

		// from light world fifth
			// to dark world zero
		if (psp.resetScale && fromState == 5 && toState == 0) ZoomTo (false, "fifth", "zero");			// zoom out

		// from seventh
			// to sixth
		if (fromState == 7 && toState == 6)	ZoomTo (true, "seventh", "fifth");							// zoom in	
			// to fifth
		else if (fromState == 7 && toState == 5) ZoomTo (true, "seventh", "fifth");						// zoom in	
			// to fourth
		else if (fromState == 5 && toState == 4) ZoomTo (true, "fifth", "third");						// zoom in
			// to third
		else if (fromState == 7 && toState == 3) ZoomTo (true, "seventh", "third");						// zoom in
			// to second
		else if (fromState == 3 && toState == 2) ZoomTo (true, "third", "first");						// zoom in
			// to first
		else if (fromState == 7 && toState == 1) ZoomTo (true, "seventh", "first");						// zoom in
			// to zero
		else if (fromState == 7 && toState == 0) ZoomTo (true, "seventh", "zero");						// zoom in

		// from light world seventh
			// to dark world zero
		if (psp.resetScale && fromState == 7 && toState == 0) ZoomTo (false, "seventh", "zero");			// zoom out

		// from ninth
			// to eighth
		if (fromState == 9 && toState == 8) ZoomTo (true, "ninth", "seventh");							// zoom in
			// to seventh
		else if (fromState == 9 && toState == 7) ZoomTo (true, "ninth", "seventh");						// zoom in
			// to sixth
		else if (fromState == 7 && toState == 6) ZoomTo (true, "seventh", "fifth");						// zoom in	
			// to fifth
		else if (fromState == 9 && toState == 5) ZoomTo (true, "ninth", "fifth");						// zoom in	
			// to fourth
		else if (fromState == 5 && toState == 4) ZoomTo (true, "fifth", "third");						// zoom in
			// to third
		else if (fromState == 9 && toState == 3) ZoomTo (true, "ninth", "third");						// zoom in
			// to second
		else if (fromState == 3 && toState == 2) ZoomTo (true, "third", "first");						// zoom in
			// to first
		else if (fromState == 9 && toState == 1) ZoomTo (true, "ninth", "first");						// zoom in
			// to zero
		else if (fromState == 9 && toState == 0) ZoomTo (true, "ninth", "zero");						// zoom in

		// from light world ninth
			// to dark world zero
		if (psp.resetScale && fromState == 7 && toState == 0) ZoomTo (false, "ninth", "zero");			// zoom out
	
	}

	public void ZoomTo (bool devol, string resetState, string setState) {

		// private void SwitchTo (bool devol, string resetState, string setState) {}
		// FIX THIS 
		Debug.Log("camera ZoomTo switchWorld: " + switchWorld);
		if (switchWorld) {
			//if (psp.lightworld) {																		// if in light world
			//	Debug.Log ("camera to dark world");
			//	anim.SetBool ("switchworld", false);														// reset switchworld
			//}
			//else if (!psp.lightworld) {																		// if in dark world
				Debug.Log ("camera: switch world");
				anim.SetBool ("switchworld", true);															// set switchworld
			//}
			if (devol) {																				// if devol true
				anim.ResetTrigger ("zoomout");																// reset scale up trigger
				anim.SetTrigger ("zoomin");																	// set scale down trigger
			}																								
			else {																						// else
				anim.ResetTrigger ("zoomin");																// reset scale down trigger
				anim.SetTrigger ("zoomout");																// set scale up trigger
			}																											

			//anim.SetBool (resetState, false);															// reset previously active state
			//anim.SetBool (setState, true);																// set new active state

			//anim.SetBool ("lightworld", false);															// reset lightworld to not keep switching worlds
		}
			
		else {

			//Debug.Log ("camera zoom out");

			anim.SetBool ("switchworld", false);													// reset lightworld to not switch worlds

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

}
