using UnityEngine;
using System.Collections;

public class ZeroPlayerState : IParticleState
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag
	private bool init = true;															// is init flag

	[HideInInspector] public bool canCollide = false;									// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset collision timer

	// constructor
	public ZeroPlayerState (PlayerStatePattern playerStatePattern)						// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// called every frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			Debug.Log("player zero state - evol entering evol check: " + psp.evol);
			Evol();																		// check evol logic
			checkEvol = false;															// reset check evol flag
		}

		if (psp.isInit && init) {														// if init
			Init();                                                         				// call init
			init = false;																	// reset is init flag
		}

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
		if (!other.gameObject.CompareTag("World")) Debug.Log ("zero player collision with " + other.gameObject.name);
		
		if (canCollide && psp.canCollide) {												// if collision allowed and not in menu
			if (other.gameObject.CompareTag ("Zero")) {										// if collide with zero
				ParticleStatePattern pspOther 
						= other.gameObject.GetComponent<ParticleStatePattern> ();				// ref other ParticleStatePattern
				if (psp.lightworld == pspOther.inLightworld) {									// if player and particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc [0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// stunned flag
					if (pspOther.isLight) {															// if light
						Debug.Log ("player+zero: add light");
						//if (pspOther.evolC == 0) 
						psp.AddLight (0.5f);									// if other evol = 0, add 0.5 to light
						//else if (pspOther.evolC > 0) psp.AddLight (pspOther.lightEvolC);				// else, add light of other
					}
					else if (!pspOther.isLight) {													// if dark
						Debug.Log ("player+zero: add dark");
						//if (pspOther.evolC == 0) 
						psp.AddDark (0.5f);									// if other evol = 0, add 0.5 to dark
						//else if (pspOther.evolC > 0) psp.AddDark (pspOther.darkEvolC);					// else, add dark of other
					}
					checkEvol = true;																// set check evol flaG
				}
			}
			else if (other.gameObject.CompareTag ("First")									// collide with first
				|| other.gameObject.CompareTag ("Second")									// collide with second
			    || other.gameObject.CompareTag ("Third")									// collide with third
			    || other.gameObject.CompareTag ("Fourth")									// collide with fourth
			    || other.gameObject.CompareTag ("Fifth")									// collide with fifth
			    || other.gameObject.CompareTag ("Sixth")									// collide with sixth
			    || other.gameObject.CompareTag ("Seventh")									// collide with seventh
			    || other.gameObject.CompareTag ("Eighth")									// collide with eighth
			    || other.gameObject.CompareTag ("Ninth")) {									// collide with ninth
				ParticleStatePattern pspOther 
					= other.gameObject.GetComponent<ParticleStatePattern> ();					// ref other ParticleStatePattern
				if (psp.lightworld == pspOther.inLightworld) {									// if player and particle in same world
					canCollide = false;																// reset has collided trigger
					psp.sc [0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// set stunned flag
					Debug.Log ("player+else: sub evol");
					if (pspOther.evolC > 0f) {														// other > 0
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC);					// sub other dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC);				// sub other light
					}
					else if (pspOther.evolC < 0f) {													// other < 0
						if (pspOther.darkEvolC != 0f) psp.SubDark (pspOther.darkEvolC * -1);			// sub other negated dark
						if (pspOther.lightEvolC != 0f) psp.SubLight (pspOther.lightEvolC * -1);			// sub other negated light
					}
					checkEvol = true;																// set check evol flag
				}
			}
		}
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0, 0);									// trigger transition effects
	}

	public void ToHalfZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0, 0);
		//ParticleStateEvents.toLightZero += psp.TransitionToLightZero;
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(0, 1, isLight, toLight, 0, 0);									// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;								// flag transition in delegate
		psp.currentState = psp.firstState;												// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(0, 2, isLight, toLight, 0, 0);									// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;							// flag transition in delegate
		psp.currentState = psp.secondState;												// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(0, 3, isLight, toLight, 0, 0);									// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;								// flag transition in delegate
		psp.currentState = psp.thirdState;												// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(0, 4, isLight, toLight, 0, 0);									// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;							// flag transition in delegate
		psp.currentState = psp.fourthState;												// set to new state
	}

	public void ToFifth(bool toLight, int toShape)
	{
		psp.TransitionTo(0, 5, isLight, toLight, 0, toShape);							// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;								// flag transition in delegate
		psp.currentState = psp.fifthState;												// set to new state
	}

	public void ToSixth(bool toLight, int toShape)
	{
		psp.TransitionTo(0, 6, isLight, toLight, 0, toShape);							// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;								// flag transition in delegate
		psp.currentState = psp.sixthState;												// set to new state
	}

	public void ToSeventh(bool toLight, int toShape)
	{
		psp.TransitionTo(0, 7, isLight, toLight, 0, toShape);							// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;							// flag transition in delegate
		psp.currentState = psp.seventhState;											// set to new state
	}

	public void ToEighth(bool toLight, int toShape)
	{
		psp.TransitionTo(0, 8, isLight, toLight, 0, toShape);							// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;							// flag transition in delegate
		psp.currentState = psp.eighthState;												// set to new state
	}

	public void ToNinth(bool toLight, int toShape)
	{
		psp.TransitionTo(0, 9, isLight, toLight, 0, toShape);							// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;							// flag transition in delegate
		psp.currentState = psp.ninthState;												// set to new state
	}

    public void Init()
    {
        evol = psp.evol;                                                                    // local evol check	

        if (evol == 0f) ToZero(true);               									    // init to light zero
        else if (evol == 1f) ToFirst(true);               									// init to light zero
        else if (evol == 1.5f) ToSecond(true);             									// init to light second
        else if (evol == 2f) ToThird(true);               									// init to light third
        else if (evol == 3f)
        {
            int i = Random.Range(0, 1);                                                         // random 0 or 1
            if (i == 0) ToFourth(false);                                                        // to dark fourth
            else ToFourth(true);                                                                // to light fourth
        }
        else if (evol == 5f)
        {
            int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
            if (i == 0) ToFifth(true, 0);                                                       // to light circle fifth
            else if (i == 1) ToFifth(true, 1);                                                  // to light triangle fifth
            else if (i == 2) ToFifth(true, 2);													// to light square fifth
        }
        else if (evol == 8f)
        {
            int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
            if (i == 0) ToSixth(true, 0);                                                       // to light circle sixth
            else if (i == 1) ToSixth(false, 1);													// to dark triangle sixth
            else if (i == 2) ToSixth(false, 2);													// to dark square sixth
        }
        else if (evol == 13f)
        {
            int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
            if (i == 0) ToSeventh(true, 0);														// to light circle seventh
            else if (i == 1) ToSeventh(true, 1);												// to light triangle seventh
            else if (i == 2) ToSeventh(true, 2);												// to light square seventh
        }
		else if (evol == 21f)
		{
			int i = Random.Range(0, 2);															// random 0 or 1 or 2
			if (i == 0) ToEighth(true, 0);														// to light circle eighth
			else if (i == 1) ToEighth(true, 1);													// to light triangle eighth
			else if (i == 2) ToEighth(true, 2);													// to light square eighth
		}
		else if (evol == 34f)
		{
			int i = Random.Range(0, 2);															// random 0 or 1 or 2
			if (i == 0) ToNinth(true, 0);														// to light circle ninth
			else if (i == 1) ToNinth(true, 1);													// to light triangle ninth
			else if (i == 2) ToNinth(true, 2);													// to light square ninth
		}
    }

    public void Evol()
	{
		evol = psp.evol;																// local evol check			
		isLight = psp.isLight;															// update light value
		deltaDark = psp.deltaDark;														// local dark check
		deltaLight = psp.deltaLight;													// local light check

		// switch world triggers
		if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;						// if to light world (if evol < 0), set to light world trigger
		else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;                  // if to dark world (evol >= 0), set dark world flag

		// zero
			// to dark world
		if (evol == 0f && psp.toDarkworld) ToZero (true);								// to dark world light zero / from light world

        // half zero
			// in either world
		if ((evol == 0.5f) || (evol == -0.5f)) {										// to either half zero
			if (deltaDark > deltaLight) ToHalfZero (false);		    						// if gain dark/lose light = to dark zero
			else if (deltaDark <= deltaLight) ToHalfZero (true);							// if gain light/lose dark = to light zero
		}
       
		// first
			// in either world
		if ((evol == 1f) || (evol == -1f)) {											// to either first
			if (deltaDark > deltaLight) ToFirst(false);                                     // if gain more dark than light = to dark first
			else if (deltaDark <= deltaLight) ToFirst(true);                                // if gain more light than dark = to light first
        }
       
		// second
			// in light world
		if (evol == -1.5f) {															// to light world second
			if (deltaDark > deltaLight) ToSecond(false);									// if lose more light than dark = to dark second
            else if (deltaDark <= deltaLight) ToSecond(true);								// if lose more dark than light = to light second
        }
       
		// third
			// in light world
		if ((evol <= -2f) && (evol > -3f)) {											// to light world third
			if (deltaDark > deltaLight) ToThird(false);										// if lose more light than dark = to dark third
			else if (deltaDark <= deltaLight) ToThird(true);								// if lose more dark than light = to light third
		}
       
		// fourth
			// in light world
		if ((evol <= -3f) && (evol > -5f)) {											// to light world fourth
			if (deltaDark > deltaLight) ToFourth(false);									// if lose more light than dark = to dark fourth
			else if (deltaDark <= deltaLight) ToFourth(true);								// if lose more dark than light = to light fourth
		}

		// fifth
			// in light world
		if ((evol <= -5f) && (evol > -8f)) {											// to light world fifth
			if (deltaDark > deltaLight) ToFifth(false, 0);									// if lose more light than dark = to dark circle fifth
			else if (deltaDark <= deltaLight) ToFifth(true, 0);								// if lose more dark than light = to light circle fifth
		}
        
		// sixth
			// in light world
		if ((evol <= -8f) && (evol > -13f)) {											// to light world sixth 
			if (deltaDark > deltaLight) ToSixth(false, 0);									// if lose more light than dark = to dark circle sixth
			else if (deltaDark <= deltaLight) ToSixth(true, 0);								// if lose more dark than light = to light circle sixth
		}
        
		// seventh
			// in light world
		if ((evol <= -13f) && (evol > -21f)) {											// to light world seventh
			if (deltaDark > deltaLight) ToSeventh(false, 0);								// if lose more light than dark = to dark circle seventh
			else if (deltaDark <= deltaLight) ToSeventh(true, 0);							// if lose more dark than light = to light circle seventh
		}

		// eighth
			// in light world
		if ((evol <= -21f) && (evol > -34f)) {										    // to light world eighth
			if (deltaDark > deltaLight) ToEighth(false, 0);									// if lose more light than dark = to dark circle eighth
			else if (deltaDark <= deltaLight) ToEighth(true, 0);							// if lose more dark than light = to light circle eighth
		}

		// ninth
			// in light world
		if ((evol <= -34f) && (evol > -55f)) {										    // to light world ninth
			if (deltaDark > deltaLight) ToNinth(false, 0);									// if lose more light than dark = to dark circle ninth
			else if (deltaDark <= deltaLight) ToNinth(true, 0);								// if lose more dark than light = to light circle ninth
		}
	}
}
