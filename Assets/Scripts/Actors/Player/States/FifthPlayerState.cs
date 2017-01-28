using UnityEngine;
using System.Collections;

public class FifthPlayerState : IParticleState 
{

	private readonly PlayerStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public FifthPlayerState (PlayerStatePattern playerStatePattern)						// constructor
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
		if (canCollide) {																		// if collision allowed
			if (other.gameObject.CompareTag ("Photon")) {											// collide with photon
				particle.GetComponent<PlayerStatePattern> ().stunned = true;							// stun for duration
				particle.AddEvol (0.5f);																// add 0.5 evol
				other.gameObject.GetComponent<ParticleStatePattern> ().SubtractEvol (3.0f);				// subtract 3 evol from other
				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Electron")) {									// collide with electron
				particle.GetComponent<PlayerStatePattern> ().stunned = true;							// stun for duration
				particle.AddEvol (1.0f);																// add 1 evol
				other.gameObject.GetComponent<ParticleStatePattern> ().SubtractEvol (3.0f);				// subtract 3 evol from other
				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Electron2")) {									// collide with electron2
				particle.GetComponent<PlayerStatePattern> ().stunned = true;							// stun for duration
				particle.AddEvol (1.0f);																// add 1 evol
				other.gameObject.GetComponent<ParticleStatePattern> ().SubtractEvol (3.0f);				// subtract 3 evol from other
				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Shell")) {										// collide with shell
				particle.GetComponent<PlayerStatePattern> ().stunned = true;							// stun for duration
				particle.AddEvol (1.0f);																// add 1 evol
				other.gameObject.GetComponent<ParticleStatePattern> ().SubtractEvol (3.0f);				// subtract 3 evol from other
				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Shell2")) {										// collide with shell
				particle.GetComponent<PlayerStatePattern> ().stunned = true;							// stun for duration
				particle.AddEvol (1.0f);																// add 1 evol
				other.gameObject.GetComponent<ParticleStatePattern> ().SubtractEvol (3.0f);				// subtract 3 evol from other
				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Atom")) {										// collide with atom
				particle.GetComponent<PlayerStatePattern> ().stunned = true;							// stun for duration
				particle.AddEvol (1.0f);																// add 1 evol
				other.gameObject.GetComponent<ParticleStatePattern> ().SubtractEvol (3.0f);				// subtract 3 evol from other
				canCollide = false;																		// reset has collided trigger
			}
			else if (other.gameObject.CompareTag ("Atom2")) {										// collide with atom2
				particle.GetComponent<PlayerStatePattern> ().stunned = true;							// stun for duration
				particle.SubtractEvol (3.0f);															// subtract 3 evol
				other.gameObject.GetComponent<ParticleStatePattern> ().AddEvol (1.0f);					// add 1 evol to other
				canCollide = false;																		// reset has collided trigger
			}
		}
	}

	public void Death()
	{
		particle.TransitionToDead(5, particle.self);									// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.SpawnElectron(2);														// spawn 2 electrons
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.deadState;										// set to new state
	}

	public void ToPhoton()
	{
		particle.TransitionToPhoton(5, particle.self);									// trigger transition effects
		ParticleStateEvents.toPhoton += particle.TransitionToPhoton;					// flag transition in delegate
		particle.SpawnElectron(2);														// spawn 2 electrons
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.photonState;									// set to new state
	}

	public void ToElectron()
	{
		particle.TransitionToElectron(5, particle.self);								// trigger transition effects
		ParticleStateEvents.toElectron += particle.TransitionToElectron;				// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(3);														// spawn 2 photons
		particle.currentState = particle.electronState;									// set to new state
	}

	public void ToElectron2()
	{
		particle.TransitionToElectron2(5, particle.self);								// trigger transition effects
		ParticleStateEvents.toElectron2 += particle.TransitionToElectron2;				// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.SpawnPhoton(2);														// spawn 2 photon
		particle.currentState = particle.electron2State;								// set to new state
	}

	public void ToShell()
	{
		particle.TransitionToShell(5, particle.self);									// trigger transition effects
		ParticleStateEvents.toShell += particle.TransitionToShell;						// flag transition in delegate
		particle.SpawnElectron(1);														// spawn 1 electron
		particle.currentState = particle.shellState;									// set to new state
	}

	public void ToShell2()
	{
		particle.TransitionToShell2(5, particle.self);									// trigger transition effects
		ParticleStateEvents.toShell2 += particle.TransitionToShell2;					// flag transition in delegate
		particle.SpawnPhoton(2);														// spawn 2 photons
		particle.currentState = particle.shell2State;									// set to new state
	}

	public void ToAtom()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToAtom2()
	{
		particle.TransitionToAtom2(5, particle.self);									// trigger transition effects
		ParticleStateEvents.toAtom2 += particle.TransitionToAtom2;						// flag transition in delegate
		particle.currentState = particle.atom2State;									// set to new state
	}

	public void ToElement()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void Evol()												// all states here for init 
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
		else if (particle.evol >= 8f)
			ToAtom2 ();
	}
}
