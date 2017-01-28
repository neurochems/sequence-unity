/* From: https://www.youtube.com/watch?v=gHeQ8Hr92P4 */

using UnityEngine;
using System.Collections;

public class ParticlePhysicsManager : MonoBehaviour {

	public FauxGravityAttractor attractor;
	private Transform particleTransform;

	// movement/physics variables
	public float moveSpeed = 7;
	public float maxSpeed = 10;
	public float xMin, xMax, zMin, zMax;
	private Vector3 moveDir;

	private Rigidbody rb;
	[HideInInspector] public bool bump = false;
	public int bumpStrength = 100;

	void Start () {
		//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		GetComponent<Rigidbody>().useGravity = false;
		particleTransform = transform;

		rb = GetComponent<Rigidbody> ();

		xMin = Random.Range (-1.0f, 1.0f);												// randomize movement headings
		xMax = Random.Range (-1.0f, 1.0f);
		zMin = Random.Range (-1.0f, 1.0f);
		zMax = Random.Range (-1.0f, 1.0f);
	}
	
	void FixedUpdate () {
		attractor.Attract(particleTransform);

		moveDir = new Vector3 (Random.Range(xMin, xMax), 0.0f, Random.Range(zMin, zMax)).normalized;		// create move direction

		rb.AddRelativeForce (moveDir * moveSpeed); 																// add force in movement direction

		if (rb.velocity.magnitude > maxSpeed) {																	// clamp at max speed
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
		}

		if (bump) {																								// bump away at collision
			rb.AddRelativeForce (Vector3.Lerp(rb.velocity, (rb.velocity * -bumpStrength), 1.0f));				// lerp force of velocity * a factor in the opposite direction
			//Debug.Log ("particle collision bump");
			bump = false;																						// reset collision trigger
		}
	}

	public void Bump(bool toggle) {
		bump = toggle;
	}

}