﻿using UnityEngine;
using System.Collections;

public class SeventhPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public SeventhPlayerState (PlayerStatePattern playerStatePattern)					// constructor
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
				|| other.gameObject.CompareTag ("Second")											// collide with second
				|| other.gameObject.CompareTag ("Third")											// collide with third
				|| other.gameObject.CompareTag ("Fourth")											// collide with fourth
				|| other.gameObject.CompareTag ("Fifth")											// collide with fifth
				|| other.gameObject.CompareTag ("Sixth")											// collide with sixth
				|| other.gameObject.CompareTag ("Seventh")) {										// collide with seventh
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
		psp.TransitionToDead(7, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.SpawnFirst(5);														// spawn 5 Firsts
		psp.SpawnFirst(2);														// spawn 2 Firsts
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero()
	{
		psp.TransitionToZero(7, psp.self);										// trigger transition effects
		ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnFirst(4);														// spawn 4 Firsts
		psp.SpawnZero(3);														// spawn 3 Zeros
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
		psp.TransitionToFirst(light, 7, psp.self);								// trigger transition effects
		ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnFirst(4);														// spawn 4 First
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool light)
	{
		psp.TransitionToSecond(light, 7, psp.self);								// trigger transition effects
		ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnFirst(3);														// spawn 3 First
		psp.SpawnZero(3);														// spawn 3 Zero
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool light)
	{
		psp.TransitionToThird(light, 7, psp.self);								// trigger transition effects
		ParticleStateEvents.toThird += psp.TransitionToThird;					// flag transition in delegate
		psp.SpawnFirst(3);														// spawn 3 First
		psp.SpawnZero(2);														// spawn 2 Zero
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool light)
	{
		psp.TransitionToFourth(light, 7, psp.self);								// trigger transition effects
		ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 First
		psp.SpawnZero(3);														// spawn 3 Zero
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool light)
	{
		psp.TransitionToFifth(light, 7, psp.self);								// trigger transition effects
		ParticleStateEvents.toFifth += psp.TransitionToFifth;					// flag transition in delegate
		psp.SpawnFirst(3);														// spawn 3 First
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool light)
	{
		psp.TransitionToSixth(light, 7, psp.self);								// trigger transition effects
		ParticleStateEvents.toSixth += psp.TransitionToSixth;					// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 First
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void Evol()
	{
		float deltaDark = psp.darkEvolDelta;																// local dark check
		float deltaLight = psp.lightEvolDelta;																// local light check

		if (psp.evol <= 0f) {																				// to dead (if evol < 0)
			Death ();																							// to dead state
		} 
		else if (psp.evol == 0) {																			// to zero (if evol = 0)
			ToZero ();																							// to zero state
		}
		else if (psp.evol == 0.5) {																			// to zero (if evol = 0.5)
			if (deltaDark <= -12.5 && deltaDark >= -20) ToLightZero();											// if lose dark = to light
			else if (deltaLight <= -12.5 && deltaLight >= -20) ToDarkZero();									// if lose light = to dark
		} 
		else if (psp.evol == 1f) {																			// to first (if evol = 1)
			if (deltaDark <= -12 && deltaDark >= -19.5) ToFirst(true);											// if lose dark = to light
			else if (deltaLight <= -12 && deltaLight >= -19.5) ToFirst(false);									// if lose light = to dark
		} 
		else if (psp.evol == 1.5f) {																		// to second (if evol = 1.5)
			if (deltaDark <= -11.5 && deltaDark >= -19) ToSecond(true);											// if lose dark = to light
			else if (deltaLight <= -11.5 && deltaLight >= -19) ToSecond(false);									// if lose light = to dark
		} 
		else if (psp.evol >= 2f && psp.evol < 3f) {															// to third (if evol >= 2 and < 3)
			if (deltaDark <= -11 && deltaDark >= -18) ToThird(true);											// if lose dark = to light			
			else if (deltaLight <= -11 && deltaLight >= -18) ToThird(false);									// if lose light = to dark
		} 
		else if (psp.evol >= 3f && psp.evol < 5f) {															// to fourth (if evol >= 3 and < 5)
			if (circle && deltaDark <= -10 && deltaDark >= -16) ToFourth(false);								// if circle & lose dark = to dark
			if (circle && deltaLight <= -10 && deltaLight >= -16) ToFourth(false);								// if circle & lose light = to dark
			else if ((triangle || square) && deltaDark <= -10 && deltaDark >= -16) ToFourth(true);				// if triangle or square & lose dark = to light
			else if ((triangle || square) && deltaLight <= -10 && deltaLight >= -16) ToFourth(true);			// if triangle or square & lose light = to light
		} 
		else if (psp.evol >= 5f && psp.evol < 8f) {															// to fifth (if evol >= 5 and < 8)
			if ((circle && !light) && deltaDark <= -8 && deltaDark >= -13) ToFifthCircle(true);					// if dark circle & lose dark = to light circle
			else if ((circle && !light) && deltaLight <= -8 && deltaLight >= -13) ToFifthCircle(false);			// if dark circle & lose light = to dark circle
			else if ((circle && light) && deltaDark <= -8 && deltaDark >= -13) ToFifthCircle(true);				// if light circle & lose dark = to light circle
			else if ((circle && light) && deltaLight <= -8 && deltaLight >= -13) ToFifthCircle(false);			// if light circle & lose light = to dark circle
			else if (triangle && deltaDark <= -8 && deltaDark >= -13) ToFifthTriangle();						// if any triangle & lose dark = to triangle
			else if (triangle && deltaLight <= -8 && deltaLight >= -13) ToFifthTriangle();						// if any triangle & lose light = to triangle
			else if (square && deltaDark <= -8 && deltaDark >= -13) ToFifthSquare();							// if any square & lose dark = to square
			else if (square && deltaLight <= -8 && deltaLight >= -13) ToFifthSquare();							// if any square & lose light = to square
		} 
		else if (psp.evol >= 8f && psp.evol < 13f) {														// to sixth (if evol >= 8 and < 13)
			if ((circle && !light) && deltaDark <= -5 && deltaDark >= -8) ToSixthCircle(true);					// if dark circle & lose dark = to light circle
			else if ((circle && !light) && deltaLight <= -5 && deltaLight >= -8) ToSixthCircle(false);			// if dark circle & lose light = to dark circle
			else if ((circle && light) && deltaDark <= -5 && deltaDark >= -8) ToSixthCircle(true);				// if light circle & lose dark = to light circle
			else if ((circle && light) && deltaLight <= -5 && deltaLight >= -8) ToSixthCircle(false);			// if light circle & lose light = to dark circle
			else if (triangle && deltaDark <= -5 && deltaDark >= -8) ToSixthTriangle();							// if any triangle & lose dark = to triangle2
			else if (triangle && deltaLight <= -5 && deltaLight >= -8) ToSixthTriangle();						// if any triangle & lose light = to triangle2
			else if (square && deltaDark <= -5 && deltaDark >= -8) ToSixthSquare();								// if any square & lose dark = to square2
			else if (square && deltaLight <= -5 && deltaLight >= -8) ToSixthSquare();							// if any square & lose light = to square2
		}
		else if (psp.evol >= 21) {																			// to eighth (if evol >= 21)
			if ((circle && !light) && (deltaDark >= 0.5 && deltaDark <= 8)) ToEighthCircle(false);				// if dark circle & gain dark = to dark circle
			else if ((circle && !light) && (deltaLight >= 0.5 && deltaLight <= 8)) ToEighthCircle(true);		// if dark circle & gain light = to light circle
			else if ((circle && light) && (deltaDark >= 0.5 && deltaDark <= 8)) ToEighthCircle(false);			// if light circle & gain dark = to dark circle
			else if ((circle && light) && (deltaLight >= 0.5 && deltaLight <= 8)) ToEighthCircle(true);			// if light circle & gain light = to light circle
			else if ((triangle && !light) && (deltaDark >= 0.5 && deltaDark <= 8)) ToEighthTriangle(false);		// if dark triangle & gain dark = to dark triangle
			else if ((triangle && !light) && (deltaLight >= 0.5 && deltaLight <= 8)) ToEighthTriangle(true);	// if dark triangle & gain light = to light triangle
			else if ((triangle && light) && (deltaDark >= 0.5 && deltaDark <= 8)) ToEighthTriangle(false);		// if light triangle & gain dark = to dark triangle
			else if ((triangle && light) && (deltaLight >= 0.5 && deltaLight <= 8)) ToEighthTriangle(true);		// if light triangle & gain light = to light triangle
			else if ((square && !light) && (deltaDark >= 0.5 && deltaDark <= 8)) ToEighthSquare(false);			// if dark square & gain dark = to dark square
			else if ((square && !light) && (deltaLight >= 0.5 && deltaLight <= 8)) ToEighthSquare(true);		// if dark square & gain light = to light square
			else if ((square && light) && (deltaDark >= 0.5 && deltaDark <= 8)) ToEighthSquare(false);			// if light square & gain dark = to dark square
			else if ((square && light) && (deltaLight >= 0.5 && deltaLight <= 8)) ToEighthSquare(true);			// if light square & gain light = to light square
		}
	}
}
