using UnityEngine;
using System.Collections;

public class FifthPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public FifthPlayerState (PlayerStatePattern playerStatePattern)						// constructor
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
			psp.stunned = false;															// reset stunned trigger
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
				|| other.gameObject.CompareTag ("Fifth")) {											// collide with fifth
				psp.stunned = true;																		// set stunned flag
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
		psp.TransitionToDead(5, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 Firsts
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero()
	{
		psp.TransitionToZero(5, psp.self);										// trigger transition effects
		ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 Firsts
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
		psp.TransitionToFirst(light, 5, psp.self);								// trigger transition effects
		ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(3);														// spawn 2 Zeros
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool light)
	{
		psp.TransitionToSecond(light, 5, psp.self);								// trigger transition effects
		ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(2);														// spawn 2 Zero
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool light)
	{
		psp.TransitionToThird(light, 5, psp.self);								// trigger transition effects
		ParticleStateEvents.toThird += psp.TransitionToThird;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool light)
	{
		psp.TransitionToFourth(light, 5, psp.self);								// trigger transition effects
		ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool light)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSixth(bool light)
	{
		psp.TransitionToSixth(light, 5, psp.self);								// trigger transition effects
		ParticleStateEvents.toSixth += psp.TransitionToSixth;					// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool light)
	{
		Debug.Log ("Seventh");
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
		else if (psp.evol == 0.5f) {																		// to zero (if evol = 0.5)
			if (deltaDark <= -4.5 && deltaDark >= -7) ToLightZero ();											// if lose dark = to light
			else if (deltaLight <= -4.5 && deltaLight >= -7) ToDarkZero ();										// if lose light = to dark
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
			if ((circle && !light) && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixthCircle(false);				// if dark circle & gain dark = to dark circle
			else if ((circle && !light) && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixthCircle(true);			// if dark circle & gain light = to light circle
			else if ((circle && light) && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixthCircle(false);			// if light circle & gain dark = to dark circle
			else if ((circle && light) && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixthCircle(true);			// if light circle & gain light = to light circle
			else if (triangle && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixthTriangle();						// if triangle & gain dark = to triangle2
			else if (triangle && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixthTriangle();						// if triangle & gain light = to triangle2
			else if (square && (deltaDark >= 0.5 && deltaDark <= 3)) ToSixthSquare();							// if square & gain dark = to square2
			else if (square && (deltaLight >= 0.5 && deltaLight <= 3)) ToSixthSquare();							// if square & gain light = to square2
		}
	}
}
