using UnityEngine;
using System.Collections;

public class Electron2ParticleState : IParticleState 
{

	private readonly ParticleStatePattern particle;								// reference to pattern/monobehaviour class

	private float stunTimer = 0f;												// timer for post-hit invulnerability

	public Electron2ParticleState (ParticleStatePattern statePatternParticle)		// constructor
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

		bool hasCollided = true;																								// has collided trigger
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
			if ((particle.evol == other.gameObject.GetComponent<ParticleStatePattern> ().evol) && rolling && hasCollided) {			// if = other electron
				Stun();																													// disable collider
				particle.RollDie(other.gameObject.GetComponent<ParticleStatePattern> (), 1.0f, 1.0f);									// roll die (win 1, lose 1)
			}
			else if ((particle.evol > other.gameObject.GetComponent<ParticleStatePattern> ().evol) && hasCollided) {				// if > other electron 
				Stun();																													// disable collider
				particle.AddEvol(1.0f);																									// add 1 evol
				hasCollided = false;																									// reset has collided trigger
			}
			else if ((particle.evol < other.gameObject.GetComponent<ParticleStatePattern> ().evol) && hasCollided) {				// if < other electron, or photon
				Stun();																													// disable collider
				particle.SubtractEvol(1.0f);																							// subtract 1 evol
				hasCollided = false;																									// reset has collided trigger
			}
		}
		else if (other.gameObject.CompareTag ("Shell")) {																		// collide with shell
			Stun();																													// disable collider	
			particle.SubtractEvol(2.0f);																							// subtract 2 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Shell2")) {																		// collide with shell
			Stun();																													// disable collider	
			particle.SubtractEvol(2.0f);																							// subtract 2 evol
			hasCollided = false;																									// reset collision trigger
		}
		else if (other.gameObject.CompareTag ("Atom")) {																						// collide with atom
			Stun();																													// disable collider	
			particle.SubtractEvol(3.0f);																							// subtract 3 evol
			hasCollided = false;																									// reset collision trigger
		}
	}

	public void Death()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToDead(particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.deadState;										// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToPhoton()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToPhoton(particle.self);										// trigger transition effects
		ParticleStateEvents.toPhoton += particle.TransitionToPhoton;					// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.photonState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToElectron()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToElectron(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.SpawnPhoton(1);														// spawn 1 photon
		particle.currentState = particle.electronState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToElectron2()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToShell()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToShell(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.currentState = particle.shellState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToShell2()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToShell2(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.currentState = particle.shell2State;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToAtom()
	{
		particle.previousState = 2;														// set previous state index
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

	public void Evol()
	{
		if (particle.evol <= 0f)
			Death ();
		else if (particle.evol < 1f)
			ToPhoton ();
		else if (particle.evol == 1f)
			ToElectron ();
		else if (particle.evol >= 2f)
			ToShell ();
	}

/*	private void Sense()
	{
		// search for photons to attract to
		RaycastHit hit;
		if (Physics.Raycast(particle.transform.position, particle.transform.forward, out hit, particle.attractionRange) && hit.collider.CompareTag("Photon")) {
			particle.attractionTarget = hit.transform;
		}
	}*/

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
