using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Animator anim;							// animator on camera ref

	void Awake () {
		anim = GetComponent<Animator>();			// init animator ref
	}
	
	public void ZoomCamera (int fromState, int toState) 
	{
		// EVOLUTIONS \\

		// from zero
			// to first
		if (fromState == 0 && toState == 1) ZoomTo (false, "zero", "first");			// zoom out
			// to third
		else if (fromState == 0 && toState == 3) ZoomTo (false, "zero", "third");		// zoom out
			// to seventh
		else if (fromState == 0 && toState == 7) ZoomTo (false, "zero", "seventh");		// zoom out
			// to ninth
		else if (fromState == 0 && toState == 9) ZoomTo (false, "zero", "ninth");		// zoom out

		// from first
			// to third
		if (fromState == 1 && toState == 3) ZoomTo (false, "first", "third");			// zoom out
			// to seventh
		else if (fromState == 1 && toState == 7) ZoomTo (false, "first", "seventh");	// zoom out
			// to ninth
		else if (fromState == 1 && toState == 9) ZoomTo (false, "first", "ninth");		// zoom out

		// from third
			// to seventh
		if (fromState == 3 && toState == 7) ZoomTo (false, "third", "seventh");			// zoom out
			// to ninth
		else if (fromState == 3 && toState == 9) ZoomTo (false, "third", "ninth");		// zoom out
			
		// from seventh
			// to ninth
		if (fromState == 7 && toState == 9) ZoomTo (false, "seventh", "ninth");			// zoom out

		// from ninth
			// to tenth
		if (fromState == 9 && toState == 10) ZoomTo (false, "ninth", "tenth");			// zoom out

		// DEVOLUTIONS \\

		// from first
			// to zero
		if (fromState == 1 && toState == 0) ZoomTo (true, "first", "zero");				// zoom in

		// from third
			// to first
		if (fromState == 3 && toState == 1) ZoomTo (true, "third", "first");			// zoom in
			// to zero
		else if (fromState == 3 && toState == 0) ZoomTo (true, "third", "zero");		// zoom in

		// from seventh
			// to third
		if (fromState == 7 && toState == 3) ZoomTo (true, "seventh", "third");			// zoom in
			// to first
		else if (fromState == 7 && toState == 1) ZoomTo (true, "seventh", "first");		// zoom in
			// to zero
		else if (fromState == 7 && toState == 0) ZoomTo (true, "seventh", "zero");		// zoom in

		// from ninth
			// to seventh
		if (fromState == 9 && toState == 7) ZoomTo (true, "ninth", "seventh");			// zoom in
			// to third
		else if (fromState == 9 && toState == 3) ZoomTo (true, "ninth", "third");		// zoom in
			// to first
		else if (fromState == 9 && toState == 1) ZoomTo (true, "ninth", "first");		// zoom in
			// to zero
		else if (fromState == 9 && toState == 0) ZoomTo (true, "ninth", "zero");		// zoom in

	}

	private void ZoomTo (bool devol, string resetState, string setState) {
		
		if (devol) {										// if devol true
			anim.ResetTrigger ("zoomout");						// reset scale up trigger
			anim.SetTrigger ("zoomin");							// set scale down trigger
		}																								
		else {												// else
			anim.ResetTrigger ("zoomin");						// reset scale down trigger
			anim.SetTrigger ("zoomout");						// set scale up trigger
		}																											

		anim.SetBool (resetState, false);					// reset previously active state
		anim.SetBool (setState, true);						// set new active state
	}

}
