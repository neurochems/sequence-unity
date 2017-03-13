using UnityEngine;
using System.Collections;

public class ThirdParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset can collide timer

	public ThirdParticleState (ParticleStatePattern particleStatePattern)				// constructor
	{
		psp = particleStatePattern;															// attach state pattern to this state 
	}

	public void UpdateState()
	{
		Evol ();

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.stunned = false;															// reset stunned flag
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern pspOther 
		= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern


		if (canCollide) {															// if collision allowed
			
			if (other.gameObject.CompareTag ("Player")) {								// colide with player
				psp.stunned = true;															// stun new particle for 3 sec
				canCollide = false;															// reset can collide trigger	
				Debug.Log ("particle contact player");
			} 
			else if (other.gameObject.CompareTag ("Zero")								// collide with zero
				|| other.gameObject.CompareTag ("First")								// collide with first
				|| other.gameObject.CompareTag ("Second")								// collide with second
				|| other.gameObject.CompareTag ("Third")) {								// collide with third
				psp.stunned = true;															// set stunned flag
				psp.AddDark (pspOther.darkEvol);											// add dark of other
				psp.AddLight (pspOther.lightEvol);											// add light of other
				canCollide = false;															// reset has collided trigger
			} 
			else {																		// collide with any other
				psp.stunned = true;															// set stunned flag
				psp.SubDark (pspOther.darkEvol);											// subtract other dark
				psp.SubLight (pspOther.lightEvol);											// subtract other light
				canCollide = false;															// reset has collided trigger
			}
		}
	}

	public void Death(bool toLight)
	{
		psp.TransitionTo(3, -1, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toDead += psp.TransitionToDead;						//  flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(3, 0, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;						//  flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(3, 1, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnZero(3);														// spawn 3 Zeros
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(3, 2, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool toLight)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(3, 4, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(3, 5, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(3, 6, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(3, 7, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.seventhState;									// set to new state
	}

	public void Evol()
	{
		float deltaDark = psp.deltaDark;												// local dark check
		float deltaLight = psp.deltaLight;												// local light check

		if (psp.evol <= 0f) {																// to dead (if evol < 0)
			Death (true);																			// to death state
		}
		else if (psp.evol == 0) {															// to zero (if evol = 0)
			ToZero (true);																			// to zero state
		}
		else if (psp.evol == 0.5f) {														// to zero (if evol = 0.5)
			if (deltaDark <= -1.5 && deltaDark >= -2) ToZero (true);							// if lose dark = to light
			else if (deltaLight <= -1.5 && deltaLight >= -2) ToZero (false);						// if lose light = to dark
		}
		else if (psp.evol == 1f) {															// to first (if evol = 1) 
			if (deltaDark <= -1 && deltaDark >= -1.5) ToFirst(true);							// if lose dark = to light
			else if (deltaLight <= -1 && deltaLight >= -1.5) ToFirst(false);					// if lose light = to dark
		}
		else if (psp.evol == 1.5f) {														// to second (if evol = 1.5)
			if (deltaDark <= -0.5 && deltaDark >= -1) ToSecond(true);							// if lose dark = to light
			else if (deltaLight <= -0.5 && deltaLight >= -1) ToSecond(false);					// if lose light = to dark
		}
		else if (psp.evol >= 3f) {															// to fourth (if evol = 3)
			if (!light && (deltaDark == 0.5 || deltaDark == 1)) ToFourth(false);				// if dark & gain dark = to dark
			else if (!light && (deltaLight == 0.5 || deltaLight == 1)) ToFourth(true);			// if dark & gain light = to light
			else if (light && (deltaDark == 0.5 || deltaDark == 1)) ToFourth(false);			// if light & gain dark = to dark
			else if (light && (deltaLight == 0.5 || deltaLight == 1)) ToFourth(true);			// if light & gain light = to light
		}
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
