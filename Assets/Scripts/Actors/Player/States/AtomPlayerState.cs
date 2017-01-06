using UnityEngine;
using System.Collections;

public class AtomPlayerState : IParticleState 
{

	private readonly PlayerStatePattern particle;										// reference to pattern/monobehaviour class

	private float stunTimer = 0f;														// timer for post-hit invulnerability

	public AtomPlayerState (PlayerStatePattern playerStatePattern)						// constructor
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
			particle.AddEvol(1.0f);														// add 1 evol
		}
	}

	public void Death()
	{
		particle.previousState = 5;														// set previous state index
		particle.TransitionToDead(particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.SpawnElectron(2);														// spawn 2 electrons
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.deadState;										// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToPhoton()
	{
		particle.previousState = 5;														// set previous state index
		particle.TransitionToPhoton(particle.self);										// trigger transition effects
		ParticleStateEvents.toPhoton += particle.TransitionToPhoton;					// flag transition in delegate
		particle.SpawnElectron(2);														// spawn 2 electrons
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.photonState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToElectron()
	{
		particle.previousState = 5;														// set previous state index
		particle.TransitionToElectron(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(3);														// spawn 2 photons
		particle.currentState = particle.electronState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToElectron2()
	{
		particle.previousState = 5;														// set previous state index
		particle.TransitionToElectron2(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(2);														// spawn 1 photon
		particle.currentState = particle.electron2State;								// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToShell()
	{
		particle.previousState = 5;														// set previous state index
		particle.TransitionToShell(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.currentState = particle.shellState;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToShell2()
	{
		particle.previousState = 5;														// set previous state index
		particle.TransitionToShell2(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.shell2State;									// set to new state
		stunTimer = 0f;																	// reset stun timer
	}

	public void ToAtom()
	{
		Debug.Log ("Can't transition to same state");
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
		else if (particle.evol >= 3f && particle.evol < 5f)
			ToShell2 ();
		//		else if (particle.evol >= 8f)
		//			ToAtom2 ();
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
