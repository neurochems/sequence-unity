using UnityEngine;
using System.Collections;

public class SeventhPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public SeventhPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
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
				psp.stunned = true;                                                                     // stun for duration

                psp.AddDark (pspOther.darkEvol);														// add dark of other
				psp.AddLight (pspOther.lightEvol);                                                      // add light of other

                Evol();																            		// check evol

                canCollide = false;																		// reset has collided trigger
			} 
			else {																					// collide with any other
				psp.stunned = true;                                                                     // set stunned flag

                psp.SubDark (pspOther.darkEvol);														// subtract other dark
				psp.SubLight (pspOther.lightEvol);                                                      // subtract other light

                Evol();																		            // check evol

                canCollide = false;																		// reset has collided trigger
			}
		}
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(7, 0, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnFirst(4);														// spawn 4 Firsts
		psp.SpawnZero(3);														// spawn 3 Zeros
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(7, 1, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnFirst(4);														// spawn 4 First
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(7, 2, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnFirst(3);														// spawn 3 First
		psp.SpawnZero(3);														// spawn 3 Zero
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(7, 3, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;					// flag transition in delegate
		psp.SpawnFirst(3);														// spawn 3 First
		psp.SpawnZero(2);														// spawn 2 Zero
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(7, 4, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 First
		psp.SpawnZero(3);														// spawn 3 Zero
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(7, 5, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;					// flag transition in delegate
		psp.SpawnFirst(3);														// spawn 3 First
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(7, 6, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;					// flag transition in delegate
		psp.SpawnFirst(2);														// spawn 2 First
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToEighth(bool toLight, int shape)
	{
		psp.TransitionTo(7, 8, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toEighth += psp.TransitionToEighth;					// flag transition in delegate
		//psp.currentState = psp.eighthState;										// set to new state
	}

	public void Evol()
	{
		evol = psp.evol;																					// local evol check			
		light = psp.light;																					// update light value
		deltaDark = psp.deltaDark;																			// local dark check
		deltaLight = psp.deltaLight;																		// local light check

		if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;											// if to light world (if evol < 0), set to light world trigger
		else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;										// if to dark world (evol >= 0), set dark world flag
	
        // zero
		if (evol == 0) {																		    		// to zero (if evol = 0)
			ToZero (true);																						// to zero state
		} 
        // half zero
		if (evol == 0.5f) {																		        	// devolve to dark world dark zero (if evol = 0.5)
			if (deltaDark > deltaLight)	ToZero (false);															// if lose more light than dark = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);													// if gain more light than dark = to light zero (no change)
		} 
		else if (evol == -0.5f) {																			// devolve to light world zero (if evol = -0.5)
			if (deltaDark < deltaLight) ToZero (true);															// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToZero (false);													// if lose more light than dark = to dark zero
		} 
        // first
		if ((evol == 1f) || (evol == -1f)) {														    	// devolve to dark or light world first (if evol == 1)
			if (deltaDark < deltaLight)	ToFirst (true);															// if lose more dark than light = to light first
			else if (deltaDark > deltaLight) ToFirst (false);													// if lose more light than dark = to dark first
		} 
        // second
		if ((evol == 1.5f) || (evol == -1.5f)) {												    		// devolve to dark or light world second (if evol == 1.5)
			if (deltaDark < deltaLight)	ToSecond (true);														// if lose more dark than light = to light second
			else if (deltaDark > deltaLight) ToSecond (false);													// if lose more light than dark = to dark second
		} 
        // third
		if ((evol >= 2f && evol < 3f) || (evol <= -2f && evol > -3f)) {						        		// devolve to dark or light world third (if evol = 2)
			if (deltaDark < deltaLight) ToThird (true);															// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird (false);													// if lose more light than dark = to dark third
		} 
        // fourth
		if (evol >= 3f && evol < 5f) {																    	// devolve to dark world fourth (if evol >= 3 and < 5)
			if (circle && (deltaDark < deltaLight)) ToFourth (false);											// if either circle & lose more dark than light = to dark fourth
			else if (circle && (deltaDark > deltaLight)) ToFourth (false);										// if either circle & lose more light than dark = to dark fourth
			else if (triangle && (deltaDark < deltaLight)) ToFourth (true);										// if triangle & lose more dark than light = to light fourth
			else if (triangle && (deltaDark > deltaLight)) ToFourth (true);										// if triangle & lose more light than dark = to light fourth
			else if (square && (deltaDark < deltaLight)) ToFourth (true);										// if square & lose more dark than light = to light fourth
			else if (square && (deltaDark > deltaLight)) ToFourth (true);										// if square & lose more light than dark = to light fourth
		} 
		else if (evol <= -3f && evol > -5f) {																// devolve to light world fourth (if evol == -3)
			if (deltaDark < deltaLight) ToFourth (true);														// if lose more dark than light = to light fourth
			else if (deltaDark > deltaLight) ToFourth (false);													// if lose more light than dark = to dark fourth
		} 
        // fifth
		if (evol >= 5f && evol < 8f) {																    	// devolve to dark world fifth (if evol >= 5 and < 8)
			if (circle && (deltaDark < deltaLight))	ToFifth (true, 0);											// if either circle & lose more dark than light = to light circle fifth
			else if (circle && (deltaDark > deltaLight)) ToFifth (false, 0);									// if either circle & lose more light than dark = to dark circle fifth
			else if (triangle && (deltaDark < deltaLight)) ToFifth (true, 1);									// if triangle & lose more dark than light = to triangle fifth
			else if (triangle && (deltaDark > deltaLight)) ToFifth (true, 1);									// if triangle & lose more light than dark = to triangle fifth
			else if (square && (deltaDark < deltaLight)) ToFifth (true, 2);										// if square & lose more dark than light = to square fifth
			else if (square && (deltaDark > deltaLight)) ToFifth (true, 2);										// if square & lose more light than dark = to square fifth
		} 
		else if (evol <= -5f && evol > -8f) {																// devolve to light world fifth (if evol = -5)
			if (deltaDark < deltaLight) ToFifth (true, 0);														// if lose more dark than light = to light circle fifth
			else if (deltaDark > deltaLight) ToFifth (false, 0);												// if lose more light than dark = to dark circle fifth
		} 
        // sixth
		if (evol >= 8f && evol < 13f) {															        	// devolve to dark world sixth (if evol >= 8 and < 13)
			if (circle && (deltaDark < deltaLight)) ToSixth (true, 0);											// if either circle & lose more dark than light = to light circle sixth
			else if (circle && (deltaDark > deltaLight)) ToSixth (false, 0);									// if either circle & lose more light than dark = to dark circle sixth
			else if (triangle && (deltaDark < deltaLight)) ToSixth (false, 1);									// if triangle & lose more dark than light = to triangle sixth
			else if (triangle && (deltaDark > deltaLight)) ToSixth (false, 1);									// if triangle & lose more light than dark = to triangle sixth
			else if (square && (deltaDark < deltaLight)) ToSixth (false, 2);									// if square & lose more dark than light = to square sixth
			else if (square && (deltaDark > deltaLight)) ToSixth (false, 2);									// if square & lose more light than dark = to square sixth
		} 
		else if (evol <= -8f && evol > -13f) {																// devolve to light world sixth (if evol = -8)
			if (deltaDark < deltaLight) ToSixth (true, 0);														// if lose more dark than light = to light circle sixth
			else if (deltaDark > deltaLight) ToSixth (false, 0);												// if lose more light than dark = to dark circle sixth
		} 
        // eighth
		if (evol >= 21) {																			    	// evolve to dark world eighth (if evol >= 21)
			if (circle && (deltaDark > deltaLight)) ToEighth (false, 0);										// if either circle & gain more dark than light = to dark circle eighth
			else if (circle && (deltaDark < deltaLight)) ToEighth (true, 0);									// if either circle & gain more light than dark = to light circle eighth
			else if (triangle && (deltaDark > deltaLight)) ToEighth (false, 1);									// if either triangle & gain more dark than light = to dark triangle eighth
			else if (triangle && (deltaDark < deltaLight)) ToEighth (true, 1);									// if either triangle & gain more light than dark = to light triangle eighth
			else if (square && (deltaDark > deltaLight)) ToEighth (false, 2);									// if either square & gain more dark than light = to dark square eighth
			else if (square && (deltaDark < deltaLight)) ToEighth (true, 2);									// if either square & gain more light than dark = to light square eighth
		} 
		else if (evol <= -21 && evol > -34) {																// devolve to light world eighth (if evol = -5)
			if (deltaDark < deltaLight) ToEighth (true, 0);														// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToEighth (false, 0);												// if lose more light than dark = to dark circle eighth
		}
	}
}
