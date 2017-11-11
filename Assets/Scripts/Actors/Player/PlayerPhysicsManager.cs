/* From: https://www.youtube.com/watch?v=gHeQ8Hr92P4 */

using UnityEngine;
using System.Collections;

public class PlayerPhysicsManager : MonoBehaviour {

	public FauxGravityAttractor attractor;
	private Transform playerTransform;

	private Rigidbody rb;

	[HideInInspector] public bool bump;
	public int bumpStrength = 100;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.useGravity = false;
		playerTransform = transform;
	}
	
	void FixedUpdate () {
		attractor.Attract(playerTransform);

		if (bump) {																								// bump away at collision
			//rb.AddRelativeForce (Vector3.Lerp(rb.velocity, (rb.velocity * -bumpStrength), 1.0f));				// lerp force of velocity * a factor in the opposite direction
			//Debug.Log ("particle collision bump");
			bump = false;																						// reset collision trigger
		}
	}

	public void Bump(bool toggle) {
		bump = toggle;
	}

}
