using UnityEngine;
using System.Collections;

public class ZeroPlayerState : IParticleState
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
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
			Debug.Log("player evol: " + psp.evol);
			Evol();																		// check evol logic
			checkEvol = false;															// reset check evol flag
		}

        if (psp.isInit) Init();                                                         // if init, init

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
		if (!other.gameObject.CompareTag("World")) Debug.Log ("zero player collision");
		
		if (canCollide) {																// if collision allowed and other in dark world
			if (other.gameObject.CompareTag ("Zero")) {										// if collide with zero
				ParticleStatePattern pspOther 
						= other.gameObject.GetComponent<ParticleStatePattern> ();				// ref other ParticleStatePattern
				if (!pspOther.inLightworld) {													// if other in dark world	
					canCollide = false;																// reset has collided trigger
					psp.sc [0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// stunned flag
					if (pspOther.isLight) {															// if light
						Debug.Log ("player+zero: add light");
						if (pspOther.evolC == 0) psp.AddLight (0.5f);									// if other evol = 0, add 0.5 to light
						else if (pspOther.evolC > 0) psp.AddLight (pspOther.lightEvolC);				// else, add light of other
					}
					else if (!pspOther.isLight) {													// if dark
						Debug.Log ("player+zero: add dark");
						if (pspOther.evolC == 0) psp.AddDark (0.5f);									// if other evol = 0, add 0.5 to dark
						else if (pspOther.evolC > 0) psp.AddDark (pspOther.darkEvolC);					// else, add dark of other
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
				if (!pspOther.inLightworld) {													// if other in dark world
					canCollide = false;																// reset has collided trigger
					psp.sc [0].enabled = false;														// disable trigger collider
					psp.stunned = true;																// set stunned flag
					Debug.Log ("player+else: sub evol");
					psp.SubDark (pspOther.darkEvol);												// subtract other dark
					psp.SubLight (pspOther.lightEvol);                                              // subtract other light
					checkEvol = true;																// set check evol flag
				}
			}
		}
	}

	public void ToZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0);									// trigger transition effects
	}

	public void ToHalfZero(bool toLight)
	{
		psp.TransitionTo(0, 0, isLight, toLight, 0);
		//ParticleStateEvents.toLightZero += psp.TransitionToLightZero;
	}

	public void ToFirst(bool toLight)
	{
		psp.TransitionTo(0, 1, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFirst += psp.TransitionToFirst;						// flag transition in delegate
		psp.currentState = psp.firstState;											// set to new state
	}

	public void ToSecond(bool toLight)
	{
		psp.TransitionTo(0, 2, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toSecond += psp.TransitionToSecond;						// flag transition in delegate
		psp.currentState = psp.secondState;											// set to new state
	}

	public void ToThird(bool toLight)
	{
		psp.TransitionTo(0, 3, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toThird += psp.TransitionToThird;						// flag transition in delegate
		psp.currentState = psp.thirdState;											// set to new state
	}

	public void ToFourth(bool toLight)
	{
		psp.TransitionTo(0, 4, isLight, toLight, 0);									// trigger transition effects
		//ParticleStateEvents.toFourth += psp.TransitionToFourth;						// flag transition in delegate
		psp.currentState = psp.fourthState;											// set to new state
	}

	public void ToFifth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 5, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toFifth += psp.TransitionToFifth;						// flag transition in delegate
		psp.currentState = psp.fifthState;											// set to new state
	}

	public void ToSixth(bool toLight, int shape)
	{
		psp.TransitionTo(0, 6, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSixth += psp.TransitionToSixth;						// flag transition in delegate
		psp.currentState = psp.sixthState;											// set to new state
	}

	public void ToSeventh(bool toLight, int shape)
	{
		psp.TransitionTo(0, 7, isLight, toLight, shape);								// trigger transition effects
		//ParticleStateEvents.toSeventh += psp.TransitionToSeventh;					// flag transition in delegate
		psp.currentState = psp.seventhState;										// set to new stateebug.Log ("Can't transition to same state");
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
            else if (i == 1) ToSixth(false, 1);                                             // to dark triangle sixth
            else if (i == 2) ToSixth(false, 2);												// to dark square sixth
        }
        else if (evol == 13f)
        {
            int i = Random.Range(0, 2);                                                         // random 0 or 1 or 2
            if (i == 0) ToSeventh(true, 0);                                                 // to light circle seventh
            else if (i == 1) ToSeventh(true, 1);                                                // to light triangle seventh
            else if (i == 2) ToSeventh(true, 2);												// to light square seventh
        }
        // new state
    }

    public void Evol()
	{
		evol = psp.evol;																// local evol check			
		isLight = psp.isLight;																// update light value
		deltaDark = psp.deltaDark;														// local dark check
		deltaLight = psp.deltaLight;													// local light check

		// switch world triggers
		if (!psp.lightworld	&& evol < 0f) psp.toLightworld = true;						// if to light world (if evol < 0), set to light world trigger
		else if (psp.lightworld && evol >= 0f) psp.toDarkworld = true;                  // if to dark world (evol >= 0), set dark world flag

        // half zero
        if (evol == 0.5f || evol == -0.5f) {											// evolve to half zero (if evol = 0.5)
			if (deltaDark == 0.5f || deltaDark == -0.5f) {
				Debug.Log("player to half zero");
				ToHalfZero (false);		    	// if gain dark = to dark zero
			}
			//else if (deltaLight == 0.5f || deltaLight == -0.5f) ToHalfZero (true);		// if gain light = to light zero
		}
        // first
        if (evol == 1f) {                                                               // evolve to dark world first
			//Debug.Log("player to first");
			if (deltaDark > deltaLight) {
				Debug.Log("player to dark first");
				ToFirst(false);                                     // if gain more dark than light = to dark first
			}
			else if (deltaDark <= deltaLight) {
				Debug.Log("player to light first");
				ToFirst(true);                                 // if gain more light than dark = to light first
			}
        }
        else if (evol == -1f) {                                                         // devolve to light world first
			Debug.Log("player to light world first");
			if (deltaDark <= deltaLight) ToFirst(true);										// if lose more dark than light = to light first
            else if (deltaDark > deltaLight) ToFirst(false);                                // if lose more light than dark = to dark first
        }
        // second
        else if (evol == -1.5f) {                                                       // devolve to light world second (if evol == -1.5)
            if (deltaDark <= deltaLight) ToSecond(true);									// if lose more dark than light = to light second
            else if (deltaDark > deltaLight) ToSecond(false);                               // if lose more light than dark = to dark second
        }
        // third
		if (evol >= -2f && evol < -3f) {								    			// devolve to light world third (if evol == -2)
			if (deltaDark <= deltaLight) ToThird(true);										// if lose more dark than light = to light third
			else if (deltaDark > deltaLight) ToThird(false);								// if lose more light than dark = to dark third
		}
        // fourth
		if (evol >= -3f && evol < -5f) {							    				// devolve to light world fourth (if evol == -3)
			if (deltaDark <= deltaLight) ToFourth(true);									// if lose more dark than light = to light fourth
			else if (deltaDark > deltaLight) ToFourth(false);								// if lose more light than dark = to dark fourth
		}
		// fifth
        if (evol >= -5f && evol < -8f) {											    // devolve to light world fifth (if evol == -5)
			if (deltaDark <= deltaLight) ToFifth(true, 0);									// if lose more dark than light = to light circle fifth
			else if (deltaDark > deltaLight) ToFifth(false, 0);								// if lose more light than dark = to dark circle fifth
		}
        // sixth
        if (evol >= -8f && evol < -13f) {											    // devolve to light world sixth (if evol == -8)
			if (deltaDark <= deltaLight) ToSixth(true, 0);									// if lose more dark than light = to light circle sixth
			else if (deltaDark > deltaLight) ToSixth(false, 0);								// if lose more light than dark = to dark circle sixth
		}
        // seventh
        if (evol >= -13f && evol < -21f) {											    // devolve to light world seventh (if evol == -13)
			if (deltaDark <= deltaLight) ToSeventh(true, 0);								// if lose more dark than light = to light circle seventh
			else if (deltaDark > deltaLight) ToSeventh(false, 0);							// if lose more light than dark = to dark circle seventh
		}

        // new state
	}
}
