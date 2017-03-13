using UnityEngine;
using System.Collections;

public class DeadPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;										// reference to pattern/monobehaviour class

	public DeadPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;													// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		Evol ();																		// check evol
	}

	public void OnTriggerEnter(Collider other)
	{
		// evol changes/collisions go here
		// replace PreventPlayerCollision with Stun()

		/*		bool hasCollided = true;																								// has collided trigger
		bool rolling = true;																									// roll die trigger

		if (other.gameObject.CompareTag ("Player") && hasCollided) {															// colide with player
			Stun();																													// disable collider
			hasCollided = false;																									// reset has collided trigger	
			Debug.Log ("particle contact player");
		} 
		else if (other.gameObject.CompareTag ("Photon")) {																		// collide with photon
			Stun();																													// disable collider	
			psp.AddEvol(0.5f);																									// add 0.5 evol
			hasCollided = false;																									// reset collision trigger
		} 
		else if (other.gameObject.CompareTag ("Electron")) {																	// collide with electron
			Stun();																													// disable collider	
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2")) {																	// collide with electron2
			Stun();																													// disable collider	
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Shell")) {																		// collide with shell
			Stun();																													// disable collider	
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Shell2")) {																		// collide with shell
			Stun();																													// disable collider	
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Atom")) {																		// collide with atom
			if ((psp.evol == other.gameObject.GetComponent<PlayerStatePattern> ().evol) && rolling && hasCollided) {			// if = other electron
				Stun();																													// disable collider
				psp.RollDie(other.gameObject.GetComponent<PlayerStatePattern> (), 1.0f, 2.0f);									// roll die (win 1, lose -2)
			}
			else if ((psp.evol > other.gameObject.GetComponent<PlayerStatePattern> ().evol) && hasCollided) {				// if > other electron 
				Stun();																													// disable collider
				psp.AddEvol(1.0f);																									// add 1 evol
				hasCollided = false;																									// reset has collided trigger
			}
			else if ((psp.evol < other.gameObject.GetComponent<PlayerStatePattern> ().evol) && hasCollided) {				// if < other electron, or photon
				Stun();																													// disable collider
				psp.SubtractEvol(2.0f);																							// subtract 2 evol
				hasCollided = false;																									// reset has collided trigger
			}
		}*/
	}

	public void Death(bool toLight)
	{
		//Debug.Log ("Can't transition to same state");
	}

	public void ToZero(bool toLight)
	{
		psp.currentState = psp.zeroState;							// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.currentState = psp.firstState;							// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.currentState = psp.secondState;						// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.currentState = psp.thirdState;							// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.currentState = psp.fourthState;							// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.currentState = psp.fifthState;								// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.currentState = psp.sixthState;							// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void Evol()
	{
		if (psp.evol <= 0f)
			Death (true);
		else if (psp.evol < 1f)
			ToZero (true);
		else if (psp.evol == 1f)
			ToFirst (true);
		else if (psp.evol == 1.5f)
			ToSecond (true);
		else if (psp.evol >= 2f && psp.evol < 3f)
			ToThird (true);
		else if (psp.evol >= 3f && psp.evol < 5f)
			ToFourth (true);
		else if (psp.evol >= 5f && psp.evol < 8f)
			ToFifth (true, 0);
		else if (psp.evol >= 8f)
			ToSixth (true, 0);
	}
}
