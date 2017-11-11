/* From: https://www.youtube.com/watch?v=gHeQ8Hr92P4 */

using UnityEngine;
using System.Collections;

public class ParticlePhysicsManager : MonoBehaviour {

	public FauxGravityAttractor attractor;
	private Transform particleTransform;

	// movement/physics variables
	public bool chill;
	public float moveSpeed = 7;
	public float maxSpeed = 10;
	public float xMin, xMax, zMin, zMax;
	private Vector3 moveDir;

	private PlayerController pc;
	private Rigidbody rb;
	private bool boost = true;
	private float boostTimer = 0f;

	[HideInInspector] public bool bump = false;
	public int bumpStrength = 100;

	void Start () {
		//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		GetComponent<Rigidbody>().useGravity = false;
		particleTransform = transform;

		pc = GameObject.Find("Player").GetComponent<PlayerController>();

		rb = GetComponent<Rigidbody> ();

		xMin = Random.Range (-1.0f, 1.0f);												// randomize movement headings
		xMax = Random.Range (-1.0f, 1.0f);
		zMin = Random.Range (-1.0f, 1.0f);
		zMax = Random.Range (-1.0f, 1.0f);
	}
	
	void FixedUpdate () {
		attractor.Attract(particleTransform);

		chill = pc.chill;

		if (chill) {
			moveSpeed = 1.0f;
			maxSpeed = 2.0f;
		}
		else {
			moveSpeed = 2.0f;
			maxSpeed = 4.0f;
		}

		if (gameObject.tag == "World") {
			moveSpeed = 35.0f;
			maxSpeed = 350.0f;
		}

		if (boost) {
			moveDir = new Vector3 (Random.Range(xMin, xMax), 0.0f, Random.Range(zMin, zMax)).normalized;		// create move direction
			rb.AddRelativeForce (moveDir * moveSpeed); 															// add force in movement direction
			boost = false;																					// reset kickstart
		}

		if (!boost) {
			boostTimer += Time.deltaTime;
			if (boostTimer >= 10.0f) boost = true;
		}

		if (rb.velocity.magnitude > maxSpeed) {																	// clamp at max speed
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
		}

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