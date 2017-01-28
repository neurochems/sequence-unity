﻿using UnityEngine;
using System.Collections;

public class ShellParticleState : IParticleState 
{

	private readonly ParticleStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset can collide timer

	public ShellParticleState (ParticleStatePattern particleStatePattern)				// constructor
	{
		particle = particleStatePattern;												// attach state pattern to this state 
	}

	public void UpdateState()
	{
		Evol ();
		//Sense ();

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;										// start timer
		if (collisionTimer >= particle.stunDuration) canCollide = true;							// set can collide ability
	}

	public void OnTriggerEnter(Collider other)
	{
		bool rolling = true;																									// roll die trigger

		if (canCollide) {																										// if collision allowed
			if (other.gameObject.CompareTag ("Player")) {																			// colide with player
				particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				canCollide = false;																										// reset can collide trigger	
				Debug.Log ("particle contact player");
			} 
			else if (other.gameObject.CompareTag ("Photon")) {																		// collide with photon
				particle.AddEvol (0.5f);																								// add 0.5 evol
				particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				canCollide = false;																										// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Electron")) {																	// collide with electron
				particle.AddEvol (1.0f);																								// add 1 evol
				particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				canCollide = false;																										// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Electron2")) {																	// collide with electron2
				particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				particle.AddEvol (1.0f);																								// add 1 evol
				canCollide = false;																										// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Shell")) {																		// collide with shell
				if (rolling && (particle.evol == other.gameObject.GetComponentInParent<ParticleStatePattern> ().evol)) {				// if = other shell
					particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
					particle.RollDie (other.gameObject.GetComponentInParent<ParticleStatePattern> (), 1.0f, 1.0f);							// roll die (win 1, lose -2)
					canCollide = false;																										// reset can collide trigger
				} 
				else if ((particle.evol > other.gameObject.GetComponent<ParticleStatePattern> ().evol)) {								// if > other electron 
					particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
					particle.AddEvol (1.0f);																								// add 1 evol
					canCollide = false;																										// reset can collide trigger
				} 
				else if ((particle.evol < other.gameObject.GetComponent<ParticleStatePattern> ().evol)) {								// if < other electron, or photon
					particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
					particle.SubtractEvol (2.0f);																							// subtract 2 evol
					canCollide = false;																										// reset can collide trigger
				}
			} 
			else if (other.gameObject.CompareTag ("Shell2")) {																		// collide with shell2
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
				particle.SubtractEvol (2.0f);																							// subtract 2 evol
				canCollide = false;																										// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Atom")) {																		// collide with atom
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
				particle.SubtractEvol (3.0f);																							// subtract 3 evol
				canCollide = false;																										// reset can collide trigger
			}
			else if (other.gameObject.CompareTag ("Atom2")) {																		// collide with atom2
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
				particle.SubtractEvol (3.0f);																							// subtract 3 evol
				canCollide = false;																										// reset can collide trigger
			}
		}
	}

	public void Death()
	{
		particle.TransitionToDead(3, particle.self);									// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(1);														// spawn 1 photon
		particle.currentState = particle.deadState;										// set to new state
	}

	public void ToPhoton()
	{
		particle.TransitionToPhoton(3, particle.self);									// trigger transition effects
		ParticleStateEvents.toPhoton += particle.TransitionToPhoton;					// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(1);														// spawn 1 photon
		particle.currentState = particle.photonState;									// set to new state
	}

	public void ToElectron()
	{
		particle.TransitionToElectron(3, particle.self);								// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.SpawnPhoton(3);														// spawn 3 photons
		particle.currentState = particle.electronState;									// set to new state
	}

	public void ToElectron2()
	{
		particle.TransitionToElectron2(3, particle.self);								// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.electron2State;								// set to new state
	}

	public void ToShell()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToShell2()
	{
		particle.TransitionToShell2(3, particle.self);									// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.currentState = particle.shell2State;									// set to new state
	}

	public void ToAtom()
	{
		particle.TransitionToAtom(3, particle.self);									// trigger transition effects
		ParticleStateEvents.toAtom += particle.TransitionToAtom;						// flag transition in delegate
		particle.currentState = particle.atomState;										// set to new state
	}

	public void ToAtom2()
	{
		particle.TransitionToAtom2(3, particle.self);									// trigger transition effects
		ParticleStateEvents.toAtom2 += particle.TransitionToAtom2;						// flag transition in delegate
		particle.currentState = particle.atom2State;									// set to new state
	}

	public void ToElement()
	{
		Debug.Log ("Can't transition to same state");
	}

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
		else if (particle.evol >= 3f)
			ToShell2 ();
	}

/*	private void Sense()
	{
		// search for photons to attract to
		RaycastHit hit;
		if (Physics.Raycast(particle.transform.position, particle.transform.forward, out hit, particle.attractionRange) && hit.collider.CompareTag("Photon")) {
			particle.attractionTarget = hit.transform;
		}
	}*/
}
