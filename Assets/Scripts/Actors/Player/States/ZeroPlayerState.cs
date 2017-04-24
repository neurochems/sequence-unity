using UnityEngine;
using System.Collections;

public class ZeroPlayerState : IParticleState
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	// constructor
	public ZeroPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// called every frame in PlayerStatePattern
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
			= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern

		if (canCollide) {																// if collision allowed

			if (other.gameObject.CompareTag ("Zero")) {										// if collide with zero
				psp.stunned = true;																// stunned flag
				if (pspOther.light) psp.AddLight (pspOther.lightEvol);							// if light, add light of other
				if (!pspOther.light) psp.AddDark (pspOther.darkEvol);							// if dark, add dark of other
				Debug.Log ("Player deltaDark on collision: " + psp.deltaDark);
				Debug.Log ("Player deltaLight on collision: " + psp.deltaLight);
				canCollide = false;																// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag("First") 									// collide with first
				|| other.gameObject.CompareTag("Second")									// collide with second
				|| other.gameObject.CompareTag("Third")										// collide with third
				|| other.gameObject.CompareTag("Fourth")									// collide with fourth
				|| other.gameObject.CompareTag("Fifth")										// collide with fifth
				|| other.gameObject.CompareTag("Sixth")										// collide with sixth
				|| other.gameObject.CompareTag("Seventh")									// collide with seventh
				|| other.gameObject.CompareTag("Eighth")									// collide with eighth
				|| other.gameObject.CompareTag("Ninth"))									// collide with ninth
			{									
				psp.stunned = true;																// set stunned flag
				psp.SubDark (pspOther.darkEvol);												// subtract other dark
				psp.SubLight (pspOther.lightEvol);												// subtract other light
				canCollide = false;																// reset has collided trigger
			}
		}
	}

	public void ToZero(bool toLight)
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToHalfZero(bool toLight)
	{
		psp.TransitionTo(0, 0, light, toLight, 0);
		//ParticleStateEvents.toLightZero += psp.TransitionToLightZero;
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(0, 1, light, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;						// flag transition in delegate
		psp.currentState = psp.firstState;											// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(0, 2, light, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;						// flag transition in delegate
		psp.currentState = psp.secondState;											// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(0, 3, light, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;						// flag transition in delegate
		psp.currentState = psp.thirdState;											// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(0, 4, light, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;						// flag transition in delegate
		psp.currentState = psp.fourthState;											// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 5, light, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;											// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 6, light, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;											// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(0, 7, light, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;					// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new stateebug.Log ("Can't transition to same state");
	}

	public void Evol()									// all states here for init \\
	{
		evol = psp.evol;																// local evol check			
		light = psp.light;																// update light value
		deltaDark = psp.deltaDark;														// local dark check
		deltaLight = psp.deltaLight;													// local light check

		if (!psp.lightworld	&& evol < 0f) psp.lightworld = true;						// if to light world (if evol < 0), set light world flag
		else if (psp.lightworld && evol >= 0f) psp.lightworld = false;					// if to dark world, reset light world flag

		else if (evol == 0.5f || evol == -0.5f) {										// evolve to half zero (if evol = 0.5)
			if (deltaDark == 0.5f || deltaDark == -0.5f) ToHalfZero (false);				// if gain dark = to dark zero
			//else if (deltaLight == 0.5f || deltaLight == -0.5f) ToHalfZero (true);			// if gain light = to light zero
		}
		else if (evol == 1f) {															// evolve to dark world first (if evol == 1)
			if (deltaDark > deltaLight) ToFirst(false);										// if gain more dark than light = to dark first
			else if (deltaDark < deltaLight) ToFirst(true);									// if gain more light than dark = to light first
		}
		else if (evol == -1f) {															// devolve to light world first (if evol == -1)
			if (deltaDark < deltaLight) ToFirst(true);										// if lose more dark than light = to light first
			else if (deltaDark > deltaLight) ToFirst(false);								// if lose more light than dark = to dark first
		}
		else if (evol == 1.5f) {														// init to dark world second (if evol == 1.5)
			ToSecond (true);																// to light second
		}
		else if (evol == -1.5f) {														// devolve to light world second (if evol == -1.5)
			if (deltaDark < deltaLight) ToSecond(true);										// if lose more dark than light = to light second
			else if (deltaDark > deltaLight) ToSecond(false);								// if lose more light than dark = to dark second
		}
		else if (evol == 2f) {															// init to dark world third (if evol == 2)
			ToThird (true);																	// to light third
		}
		else if (evol <= -2f && evol > -3f) {											// devolve to light world third (if evol == -2)
			if (deltaDark < deltaLight) ToThird(true);										// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);								// if lose more light than dark = to dark third
		}
		else if (evol == 3f) {															// init to dark world fourth (if evol == 3)
			int i = Random.Range(0,1);														// random 0 or 1
			if (i == 0) ToFourth (false);													// to dark fourth
			else ToFourth (true);															// to light fourth
		}
		else if (evol <= -3f && evol > -5f) {											// devolve to light world fourth (if evol == -3)
			if (deltaDark < deltaLight) ToFourth(true);										// if lose more dark than light = to light fourth
			else if (deltaDark > deltaLight) ToFourth(false);								// if lose more light than dark = to dark fourth
		}
		else if (evol == 5f) {															// init to dark world fifth (if evol == 5)
			int i = Random.Range(0,2);														// random 0 or 1 or 2
			if (i == 0) ToFifth (true, 0);													// to light circle fifth
			else if (i == 1) ToFifth (true, 1);												// to light triangle fifth
			else if (i == 2) ToFifth (true, 2);												// to light square fifth
		}
		else if (evol <= -5f && evol > -8f) {											// devolve to light world fifth (if evol == -5)
			if (deltaDark < deltaLight) ToFifth(true, 0);									// if lose more dark than light = to light circle fifth
			else if (deltaDark > deltaLight) ToFifth(false, 0);								// if lose more light than dark = to dark circle fifth
		}
		else if (evol == 8f) {															// init to dark world sixth (if evol == 8)
			int i = Random.Range(0,2);														// random 0 or 1 or 2
			if (i == 0) ToSixth (true, 0);													// to light circle sixth
			else if (i == 1) ToSixth (false, 1);											// to dark triangle sixth
			else if (i == 2) ToSixth (false, 2);											// to dark square sixth
		}
		else if (evol <= -8f && evol > -13f) {											// devolve to light world sixth (if evol == -8)
			if (deltaDark < deltaLight) ToSixth(true, 0);									// if lose more dark than light = to light circle sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);								// if lose more light than dark = to dark circle sixth
		}
		else if (evol == 13f) {															// init to dark world seventh (if evol == 13)
			int i = Random.Range(0,2);														// random 0 or 1 or 2
			if (i == 0) ToSeventh (true, 0);												// to light circle seventh
			else if (i == 1) ToSeventh (true, 1);											// to light triangle seventh
			else if (i == 2) ToSeventh (true, 2);											// to light square seventh
		}
		else if (evol <= -13f && evol > -21f) {											// devolve to light world seventh (if evol == -13)
			if (deltaDark < deltaLight) ToSeventh(true, 0);									// if lose more dark than light = to light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);							// if lose more light than dark = to dark circle seventh
		}
	}
}
