using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public bool godMode = false;
	public bool chill;

	public AnimationCurve accelCurve;
	float accelTime = 0;

	public float accelSpeed = 10f;
	public float floatSpeed = 10f;
	public float weight = 50f;
	public float decelFactor = 0.003f;

	public Vector3 moveDir;
	public Vector3 vectorAccel;
	public float velocity;

	private Rigidbody rb;

	private PlayerStatePattern psp;
	private UIManager uim;
	private StartOptions so;

	void Start() {
		// init components
		rb = GetComponent<Rigidbody> ();
		uim = GameObject.Find("Player").GetComponent<UIManager> ();
		so = GameObject.Find("Player").GetComponent<UIManager> ().ui.GetComponent<StartOptions>();
		psp = GetComponent<PlayerStatePattern> ();

		if (godMode) {
			accelSpeed = 20f;
			floatSpeed = 15f;
		}

		// init moveDir
		moveDir = new Vector3 (0.0f, 0.0f, 0.0f).normalized;	

	}

	void FixedUpdate() {

		// input

		if (!so.inMainMenu) {
			// how fast moveDir increases
			moveDir.x += (Input.GetAxisRaw ("Horizontal") / weight);
			moveDir.z += (Input.GetAxisRaw ("Vertical") / weight);
		}

		// speed change sensitivity/max moveDir threshold
		moveDir = Vector3.ClampMagnitude (moveDir, weight);

		// movement

		if(Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical")!=0){
			accelTime += Time.fixedDeltaTime;
		}else{
			accelTime -= Time.fixedDeltaTime * 0.003f;
		}

		accelSpeed = accelCurve.Evaluate (accelTime);

		// physics

		vectorAccel = (moveDir * accelSpeed);				// calculate force for inspector visibility / move direction * sprint threshold (larget to account for mass)

		velocity = rb.velocity.magnitude;					// velocity for inspector visibility

		rb.AddRelativeForce (rb.mass * vectorAccel);								// apply force = mass * accel

		// float speed clamp
		if (rb.velocity.magnitude > floatSpeed) {									// if < float speed	
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, floatSpeed);				// clamp to float speed
		}
	}

	void Update() {

		if (chill) {
			accelSpeed = 5.0f;
			floatSpeed = 2.0f;
		}
		else {
			accelSpeed = 6.0f;
			floatSpeed = 4.0f;
		}

		if (Input.GetButtonDown ("Restart")) {
			uim.ui.gameObject.tag = "Destroy";
			SceneManager.LoadScene("Sequence1");									// restart scene		
		}
	}

	public void Chill(bool toggle) {
		chill = toggle;
	}
		
}
