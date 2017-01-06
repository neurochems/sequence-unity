using UnityEngine;
using System.Collections;

public class DeadParticleState : IParticleState 
{

	private readonly ParticleStatePattern particle;								// reference to pattern/monobehaviour class

	private float stunTimer = 0f;												// timer for post-hit invulnerability

	public DeadParticleState (ParticleStatePattern statePatternParticle)		// constructor
	{
		particle = statePatternParticle;										// attach state pattern to this state 
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
			Stun();																													// disable collider
			hasCollided = false;																									// reset has collided trigger	
			Debug.Log ("particle contact player");
		} 
		else if (other.gameObject.CompareTag ("Photon")) {																		// collide with photon
			Stun();																													// disable collider	
			particle.AddEvol(0.5f);																									// add 0.5 evol
			hasCollided = false;																									// reset collision trigger
		} 
		else if (other.gameObject.CompareTag ("Electron")) {																	// collide with electron
			Stun();																													// disable collider	
			particle.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2")) {																	// collide with electron2
			Stun();																													// disable collider	
			particle.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Shell")) {																		// collide with shell
			Stun();																													// disable collider	
			particle.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Shell2")) {																		// collide with shell
			Stun();																													// disable collider	
			particle.AddEvol(1.0f);																									// add 1 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Atom")) {																		// collide with atom
			if ((particle.evol == other.gameObject.GetComponent<ParticleStatePattern> ().evol) && rolling && hasCollided) {			// if = other electron
				Stun();																													// disable collider
				particle.RollDie(other.gameObject.GetComponent<ParticleStatePattern> (), 1.0f, 2.0f);									// roll die (win 1, lose -2)
			}
			else if ((particle.evol > other.gameObject.GetComponent<ParticleStatePattern> ().evol) && hasCollided) {				// if > other electron 
				Stun();																													// disable collider
				particle.AddEvol(1.0f);																									// add 1 evol
				hasCollided = false;																									// reset has collided trigger
			}
			else if ((particle.evol < other.gameObject.GetComponent<ParticleStatePattern> ().evol) && hasCollided) {				// if < other electron, or photon
				Stun();																													// disable collider
				particle.SubtractEvol(2.0f);																							// subtract 2 evol
				hasCollided = false;																									// reset has collided trigger
			}
		}*/
	}

	public void Death()
	{
		//Debug.Log ("Can't transition to same state");
	}

	public void ToPhoton()
	{
		particle.currentState = particle.photonState;							// set to new state
		stunTimer = 0f;															// reset stun timer
	}

	public void ToElectron()
	{
		particle.currentState = particle.electronState;							// set to new state
		stunTimer = 0f;															// reset stun timer
	}

	public void ToElectron2()
	{
		particle.currentState = particle.electron2State;						// set to new state
		stunTimer = 0f;															// reset stun timer
	}

	public void ToShell()
	{
		particle.currentState = particle.shellState;							// set to new state
		stunTimer = 0f;															// reset stun timer
	}

	public void ToShell2()
	{
		particle.currentState = particle.shell2State;							// set to new state
		stunTimer = 0f;															// reset stun timer
	}

	public void ToAtom()
	{
		particle.currentState = particle.atomState;								// set to new state
		stunTimer = 0f;															// reset stun timer
	}

/*	public void ToAtom2()
	{
		particle.currentState = particle.atom2State;							// set to new state
		stunTimer = 0f;															// reset stun timer
	}*/

	public void Evol()
	{
		if (particle.evol <= 0f)
			Death ();
		else if (particle.evol < 1f)
			ToPhoton ();
		else if (particle.evol == 1f)
			ToElectron ();
		else if (particle.evol == 1.5f)
			ToElectron2 ();
		else if (particle.evol >= 2f && particle.evol < 3f)
			ToShell ();
		else if (particle.evol >= 3f && particle.evol < 5f)
			ToShell2 ();
		//		else if (particle.evol >= 8f)
		//			ToAtom2 ();
	}

/*	private void Sense()
	{
		// search for photons to attract to
		RaycastHit hit;
		if (Physics.Raycast(particle.transform.position, particle.transform.forward, out hit, particle.attractionRange) && hit.collider.CompareTag("Photon")) {
			particle.attractionTarget = hit.transform;

		}
	} */

	private void Stun()															/* trigger post-hit invulnerability */
	{
		particle.Stun (true);													// disable collider
		stunTimer += Time.deltaTime;											// start timer
		if (stunTimer >= particle.stunDuration) {								// if timer >= duration
			particle.Stun (false);													// enable collider
			stunTimer = 0f;															// reset timer
		}
	}

}
