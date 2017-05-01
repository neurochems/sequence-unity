﻿using UnityEngine;
using System.Collections;

public class FifthParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	private bool lightworld;															// is light world ref
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset can collide timer

	public FifthParticleState (ParticleStatePattern statePatternParticle)				// constructor
	{
		psp = statePatternParticle;															// attach state pattern to this state 
	}

	public void UpdateState()
	{
		Evol ();

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;										// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.stunned = false;															// reset stunned trigger
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern pspOther 
			= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern

		if (canCollide) {																	// if collision allowed
		
			if (other.gameObject.CompareTag ("Player")) {										// colide with player
				psp.stunned = true;																	// stun new particle for 3 sec
				canCollide = false;																	// reset can collide trigger	
				Debug.Log ("particle contact player");
			} 
			if (other.gameObject.CompareTag ("Zero")											// collide with zero
				|| other.gameObject.CompareTag ("First")										// collide with first
				|| other.gameObject.CompareTag ("Second")										// collide with second
				|| other.gameObject.CompareTag ("Third")										// collide with third
				|| other.gameObject.CompareTag ("Fourth")										// collide with fourth
				|| other.gameObject.CompareTag ("Fifth")) {										// collide with fifth
				psp.stunned = true;																	// set stunned flag
				psp.AddDark (pspOther.darkEvol);													// add dark of other
				psp.AddLight (pspOther.lightEvol);													// add light of other
				canCollide = false;																	// reset has collided trigger
			} 
			else {																				// collide with any other
				psp.stunned = true;																	// set stunned flag
				psp.SubDark (pspOther.darkEvol);													// subtract other dark
				psp.SubLight (pspOther.lightEvol);													// subtract other light
				canCollide = false;																	// reset has collided trigger
			} 
		}
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

		//ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
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

	public void ToFifth(bool toLight, int shape)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(5, 6, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;					// flag transition in delegate
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
		evol = psp.evol;																	// local evol check			
		lightworld = psp.lightworld;														// local lightworld check
		light = psp.light;																	// update light value
		deltaDark = psp.deltaDark;															// local dark check
		deltaLight = psp.deltaLight;														// local light check

		//if (!psp.lightworld	&& evol < 0f) psp.inLightworld = true;							// if to light world (if evol < 0), set light world flag
		//else if (psp.lightworld && evol >= 0f) psp.inLightworld = false;						// if to dark world, reset light world flag

		// zero
		if (evol == 0f && !lightworld) ToZero (true); 										// devolve to zero within dark world, to zero state
		//else if (evol == 0f && lightworld) ToOtherWorld(false, 1, 0, false);				// evolve to zero from light world, transition to dark world light zero
		// half zero
		if (evol == 0.5f && !lightworld) {													// devolve to dark world dark zero within dark world
			if (deltaDark > deltaLight) ToZero(false);											// if lose more light than dark = to dark world dark zero
			// else if (deltaDark < deltaLight) ToZero(true);									// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f && !lightworld) {											// devolve to light world zero from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 0, true);							// if lose more dark than light = to light world light zero
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 0, true);					// if lose more light than dark = to light world dark zero
		}
		else if (evol == -0.5f && lightworld) {												// evolve to light world zero within light world 
			ToZero(true);																		// to light world light zero
		} 
		// first
		if (evol == 1f && !lightworld) {													// devolve to dark world first within dark world
			if (deltaDark < deltaLight) ToFirst(true);											// if lose more dark than light = to dark world light first
			else if (deltaDark > deltaLight) ToFirst(false);									// if lose more light than dark = to dark world dark first
		}
		else if (evol == -1f && !lightworld) {												// devolve to light world first from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 1, true);							// if lose more dark than light = to light world light first
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 1, false);					// if lose more light than dark = to light world dark first
		}
		else if (evol == -1f && lightworld) {												// evolve to light world first within light world
			if (deltaDark > deltaLight) ToFirst(false);											// if gain more dark than light = to light world dark first
			else if (deltaDark < deltaLight) ToFirst(true);										// if gain more light than dark = to light world light first
		}
		// second
		if (evol == 1.5f && !lightworld) {													// devolve to dark world second within dark world
			if (deltaDark < deltaLight) ToSecond(true);											// if lose more dark than light = to dark world light second
			else if (deltaDark > deltaLight) ToSecond(false);									// if lose more light than dark = to dark world dark second
		}
		else if (evol == -1.5f && !lightworld) {											// devolve to light world second from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 2, true);							// if lose more dark than light = to light world light second
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 2, false);					// if lose more light than dark = to light world dark second
		}
		else if (evol == -1.5f && lightworld) {												// evolve to light world second within light world
			if (deltaDark > deltaLight) ToSecond(false);										// if gain more dark than light = to light world dark second
			else if (deltaDark < deltaLight) ToSecond(true);									// if gain more light than dark = to light world light second
		}
		// third
		if ((evol >= 2f && evol < 3f) && !lightworld) {										// devolve to dark world third within dark world
			if (deltaDark < deltaLight) ToThird(true);											// if lose more dark than light = to dark world ight third
			else if (deltaDark > deltaLight) ToThird(false);									// if lose more light than dark = to dark world dark third
		}
		else if ((evol <= -2f && evol > -3f) && !lightworld) {								// devolve to light world third from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 3, true);							// if lose more dark than light = to light world light third
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 3, false);					// if lose more light than dark = to light world dark third
		}
		else if ((evol <= -2f && evol > -3f) && lightworld) {								// evolve to light world third within light world
			if (deltaDark > deltaLight) ToThird(false);											// if gain more dark than light = to light world dark third
			else if (deltaDark < deltaLight) ToThird(true);										// if gain more light than dark = to light world light third
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
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 4, true);							// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 4, false);					// if lose more light than dark = to light world dark fourth
		}
		else if ((evol <= -3f && evol > -5f) && lightworld) {								// devolve to light world fourth within light world
			if (deltaDark < deltaLight) ToFourth(true);											// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToFourth(false);									// if lose more light than dark = to light world dark fourth
		}
		// fifth
		if ((evol <= -5f && evol > -8f) && !lightworld) {									// devolve to light world fifth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 5, true);							// if lose more dark than light = to light world light circle fifth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 5, false);					// if lose more light than dark = to light world dark circle fifth
		} 
		// sixth
		if ((evol >= 8f && evol < 13f) && !lightworld) {									// evolve to dark world sixth within dark world
			if (circle && (deltaDark < deltaLight)) ToSixth(false, 0);							// if either circle & gain more dark than light = to dark circle sixth
			else if (circle && (deltaDark > deltaLight)) ToSixth(true, 0);						// if either circle & gain more light than dark = to light circle sixth
			else if (triangle && (deltaDark < deltaLight)) ToSixth(false, 1);					// if triangle & gain more dark than light = to dark triangle sixth
			else if (triangle && (deltaDark > deltaLight)) ToSixth(false, 1);					// if triangle & gain more light than dark = to dark triangle sixth
			else if (square && (deltaDark < deltaLight)) ToSixth(false, 2);						// if square & gain more dark than light = to dark square sixth
			else if (square && (deltaDark > deltaLight)) ToSixth(false, 2);						// if square & gain more light than dark = to dark square sixth
		}
		else if ((evol <= -8f && evol > -13f) && !lightworld) {								// devolve to light world sixth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 6, true);							// if lose more dark than light = to light world light circle sixth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 6, false);					// if lose more light than dark = to light world dark circle sixth
		}
		else if ((evol <= -8f && evol > -13f) && lightworld) {								// devolve to light world sixth within light world
			if (deltaDark < deltaLight) ToSixth(true, 0);										// if lose more dark than light = to light world light circle sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);									// if lose more light than dark = to light world dark circle sixth
		}
		// seventh
		if ((evol <= -13f && evol > -21f) && !lightworld) {									// devolve to light world seventh from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 5, 7, true);							// if lose more dark than light = to light world light circle seventh
			else if (deltaDark > deltaLight) ToOtherWorld(true, 5, 7, false);					// if lose more light than dark = to light world dark circle seventh
		}
		else if ((evol <= -13f && evol > -21f) && lightworld) {								// devolve to light world seventh within light world
			if (deltaDark < deltaLight) ToSeventh(true, 0);										// if lose more dark than light = to light world light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);								// if lose more light than dark = to light world dark circle seventh
		}
		/*else if (evol <= -21f && evol > -34f) {															// devolve to light world eighth (if evol = -21)
			if (deltaDark < deltaLight) ToEighth(true, 0);														// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToEighth(false, 0);												// if lose more dark than light = to dark circle eighth
		}*/
	}
}
