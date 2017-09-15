using UnityEngine;
using System.Collections;

public class ZeroParticleState : IParticleState 
{
	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight;																// 'is light' flag
	private bool lightworld, inLightworld;												// is light world ref, in light world ref
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol = false;														// check evol flag
	private bool init = true;															// is init flag

	private bool canCollide = false, takeHit = false;									// can collide flag (init false to begin stunned), take hit flag (delay flagging stunned to prevent collision logic conflicts)
	private float collisionTimer, takeHitTimer;											// reset collision timer, take hit timer

	public int die;																		// collision conflict resolution
	private bool rolling = false;														// is rolling flag

	// constructor
	public ZeroParticleState (ParticleStatePattern statePatternParticle)				
	{
		psp = statePatternParticle;														// attach state pattern to this state 
	}

	public void UpdateState()
	{
        // check evol
		if (checkEvol) {
			//Debug.Log("check particle evol");
			Evol();																		// check evol logic
			checkEvol = false;															// reset check evol flag
		}

		if (psp.psp.isInit && init) {														// if init
			Init();                                                         				// call init
			init = false;																	// reset is init flag
		}

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.sc[0].enabled = true;														// enable trigger collider
			psp.stunned = false;											// update stunned to false
			collisionTimer = 0f;															// reset collision timer
		}
		// take hit flag timer
		if (!takeHit) takeHitTimer += Time.deltaTime;									// start timer
		if (takeHitTimer >= 0.2f) {														// if timer is up
			psp.stunned = true;																// set stunned flag
			takeHit = false;																// reset take hit trigger
			takeHitTimer = 0f;																// reset take hit timer
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		//if (!other.gameObject.CompareTag("World")) Debug.Log ("zero particle collision");
		if (canCollide) {																		// if collision allowed and player is not stunned
			if (other.gameObject.CompareTag ("Player")) {											// if colide with collidable player
				PlayerStatePattern pspOther 
					= other.gameObject.GetComponent<PlayerStatePattern>();								// ref other ParticleStatePattern
				if (!pspOther.stunned && (pspOther.lightworld == psp.inLightworld)) {						// if particle and not stunned player in same world
					if (pspOther.evolC == 0f) {																// other = 0
						Debug.Log ("zero particle+player=0: sub evol");
						psp.SubLight (0.5f);																	// subtract 0.5 light
					}
					else if (pspOther.evolC > 0f) {															// other > 0
						Debug.Log ("zero particle+player>0: sub evol");
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);							// sub other dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);						// sub other light
					}
					else if (pspOther.evolC < 0f) {															// player evol is lower
						Debug.Log ("zero particle+player<0: sub evol");
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);					// sub other negated dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);					// sub other negated light
					}
					canCollide = false;																		// reset can collide trigger	
					psp.sc[0].enabled = false;																// disable trigger collider
					takeHit = true;																			// set take hit flag
					checkEvol = true;																		// set check evol flag
				}
				pspOther = null;																	// clear pspOther
			} 
			else if (other.gameObject.CompareTag ("Zero")) {									// if collide with zero
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (!pspOther.stunned && (pspOther.inLightworld == psp.inLightworld)) {				// if particle and not stunned particle in same world
					canCollide = false;																	// reset has collided trigger
					psp.sc[0].enabled = false;															// disable trigger collider
					//Debug.Log ("zero particle+zero: roll die");
					RollDie (pspOther);																	// roll die
					takeHit = true;																		// set take hit flag
					checkEvol = true;																	// set check evol flag
				}
				pspOther = null;																	// clear pspOther
			} 
			else if (other.gameObject.CompareTag("First") 									// collide with first
				|| other.gameObject.CompareTag("Second")									// collide with second
				|| other.gameObject.CompareTag("Third")								        // collide with third
				|| other.gameObject.CompareTag("Fourth")								    // collide with fourth
				|| other.gameObject.CompareTag("Fifth")										// collide with fifth
				|| other.gameObject.CompareTag("Sixth")									    // collide with sixth
				|| other.gameObject.CompareTag("Seventh")								    // collide with seventh
				|| other.gameObject.CompareTag("Eighth")								    // collide with eighth
				|| other.gameObject.CompareTag("Ninth"))								    // collide with ninth
			{									
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern
				if (!pspOther.stunned && (pspOther.inLightworld == psp.inLightworld)) {			// if particle and not stunned particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc[0].enabled = false;														// disable trigger collider
					//Debug.Log ("zero particle+else: sub evol");
					if (pspOther.evolC > 0f) {															// if other > 0
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);						// subtract other dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);					// subtract other light
					}
					else if (pspOther.evolC < 0f) {														// if other < 0
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);				// add other negative dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);				// add other negative light
					}
					takeHit = true;																		// set take hit flag
					checkEvol = true;																	// set check evol flag
				}
				pspOther = null;																	// clear pspOther
			}
		}
	}

	private void RollDie(ParticleStatePattern pspOther) {
		if (psp.die > pspOther.die) {														// if this die > other die
			//Debug.Log ("die roll: this > other");
			if (pspOther.evolC == 0) psp.AddLight (0.5f);										// if other = 0, add light
			else if (pspOther.evolC > 0f) {														// if other > 0
				if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);					// add other dark
				if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);				// add other light
			}
			else if (pspOther.evolC < 0f) {													// if other < 0
				if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);			// add other negative dark
				if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);			// add other negative light
			}
			psp.roll = true;																	// re-roll die
		}
		else if (psp.die < pspOther.die) {													// if this die < other die
			//Debug.Log ("die roll: this < other");
			if (pspOther.evolC == 0) psp.SubLight (0.5f);										// if other = 0, add light
			else if (pspOther.evolC > 0f) {														// if other > 0
				if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);						// sub other dark
				if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);					// sub other light
			}
			else if (pspOther.evolC < 0f) {														// if other < 0
				if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);				// sub other negative dark
				if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);				// sub other negative light
			}
			psp.roll = true;																	// re-roll die
		}
		else if (psp.die == pspOther.die) {													// if die are same
			psp.roll = true;																	// re-roll die
			// do nothing - cancelled out!
		}
	}

	public void ToOtherWorld(bool toLW, int tst8, bool tl, int ts)
	{
		if (tst8 == 1) psp.currentState = psp.firstState;		    			   			// set to first state
		else if (tst8 == 2) psp.currentState = psp.secondState;  							// set to second state
		else if (tst8 == 3) psp.currentState = psp.thirdState;							    // set to third state
		else if (tst8 == 4) psp.currentState = psp.fourthState;						    	// set to fourth state
		else if (tst8 == 5) psp.currentState = psp.fifthState;					    		// set to fifth state
		else if (tst8 == 6) psp.currentState = psp.sixthState;		    					// set to sixth state
		else if (tst8 == 7) psp.currentState = psp.seventhState;				    		// set to seventh state
		else if (tst8 == 8) psp.currentState = psp.eighthState;			    				// set to eighth state
		else if (tst8 == 9) psp.currentState = psp.ninthState;			    				// set to ninth state

		psp.ChangeWorld(toLW, 0, tst8, tl, 0, ts);					    							// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;				        			// flag transition in delegate
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0, 0);								        // trigger transition effects
	}

	public void ToHalfZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0, 0);										// trigger transition effects
		//ParticleStateEvents.toLightZero += psp.TransitionToLightZero;
	}

	public void ToFirst(bool toLight)
	{
		psp.currentState = psp.firstState;									        		// set to new state
		psp.TransitionTo(0, 1, isLight, toLight, 0, 0);								        // trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;				        			// flag transition in delegate
	}

	public void ToSecond(bool toLight)
	{
		psp.currentState = psp.secondState;							    			    	// set to new state
		psp.TransitionTo(0, 2, isLight, toLight, 0, 0);							        	// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;		    		    		// flag transition in delegate
	}

	public void ToThird(bool toLight)
	{
		psp.currentState = psp.thirdState;									           		// set to new state
		psp.TransitionTo(0, 3, isLight, toLight, 0, 0);					    			    // trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;			    		    		// flag transition in delegate
	}

	public void ToFourth(bool toLight)
	{
		psp.currentState = psp.fourthState;									      	    	// set to new state
		psp.TransitionTo(0, 4, isLight, toLight, 0, 0);					    			    // trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;		    	    			// flag transition in delegate
	}

	public void ToFifth(bool toLight, int toShape)
	{
		psp.currentState = psp.fifthState;									        		// set to new state
		psp.TransitionTo(0, 5, isLight, toLight, 0, toShape);				    			   // trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;			    	    			// flag transition in delegate
	}

	public void ToSixth(bool toLight, int toShape)
	{
		psp.currentState = psp.sixthState;								    	    		// set to new state
		psp.TransitionTo(0, 6, isLight, toLight, 0, toShape);				    		    // trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;			        				// flag transition in delegate
	}

	public void ToSeventh(bool toLight, int toShape)
	{
		psp.currentState = psp.seventhState;								    	 	   	// set to new state
		psp.TransitionTo(0, 7, isLight, toLight, 0, toShape);					    		   // trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;			        			// flag transition in delegate
	}

	public void ToEighth(bool toLight, int toShape)
	{
		psp.currentState = psp.eighthState;													// set to new state
		psp.TransitionTo(0, 8, isLight, toLight, 0, toShape);								// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;								// flag transition in delegate
	}

	public void ToNinth(bool toLight, int toShape)
	{
		psp.currentState = psp.ninthState;												// set to new state
		psp.TransitionTo(0, 9, isLight, toLight, 0, toShape);							// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;							// flag transition in delegate
	}

    public void Init()
    {
		evol = psp.evol;                                                                    // local evol check
		isLight = psp.isLight;																	// update light value

        if (evol == 0f) ToZero(true);               									    // init to light zero
		else if (evol == 0.5f) ToHalfZero(isLight);              							// init to half zero
		else if (evol == 1f) {																// if first
			if (isLight) ToFirst(true);               											// init to light first
			if (!isLight) ToFirst(false);															// init to dark first
		}
		else if (evol == 1.5f) {															// if second
			if (isLight) ToSecond(true);               											// init to light second
			if (!isLight) ToSecond(false);														// init to dark second
		}
        else if (evol == 2f) ToThird(true);               									// init to light third
        else if (evol == 3f) {																// init to light fourth
            int i = Random.Range(0, 1);                                                         // random 0 or 1
            if (i == 0) ToFourth(false);                                                        // to dark fourth
            else ToFourth(true);                                                                // to light fourth
        }
		else if (evol == 5f) {																// init to light fifth
            int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
            if (i == 0) ToFifth(true, 0);                                                       // to light circle fifth
            else if (i == 1) ToFifth(true, 1);                                                  // to light triangle fifth
            else if (i == 2) ToFifth(true, 2);													// to light square fifth
        }
		else if (evol == 8f) {																// init to light sixth
            int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
            if (i == 0) ToSixth(true, 0);                                                       // to light circle sixth
            else if (i == 1) ToSixth(false, 1);                                             	// to dark triangle sixth
            else if (i == 2) ToSixth(false, 2);													// to dark square sixth
        }
		else if (evol == 13f) {																// init to light seventh
            int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
            if (i == 0) ToSeventh(true, 0);                                                 	// to light circle seventh
            else if (i == 1) ToSeventh(true, 1);                                                // to light triangle seventh
            else if (i == 2) ToSeventh(true, 2);												// to light square seventh
        }
		else if (evol == 21f) {																// init to light eighth
			int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
			if (i == 0) ToEighth(true, 0);														// to light circle seventh
			else if (i == 1) ToEighth(true, 1);													// to light triangle seventh
			else if (i == 2) ToEighth(true, 2);													// to light square seventh
		}
		else if (evol == 34f) {																// init to light ninth
			int i = Random.Range(0, 2);															// random 0 or 1 or 2
			if (i == 0) ToNinth(true, 0);														// to light circle ninth
			else if (i == 1) ToNinth(true, 1);													// to light triangle ninth
			else if (i == 2) ToNinth(true, 2);													// to light square ninth
		}

		//checkEvol = true;																	// set check evol flag

    }

	public void Evol()
	{

		//Debug.Log ("particle Evol() on collision");

		evol = psp.evol;																	// local evol check			
		lightworld = psp.lightworld;														// local lightworld check
		inLightworld = psp.inLightworld;													// local inlightworld check
		isLight = psp.isLight;																// update light value
		deltaDark = psp.deltaDark;															// local dark check
		deltaLight = psp.deltaLight;														// local light check

		// zero
			// in dark world
		if (evol == 0f && !inLightworld) {													// to dark world light zero / within dark world
			if (deltaDark == -0.5f || deltaLight == -0.5f) ToZero (true);						// to dark world light zero
		}
			// to dark world
		if (evol == 0f && inLightworld) {													// to dark world light zero / from light world
			ToOtherWorld(false, 0, true, 0);													// to dark world light zero
		}

		// half zero
			// in dark world
		if (evol == 0.5f && !inLightworld && !lightworld) {									// to dark world half zero / from dark world / while dark world
			if (deltaDark > deltaLight) ToHalfZero (false);										// if gain more dark than light = to dark world dark zero
			//else if (deltaDark <= deltaLight) ToHalfZero (true);								// if gain more light than dark = to dark world light zero
		}
		else if (evol == 0.5f && !inLightworld && lightworld) {								// to dark world half zero / from dark world / while light world
			if (deltaDark > deltaLight) ToHalfZero (false);										// if gain more dark than light = to dark world dark zero
			else if (deltaDark <= deltaLight) ToHalfZero (true);								// if gain more light than dark = to dark world light zero
		}
			// to light world
		else if (evol == -0.5f && !inLightworld) {											// to light world half zero / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 0, false, 0);						// if lose more light than dark = to light world dark zero
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 0, true, 0);					// if lose more dark than light = to light world light zero
		}
			// in light world
		else if (evol == -0.5f && inLightworld) {											// to light world half zero / from light world
			if (deltaDark > deltaLight) ToHalfZero(false);										// if lose more light than dark, to light world dark zero
			else if (deltaDark <= deltaLight) ToHalfZero(true);									// if lose more dark than light, to light world light zero
		}
			// to dark world
		else if (evol == 0.5f && inLightworld) {											// to dark world half zero / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 0, false, 0);						// if lose more light than dark = to dark world dark zero
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 0, true, 0);					// if lose more dark than light = to dark world light zero
		}

		// first
			// in dark world
		if (evol == 1f && !inLightworld) {													// to dark world first / from dark world
			if (deltaDark > deltaLight) ToFirst(false);											// if gain more dark than light = to dark world dark first
			else if (deltaDark <= deltaLight) ToFirst(true);									// if gain more light than dark = to dark world light first
		}
			// to light world
		else if (evol == -1f && !inLightworld) {											// to light world first / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 1, false, 0);						// if lose more light than dark = to light world dark first
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 1, true, 0);					// if lose more dark than light = to light world light first
		}
			// in light world
		else if (evol == -1f && inLightworld) {												// to light world first / from light world
			if (deltaDark > deltaLight) ToFirst(false);											// if lose more light than dark = to light world dark first
			else if (deltaDark <= deltaLight) ToFirst(true);									// if lose more dark than light = to light world light first
		}
			// to dark world
		else if (evol == 1f && inLightworld) {												// to dark world first / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 1, false, 0);						// if lose more light than dark = to dark world dark first
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 1, true, 0);					// if lose more dark than light = to dark world light first
		}

		// second
			// in dark world
				// can't evolve zero to second
			// to light world
		if (evol == -1.5f && !inLightworld) {												// to light world second / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 2, false, 0);						// if lose more light than dark = to light world dark second
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 2, true, 0);					// if lose more dark than light = to light world light second
		}
			// in light world
		else if (evol == -1.5f && inLightworld) {											// to light world second / from light world
			if (deltaDark > deltaLight) ToSecond(false);										// if lose more light than dark = to light world dark second
			else if (deltaDark <= deltaLight) ToSecond(true);									// if lose more dark than light = to light world light second
		}
			// to dark world
		else if (evol == 1.5f && inLightworld) {											// to dark world second / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 2, false, 0);						// if lose more light than dark = to dark world dark second
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 2, true, 0);					// if lose more dark than light = to dark world light second
		}
	
		// third
			// in dark world
				// can't evolve zero to third
			// to light world
		if ((evol <= -2f && evol > -3f) && !inLightworld) {									// to light world third / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 3, false, 0);						// if lose more light than dark = to light world dark third
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 3, true, 0);					// if lose more dark than light = to light world light third
		}
			// in light world
		else if ((evol <= -2f && evol > -3f) && inLightworld) {								// to light world third / from dark world
			if (deltaDark > deltaLight) ToThird(false);											// if lose more light than dark = to light world dark third
			else if (deltaDark <= deltaLight) ToThird(true);									// if lose more dark than light = to light world light third
		}
			// to dark world
		else if ((evol >= 2f && evol < 3f) && inLightworld) {								// to dark world third / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 3, false, 0);						// if lose more light than dark = to dark world dark third
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 3, true, 0);					// if lose more dark than light = to dark world light third
		}

		// fourth
			// in dark world
				// can't evolve zero to fourth
			// to light world
		if ((evol <= -3f && evol > -5f) && !inLightworld) {									// to light world fourth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 4, false, 0);						// if lose more light than dark = to light world dark fourth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 4, true, 0);					// if lose more dark than light = to light world light fourth
		}
			// in light world
		else if ((evol <= -3f && evol > -5f) && inLightworld) {								// to light world fourth / from light world
			if (deltaDark > deltaLight) ToFourth(false);										// if lose more light than dark = to light world dark fourth
			else if (deltaDark <= deltaLight) ToFourth(true);									// if lose more dark than light = to light world light fourth
		}
			// to dark world
		else if ((evol >= 3f && evol < 5f) && inLightworld) {								// to dark world fourth / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 4, false, 0);						// if lose more light than dark = to dark world dark fourth
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 4, true, 0);					// if lose more dark than light = to dark world light fourth
		}

		///// fifth
			// in dark world
				// can't evolve zero to fifth
			// to light world
		if ((evol <= -5f && evol > -8f) && !inLightworld) {									// to light world fifth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 5, false, 0);						// if lose more light than dark = to light world dark fifth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 5, true, 0);					// if lose more dark than light = to light world light fifth
		}
			// in light world
		else if ((evol <= -5f && evol > -8f) && inLightworld) {								// to light world fifth / from light world
			if (deltaDark > deltaLight) ToFifth(false, 0);										// if lose more light than dark = to light world dark fifth
			else if (deltaDark <= deltaLight) ToFifth(true, 0);									// if lose more dark than light = to light world light fifth
		}

		// sixth
			// in dark world
				// can't evolve zero to fifth
			// to light world
		if ((evol <= -8f && evol > -13f) && !inLightworld) {								// to light world sixth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 6, false, 0);						// if lose more light than dark = to light world dark sixth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 6, true, 0);					// if lose more dark than light = to light world light sixth
		}
			// in light world
		else if ((evol <= -8f && evol > -13f) && inLightworld) {							// to light world sixth / from light world
			if (deltaDark > deltaLight) ToSixth(false, 0);										// if lose more light than dark = to light world dark sixth
			else if (deltaDark <= deltaLight) ToSixth(true, 0);									// if lose more dark than light = to light world light sixth
		}

		// seventh
			// in dark world
				// can't evolve zero to fifth
			// to light world
		if ((evol <= -13f && evol > -21f) && !inLightworld) {								// to light world seventh / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 7, false, 0);						// if lose more light than dark = to light world dark seventh
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 7, true, 0);					// if lose more dark than light = to light world light seventh
		}
			// in light world
		else if ((evol <= -13f && evol > -21f) && inLightworld) {							// to light world seventh / from light world
			if (deltaDark > deltaLight) ToSeventh(false, 0);									// if lose more light than dark = to light world dark seventh
			else if (deltaDark <= deltaLight) ToSeventh(true, 0);								// if lose more dark than light = to light world light seventh
		}

		// eighth
			// in dark world
				// can't evolve zero to fifth
			// to light world
		if ((evol <= -21f && evol > -34f) && !inLightworld) {								// to light world eighth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 8, false, 0);						// if lose more light than dark = to light world dark eighth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 8, true, 0);					// if lose more dark than light = to light world light eighth
		}
			// in light world
		else if ((evol <= -21f && evol > -34f) && inLightworld) {							// to light world eighth / from light world
			if (deltaDark > deltaLight) ToEighth(false, 0);										// if lose more light than dark = to light world dark eighth
			else if (deltaDark <= deltaLight) ToEighth(true, 0);								// if lose more dark than light = to light world light eighth
		}

		// ninth
			// in dark world
				// can't evolve zero to fifth
			// to light world
		if ((evol <= -34f && evol > -55f) && !inLightworld) {								// to light world ninth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 9, false, 0);						// if lose more light than dark = to light world dark ninth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 9, true, 0);					// if lose more dark than light = to light world light ninth
		}
			// in light world
		else if ((evol <= -34f && evol > -55f) && inLightworld) {							// to light world ninth / from light world
			if (deltaDark > deltaLight) ToNinth(false, 0);										// if lose more light than dark = to light world dark ninth
			else if (deltaDark <= deltaLight) ToNinth(true, 0);									// if lose more dark than light = to light world light ninth
		}

	}
}
