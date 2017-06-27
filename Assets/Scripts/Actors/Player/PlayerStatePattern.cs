using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class PlayerStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public int state;														// state indicator for inspector
	public new bool isLight;													// is light flag
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float darkEvolStart, lightEvolStart;								// last dark & light evolution level (for delta calc)
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public float evolC, darkEvolC, lightEvolC;								// evol values at start of collision

	public bool circle = true, triangle, square;							// shape flags

	private bool updateStateIndicator;										// update state indicator flag
	[HideInInspector] public IParticleState currentState;					// other object state

	[HideInInspector] public ZeroPlayerState zeroState;						// instance of zero state
	[HideInInspector] public FirstPlayerState firstState;					// instance of first state
	[HideInInspector] public SecondPlayerState secondState;					// instance of second state
	[HideInInspector] public ThirdPlayerState thirdState;					// instance of third state
	[HideInInspector] public FourthPlayerState fourthState;					// instance of fourth state
	[HideInInspector] public FifthPlayerState fifthState;					// instance of fifth state
	[HideInInspector] public SixthPlayerState sixthState;					// instance of sixth state
	[HideInInspector] public SeventhPlayerState seventhState;				// instance of seventh state
	[HideInInspector] public EighthPlayerState eighthState;					// instance of eighth state
	[HideInInspector] public NinthPlayerState ninthState;					// instance of ninth state
	// new state

	public bool lightworld;													// is light world flag
	public bool toLightworld, toDarkworld;									// to light world trigger, to dark world trigger
	public bool changeParticles;											// change particle colour flag

	// component references
	private CameraManager cam;												// main camera animator
	private PlayerCoreManager pcm;											// player core manager (for animations)
	private PlayerShellManager psm;											// player shell manager (for animations)
	private PlayerNucleusManager pnm;										// player nucleus manager (for animations)
	private UIManager uim;													// UI manager
	private Rigidbody rb;													// player rigidbody
	//private PlayerPhysicsManager ppm;										// player physics manager
	[HideInInspector] public SphereCollider[] sc;							// sphere colliders

	private MeshRenderer rendWorld, rendCore, rendShell, rendNucleus;		// mesh renderers (for lightworld colour changes)

	private int toState, fromState;											// state transitioning to/from (for timers)
	private bool changeWorld = false;										// timer trigger for changing colour, 
	[HideInInspector] public bool resetScale = false;						// timer trigger for resetting scale after world switch (public for camera manager access)
	private float changeWorldTimer, resetScaleTimer, changeParticlesTimer;	// change shape timer, reset scale timer, change particles timer

	// timers & flags
	public bool isInit = true;												// is init flag
	private int die;														// roll for collision conflicts
	public bool stunned;													// stunned?
	public float stunDuration = 5f;											// duration of post-hit invulnerability
	private float stunTimer = 0f;											// stun timer
	//private bool shellShrinking, shell2Shrinking, nucleusDeactivating;		// shell/shell2 shrinking flag, nucleus deactivating flag
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer
	// UI
	[HideInInspector] public float lastStateChange = 0.0f;					// since last state change
	[HideInInspector] public float sincePlaytimeBegin = 0.0f;				// since game start
	private bool timeCheck = true;											// time check flag

	// audio
	public AudioMixerSnapshot[] musicSnapshots;								// ref to music mixer snapshots
	public AudioMixerSnapshot[] effectsSnapshots;							// ref to main mixer effects snapshots

	void Awake()
	{
		//evol = 0f;															// initialize evol
		//darkEvol = 0f;														// initialize dark evol
		//lightEvol = 0f;														// initialize light evol

		deltaDark = 0f;														// initialize delta dark evol
		deltaLight = 0f;													// initialize delta light evol

		zeroState = new ZeroPlayerState (this);								// initialize zero state
		firstState = new FirstPlayerState (this);							// initialize first state
		secondState = new SecondPlayerState (this);							// initialize second state
		thirdState = new ThirdPlayerState (this);							// initialize third state
		fourthState = new FourthPlayerState (this);							// initialize fourth state
		fifthState = new FifthPlayerState (this);							// initialize fifth state
		sixthState = new SixthPlayerState (this);							// initialize sixth state
		seventhState = new SeventhPlayerState (this);						// initialize seventh state
		eighthState = new EighthPlayerState (this);							// initialize eighth state
		ninthState = new NinthPlayerState (this);							// initialize ninth state
		// new state

	}

	void Start () 
	{
		cam = transform.FindChild ("Follow Camera")
			.gameObject.GetComponent<CameraManager> ();						// initialize camera manager ref
		pcm = GameObject.Find("Player Core")
			.gameObject.GetComponent<PlayerCoreManager> ();					// initialize core manager ref
		psm = transform.FindChild ("Player Shell")
			.gameObject.GetComponent<PlayerShellManager>();					// initialize shell manager ref
		pnm = transform.FindChild ("Player Nucleus")
			.gameObject.GetComponent<PlayerNucleusManager>();				// initialize nucleus manager ref

		sc = GetComponents<SphereCollider> ();								// init sphere collider ref

		rb = GetComponent<Rigidbody> ();									// init rigidbody ref

		rendWorld = GameObject.Find("World")
			.GetComponent<MeshRenderer>();									// init world mesh renderer ref
		rendCore = GameObject.Find("Player Core")
			.gameObject.GetComponent<MeshRenderer> ();						// init core mesh renderer ref
		rendShell = transform.FindChild ("Player Shell")
			.gameObject.GetComponent<MeshRenderer> ();						// init shell mesh renderer ref
		rendNucleus = transform.FindChild ("Player Nucleus")
			.gameObject.GetComponent<MeshRenderer> ();						// init nucleus mesh renderer ref

		//ppm = GetComponent<PlayerPhysicsManager> ();						// init player physics manager ref

		uim = GetComponent<UIManager> ();									// init ui manager ref
		Destroy(GameObject.FindGameObjectWithTag("Destroy"));				// destroy old UI

		//light = true;														// start as light
		//lightEvol = 0f;													    // start at 0.5 light evol
		//darkEvol = 0f;													    // start at 0.5 light evol
		currentState = zeroState;											// start at zero state
		//TransitionTo(0, 0, light, toLight, 0);								// start at zero size
		state = 0;
	}

	void Update () 
	{
		evol = lightEvol + darkEvol;														// update total evol value

		deltaDark = darkEvol - darkEvolStart;												// calculate deltaDark
		deltaLight = lightEvol - lightEvolStart;											// calculate deltaLight


		/*if (uim.uI.GetComponent<StartOptions> ().inMainMenu) {								// if in menu
			evol = 0f;																			// prevent evol changes (no death in menu)
		}*/

		currentState.UpdateState ();														// frame updates from current state class

		// debug - show current state
		if (updateStateIndicator) {
			if (currentState == zeroState) state = 0;
			else if (currentState == firstState) state = 1;
			else if (currentState == secondState) state = 2;
			else if (currentState == thirdState) state = 3;
			else if (currentState == fourthState) state = 4;
			else if (currentState == fifthState) state = 5;
			else if (currentState == sixthState) state = 6;
			else if (currentState == seventhState) state = 7;
			else if (currentState == eighthState) state = 8;
			else if (currentState == ninthState) state = 9;
			//else if (currentState == tenthState) state = 10;
			updateStateIndicator = false;
		}

		// trigger timed stun
		if (stunned) {
			//Stun ();
			stunTimer += Time.deltaTime;													// start timer
		} 

		// change particles timer
		if (changeParticles) changeParticlesTimer += Time.deltaTime;														// start timer
		if (changeParticlesTimer >= 0.5f) {																				// when timer >= 2 sec
			changeParticles = false;																						// reset change colour flag
			changeParticlesTimer = 0f;																						// reset timer
		}
		// change colour timer
		if (changeWorld) changeWorldTimer += Time.deltaTime;														// start timer
		if (changeWorldTimer >= 2.5f) {																				// when timer >= 2 sec
			Debug.Log ("player changing world");
			ChangeWorld();
			changeWorld = false;																						// reset change colour flag
			changeWorldTimer = 0f;																						// reset timer
		}
		// reset scale timer
		if (resetScale) resetScaleTimer += Time.deltaTime;															// start timer
		if (resetScaleTimer >= 3.0f) {																				// when timer >= 3 sec
			Debug.Log ("reset camera zoom");
			cam.ZoomCamera (false, fromState, toState);															// zoom camera out to appropriate state
			resetScale = false;																							// reset reset scale flag
			resetScaleTimer = 0f;																						// reset timer
		}


		// checks for OVERLAY TEXT
		if (!uim.uI.GetComponent<StartOptions>().inMainMenu && timeCheck == true) {			// if game start (not in menu)
			isInit = false;																		// reset is init flag
			sincePlaytimeBegin = Time.time;														// check time
			timeCheck = false;																	// check time only once
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		evolC = evol;																		// store evol before collision changes
		darkEvolC = darkEvol;																// store dark evol before collision changes
		lightEvolC = lightEvol;																// store light evol before collision changes	

		currentState.OnTriggerEnter (other);												// pass collider into state class
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
		//ParticleStateEvents.toSeventh -= TransitionToSeventh;				// untrigger transition event
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
		//ParticleStateEvents.toSeventh -= TransitionToSeventh;				// untrigger transition event
		// new state
	}

	// EVOL CHANGES \\ - PUT IN SEPARATE SCRIPT


	public void AddEvol(float changeAmount) 
	{
		evol += changeAmount;												// add evol level
	}
	public void SubEvol(float changeAmount) 
	{
		evol -= changeAmount;												// subtract evol level
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

	// BEHAVIOURS \\ - PUT IN SEPARATE SCRIPT

	/*public void Stun ()											// post-hit invulnerability
	{
		if (GetComponent<SphereCollider> ().enabled) {
			//GetComponent<SphereCollider> ().enabled = false;					// disable collider
			//ppm.Bump (true);													// enable bump
		}

		if (stunTimer >= stunDuration) {									// if timer >= duration
			//GetComponent<SphereCollider> ().enabled = true;						// enable collider
			stunned = false;													// reset stunned flag
			stunTimer = 0f;														// reset timer
		}
	}*/

	public void SpawnZero (int num)
	{
		GetComponent<SpawnParticle> ().SpawnPhoton (num);
	}

	public void SpawnFirst (int num)
	{
		GetComponent<SpawnParticle> ().SpawnElectron (num);
	}

	// STATE TRANSTITIONS \\

	public void TransitionTo (int fromState, int toState, bool fromLight, bool toLight, int shape)
	{

		updateStateIndicator = true;

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

		Debug.Log ("player transition to");

		if (toState == 0) { 														// to zero
			rb.mass = 1.0f;																// set mass
			sc[0].radius = 0.205f;														// update collision radius	
			sc[1].radius = 0.200f;														// update collision radius
			if (toLightworld) Debug.Log ("player to light world");
			if (toDarkworld) Debug.Log ("player to dark world");
			if (!toLightworld) effectsSnapshots[0].TransitionTo(5.0f);					// AUDIO: transition to default/dark world effects snapshot
			if (toLightworld) effectsSnapshots[5].TransitionTo(5.0f);					// AUDIO: transition to light world effects snapshot
			if (evol == 0f) musicSnapshots[0].TransitionTo(5.0f);						// AUDIO: transition to zero state music snapshot
			if (evol == 0.5f) musicSnapshots[1].TransitionTo(5.0f);						// AUDIO: transition to half zero state music snapshot
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 1) {													// to first
			rb.mass = 2.0f;																// set mass
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			//sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world	
			musicSnapshots[2].TransitionTo(5.0f);										// AUDIO: transition to first state music snapshot
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 2) {													// to second
			rb.mass = 2.5f;																// set mass
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			musicSnapshots[3].TransitionTo(5.0f);										// AUDIO: transition to second state music snapshot
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 3) {													// to third
			rb.mass = 3.0f;																// set mass
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 1.00f;														// update collision radius
			musicSnapshots[4].TransitionTo(5.0f);										// AUDIO: transition to third state music snapshot	
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 4) {													// to fourth
			rb.mass = 3.5f;																// set mass
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 1.00f;														// update collision radius
			musicSnapshots[5].TransitionTo(5.0f);										// AUDIO: transition to fourth state music snapshot	
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 5) {													// to fifth
			rb.mass = 4.0f;																// set mass
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			musicSnapshots[6].TransitionTo(5.0f);										// AUDIO: transition to fifth state music snapshot
			if (shape == 0 && !isLight) effectsSnapshots[1].TransitionTo(5.0f);			// AUDIO: transition to dark circle effects snapshot
			if (shape == 0 && isLight) effectsSnapshots[2].TransitionTo(5.0f);			// AUDIO: transition to light circle effects snapshot
			if (shape == 1) effectsSnapshots[3].TransitionTo(5.0f);						// AUDIO: transition to triangle effects snapshot
			if (shape == 2) effectsSnapshots[4].TransitionTo(5.0f);						// AUDIO: transition to square effects snapshot
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 6) {													// to sixth
			rb.mass = 4.5f;																// set mass
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			musicSnapshots[7].TransitionTo(5.0f);										// AUDIO: transition to sixth state music snapshot
			if (shape == 0 && !isLight) effectsSnapshots[1].TransitionTo(5.0f);			// AUDIO: transition to dark circle effects snapshot
			if (shape == 0 && isLight) effectsSnapshots[2].TransitionTo(5.0f);			// AUDIO: transition to light circle effects snapshot
			if (shape == 1) effectsSnapshots[3].TransitionTo(5.0f);						// AUDIO: transition to triangle effects snapshot
			if (shape == 2) effectsSnapshots[4].TransitionTo(5.0f);						// AUDIO: transition to square effects snapshot	
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 7) {													// to seventh
			rb.mass = 5.5f;																// set mass
			sc[0].radius = 1.53f;														// update collision radius
			sc[1].radius = 1.50f;														// update collision radius
			musicSnapshots[8].TransitionTo(5.0f);										// AUDIO: transition to seventh state music snapshot
			if (shape == 0 && !isLight) effectsSnapshots[1].TransitionTo(5.0f);			// AUDIO: transition to dark circle effects snapshot
			if (shape == 0 && isLight) effectsSnapshots[2].TransitionTo(5.0f);			// AUDIO: transition to light circle effects snapshot
			if (shape == 1) effectsSnapshots[3].TransitionTo(5.0f);						// AUDIO: transition to triangle effects snapshot
			if (shape == 2) effectsSnapshots[4].TransitionTo(5.0f);						// AUDIO: transition to square effects snapshot
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 8) {													// to eighth
			rb.mass = 7.0f;																// set mass
			sc[0].radius = 1.53f;														// update collision radius
			sc[1].radius = 1.50f;														// update collision radius
			musicSnapshots[9].TransitionTo(5.0f);										// AUDIO: transition to seventh state music snapshot
			if (shape == 0 && !isLight) effectsSnapshots[1].TransitionTo(5.0f);			// AUDIO: transition to dark circle effects snapshot
			if (shape == 0 && isLight) effectsSnapshots[2].TransitionTo(5.0f);			// AUDIO: transition to light circle effects snapshot
			if (shape == 1) effectsSnapshots[3].TransitionTo(5.0f);						// AUDIO: transition to triangle effects snapshot
			if (shape == 2) effectsSnapshots[4].TransitionTo(5.0f);						// AUDIO: transition to square effects snapshot
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 9) {													// to eighth
			rb.mass = 8.5f;																// set mass
			sc[0].radius = 2.04f;														// update collision radius
			sc[1].radius = 2.00f;														// update collision radius
			musicSnapshots[10].TransitionTo(5.0f);										// AUDIO: transition to seventh state music snapshot
			if (shape == 0 && !isLight) effectsSnapshots[1].TransitionTo(5.0f);			// AUDIO: transition to dark circle effects snapshot
			if (shape == 0 && isLight) effectsSnapshots[2].TransitionTo(5.0f);			// AUDIO: transition to light circle effects snapshot
			if (shape == 1) effectsSnapshots[3].TransitionTo(5.0f);						// AUDIO: transition to triangle effects snapshot
			if (shape == 2) effectsSnapshots[4].TransitionTo(5.0f);						// AUDIO: transition to square effects snapshot
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		//new state 10 mass = 10

		isLight = toLight;															// update light value

		lastStateChange = Time.time;												// reset time since last state change

		darkEvolStart = darkEvol;													// store dark evol at start of state
		lightEvolStart = lightEvol;													// store light evol at start of state 
	}

	// camera - PUT IN SEPARATE SCRIPT
	private void SetZoomCamera (int from, int to) 
	{
		fromState = from;
		toState = to;
		if (toLightworld || toDarkworld) {											// if changing worlds
			cam.ZoomCamera (true, from, to);											// zoom camera in from particular state
			changeWorld = true;															// switch properties
			resetScale = true;															// zoom camera out to appropriate state
		} 
		else {
			cam.ZoomCamera (false, from, to);										// else in dark world, zoom between states
		}
	}

	private void ChangeWorld () 
	{
		if (toLightworld) {															// if switching to light world from dark world
			lightworld = true;
			changeParticles = true;														// set change particle property trigger

			rendWorld.material.SetColor("_Color", Color.white);							// change world to white
			//pcm.SetLight (false);														// set core to black
			rendCore.material.SetColor("_Color", Color.black);							// change core to black
			rendShell.material.SetColor("_Color", Color.black);							// change shell to black
			rendNucleus.material.SetColor("_Color", Color.white);						// change nucleus to white

			effectsSnapshots[5].TransitionTo(5.0f);										// AUDIO: transition to light world effects snapshot

			//changeParticles = false;													// reset change particle property trigger
			toLightworld = false;														// reset to light world trigger
		}
		else if (toDarkworld) {														// if switching to dark world from light world
			lightworld = false;
			changeParticles = true;

			rendWorld.material.SetColor("_Color", Color.black);							// change world to white
			//pcm.SetLight (true);														// set core to white
			rendCore.material.SetColor("_Color", Color.white);							// change core to white
			rendShell.material.SetColor("_Color", Color.white);							// change shell to black
			rendNucleus.material.SetColor("_Color", Color.black);						// change nucleus to black
			
			toDarkworld = false;														// reset to dark world trigger
		}

	}

	// set player parts
	private void SetParts(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		pcm.Core (fromState, toState, fromLight, toLight, shape);					// change circle
		psm.Shell (fromState, toState, fromLight, toLight, shape);					// change shell
		pnm.Nucleus(fromState, toState, fromLight, toLight, shape);					// change nucleus
	}

}