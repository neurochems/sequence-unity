using UnityEngine;
using System.Collections;

public class ParticleStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public int state;														// state indicator for inspector
	public bool isLight;													// is light flag
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float darkEvolStart, lightEvolStart;								// last dark & light evolution level (for delta calc)
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public float evolC, darkEvolC, lightEvolC;								// evol values at start of collision

	public int shape;														// shape indicator
	[HideInInspector] public bool circle = true, triangle, square;			// shape flags

	private bool updateStateIndicator;										// update state indicator flag
	[HideInInspector] public IParticleState currentState;					// other object state
	//[HideInInspector] public int previousState;								// previous state index / Fn

	[HideInInspector] public ZeroParticleState zeroState;					// instance of zero state
	[HideInInspector] public FirstParticleState firstState;					// instance of first state
	[HideInInspector] public SecondParticleState secondState;				// instance of second state
	[HideInInspector] public ThirdParticleState thirdState;					// instance of third state
	[HideInInspector] public FourthParticleState fourthState;				// instance of fourth state
	[HideInInspector] public FifthParticleState fifthState;					// instance of fifth state
	[HideInInspector] public SixthParticleState sixthState;					// instance of sixth state
	[HideInInspector] public SeventhParticleState seventhState;				// instance of seventh state
	[HideInInspector] public EighthParticleState eighthState;				// instance of eighth state
	[HideInInspector] public NinthParticleState ninthState;					// instance of ninth state

	public bool lightworld = false;											// is light world flag
	public bool toWorld, toLightworld, toDarkworld, inLightworld;			// to world indicator, to light world trigger, to dark world trigger, in light world flag
	public bool changeParticles;											// timing trigger for colour change

	//public float attractionRange = 20f;										// particle sensing distance
	//[HideInInspector] public Transform attractionTarget;					// particle sensing target

	//private GameObject nucleus, shell;										// reference to nucleus and shell children
	[HideInInspector] public GameObject self;								// reference to this gameobject

	//component references
	[HideInInspector] public PlayerStatePattern psp;						// player state pattern (lightworld)

	private ParticleCoreManager pcm;										// particle core manager (animations)
	private ParticleShellManager psm;										// particle core manager (animations)
	private ParticleNucleusManager pnm;										// particle core manager (animations)
	private Rigidbody rb;													// particle rigidbody
	//private ParticlePhysicsManager ppm;										// particle physics manager
	[HideInInspector] public SphereCollider[] sc;							// sphere colliders

	// part changes
	private int fromState, toState, fromShape, toShape;						// transition properties
	private bool fromLight, toLight;										// transition properties
	private bool setShell, setNucleus;										// set part flags
	private bool cwShell, cwNucleus;										// set change world part flags
	private float setShellTimer, setNucleusTimer;							// set part timers
	private float cwShellTimer, cwNucleusTimer;								// set change world part timers
	private float inLightworldTimer;										// set change world part timers

	// collision
	public int die;															// collision conflict check
	public bool roll;														// collision conflict check
	public bool stunned;													// stunned?
	public float stunDuration = 7f;											// duration of post-hit invulnerability
	private float activeTimer = 0f;							// stun timer, active timer

	void Awake()
	{
		//evol = 0f;															// initialize evol
		//darkEvol = 0f;														// initialize dark evol
		//lightEvol = 0f;														// initialize light evol

		deltaDark = 0f;														// initialize delta dark evol
		deltaLight = 0f;													// initialize delta light evol

		zeroState = new ZeroParticleState (this);							// initialize zero state
		firstState = new FirstParticleState (this);							// initialize first state
		secondState = new SecondParticleState (this);						// initialize second state
		thirdState = new ThirdParticleState (this);							// initialize third state
		fourthState = new FourthParticleState (this);						// initialize fourth state
		fifthState = new FifthParticleState (this);							// initialize fifth state
		sixthState = new SixthParticleState (this);							// initialize sixth state
		seventhState = new SeventhParticleState (this);						// initialize seventh state
		eighthState = new EighthParticleState (this);						// initialize eighth state
		ninthState = new NinthParticleState (this);							// initialize ninth state

	}

	void Start () 
	{
		psp = GameObject.Find ("Player").
			gameObject.GetComponent<PlayerStatePattern> ();					// init player state pattern ref

		pcm = transform.FindChild("Core")
			.gameObject.GetComponent<ParticleCoreManager> ();				// initialize core manager ref
		psm = transform.FindChild ("Shell")
			.gameObject.GetComponent<ParticleShellManager>();				// initialize shell manager ref
		pnm = transform.FindChild ("Nucleus")
			.gameObject.GetComponent<ParticleNucleusManager>();				// initialize nucleus manager ref

		sc = GetComponents<SphereCollider> ();								// init sphere colliders ref

		rb = GetComponent<Rigidbody> ();									// init rigidbody ref

		currentState = zeroState;											// start at zero state
		state = 0;															// start at zero state

		roll = true;														// roll die
	}

	void Update () 
	{
	// evol management \\

		// calculate evol
		evol = lightEvol + darkEvol;										// update total evol value
		// cap evol
		if (evol > 55) evol = 55;											// cap evol positive
		if (evol < -55f) evol = -55f;										// cap evol negative

		// calculate delta
		deltaDark = darkEvol - darkEvolStart;								// calculate deltaDark
		deltaLight = lightEvol - lightEvolStart;							// calculate deltaLight

	// world management \\

		lightworld = psp.lightworld;										// update if lightworld
		changeParticles = psp.changeParticles;								// if changeParticles is true, update from player state pattern

	// state indicator \\

		// current state as int
		if (updateStateIndicator) {											// if update state indicator flag
			if (currentState == zeroState) state = 0;							// zero
			else if (currentState == firstState) state = 1;						// first
			else if (currentState == secondState) state = 2;					// second
			else if (currentState == thirdState) state = 3;						// third
			else if (currentState == fourthState) state = 4;					// fourth
			else if (currentState == fifthState) state = 5;						// fifth
			else if (currentState == sixthState) state = 6;						// sixth
			else if (currentState == seventhState) state = 7;					// seventh
			else if (currentState == eighthState) state = 8;					// eighth
			else if (currentState == ninthState) state = 9;						// ninth
			updateStateIndicator = false;										// reset flag
		}
	
	// timers \\

		// set part world change timers
		if (cwShell) {																					// if set shell
			cwShellTimer += Time.deltaTime;																	// start timer
			if (cwShellTimer >= 0.1f) {																		// if timer is 0.1 sec
				psm.ToOtherWorld (toWorld, fromState, toState, fromLight, toLight, fromShape, toShape);			// change shell
				cwShell = false;																				// reset set shell flag
				cwShellTimer = 0f;																				// reset set shell timer
			}
		}
		if (cwNucleus) {																				// if set nucleus
			cwNucleusTimer += Time.deltaTime;																// start timer
			if (cwNucleusTimer >= 0.2f) {																	// if timer is 0.2 sec
				pnm.ToOtherWorld (toWorld, fromState, toState, toLight, fromShape, toShape);				// change nucleus
				cwNucleus = false;																				// reset set nucleus flag
				cwNucleusTimer = 0f;																			// reset set nucleus timer
			}
		}

		// world change indicator reset/collider enable/update isLight 
		if (toLightworld || toDarkworld) {										// if to opposite world
			inLightworldTimer += Time.deltaTime;									// start timer
			if ((inLightworldTimer >= 0.5f) && toLightworld) {						// if timer is 0.5 sec and going to light world
				inLightworld = true;													// set in lightworld flag sooner to avoid player collisions with particles just sent to light world
			} 
			else if ((inLightworldTimer >= 2.5f) && toLightworld) {					// if timer is 2.5 sec and going to light world

				toLightworld = false;													// reset to light world flag
				sc [0].enabled = true;													// enable trigger collider

				isLight = toLight;														// set is light indicator

				inLightworldTimer = 0f;													// reset timer
			} 
			else if ((inLightworldTimer >= 0.5f) && toDarkworld) {					// if timer is 0.5 sec and going to dark world
				inLightworld = false;													// reset in lightworld flag
			}
			else if ((inLightworldTimer >= 2.5f) && toDarkworld) {					// if timer is 2.5 sec and going to dark world
				toDarkworld = false;													// reset to light world flag
				sc[0].enabled = true;													// enable trigger collider
				isLight = toLight;														// set is light indicator
				inLightworldTimer = 0f;													// reset timer
			}
		}
		// part change timers
		if (setShell) {																			// if set shell
			setShellTimer += Time.deltaTime;														// start timer
			if (setShellTimer >= 0.75f) {															// if timer is 0.75 sec
				psm.Shell (fromState, toState, fromLight, toLight, fromShape, toShape);					// change shell
				setShell = false;																		// reset set shell flag
				setShellTimer = 0f;																		// reset set shell timer
			}
		}
		if (setNucleus) {																		// if set nucleus
			setNucleusTimer += Time.deltaTime;														// start timer
			if (setNucleusTimer >= 1.0f) {															// if timer is 1.0 sec
				pnm.Nucleus (fromState, toState, fromLight, toLight, fromShape, toShape);				// change nucleus
				setNucleus = false;																		// reset set nucleus flag
				setNucleusTimer = 0f;																	// reset set nucleus timer
			}
		}

		// tenth state deactivation timer
		if (psp.state == 10) {																	// if player is state 10
			activeTimer += Time.deltaTime;															// start timer
			if (activeTimer >= 10.0f) {																// if timer is 10 sec
				//TransitionTo(state, 10, isLight, false, toShape, toShape);								// transition to hidden
			}
			if (activeTimer >= 30.0f) {																// if timer is 20 sec
				gameObject.SetActive (false);															// deactivate
				activeTimer = 0f;																		// reset active timer
			}
		}

	// roll a d20 \\

		if (roll) {
			die = Random.Range(1,20);														// roll die
			roll = false;																	// reset flag
		}

	// update state class \\

	/////
		currentState.UpdateState ();										// frame updates from current state class
	/////

	}

	private void OnTriggerEnter(Collider other)
	{
		evolC = evol;																		// store evol before collision changes
		darkEvolC = darkEvol;																// store dark evol before collision changes
		lightEvolC = lightEvol;																// store light evol before collision changes	

		currentState.OnTriggerEnter (other);								// pass collider into state class
	}

	void OnDisable()
	{
		//ParticleStateEvents.toZero -= TransitionToZero;						// untrigger transition event
		//ParticleStateEvents.toFirst -= TransitionToFirst;					// untrigger transition event
		//ParticleStateEvents.toSecond -= TransitionToSecond;					// untrigger transition event
		//ParticleStateEvents.toThird -= TransitionToThird;					// untrigger transition event
		//ParticleStateEvents.toFourth -= TransitionToFourth;					// untrigger transition event
		//ParticleStateEvents.toFifth -= TransitionToFifth;					// untrigger transition event
		//ParticleStateEvents.toSixth -= TransitionToSixth;					// untrigger transition event
		//ParticleStateEvents.toSeventh -= TransitionToSeventh;					// untrigger transition event
		// new state
	}

	void OnDestroy()
	{
		//ParticleStateEvents.toZero -= TransitionToZero;						// untrigger transition event
		//ParticleStateEvents.toFirst -= TransitionToFirst;					// untrigger transition event
		//ParticleStateEvents.toSecond -= TransitionToSecond;					// untrigger transition event
		//ParticleStateEvents.toThird -= TransitionToThird;					// untrigger transition event
		//ParticleStateEvents.toFourth -= TransitionToFourth;					// untrigger transition event
		//ParticleStateEvents.toFifth -= TransitionToFifth;					// untrigger transition event
		//ParticleStateEvents.toSixth -= TransitionToSixth;					// untrigger transition event
		//ParticleStateEvents.toSeventh -= TransitionToSeventh;					// untrigger transition event
		// new state
	}

	// EVOL CHANGES \\ - PUT IN SEPARATE SCRIPT

	public void SubEvol(float changeAmount) 
	{
		evol -= changeAmount;												// subtract evol level
	}
	public void AddEvol(float changeAmount) 
	{
		evol += changeAmount;												// add evol level
	}

	public void AddDark(float changeAmount) {
		darkEvol += changeAmount;											// add dark evol level
	}
	public void SubDark(float changeAmount) {
		darkEvol -= changeAmount;											// subtract dark evol level
	}

	public void AddLight(float changeAmount) {
		lightEvol += changeAmount;											// add light evol level
	}
	public void SubLight(float changeAmount) {
		lightEvol -= changeAmount;											// subtract light evol level
	}

	// STATE TRANSTITIONS \\

	///<summary>
	///<para>== TransitionTo ==</para>
	///<para>trigger animations of changes between states</para>
	///<para>f = from state</para>
	///<para>t = to state</para>
	///<para>fl = from light</para>
	///<para>tl = to light</para>
	///<para>fs = from shape [0=circ, 1=tri, 2=sq]</para>
	///<para>ts = to shape</para>
	///</summary>
	public void TransitionTo(int f, int t, bool fl, bool tl, int fs, int ts)
	{
		// early updates
		fromState = f;																// store from state
		toState = t;																// store to state
		fromLight = fl;																// store from light
		toLight = tl;																// store to light
		fromShape = fs;																// store from shape
		toShape = ts;																// store to shape

		updateStateIndicator = true;												// update state indicator

		// set shape
		shape = toShape;															// set shape indicator
		if (toShape == 0) {															// if shape is circle
			circle = true;																// set circle flag
			triangle = false;															// reset triangle flag
			square = false;																// reset square flag
		}
		else if (toShape == 1) {													// if shape is circle
			circle = false;																// reset circle flag
			triangle = true;															// set triangle flag
			square = false;																// reset square flag
		}
		else if (toShape == 2) {													// if shape is circle
			circle = false;																// reset circle flag
			triangle = false;															// reset triangle flag
			square = true;																// set square flag
		}

		// trigger animations
		pcm.Core (fromState, toState, fromLight, toLight, fromShape, toShape);		// change core
		setShell = true;															// start set shell timer
		if (evol >= 0) setNucleus = true;											// start set nucleus timer (dark world only)

		// physics changes
		if (toState == 0)	{ 														// to zero
			rb.mass = 1.0f;																// set mass
			gameObject.tag = "Zero";													// set tag
			sc[0].radius = 0.208f;														// update collision radius
			sc[1].radius = 0.205f;														// update collision radius
		}
		else if (toState == 1) {													// to first
			rb.mass = 2.0f;																// set mass
			gameObject.tag = "First";													// set tag
			sc[0].radius = 0.54f;														// update collision radius
			sc[1].radius = 0.52f;														// update collision radius
		}
		else if (toState == 2) {													// to second
			rb.mass = 2.5f;																// set mass
			gameObject.tag = "Second";													// set tag
			sc[0].radius = 0.54f;														// update collision radius
			sc[1].radius = 0.52f;														// update collision radius
		}
		else if (toState == 3) {													// to third
			rb.mass = 3.0f;																// set mass
			gameObject.tag = "Third";													// set tag
			if (toLight) {
				sc[0].radius = 1.06f;														// update collision radius
				sc[1].radius = 1.04f;														// update collision radius
			}
			else if (!toLight) {
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
		}
		else if (toState == 4) {													// to fourth
			rb.mass = 3.5f;																// set mass
			gameObject.tag = "Fourth";													// set tag
			if (toLight) {
				sc[0].radius = 1.06f;														// update collision radius
				sc[1].radius = 1.04f;														// update collision radius
			}
			else if (!toLight) {
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
		}
		else if (toState == 5) {													// to fifth
			rb.mass = 4.0f;																// set mass
			gameObject.tag = "Fifth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
			else if (toShape == 1 || toShape == 2) {									// if triangle or square
				sc[0].radius = 1.05f;														// update collision radius
				sc[1].radius = 1.00f;														// update collision radius
			}
		}
		else if (toState == 6) {													// to sixth
			rb.mass = 4.5f;																// set mass
			gameObject.tag = "Sixth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
			else if (toShape == 1 || toShape == 2) {									// if triangle or square
				sc[0].radius = 1.05f;														// update collision radius
				sc[1].radius = 1.00f;														// update collision radius
			}
		}
		else if (toState == 7) {													// to seventh
			rb.mass = 5.5f;																// set mass
			gameObject.tag = "Seventh";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 4.75f;														// update collision radius
				sc[1].radius = 1.54f;														// update collision radius
			} 
			else if (!toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 4.75f;														// update collision radius
				sc[1].radius = 1.95f;														// update collision radius
			}
			else if (toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 1.95f;														// update collision radius
				sc[1].radius = 1.95f;														// update collision radius
			}
		}
		else if (toState == 8) {													// to eighth
			rb.mass = 7.0f;																// set mass
			gameObject.tag = "Eighth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 4.75f;														// update collision radius
				sc[1].radius = 1.54f;														// update collision radius
			} 
			else if (!toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 4.75f;														// update collision radius
				sc[1].radius = 1.95f;														// update collision radius
			}
			else if (toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 1.95f;														// update collision radius
				sc[1].radius = 1.95f;														// update collision radius
			}
		}
		else if (toState == 9) {													// to ninth
			rb.mass = 8.5f;																// set mass
			gameObject.tag = "Ninth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 7.25f;														// update collision radius
				sc[1].radius = 2.05f;														// update collision radius
			} 
			else if (!toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 4.75f;														// update collision radius
				sc[1].radius = 2.65f;														// update collision radius
			}
			else if (toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 3.65f;														// update collision radius
				sc[1].radius = 2.25f;														// update collision radius
			}
		}
		else if (toState == 10)	{ 														// to zero
			rb.mass = .0f;																// set mass
			gameObject.tag = "Tenth";													// set tag
			sc[0].radius = 0.01f;														// update collision radius
			sc[1].radius = 0.01f;														// update collision radius
		}

		// late updates
		isLight = toLight;															// set light flag
		darkEvolStart = darkEvol;													// store dark evol at start of state
		lightEvolStart = lightEvol;													// store light evol at start of state

	}

	public void ChangeWorld(bool toLW, int f, int t, bool tl, int fs, int ts)
	{
		// early updates
		if (toLW) toLightworld = true;												// set to light world flag
		if (!toLW) toDarkworld = true;												// set to dark world flag
		toWorld = toLW;																// store which world to switch to
		fromState = f;																// store from state
		toState = t;																// store to state
		toLight = tl;																// store to light
		fromShape = fs;																// store from shape
		toShape = ts;																// store to shape

		updateStateIndicator = true;												// update state indicator

		// set shape
		shape = toShape;															// set shape indicator
		if (toShape == 0) {															// if shape is circle
			circle = true;																// set circle flag
			triangle = false;															// reset triangle flag
			square = false;																// reset square flag
		}
		else if (toShape == 1) {													// if shape is circle
			circle = false;																// reset circle flag
			triangle = true;															// set triangle flag
			square = false;																// reset square flag
		}
		else if (toShape == 2) {													// if shape is circle
			circle = false;																// reset circle flag
			triangle = false;															// reset triangle flag
			square = true;																// set square flag
		}

		// part changes
		pcm.ToOtherWorld (toLW, f, t, tl, fs, ts);									// change core
		cwShell = true;																// start shell change timer
		cwNucleus = true;															// start nucleus change timer

		// physics changes
		if (toState == 0)	{ 														// to zero
			rb.mass = 1.0f;																// set mass
			gameObject.tag = "Zero";													// set tag
			sc[0].radius = 0.208f;														// update collision radius
			sc[1].radius = 0.205f;														// update collision radius
		}
		else if (toState == 1) {													// to first
			rb.mass = 2.0f;																// set mass
			gameObject.tag = "First";													// set tag
			sc[0].radius = 0.54f;														// update collision radius
			sc[1].radius = 0.52f;														// update collision radius
		}
		else if (toState == 2) {													// to second
			rb.mass = 2.5f;																// set mass
			gameObject.tag = "Second";													// set tag
			sc[0].radius = 0.54f;														// update collision radius
			sc[1].radius = 0.52f;														// update collision radius
		}
		else if (toState == 3) {													// to third
			rb.mass = 3.0f;																// set mass
			gameObject.tag = "Third";													// set tag
			if (toLight) {
				sc[0].radius = 1.06f;														// update collision radius
				sc[1].radius = 1.04f;														// update collision radius
			}
			else if (!toLight) {
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
		}
		else if (toState == 4) {													// to fourth
			rb.mass = 3.5f;																// set mass
			gameObject.tag = "Fourth";													// set tag
			if (toLight) {
				sc[0].radius = 1.06f;														// update collision radius
				sc[1].radius = 1.04f;														// update collision radius
			}
			else if (!toLight) {
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
		}
		else if (toState == 5) {													// to fifth
			rb.mass = 4.0f;																// set mass
			gameObject.tag = "Fifth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
			else if (toShape == 1 || toShape == 2) {									// if triangle or square
				sc[0].radius = 0.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
		}
		else if (toState == 6) {													// to sixth
			rb.mass = 4.5f;																// set mass
			gameObject.tag = "Sixth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 1.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
			else if (toShape == 1 || toShape == 2) {									// if triangle or square
				sc[0].radius = 0.54f;														// update collision radius
				sc[1].radius = 0.52f;														// update collision radius
			}
		}
		else if (toState == 7) {													// to seventh
			rb.mass = 5.5f;																// set mass
			gameObject.tag = "Seventh";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 5.2f;														// update collision radius
				sc[1].radius = 1.54f;														// update collision radius
			} 
			else if (!toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 5.2f;														// update collision radius
				sc[1].radius = 1.54f;														// update collision radius
			}
			else if (toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 3.25f;														// update collision radius
				sc[1].radius = 2.25f;														// update collision radius
			}
		}
		else if (toState == 8) {													// to eighth
			rb.mass = 7.0f;																// set mass
			gameObject.tag = "Eighth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 5.2f;														// update collision radius
				sc[1].radius = 1.54f;														// update collision radius
			} 
			else if (!toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 5.2f;														// update collision radius
				sc[1].radius = 1.54f;														// update collision radius
			}
			else if (toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 3.25f;														// update collision radius
				sc[1].radius = 2.25f;														// update collision radius
			}
		}
		else if (toState == 9) {													// to ninth
			rb.mass = 8.5f;																// set mass
			gameObject.tag = "Ninth";													// set tag
			if (toShape == 0) {															// if circle
				sc[0].radius = 5.2f;														// update collision radius
				sc[1].radius = 1.54f;														// update collision radius
			} 
			else if (!toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 2.06f;														// update collision radius
				sc[1].radius = 1.04f;														// update collision radius
			}
			else if (toLight && (toShape == 1 || toShape == 2)) {						// if triangle or square
				sc[0].radius = 1.06f;														// update collision radius
				sc[1].radius = 1.04f;														// update collision radius
			}
		}

		// late updates
		isLight = toLight;															// set light flag
		darkEvolStart = darkEvol;													// store dark evol at start of state
		lightEvolStart = lightEvol;													// store light evol at start of state

	}

}
