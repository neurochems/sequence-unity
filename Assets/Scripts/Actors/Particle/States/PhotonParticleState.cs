using UnityEngine;
using System.Collections;

public class PhotonParticleState : IParticleState 
{

	private readonly ParticleStatePattern particle;										// reference to pattern/monobehaviour class

	private float stunTimer = 0f;														// timer for post-hit invulnerability

	//private bool start = true;															// first frame in this state flag

	// constructor
	public PhotonParticleState (ParticleStatePattern statePatternParticle)				
	{
		particle = statePatternParticle;												// attach state pattern to this state 
	}

	public void UpdateState()
	{
/*		if (start) {
			particle.TransitionToPhoton (particle.self);
			Stun ();
			start = false;
		}*/

		Evol ();
		//Sense ();
	}

	public void OnTriggerEnter(Collider other)
	{
		// evol changes/collisions go here
			// replace PreventPlayerCollision with Stun()

		bool hasCollided = true;																								// has collided trigger
		bool rolling = true;																									// roll die trigger

		if (other.gameObject.CompareTag ("Player") && hasCollided) {															// colide with player
			Stun();																													// disable collider
			hasCollided = false;																									// reset has collided trigger	
			Debug.Log ("particle contact player");
		} 
		else if (other.gameObject.CompareTag ("Photon")) {																		// collide with photon
			if ((particle.evol == other.gameObject.GetComponent<ParticleStatePattern>().evol) && rolling && hasCollided) {			// if = other photon
				Stun();																													// disable collider
				particle.RollDie(other.gameObject.GetComponent<ParticleStatePattern> (), 0.5f, 1.0f);									// roll die (win 0.5, lose 1)
				hasCollided = false;																									// reset has collided trigger
				rolling = false;																										//reset rolling trigger
			} 
			else if ((particle.evol > other.gameObject.GetComponent<ParticleStatePattern>().evol) && hasCollided) {					// if > other photon
				Stun();																													// disable collider
				particle.AddEvol(0.5f);																									// add 0.5 evol
				hasCollided = false;																									// reset has collided trigger
			}
			else if ((particle.evol < other.gameObject.GetComponent<ParticleStatePattern>().evol) && hasCollided) {					// if < other photon
				Stun();																													// disable collider
				particle.SubtractEvol(1.0f);																							// subtract 1 evol
				hasCollided = false;																									// reset has collided trigger
			}
		} 
		else if (other.gameObject.CompareTag ("Electron")) {																	// collide with electron
			Stun();																													// disable collider
			particle.SubtractEvol(1.0f);																							// subtract 1 evol
			hasCollided = false;																									// reset has collided trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2")) {																	// collide with electron2
			Stun();																													// disable collider	
			particle.SubtractEvol(1.0f);																							// subtract 1 evol
			hasCollided = false;																									// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Shell")) {																		// collide with shell
			Stun();																													// disable collider	
			particle.SubtractEvol(1.0f);																							// subtract 1 evol
			hasCollided = false;				
		}
		else if (other.gameObject.CompareTag ("Shell2")) {																		// collide with shell
			Stun();																													// disable collider	
			particle.SubtractEvol(1.0f);																							// subtract 1 evol
			hasCollided = false;				
		}
		else if (other.gameObject.CompareTag ("Atom")) {																						// collide with atom
			Stun();																													// disable collider	
			particle.SubtractEvol(1.0f);																							// subtract 1 evol
			hasCollided = false;				
		}
	}

	public void Death()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToDead(particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.currentState = particle.deadState;										// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToPhoton()
	{
		//Debug.Log ("Can't transition to same state");
	}

	public void ToElectron()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToElectron(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.currentState = particle.electronState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToElectron2()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToElectron2(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.currentState = particle.electron2State;								// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToShell()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToShell(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.currentState = particle.shellState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToShell2()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToShell2(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.currentState = particle.shell2State;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToAtom()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToAtom(particle.self);										// trigger transition effects
		ParticleStateEvents.toAtom += particle.TransitionToAtom;						// flag transition in delegate
		particle.currentState = particle.atomState;										// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

/*	public void ToAtom2()
	{
		particle.currentState = particle.atom2State;							// set to new state
		stunTimer = 0f;															// reset stun timer
	}*/

	public void Evol()									// all states here for init 
	{
		if (particle.evol < 0f)															// if evol < 0
			Death ();																		// change to dead state
		else if (particle.evol == 1f)													// if evol == 1
			ToElectron ();																	// evolve to electron
		else if (particle.evol == 1.5f)													// if evol == 1.5
			ToElectron2 ();																	// evolve to electron2
		else if (particle.evol == 2f)													// if evol == 2
			ToShell ();																		// evolve to shell
		else if (particle.evol == 3f)													// if evol == 3
			ToShell2 ();																	// evolve to shell2
		else if (particle.evol == 5f)													// if evol == 5
			ToAtom ();																		// evolve to atom
	}

/*	private void Sense()
	{
		// search for photons to attract to
		RaycastHit hit;
		if (Physics.Raycast(particle.transform.position, particle.transform.forward, out hit, particle.attractionRange) && hit.collider.CompareTag("Photon")) {
			particle.attractionTarget = hit.transform;

		}
	}*/

	/* trigger post-hit invulnerability */
	private void Stun()															
	{
		particle.Stun (true);															// disable collider
		stunTimer += Time.deltaTime;													// start timer
		if (stunTimer >= particle.stunDuration) {										// if timer >= duration
			particle.Stun (false);															// enable collider
			stunTimer = 0f;																	// reset timer
		}
	}

}
