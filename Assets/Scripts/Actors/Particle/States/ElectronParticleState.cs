using UnityEngine;
using System.Collections;

public class ElectronParticleState : IParticleState 
{

	private readonly ParticleStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset can collide timer

	public ElectronParticleState (ParticleStatePattern statePatternParticle)			// constructor
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
		bool rolling = true;																									// roll die trigger

		if (other.gameObject.CompareTag ("Player") && canCollide) {																// colide with player
			particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			canCollide = false;																										// reset can collide trigger	
			Debug.Log ("particle contact player");
		} 
		else if (other.gameObject.CompareTag ("Photon") && canCollide) {														// collide with photon
			particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			particle.AddEvol(0.5f);																									// add 0.5 evol
			canCollide = false;																										// reset can collide trigger
			
		} 
		else if (other.gameObject.CompareTag ("Electron") && canCollide) {														// collide with electron
			if ((particle.evol == other.gameObject.GetComponent<ParticleStatePattern> ().evol) && rolling && canCollide) {			// if = other electron
				particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				particle.RollDie(other.gameObject.GetComponent<ParticleStatePattern> (), 1.0f, 1.0f);									// roll die (win 1, lose 1)
				canCollide = false;																										// reset can collide trigger
			}
			else if ((particle.evol > other.gameObject.GetComponent<ParticleStatePattern> ().evol) && canCollide) {					// if > other electron 
				particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				particle.AddEvol(1.0f);																									// add 1 evol
				canCollide = false;																										// reset can collide trigger
			}
			else if ((particle.evol < other.gameObject.GetComponent<ParticleStatePattern> ().evol) && canCollide) {					// if < other electron, or photon
				particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				particle.SubtractEvol(1.0f);																							// subtract 1 evol
				canCollide = false;																										// reset can collide trigger
			}
		} 
		else if (other.gameObject.CompareTag ("Electron2") && canCollide) {														// collide with electron2
			particle.GetComponent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			particle.SubtractEvol(1.0f);																							// subtract 1 evol
			canCollide = false;																										// reset can collide trigger
		}
		else if (other.gameObject.CompareTag ("Shell") && canCollide) {															// collide with shell
			particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
			particle.SubtractEvol(2.0f);																							// subtract 1 evol
			canCollide = false;																										// reset can collide trigger
		}
		else if (other.gameObject.CompareTag ("Shell2") && canCollide) {														// collide with shell2
			particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
			particle.SubtractEvol(2.0f);																							// subtract 1 evol
			canCollide = false;																										// reset can collide trigger
		}
		else if (other.gameObject.CompareTag ("Atom") && canCollide) {															// collide with atom
			particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;													// stun new particle for 3 sec
			particle.SubtractEvol(3.0f);																							// subtract 1 evol
			canCollide = false;																										// reset can collide trigger
		}
	}

	public void Death()
	{
		particle.previousState = 1;														// set previous state index
		particle.TransitionToDead(particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.SpawnPhoton(1);														// spawn 2 photons
		particle.currentState = particle.deadState;										// set to new state
	}

	public void ToPhoton()
	{
		particle.previousState = 1;														// set previous state index
		particle.TransitionToPhoton(particle.self);										// trigger transition effects
		ParticleStateEvents.toPhoton += particle.TransitionToPhoton;					// flag transition in delegate
		particle.SpawnPhoton(1);														// spawn 1 photon
		particle.currentState = particle.photonState;									// set to new state
	}

	public void ToElectron()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToElectron2()
	{
		particle.previousState = 1;														// set previous state index
		particle.TransitionToElectron2(particle.self);										// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.currentState = particle.electron2State;								// set to new state
	}

	public void ToShell()
	{
		particle.previousState = 1;														// set previous state index
		particle.TransitionToShell(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.currentState = particle.shellState;									// set to new state
	}

	public void ToShell2()
	{
		particle.previousState = 1;														// set previous state index
		particle.TransitionToShell2(particle.self);										// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.currentState = particle.shell2State;									// set to new state
	}

	public void ToAtom()
	{
		particle.previousState = 1;														// set previous state index
		particle.TransitionToAtom(particle.self);										// trigger transition effects
		ParticleStateEvents.toAtom += particle.TransitionToAtom;						// flag transition in delegate
		particle.currentState = particle.atomState;										// set to new state
	}

/*	public void ToAtom2()
	{
		particle.currentState = particle.atom2State;							// set to new state
	}*/

	public void Evol()
	{
		if (particle.evol <= 0f)
			Death ();
		else if (particle.evol < 1f)
			ToPhoton ();
		else if (particle.evol == 1.5f)
			ToElectron2 ();
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
}
