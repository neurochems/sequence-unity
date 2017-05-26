using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Animator anim;							// animator on camera ref

	void Awake () {
		anim = GetComponent<Animator>();			// init animator ref
	}
	
	public void ZoomCamera (bool switchworld, int fromState, int toState)
	{

		// EVOLUTIONS \\

		// from zero
			// to zero
		if (fromState == 0 && toState == 0) {
			ZoomTo (switchworld, true, "zero", "zero");		// zoom out
		}
			// to first
		//if (switchworld) {
		if (fromState == 0 && toState == 1) {
			ZoomTo (switchworld, false, "zero", "first");		// zoom out
		}
		//}
			// to third
		else if (fromState == 0 && toState == 3) ZoomTo (switchworld, false, "zero", "third");		// zoom out
			// to fifth
		else if (fromState == 0 && toState == 5) ZoomTo (switchworld, false, "zero", "fifth");		// zoom out
			// to seventh
		else if (fromState == 0 && toState == 7) ZoomTo (switchworld, false, "zero", "seventh");	// zoom out
			// to ninth
		else if (fromState == 0 && toState == 9) ZoomTo (switchworld, false, "zero", "ninth");		// zoom out

		// from first
			// to third
		if (fromState == 1 && toState == 3)	ZoomTo (switchworld, false, "first", "third");			// zoom out
			// to fifth
		else if (fromState == 1 && toState == 5) ZoomTo (switchworld, false, "first", "fifth");		// zoom out	
			// to seventh
		else if (fromState == 1 && toState == 7) ZoomTo (switchworld, false, "first", "seventh");	// zoom out
			// to ninth
		else if (fromState == 1 && toState == 9) ZoomTo (switchworld, false, "first", "ninth");		// zoom out

		// from third
			// to fifth
		if (fromState == 3 && toState == 5) ZoomTo (switchworld, false, "third", "fifth");			// zoom out	
			// to seventh
		else if (fromState == 3 && toState == 7) ZoomTo (switchworld, false, "third", "seventh");	// zoom out
			// to ninth
		else if (fromState == 3 && toState == 9) ZoomTo (switchworld, false, "third", "ninth");		// zoom out

		// from fifth
			// to seventh
		if (fromState == 5 && toState == 7) ZoomTo (switchworld, false, "fifth", "seventh");		// zoom out
			// to ninth
		else if (fromState == 5 && toState == 9) ZoomTo (switchworld, false, "fifth", "ninth");		// zoom out

		// from seventh
			// to ninth
		if (fromState == 7 && toState == 9)	ZoomTo (switchworld, false, "seventh", "ninth");		// zoom out

		// from ninth
			// to tenth
		if (fromState == 9 && toState == 10) ZoomTo (switchworld, false, "ninth", "tenth");			// zoom out

		// DEVOLUTIONS \\

		// from first
			// to zero
		if (fromState == 1 && toState == 0) ZoomTo (switchworld, true, "first", "zero");			// zoom in
			
		// from third
			// to first
		if (fromState == 3 && toState == 1)	ZoomTo (switchworld, true, "third", "first");			// zoom in
			// to zero
		else if (fromState == 3 && toState == 0) ZoomTo (switchworld, true, "third", "zero");		// zoom in
			
		// from fifth
			// to third
		if (fromState == 5 && toState == 3)	ZoomTo (switchworld, true, "fifth", "third");			// zoom in
			// to first
		else if (fromState == 5 && toState == 1) ZoomTo (switchworld, true, "fifth", "first");		// zoom in
			// to zero
		else if (fromState == 5 && toState == 0) ZoomTo (switchworld, true, "fifth", "zero");		// zoom in

		// from seventh
			// to fifth
		if (fromState == 7 && toState == 5)	ZoomTo (switchworld, true, "seventh", "fifth");			// zoom in	
			// to third
		else if (fromState == 7 && toState == 3) ZoomTo (switchworld, true, "seventh", "third");	// zoom in
			// to first
		else if (fromState == 7 && toState == 1) ZoomTo (switchworld, true, "seventh", "first");	// zoom in
			// to zero
		else if (fromState == 7 && toState == 0) ZoomTo (switchworld, true, "seventh", "zero");		// zoom in
			
		// from ninth
			// to seventh
		if (fromState == 9 && toState == 7) ZoomTo (switchworld, true, "ninth", "seventh");			// zoom in
			// to fifth
		else if (fromState == 9 && toState == 5) ZoomTo (switchworld, true, "ninth", "fifth");		// zoom in	
			// to third
		else if (fromState == 9 && toState == 3) ZoomTo (switchworld, true, "ninth", "third");		// zoom in
			// to first
		else if (fromState == 9 && toState == 1) ZoomTo (switchworld, true, "ninth", "first");		// zoom in
			// to zero
		else if (fromState == 9 && toState == 0) ZoomTo (switchworld, true, "ninth", "zero");		// zoom in
	
	}

	private void ZoomTo (bool switchworld, bool devol, string resetState, string setState) {

		// private void SwitchTo (bool devol, string resetState, string setState) {}
		// FIX THIS 

		if (switchworld) {
			anim.SetBool ("lightworld", true);											// set lightworld to switch worlds

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

			anim.SetBool ("lightworld", false);															// reset lightworld to not keep switching worlds
		}
			
		else {

			//Debug.Log ("camera zoom out");

			anim.SetBool ("lightworld", false);													// reset lightworld to not switch worlds

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
