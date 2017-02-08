using UnityEngine;
using System.Collections;

public class ZeroPlayerState : IParticleState
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	//public float darkEvolDelta, lightEvolDelta;	

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
				psp.AddDark (pspOther.darkEvol);												// add dark of other
				psp.AddLight (pspOther.lightEvol);												// add light of other
				canCollide = false;																// reset has collided trigger
			} 
			else {																			// collide with any other
				psp.stunned = true;																// set stunned flag
				psp.SubDark (pspOther.darkEvol);												// subtract other dark
				psp.SubLight (pspOther.lightEvol);												// subtract other light
				canCollide = false;																// reset has collided trigger
			}
		}
	}

	public void Death()
	{
		psp.TransitionToDead(0, psp.self);										// trigger transition effects
		ParticleStateEvents.toDead += psp.TransitionToDead;						// flag transition in delegate
		psp.currentState = psp.deadState;										// set to new state
	}

	public void ToZero()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToLightZero(bool light)
	{
		// psp.TransitionToZero(true, 0, psp.self);
		ParticleStateEvents.toLightZero += psp.TransitionToLightZero;
	}

	public void ToDarkZero(bool light)
	{
		psp.TransitionToZero(light, 0, psp.self);
		ParticleStateEvents.toDarkZero += psp.TransitionToDarkZero;
	}

	public void ToFirst(bool light)
	{
		psp.TransitionToFirst(light, 0, psp.self);									// trigger transition effects
		ParticleStateEvents.toFirst += psp.TransitionToFirst;						// flag transition in delegate
		psp.currentState = psp.firstState;											// set to new state
	}

	public void ToSecond(bool light)
	{
		psp.TransitionToSecond(light, 0, psp.self);									// trigger transition effects
		ParticleStateEvents.toSecond += psp.TransitionToSecond;						// flag transition in delegate
		psp.currentState = psp.secondState;											// set to new state
	}

	public void ToThird(bool light)
	{
		psp.TransitionToThird(light, 0, psp.self);									// trigger transition effects
		ParticleStateEvents.toThird += psp.TransitionToThird;						// flag transition in delegate
		psp.currentState = psp.thirdState;											// set to new state
	}

	public void ToFourth(bool light)
	{
		psp.TransitionToFourth(light, 0, psp.self);									// trigger transition effects
		ParticleStateEvents.toFourth += psp.TransitionToFourth;						// flag transition in delegate
		psp.currentState = psp.fourthState;											// set to new state
	}

	public void ToFifth(bool light)
	{
		psp.TransitionToFifth(light, 0, psp.self);									// trigger transition effects
		ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;											// set to new state
	}

	public void ToSixth(bool light)
	{
		psp.TransitionToSixth(light, 0, psp.self);									// trigger transition effects
		ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;											// set to new state
	}

	public void ToSeventh(bool light)
	{
		psp.TransitionToSeventh(light, 0, psp.self);								// trigger transition effects
		ParticleStateEvents.toSeventh += psp.TransitionToSeventh;					// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new stateebug.Log ("Can't transition to same state");
	}

	public void Evol()									// all states here for init \\
	{
		float deltaDark = psp.darkEvolDelta;										// local dark check
		float deltaLight = psp.lightEvolDelta;										// local light check

		if (psp.evol < 0f) {														// to dead (if evol < 0)
			Death ();																	// change to dead state
		}
		else if (psp.evol == 0.5f) {												// to zero (if evol = 0.5)
			if (deltaDark == 0.5f) ToDarkZero (false);									// if gain dark = to dark zero
			else if (deltaLight == 0.5f) ToLightZero (true);							// if gain light = to light zero
		}
		else if (psp.evol == 1f) {													// to first (if evol == 1)
			if (!light && deltaDark == 0.5) ToFirst (false);							// if dark & gain dark = to dark first
			else if (!light && deltaLight == 0.5) ToFirst(true);						// if dark & gain light = to light first
			else if (light && deltaDark == 0.5) ToFirst(false);							// if light & gain dark = to dark first
			else if (light && deltaLight == 0.5) ToFirst(true);							// if light & gain light = to light first
		}
		else if (psp.evol == 1.5f) {												// to second (if evol == 1.5)
			ToSecond (true);															// evolve to second
		}
		else if (psp.evol == 2f) {													// to third (if evol == 2)
			ToThird (true);																// evolve to third
		}
		else if (psp.evol == 3f) {													// to fourth (if evol == 3)
			int i = Random.Range(0,1);													// random 0 or 1
			if (i == 0) ToFourth (false);												// evolve to dark fourth
			else ToFourth (true);														// evolve to light fourth
		}
		else if (psp.evol == 5f) {													// to fifth (if evol == 5)
			int i = Random.Range(0,2);													// random 0 or 1 or 2
			if (i == 0) ToFifthCircle (true);											// evolve to circle fifth
			else if (i == 1) ToFifthTriangle (true);									// evolve to triangle fifth
			else if (i == 2) ToFifthSquare (true);										// evolve to square fifth
		}
		else if (psp.evol == 8f) {													// to sixth (if evol == 8)
			int i = Random.Range(0,2);													// random 0 or 1 or 2
			if (i == 0) ToSixthCircle (true);											// evolve to circle sixth
			else if (i == 1) ToSixthTriangle (true);									// evolve to triangle sixth
			else if (i == 2) ToSixthSquare (true);									// evolve to square sixth
		}
		else if (psp.evol == 13f) {													// to seventh (if evol == 13)
			int i = Random.Range(0,2);													// random 0 or 1 or 2
			if (i == 0) ToSeventhCircle (true);											// evolve to circle seventh
			else if (i == 1) ToSevenhTriangle (true);									// evolve to triangle seventh
			else if (i == 2) ToSeventhSquare (true);									// evolve to square seventh
		}
	}
}
