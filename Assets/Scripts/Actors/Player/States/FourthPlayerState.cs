using UnityEngine;
using System.Collections;

public class FourthPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public FourthPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		Evol ();																		// check evol

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.stunned = false;															// reset stunned flag
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern pspOther 
			= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern

		if (canCollide) {																		// if collision allowed

			if (other.gameObject.CompareTag ("Zero")												// collide with zero
				|| other.gameObject.CompareTag ("First")											// collide with first
				|| other.gameObject.CompareTag ("second")											// collide with second
				|| other.gameObject.CompareTag ("Third")											// collide with third
				|| other.gameObject.CompareTag ("Fourth")) {										// collide with fourth
				psp.stunned = true;																		// stun for duration
				psp.AddDark (pspOther.darkEvol);														// add dark of other
				psp.AddLight (pspOther.lightEvol);														// add light of other
				canCollide = false;																		// reset has collided trigger
			} 
			else {																					// collide with any other
				psp.stunned = true;																		// set stunned flag
				psp.SubDark (pspOther.darkEvol);														// subtract other dark
				psp.SubLight (pspOther.lightEvol);														// subtract other light
				canCollide = false;																		// reset has collided trigger
			}
		}
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(4, 0, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnFirst(1);														// spawn 1 First
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(4, 1, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;					// flag transition in delegate
		psp.SpawnZero(3);														// spawn 3 Zeros
		psp.currentState = psp.firstState;										// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(4, 2, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.SpawnZero(2);														// spawn 2 Zeros
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(4, 3, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;					// flag transition in delegate
		psp.SpawnZero(1);														// spawn 1 Zero
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool toLight)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(4, 5, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;					// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(4, 6, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(4, 7, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new state
	}

	public void Evol()													
	{
		evol = psp.evol;																					// local evol check			
		light = psp.light;																					// update light value
		deltaDark = psp.deltaDark;																			// local dark check
		deltaLight = psp.deltaLight;																		// local light check

		if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;											// if to light world (if evol < 0), set to light world trigger
		else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;										// if to dark world (evol >= 0), set dark world flag

		else if (evol == 0) {																				// to zero (if evol = 0)
			ToZero (true);																						// to zero state
		}
		else if (evol == 0.5f) {																			// devolve to dark world dark zero (if evol = 0.5)
			if (deltaDark > deltaLight) ToZero(false);															// if lose more light than dark = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);													// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f) {																			// devolve to light world zero (if evol = -0.5)
			if (deltaDark < deltaLight) ToZero(true);															// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToZero(false);														// if lose more light than dark = to dark zero
		} 
		else if ((evol == 1f) || (evol == -1f)) {															// devolve to dark or light world first (if evol == 1)
			if (deltaDark < deltaLight) ToFirst(true);															// if lose more dark than light = to light first
			else if (deltaDark > deltaLight) ToFirst(false);													// if lose more light than dark = to dark first
		}
		else if ((evol == 1.5f) || (evol == -1.5f)) {														// devolve to dark or light world second (if evol == 1.5)
			if (deltaDark < deltaLight) ToSecond(true);															// if lose more dark than light = to light second
			else if (deltaDark > deltaLight) ToSecond(false);													// if lose more light than dark = to dark second
		}
		else if ((evol >= 2f && evol < 3f) || (evol <= -2f && evol > -3f)) {								// devolve to dark or light world third (if evol = 2)
			if (deltaDark < deltaLight) ToThird(true);															// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);													// if lose more light than dark = to dark third
		}
		else if (evol >= 5f && evol < 8f) {																	// evolve to dark world fifth (if evol >= 5 and < 8)
			if (!light && (deltaDark > deltaLight)) ToFifth(false, 0);											// if dark & gain more dark than light = to dark circle fifth
			else if (!light && (deltaDark < deltaLight)) ToFifth(true, 0);										// if dark & gain more light than dark = to light circle fifth
			else if (light && (deltaDark > deltaLight)) ToFifth(true, 1);										// if light & gain more dark than light = to triangle fifth
			else if (light && (deltaDark < deltaLight)) ToFifth(true, 2);										// if light & gain more light than dark = to square fifth
		}
		else if (evol <= -5f && evol > -8f) {																// devolve to light world fifth (if evol = -5)
			if (deltaDark < deltaLight) ToFifth(true, 0);														// if lose more dark than light = to light circle fifth
			else if (deltaDark > deltaLight) ToFifth(false, 0);													// if lose more light than dark = to dark circle fifth
		}
		else if (evol <= -8f && evol > -13f) {																// devolve to light world sixth (if evol = -8)
			if (deltaDark < deltaLight) ToSixth(true, 0);														// if lose more dark than light = to light circle sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);													// if lose more dark than light = to dark circle sixth
		}
		else if (evol <= -13f && evol > -21f) {																// devolve to light world seventh (if evol = -13)
			if (deltaDark < deltaLight) ToSeventh(true, 0);														// if lose more dark than light = to light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);												// if lose more dark than light = to dark circle seventh
		}
		/*else if (evol <= -21f && evol > -34f) {															// devolve to light world eighth (if evol = -21)
			if (deltaDark < deltaLight) ToEighth(true, 0);														// if lose more dark than light = to light circle eighth
			else if (deltaDark > deltaLight) ToEighth(false, 0);												// if lose more dark than light = to dark circle eighth
		}*/
	}
}
