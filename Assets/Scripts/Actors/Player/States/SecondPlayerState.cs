using UnityEngine;
using System.Collections;

public class SecondPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public SecondPlayerState (PlayerStatePattern playerStatePattern)					// constructor
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
			psp.stunned = true;																// reset stunned flag
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern pspOther 
			= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern

		if (canCollide) {																		// if collision allowed
			
			if (other.gameObject.CompareTag ("Zero") 											// collide with Zero
				|| other.gameObject.CompareTag ("First") 										// collide with first
				|| other.gameObject.CompareTag ("Second")) {									// collide with second
				psp.stunned = true;																	// set stunned flag
				psp.AddDark (pspOther.darkEvol);													// add dark of other
				psp.AddLight (pspOther.lightEvol);													// add light of other
				canCollide = false;																		// reset has collided trigger
			} 
			else {																				// collide with any other
				psp.stunned = true;																	// set stunned flag
				psp.SubDark (pspOther.darkEvol);													// subtract other dark
				psp.SubLight (pspOther.lightEvol);													// subtract other light
				canCollide = false;																	// reset has collided trigger
			}
		}
	}

	public void Death()
	{
		psp.TransitionToDead(2, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero()
	{
		psp.TransitionToZero(2, psp.self);										// trigger transition effects
		ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
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
		psp.TransitionToFirst(light, 2, psp.self);								// trigger transition effects
		ParticleStateEvents.toFirst += psp.TransitionToFirst;				// flag transition in delegate
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.firstState;									// set to new state
	}

	public void ToSecond(bool light)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToThird(bool light)
	{
		psp.TransitionToThird(light, 2, psp.self);									// trigger transition effects
		ParticleStateEvents.toThird += psp.TransitionToThird;						// flag transition in delegate
		psp.currentState = psp.thirdState;									// set to new state
	}

	public void ToFourth(bool light)
	{
		psp.TransitionToFourth(light, 2, psp.self);									// trigger transition effects
		ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.currentState = psp.fourthState;									// set to new state
	}

	public void ToFifth(bool light)
	{
		psp.TransitionToFifth(light, 2, psp.self);									// trigger transition effects
		ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool light)
	{
		psp.TransitionToSixth(light, 2, psp.self);									// trigger transition effects
		ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;									// set to new state
	}

	public void ToSeventh(bool light)
	{
		Debug.Log ("Seventh");
	}

	public void Evol() 
	{
		float deltaDark = psp.darkEvolDelta;										// local dark check
		float deltaLight = psp.lightEvolDelta;										// local light check

		if (psp.evol <= 0f) {														// to dead (if evol < 0)
			Death ();																	// to death state
		}
		else if (psp.evol == 0) {													// to zero (if evol = 0)
			ToZero ();																	// to zero state
		}
		else if (psp.evol == 0.5f) {												// to zero (if evol = 0.5)
			if (deltaDark == -1f) ToDarkZero ();										// if gain dark = to dark zero
			else if (deltaLight == -1f) ToLightZero ();									// if gain light = to light zero
		}
		else if (psp.evol == 1f) {													// to first (if evol = 1)
			if (deltaDark == -0.5) ToFirst(true);										// if lose dark = to light
			else if (deltaLight == -0.5) ToFirst(false);								// if lose light = to dark															
		}
		else if (psp.evol >= 2f) {													// to third (if evol = 2)
			if (!light && deltaDark == 0.5) ToThird(false);								// if dark & gain dark = to dark
			else if (!light && deltaLight == 0.5) ToThird(true);						// if dark & gain light = to light
			else if (light && deltaDark == 0.5) ToThird(false);							// if light & gain dark = to dark
			else if (light && deltaLight == 0.5) ToThird(true);							// if light & gain light = to light
		}
	}
}
