using UnityEngine;
using System.Collections;

public class Atom2PlayerState : IParticleState 
{

	private readonly PlayerStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public Atom2PlayerState (PlayerStatePattern playerStatePattern)						// constructor
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
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(3.0f);			// subtract 3 evol from other
			canCollide = false;																	// reset has collided trigger
		} 
		else if (other.gameObject.CompareTag ("Electron") && canCollide) {					// collide with electron
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(1.0f);																// add 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(3.0f);			// subtract 3 evol from other
			canCollide = false;																	// reset has collided trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2") && canCollide) {					// collide with electron2
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(1.0f);																// add 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(3.0f);			// subtract 3 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Shell") && canCollide) {						// collide with shell
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(1.0f);																// add 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(3.0f);			// subtract 3 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Shell2") && canCollide) {					// collide with shell
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(1.0f);																// add 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(3.0f);			// subtract 3 evol from other
			canCollide = false;																	// reset has collided trigger
		}
		else if (other.gameObject.CompareTag ("Atom") && canCollide) {						// collide with atom
			particle.GetComponent<PlayerStatePattern> ().stunned = true;						// stun for duration
			particle.AddEvol(1.0f);																// add 1 evol
			other.gameObject.GetComponent<ParticleStatePattern>().SubtractEvol(3.0f);			// subtract 3 evol from other
			canCollide = false;																	// reset has collided trigger
		}
	}

	public void Death()
	{
		particle.currentState = particle.deadState;								// set to new state
	}

	public void ToPhoton()
	{
		particle.currentState = particle.photonState;							// set to new state
	}

	public void ToElectron()
	{
		particle.currentState = particle.electronState;							// set to new state
	}

	public void ToElectron2()
	{
		particle.currentState = particle.electron2State;						// set to new state
	}

	public void ToShell()
	{
		particle.currentState = particle.shellState;							// set to new state
	}

	public void ToShell2()
	{
		particle.currentState = particle.shell2State;							// set to new state
	}

	public void ToAtom()
	{
		particle.currentState = particle.atomState;								// set to new state
	}

	public void ToAtom2()
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
		else if (particle.evol >= 2f && particle.evol < 3f)
			ToShell ();
		else if (particle.evol >= 3f && particle.evol < 5f)
			ToShell2 ();
		//		else if (particle.evol >= 8f)
		//			ToAtom2 ();
	}
}
