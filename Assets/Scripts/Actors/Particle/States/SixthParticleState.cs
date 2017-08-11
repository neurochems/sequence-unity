using UnityEngine;
using System.Collections;

public class SixthParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	private bool lightworld;															// is light world ref
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset can collide timer

	public int die;																		// collision conflict resolution
	private bool rolling = false;														// is rolling flag

	public SixthParticleState (ParticleStatePattern statePatternParticle)				// constructor
	{
		psp = statePatternParticle;															// attach state pattern to this state 
	}

	public void UpdateState()
	{
		//Debug.Log (psp.transform.name + "particle sixth state");
		// check evol
		if (checkEvol) {
			Evol();																		// check evol logic
			//Debug.Log("check particle evol");
			checkEvol = false;															// reset check evol flag
		}

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
		// state class doesn't know what shape it is until it contacts another
		circle = psp.circle;																	// set current circle flag
		triangle = psp.triangle;																// set current triangle flag
		square = psp.square;																	// set current square flag

		if (canCollide && !psp.psp.stunned) {											// if collision allowed and player is not stunned
			if (other.gameObject.CompareTag ("Player")) {									// colide with player
				PlayerStatePattern pspOther 
				= other.gameObject.GetComponent<PlayerStatePattern>();							// ref other ParticleStatePattern
				if (pspOther.lightworld == psp.inLightworld) {									// if player and particle in same world
					canCollide = false;																// reset can collide trigger	
					psp.sc[0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// stun for duration
					if (psp.evolC > pspOther.evolC) {													// if player evol is lower
						if (pspOther.darkEvolC != 0f) psp.AddDark(pspOther.darkEvol);						// add player dark evol
						if (pspOther.lightEvolC != 0f) psp.AddLight(pspOther.lightEvol);					// add player light evol
					}
					else if (psp.evolC <= pspOther.evolC) {												// else player is higher
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvol);						// subtract player dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvol);					// subtract player light
					}
					checkEvol = true;																// check evol flag
					Debug.Log ("particle contact player");
				}
				pspOther = null;																// clear pspOther
			} 
			else if (other.gameObject.CompareTag ("Zero")									// collide with zero
				|| other.gameObject.CompareTag ("First")									// collide with first
				|| other.gameObject.CompareTag ("Second")									// collide with second
				|| other.gameObject.CompareTag ("Third")									// collide with third
				|| other.gameObject.CompareTag ("Fourth")									// collide with fourth
				|| other.gameObject.CompareTag ("Fifth")) {									// collide with fifth
				ParticleStatePattern pspOther 
				= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (pspOther.lightworld == psp.inLightworld) {									// if player and particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc[0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// set stunned flag
					if (pspOther.evolC == 0f) {														// if other = 0
						psp.AddLight (0.5f);															// add 0.5 light
					}
					else if (pspOther.evolC > 0f) {													// if other > 0
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvol);					// add dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvol);				// add light of other
					}
					else if (pspOther.evolC < 0f) {													// if other < 0
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvol * -1);				// add dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvol * -1);			// add light of other
					}
					checkEvol = true;																// check evol flag
				}
				pspOther = null;																// clear pspOther
			} 
			else if (other.gameObject.CompareTag ("Sixth")) {								// collide with sixth
				ParticleStatePattern pspOther 
				= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (pspOther.lightworld == psp.inLightworld) {									// if player and particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc[0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// stun for duration
					if (psp.evolC > pspOther.evolC) {													// if evol > other
						if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvol);						// add dark of other
						if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvol);					// add light of other
					}
					else if (psp.evolC == pspOther.evolC) RollDie (pspOther);							// if evol = other, roll die
					if (psp.evolC < pspOther.evolC) {													// if evol < other
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvol);						// sub dark of other
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvol);					// sub light of other
					}
					checkEvol = true;																// check evol flag
				}
				pspOther = null;																// clear pspOther
			}
			else if (other.gameObject.CompareTag("Seventh")								    // collide with seventh
				|| other.gameObject.CompareTag("Eighth")								    // collide with eighth
				|| other.gameObject.CompareTag("Ninth"))								    // collide with ninth
			{
				ParticleStatePattern pspOther 
				= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (pspOther.lightworld == psp.inLightworld) {									// if player and particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc[0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// stun for duration
					if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvol);					// subtract other dark
					if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvol);				// subtract other light
					checkEvol = true;																// check evol flag
				}
				pspOther = null;																// clear pspOther
			}
		}
	}

	private void RollDie(ParticleStatePattern pspOther) {
		do {
			die = Random.Range(1,6);														// roll die
			psp.die = die;																	// make die value visible to other
			if (die > pspOther.die) {														// if this die > other die
				if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvol);					// add dark of other
				if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvol);				// add light of other
				rolling = false;																// exit roll
			}
			else if (die < pspOther.die) {													// if this die < other die
				if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvol);					// add dark of other
				if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvol);				// add light of other
				rolling = false;																// exit roll
			}
		} while (rolling);																	// reroll if same die
	}

	public void ToOtherWorld(bool toLW, int fromState, int toState, bool toLight)
	{
		psp.ChangeWorld(toLW, fromState, toState, toLight);								// trigger transition effects
		//psp.SpawnZero(1);																	// spawn 1 zero
		if (toState == 0) psp.currentState = psp.zeroState;								// set to zero state
		else if (toState == 1) psp.currentState = psp.firstState;						// set to first state
		else if (toState == 2) psp.currentState = psp.secondState;						// set to second state
		else if (toState == 3) psp.currentState = psp.thirdState;						// set to third state
		else if (toState == 4) psp.currentState = psp.fourthState;						// set to fourth state
		else if (toState == 5) psp.currentState = psp.fifthState;						// set to fifth state
		else if (toState == 6) psp.currentState = psp.sixthState;						// set to sixth state
		else if (toState == 7) psp.currentState = psp.seventhState;						// set to seventh state
		else if (toState == 8) psp.currentState = psp.eighthState;						// set to eighth state
		else if (toState == 9) psp.currentState = psp.ninthState;						// set to ninth state

		//ParticleStateEvents.toZero += psp.TransitionToZero;								// flag transition in delegate
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(6, 0, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;								// flag transition in delegate
		psp.SpawnFirst(2);																// spawn 2 Firsts
		psp.SpawnZero(3);																// spawn 3 Zeros
		psp.currentState = psp.zeroState;												// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(6, 1, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;								// flag transition in delegate
		psp.SpawnFirst(2);																// spawn 1 First
		psp.currentState = psp.firstState;												// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(6, 2, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TranitionToSecond;							// flag transition in delegate
		psp.SpawnFirst(1);																// spawn 1 First
		psp.SpawnZero(3);																// spawn 3 Zero
		psp.currentState = psp.secondState;												// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(6, 3, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;								// flag transition in delegate
		psp.SpawnFirst(1);																// spawn 1 First
		psp.SpawnZero(2);																// spawn 2 Zero
		psp.currentState = psp.thirdState;												// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(6, 4, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;							// flag transition in delegate
		psp.SpawnFirst(1);																// spawn 1 First
		psp.SpawnZero(1);																// spawn 1 Zero
		psp.currentState = psp.fourthState;												// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(6, 5, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;								// flag transition in delegate
		psp.SpawnFirst(1);																// spawn 1 First
		psp.currentState = psp.fifthState;												// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(6, 7, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;							// flag transition in delegate
		psp.currentState = psp.seventhState;											// set to new state
	}

	public void ToEighth(bool toLight, int shape)
	{
		psp.TransitionTo(6, 8, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;							// flag transition in delegate
		psp.currentState = psp.eighthState;												// set to new state
	}

	public void ToNinth(bool toLight, int shape)
	{
		psp.TransitionTo(6, 9, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;							// flag transition in delegate
		psp.currentState = psp.ninthState;												// set to new state
	}

	public void Evol()
	{
		evol = psp.evol;																	// local evol check			
		lightworld = psp.lightworld;														// local lightworld check
		isLight = psp.isLight;																	// update light value
		deltaDark = psp.deltaDark;															// local dark check
		deltaLight = psp.deltaLight;														// local light check

		//if (!psp.lightworld	&& evol < 0f) psp.inLightworld = true;							// if to light world (if evol < 0), set light world flag
		//else if (psp.lightworld && evol >= 0f) psp.inLightworld = false;						// if to dark world, reset light world flag

		// zero
		if (evol == 0f && !lightworld) ToZero (true); 										// devolve to zero within dark world, to zero state
		//else if (evol == 0f && lightworld) ToOtherWorld(false, 1, 0, false);				// evolve to zero from light world, transition to dark world light zero
		// half zero
		if (evol == 0.5f && !lightworld) {													// devolve to dark world dark zero within dark world
			if (deltaDark > deltaLight) ToZero(false);											// if lose more light than dark = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);									// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f && !lightworld) {											// devolve to light world zero from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 0, true);						// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 0, true);					// if lose more light than dark = to dark zero
		}
		else if (evol == -0.5f && lightworld) {												// evolve to light world zero within light world 
			ToZero(true);																		// to light zero
		} 
		// first
		if (evol == 1f && !lightworld) {													// devolve to dark world first within dark world
			if (deltaDark <= deltaLight) ToFirst(true);											// if lose more dark than light = to dark world light first
			else if (deltaDark > deltaLight) ToFirst(false);									// if lose more light than dark = to dark world dark first
		}
		else if (evol == -1f && !lightworld) {												// devolve to light world first from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 1, true);						// if lose more dark than light = to light world light first
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 1, false);					// if lose more light than dark = to light world dark first
		}
		else if (evol == -1f && lightworld) {												// evolve to light world first within light world
			if (deltaDark > deltaLight) ToFirst(false);											// if gain more dark than light = to light world dark first
			else if (deltaDark <= deltaLight) ToFirst(true);									// if gain more light than dark = to light world light first
		}
		// second
		if (evol == 1.5f && !lightworld) {													// devolve to dark world second within dark world
			if (deltaDark <= deltaLight) ToSecond(true);										// if lose more dark than light = to dark world light second
			else if (deltaDark > deltaLight) ToSecond(false);									// if lose more light than dark = to dark world dark second
		}
		else if (evol == -1.5f && !lightworld) {											// devolve to light world second from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 2, true);						// if lose more dark than light = to light world light second
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 2, false);					// if lose more light than dark = to light world dark second
		}
		else if (evol == -1.5f && lightworld) {												// evolve to light world second within light world
			if (deltaDark > deltaLight) ToSecond(false);										// if gain more dark than light = to light world dark second
			else if (deltaDark <= deltaLight) ToSecond(true);									// if gain more light than dark = to light world light second
		}
		// third
		if ((evol >= 2f && evol < 3f) && !lightworld) {										// devolve to dark world third within dark world
			if (deltaDark <= deltaLight) ToThird(true);											// if lose more dark than light = to dark world light third
			else if (deltaDark > deltaLight) ToThird(false);									// if lose more light than dark = to dark world dark third
		}
		else if ((evol <= -2f && evol > -3f) && !lightworld) {								// devolve to light world third from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 3, true);						// if lose more dark than light = to light world light third
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 3, false);					// if lose more light than dark = to light world dark third
		}
		else if ((evol <= -2f && evol > -3f) && lightworld) {								// evolve to light world third within light world
			if (deltaDark > deltaLight) ToThird(false);											// if gain more dark than light = to dark world dark third
			else if (deltaDark <= deltaLight) ToThird(true);									// if gain more light than dark = to light world light third
		}
		// fourth
		if ((evol >= 3f && evol < 5f) && !lightworld) {										// devolve to dark world fourth within dark world
			if (circle && (deltaDark <= deltaLight)) ToFourth(false);							// if either circle & lose more dark than light = to dark fourth
			else if (circle && (deltaDark > deltaLight)) ToFourth(false);						// if either circle & lose more light than dark = to dark fourth
			else if (triangle && (deltaDark <= deltaLight)) ToFourth(true);						// if triangle & lose more dark than light = to light fourth
			else if (triangle && (deltaDark > deltaLight)) ToFourth(true);						// if triangle & lose more light than dark = to light fourth
			else if (square && (deltaDark <= deltaLight)) ToFourth(true);						// if square & lose more dark than light = to light fourth
			else if (square && (deltaDark > deltaLight)) ToFourth(true);						// if square & lose more light than dark = to light fourth
		}
		else if ((evol <= -3f && evol > -5f) && !lightworld) {								// devolve to light world fourth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 4, true);						// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 4, false);					// if lose more light than dark = to dark world dark fourth
		}
		else if ((evol <= -3f && evol > -5f) && lightworld) {								// evolve to light world fourth within light world
			if (deltaDark < deltaLight) ToFourth(false);										// if gain more dark than light = to light world dark fourth
			else if (deltaDark >= deltaLight) ToFourth(true);									// if gain more light than light = to light world light fourth
		}
		// fifth
		if ((evol >= 5f && evol < 8f) && !lightworld) {										// devolve to dark world fifth (if evol >= 5 and < 8)
			if (circle && (deltaDark <= deltaLight)) ToFifth(true, 0);							// if either circle & lose more dark than light = to light circle fifth
			else if (circle && (deltaDark > deltaLight)) ToFifth(false, 0);						// if either circle & lose more light than dark = to dark circle fifth
			else if (triangle && (deltaDark <= deltaLight)) ToFifth(true, 1);					// if triangle & lose more dark than light = to triangle fifth
			else if (triangle && (deltaDark > deltaLight)) ToFifth(true, 1);					// if triangle & lose more light than dark = to triangle fifth
			else if (square && (deltaDark <= deltaLight)) ToFifth(true, 2);						// if square & lose more dark than light = to square fifth
			else if (square && (deltaDark > deltaLight)) ToFifth(true, 2);						// if square & lose more light than dark = to square fifth
		} 
		else if ((evol <= -5f && evol > -8f) && !lightworld) {								// devolve to light world fifth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 5, true);						// if lose more dark than light = to light world light circle fifth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 5, false);					// if lose more light than dark = to light world dark circle fifth
		}
		else if ((evol <= -5f && evol > -8f) && lightworld) {								// evolve to light world fifth within light world
			if (deltaDark > deltaLight) ToFifth(false, 0);										// if gain more dark than light = to light world dark circle fifth
			else if (deltaDark <= deltaLight) ToFifth(true, 0);									// if gain more light than dark = to light world light circle fifth
		}
		// sixth
		if ((evol <= -8f && evol > -13f) && !lightworld) {									// devolve to light world sixth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 6, true);						// if lose more dark than light = to light world light circle sixth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 6, false);					// if lose more light than dark = to light world dark circle sixth
		}
		// seventh
		if ((evol >= 13f) && !lightworld) {													// evolve to dark world seventh (if evol >= 13)
			if (circle && (deltaDark > deltaLight)) ToSeventh(false, 0);						// if either circle & gain more dark than light = to dark circle seventh
			else if (circle && (deltaDark <= deltaLight)) ToSeventh(true, 0);					// if either circle & gain more light than dark = to light circle seventh
			else if (triangle && (deltaDark > deltaLight)) ToSeventh(false, 1);					// if triangle & gain more dark than light = to dark triangle seventh
			else if (triangle && (deltaDark <= deltaLight)) ToSeventh(true, 1);					// if triangle & gain more light than dark = to light triangle seventh
			else if (square && (deltaDark > deltaLight)) ToSeventh(false, 2);					// if square & gain more dark than light = to dark square seventh
			else if (square && (deltaDark <= deltaLight)) ToSeventh(true, 2);					// if square & gain more light than dark = to light square seventh
		}
		else if ((evol <= -13f && evol > -21f) && !lightworld) {							// devolve to light world seventh from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 7, true);						// if lose more dark than light = to light world light circle seventh
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 7, false);					// if lose more light than dark = to light world dark circle seventh
		}
		else if ((evol <= -13f && evol > -21f) && lightworld) {								// devolve to light world seventh within light world
			if (deltaDark <= deltaLight) ToSeventh(true, 0);									// if lose more dark than light = to light world light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);								// if lose more light than dark = to light world dark circle seventh
		}
		// eighth
		if ((evol <= -21f && evol > -34f) && !lightworld) {									// devolve to light world eighth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 8, true);						// if lose more dark than light = to light world light eighth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 8, false);					// if lose more light than dark = to light world dark eighth
		}
		else if ((evol <= -21f && evol > -34f) && lightworld) {								// devolve to light world eighth within light world
			if (deltaDark <= deltaLight) ToEighth(true, 0);										// if lose more dark than light = to light world light eighth
			else if (deltaDark > deltaLight) ToEighth(false, 0);								// if lose more light than dark = to light world dark eighth
		}
		// ninth
		if ((evol <= -34f && evol > -55f) && !lightworld) {									// devolve to light world ninth from dark world
			if (deltaDark <= deltaLight) ToOtherWorld(true, 6, 9, true);						// if lose more dark than light = to light world light ninth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 6, 9, false);					// if lose more light than dark = to light world dark ninth
		}
		else if ((evol <= -34f && evol > -55f) && lightworld) {								// devolve to light world ninth within light world
			if (deltaDark <= deltaLight) ToNinth(true, 0);										// if lose more dark than light = to light world light ninth
			else if (deltaDark > deltaLight) ToNinth(false, 0);									// if lose more light than dark = to light world dark ninth
		}
	}
}
