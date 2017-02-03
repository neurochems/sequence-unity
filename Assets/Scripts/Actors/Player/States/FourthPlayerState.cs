using UnityEngine;
using System.Collections;

public class FourthPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public FourthPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		Evol ();																		// check evol

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
			= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern

		if (canCollide) {																		// if collision allowed

			if (other.gameObject.CompareTag ("Zero")												// collide with zero
				|| other.gameObject.CompareTag ("First")											// collide with first
				|| other.gameObject.CompareTag ("second")											// collide with second
				|| other.gameObject.CompareTag ("Third")											// collide with third
				|| other.gameObject.CompareTag ("Fourth")) {										// collide with fourth
				psp.stunned = true;																		// stun for duration
				psp.AddDark (pspOther.darkEvol);														// add dark of other
				psp.AddLight (pspOther.lightEvol);														// add light of other
				canCollide = false;																		// reset has collided trigger
			} 
			else {																					// collide with any other
				psp.stunned = true;																		// set stunned flag
				psp.SubDark (pspOther.darkEvol);														// subtract other dark
				psp.SubLight (pspOther.lightEvol);														// subtract other light
				canCollide = false;																		// reset has collided trigger
			}
		}
	}

	public void Death()
	{
		psp.TransitionToDead(4, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero()
	{
		psp.TransitionToZero(4, psp.self);										// trigger transition effects
		ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToLightZero()
	{
		// fill out
		// psp.TransitionToZero(true, 0, psp.self);
		// ParticleStateEvents.toLightZero += psp.TransitionToLightZero;
	}

	public void ToDarkZero()
	{
		// fill out
		// psp.TransitionToZero(false, 0, psp.self);
		// ParticleStateEvents.toDarkZero += psp.TransitionToDarkZero;
	}

	public void ToFirst(bool light)
	{
		psp.TransitionToFirst(light, 4, psp.self);								// trigger transition effects
		ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnZero(3);														// spawn 3 Zeros
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool light)
	{
		psp.TransitionToSecond(light, 4, psp.self);								// trigger transition effects
		ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool light)
	{
		psp.TransitionToThird(light, 4, psp.self);								// trigger transition effects
		ParticleStateEvents.toThird += psp.TransitionToThird;					// flag transition in delegate
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool light)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToFifth(bool light)
	{
		psp.TransitionToFifth(light, 4, psp.self);								// trigger transition effects
		ParticleStateEvents.toFifth += psp.TransitionToFifth;					// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool light)
	{
		psp.TransitionToSixth(light, 4, psp.self);								// trigger transition effects
		ParticleStateEvents.toSixth += psp.TransitionToSixth;					// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool light)
	{
		Debug.Log ("Seventh");
	}

	public void Evol()													
	{

		float deltaDark = psp.darkEvolDelta;													// local dark check
		float deltaLight = psp.lightEvolDelta;													// local light check

		if (psp.evol <= 0f) {																	// to dead (if evol < 0)
			Death ();																				// to dead state
		}
		else if (psp.evol = 0) {																// to zero (if evol = 0)
			ToZero ();																				// to zero state
		}
		else if (psp.evol = 0.5f) {																// to zero (if evol = 0.5)
			if (deltaDark <= -2.5 && deltaDark >= -4) ToLightZero();								// if lose dark = to light
			else if (deltaLight <= -2.5 && deltaLight >= -4) ToDarkZero();							// if lose light = to dark
		}
		else if (psp.evol == 1f) {																// to first (if evol = 1)
			if (deltaDark <= -2 && deltaDark >= -3.5) ToFirst(true);								// if lose dark = to light
			else if (deltaLight <= -2 && deltaLight >= -3.5) ToFirst(false);						// if lose light = to dark
		}
		else if (psp.evol == 1.5f) {															// to second (if evol = 1.5)
			if (deltaDark <= -1.5 && deltaDark >= -3) ToSecond(true);								// if lose dark = to light
			else if (deltaLight <= -1.5 && deltaLight >= -3) ToSecond(false);						// if lose light = to dark
		}
		else if (psp.evol >= 2f && psp.evol < 3f) {												// to third (if evol = 2)
			if (deltaDark <= -1 && deltaDark >= -2) ToThird(true);									// if lose dark = to light	
			else if (deltaLight <= -1 && deltaLight >= -2) ToThird(false);							// if lose light = to dark
		}
		else if (psp.evol >= 5f) {																// to fifth (if evol = 5)
			if (!light && (deltaDark >= 0.5 && deltaDark <= 2)) ToFifthCircle(false);				// if dark & gain dark = to dark circle
			else if (!light && (deltaLight >= 0.5 && deltaLight <= 2)) ToFifthCircle(true);			// if dark & gain light = to light circle
			else if (light && (deltaDark >= 0.5 && deltaDark <= 2)) ToFifthTriangle();				// if light & gain dark = to triangle
			else if (light && (deltaLight >= 0.5 && deltaLight <= 2)) ToFifthSquare();				// if light & gain light = to square
		}
	}
}
