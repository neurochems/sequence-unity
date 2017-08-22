using UnityEngine;
using System.Collections;

public class ParticleStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public int state;														// state indicator for inspector
	public new bool isLight;												// is light flag
	public int currentShape;												// current shape
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float darkEvolStart, lightEvolStart;								// last dark & light evolution level (for delta calc)
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public float evolC, darkEvolC, lightEvolC;								// evol values at start of collision

	public bool circle = true, triangle, square;							// shape flags

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
	public bool toLightworld, toDarkworld, inLightworld;					// to light world trigger, to dark world trigger, in light world flag
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

	private int fromState, toState, shape;									// transition properties
	private bool fromLight, toLight;										// transition properties
	//private bool toLightworld;													// transition properties: to light world

	private bool setShell, setNucleus;										// set part flags
	private bool cwShell, cwNucleus;										// set change world part flags
	private float setShellTimer, setNucleusTimer;							// set part timers
	private float cwShellTimer, cwNucleusTimer;								// set change world part timers
	private float inLightworldTimer;										// set change world part timers

	public int die;															// collision conflict check
	public bool roll;														// collision conflict check
	public bool stunned;													// stunned?
	public float stunDuration = 3f;											// duration of post-hit invulnerability
	private float stunTimer = 0f, activeTimer = 0f;							// stun timer, active timer

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
		evol = lightEvol + darkEvol;										// update total evol value

		deltaDark = darkEvol - darkEvolStart;								// calculate deltaDark
		deltaLight = lightEvol - lightEvolStart;							// calculate deltaLight

		lightworld = psp.lightworld;										// update if lightworld
		changeParticles = psp.changeParticles;								// if changeParticles is true, update from player state pattern

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

		// set part world change timers
		if (cwShell) {														// if set shell
			cwShellTimer += Time.deltaTime;									// start timer
			if (cwShellTimer >= 0.1f) {										// if timer is 0.1 sec
				psm.ToOtherWorld (toLightworld, fromState, toState, toLight);		// change shell
				cwShell = false;													// reset set shell flag
				cwShellTimer = 0f;													// reset set shell timer
			}
		}
		if (cwNucleus) {													// if set nucleus
			setNucleusTimer += Time.deltaTime;									// start timer
			if (setNucleusTimer >= 0.2f) {										// if timer is 0.2 sec
				pnm.ToOtherWorld (toLightworld, fromState, toState, toLight);		// change nucleus
				cwNucleus = false;													// reset set nucleus flag
				cwNucleusTimer = 0f;												// reset set nucleus timer
			}
		}

		if (toLightworld || toDarkworld) {										// if to opposite world
			inLightworldTimer += Time.deltaTime;									// start timer
			if (inLightworldTimer >= 2.5f) {										// if timer is 2.5 sec
				if (toLightworld) {
					inLightworld = true;												// set in lightworld flag
					toLightworld = false;												// reset to light world flag
					sc[0].enabled = true;												// enable trigger collider
				}
				else if (toDarkworld) {
					inLightworld = false;												// set in lightworld flag
					toDarkworld = false;												// reset to light world flag
					sc[0].enabled = true;												// enable trigger collider
				}
				inLightworldTimer = 0f;												// reset timer
			}
		}

		// set parts timers
		if (setShell) {														// if set shell
			setShellTimer += Time.deltaTime;									// start timer
			if (setShellTimer >= 0.75f) {										// if timer is 0.75 sec
				psm.Shell (fromState, toState, fromLight, toLight, shape);			// change shell
				setShell = false;													// reset set shell flag
				setShellTimer = 0f;													// reset set shell timer
			}
		}
		if (setNucleus) {													// if set nucleus
			setNucleusTimer += Time.deltaTime;									// start timer
			if (setNucleusTimer >= 1.5f) {										// if timer is 1.5 sec
				pnm.Nucleus (fromState, toState, fromLight, toLight, shape);		// change nucleus
				setNucleus = false;													// reset set nucleus flag
				setNucleusTimer = 0f;												// reset set nucleus timer
			}
		}

		// tenth state deactivation timer
		if (psp.state == 10) {												// if player is state 10
			activeTimer += Time.deltaTime;										// start timer
			TransitionTo(state, 10, isLight, false, currentShape);				// transition to hidden
			if (activeTimer >= 4.0f) {											// if timer is 4 sec
				gameObject.SetActive (false);										// deactivate
				activeTimer = 0f;													// reset active timer
			}
		}

		// roll die
		if (roll) {
			die = Random.Range(1,6);														// roll die
			roll = false;																	// reset flag
		}

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

	public void TransitionTo(int f, int t, bool fl, bool tl, int s)
	{

		fromState = f;																// store from state
		toState = t;																// store to state
		fromLight = fl;																// store from light
		toLight = tl;																// store to light
		shape = s;																	// store shape

		updateStateIndicator = true;												// update state indicator

		if (shape == 0) {															// if shape is circle
			circle = true;																// set circle flag
			triangle = false;															// reset triangle flag
			square = false;																// reset square flag
		}
		else if (shape == 1) {														// if shape is circle
			circle = false;																// reset circle flag
			triangle = true;															// set triangle flag
			square = false;																// reset square flag
		}
		else if (shape == 2) {														// if shape is circle
			circle = false;																// reset circle flag
			triangle = false;															// reset triangle flag
			square = true;																// set square flag
		}

		if (toState == 0)	{ 														// to zero
			SetParts();																	// set player parts
			rb.mass = 1.0f;																// set mass
			gameObject.tag = "Zero";													// set tag
			sc[0].radius = 0.205f;														// update collision radius
			sc[1].radius = 0.195f;														// update collision radius
			//Debug.Log(psp.gameObject.name + " nucleus init to dark zero - TransitionTo(0)");
		}
		else if (toState == 1) {													// to first
			SetParts();																	// set player parts
			rb.mass = 2.0f;																// set mass
			gameObject.tag = "First";													// set tag
			sc[0].radius = 0.52f;														// update collision radius
			sc[1].radius = 0.48f;														// update collision radius
		}
		else if (toState == 2) {													// to second
			SetParts();																	// set player parts
			rb.mass = 2.5f;																// set mass
			gameObject.tag = "Second";													// set tag
			sc[0].radius = 0.52f;														// update collision radius
			sc[1].radius = 0.48f;														// update collision radius
		}
		else if (toState == 3) {													// to third
			SetParts();																	// set player parts
			rb.mass = 3.0f;																// set mass
			gameObject.tag = "Third";													// set tag
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 0.97f;														// update collision radius
		}
		else if (toState == 4) {													// to fourth
			SetParts();																	// set player parts
			rb.mass = 3.5f;																// set mass
			gameObject.tag = "Fourth";													// set tag
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 0.97f;														// update collision radius
		}
		else if (toState == 5) {													// to fifth
			SetParts();																	// set player parts
			rb.mass = 4.0f;																// set mass
			gameObject.tag = "Fifth";													// set tag
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 0.97f;														// update collision radius
		}
		else if (toState == 6) {													// to sixth
			SetParts();																	// set player parts
			rb.mass = 4.5f;																// set mass
			gameObject.tag = "Sixth";													// set tag
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 0.97f;														// update collision radius
		}
		else if (toState == 7) {													// to seventh
			SetParts();																	// set player parts
			rb.mass = 5.5f;																// set mass
			gameObject.tag = "Seventh";													// set tag
			sc[0].radius = 1.53f;														// update collision radius
			sc[1].radius = 1.47f;														// update collision radius
		}
		else if (toState == 8) {													// to eighth
			SetParts();																	// set player parts
			rb.mass = 7.0f;																// set mass
			gameObject.tag = "Eighth";													// set tag
			sc[0].radius = 1.53f;														// update collision radius
			sc[1].radius = 1.47f;														// update collision radius
		}
		else if (toState == 9) {													// to ninth
			SetParts();																	// set player parts
			rb.mass = 8.5f;																// set mass
			gameObject.tag = "Ninth";													// set tag
			sc[0].radius = 2.03f;														// update collision radius
			sc[1].radius = 1.98f;														// update collision radius
		}

		isLight = toLight;															// set light flag
		currentShape = shape;														// set current shape

		darkEvolStart = darkEvol;													// store dark evol at start of state
		lightEvolStart = lightEvol;													// store light evol at start of state
	}

	public void ChangeWorld(bool toLW, int f, int t, bool tl) 
	{
		if (toLW) toLightworld = true;												// set to light world flag
		if (!toLW) toDarkworld = true;												// set to dark world flag
		fromState = f;																// store from state
		toState = t;																// store to state
		toLight = tl;																// store to light

		pcm.ToOtherWorld (toLW, f, t, tl);											// change core
		cwShell = true;																// start shell change timer
		cwNucleus = true;															// start nucleus change timer

		if (toState == 0) gameObject.tag = "Zero";									// if zero, set tag
		else if (toState == 1) gameObject.tag = "First";							// if first, set tag
		else if (toState == 2) gameObject.tag = "Second";							// if second, set tag
		else if (toState == 3) gameObject.tag = "Third";							// if third, set tag
		else if (toState == 4) gameObject.tag = "Fourth";							// if fourth, set tag
		else if (toState == 5) gameObject.tag = "Fifth";							// if fifth, set tag
		else if (toState == 6) gameObject.tag = "Sixth";							// if sixth, set tag
		else if (toState == 7) gameObject.tag = "Seventh";							// if seventh, set tag
		else if (toState == 8) gameObject.tag = "Eighth";							// if eighth, set tag
		else if (toState == 9) gameObject.tag = "Ninth";							// if ninth, set tag

		updateStateIndicator = true;												// update state indicator

	}

	// set particle parts (normal state transitions)
	private void SetParts()
	{
		pcm.Core (fromState, toState, fromLight, toLight, shape);					// change circle
		setShell = true;															// start set shell timer
		setNucleus = true;															// start set nucleus timer
	}

}
