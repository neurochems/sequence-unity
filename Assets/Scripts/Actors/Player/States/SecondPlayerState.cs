using UnityEngine;
using System.Collections;

public class SecondPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset collision timer

	public SecondPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			Evol();																		// check evol logic
			Debug.Log("player second: check evol");
			checkEvol = false;															// reset check evol flag
		}

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.sc[0].enabled = true;														// enable trigger collider
			psp.stunned = true;																// reset stunned flag
			collisionTimer = 0f;															// reset collision timer
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("World")) Debug.Log ("second player collision");

		if (canCollide) {																	// if collision allowed and other in dark world
			if (other.gameObject.CompareTag ("Zero") 											// collide with Zero
				|| other.gameObject.CompareTag ("First") 										// collide with first
				|| other.gameObject.CompareTag ("Second")) {									// collide with second
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (psp.lightworld == pspOther.inLightworld) {										// if player and particle in same world
					canCollide = false;																	// reset has collided trigger
					psp.sc[0].enabled = false;															// disable trigger collider
					psp.stunned = true;                                                                 // set stunned flag
					if (pspOther.evolC == 0f) {															// if other = 0
						Debug.Log ("player second + 0/1/2=0: add evol");
						psp.AddLight (0.5f);																// add 0.5 light
					}
					else if (pspOther.evolC > 0f) {														// if other > 0
						Debug.Log ("player second + 0/1/2>0: add evol");
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);						// add dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);                	// add light of other
					}
					else if (pspOther.evolC < 0f) {														// if other < 0
						Debug.Log ("player second + 0/1/2<0: add evol");
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);				// add positive dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);				// add positive light of other
					}
					checkEvol = true;																	// set check evol flag
				}
			} 
			else if (other.gameObject.CompareTag("Third")										// collide with third
				|| other.gameObject.CompareTag("Fourth")										// collide with fourth
				|| other.gameObject.CompareTag("Fifth")											// collide with fifth
				|| other.gameObject.CompareTag("Sixth")											// collide with sixth
				|| other.gameObject.CompareTag("Seventh")										// collide with seventh
				|| other.gameObject.CompareTag("Eighth")										// collide with eighth
				|| other.gameObject.CompareTag("Ninth"))										// collide with ninth
			{
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (psp.lightworld == pspOther.inLightworld) {										// if player and particle in same world
					canCollide = false;																	// reset has collided trigger
					psp.sc[0].enabled = false;															// disable trigger collider
					psp.stunned = true;                                                                 // set stunned flag
					if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);						// subtract other dark
					if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);					// subtract other light
					checkEvol = true;																	// set check evol flag
				}
			}
		}
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(2, 0, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;							// flag transition in delegate
		psp.SpawnZero(2);															// spawn 2 Zeros
		psp.currentState = psp.zeroState;											// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(2, 1, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;							// flag transition in delegate
		psp.SpawnZero(1);															// spawn 1 Zero
		psp.currentState = psp.firstState;											// set to new state
	}

	public void ToSecond(bool toLight)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(2, 3, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;							// flag transition in delegate
		psp.currentState = psp.thirdState;											// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(2, 4, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;						// flag transition in delegate
		psp.currentState = psp.fourthState;											// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(2, 5, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;							// flag transition in delegate
		psp.currentState = psp.fifthState;											// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(2, 6, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.sixthState;											// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(2, 7, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new state
	}

	public void ToEighth(bool toLight, int shape)
	{
		psp.TransitionTo(2, 8, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.eighthState;											// set to new state
	}

	public void ToNinth(bool toLight, int shape)
	{
		psp.TransitionTo(2, 9, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;						// flag transition in delegate
		psp.currentState = psp.ninthState;											// set to new state
	}

	public void Evol() 
	{
		evol = psp.evol;																					// local evol check			
		isLight = psp.isLight;																					// update light value
		deltaDark = psp.deltaDark;																			// local dark check
		deltaLight = psp.deltaLight;																		// local light check

		if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;											// if to light world (if evol < 0), set to light world trigger
		else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;										// if to dark world (evol >= 0), set dark world flag

        // zero
		if (evol == 0) {																				    // to zero (if evol = 0)
			ToZero (true);																						// to zero state
		}
        // half zero
		if (evol == 0.5f) {																			        // devolve to dark world dark zero (if evol = 0.5)
			if (deltaDark > deltaLight) ToZero(false);															// if lose more light than dark = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);													// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f) {																			// devolve to light world zero (if evol = -0.5)
			if (deltaDark <= deltaLight) ToZero(true);															// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToZero(false);														// if lose more light than dark = to dark zero
		} 
        // first
		if ((evol == 1f) || (evol == -1f)) {															    // devolve to dark or light world first (if evol == 1)
			if (deltaDark <= deltaLight) ToFirst(true);															// if lose more dark than light = to light first
			else if (deltaDark > deltaLight) ToFirst(false);													// if lose more light than dark = to dark first
		}
        // third
		if (evol >= 2f && evol < 3f) {																		// evolve to dark world third (if evol = 2)
			if (deltaDark > deltaLight) ToThird(false);															// if gain more dark than light = to dark third
			else if (deltaDark <= deltaLight) ToThird(true);													// if gain more light than dark = to light third
		}
		else if (evol >= -2f && evol < -3f) {																// devolve to light world third (if evol = -2)
			if (deltaDark <= deltaLight) ToThird(true);															// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);													// if lose more light than dark = to dark third
		}
        // fourth
		if (evol >= 3f && evol < 5f) {																		// evolve to dark world fourth (if evol = 3)
			if (deltaDark > deltaLight) ToFourth(false);														// if gain more dark than light = to dark fourth
			else if (deltaDark <= deltaLight) ToFourth(true);													// if gain more light than dark = to light fourth
		}
		if (evol >= -3f && evol < -5f) {															    	// devolve to light world fourth (if evol = -3)
			if (deltaDark <= deltaLight) ToFourth(true);														// if lose more dark than light = to light fourth
			else if (deltaDark > deltaLight) ToFourth(false);													// if lose more light than dark = to dark fourth
		}
        // fifth
		if (evol >= -5f && evol < -8f) {															    	// devolve to light world fifth (if evol = -5)
			if (deltaDark <= deltaLight) ToFifth(true, 0);														// if lose more dark than light = to light circle fifth
			else if (deltaDark > deltaLight) ToFifth(false, 0);													// if lose more light than dark = to dark circle fifth
		}
        // sixth
		if (evol >= -8f && evol < -13f) {																    // devolve to light world sixth (if evol = -8)
			if (deltaDark <= deltaLight) ToSixth(true, 0);														// if lose more dark than light = to light circle sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);													// if lose more light than dark = to dark circle sixth
		}
        // seventh
		if (evol >= -13f && evol < -21f) {															    	// devolve to light world seventh (if evol = -13)
			if (deltaDark <= deltaLight) ToSeventh(true, 0);													// if lose more dark than light = to light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);												// if lose more light than dark = to dark circle seventh
		}
        // eighth
		if (evol >= -21f && evol < -34f) {																	// devolve to light world eighth (if evol = -21)
			if (deltaDark <= deltaLight) ToEighth(true, 0);														// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToEighth(false, 0);												// if lose more light than dark = to dark circle eighth
		}
		// ninth
		if (evol >= -34f && evol < -55f) {																	// devolve to light world ninth (if evol == -34)
			if (deltaDark <= deltaLight) ToNinth(true, 0);														// if lose more dark than light = to light circle ninth
			else if (deltaDark > deltaLight) ToNinth(false, 0);													// if lose more light than dark = to dark circle ninth
		}
	}
}
