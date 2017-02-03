using UnityEngine;
using System.Collections;

public class SixthPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public SixthPlayerState (PlayerStatePattern playerStatePattern)						// constructor
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
				|| other.gameObject.CompareTag ("Sixth")) {											// collide with sixth
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
		psp.TransitionToDead(6, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 Firsts
		psp.SpawnZero(4);														// spawn 4 Zeros
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero()
	{
		psp.TransitionToZero(6, psp.self);										// trigger transition effects
		ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 Firsts
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
		psp.TransitionToFirst(light, 6, psp.self);								// trigger transition effects
		ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 1 First
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool light)
	{
		psp.TransitionToSecond(light, 6, psp.self);								// trigger transition effects
		ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(3);														// spawn 3 Zero
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool light)
	{
		psp.TransitionToThird(light, 6, psp.self);								// trigger transition effects
		ParticleStateEvents.toThird += psp.TransitionToThird;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(2);														// spawn 2 Zero
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool light)
	{
		psp.TransitionToFourth(light, 6, psp.self);								// trigger transition effects
		ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool light)
	{
		psp.TransitionToFifth(light, 6, psp.self);								// trigger transition effects
		ParticleStateEvents.toFifth += psp.TransitionToFifth;					// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool light)
	{
		Debug.Log ("Can't transition to same state");
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
		else if (psp.evol == 0.5) {																			// to zero (if evol = 0.5)
			if (deltaDark <= -7.5 && deltaDark >= -12) ToLightZero();											// if lose dark = to light
			else if (deltaLight <= -7.5 && deltaLight >= -12) ToDarkZero();										// if lose light = to dark
		} 
		else if (psp.evol == 1f) {																			// to first (if evol = 1)
			if (deltaDark <= -7 && deltaDark >= -11.5) ToFirst(true);											// if lose dark = to light
			else if (deltaLight <= -7 && deltaLight >= -11.5) ToFirst(false);									// if lose light = to dark
		} 
		else if (psp.evol == 1.5f) {																		// to second (if evol = 1.5)
			if (deltaDark <= -6.5 && deltaDark >= -11) ToSecond(true);											// if lose dark = to light
			else if (deltaLight <= -6.5 && deltaLight >= -11) ToSecond(false);									// if lose light = to dark
		} 
		else if (psp.evol >= 2f && psp.evol < 3f) {															// to third (if evol >= 2 and < 3)
			if (deltaDark <= -6 && deltaDark >= -10) ToThird(true);												// if lose dark = to light
			else if (deltaLight <= -6 && deltaLight >= -10) ToThird(false);										// if lose light = to dark
		} 
		else if (psp.evol >= 3f && psp.evol < 5f) {															// to fourth (if evol >= 3 and < 5)
			if (circle && deltaDark <= -5 && deltaDark >= -8) ToFourth(false);									// if circle & lose dark = to dark
			else if (circle && deltaLight <= -5 && deltaLight >= -8) ToFourth(false);							// if circle & lose light = to dark
			else if ((triangle || square) && deltaDark <= -5 && deltaDark >= -8) ToFourth(true);				// if triangle or square & lose dark = to light
			else if ((triangle || square) && deltaLight <= -5 && deltaLight >= -8) ToFourth(true);				// if triangle or square & lose light = to light
		} 
		else if (psp.evol >= 5f && psp.evol < 8f) {															// to fifth (if evol >= 5 and < 8)
			if ((circle && !light) && deltaDark <= -3 && deltaDark >= -5) ToFifthCircle(true);					// if dark circle & lose dark = to light circle
			else if ((circle && !light) && deltaLight <= -3 && deltaLight >= -5) ToFifthCircle(false);			// if dark circle & lose light = to dark circle
			else if ((circle && light) && deltaDark <= -3 && deltaDark >= -5) ToFifthCircle(true);				// if light circle & lose dark = to light circle
			else if ((circle && light) && deltaLight <= -3 && deltaLight >= -5) ToFifthCircle(false);			// if light circle & lose light = to dark circle
			else if (triangle && deltaDark <= -3 && deltaDark >= -5) ToFifthTriangle();							// if triangle2 & lose dark = to triangle
			else if (triangle && deltaLight <= -3 && deltaLight >= -5) ToFifthTriangle();						// if triangle2 & lose light = to triangle
			else if (square && deltaDark <= -3 && deltaDark >= -5) ToFifthSquare();								// if square2 & lose dark = to square
			else if (square && deltaLight <= -3 && deltaLight >= -5) ToFifthSquare();							// if square2 & lose light = to square
		} 
		else if (psp.evol >= 13f) {																			// to seventh (if evol >= 13)
			if ((circle && !light) && (deltaDark >= 0.5 && deltaDark <= 5)) ToSeventhCircle(false);				// if dark circle & gain dark = to dark circle
			else if ((circle && !light) && (deltaLight >= 0.5 && deltaLight <= 5)) ToSeventhCircle(true);		// if dark circle & gain light = to light circle
			else if ((circle && light) && (deltaDark >= 0.5 && deltaDark <= 5)) ToSeventhCircle(false);			// if light circle & gain dark = to dark circle
			else if ((circle && light) && (deltaLight >= 0.5 && deltaLight <= 5)) ToSeventhCircle(true);		// if light circle & gain light = to light circle
			else if (triangle && (deltaDark >= 0.5 && deltaDark <= 5)) ToSeventhTriangle(false);				// if triangle2 & gain dark = to dark triangle
			else if (triangle && (deltaLight >= 0.5 && deltaLight <= 5)) ToSeventhTriangle(true);				// if triangle2 & gain light = to light triangle
			else if (square && (deltaDark >= 0.5 && deltaDark <= 5)) ToSeventhSquare(false);					// if square2 & gain dark = to dark square
			else if (square && (deltaLight >= 0.5 && deltaLight <= 5)) ToSeventhSquare(true);					// if square2 & gain light = to light square
		}
	}
}
