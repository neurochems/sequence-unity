using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	private PlayerStatePattern psp;														// player state pattern
	private Animator anim;																// animator

	void Start () {
		psp = GameObject.Find ("Player").GetComponent<PlayerStatePattern> ();			// get psp
		anim = GetComponent<Animator> ();												// get animator
	}

	void Update () {
		if (psp.state == 10) {
			anim.SetBool("hidden", true);							// set hidden
			anim.SetBool("default", false);							// reset default
			anim.SetTrigger("scaledown");							// trigger scaledown
		}
	}
}
