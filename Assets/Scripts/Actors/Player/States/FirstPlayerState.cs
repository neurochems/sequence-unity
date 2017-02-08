﻿using UnityEngine;
using System.Collections;

public class FirstPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public FirstPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		Evol ();																		// check evol

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer up
			canCollide = true;																// set collision ability
			psp.stunned = false;															// reset stunned flag
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern pspOther 
			= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern

		if (canCollide) {																// if collision allowed

			if (other.gameObject.CompareTag ("Zero")										// collide with zero
				|| other.gameObject.CompareTag ("First")) {									// collide with first	
				psp.stunned = true;																// stun for duration
				psp.AddDark (pspOther.darkEvol);												// add dark of other
				psp.AddLight (pspOther.lightEvol);												// add light of other
				canCollide = false;																// reset has collided trigger
			}
			else {																			// collide with any other
				psp.stunned = true;																// stun for duration
				psp.SubDark (pspOther.darkEvol);												// subtract other dark
				psp.SubLight (pspOther.lightEvol);												// subtract other light
				canCollide = false;																// reset has collided trigger
			}
		}				
	}

	public void Death()
	{
		psp.TransitionToDead(1, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.SpawnZero(2);														// spawn 2 zero
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero(bool light)
	{
		psp.TransitionToZero(light, 1, psp.self);								// trigger transition effects
		ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnZero(1);														// spawn 1 zero
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToLightZero(bool light)
	{
		psp.TransitionToZero(light, 0, psp.self);								// trigger transition effects
		ParticleStateEvents.toLightZero += psp.TransitionToLightZero;			// flag transition in delegate
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToDarkZero(bool light)
	{
		psp.TransitionToZero(light, 0, psp.self);								// trigger transition effects
		ParticleStateEvents.toDarkZero += psp.TransitionToDarkZero;				// flag transition in delegate
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToFirst(bool light)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSecond(bool light)
	{
		psp.TransitionToSecond(light, 1, psp.self);								// trigger transition effects
		ParticleStateEvents.toSecond += psp.TransitionToSecond;				// flag transition in delegate
		psp.currentState = psp.secondState;								// set to new state
	}

	public void ToThird(bool light)
	{
		psp.TransitionToThird(light, 1, psp.self);									// trigger transition effects
		ParticleStateEvents.toThird += psp.TransitionToThird;						// flag transition in delegate
		psp.currentState = psp.thirdState;									// set to new state
	}

	public void ToFourth(bool light)
	{
		psp.TransitionToFourth(light, 1, psp.self);									// trigger transition effects
		ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.currentState = psp.fourthState;									// set to new state
	}

	public void ToFifth(bool light)
	{
		psp.TransitionToFifth(light, 1, psp.self);									// trigger transition effects
		ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool light)
	{
		psp.TransitionToSixth(light, 1, psp.self);									// trigger transition effects
		ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;									// set to new state
	}

	public void ToSeventh(bool light)
	{
		Debug.Log ("Seventh");
	}

	public void Evol()									// all states here for init 
	{
		float deltaDark = psp.darkEvolDelta;										// local dark check
		float deltaLight = psp.lightEvolDelta;										// local light check

		if (psp.evol <= 0f) {														// to dead (if evol < 0)
			Death ();																	// to dead state
		} 
		else if (psp.evol == 0) {													// to zero (if evol = 0)
			ToZero ();																	// to zero state
		}
		else if (psp.evol == 0.5f) {												// to zero (if evol = 0.5)
			if (deltaDark == -0.5f) ToDarkZero ();										// if gain dark = to dark zero
			else if (deltaLight == -0.5f) ToLightZero ();								// if gain light = to light zero
		}
		else if (psp.evol >= 1.5f) {												// to second (if evol = 1.5)
			if (!light && deltaDark == 0.5) ToSecond(false);							// if dark & gain dark = to dark
			else if (!light && deltaLight == 0.5) ToSecond(true);						// if dark & gain light = to light
			else if (light && deltaDark == 0.5) ToSecond(false);						// if light & gain dark = to dark
			else if (light && deltaLight == 0.5) ToSecond(true);						// if light & gain light = to light
		} 
	}
}
