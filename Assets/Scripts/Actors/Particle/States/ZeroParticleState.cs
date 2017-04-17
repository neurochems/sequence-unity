using UnityEngine;
using System.Collections;

public class ZeroParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	//public float darkEvolDelta, lightEvolDelta;	

	private bool canCollide = false;													// can collide flag (false to stun on new spawn)
	private float collisionTimer;														// reset can collide timer	

	// constructor
	public ZeroParticleState (ParticleStatePattern statePatternParticle)				
	{
		psp = statePatternParticle;														// attach state pattern to this state 
	}

	public void UpdateState()
	{
		Evol ();

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

		if (canCollide) {																								// if collision allowed
			if (other.gameObject.CompareTag ("Player")) {									// colide with player
				psp.stunned = true;																// stunned flag
				psp.SubDark (other.gameObject.GetComponent<PlayerStatePattern>().darkEvol);		// subtract other dark
				psp.SubLight (other.gameObject.GetComponent<PlayerStatePattern>().lightEvol);	// subtract other light
				canCollide = false;																								// reset can collide trigger	
				Debug.Log ("particle contact player");
			} 
			else if (other.gameObject.CompareTag ("Zero")) {								// if collide with zero
				psp.stunned = true;																// stunned flag
				psp.AddDark (pspOther.darkEvol);												// add dark of other
				psp.AddLight (pspOther.lightEvol);												// add light of other
				canCollide = false;																// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag("First") 										// collide with first
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

	public void Death(bool toLight)
	{
		psp.TransitionTo(0, -1, light, light, 0);										// trigger transition effects
		//ParticleStateEvents.toDead += psp.TransitionToDead;								// flag transition in delegate
		psp.currentState = psp.deadState;												// set to new state		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
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
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;							// flag transition in delegate
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
		//ParticleStateEvents.toThird += psp.TransitionToThird;							// flag transition in delegate
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
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;							// flag transition in delegate
		psp.currentState = psp.fifthState;											// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 6, light, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;							// flag transition in delegate
		psp.currentState = psp.sixthState;											// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(0, 7, light, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;						// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new stateebug.Log ("Can't transition to same state");
	}

	public void Evol()									// all states here for init 
	{
		float deltaDark = psp.deltaDark;										// local dark check
		float deltaLight = psp.deltaLight;										// local light check

		if (psp.evol < 0f) {														// to dead (if evol < 0)
			Death (true);																	// change to dead state
		}
		else if (psp.evol == 0.5f) {												// to zero (if evol = 0.5)
			if (deltaDark == 0.5f) ToHalfZero (false);									// if gain dark = to dark zero
			else if (deltaLight == 0.5f) ToHalfZero (true);							// if gain light = to light zero
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
			if (i == 0) ToFifth (true, 0);												// evolve to circle fifth
			else if (i == 1) ToFifth (true, 1);											// evolve to triangle fifth
			else if (i == 2) ToFifth (true, 2);											// evolve to square fifth
		}
		else if (psp.evol == 8f) {													// to sixth (if evol == 8)
			int i = Random.Range(0,2);													// random 0 or 1 or 2
			if (i == 0) ToSixth (true, 0);												// evolve to circle sixth
			else if (i == 1) ToSixth (true, 1);											// evolve to triangle sixth
			else if (i == 2) ToSixth (true, 2);											// evolve to square sixth
		}
		else if (psp.evol == 13f) {													// to seventh (if evol == 13)
			int i = Random.Range(0,2);													// random 0 or 1 or 2
			if (i == 0) ToSeventh (true, 0);											// evolve to circle seventh
			else if (i == 1) ToSeventh (true, 1);										// evolve to triangle seventh
			else if (i == 2) ToSeventh (true, 2);										// evolve to square seventh
		}
	}
}
