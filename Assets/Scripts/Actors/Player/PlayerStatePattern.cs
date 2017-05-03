using UnityEngine;
using System.Collections;

public class PlayerStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float darkEvolStart, lightEvolStart;								// last dark & light evolution level (for delta calc)
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public new bool light;													// is light flag
	public bool toLight;													// to light flag

	[HideInInspector] public IParticleState currentState;					// other object state

	//[HideInInspector] public DeadPlayerState deadState;						// instance of dead state
	[HideInInspector] public ZeroPlayerState zeroState;						// instance of photon state
	[HideInInspector] public FirstPlayerState firstState;					// instance of electron state
	[HideInInspector] public SecondPlayerState secondState;					// instance of electron2 state
	[HideInInspector] public ThirdPlayerState thirdState;					// instance of shell state
	[HideInInspector] public FourthPlayerState fourthState;					// instance of shell2 state
	[HideInInspector] public FifthPlayerState fifthState;					// instance of atom state
	[HideInInspector] public SixthPlayerState sixthState;					// instance of atom2 state
	[HideInInspector] public SeventhPlayerState seventhState;				// instance of element state
	// new state

	public bool lightworld;													// is light world flag
	public bool toLightworld, toDarkworld;									// to light world trigger, to dark world trigger
	public bool changeParticles;											// change particle colour flag

	// component references
	private CameraManager cam;													// main camera animator
	private PlayerCoreManager pcm;											// player core manager (for animations)
	private PlayerShellManager psm;											// player shell manager (for animations)
	private PlayerNucleusManager pnm;										// player nucleus manager (for animations)
	private UIManager uim;													// UI manager
	//private PlayerPhysicsManager ppm;										// player physics manager
	private SphereCollider[] sc;											// sphere colliders

	private MeshRenderer rendWorld, rendCore, rendShell, rendNucleus;		// mesh renderers (for lightworld colour changes)

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

	void Awake()
	{
		evol = 0f;															// initialize evol
		//darkEvol = 0f;														// initialize dark evol
		//lightEvol = 0f;														// initialize light evol

		deltaDark = 0f;														// initialize delta dark evol
		deltaLight = 0f;													// initialize delta light evol

		//deadState = new DeadPlayerState (this);								// initialize dead state
		zeroState = new ZeroPlayerState (this);								// initialize zero state
		firstState = new FirstPlayerState (this);							// initialize first state
		secondState = new SecondPlayerState (this);							// initialize second state
		thirdState = new ThirdPlayerState (this);							// initialize third state
		fourthState = new FourthPlayerState (this);							// initialize fourth state
		fifthState = new FifthPlayerState (this);							// initialize fifth state
		sixthState = new SixthPlayerState (this);							// initialize sixth state
		seventhState = new SeventhPlayerState (this);						// initialize seventh state
		// new state

		cam = transform.FindChild ("Follow Camera")
			.gameObject.GetComponent<CameraManager> ();						// initialize camera manager ref
		pcm = GameObject.Find("Player Core")
			.gameObject.GetComponent<PlayerCoreManager> ();					// initialize core manager ref
		psm = transform.FindChild ("Player Shell")
			.gameObject.GetComponent<PlayerShellManager>();					// initialize shell manager ref
		pnm = transform.FindChild ("Player Nucleus")
			.gameObject.GetComponent<PlayerNucleusManager>();				// initialize nucleus manager ref

		sc = GameObject.Find("Player Core")
			.gameObject.GetComponents<SphereCollider> ();					// init sphere collider ref

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

	}

	void Start () 
	{
		light = true;														// start as light
		//lightEvol = 0f;													// start at 0.5 light evol
		//darkEvol = 0f;													// start at 0.5 light evol
		currentState = zeroState;											// start at zero state
		//TransitionTo(0, 0, light, toLight, 0);								// start at zero size
	}

	void Update () 
	{
		evol = lightEvol + darkEvol;														// update total evol value

		deltaDark = darkEvol - darkEvolStart;												// calculate deltaDark
		deltaLight = lightEvol - lightEvolStart;											// calculate deltaLight

		/*if (toLightworld) {
			lightworld = true;
		}*/

		/*if (uim.uI.GetComponent<StartOptions> ().inMainMenu) {								// if in menu
			evol = 0f;																			// prevent evol changes (no death in menu)
		}*/

		currentState.UpdateState ();														// frame updates from current state class

		// trigger timed stun
		if (stunned) {
			//Stun ();
			stunTimer += Time.deltaTime;													// start timer
		} 

		//if (shellShrinking || nucleusDeactivating) shrinkTimer += Time.deltaTime;			// start timer

		// checks for OVERLAY TEXT
		if (!uim.uI.GetComponent<StartOptions>().inMainMenu && timeCheck == true) {			// if game start (not in menu)
			isInit = false;																		// reset is init flag
			sincePlaytimeBegin = Time.time;														// check time
			timeCheck = false;																	// check time only once
		}

	}

	private void OnTriggerEnter(Collider other)
	{
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
		lightEvol -= changeAmount;											// subtract dark evol level
	}

	public void AddLight(float changeAmount) {
		darkEvol += changeAmount;											// add light evol level
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

		//light = toLight;															// update light value

		if (toState == 0)	{ 														// to zero
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 1) {													// to first
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 2) {													// to second
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 3) {													// to third
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 4) {													// to fourth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 5) {													// to fifth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 6) {													// to sixth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 7) {													// to seventh
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(fromState, toState);											// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		//new state

		lastStateChange = Time.time;												// reset time since last state change

		darkEvolStart = darkEvol;													// store dark evol at start of state
		lightEvolStart = lightEvol;													// store light evol at start of state 
	}

	// camera - PUT IN SEPARATE SCRIPT
	private void SetZoomCamera (int fromState, int toState) 
	{
		if (toLightworld) {															// if light world
			cam.ZoomCamera (lightworld, fromState, toState);							// zoom camera in from particular state
			ChangeWorld ();																// switch properties
			cam.ZoomCamera (lightworld, fromState, toState);							// zoom camera out to appropriate state
		}
		else if (!lightworld && toDarkworld) {										// if not light world && switching back to dark world
			cam.ZoomCamera (lightworld, fromState, toState);							// zoom camera in from particular state
			ChangeWorld ();																// switch properties
			cam.ZoomCamera (lightworld, fromState, toState);							// zoom out to appropriate state

		}
		else cam.ZoomCamera (lightworld, fromState, toState);						// else in dark world, zoom between states
	}

	private void ChangeWorld () 
	{
		if (toLightworld) {															// if switching to light world from dark world
			lightworld = true;
			changeParticles = true;														// set change particle property trigger

			rendWorld.material.SetColor("_Color", Color.white);							// change world to white
			rendCore.material.SetColor("_Color", Color.black);							// change core to black
			rendShell.material.SetColor("_Color", Color.black);							// change shell to black
			rendNucleus.material.SetColor("_Color", Color.black);						// change nucleus to black

			//changeParticles = false;													// reset change particle property trigger
			toLightworld = false;														// reset to light world trigger
		}
		else if (toDarkworld) {														// if switching to dark world from light world
			lightworld = false;
			
			rendWorld.material.SetColor("_Color", Color.black);							// change world to white
			rendCore.material.SetColor("_Color", Color.white);							// change core to black
			rendShell.material.SetColor("_Color", Color.white);							// change shell to black
			rendNucleus.material.SetColor("_Color", Color.white);						// change nucleus to black
			
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