using UnityEngine;
using System.Collections;

public class NinthParticleState : IParticleState 
{

	private readonly ParticleStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	private bool lightworld, inLightworld;												// is light world ref, in light world ref
	private int shape;																	// shape indicator
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false, takeHit = false;									// can collide flag (init false to begin stunned), take hit flag (delay flagging stunned to prevent collision logic conflicts)
	private float collisionTimer, takeHitTimer;											// reset collision timer, take hit timer

	public int die;																		// collision conflict resolution
	private bool rolling = false;														// is rolling flag

	public NinthParticleState (ParticleStatePattern particleStatePattern)				// constructor
	{
		psp = particleStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			//Debug.Log("check particle evol");
			Evol();																		// check evol logic
			checkEvol = false;															// reset check evol flag
		}

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= psp.stunDuration) {										// if timer is up
			canCollide = true;																// set collision ability
			psp.sc[0].enabled = true;														// enable trigger collider
			psp.stunned = false;															// update stunned to false
			collisionTimer = 0f;															// reset collision timer
		}
		// take hit flag timer
		if (takeHit) takeHitTimer += Time.deltaTime;									// start timer
		if (takeHitTimer >= 0.2f) {														// if timer is up
			psp.stunned = true;																// set stunned flag
			takeHit = false;																// reset take hit trigger
			takeHitTimer = 0f;																// reset take hit timer
		}

	}

	public void OnTriggerEnter(Collider other)
	{
		// state class doesn't know what shape it is until it contacts another
		shape = psp.shape;																		// set shape indicator
		circle = psp.circle;																	// set current circle flag
		triangle = psp.triangle;																// set current triangle flag
		square = psp.square;																	// set current square flag

		if (canCollide) {																// if collision allowed and player is not stunned
			if (other.gameObject.CompareTag ("Player") && psp.psp.canCollide) {				// colide with collidable player
				PlayerStatePattern pspOther 
				= other.gameObject.GetComponent<PlayerStatePattern>();								// ref other ParticleStatePattern
				if (!pspOther.stunned && (pspOther.lightworld == psp.inLightworld)) {				// if particle and not stunned player in same world
					if (pspOther.state != 9 && (psp.evolC > pspOther.evolC)) {							// if player evol is lower
						if (pspOther.evolC > 0f) {															// other > 0
							if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);						// add other dark
							if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);					// add other light
						}
						else if (pspOther.evolC < 0f) {														// other < 0
							if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);				// add other negated dark
							if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);				// add other negated light
						}
					} 
					else if (pspOther.state != 9 && (psp.evolC <= pspOther.evolC)) {					// else player is higher
						if (pspOther.evolC > 0f) {															// other > 0
							if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);						// sub other dark
							if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);					// sub other light
						}
						else if (pspOther.evolC < 0f) {														// other < 0
							if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);				// sub other negated dark
							if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);				// sub other negated light
						}
					}
					canCollide = false;																// reset can collide trigger	
					psp.sc[0].enabled = false;														// disable trigger collider
					takeHit = true;																	// set take hit flag
					checkEvol = true;																// check evol flag
				}
				pspOther = null;																// clear pspOther
			}
			else if (other.gameObject.CompareTag ("Zero")									// collide with zero
				|| other.gameObject.CompareTag ("First")									// collide with first
				|| other.gameObject.CompareTag ("Second")									// collide with second
				|| other.gameObject.CompareTag ("Third")									// collide with third
				|| other.gameObject.CompareTag ("Fourth")									// collide with fourth
				|| other.gameObject.CompareTag ("Fifth")									// collide with fifth
				|| other.gameObject.CompareTag ("Sixth") 									// collide with sixth
				|| other.gameObject.CompareTag ("Seventh") 									// collide with seventh
				|| other.gameObject.CompareTag ("Eighth")) {								// collide with eighth
				ParticleStatePattern pspOther 
				= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (!pspOther.stunned && (pspOther.inLightworld == psp.inLightworld)) {			// if particle and not stunned particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc[0].enabled = false;														// disable trigger collider
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
					takeHit = true;																	// set take hit flag
					checkEvol = true;																// check evol flag
				}
				pspOther = null;																// clear pspOther
			}
			else if (other.gameObject.CompareTag ("Ninth")) {								// collide with seventh
				ParticleStatePattern pspOther 
				= other.gameObject.GetComponent<ParticleStatePattern>();						// ref other ParticleStatePattern
				if (!pspOther.stunned && (pspOther.inLightworld == psp.inLightworld)) {			// if particle and not stunned particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc[0].enabled = false;														// disable trigger collider
					if (psp.evolC > pspOther.evolC) {													// if evol > other
						if (pspOther.evolC > 0f) {															// other > 0
							if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);						// add other dark
							if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);					// add other light
						}
						else if (pspOther.evolC < 0f) {														// other < 0
							if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);				// add other negated dark
							if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);				// add other negated light
						}
					}
					else if (psp.evolC == pspOther.evolC) RollDie (pspOther);							// if evol = other, roll die
					if (psp.evolC < pspOther.evolC) {													// if evol < other
						if (pspOther.evolC > 0f) {															// other > 0
							if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);						// sub other dark
							if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);					// sub other light
						}
						else if (pspOther.evolC < 0f) {														// other < 0
							if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);				// sub other negated dark
							if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);				// sub other negated light
						}
					}
					takeHit = true;																	// set take hit flag
					checkEvol = true;																// check evol flag
				}
				pspOther = null;																// clear pspOther
			}
		}
	}

	private void RollDie(ParticleStatePattern pspOther) {
		if (psp.die > pspOther.die) {														// if this die > other die
			//Debug.Log ("die roll: this > other");
			if (pspOther.evolC > 0f) {															// other > 0
				if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC);						// add other dark
				if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC);					// add other light
			}
			else if (pspOther.evolC < 0f) {														// other < 0
				if (pspOther.darkEvolC != 0f) psp.AddDark (pspOther.darkEvolC * -1);				// add other negated dark
				if (pspOther.lightEvolC != 0f) psp.AddLight (pspOther.lightEvolC * -1);				// add other negated light
			}
			psp.roll = true;																	// re-roll die
		}
		else if (psp.die < pspOther.die) {													// if this die < other die
			//Debug.Log ("die roll: this < other");
			if (pspOther.evolC > 0f) {															// other > 0
				if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);						// sub other dark
				if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);					// sub other light
			}
			else if (pspOther.evolC < 0f) {														// other < 0
				if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);				// sub other negated dark
				if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);				// sub other negated light
			}
			psp.roll = true;																	// re-roll die
		}
		else if (psp.die == pspOther.die) {													// if die are same
			psp.roll = true;																	// re-roll die
			// do nothing - cancelled out!
		}
	}

	public void ToOtherWorld(bool toLW, int tst8, bool tl, int ts)
	{
		if (tst8 == 0) psp.currentState = psp.zeroState;								// set to zero state
		else if (tst8 == 1) psp.currentState = psp.firstState;							// set to first state
		else if (tst8 == 2) psp.currentState = psp.secondState;							// set to second state
		else if (tst8 == 3) psp.currentState = psp.thirdState;							// set to third state
		else if (tst8 == 4) psp.currentState = psp.fourthState;							// set to fourth state
		else if (tst8 == 5) psp.currentState = psp.fifthState;							// set to fifth state
		else if (tst8 == 6) psp.currentState = psp.sixthState;							// set to sixth state
		else if (tst8 == 7) psp.currentState = psp.seventhState;						// set to seventh state

		psp.ChangeWorld(toLW, 9, tst8, tl, shape, ts);									// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;								// flag transition in delegate
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(8, 0, isLight, toLight, shape, 0);								// trigger transition effects
		//ParticleStateEvents.toZero += psp.TransitionToZero;								// flag transition in delegate
		psp.currentState = psp.zeroState;												// set to new state
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(8, 1, isLight, toLight, shape, 0);								// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;								// flag transition in delegate
		psp.currentState = psp.firstState;												// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(8, 2, isLight, toLight, shape, 0);								// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;							// flag transition in delegate
		psp.currentState = psp.secondState;												// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(8, 3, isLight, toLight, shape, 0);								// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;								// flag transition in delegate
		psp.currentState = psp.thirdState;												// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(8, 4, isLight, toLight, shape, 0);								// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;							// flag transition in delegate
		psp.currentState = psp.fourthState;												// set to new state
	}

	public void ToFifth(bool toLight, int toShape)
	{
		psp.TransitionTo(8, 5, isLight, toLight, shape, toShape);						// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;								// flag transition in delegate
		psp.currentState = psp.fifthState;												// set to new state
	}

	public void ToSixth(bool toLight, int toShape)
	{
		psp.TransitionTo(8, 6, isLight, toLight, shape, toShape);						// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;								// flag transition in delegate
		psp.currentState = psp.sixthState;												// set to new state
	}

	public void ToSeventh(bool toLight, int toShape)
	{
		psp.TransitionTo(8, 7, isLight, toLight, shape, toShape);						// trigger transition effects
		//ParticleStateEvents.toEighth += psp.TransitionToEighth;							// flag transition in delegate
		psp.currentState = psp.seventhState;											// set to new state
	}

	public void ToEighth(bool toLight, int toShape)
	{
		psp.TransitionTo(9, 8, isLight, toLight, shape, toShape);						// trigger transition effects
		//ParticleStateEvents.toEighth += psp.TransitionToEighth;							// flag transition in delegate
		psp.currentState = psp.eighthState;												// set to new state
	}

	public void ToNinth(bool toLight, int toShape)
	{
		psp.TransitionTo(9, 9, isLight, toLight, shape, toShape);						// trigger transition effects
	}

	public void ToTenth(bool toLight, int toShape)
	{
		psp.TransitionTo(9, 10, isLight, toLight, shape, toShape);						// trigger transition effects
	}

	public void Evol()
	{
		evol = psp.evol;																	// local evol check			
		lightworld = psp.lightworld;														// local lightworld check
		inLightworld = psp.inLightworld;													// local inlightworld check
		isLight = psp.isLight;																// update light value
		deltaDark = psp.deltaDark;															// local dark check
		deltaLight = psp.deltaLight;														// local light check

		// zero
			// in dark world
		if (evol == 0f && !inLightworld) {													// to dark world light zero / from dark world
			ToZero (true); 																		// to dark world light zero
		}
			// to dark world
		else if (evol == 0f && inLightworld) {												// to dark world zero / from light world
			ToOtherWorld(false, 0, true, 0);													// to dark world light zero
		}

		// half zero
			// in dark world
		if (evol == 0.5f && !inLightworld) {												// to dark world zero / from dark world
			if (deltaDark > deltaLight) ToZero(false);											// if lose more light than dark = to dark world dark zero
			else if (deltaDark <= deltaLight) ToZero(true);										// if lose more dark than light = to dark world light zero
		}
			// to light world
		else if (evol == -0.5f && !inLightworld) {											// to light world zero / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 0, true, 0);							// if lose more light than dark = to light world dark zero
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 0, true, 0);					// if lose more dark than light = to light world light zero
		}
			// in light world
		else if (evol == -0.5f && inLightworld) {											// to light world zero / from light world 
			if (deltaDark > deltaLight) ToZero(false);											// if lose more light than dark, to light world dark zero
			else if (deltaDark <= deltaLight) ToZero(true);										// if lose more dark than light, to light world light zero
		}
			// to dark world
		else if ((evol == 0.5f) && inLightworld) {											// to dark world zero / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 0, false, 0);						// if lose more light than dark = to dark world dark zero
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 0, true, 0);					// if lose more dark than light = to dark world light zero
		}

		// first
			// in dark world
		if (evol == 1f && !inLightworld) {													// to dark world first / from dark world
			if (deltaDark > deltaLight) ToFirst(false);											// if lose more light than dark = to dark world dark first
			else if (deltaDark <= deltaLight) ToFirst(true);									// if lose more dark than light = to dark world light first
		}
			// to light world
		else if (evol == -1f && !inLightworld) {											// to light world first / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 1, false, 0);						// if lose more light than dark = to light world dark first
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 1, true, 0);					// if lose more dark than light = to light world light first
		}
			// in light world
		else if (evol == -1f && inLightworld) {												// to light world first / from light world
			if (deltaDark > deltaLight) ToFirst(false);											// if lose more light than dark = to light world dark first
			else if (deltaDark <= deltaLight) ToFirst(true);									// if lose more dark than light = to light world light first
		}
			// to dark world
		else if ((evol == 1f) && inLightworld) {											// to dark world first / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 1, false, 0);						// if lose more light than dark = to dark world dark first
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 1, true, 0);					// if lose more dark than light = to dark world light first
		}

		// second
			// in dark world
		if (evol == 1.5f && !inLightworld) {												// to dark world second / from dark world
			if (deltaDark > deltaLight) ToSecond(false);										// if lose more light than dark = to dark world dark second
			else if (deltaDark <= deltaLight) ToSecond(true);									// if lose more dark than light = to dark world light second
		}
			// to light world
		else if (evol == -1.5f && !inLightworld) {											// to light world second / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 2, false, 0);						// if lose more light than dark = to light world dark second
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 2, true, 0);					// if lose more dark than light = to light world light second
		}
			// in light world
		else if (evol == -1.5f && inLightworld) {											// to light world second / from light world
			if (deltaDark > deltaLight) ToSecond(false);										// if lose more light than dark = to light world light second
			else if (deltaDark <= deltaLight) ToSecond(true);									// if lose more dark than light = to light world dark second
		}
			// to dark world
		else if ((evol == 1.5f) && inLightworld) {											// to dark world second / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 2, false, 0);						// if lose more light than dark = to dark world dark second
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 2, true, 0);					// if lose more dark than light = to dark world light second
		}

		// third
			// in dark world
		if ((evol >= 2f && evol < 3f) && !inLightworld) {									// to dark world third / from dark world
			if (deltaDark > deltaLight) ToThird(false);											// if lose more light than dark = to dark world dark third
			else if (deltaDark <= deltaLight) ToThird(true);									// if lose more dark than light = to dark world light third
		}
			// to light world
		else if ((evol <= -2f && evol > -3f) && !inLightworld) {							// to light world third / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 3, false, 0);						// if lose more light than dark = to light world dark third
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 3, true, 0);					// if lose more dark than light = to light world light third
		}
			// in light world
		else if ((evol <= -2f && evol > -3f) && inLightworld) {								// to light world third / from light world
			if (deltaDark > deltaLight) ToThird(false);											// if lose more light than dark = to light world dark third
			else if (deltaDark <= deltaLight) ToThird(true);									// if lose more dark than light = to light world light third
		}
			// to dark world
		else if ((evol >= 2f && evol < 3f) && inLightworld) {								// to dark world third / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 3, false, 0);						// if lose more light than dark = to dark world dark third
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 3, true, 0);					// if lose more dark than light = to dark world light third
		}

		// fourth
			// in dark world
		if ((evol >= 3f && evol < 5f) && !inLightworld) {									// to dark world fourth / from dark world
			if (circle && (deltaDark > deltaLight)) ToFourth(false);							// if circle & lose more light than dark = to dark fourth
			else if (circle && (deltaDark <= deltaLight)) ToFourth(false);						// if circle & lose more dark than light = to dark fourth
			else if (triangle && (deltaDark > deltaLight)) ToFourth(true);						// if triangle & lose more light than dark = to light fourth
			else if (triangle && (deltaDark <= deltaLight)) ToFourth(true);						// if triangle & lose more dark than light = to light fourth
			else if (square && (deltaDark > deltaLight)) ToFourth(true);						// if square & lose more light than dark = to light fourth
			else if (square && (deltaDark <= deltaLight)) ToFourth(true);						// if square & lose more dark than light = to light fourth
		}
			// to light world
		else if ((evol <= -3f && evol > -5f) && !inLightworld) {							// to light world fourth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 4, false, 0);						// if lose more light than dark = to light world dark fourth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 4, true, 0);					// if lose more dark than light = to light world light fourth
		}
			// in light world
		else if ((evol <= -3f && evol > -5f) && inLightworld) {								// to light world fourth / from light world
			if (deltaDark > deltaLight) ToFourth(false);										// if lose more light than dark = to lgith world dark fourth
			else if (deltaDark <= deltaLight) ToFourth(true);									// if lose more dark than light = to light world light fourth
		}
			// to dark world
		else if ((evol >= 3f && evol < 5f) && inLightworld) {								// to dark world fourth / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 4, false, 0);						// if lose more light than dark = to dark world dark fourth
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 4, true, 0);					// if lose more dark than light = to dark world light fourth
		}

		// fifth
			// in dark world
		if ((evol >= 5f && evol < 8f) && !inLightworld) {									// to dark world fifth / from dark world
			if (circle && (deltaDark > deltaLight)) ToFifth(false, 0);							// if circle & lose more light than dark = to dark circle fifth
			else if (circle && (deltaDark <= deltaLight)) ToFifth(true, 0);						// if circle & lose more dark than light = to light circle fifth
			else if (triangle && (deltaDark > deltaLight)) ToFifth(true, 1);					// if triangle & lose more light than dark = to triangle fifth
			else if (triangle && (deltaDark <= deltaLight)) ToFifth(true, 1);					// if triangle & lose more dark than light = to triangle fifth
			else if (square && (deltaDark > deltaLight)) ToFifth(true, 2);						// if square & lose more light than dark = to square fifth
			else if (square && (deltaDark <= deltaLight)) ToFifth(true, 2);						// if square & lose more dark than light = to square fifth
		} 
			// to light world
		else if ((evol <= -5f && evol > -8f) && !inLightworld) {							// to light world fifth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 5, false, 0);						// if lose more light than dark = to light world dark circle fifth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 5, true, 0);					// if lose more dark than light = to light world light circle fifth
		}
			// in light world
		else if ((evol <= -5f && evol > -8f) && inLightworld) {								// to light world fifth / from light world
			if (deltaDark > deltaLight) ToFifth(false, 0);										// if lose more light than dark = to light world dark circle fifth
			else if (deltaDark <= deltaLight) ToFifth(true, 0);									// if lose more dark than light = to light world light circle fifth
		}
			// to dark world
		else if ((evol >= 5f && evol < 8f) && inLightworld) {								// to dark world fifth / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 5, false, 0);						// if lose more light than dark = to dark world dark fifth
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 5, true, 0);					// if lose more dark than light = to dark world light fifth
		}

		// sixth
			// in dark world
		if ((evol >= 8f && evol < 13f) && !inLightworld) {									// to dark world sixth / from dark world
			if (circle && (deltaDark > deltaLight)) ToSixth (false, 0);							// if circle & lose more light than dark = to dark circle sixth
			else if (circle && (deltaDark <= deltaLight)) ToSixth (true, 0);					// if circle & lose more dark than light = to light circle sixth
			else if (triangle && (deltaDark > deltaLight)) ToSixth (false, 1);					// if triangle & lose more light than dark = to triangle sixth
			else if (triangle && (deltaDark <= deltaLight)) ToSixth (false, 1);					// if triangle & lose more dark than light = to triangle sixth
			else if (square && (deltaDark > deltaLight)) ToSixth (false, 2);					// if square & lose more light than dark = to square sixth
			else if (square && (deltaDark <= deltaLight)) ToSixth (false, 2);					// if square & lose more dark than light = to square sixth
		} 
			// to light world
		else if ((evol <= -8f && evol > -13f) && !inLightworld) {							// to light world sixth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 6, false, 0);						// if lose more light than dark = to light world dark circle sixth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 6, true, 0);					// if lose more dark than light = to light world light circle sixth
		}
			// in light world
		else if ((evol <= -8f && evol > -13f) && inLightworld) {							// to light world sixth / from light world
			if (deltaDark > deltaLight) ToSixth(false, 0);										// if lose more light than dark = to light world dark circle sixth
			else if (deltaDark <= deltaLight) ToSixth(true, 0);									// if lose more dark than light = to light world light circle sixth
		}
			// to dark world
		else if ((evol >= 8f && evol < 13f) && inLightworld) {								// to dark world sixth / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 6, false, 0);						// if lose more light than dark = to dark world dark sixth
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 6, true, 0);					// if lose more dark than light = to dark world light sixth
		}

		// seventh
			// in dark world
		if ((evol >= 13f && evol < 21f) && !inLightworld) {									// to dark world seventh / from dark world
			if (circle && (deltaDark > deltaLight)) ToSeventh (false, 0);						// if circle & lose more light than dark = to dark circle eighth
			else if (circle && (deltaDark <= deltaLight)) ToSeventh (true, 0);					// if circle & lose more dark than light = to light circle eighth
			else if (triangle && (deltaDark > deltaLight)) ToSeventh (false, 1);				// if triangle & lose more light than dark = to dark triangle eighth
			else if (triangle && (deltaDark <= deltaLight)) ToSeventh (true, 1);				// if triangle & lose more dark than light = to light triangle eighth
			else if (square && (deltaDark > deltaLight)) ToSeventh (false, 2);					// if square & lose more light than dark = to dark square eighth
			else if (square && (deltaDark <= deltaLight)) ToSeventh (true, 2);					// if square & lose more dark than light = to light square eighth
		} 
			// to light world
		else if ((evol <= -13f && evol > -21f) && !inLightworld) {							// to light world seventh / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 7, false, 0);						// if lose more light than dark = to light world dark circle seventh
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 7, true, 0);					// if lose more dark than light = to light world light circle seventh
		}
			// in light world
		else if ((evol <= -13f && evol > -21f) && inLightworld) {							// to light world seventh / from light world
			if (deltaDark > deltaLight) ToSeventh (false, 0);									// if lose more light than dark = to light world dark circle eighth
			else if (deltaDark <= deltaLight) ToSeventh (true, 0);								// if lose more dark than light = to light world light circle eighth
		}
			// to dark world
		else if ((evol >= 13f && evol < 21f) && inLightworld) {								// to dark world seventh / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 7, false, 0);						// if lose more light than dark = to dark world dark seventh
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 7, true, 0);					// if lose more dark than light = to dark world light seventh
		}

		// eighth
			// in dark world
		if ((evol >= 21 && evol < 34) && !inLightworld) {									// to dark world eighth / from dark world
			if (circle && (deltaDark > deltaLight)) ToEighth (false, 0);						// if circle & lose more light than dark = to dark circle eighth
			else if (circle && (deltaDark <= deltaLight)) ToEighth (true, 0);					// if circle & lose more dark than light = to light circle eighth
			else if (triangle && (deltaDark > deltaLight)) ToEighth (false, 1);					// if triangle & lose more light than dark = to dark triangle eighth
			else if (triangle && (deltaDark <= deltaLight)) ToEighth (true, 1);					// if triangle & lose more dark than light = to light triangle eighth
			else if (square && (deltaDark > deltaLight)) ToEighth (false, 2);					// if square & lose more light than dark = to dark square eighth
			else if (square && (deltaDark <= deltaLight)) ToEighth (true, 2);					// if square & lose more dark than light = to light square eighth
		}
			// to light world
		else if ((evol <= -21f && evol > -34f) && !inLightworld) {							// to light world eighth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 8, false, 0);						// if lose more light than dark = to light world dark eighth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 8, true, 0);					// if lose more dark than light = to light world light eighth
		}
			// in light world
		else if (evol <= -21 && evol > -34) {												// to light world eighth / from light world
			if (deltaDark > deltaLight) ToEighth (false, 0);									// if lose more light than dark = to light world dark circle eighth
			else if (deltaDark <= deltaLight) ToEighth (true, 0);								// if lose more dark than light = to light world light circle eighth
		}
			// to dark world
		else if ((evol >= 21f && evol < 34f) && inLightworld) {								// to dark world eighth / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 8, false, 0);						// if lose more light than dark = to dark world dark eighth
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 8, true, 0);					// if lose more dark than light = to dark world light eighth
		}

		// ninth
			// in dark world
				// same state
			// to light world
		if ((evol <= -34f && evol > -55f) && !inLightworld) {								// to light world ninth / from dark world
			if (deltaDark > deltaLight) ToOtherWorld(true, 9, false, 0);						// if lose more light than dark = to light world dark ninth
			else if (deltaDark <= deltaLight) ToOtherWorld(true, 9, true, 0);					// if lose more dark than light = to light world light ninth
		}
			// in light world
				// same state
			// to dark world
		else if ((evol >= 34f && evol < 55f) && inLightworld) {								// to dark world ninth / from light world
			if (deltaDark > deltaLight) ToOtherWorld(false, 9, false, 0);						// if lose more light than dark = to dark world dark ninth
			else if (deltaDark <= deltaLight) ToOtherWorld(false, 9, true, 0);					// if lose more dark than light = to dark world light ninth
		}

		// tenth
			// in dark world
		if ((evol >= 55f) || (evol <= -55f)) {												// to tenth
			ToTenth(true, 0);																	// transition to tenth
		}
	}
}
