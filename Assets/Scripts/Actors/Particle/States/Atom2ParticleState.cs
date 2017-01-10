using UnityEngine;
using System.Collections;

public class Atom2ParticleState : IParticleState 
{

	private readonly ParticleStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset can collide timer

	public Atom2ParticleState (ParticleStatePattern statePatternParticle)				// constructor
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
		bool rolling = true;																											// roll die trigger

		if (other.gameObject.CompareTag ("Player") && canCollide) {																		// colide with player
			particle.GetComponent<ParticleStatePattern> ().stunned = true;																	// stun new particle for 3 sec
			canCollide = false;																												// reset can collide trigger	
			Debug.Log ("particle contact player");
		} 
		else if (other.gameObject.CompareTag ("Photon") && canCollide) {																// collide with photon
			particle.GetComponent<ParticleStatePattern> ().stunned = true;																	// stun new particle for 3 sec
			particle.AddEvol(0.5f);																											// add 0.5 evol
			canCollide = false;																												// reset can collide trigger
		} 
		else if (other.gameObject.CompareTag ("Electron") && canCollide) {																// collide with electron
			particle.GetComponent<ParticleStatePattern> ().stunned = true;																	// stun new particle for 3 sec
			particle.AddEvol(1.0f);																											// add 1 evol
			canCollide = false;																												// reset can collide trigger
		} 
		else if (other.gameObject.CompareTag ("Electron2") && canCollide) {																// collide with electron2
			particle.GetComponent<ParticleStatePattern> ().stunned = true;																	// stun new particle for 3 sec
			particle.AddEvol(1.0f);																											// add 1 evol
			canCollide = false;																												// reset can collide trigger
		}
		else if (other.gameObject.CompareTag ("Shell") && canCollide) {																	// collide with shell
			particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			particle.AddEvol(1.0f);																											// add 1 evol
			canCollide = false;																												// reset can collide trigger
		}
		else if (other.gameObject.CompareTag ("Shell2") && canCollide) {																// collide with shell2
			particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
			particle.AddEvol(1.0f);																											// add 1 evol
			canCollide = false;																												// reset can collide trigger
		}
		else if (other.gameObject.CompareTag ("Atom") && canCollide) {																	// collide with atom
			if ((particle.evol == other.gameObject.GetComponentInParent<ParticleStatePattern> ().evol) && rolling && canCollide) {			// if = other electron
				particle.GetComponent<ParticleStatePattern> ().stunned = true;																	// stun new particle for 3 sec
				particle.RollDie(other.gameObject.GetComponentInParent<ParticleStatePattern> (), 1.0f, 2.0f);									// roll die (win 1, lose -2)
				canCollide = false;																												// reset can collide trigger
			}
			else if ((particle.evol > other.gameObject.GetComponentInParent<ParticleStatePattern> ().evol) && canCollide) {					// if > other electron 
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				particle.AddEvol(1.0f);																											// add 1 evol
				canCollide = false;																												// reset can collide trigger
			}
			else if ((particle.evol < other.gameObject.GetComponentInParent<ParticleStatePattern> ().evol) && canCollide) {					// if < other electron, or photon
				particle.GetComponentInParent<ParticleStatePattern> ().stunned = true;															// stun new particle for 3 sec
				particle.SubtractEvol(2.0f);																									// subtract 2 evol
				canCollide = false;																												// reset can collide trigger
			}
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

/*	private void Sense()
	{
		// search for photons to attract to
		RaycastHit hit;
		Vector3 particleToTarget = particle.attractionTarget.position;																				// attract to other
		if (Physics.Raycast(particle.transform.position, particleToTarget, out hit, particle.attractionRange) && hit.collider.CompareTag("Photon")) {
			particle.attractionTarget = hit.transform;
		}
	}*/

/*	private void Stun()															/* trigger post-hit invulnerability
	{
		particle.Stun ();

		/*		particle.Stun (true);															// disable collider
		stunTimer += Time.deltaTime;													// start timer
		if (stunTimer >= particle.stunDuration) {										// if timer >= duration
			particle.Stun (false);															// enable collider
			stunTimer = 0f;																	// reset timer
		}
	} */

}
