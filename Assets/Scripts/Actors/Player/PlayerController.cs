using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public bool godMode = false;

	public float sprintSpeed = 10;
	public float floatSpeed = 10;
	public float smoothingFactor = 50;

	public Vector3 moveDir;
	public Vector3 force;
	public float velocity;

	private Rigidbody rb;

	private UIManager uim;

	void Start() {
		// init components
		rb = GetComponent<Rigidbody> ();
		uim = GetComponent<UIManager> ();

		if (godMode) {
			sprintSpeed = 20;
			floatSpeed = 15;
		}

		// init moveDir
		moveDir = new Vector3 (0.0f, 0.0f, 0.0f).normalized;	

	}

	void FixedUpdate() {
		// input
		if (!uim.uI.GetComponent<StartOptions>().inMainMenu) {
			// how fast moveDir increases
			moveDir.x += (Input.GetAxisRaw ("Horizontal") / smoothingFactor);
			moveDir.z += (Input.GetAxisRaw ("Vertical") / smoothingFactor);
		}

		// speed change sensitivity/max moveDir threshold
		moveDir = Vector3.ClampMagnitude (moveDir, smoothingFactor);

		// movement

		force = (moveDir * sprintSpeed);			// calculate force for inspector visibility / move direction * sprint threshold (larget to account for mass)
		rb.AddRelativeForce (force * rb.mass);		// apply force as a factor of mass

		velocity = rb.velocity.magnitude;			// velocity for inspector visibility

		// float speed clamp
		if (rb.velocity.magnitude > floatSpeed) {									// if > float speed	
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, floatSpeed);				// clamp to float speed
		}
	}

	void Update() {
		if (Input.GetButtonDown ("Restart"))
			SceneManager.LoadScene("Sequence1");									// restart scene		
	}
		
}
