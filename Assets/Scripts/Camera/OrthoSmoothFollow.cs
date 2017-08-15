using UnityEngine;
using System.Collections;

public class OrthoSmoothFollow : MonoBehaviour {

	public Transform player;																							// player
	public float smoothTime = 0.3f;																						// smooth time
	public float xOffset = 1.0f, yOffset = 1.0f, zOffset = 1.0f;														// damping circle size
	public float returnSpeed = 3.0f;																					// return to centre at stop speed

	private PlayerStatePattern psp;																						// player state pattern ref

	private Vector3 velocity = Vector3.zero;																			// velocity of camera

	private Rigidbody rbPlayer;																							// player rigidbody 

	void Start() 
	{
		rbPlayer = GetComponentInParent<Rigidbody> ();
		psp = GetComponentInParent<PlayerStatePattern> ();
	}

	void Update () {

		if (!psp.camOrbit) {																							// if not in tenth state

			Vector3 goalPos = player.position;																				// save player position

			goalPos.x += (player.position.x * xOffset);																		// fix x axis
			goalPos.y += (player.position.y * yOffset);																		// fix y axis
			goalPos.z += (player.position.z * zOffset);																		// fix z axis

			if (rbPlayer.velocity.magnitude > 1.0f) 																		// if velocity > 1
				transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);				// perform smoothdamp on camera position
				//transform.position = Vector3.Lerp (transform.position, goalPos, smoothTime);										// perform lerp on camera position

			if (rbPlayer.velocity.magnitude <= 1.0f) 																		// if velocity < 1
				transform.position = Vector3.SmoothDamp 
					(transform.position, player.position, ref velocity, smoothTime * returnSpeed);								// perform smoothdamp on camera position back to player at centre

			if (psp.toLightworld || psp.toDarkworld) {																		// if to lightworld or darkworld
				//Debug.Log("OrthoSmoothFollow - return to player centre on toLightworld = true - CAMERA");			
				transform.position = Vector3.SmoothDamp 
					(transform.position, player.position, ref velocity, 0.75f);													// perform smoothdamp on camera position back to player at centre
			}
		}
	}
}
