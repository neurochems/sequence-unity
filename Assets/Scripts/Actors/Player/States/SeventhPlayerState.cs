using UnityEngine;
using System.Collections;

public class SeventhPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset collision timer

	public SeventhPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			Evol();																		// check evol logic
			Debug.Log("check player evol");
			checkEvol = false;															// reset check evol flag
		}

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.sc[0].enabled = true;														// enable trigger collider
			psp.stunned = false;															// reset stunned flag
			collisionTimer = 0f;															// reset collision timer
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		circle = psp.circle;																	// set current circle flag
		triangle = psp.triangle;																// set current triangle flag
		square = psp.square;																	// set current square flag

		if (canCollide) {																		// if collision allowed and other in dark world
			if (other.gameObject.CompareTag ("Zero")												// collide with zero
				|| other.gameObject.CompareTag ("First")											// collide with first
				|| other.gameObject.CompareTag ("Second")											// collide with second
				|| other.gameObject.CompareTag ("Third")											// collide with third
				|| other.gameObject.CompareTag ("Fourth")											// collide with fourth
				|| other.gameObject.CompareTag ("Fifth")											// collide with fifth
				|| other.gameObject.CompareTag ("Sixth")											// collide with sixth
				|| other.gameObject.CompareTag ("Seventh")) {										// collide with seventh
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern
				if (psp.lightworld == pspOther.inLightworld) {											// if player and particle in same world
					canCollide = false;																		// reset has collided trigger
					psp.sc[0].enabled = false;																// disable trigger collider
					psp.stunned = true;                                                                     // stun for duration
					if (pspOther.evolC == 0f) {																// if other = 0
						psp.AddLight (0.5f);																	// add 0.5 light
					}
					else if (pspOther.evolC > 0f) {															// if other > 0
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);							// add dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);                      // add light of other
					}
					else if (pspOther.evolC < 0f) {															// if other < 0
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);					// add positive dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);					// add positive light of other
					}
					checkEvol = true;																		// set check evol flag
				}
			} 
			else if (other.gameObject.CompareTag("Eighth")											// collide with eighth
				|| other.gameObject.CompareTag("Ninth"))											// collide with ninth
			{	
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern
				if (psp.lightworld == pspOther.inLightworld) {											// if player and particle in same world
					canCollide = false;																		// reset has collided trigger
					psp.sc[0].enabled = false;																// disable trigger collider
					psp.stunned = true;                                                                     // set stunned flag
					if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);							// subtract other dark
					if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);                      // subtract other light
					checkEvol = true;																		// set check evol flag
				}
			}
		}
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(7, 0, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;							// flag transition in delegate
		psp.currentState = psp.zeroState;											// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(7, 1, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;							// flag transition in delegate
		psp.currentState = psp.firstState;											// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(7, 2, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;						// flag transition in delegate
		psp.currentState = psp.secondState;											// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(7, 3, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;							// flag transition in delegate
		psp.currentState = psp.thirdState;											// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(7, 4, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;						// flag transition in delegate
		psp.currentState = psp.fourthState;											// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(7, 5, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;							// flag transition in delegate
		psp.currentState = psp.fifthState;											// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(7, 6, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.sixthState;											// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(7, 7, isLight, toLight, 0);								// trigger transition effects
	}

	public void ToEighth(bool toLight, int shape)
	{
		psp.TransitionTo(7, 8, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toEighth += psp.TransitionToEighth;						// flag transition in delegate
		psp.currentState = psp.eighthState;											// set to new state
	}

	public void ToNinth(bool toLight, int shape)
	{
		psp.TransitionTo(6, 9, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;						// flag transition in delegate
		psp.currentState = psp.ninthState;											// set to new state
	}

	public void Evol()
	{
		evol = psp.evol;																					// local evol check			
		isLight = psp.isLight;																				// update light value
		deltaDark = psp.deltaDark;																			// local dark check
		deltaLight = psp.deltaLight;																		// local light check

		if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;											// if to light world (if evol < 0), set to light world trigger
		else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;										// if to dark world (evol >= 0), set dark world flag
	
        // zero
			// to/in either world
		if (evol == 0) ToZero (true);																		// to dark world light zero / from either state
        
		// half zero
			// to/in either world
		if ((evol == 0.5f) || (evol == -0.5f)) {															// to half zero
			if (deltaDark > deltaLight)	ToZero (false);															// if lose more light than dark = to dark zero
			else if (deltaDark <= deltaLight) ToZero(true);														// if gain more light than dark = to light zero (no change)
		} 
        
		// first
			// to/in either world
		if ((evol == 1f) || (evol == -1f)) {														    	// to first
			if (deltaDark > deltaLight) ToFirst (false);														// if lose more light than dark = to dark first
			else if (deltaDark <= deltaLight) ToFirst (true);													// if lose more dark than light = to light first
		} 
        
		// second
			// to/in either world
		if ((evol == 1.5f) || (evol == -1.5f)) {												    		// to second
			if (deltaDark > deltaLight) ToSecond (false);														// if lose more light than dark = to dark second
			else if (deltaDark <= deltaLight) ToSecond (true);													// if lose more dark than light = to light second
		} 
        
		// third
			// to/in either world
		if (((evol >= 2f) && (evol < 3f)) || ((evol <= -2f) && (evol > -3f))) {				        		// to third
			if (deltaDark <= deltaLight) ToThird (true);														// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird (false);													// if lose more light than dark = to dark third
		} 
        
		// fourth
			// in dark world
		if ((evol >= 3f) && (evol < 5f)) {															    	// to dark world fourth
			if (circle && (deltaDark > deltaLight)) ToFourth (false);											// if circle & lose more light than dark = to dark fourth
			else if (circle && (deltaDark <= deltaLight)) ToFourth (false);										// if circle & lose more dark than light = to dark fourth
			else if (triangle && (deltaDark > deltaLight)) ToFourth (true);										// if triangle & lose more light than dark = to light fourth
			else if (triangle && (deltaDark <= deltaLight)) ToFourth (true);									// if triangle & lose more dark than light = to light fourth
			else if (square && (deltaDark > deltaLight)) ToFourth (true);										// if square & lose more light than dark = to light fourth
			else if (square && (deltaDark <= deltaLight)) ToFourth (true);										// if square & lose more dark than light = to light fourth
		}
			// to/in light world
		else if ((evol <= -3f) && (evol > -5f)) {															// to light world fourth
			if (deltaDark > deltaLight) ToFourth (false);														// if lose more light than dark = to dark fourth
			else if (deltaDark <= deltaLight) ToFourth (true);													// if lose more dark than light = to light fourth
		} 
        
		// fifth
			// in dark world
		if ((evol >= 5f) && (evol < 8f)) {															    	// to dark world fifth
			if (circle && (deltaDark > deltaLight)) ToFifth (false, 0);											// if circle & lose more light than dark = to dark circle fifth
			else if (circle && (deltaDark <= deltaLight)) ToFifth (true, 0);									// if circle & lose more dark than light = to light circle fifth
			else if (triangle && (deltaDark > deltaLight)) ToFifth (true, 1);									// if triangle & lose more light than dark = to triangle fifth
			else if (triangle && (deltaDark <= deltaLight)) ToFifth (true, 1);									// if triangle & lose more dark than light = to triangle fifth
			else if (square && (deltaDark > deltaLight)) ToFifth (true, 2);										// if square & lose more light than dark = to square fifth
			else if (square && (deltaDark <= deltaLight)) ToFifth (true, 2);									// if square & lose more dark than light = to square fifth
		}
			// to/in light world
		else if ((evol <= -5f) && (evol > -8f)) {															// to light world fifth
			if (deltaDark > deltaLight) ToFifth (false, 0);														// if lose more light than dark = to dark circle fifth
			else if (deltaDark <= deltaLight) ToFifth (true, 0);												// if lose more dark than light = to light circle fifth
		} 
       
		// sixth
			// in dark world
		if ((evol >= 8f) && (evol < 13f)) {														        	// to dark world sixth
			if (circle && (deltaDark > deltaLight)) ToSixth (false, 0);											// if circle & lose more light than dark = to dark circle sixth
			else if (circle && (deltaDark <= deltaLight)) ToSixth (true, 0);									// if circle & lose more dark than light = to light circle sixth
			else if (triangle && (deltaDark > deltaLight)) ToSixth (false, 1);									// if triangle & lose more light than dark = to triangle sixth
			else if (triangle && (deltaDark <= deltaLight)) ToSixth (false, 1);									// if triangle & lose more dark than light = to triangle sixth
			else if (square && (deltaDark > deltaLight)) ToSixth (false, 2);									// if square & lose more light than dark = to square sixth
			else if (square && (deltaDark <= deltaLight)) ToSixth (false, 2);									// if square & lose more dark than light = to square sixth
		}
			// to/in light world
		else if ((evol <= -8f) && (evol > -13f)) {															// to light world sixth
			if (deltaDark > deltaLight) ToSixth (false, 0);														// if lose more light than dark = to dark circle sixth
			else if (deltaDark <= deltaLight) ToSixth (true, 0);												// if lose more dark than light = to light circle sixth
		} 

		// seventh
			// to light world
		if ((evol <= -13f) && (evol > -21f)) {																// to light world seventh
			if (deltaDark > deltaLight) ToSeventh (false, 0);													// if lose more light than dark = to dark circle seventh
			else if (deltaDark <= deltaLight) ToSeventh (true, 0);												// if lose more dark than light = to light circle seventh
		}

        // eighth
			// in dark world
		if ((evol >= 21) && (evol < 34f)) {																	// to dark world eighth
			if (circle && (deltaDark > deltaLight)) ToEighth (false, 0);										// if circle & gain more dark than light = to dark circle eighth
			else if (circle && (deltaDark <= deltaLight)) ToEighth (true, 0);									// if circle & gain more light than dark = to light circle eighth
			else if (triangle && (deltaDark > deltaLight)) ToEighth (false, 1);									// if triangle & gain more dark than light = to dark triangle eighth
			else if (triangle && (deltaDark <= deltaLight)) ToEighth (true, 1);									// if triangle & gain more light than dark = to light triangle eighth
			else if (square && (deltaDark > deltaLight)) ToEighth (false, 2);									// if square & gain more dark than light = to dark square eighth
			else if (square && (deltaDark <= deltaLight)) ToEighth (true, 2);									// if square & gain more light than dark = to light square eighth
		}
			// to/in light world
		else if ((evol <= -21) && (evol > -34)) {															// to light world eighth
			if (deltaDark > deltaLight) ToEighth (false, 0);													// if lose more light than dark = to dark circle eighth
			else if (deltaDark <= deltaLight) ToEighth (true, 0);												// if lose more dark than light = to light circle eighth
		}

		// ninth
			// in dark world
		if ((evol >= 34) && (evol < 55f)) {																   	// to dark world ninth
			if (circle && (deltaDark > deltaLight)) ToNinth (false, 0);											// if circle & gain more dark than light = to dark circle ninth
			else if (circle && (deltaDark <= deltaLight)) ToNinth (true, 0);									// if circle & gain more light than dark = to light circle ninth
			else if (triangle && (deltaDark > deltaLight)) ToNinth (false, 1);									// if triangle & gain more dark than light = to dark triangle ninth
			else if (triangle && (deltaDark <= deltaLight)) ToNinth (true, 1);									// if triangle & gain more light than dark = to light triangle ninth
			else if (square && (deltaDark > deltaLight)) ToNinth (false, 2);									// if square & gain more dark than light = to dark square ninth
			else if (square && (deltaDark <= deltaLight)) ToNinth (true, 2);									// if square & gain more light than dark = to light square ninth
		} 
			// to/in light world
		else if ((evol >= -34f) && (evol < -55f)) {															// to light world ninth
			if (deltaDark > deltaLight) ToNinth(false, 0);														// if lose more light than dark = to dark circle ninth
			else if (deltaDark <= deltaLight) ToNinth(true, 0);													// if lose more dark than light = to light circle ninth
		}

	}
}
