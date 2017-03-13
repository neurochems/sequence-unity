using UnityEngine;
using System.Collections;

public class FirstParticleState : IParticleState
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset can collide timer

	public FirstParticleState (ParticleStatePattern statePatternParticle)				// constructor
	{
		psp = statePatternParticle;															// attach state pattern to this state 
	}

	public void UpdateState()
	{
		Evol ();

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
			= other.gameObject.GetComponent<ParticleStatePattern>();					// ref other ParticleStatePattern																							// roll die trigger

		if (canCollide) {																	// if collision allowed
			
			if (other.gameObject.CompareTag ("Player")) {										// colide with player
				psp.stunned = true;																// stun for duration
				canCollide = false;																// reset can collide trigger	
				Debug.Log ("particle contact player");
			}
			else if (other.gameObject.CompareTag ("Zero")									// collide with zero
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

	public void Death(bool toLight)
	{
		psp.TransitionTo(1, -1, light, toLight, 0);								// trigger transition effects
		//ParticleStateEvents.toDead += particle.TransitionToDead;					// flag transition in delegate
		psp.SpawnZero(1);														// spawn 2 photons
		psp.currentState = psp.deadState;										// set to new state
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
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;				// flag transition in delegate
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
		float deltaDark = psp.deltaDark;										// local dark check
		float deltaLight = psp.deltaLight;										// local light check

		if (psp.evol <= 0f) {														// to dead (if evol < 0)
			Death (true);																	// to dead state
		} 
		else if (psp.evol == 0) {													// to zero (if evol = 0)
			ToZero (true);																	// to zero state
		}
		else if (psp.evol == 0.5f) {												// to zero (if evol = 0.5)
			if (deltaDark == -0.5f) ToZero (false);										// if gain dark = to dark zero
			else if (deltaLight == -0.5f) ToZero (true);								// if gain light = to light zero
		}
		else if (psp.evol >= 1.5f) {												// to second (if evol = 1.5)
			if (!light && deltaDark == 0.5) ToSecond(false);							// if dark & gain dark = to dark
			else if (!light && deltaLight == 0.5) ToSecond(true);						// if dark & gain light = to light
			else if (light && deltaDark == 0.5) ToSecond(false);						// if light & gain dark = to dark
			else if (light && deltaLight == 0.5) ToSecond(true);						// if light & gain light = to light
		}
	}
}
