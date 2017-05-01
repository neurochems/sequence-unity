using UnityEngine;
using System.Collections;

public class FirstPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	public FirstPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		Evol ();																		// check evol

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer up
			canCollide = true;																// set collision ability
			psp.stunned = false;															// reset stunned flag
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern pspOther 
			= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern

		if (canCollide) {																// if collision allowed

			if (other.gameObject.CompareTag ("Zero")										// collide with zero
				|| other.gameObject.CompareTag ("First")) {									// collide with first	
				psp.stunned = true;																// stun for duration
				psp.AddDark (pspOther.darkEvol);												// add dark of other
				psp.AddLight (pspOther.lightEvol);												// add light of other
				canCollide = false;																// reset has collided trigger
			}
			else {																			// collide with any other
				psp.stunned = true;																// stun for duration
				psp.SubDark (pspOther.darkEvol);												// subtract other dark
				psp.SubLight (pspOther.lightEvol);												// subtract other light
				canCollide = false;																// reset has collided trigger
			}
		}				
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(1, 0, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;						// flag transition in delegate
		psp.SpawnZero(1);														// spawn 1 zero
		psp.currentState = psp.zeroState;										// set to new state
	}

	public void ToFirst(bool toLight)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(1, 2, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;					// flag transition in delegate
		psp.currentState = psp.secondState;										// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(1, 3, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;						// flag transition in delegate
		psp.currentState = psp.thirdState;										// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(1, 4, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;					// flag transition in delegate
		psp.currentState = psp.fourthState;										// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(1, 5, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;										// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(1, 6, light, toLight, shape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;										// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(1, 7, light, toLight, shape);							// trigger transition effects
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

		else if (evol == 0f) {																				// to zero (if evol = 0)
			ToZero (true);																						// to light zero
		}
		else if (evol == 0.5f) {																			// devolve to dark world zero (if evol = 0.5)
			if (deltaDark > deltaLight) ToZero(false);															// if gain more dark than light = to dark zero
			// else if (deltaDark < deltaLight) ToZero(true);														// if gain more light than dark = to light zero (no change)
		}
		else if (evol == -0.5f) {																			// devolve to light world zero (if evol = -0.5)
			if (deltaDark < deltaLight) ToZero(true);															// if lose more dark than light = to light zero
			else if (deltaDark > deltaLight) ToZero(false);														// if lose more dark than light = to dark zero
		}
		else if (evol >= 1.5f) {																			// evolve to dark world second (if evol = 1.5)
			if (deltaDark > deltaLight) ToSecond(false);														// if gain more dark than light = to dark second
			else if (deltaDark < deltaLight) ToSecond(true);													// if gain more light than dark = to light second
		}
		else if (evol == -1.5f) {																			// devolve to light world second (if evol = -1.5)
			if (deltaDark < deltaLight) ToSecond(true);															// if lose more dark than light = to light second
			else if (deltaDark > deltaLight) ToSecond(false);													// if lose more dark than light = to dark second
		}
		else if (evol <= -2f && evol > -3f) {																// devolve to light world third (if evol = -2)
			if (deltaDark < deltaLight) ToThird(true);															// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);													// if lose more dark than light = to dark third
		}
		else if (evol <= -3f && evol > -5f) {																// devolve to light world fourth (if evol = -3)
			if (deltaDark < deltaLight) ToFourth(true);															// if lose more dark than light = to light fourth
			else if (deltaDark > deltaLight) ToFourth(false);													// if lose more dark than light = to dark fourth
		}
		else if (evol <= -5f && evol > -8f) {																// devolve to light world fifth (if evol = -8)
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
