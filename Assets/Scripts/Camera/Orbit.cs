using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Transform player;
	public Transform core;
	private PlayerStatePattern psp;

	public float turnSpeed;
	public float maxTurnSpeed;
	public float accelRate;
	public float resetRate;

	public Vector3 offset;
	private Vector3 defaultEuler;
	private Vector3 velocity = Vector3.zero;																			// velocity of camera
	public bool resetStop = false;

	void Start () 
	{
		offset = new Vector3(player.position.x, player.position.y + 8.0f, player.position.z + 7.0f);
		psp = GetComponentInParent<PlayerStatePattern> ();
	}

	void Update() 
	{
		if (psp.camOrbit && !resetStop) {
			turnSpeed += Mathf.Pow(Time.deltaTime, 3.0f) * accelRate;
			if (turnSpeed > maxTurnSpeed) turnSpeed = maxTurnSpeed;				// clamp
		}

		if (!psp.camOrbit && !resetStop) {
			turnSpeed -= Mathf.Pow(Time.deltaTime, 3.0f) * accelRate;
			if (turnSpeed <= 0f) {
				turnSpeed = 0f;								// clamp
				resetStop = true;
			}
		}

		if (!psp.camOrbit && resetStop) {
			offset = Vector3.zero;
			resetStop = false;
		}

	}

	void LateUpdate()
	{
		if (psp.camOrbit) {
			offset = Quaternion.AngleAxis (turnSpeed, Vector3.up) * offset;
			transform.position = core.position + offset; 
			transform.LookAt (player.position, Vector3.up);
		}

		if (!psp.camOrbit && !resetStop) {

			// smoothdamp position back to zero
			transform.position = Vector3.SmoothDamp (transform.position, core.position, ref velocity, 0.01f);				// perform smoothdamp on camera position
			// rotate camera back overhead player
			transform.rotation = Quaternion.Lerp(transform.rotation, core.rotation, resetRate);

		}
	}
}