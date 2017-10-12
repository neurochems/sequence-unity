/* From: https://www.youtube.com/watch?v=gHeQ8Hr92P4 */

using UnityEngine;
using System.Collections;

public class FauxGravityAttractor : MonoBehaviour {

	public float gravity = -10;

	private PlayerStatePattern psp;														// player state pattern

	//private Rigidbody rb;

	//private float photonRadius = 100.2f;
	//private float electronRadius = 101.0f;
	//private float shellRadius = 102.4f;

	void Start() {
		psp = GameObject.Find ("Player").GetComponent<PlayerStatePattern> ();			// get psp
		//rb = GetComponent<Rigidbody>();
	}

	void Update() 
	{
		if (psp.state == 10)
			gravity = -9.8f;
	}

	public void Attract(Transform body) {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 bodyUp = body.up;

		// gravity force
		body.GetComponent<Rigidbody>().AddForce (gravityUp * gravity);

		// attempt at calculating Newton's universal gravitational force, need to be a Vector3
		//body.GetComponent<Rigidbody>().AddForce (6.674e-11 * ((rb.mass * body.GetComponent<Rigidbody>().mass) / (Mathf.Pow(electronRadius, 2.0f))));

		// rotation
		Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;

		// rotate body
		body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
	}
}
