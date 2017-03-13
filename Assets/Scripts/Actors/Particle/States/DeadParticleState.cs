using UnityEngine;
using System.Collections;

public class DeadParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;								// reference to pattern/monobehaviour class

	//private float stunTimer = 0f;												// timer for post-hit invulnerability

	public DeadParticleState (ParticleStatePattern statePatternParticle)		// constructor
	{
		psp = statePatternParticle;										// attach state pattern to this state 
	}

	public void UpdateState()
	{
		Evol ();
		//Sense ();
	}

	public void OnTriggerEnter(Collider other)
	{
		// evol changes/collisions go here
			// replace PreventPlayerCollision with Stun()

/*		bool hasCollided = true;																								// has collided trigger
		bool rolling = true;																									// roll die trigger

		if (other.gameObject.CompareTag ("Player") && hasCollided) {															// colide with player
			psp.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			hasCollided = false;																									// reset has collided trigger	
			Debug.Log ("particle contact player");
		} 
		else if (other.gameObject.CompareTag ("Photon")) {																		// collide with photon
			psp.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			psp.AddEvol(0.5f);																									// add 0.5 evol
			hasCollided = false;																									// reset collision trigger
		} 
		else if (other.gameObject.CompareTag ("Electron")) {																	// collide with electron
			psp.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2")) {																	// collide with electron2
			psp.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Shell")) {																		// collide with shell
			psp.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Shell2")) {																		// collide with shell
			psp.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			psp.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Atom")) {																		// collide with atom
			if ((psp.evol == other.gameObject.GetComponentInParent<ParticleStatePattern> ().evol) && rolling && hasCollided) {			// if = other electron
				psp.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				psp.RollDie(other.gameObject.GetComponent<ParticleStatePattern> (), 1.0f, 2.0f);									// roll die (win 1, lose -2)
			}
			else if ((psp.evol > other.gameObject.GetComponentInParent<ParticleStatePattern> ().evol) && hasCollided) {				// if > other electron 
				psp.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				psp.AddEvol(1.0f);																									// add 1 evol
				hasCollided = false;																									// reset has collided trigger
			}
			else if ((psp.evol < other.gameObject.InParentGetComponent<ParticleStatePattern> ().evol) && hasCollided) {				// if < other electron, or photon
				psp.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
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
		else if (psp.evol >= 8f)
			ToSixth (true, 0);
	}

/*	private void Sense()
	{
		// search for photons to attract to
		RaycastHit hit;
		if (Physics.Raycast(psp.transform.position, psp.transform.forward, out hit, psp.attractionRange) && hit.collider.CompareTag("Photon")) {
			psp.attractionTarget = hit.transform;

		}
	} */

}
