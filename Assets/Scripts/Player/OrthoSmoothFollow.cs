using UnityEngine;
using System.Collections;

public class OrthoSmoothFollow : MonoBehaviour {

	public Transform player;																				// player
	public float smoothTime = 0.3f;																			// smooth time
	public float xOffset = 1.0f, yOffset = 1.0f, zOffset = 1.0f;											// damping circle size

	private Vector3 velocity = Vector3.zero;																// velocity of camera

	void Update () {

		Vector3 goalPos = player.position;																	// save player position

		goalPos.x += (player.position.x * xOffset);																		// fix y axis
		goalPos.y += (player.position.y * yOffset);																		// fix y axis
		goalPos.z += (player.position.z * zOffset);																		// fix y axis

		transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);	// perform smoothdamp on camera position
		//transform.position = Vector3.Lerp (transform.position, goalPos, smoothTime);						// perform lerp on camera position



	}
}
