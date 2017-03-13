using UnityEngine;
using System.Collections;

public class FifthParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset can collide timer

	public FifthParticleState (ParticleStatePattern statePatternParticle)				// constructor
	{
		psp = statePatternParticle;															// attach state pattern to this state 
	}

	public void UpdateState()
	{
		Evol ();

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;										// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.stunned = false;															// reset stunned trigger
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern pspOther 
			= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern

		if (canCollide) {																	// if collision allowed
		
			if (other.gameObject.CompareTag ("Player")) {										// colide with player
				psp.stunned = true;																	// stun new particle for 3 sec
				canCollide = false;																	// reset can collide trigger	
				Debug.Log ("particle contact player");
			} 
			if (other.gameObject.CompareTag ("Zero")											// collide with zero
				|| other.gameObject.CompareTag ("First")										// collide with first
				|| other.gameObject.CompareTag ("Second")										// collide with second
				|| other.gameObject.CompareTag ("Third")										// collide with third
				|| other.gameObject.CompareTag ("Fourth")										// collide with fourth
				|| other.gameObject.CompareTag ("Fifth")) {										// collide with fifth
				psp.stunned = true;																	// set stunned flag
				psp.AddDark (pspOther.darkEvol);													// add dark of other
				psp.AddLight (pspOther.lightEvol);													// add light of other
				canCollide = false;																	// reset has collided trigger
			} 
			else {																				// collide with any other
				psp.stunned = true;																	// set stunned flag
				psp.SubDark (pspOther.darkEvol);													// subtract other dark
				psp.SubLight (pspOther.lightEvol);													// subtract other light
				canCollide = false;																	// reset has collided trigger
			} 
		}
	}

	public void Death(bool toLight)
	{
		psp.TransitionTo(5, -1, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 Firsts
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(5, 0, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 Firsts
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(5, 1, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(3);														// spawn 2 Zeros
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(5, 2, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(2);														// spawn 2 Zero
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(5, 3, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(5, 4, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(5, 6, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;					// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(5, 7, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.seventhState;									// set to new state
	}

	public void Evol()
	{
		float deltaDark = psp.deltaDark;																// local dark check
		float deltaLight = psp.deltaLight;																// local light check

		if (psp.evol <= 0f) {																				// to dead (if evol < 0)
			Death (true);																							// to dead state
		} 
		else if (psp.evol == 0) {																			// to zero (if evol = 0)
			ToZero (true);																							// to zero state
		} 
		else if (psp.evol == 0.5f) {																		// to zero (if evol = 0.5)
			if (deltaDark <= -4.5 && deltaDark >= -7) ToZero (true);											// if lose dark = to light
			else if (deltaLight <= -4.5 && deltaLight >= -7) ToZero (false);										// if lose light = to dark
		}
		else if (psp.evol == 1f) {																			// to first (if evol = 1)
			if (deltaDark <= -4 && deltaDark >= -6.5) ToFirst (true);											// if lose dark = to light
			else if (deltaLight <= -4 && deltaLight >= -6.5) ToFirst (false);									// if lose light = to dark
		}
		else if (psp.evol == 1.5f) {																		// to second (if evol = 1.5)
			if (deltaDark <= -3.5 && deltaDark >= -6) ToSecond (true);											// if lose dark = to light
			else if (deltaLight <= -3.5 && deltaLight >= -6) ToSecond (false);									// if lose light = to dark
		}
		else if (psp.evol >= 2f && psp.evol < 3f) {															// to third (if evol >= 2 and < 3)
			if (deltaDark <= -3 && deltaDark >= -5) ToThird (true);												// if lose dark = to light
			else if (deltaLight <= -3 && deltaLight >= -5) ToThird (false);										// if lose light = to dark
		}
		else if (psp.evol >= 3f && psp.evol < 5f) {															// to fourth (if evol >= 3 and < 5)
			if (circle && deltaDark <= -2 && deltaDark >= -3) ToFourth (false);									// if circle & lose dark = to dark
			else if (circle && deltaLight <= -2 && deltaLight >= -3) ToFourth (false);							// if circle & lose light = to dark
			else if ((triangle || square) && deltaDark <= -2 && deltaDark >= -3) ToFourth (true);				// if triangle or square & lose dark = to light
			else if ((triangle || square) && deltaLight <= -2 && deltaLight >= -3) ToFourth (true);				// if triangle or square & lose light = to light
		}
		else if (psp.evol >= 8f) {																			// to sixth (if evol >= 8)
			if ((circle && !light) && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixth(false, 0);					// if dark circle & gain dark = to dark circle
			else if ((circle && !light) && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixth(true, 0);			// if dark circle & gain light = to light circle
			else if ((circle && light) && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixth(false, 0);				// if light circle & gain dark = to dark circle
			else if ((circle && light) && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixth(true, 0);				// if light circle & gain light = to light circle
			else if (triangle && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixth(false, 1);						// if triangle & gain dark = to triangle2
			else if (triangle && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixth(false, 1);						// if triangle & gain light = to triangle2
			else if (square && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixth(false, 2);							// if square & gain dark = to square2
			else if (square && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixth(false, 2);						// if square & gain light = to square2
		}
	}

/*	private void Sense()
	{
		// search for photons to attract to
		RaycastHit hit;
		Vector3 particleToTarget = particle.attractionTarget.position;																				// attract direct to other
		if (Physics.Raycast(particle.transform.position, particleToTarget, out hit, particle.attractionRange) && hit.collider.CompareTag("Photon")) {
			particle.attractionTarget = hit.transform;
		}
		else if (Physics.Raycast(particle.transform.position, particleToTarget, out hit, particle.attractionRange) && hit.collider.CompareTag("Electron"))
		{
			particle.attractionTarget = hit.transform;
		}
	} */
}
