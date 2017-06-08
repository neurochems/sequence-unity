using UnityEngine;
using System.Collections;

public class FifthPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset collision timer

	public FifthPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			Evol();																		// check evol logic
			Debug.Log("player fifth: check evol");
			Debug.Log("player circle: " + circle);
			Debug.Log("player triangle: " + triangle);
			Debug.Log("player square: " + square);
			checkEvol = false;															// reset check evol flag
		}

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.sc[0].enabled = true;														// enable trigger collider
			psp.stunned = false;															// reset stunned trigger
			collisionTimer = 0f;															// reset collision timer
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (canCollide) {																		// if collision allowed
			if (other.gameObject.CompareTag ("Zero")												// collide with zero
				|| other.gameObject.CompareTag ("First")											// collide with first
				|| other.gameObject.CompareTag ("Second")											// collide with second
				|| other.gameObject.CompareTag ("Third")											// collide with third
				|| other.gameObject.CompareTag ("Fourth")											// collide with fourth
				|| other.gameObject.CompareTag ("Fifth")) {											// collide with fifth
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern
				canCollide = false;																		// reset has collided trigger
				psp.sc[0].enabled = false;																// disable trigger collider
				psp.stunned = true;                                                                     // set stunned flag
				if (pspOther.evolC == 0f) {																// if other = 0
					Debug.Log ("player fifth + other=0: add evol");
					psp.AddLight (0.5f);																	// add 0.5 light
				}
				else if (pspOther.evolC > 0f) {															// if other > 0
					Debug.Log ("player fifth + other>0: add evol");
					if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);							// add dark of other
					if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);                      // add light of other
				}
				else if (pspOther.evolC < 0f) {															// if other < 0
					Debug.Log ("player fourth + other<0: add evol");
					if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);					// add positive dark of other
					if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);					// add positive light of other
				}
				checkEvol = true;																		// set check evol flag
			} 
			else if (other.gameObject.CompareTag("Sixth")											// collide with sixth
				|| other.gameObject.CompareTag("Seventh")											// collide with seventh
				|| other.gameObject.CompareTag("Eighth")											// collide with eighth
				|| other.gameObject.CompareTag("Ninth"))											// collide with ninth
			{	
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern
				canCollide = false;																		// reset has collided trigger
				psp.sc[0].enabled = false;																// disable trigger collider
				psp.stunned = true;																		// set stunned flag
				if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);							// subtract other dark
				if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);						// subtract other light
				checkEvol = true;																		// set check evol flag
			}
		}
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

	public void ToFifth(bool light, int shape)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(5, 6, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
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
			if (deltaDark > deltaLight) ToZero(false);															// if lose more light than dark = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);													// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f) {																			// devolve to light world zero (if evol = -0.5)
			if (deltaDark <= deltaLight) ToZero(true);															// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToZero(false);														// if lose more light than dark = to dark zero
		} 
        // first
		if ((evol == 1f) || (evol == -1f)) {													    		// devolve to dark or light world first (if evol == 1)
			if (deltaDark <= deltaLight) ToFirst(true);															// if lose more dark than light = to light first
			else if (deltaDark > deltaLight) ToFirst(false);													// if lose more light than dark = to dark first
		}
        // second
		if ((evol == 1.5f) || (evol == -1.5f)) {												    		// devolve to dark or light world second (if evol == 1.5)
			if (deltaDark <= deltaLight) ToSecond(true);															// if lose more dark than light = to light second
			else if (deltaDark > deltaLight) ToSecond(false);													// if lose more light than dark = to dark second
		}
        // third
		if ((evol >= 2f && evol < 3f) || (evol <= -2f && evol > -3f)) {							        	// devolve to dark or light world third (if evol = 2)
			if (deltaDark <= deltaLight) ToThird(true);															// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);													// if lose more light than dark = to dark third
		}
        // fourth
		if (evol >= 3f && evol < 5f) {															    		// devolve to dark world fourth (if evol >= 3 and < 5)
			if (circle && (deltaDark < deltaLight)) ToFourth(false);											// if either circle & lose more dark than light = to dark fourth
			else if (circle && (deltaDark >= deltaLight)) ToFourth(false);										// if either circle & lose more light than dark = to dark fourth
			else if (triangle && (deltaDark < deltaLight)) ToFourth(true);										// if triangle & lose more dark than light = to light fourth
			else if (triangle && (deltaDark > deltaLight)) ToFourth(true);										// if triangle & lose more light than dark = to light fourth
			else if (square && (deltaDark < deltaLight)) ToFourth(true);										// if square & lose more dark than light = to light fourth
			else if (square && (deltaDark > deltaLight)) ToFourth(true);										// if square & lose more light than dark = to light fourth
		}
		else if (evol <= -3f && evol > -5f) {																// devolve to light world fourth (if evol == -3)
			if (deltaDark < deltaLight) ToFourth(true);															// if lose more dark than light = to light fourth
			else if (deltaDark > deltaLight) ToFourth(false);													// if lose more light than dark = to dark fourth
		}
        // sixth
		if (evol >= 8f && evol < 13f) {															        	// evolve to dark world sixth (if evol >= 8 and < 13)
			if (circle && (deltaDark < deltaLight)) ToSixth(false, 0);											// if either circle & gain more dark than light = to dark circle sixth
			else if (circle && (deltaDark >= deltaLight)) ToSixth(true, 0);										// if either circle & gain more light than dark = to light circle sixth
			else if (triangle && (deltaDark < deltaLight)) ToSixth(false, 1);									// if triangle & gain more dark than light = to dark triangle sixth
			else if (triangle && (deltaDark > deltaLight)) ToSixth(false, 1);									// if triangle & gain more light than dark = to dark triangle sixth
			else if (square && (deltaDark < deltaLight)) ToSixth(false, 2);										// if square & gain more dark than light = to dark square sixth
			else if (square && (deltaDark > deltaLight)) ToSixth(false, 2);										// if square & gain more light than dark = to dark square sixth
		}
		else if (evol <= -8f && evol > -13f) {																// devolve to light world sixth (if evol = -8)
			if (deltaDark <= deltaLight) ToSixth(true, 0);														// if lose more dark than light = to light circle sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);													// if lose more dark than light = to dark circle sixth
		}
        // seventh
		if (evol <= -13f && evol > -21f) {															    	// devolve to light world seventh (if evol = -13)
			if (deltaDark <= deltaLight) ToSeventh(true, 0);													// if lose more dark than light = to light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);												// if lose more dark than light = to dark circle seventh
		}
        // eighth
		/*if (evol <= -21f && evol > -34f) {														    	// devolve to light world eighth (if evol = -21)
			if (deltaDark < deltaLight) ToEighth(true, 0);														// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToEighth(false, 0);												// if lose more dark than light = to dark circle eighth
		}*/
	}
}
