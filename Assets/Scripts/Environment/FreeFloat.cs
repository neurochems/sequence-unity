using UnityEngine;
using System.Collections;

public class FreeFloat : MonoBehaviour {

	private PlayerStatePattern psp;														// player state pattern
	private Rigidbody rb;																// rigidbody

	void Start () {
		psp = GameObject.Find ("Player").GetComponent<PlayerStatePattern> ();			// get psp
		rb = GetComponent<Rigidbody> ();												// get rigidbody
	}
	
	void Update () {
		if (psp.state == 10) rb.isKinematic = false;									// reset is kinematic
	}
}
