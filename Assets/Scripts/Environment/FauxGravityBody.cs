/* From: https://www.youtube.com/watch?v=gHeQ8Hr92P4 */

using UnityEngine;
using System.Collections;

public class FauxGravityBody : MonoBehaviour {

	public FauxGravityAttractor attractor;
	private Transform myTransform;

	void Start () {
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		GetComponent<Rigidbody>().useGravity = false;
		myTransform = transform;
	}
	
	void FixedUpdate () {
		attractor.Attract(myTransform);	
	}
}