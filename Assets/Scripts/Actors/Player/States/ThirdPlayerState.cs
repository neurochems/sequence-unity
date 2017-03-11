using UnityEngine;
using System.Collections;

public class ThirdPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public ThirdPlayerState (PlayerStatePattern playerStatePattern)						// constructor
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
			= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern

		if (canCollide) {																		// if collision allowed
			
			if (other.gameObject.CompareTag ("Zero")										// collide with zero
				|| other.gameObject.CompareTag ("First")									// collide with first
				|| other.gameObject.CompareTag ("Second")									// collide with second
				|| other.gameObject.CompareTag ("Third")) {									// collide with third
				psp.stunned = true;																// set stunned flag
				psp.AddDark (pspOther.darkEvol);												// add dark of other
				psp.AddLight (pspOther.lightEvol);												// add light of other
				canCollide = false;																// reset has collided trigger
			} 
			else {																			// collide with any other
				psp.stunned = true;																// set stunned flag
				psp.SubDark (pspOther.darkEvol);												// subtract other dark
				psp.SubLight (pspOther.lightEvol);												// subtract other light
				canCollide = false;																// reset has collided trigger
			}
		}
	}

	public void Death()
	{
		psp.TransitionToDead(3, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						//  flag transition in delegate
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

	public void ToFifth(bool light)
	{
		psp.TransitionToFifth(light, 3, psp.self);								// trigger transition effects
		ParticleStateEvents.toFifth += psp.TransitionToFifth;					// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool light)
	{
		psp.TransitionToSixth(light, 3, psp.self);								// trigger transition effects
		ParticleStateEvents.toSixth += psp.TransitionToSixth;					// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool light)
	{
		Debug.Log ("Seventh");
	}

	public void Evol()
	{
		float deltaDark = psp.deltaDark;												// local dark check
		float deltaLight = psp.deltaLight;												// local light check

		if (psp.evol <= 0f) {																// to dead (if evol < 0)
			Death ();																			// to death state
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
}
