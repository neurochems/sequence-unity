using UnityEngine;
using System.Collections;

public class FirstPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs

	private bool checkEvol;																// check evol flag

	private bool canCollide = false, takeHit = false;									// can collide flag (init false to begin stunned), take hit flag
	private float collisionTimer, takeHitTimer;											// reset collision timer, take hit timer

	public FirstPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			Debug.Log("player first: check evol");
			Evol();																		// check evol logic
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
		// take hit flag timer
		if (takeHit) takeHitTimer += Time.deltaTime;									// start timer
		if (takeHitTimer >= 0.2f) {														// if timer is up
			psp.stunned = true;															// set stunned flag
			takeHit = false;															// reset take hit trigger
			takeHitTimer = 0f;															// reset take hit timer
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
				if (!pspOther.stunned && psp.lightworld == pspOther.inLightworld) {						// if player and not stunned particle in same world
					canCollide = false;																		// reset has collided trigger
					psp.sc[0].enabled = false;																// disable trigger collider
					takeHit = true;																			// set stunned flag
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
				if (!pspOther.stunned && psp.lightworld == pspOther.inLightworld) {						// if player and not stunned particle in same world
					canCollide = false;																		// reset has collided trigger
					psp.sc[0].enabled = false;																// disable trigger collider
					takeHit = true;																			// set stunned flag
					Debug.Log ("player first +else: sub evol");
					if (pspOther.evolC > 0f) {																// other > 0
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);							// sub other dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);						// sub other light
					}
					else if (pspOther.evolC < 0f) {															// other < 0
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);					// sub other negated dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);					// sub other negated light
					}
					checkEvol = true;																		// set check evol flag
				}
			}
		}				
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(1, 0, isLight, toLight, 0, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;							// flag transition in delegate
		psp.currentState = psp.zeroState;											// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(1, 1, isLight, toLight, 0, 0);								// trigger transition effects
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(1, 2, isLight, toLight, 0, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;						// flag transition in delegate
		psp.currentState = psp.secondState;											// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(1, 3, isLight, toLight, 0, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;							// flag transition in delegate
		psp.currentState = psp.thirdState;											// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(1, 4, isLight, toLight, 0, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;						// flag transition in delegate
		psp.currentState = psp.fourthState;											// set to new state
	}

	public void ToFifth(bool toLight, int toShape)
	{
		psp.TransitionTo(1, 5, isLight, toLight, 0, toShape);						// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;							// flag transition in delegate
		psp.currentState = psp.fifthState;											// set to new state
	}

	public void ToSixth(bool toLight, int toShape)
	{
		psp.TransitionTo(1, 6, isLight, toLight, 0, toShape);						// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.sixthState;											// set to new state
	}

	public void ToSeventh(bool toLight, int toShape)
	{
		psp.TransitionTo(1, 7, isLight, toLight, 0, toShape);						// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new state
	}

	public void ToEighth(bool toLight, int toShape)
	{
		psp.TransitionTo(1, 8, isLight, toLight, 0, toShape);						// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.eighthState;											// set to new state
	}

	public void ToNinth(bool toLight, int toShape)
	{
		psp.TransitionTo(1, 9, isLight, toLight, 0, toShape);						// trigger transition effects
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
		if (evol == 0f) ToZero (true);																		// to dark world light zero / from either world

		// half zero
			// to/in either world
		if ((evol == 0.5f) || (evol == -0.5f)) {															// to zero
			if (deltaDark > deltaLight) ToZero(false);															// if gain more dark than light = to dark zero
			else if (deltaDark <= deltaLight) ToZero(true);														// if gain more light than dark = to light zero
		}

		// first
			// to light world
		if (evol == -1.0f) {																				// to light world first
			if (deltaDark > deltaLight) ToFirst(false);															// if lose more dark than light = to dark second
			else if (deltaDark <= deltaLight) ToFirst(true);													// if lose more dark than light = to light first
		}
        
		// second
			// to/in either world
		if ((evol >= 1.5f) || (evol == -1.5f)) {															// to second
			if (deltaDark > deltaLight) ToSecond(false);														// if gain more dark than light = to dark second
			else if (deltaDark <= deltaLight) ToSecond(true);													// if gain more light than dark = to light second
		}
        
		// third
			// to/in either world
		if (((evol >= 2f) && (evol < 3f)) || ((evol <= -2f) && (evol > -3f))) {								// to third
			if (deltaDark > deltaLight) ToThird(false);															// if gain more dark than light = to dark third
			else if (deltaDark <= deltaLight) ToThird(true);													// if gain more dark than light = to light third
		}
        
		// fourth
			// to/in light world
        if ((evol <= -3f) && (evol > -5f)) {													    		// to light world fourth
			if (deltaDark > deltaLight) ToFourth(false);														// if lose more dark than light = to dark fourth
			else if (deltaDark <= deltaLight) ToFourth(true);													// if lose more dark than light = to light fourth
		}
        
		// fifth
			// to/in light world
        if ((evol <= -5f) && (evol > -8f)) {													    		// to light world fifth
			if (deltaDark > deltaLight) ToFifth(false, 0);														// if lose more light than dark = to dark circle fifth
			else if (deltaDark <= deltaLight) ToFifth(true, 0);													// if lose more dark than light = to light circle fifth
		}
        
		// sixth
			// to/in light world
        if ((evol <= -8f) && (evol > -13f)) {													    		// to light world sixth
			if (deltaDark > deltaLight) ToSixth(false, 0);														// if lose more dark than light = to dark circle sixth
			else if (deltaDark <= deltaLight) ToSixth(true, 0);													// if lose more dark than light = to light circle sixth
		}
        
		// seventh
			// to/in light world
		if ((evol <= -13f) && (evol > -21f)) {															    // to light world seventh
			if (deltaDark > deltaLight) ToSeventh(false, 0);													// if lose more dark than light = to dark circle seventh
			else if (deltaDark <= deltaLight) ToSeventh(true, 0);												// if lose more dark than light = to light circle seventh
		}
        
		// eighth
			// to/in light world
		if ((evol <= -21f) && (evol > -34f)) {																// to light world eighth
			if (deltaDark > deltaLight) ToEighth(false, 0);														// if lose more dark than light = to dark circle eighth
			else if (deltaDark <= deltaLight) ToEighth(true, 0);												// if lose more dark than light = to light circle eighth
		}

		// ninth
			// to/in light world
		if ((evol <= -34f) && (evol > -55f)) {															    // to light world ninth
			if (deltaDark > deltaLight) ToNinth(false, 0);														// if lose more light than dark = to dark circle ninth
			else if (deltaDark <= deltaLight) ToNinth(true, 0);													// if lose more dark than light = to light circle ninth
		}
	}
}
