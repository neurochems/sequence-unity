using UnityEngine;
using System.Collections;

public class PhotonPlayerState : IParticleState
{

	private readonly PlayerStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	// constructor
	public PhotonPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		particle = playerStatePattern;													// attach state pattern to this state 
	}

	public void UpdateState()															// called every frame in PlayerStatePattern
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
			particle.SubtractEvol(1.0f);														// subtract 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(0.5f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2") && canCollide) {					// collide with electron2
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.SubtractEvol(1.0f);														// subtract 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(0.5f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Shell") && canCollide) {						// collide with shell
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.SubtractEvol(1.0f);														// subtract 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(0.5f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Shell2") && canCollide) {					// collide with shell
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.SubtractEvol(1.0f);														// subtract 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(0.5f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Atom") && canCollide) {						// collide with atom
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.SubtractEvol(1.0f);														// subtract 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().AddEvol(0.5f);				// add 1 evol from other
			canCollide = false;																	// reset has collided trigger
		}
	}

	public void Death()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToDead(particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.currentState = particle.deadState;										// set to new state
	}

	public void ToPhoton()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToElectron()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToElectron(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.currentState = particle.electronState;									// set to new state
	}

	public void ToElectron2()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToElectron2(particle.self);									// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.currentState = particle.electron2State;								// set to new state
	}

	public void ToShell()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToShell(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.currentState = particle.shellState;									// set to new state
	}

	public void ToShell2()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToShell2(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.currentState = particle.shell2State;									// set to new state
	}

	public void ToAtom()
	{
		particle.previousState = 0;														// set previous state index
		particle.TransitionToAtom(particle.self);										// trigger transition effects
		ParticleStateEvents.toAtom += particle.TransitionToAtom;						// flag transition in delegate
		particle.currentState = particle.atomState;										// set to new state
	}

	/*	public void ToAtom2()
	{
		particle.currentState = particle.atom2State;											// set to new state
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
}
