using UnityEngine;
using System.Collections;

public class FirstPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs

	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset collision timer

	public FirstPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			Evol();																		// check evol logic
			Debug.Log("player first: check evol");
			checkEvol = false;															// reset check evol flag
		}

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer up
			canCollide = true;																// set collision ability
			psp.sc[0].enabled = true;														// disable trigger collider
			psp.stunned = false;															// reset stunned flag
			collisionTimer = 0f;															// reset collision timer
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("World")) Debug.Log ("first player collision");

		if (canCollide) {																		// if collision allowed and other in dark world
			if (other.gameObject.CompareTag ("Zero")												// collide with zero
				|| other.gameObject.CompareTag ("First")) {											// collide with first	
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern
				if (!pspOther.inLightworld) {															// if other in dark world
					canCollide = false;																		// reset has collided trigger
					psp.sc[0].enabled = false;																// disable trigger collider
					psp.stunned = true;                                                     		        // stun for duration
					if (pspOther.evolC == 0f) {																// if other = 0
						psp.AddLight (0.5f);																	// add 0.5 light
					}
					else if (pspOther.evolC > 0f) {															// if other > 0
						//Debug.Log ("player first + 0/1>0: add evol");
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);							// add dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);            		    // add light of other
						Debug.Log ("player first + 0/1>0: add " + pspOther.lightEvolC + " light");
					}
					else if (pspOther.evolC < 0f) {															// if other < 0
						Debug.Log ("player first + 0/1<0: add evol");
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);						// add positive dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);					// add positive light of other
					}
					checkEvol = true;																		// set check evol flag
				}
			}
			else if (other.gameObject.CompareTag("Second")											// collide with second
				|| other.gameObject.CompareTag("Third")												// collide with third
				|| other.gameObject.CompareTag("Fourth")											// collide with fourth
				|| other.gameObject.CompareTag("Fifth")												// collide with fifth
				|| other.gameObject.CompareTag("Sixth")												// collide with sixth
				|| other.gameObject.CompareTag("Seventh")											// collide with seventh
				|| other.gameObject.CompareTag("Eighth")											// collide with eighth
				|| other.gameObject.CompareTag("Ninth"))											// collide with ninth
			{	
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern
				if (!pspOther.inLightworld) {															// if other in dark world
					canCollide = false;																		// reset has collided trigger
					psp.sc[0].enabled = false;																// disable trigger collider
					psp.stunned = true;																		// stun for duration
					Debug.Log ("player first +else: sub evol");
					psp.SubDark (pspOther.darkEvolC);														// subtract other dark
					psp.SubLight (pspOther.lightEvolC);      		                                        // subtract other light
					checkEvol = true;																		// set check evol flag
				}
			}
		}				
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(1, 0, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		Debug.Log ("player first to zero");
		//psp.SpawnZero(1);														// spawn 1 zero
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToFirst(bool toLight)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(1, 2, isLight, toLight, 0);								// trigger transition effects
		Debug.Log ("player first to second");
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(1, 3, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;						// flag transition in delegate
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(1, 4, isLight, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(1, 5, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(1, 6, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(1, 7, isLight, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new state
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
		if (evol == 0f) {																				    // to zero (if evol = 0)
			ToZero (true);																						// to light zero
		}
        // half zero
        if (evol == 0.5f) {																	        		// devolve to dark world zero (if evol = 0.5)
			if (deltaDark > deltaLight) ToZero(false);															// if gain more dark than light = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);														// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f) {																			// devolve to light world zero (if evol = -0.5)
			if (deltaDark < deltaLight) ToZero(true);															// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToZero(false);														// if lose more dark than light = to dark zero
		}
        // second
        if (evol >= 1.5f) {																	        		// evolve to dark world second (if evol = 1.5)
			if (deltaDark > deltaLight) ToSecond(false);														// if gain more dark than light = to dark second
			else if (deltaDark <= deltaLight) ToSecond(true);													// if gain more light than dark = to light second
		}
		else if (evol == -1.5f) {																			// devolve to light world second (if evol = -1.5)
			if (deltaDark <= deltaLight) ToSecond(true);															// if lose more dark than light = to light second
			else if (deltaDark > deltaLight) ToSecond(false);													// if lose more dark than light = to dark second
		}
        // third
		if (evol == 2f) {															    					// evolve to light world third (if evol = 2)
			if (deltaDark > deltaLight) ToThird(false);															// if gain more dark than light = to dark third
			else if (deltaDark <= deltaLight) ToThird(true);														// if gain more dark than light = to light third
		}
		else if (evol >= -2f && evol < -3f) {															    // devolve to light world third (if evol = -2)
			if (deltaDark <= deltaLight) ToThird(true);															// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);													// if lose more dark than light = to dark third
		}
        // fourth
        if (evol >= -3f && evol < -5f) {														    		// devolve to light world fourth (if evol = -3)
			if (deltaDark <= deltaLight) ToFourth(true);															// if lose more dark than light = to light fourth
			else if (deltaDark > deltaLight) ToFourth(false);													// if lose more dark than light = to dark fourth
		}
        // fifth
        if (evol >= -5f && evol < -8f) {														    		// devolve to light world fifth (if evol = -8)
			if (deltaDark <= deltaLight) ToFifth(true, 0);														// if lose more dark than light = to light circle fifth
			else if (deltaDark > deltaLight) ToFifth(false, 0);													// if lose more light than dark = to dark circle fifth
		}
        // sixth
        if (evol >= -8f && evol < -13f) {														    		// devolve to light world sixth (if evol = -8)
			if (deltaDark <= deltaLight) ToSixth(true, 0);														// if lose more dark than light = to light circle sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);													// if lose more dark than light = to dark circle sixth
		}
        // seventh
		if (evol >= -13f && evol < -21f) {																    // devolve to light world seventh (if evol = -13)
			if (deltaDark <= deltaLight) ToSeventh(true, 0);														// if lose more dark than light = to light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);												// if lose more dark than light = to dark circle seventh
		}
        // eighth
		/*if (evol >= -21f && evol < -34f) {															// devolve to light world eighth (if evol = -21)
			if (deltaDark < deltaLight) ToEighth(true, 0);														// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToEighth(false, 0);												// if lose more dark than light = to dark circle eighth
		}*/
	}
}
