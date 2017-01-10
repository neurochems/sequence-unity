using UnityEngine;
using System.Collections;

public class Electron2PlayerState : IParticleState 
{

	private readonly PlayerStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public Electron2PlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		particle = playerStatePattern;													// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		Evol ();																		// check evol

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= particle.stunDuration) canCollide = true;					// set collision ability
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Photon") && canCollide) {							// collide with photon
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(0.5f);																// add 0.5 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(1.0f);			// subtract 1 evol from other
			canCollide = false;																	// reset has collided trigger
		} 
		else if (other.gameObject.CompareTag ("Electron") && canCollide) {					// collide with electron
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(1.0f);																// add 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(1.0f);			// subtract 1 evol from other
			canCollide = false;																	// reset has collided trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2") && canCollide) {					// collide with electron2
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(1.0f);																// add 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(1.0f);			// subtract 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Shell") && canCollide) {						// collide with shell
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.SubtractEvol(2.0f);														// subtract 2 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(1.0f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Shell2") && canCollide) {					// collide with shell
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.SubtractEvol(2.0f);														// subtract 2 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(1.0f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Atom") && canCollide) {						// collide with atom
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.SubtractEvol(3.0f);														// subtract 3 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(1.0f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
	}

	public void Death()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToDead(particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.deadState;										// set to new state
	}

	public void ToPhoton()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToPhoton(particle.self);										// trigger transition effects
		ParticleStateEvents.toPhoton += particle.TransitionToPhoton;					// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.photonState;									// set to new state
	}

	public void ToElectron()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToElectron(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.SpawnPhoton(1);														// spawn 1 photon
		particle.currentState = particle.electronState;									// set to new state
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
	}

	public void ToShell2()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToShell2(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.currentState = particle.shell2State;									// set to new state
	}

	public void ToAtom()
	{
		particle.previousState = 2;														// set previous state index
		particle.TransitionToShell2(particle.self);										// trigger transition effects
		ParticleStateEvents.toAtom += particle.TransitionToAtom;						// flag transition in delegate
		particle.currentState = particle.atomState;										// set to new state
	}

	/*	public void ToAtom2()
	{
		particle.currentState = particle.atom2State;											// set to new state
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
}
