using UnityEngine;
using System.Collections;

public class Shell2PlayerState : IParticleState 
{

	private readonly PlayerStatePattern particle;										// reference to pattern/monobehaviour class

	private float stunTimer = 0f;														// timer for post-hit invulnerability

	public Shell2PlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		particle = playerStatePattern;													// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		Evol ();																		// check evol
	}

	public void OnTriggerEnter(Collider other)
	{
		// evol changes/collisions go here
		// replace PreventPlayerCollision with Stun()

		if (other.gameObject.CompareTag ("Photon")) {									// collide with photon
			Stun();																		// disable collider	
			particle.AddEvol(0.5f);														// add 0.5 evol
		} 
		else if (other.gameObject.CompareTag ("Electron")) {							// collide with electron
			Stun();																		// disable collider	
			particle.AddEvol(1.0f);														// add 1 evol
		} 
		else if (other.gameObject.CompareTag ("Electron2")) {							// collide with electron2
			Stun();																		// disable collider	
			particle.AddEvol(1.0f);														// add 1 evol
		}
		else if (other.gameObject.CompareTag ("Shell")) {								// collide with shell
			Stun();																		// disable collider	
			particle.AddEvol(1.0f);														// add 1 evol
		}
		else if (other.gameObject.CompareTag ("Shell2")) {								// collide with shell
			Stun();																		// disable collider
			particle.AddEvol(1.0f);														// add 1 evol
		}
		else if (other.gameObject.CompareTag ("Atom")) {								// collide with atom
			Stun();																		// disable collider	
			particle.SubtractEvol(3.0f);												// subtract 2 evol
		}
	}

	public void Death()
	{
		particle.previousState = 4;														// set previous state index
		particle.TransitionToDead(particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.deadState;										// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToPhoton()
	{
		particle.previousState = 4;														// set previous state index
		particle.TransitionToPhoton(particle.self);										// trigger transition effects
		ParticleStateEvents.toPhoton += particle.TransitionToPhoton;					// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.photonState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToElectron()
	{
		particle.previousState = 4;														// set previous state index
		particle.TransitionToElectron(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.SpawnPhoton(3);														// spawn 3 photons
		particle.currentState = particle.electronState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToElectron2()
	{
		particle.previousState = 4;														// set previous state index
		particle.TransitionToElectron2(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.electron2State;								// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToShell()
	{
		particle.previousState = 4;														// set previous state index
		particle.TransitionToShell(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.SpawnPhoton(1);														// spawn 1 photon
		particle.currentState = particle.shellState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToShell2()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToAtom()
	{
		particle.previousState = 4;														// set previous state index
		particle.TransitionToAtom(particle.self);										// trigger transition effects
		ParticleStateEvents.toAtom += particle.TransitionToAtom;						// flag transition in delegate
		particle.currentState = particle.atomState;										// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	/*	public void ToAtom2()
	{
		particle.currentState = particle.atom2State;											// set to new state
		stunTimer = 0f;																			// reset stun timer
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
		else if (particle.evol >= 5f)
			ToAtom ();
	}

	private void Stun()																	/* trigger post-hit invulnerability */
	{
		particle.Stun (true);															// disable collider
		stunTimer += Time.deltaTime;													// start timer
		if (stunTimer >= particle.stunDuration) {										// if timer >= duration
			particle.Stun (false);															// enable collider
			stunTimer = 0f;																	// reset timer
		}
	}

}
