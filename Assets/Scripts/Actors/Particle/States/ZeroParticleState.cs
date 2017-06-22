using UnityEngine;
using System.Collections;

public class ZeroParticleState : IParticleState 
{
	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight;																	// 'is light' flag
	private bool lightworld;															// is light world ref
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol = false;																// check evol flag

	private bool canCollide = false;													// can collide flag (false to stun on new spawn)
	private float collisionTimer;														// reset can collide timer	

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
			Evol();																		// check evol logic
			//Debug.Log("check particle evol");
			checkEvol = false;															// reset check evol flag
		}

        if (psp.psp.isInit) Init();                                                     // if player init, particle init

		if (psp.inLightworld && !psp.lightworld) canCollide = false;					// if in lightworld and is dark world, prevent evol counting
		else if (psp.inLightworld && psp.lightworld) canCollide = true;					// if in lightworld and is light world, start evol counting

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
		if (!other.gameObject.CompareTag("World")) Debug.Log ("zero particle collision");
		if (canCollide) {										// if collision allowed and player is not stunned
			if (other.gameObject.CompareTag ("Player")) {								// if colide with player
				PlayerStatePattern pspOther 
					= other.gameObject.GetComponent<PlayerStatePattern>();					// ref other ParticleStatePattern
				if (pspOther.lightworld == psp.inLightworld) {								// if player and particle in same world
					canCollide = false;															// reset can collide trigger	
					psp.sc[0].enabled = false;													// disable trigger collider
					psp.stunned = true;															// stun for duration
					if (pspOther.evolC == 0f) {													// if player = 0
						Debug.Log ("zero particle+player=0: sub evol");
						psp.SubLight (0.5f);														// subtract 0.5 light
					}
					else if (pspOther.evolC > 0f) {												// else player > 0
						Debug.Log ("zero particle+player>0: sub evol");
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);				// subtract player dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);			// subtract player light
					}
					else if (pspOther.evolC < 0f) {												// if player evol is lower
						Debug.Log ("zero particle+player<0: sub evol");
						if (pspOther.darkEvolC != 0f) psp.SubDark(pspOther.darkEvolC);			// subtract player dark
						if (pspOther.lightEvolC != 0f) psp.SubLight(pspOther.lightEvolC);		// subtract player light
					}
					checkEvol = true;															// set check evol flag
				}
			} 
			else if (other.gameObject.CompareTag ("Zero")) {							// if collide with zero
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();				// ref other ParticleStatePattern
				if (pspOther.lightworld == psp.inLightworld) {								// if player and particle in same world
					canCollide = false;															// reset has collided trigger
					psp.sc[0].enabled = false;													// disable trigger collider
					psp.stunned = true;													    	// stunned flag
					Debug.Log ("zero particle+zero: roll die");
					RollDie (pspOther);															// roll die
					checkEvol = true;															// set check evol flag
				}
			} 
			else if (other.gameObject.CompareTag("First") 								// collide with first
				|| other.gameObject.CompareTag("Second")								// collide with second
				|| other.gameObject.CompareTag("Third")							        // collide with third
				|| other.gameObject.CompareTag("Fourth")							    // collide with fourth
				|| other.gameObject.CompareTag("Fifth")									// collide with fifth
				|| other.gameObject.CompareTag("Sixth")								    // collide with sixth
				|| other.gameObject.CompareTag("Seventh")							    // collide with seventh
				|| other.gameObject.CompareTag("Eighth")							    // collide with eighth
				|| other.gameObject.CompareTag("Ninth"))							    // collide with ninth
			{									
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern>();				// ref other ParticleStatePattern
				if (pspOther.lightworld == psp.inLightworld) {								// if player and particle in same world
					canCollide = false;															// reset has collided trigger
					psp.sc[0].enabled = false;													// disable trigger collider
					psp.stunned = true;															// set stunned flag
					Debug.Log ("zero particle+else: sub evol");
					if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);				// subtract other dark
					if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);			// subtract other light
					checkEvol = true;															// set check evol flag
				}
			}
		}
	}

	private void RollDie(ParticleStatePattern pspOther) {
		do {
			die = Random.Range(1,6);														// roll die
			psp.die = die;																	// make die value visible to other
			if (die > pspOther.die) {														// if this die > other die
				Debug.Log ("die roll: this > other");
				if (pspOther.evolC == 0) psp.AddLight (0.5f);									// if other = 0, add light
				else {																			// else
					if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);					// add dark of other
					if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);				// add light of other
				}
				rolling = false;																// exit roll
			}
			else if (die < pspOther.die) {													// if this die < other die
				Debug.Log ("die roll: this < other");
				if (pspOther.evolC == 0) psp.SubLight (0.5f);									// if other = 0, add light
				else {																			// else
					if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvol);					// add dark of other
					if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvol);				// add light of other
				}
				rolling = false;																// exit roll
			}
		} while (rolling);																	// reroll if same die
	}

	public void ToOtherWorld(bool toLW, int fromState, int toState, bool toLight)
	{
		psp.ChangeWorld(toLW, fromState, toState, toLight);					    			// trigger transition effects
		//psp.SpawnZero(1);												    					// spawn 1 zero
		if (toState == 1) psp.currentState = psp.firstState;		    			    	// set to first state
		else if (toState == 2) psp.currentState = psp.secondState;  						// set to second state
		else if (toState == 3) psp.currentState = psp.thirdState;						    // set to third state
		else if (toState == 4) psp.currentState = psp.fourthState;					    	// set to fourth state
		else if (toState == 5) psp.currentState = psp.fifthState;				    		// set to fifth state
		else if (toState == 6) psp.currentState = psp.sixthState;		    				// set to sixth state
		else if (toState == 7) psp.currentState = psp.seventhState;			    			// set to seventh state
		else if (toState == 8) psp.currentState = psp.eighthState;			    			// set to eighth state

		//ParticleStateEvents.toZero += psp.TransitionToZero;				        		// flag transition in delegate
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0);								        // trigger transition effects
	}

	public void ToHalfZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0);                                         // trigger transition effects
		//ParticleStateEvents.toLightZero += psp.TransitionToLightZero;
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(0, 1, isLight, toLight, 0);								         // trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;				        			// flag transition in delegate
		psp.currentState = psp.firstState;									        		// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(0, 2, isLight, toLight, 0);							        	// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;		    		    		// flag transition in delegate
		psp.currentState = psp.secondState;							    			    	// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(0, 3, isLight, toLight, 0);					    			    // trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;			    		    		// flag transition in delegate
		psp.currentState = psp.thirdState;									           		// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(0, 4, isLight, toLight, 0);					    			    // trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;		    	    			// flag transition in delegate
		psp.currentState = psp.fourthState;									      	    	// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 5, isLight, toLight, shape);				    			    // trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;			    	    			// flag transition in delegate
		psp.currentState = psp.fifthState;									        		// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 6, isLight, toLight, shape);				    		    	// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;			        				// flag transition in delegate
		psp.currentState = psp.sixthState;								    	    		// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(0, 7, isLight, toLight, shape);					    		    // trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;			        			// flag transition in delegate
		psp.currentState = psp.seventhState;								    	 	   	// set to new state
	}

	public void ToEighth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 8, isLight, toLight, shape);									// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;								// flag transition in delegate
		psp.currentState = psp.eighthState;													// set to new state
	}

    public void Init()
    {
		evol = psp.evol;                                                                    // local evol check
		isLight = psp.isLight;																	// update light value

        if (evol == 0f) ToZero(true);               									    // init to light zero
		else if (evol == 0.5f) ToHalfZero(false);               							// init to half zero
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
		else if (evol == 21f) {																// init to light seventh
			int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
			if (i == 0) ToEighth(true, 0);														// to light circle seventh
			else if (i == 1) ToEighth(true, 1);													// to light triangle seventh
			else if (i == 2) ToEighth(true, 2);													// to light square seventh
		}
        // new state
		checkEvol = true;																	// set check evol flag
    }

	public void Evol()
	{

		//Debug.Log ("particle Evol() on collision");

		evol = psp.evol;																	// local evol check			
		lightworld = psp.lightworld;														// local lightworld check
		isLight = psp.isLight;																	// update light value
		deltaDark = psp.deltaDark;															// local dark check
		deltaLight = psp.deltaLight;														// local light check

		//if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;							// if to light world (if evol < 0), set light world flag
		//else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;						// if to dark world, reset light world flag

		// zero
		if (evol == 0f && !lightworld) {													// devolve to dark world zero within dark world
			//Debug.Log ("fucking shit up");
			if (deltaDark == -0.5f || deltaLight == -0.5f) ToZero (true);						// to dark world zero
		}

		if (evol == 0f && lightworld) {														// evolve to dark world zero from light world
			//Debug.Log ("fucking shit up");
			ToOtherWorld(false, 0, 0, true);					// to dark world light zero
		}

		// half zero
		if (evol == 0.5f && !lightworld) {													// evolve to dark world half zero within dark world
			if (deltaDark > deltaLight) ToHalfZero (false);										// if gain more dark than light = to dark world dark zero
			//else if (deltaDark < deltaLight) ToHalfZero (true);								// if gain more light than dark = to dark world light zero
		}
		else if (evol == -0.5f && !lightworld) {											// devolve to light world half zero from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 0, true);						// if lose more dark than light = to light world light zero
			else if (deltaDark > deltaLight) {
				ToOtherWorld(true, 0, 0, false);					// if lose more light than dark = to light world dark zero
				//Debug.Log ("particle to other world, deltaLight < deltaDark");
			}
		}
		else if (evol == 0.5f && lightworld) {												// evolve to dark world half zero from light world
			if (deltaDark <= deltaLight) ToOtherWorld(false, 0, 0, true);						// if lose more dark than light = to dark world light zero
			else if (deltaDark > deltaLight) {
				ToOtherWorld(false, 0, 0, false);					// if lose more light than dark = to dark world dark zero
				//Debug.Log ("particle to other world, deltaLight < deltaDark");
			}
		}
		else if (evol == -0.5f && lightworld) {												// devolve to light world half zero from dark world
			if (deltaDark <= deltaLight) ToHalfZero(true);										// if lose more dark than light = to light world light zero
			else if (deltaDark > deltaLight) ToHalfZero(false);									// if lose more light than dark = to light world dark zero
		}
		// first
		if (evol == 1f && !lightworld) {													// evolve to dark world first within dark world
			if (deltaDark > deltaLight) ToFirst(false);											// if gain more dark than light = to dark world dark first
			else if (deltaDark <= deltaLight) ToFirst(true);									// if gain more light than dark = to dark world light first
		}
		else if (evol == -1f && !lightworld) {												// devolve to light world first from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 1, true);						// if lose more dark than light = to light world light first
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 1, false);					// if lose more light than dark = to light world dark first
		}
		else if (evol == -1f && lightworld) {												// devolve to light world first within light world
			if (deltaDark <= deltaLight) ToFirst(true);											// if lose more dark than light = to light world light first
			else if (deltaDark > deltaLight) ToFirst(false);									// if lose more light than dark = to light world dark first
		}
		// second
		if (evol == -1.5f && !lightworld) {										            // devolve to light world second from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 1, 2, true);						// if lose more dark than light = to light world light second
			else if (deltaDark > deltaLight) ToOtherWorld(true, 1, 2, false);					// if lose more light than dark = to light world dark second
		}
		else if (evol == -1.5f && lightworld) {												// devolve to light world second within light world
			if (deltaDark <= deltaLight) ToSecond(true);										// if lose more dark than light = to light world light second
			else if (deltaDark > deltaLight) ToSecond(false);									// if lose more light than dark = to light world dark second
		}
		// third
		if ((evol <= -2f && evol > -3f) && !lightworld) {								    // devolve to light world third from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 3, true);						// if lose more dark than light = to light world light third
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 3, false);					// if lose more light than dark = to light world dark third
		}
		else if ((evol <= -2f && evol > -3f) && lightworld) {								// devolve to light world third within light world
			if (deltaDark <= deltaLight) ToThird(true);											// if lose more dark than light = to light world light third
			else if (deltaDark > deltaLight) ToThird(false);									// if lose more light than dark = to light world dark third
		}
		// fourth
		if ((evol <= -3f && evol > -5f) && !lightworld) {									// devolve to light world fourth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 4, true);						// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 4, false);					// if lose more light than dark = to light world dark fourth
		}
		else if ((evol <= -3f && evol > -5f) && lightworld) {								// devolve to light world fourth within light world
			if (deltaDark <= deltaLight) ToFourth(true);										// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToFourth(false);									// if lose more light than dark = to light world dark fourth
		}
		// fifth
		if ((evol <= -5f && evol > -8f) && !lightworld) {									// devolve to light world fifth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 5, true);						// if lose more dark than light = to light world light fifth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 5, false);					// if lose more light than dark = to light world dark fifth
		}
		else if ((evol <= -5f && evol > -8f) && lightworld) {								// devolve to light world fifth within light world
			if (deltaDark <= deltaLight) ToFifth(true, 0);										// if lose more dark than light = to light world light fifth
			else if (deltaDark > deltaLight) ToFifth(false, 0);									// if lose more light than dark = to light world dark fifth
		}
		// sixth
		if ((evol <= -8f && evol > -13f) && !lightworld) {									// devolve to light world sixth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 6, true);						// if lose more dark than light = to light world light sixth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 6, false);					// if lose more light than dark = to light world dark sixth
		}
		else if ((evol <= -8f && evol > -13f) && lightworld) {								// devolve to light world sixth within light world
			if (deltaDark <= deltaLight) ToSixth(true, 0);										// if lose more dark than light = to light world light sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);									// if lose more light than dark = to light world dark sixth
		}
		// seventh
		if ((evol <= -13f && evol > -21f) && !lightworld) {									// devolve to light world seventh from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 7, true);						// if lose more dark than light = to light world light seventh
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 7, false);					// if lose more light than dark = to light world dark seventh
		}
		else if ((evol <= -13f && evol > -21f) && lightworld) {								// devolve to light world seventh within light world
			if (deltaDark <= deltaLight) ToSeventh(true, 0);									// if lose more dark than light = to light world light seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);								// if lose more light than dark = to light world dark seventh
		}
		// eighth
		if ((evol <= -21f && evol > -34f) && !lightworld) {									// devolve to light world eighth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 0, 8, true);						// if lose more dark than light = to light world light eighth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 8, false);					// if lose more light than dark = to light world dark eighth
		}
		else if ((evol <= -21f && evol > -34f) && lightworld) {								// devolve to light world eighth within light world
			if (deltaDark <= deltaLight) ToEighth(true, 0);										// if lose more dark than light = to light world light eighth
			else if (deltaDark > deltaLight) ToEighth(false, 0);								// if lose more light than dark = to light world dark eighth
		}
		// new state
	}
}
