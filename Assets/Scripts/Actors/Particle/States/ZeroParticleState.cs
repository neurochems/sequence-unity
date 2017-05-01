using UnityEngine;
using System.Collections;

public class ZeroParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool light = true;															// 'is light' flag
	private bool lightworld;															// is light world ref
	public float evol, deltaDark, deltaLight;											// evol tracking refs

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
				Debug.Log ("Particle deltaDark on collision: " + psp.deltaDark);
				Debug.Log ("Particle deltaLight on collision: " + psp.deltaLight);
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

	public void ToOtherWorld(bool toLW, int fromState, int toState, bool toLight)
	{
		psp.ChangeWorld(toLW, fromState, toState, toLight);								// trigger transition effects
		//psp.SpawnZero(1);																	// spawn 1 zero
		if (toState == 1) psp.currentState = psp.firstState;						// set to first state
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
		evol = psp.evol;																	// local evol check			
		lightworld = psp.lightworld;														// local lightworld check
		light = psp.light;																	// update light value
		deltaDark = psp.deltaDark;															// local dark check
		deltaLight = psp.deltaLight;														// local light check

		//if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;							// if to light world (if evol < 0), set light world flag
		//else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;						// if to dark world, reset light world flag

		// zero
		if (evol == 0f && lightworld) {														// evolve to dark world light zero from light world dark zero
			ToOtherWorld (false, 0, 0, true);													// to dark world light zero
		}
		// half zero
		if (evol == 0.5f && !lightworld) {													// evolve to dark world dark zero within dark world
			if (deltaDark > deltaLight) ToHalfZero (false);										// if gain more dark than light = to dark world dark zero
			//else if (deltaDark < deltaLight) ToHalfZero (true);								// if gain more light than dark = to dark world light zero
		}
		else if (evol == -0.5f && !lightworld) {											// devolve to light world dark zero from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 0, 0, true);							// if lose more dark than light = to light world light zero
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 0, false);					// if lose more light than dark = to light world dark zero
		}
		else if (evol == -0.5f && lightworld) {												// devolve to light world dark zero from dark world
			if (deltaDark < deltaLight) ToHalfZero(true);										// if lose more dark than light = to light world light zero
			else if (deltaDark > deltaLight) ToHalfZero(false);									// if lose more light than dark = to light world dark zero
		}
		// first
		if (evol == 1f && !lightworld) {													// evolve to dark world first within dark world
			if (deltaDark > deltaLight) ToFirst(false);											// if gain more dark than light = to dark world dark first
			else if (deltaDark < deltaLight) ToFirst(true);										// if gain more light than dark = to dark world light first
			else if (deltaDark == deltaLight) ToFirst(true);									// if dark and light equal, init to dark world light first
		}
		else if (evol == -1f && !lightworld) {												// devolve to light world first from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 0, 1, true);							// if lose more dark than light = to light world light first
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 1, false);					// if lose more light than dark = to light world dark first
		}
		else if (evol == -1f && lightworld) {												// devolve to light world first within light world
			if (deltaDark < deltaLight) ToFirst(true);											// if lose more dark than light = to light world light first
			else if (deltaDark > deltaLight) ToFirst(false);									// if lose more light than dark = to light world dark first
		}
		// second
		if (evol == 1.5f && !lightworld) {													// init to dark world second (if evol == 1.5)
			ToSecond (true);																	// to light second
		}
		else if (evol == -1.5f && !lightworld) {											// devolve to light world second from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 1, 2, true);							// if lose more dark than light = to light world light second
			else if (deltaDark > deltaLight) ToOtherWorld(true, 1, 2, false);					// if lose more light than dark = to light world dark second
		}
		else if (evol == -1.5f && lightworld) {												// devolve to light world second within light world
			if (deltaDark < deltaLight) ToSecond(true);											// if lose more dark than light = to light world light second
			else if (deltaDark > deltaLight) ToSecond(false);									// if lose more light than dark = to light world dark second
		}
		// third
		if (evol == 2f && !lightworld) {													// init to dark world third
			ToThird (true);																		// to light third
		} 
		else if ((evol <= -2f && evol > -3f) && !lightworld) {								// devolve to light world third from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 0, 3, true);							// if lose more dark than light = to light world light third
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 3, false);					// if lose more light than dark = to light world dark third
		}
		else if ((evol <= -2f && evol > -3f) && lightworld) {								// devolve to light world third within light world
			if (deltaDark < deltaLight) ToThird(true);											// if lose more dark than light = to light world light third
			else if (deltaDark > deltaLight) ToThird(false);									// if lose more light than dark = to light world dark third
		}
		// fourth
		if (evol == 3f && !lightworld) {													// init to dark world fourth
			int i = Random.Range(0,1);															// random 0 or 1
			if (i == 0) ToFourth (false);														// to dark fourth
			else ToFourth (true);																// to light fourth
		} 
		else if ((evol <= -3f && evol > -5f) && !lightworld) {								// devolve to light world fourth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 0, 4, true);							// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 4, false);					// if lose more light than dark = to light world dark fourth
		}
		else if ((evol <= -3f && evol > -5f) && lightworld) {								// devolve to light world fourth within light world
			if (deltaDark < deltaLight) ToFourth(true);											// if lose more dark than light = to light world light fourth
			else if (deltaDark > deltaLight) ToFourth(false);									// if lose more light than dark = to light world dark fourth
		}
		// fifth
		if (evol == 5f && !lightworld) {													// init to dark world fifth
			int i = Random.Range(0,2);															// random 0 or 1 or 2
			if (i == 0) ToFifth (true, 0);														// to light circle fifth
			else if (i == 1) ToFifth (true, 1);													// to light triangle fifth
			else if (i == 2) ToFifth (true, 2);													// to light square fifth
		}
		else if ((evol <= -5f && evol > -8f) && !lightworld) {								// devolve to light world fifth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 0, 5, true);							// if lose more dark than light = to light world light fifth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 5, false);					// if lose more light than dark = to light world dark fifth
		}
		else if ((evol <= -5f && evol > -8f) && lightworld) {								// devolve to light world fifth within light world
			if (deltaDark < deltaLight) ToFifth(true, 0);										// if lose more dark than light = to light world light fifth
			else if (deltaDark > deltaLight) ToFifth(false, 0);									// if lose more light than dark = to light world dark fifth
		}
		// sixth
		if (evol == 8f && !lightworld) {													// init to dark world sixth
			int i = Random.Range(0,2);															// random 0 or 1 or 2
			if (i == 0) ToSixth (true, 0);														// to light circle sixth
			else if (i == 1) ToSixth (false, 1);												// to dark triangle sixth
			else if (i == 2) ToSixth (false, 2);												// to dark square sixth
		}
		else if ((evol <= -8f && evol > -13f) && !lightworld) {								// devolve to light world sixth from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 0, 6, true);							// if lose more dark than light = to light world light sixth
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 6, false);					// if lose more light than dark = to light world dark sixth
		}
		else if ((evol <= -8f && evol > -13f) && lightworld) {								// devolve to light world sixth within light world
			if (deltaDark < deltaLight) ToSixth(true, 0);										// if lose more dark than light = to light world light sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);									// if lose more light than dark = to light world dark sixth
		}
		// seventh
		if (evol == 13f && !lightworld) {													// init to dark world seventh
			int i = Random.Range(0,2);															// random 0 or 1 or 2
			if (i == 0) ToSeventh (true, 0);													// to light circle seventh
			else if (i == 1) ToSeventh (true, 1);												// to light triangle seventh
			else if (i == 2) ToSeventh (true, 2);												// to light square seventh
		}
		else if ((evol <= -13f && evol > -21f) && !lightworld) {							// devolve to light world seventh from dark world
			if (deltaDark < deltaLight) ToOtherWorld(true, 0, 7, true);							// if lose more dark than light = to light world light seventh
			else if (deltaDark > deltaLight) ToOtherWorld(true, 0, 7, false);					// if lose more light than dark = to light world dark seventh
		}
		else if ((evol <= -13f && evol > -21f) && lightworld) {								// devolve to light world seventh within light world
			if (deltaDark < deltaLight) ToSeventh(true, 0);										// if lose more dark than light = to light world light seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);								// if lose more light than dark = to light world dark seventh
		}
	}
}
