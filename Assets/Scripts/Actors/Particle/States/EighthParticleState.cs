﻿using UnityEngine;
using System.Collections;

public class EighthParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	private bool lightworld;															// is light world ref
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset collision timer

	public int die;																		// collision conflict resolution
	private bool rolling = false;														// is rolling flag

	public EighthParticleState (ParticleStatePattern particleStatePattern)				// constructor
	{
		psp = particleStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
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
				|| other.gameObject.CompareTag ("Fifth")									// collide with fifth
				|| other.gameObject.CompareTag ("Sixth")									// collide with sixth
				|| other.gameObject.CompareTag ("Seventh")) {								// collide with seventh
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
			else if (other.gameObject.CompareTag ("Eighth")) {								// collide with eighth
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
			else if (other.gameObject.CompareTag("Ninth"))								    // collide with ninth
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
		psp.TransitionTo(8, 0, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;								// flag transition in delegate
		psp.SpawnFirst(4);																// spawn 4 Firsts
		psp.SpawnZero(3);																// spawn 3 Zeros
		psp.currentState = psp.zeroState;												// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(8, 1, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;								// flag transition in delegate
		psp.SpawnFirst(4);																// spawn 4 First
		psp.SpawnZero(2);																// spawn 2 Zeros
		psp.currentState = psp.firstState;												// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(8, 2, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;							// flag transition in delegate
		psp.SpawnFirst(3);																// spawn 3 First
		psp.SpawnZero(3);																// spawn 3 Zero
		psp.currentState = psp.secondState;												// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(8, 3, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;								// flag transition in delegate
		psp.SpawnFirst(3);																// spawn 3 First
		psp.SpawnZero(2);																// spawn 2 Zero
		psp.currentState = psp.thirdState;												// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(8, 4, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;							// flag transition in delegate
		psp.SpawnFirst(2);																// spawn 2 First
		psp.SpawnZero(3);																// spawn 3 Zero
		psp.currentState = psp.fourthState;												// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(8, 5, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;								// flag transition in delegate
		psp.SpawnFirst(3);																// spawn 3 First
		psp.currentState = psp.fifthState;												// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(8, 6, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;								// flag transition in delegate
		psp.SpawnFirst(2);																// spawn 2 First
		psp.SpawnZero(1);																// spawn 1 Zero
		psp.currentState = psp.sixthState;												// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(8, 7, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toEighth += psp.TransitionToEighth;							// flag transition in delegate
		psp.currentState = psp.seventhState;											// set to new state
	}

	public void ToEighth(bool toLight, int shape)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToNinth(bool toLight, int shape)
	{
		psp.TransitionTo(8, 9, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toEighth += psp.TransitionToEighth;							// flag transition in delegate
		psp.currentState = psp.ninthState;												// set to new state
	}

	public void Evol()
	{
		evol = psp.evol;																	// local evol check			
		lightworld = psp.lightworld;														// local lightworld check
		isLight = psp.isLight;																	// update light value
		deltaDark = psp.deltaDark;															// local dark check
		deltaLight = psp.deltaLight;														// local light check

		//if (!psp.lightworld	&& evol < 0f) psp.inLightworld = true;						// if to light world (if evol < 0), set light world flag
		//else if (psp.lightworld && evol >= 0f) psp.inLightworld = false;					// if to dark world, reset light world flag

		// zero
		if (evol == 0f && !lightworld) ToZero (true); 										// devolve to zero within dark world, to zero state
		//else if (evol == 0f && lightworld) ToOtherWorld(false, 1, 0, false);				// evolve to zero from light world, transition to dark world light zero
		// half zero
		if (evol == 0.5f && !lightworld) {													// devolve to dark world dark zero within dark world
			if (deltaDark > deltaLight) ToZero(false);											// if lose more light than dark = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);									// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f && !lightworld) {											// devolve to light world zero from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 7, 0, true);							// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToOtherWorld(true, 7, 0, true);					// if lose more light than dark = to dark zero
		}
		else if (evol == -0.5f && lightworld) {												// evolve to light world zero within light world 
			ToZero(true);																		// to light zero
		} 
		// first
		if (evol == 1f && !lightworld) {													// devolve to dark world first within dark world
			if (deltaDark < deltaLight) ToFirst(true);											// if lose more dark than light = to light first
			else if (deltaDark > deltaLight) ToFirst(false);									// if lose more light than dark = to dark first
		}
		else if (evol == -1f && !lightworld) {												// devolve to light world first from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 7, 1, true);							// if lose more dark than light = to light first
			else if (deltaDark > deltaLight) ToOtherWorld(true, 7, 1, false);					// if lose more light than dark = to dark first
		}
		else if (evol == -1f && lightworld) {												// evolve to light world first within light world
			if (deltaDark > deltaLight) ToFirst(false);											// if gain more dark than light = to light first
			else if (deltaDark < deltaLight) ToFirst(true);										// if gain more light than dark = to dark first
		}
		// second
		if (evol == 1.5f && !lightworld) {													// devolve to dark world second within dark world
			if (deltaDark < deltaLight) ToSecond(true);											// if lose more dark than light = to light second
			else if (deltaDark > deltaLight) ToSecond(false);									// if lose more light than dark = to dark second
		}
		else if (evol == -1.5f && !lightworld) {											// devolve to light world second from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 7, 2, true);							// if lose more dark than light = to light world light second
			else if (deltaDark > deltaLight) ToOtherWorld(true, 7, 2, false);					// if lose more light than dark = to light world dark second
		}
		else if (evol == -1.5f && lightworld) {												// evolve to light world second within light world
			if (deltaDark > deltaLight) ToSecond(false);										// if gain more dark than light = to dark world light second
			else if (deltaDark < deltaLight) ToSecond(true);									// if gain more light than dark = to light world dark second
		}
		// third
		if ((evol >= 2f && evol < 3f) && !lightworld) {										// devolve to dark or light world third within dark world
			if (deltaDark < deltaLight) ToThird(true);											// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);									// if lose more light than dark = to dark third
		}
		else if ((evol <= -2f && evol > -3f) && !lightworld) {								// devolve to light world third from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 7, 3, true);							// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToOtherWorld(true, 7, 3, false);					// if lose more light than dark = to dark third
		}
		else if ((evol <= -2f && evol > -3f) && lightworld) {								// evolve to light world third within light world
			if (deltaDark > deltaLight) ToThird(false);											// if gain more dark than light = to dark world light third
			else if (deltaDark < deltaLight) ToThird(true);										// if gain more dark than light = to light world dark third
		}
		// fourth
		if ((evol >= 3f && evol < 5f) && !lightworld) {										// devolve to dark world fourth within dark world
			if (circle && (deltaDark < deltaLight)) ToFourth(false);							// if either circle & lose more dark than light = to dark fourth
			else if (circle && (deltaDark > deltaLight)) ToFourth(false);						// if either circle & lose more light than dark = to dark fourth
			else if (triangle && (deltaDark < deltaLight)) ToFourth(true);						// if triangle & lose more dark than light = to light fourth
			else if (triangle && (deltaDark > deltaLight)) ToFourth(true);						// if triangle & lose more light than dark = to light fourth
			else if (square && (deltaDark < deltaLight)) ToFourth(true);						// if square & lose more dark than light = to light fourth
			else if (square && (deltaDark > deltaLight)) ToFourth(true);						// if square & lose more light than dark = to light fourth
		}
		else if ((evol <= -3f && evol > -5f) && !lightworld) {								// devolve to light world fourth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 7, 4, true);							// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 7, 4, false);					// if lose more dark than light = to light world dark fourth
		}
		else if ((evol <= -3f && evol > -5f) && lightworld) {								// evolve to light world fourth within light world
			if (deltaDark > deltaLight) ToFourth(false);										// if gain more dark than light = to dark world light fourth
			else if (deltaDark < deltaLight) ToFourth(true);									// if gain more dark than light = to light world dark fourth
		}
		// fifth
		if ((evol >= 5f && evol < 8f) && !lightworld) {										// devolve to dark world fifth (if evol >= 5 and < 8)
			if (circle && (deltaDark < deltaLight)) ToFifth(true, 0);							// if either circle & lose more dark than light = to light circle fifth
			else if (circle && (deltaDark > deltaLight)) ToFifth(false, 0);						// if either circle & lose more light than dark = to dark circle fifth
			else if (triangle && (deltaDark < deltaLight)) ToFifth(true, 1);					// if triangle & lose more dark than light = to triangle fifth
			else if (triangle && (deltaDark > deltaLight)) ToFifth(true, 1);					// if triangle & lose more light than dark = to triangle fifth
			else if (square && (deltaDark < deltaLight)) ToFifth(true, 2);						// if square & lose more dark than light = to square fifth
			else if (square && (deltaDark > deltaLight)) ToFifth(true, 2);						// if square & lose more light than dark = to square fifth
		} 
		else if ((evol <= -5f && evol > -8f) && !lightworld) {								// devolve to light world fifth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 7, 5, true);							// if lose more dark than light = to light world light circle fifth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 7, 5, false);					// if lose more light than dark = to light world dark circle fifth
		}
		else if ((evol <= -5f && evol > -8f) && lightworld) {								// evolve to light world fifth within light world
			if (deltaDark > deltaLight) ToFifth(false, 0);										// if gain more dark than light = to dark world light circle fifth
			else if (deltaDark < deltaLight) ToFifth(true, 0);									// if gain more light than dark = to light world dark circle fifth
		}
		// sixth
		if ((evol >= 8f && evol < 13f) && !lightworld) {									// devolve to dark world sixth (if evol >= 8 and < 13)
			if (circle && (deltaDark < deltaLight)) ToSixth (true, 0);							// if either circle & lose more dark than light = to light circle sixth
			else if (circle && (deltaDark > deltaLight)) ToSixth (false, 0);					// if either circle & lose more light than dark = to dark circle sixth
			else if (triangle && (deltaDark < deltaLight)) ToSixth (false, 1);					// if triangle & lose more dark than light = to triangle sixth
			else if (triangle && (deltaDark > deltaLight)) ToSixth (false, 1);					// if triangle & lose more light than dark = to triangle sixth
			else if (square && (deltaDark < deltaLight)) ToSixth (false, 2);					// if square & lose more dark than light = to square sixth
			else if (square && (deltaDark > deltaLight)) ToSixth (false, 2);					// if square & lose more light than dark = to square sixth
		} 
		else if ((evol <= -8f && evol > -13f) && !lightworld) {								// devolve to light world sixth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 7, 6, true);							// if lose more dark than light = to light world light circle sixth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 7, 6, false);					// if lose more dark than light = to light world dark circle sixth
		}
		else if ((evol <= -8f && evol > -13f) && lightworld) {								// evolve to light world sixth within light world
			if (deltaDark > deltaLight) ToSixth(false, 0);										// if gain more dark than light = to dark world light circle sixth
			else if (deltaDark < deltaLight) ToSixth(true, 0);									// if gain more light than dark = to light world dark circle sixth
		}
		// seventh
		if (evol >= 13f && evol < 21f) {																	// devolve to dark world seventh (if evol >= 13 and < 21)
			if (circle && (deltaDark > deltaLight)) ToSeventh (false, 0);										// if either circle & gain more dark than light = to dark circle eighth
			else if (circle && (deltaDark <= deltaLight)) ToSeventh (true, 0);									// if either circle & gain more light than dark = to light circle eighth
			else if (triangle && (deltaDark > deltaLight)) ToSeventh (false, 1);								// if either triangle & gain more dark than light = to dark triangle eighth
			else if (triangle && (deltaDark <= deltaLight)) ToSeventh (true, 1);								// if either triangle & gain more light than dark = to light triangle eighth
			else if (square && (deltaDark > deltaLight)) ToSeventh (false, 2);									// if either square & gain more dark than light = to dark square eighth
			else if (square && (deltaDark <= deltaLight)) ToSeventh (true, 2);									// if either square & gain more light than dark = to light square eighth
		} 
		else if (evol <= -13 && evol > -21) {																// devolve to light world seventh (if evol >= -13 and < -21)
			if (deltaDark <= deltaLight) ToSeventh (true, 0);													// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToSeventh (false, 0);												// if lose more light than dark = to dark circle eighth
		}
		// ninth
		if (evol >= 34) {																					// evolve to dark world ninth (if evol >= 34)
			if (circle && (deltaDark > deltaLight)) ToNinth (false, 0);											// if either circle & gain more dark than light = to dark circle eighth
			else if (circle && (deltaDark <= deltaLight)) ToNinth (true, 0);									// if either circle & gain more light than dark = to light circle eighth
			else if (triangle && (deltaDark > deltaLight)) ToNinth (false, 1);									// if either triangle & gain more dark than light = to dark triangle eighth
			else if (triangle && (deltaDark <= deltaLight)) ToNinth (true, 1);									// if either triangle & gain more light than dark = to light triangle eighth
			else if (square && (deltaDark > deltaLight)) ToNinth (false, 2);									// if either square & gain more dark than light = to dark square eighth
			else if (square && (deltaDark <= deltaLight)) ToNinth (true, 2);									// if either square & gain more light than dark = to light square eighth
		} 
		else if (evol <= -34) {																				// devolve to light world eighth (if evol >= -34)
			if (deltaDark <= deltaLight) ToNinth (true, 0);														// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToNinth (false, 0);												// if lose more light than dark = to dark circle eighth
		}
	}
}