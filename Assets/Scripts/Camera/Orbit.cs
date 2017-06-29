using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Transform player;
	private PlayerStatePattern psp;

	public float turnSpeed = 4.0f;

	private Vector3 offset;

	void Start () {
		offset = new Vector3(player.position.x, player.position.y + 8.0f, player.position.z + 7.0f);
		psp = GetComponentInParent<PlayerStatePattern> ();
	}

	void LateUpdate()
	{
		if (psp.camOrbit) {
			offset = Quaternion.AngleAxis (turnSpeed, Vector3.up) * offset;
			transform.position = player.position + offset; 
			transform.LookAt (player.position);
		}
	}
}