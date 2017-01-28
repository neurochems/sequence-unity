using UnityEngine;
using System.Collections;

public class PhotonParticleState : IParticleState 
{

	private readonly ParticleStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = false;													// can collide flag (false to stun on new spawn)
	private float collisionTimer;														// reset can collide timer	

	// constructor
	public PhotonParticleState (ParticleStatePattern statePatternParticle)				
	{
		particle = statePatternParticle;												// attach state pattern to this state 
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
		bool rolling = true;																							// roll die trigger

		if (canCollide) {																								// if collision allowed
			if (other.gameObject.CompareTag ("Player")) {																	// colide with player
				particle.GetComponent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
				canCollide = false;																								// reset can collide trigger	
				Debug.Log ("particle contact player");
			} 
			else if (other.gameObject.CompareTag ("Photon")) {																// collide with photon
				if (rolling && (particle.evol == other.gameObject.GetComponent<ParticleStatePattern> ().evol)) {				// if = other photon
					particle.RollDie (other.gameObject.GetComponent<ParticleStatePattern> (), 0.5f, 1.0f);							// roll die (win 0.5, lose 1)
					particle.stunned = true;																						// stun new particle for 3 sec
					canCollide = false;																								// reset can collide trigger
					rolling = false;																								// reset rolling trigger
				} 
				else if ((particle.evol > other.gameObject.GetComponent<ParticleStatePattern> ().evol)) {						// if > other photon
					particle.AddEvol (0.5f);																						// add 0.5 evol
					particle.stunned = true;																						// stun new particle for 3 sec
					canCollide = false;																								// reset can collide trigger
				} 
				else if ((particle.evol < other.gameObject.GetComponent<ParticleStatePattern> ().evol)) {						// if < other photon
					particle.SubtractEvol (1.0f);																					// subtract 1 evol
					particle.stunned = true;																						// stun new particle for 3 sec
					canCollide = false;																								// reset can collide trigger
				}
			} 
			else if (other.gameObject.CompareTag ("Electron")) {															// collide with electron
				particle.GetComponent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
				particle.SubtractEvol (1.0f);																					// subtract 1 evol
				canCollide = false;																								// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Electron2")) {															// collide with electron2
				particle.GetComponent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
				particle.SubtractEvol (1.0f);																					// subtract 1 evol
				canCollide = false;																								// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Shell")) {																// collide with shell
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;											// stun new particle for 3 sec
				particle.SubtractEvol (1.0f);																					// subtract 1 evol
				canCollide = false;																								// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Shell2")) {																// collide with shell2
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;											// stun new particle for 3 sec
				particle.SubtractEvol (1.0f);																					// subtract 1 evol
				canCollide = false;																								// reset can collide trigger
			} 
			else if (other.gameObject.CompareTag ("Atom")) {																// collide with atom
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;											// stun new particle for 3 sec
				particle.SubtractEvol (1.0f);																					// subtract 1 evol
				canCollide = false;																								// reset can collide trigger
			}
			else if (other.gameObject.CompareTag ("Atom2")) {																// collide with atom2
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;											// stun new particle for 3 sec
				particle.SubtractEvol (1.0f);																					// subtract 1 evol
				canCollide = false;																								// reset can collide trigger
			}
		}
	}

	public void Death()
	{
		particle.TransitionToDead(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.currentState = particle.deadState;										// set to new state
	}

	public void ToPhoton()
	{
		//Debug.Log ("Can't transition to same state");
	}

	public void ToElectron()
	{
		particle.TransitionToElectron(0, particle.self);								// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.currentState = particle.electronState;									// set to new state
	}

	public void ToElectron2()
	{
		particle.TransitionToElectron2(0, particle.self);								// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.currentState = particle.electron2State;								// set to new state
	}

	public void ToShell()
	{
		particle.TransitionToShell(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.currentState = particle.shellState;									// set to new state
	}

	public void ToShell2()
	{
		particle.TransitionToShell2(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.currentState = particle.shell2State;									// set to new state
	}

	public void ToAtom()
	{
		particle.TransitionToAtom(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toAtom += particle.TransitionToAtom;						// flag transition in delegate
		particle.currentState = particle.atomState;										// set to new state
	}

	public void ToAtom2()
	{
		particle.TransitionToAtom2(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toAtom2 += particle.TransitionToAtom2;						// flag transition in delegate
		particle.currentState = particle.atom2State;									// set to new state
	}

	public void ToElement()
	{
		Debug.Log ("Can't transition to same state");
	}

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
		else if (particle.evol == 8f)													// if evol == 8
			ToAtom2 ();																		// evolve to atom2
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
