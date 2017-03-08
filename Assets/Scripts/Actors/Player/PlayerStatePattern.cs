using UnityEngine;
using System.Collections;

public class PlayerStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	[HideInInspector] public IParticleState currentState;					// other object state

	[HideInInspector] public DeadPlayerState deadState;						// instance of dead state
	[HideInInspector] public ZeroPlayerState zeroState;						// instance of photon state
	[HideInInspector] public FirstPlayerState firstState;					// instance of electron state
	[HideInInspector] public SecondPlayerState secondState;					// instance of electron2 state
	[HideInInspector] public ThirdPlayerState thirdState;					// instance of shell state
	[HideInInspector] public FourthPlayerState fourthState;					// instance of shell2 state
	[HideInInspector] public FifthPlayerState fifthState;					// instance of atom state
	[HideInInspector] public SixthPlayerState sixthState;					// instance of atom2 state
	[HideInInspector] public SeventhPlayerState seventhState;				// instance of element state
	// new state

	[HideInInspector] public GameObject self;								// reference to this gameobject
	private GameObject nucleus, shell, shell2;								// reference to nucleus and shell children

	// component references
	private UIManager uim;													// UI manager
	//private PlayerPhysicsManager ppm;										// player physics manager
	private SphereCollider[] sc;											// sphere colliders

	// timers & flags
	public bool stunned;													// stunned?
	public float stunDuration = 5f;											// duration of post-hit invulnerability
	private int die;														// roll for collision conflicts
	private float stunTimer = 0f;											// stun timer
	private bool shellShrinking, shell2Shrinking, nucleusDeactivating;		// shell/shell2 shrinking flag, nucleus deactivating flag
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer
	[HideInInspector] public float lastStateChange = 0.0f;					// since last state change
	[HideInInspector] public float sincePlaytimeBegin = 0.0f;				// since game start
	private bool timeCheck = true;											// time check flag

	void Awake()
	{
		evol = 0f;															// initialize evol
		darkEvol = 0f;														// initialize dark evol
		lightEvol = 0f;														// initialize light evol

		deltaDark = 0f;														// initialize delta dark evol
		deltaLight = 0f;														// initialize delta light evol

		deadState = new DeadPlayerState (this);								// initialize dead state
		zeroState = new ZeroPlayerState (this);								// initialize zero state
		firstState = new FirstPlayerState (this);							// initialize first state
		secondState = new SecondPlayerState (this);							// initialize second state
		thirdState = new ThirdPlayerState (this);							// initialize third state
		fourthState = new FourthPlayerState (this);							// initialize fourth state
		fifthState = new FifthPlayerState (this);							// initialize fifth state
		sixthState = new SixthPlayerState (this);							// initialize sixth state
		seventhState = new SeventhPlayerState (this);						// initialize seventh state
		// new state

		nucleus = transform.FindChild ("Player Nucleus").gameObject;		// initialize nucleus
		shell = transform.FindChild ("Player Shell").gameObject;			// initialize shell
		shell2 = transform.FindChild ("Player Shell 2").gameObject;			// initialize shell2

		uim = GetComponent<UIManager> ();									// init ui manager ref
		//ppm = GetComponent<PlayerPhysicsManager> ();						// init player physics manager ref
		sc = GetComponents<SphereCollider> ();								// init sphere collider ref

		self = gameObject;													// init self reference

		Destroy(GameObject.FindGameObjectWithTag("Destroy"));				// destroy old UI

	}

	void Start () 
	{
		currentState = zeroState;											// start at photon state
		TransitionToZero(0, gameObject);										// CORE: shrink to photon size, fade to white
	}

	void Update () 
	{
		if (uim.uI.GetComponent<StartOptions> ().inMainMenu) {								// if in menu
			evol = 0f;																			// prevent evol changes (no death in menu)
		}

		currentState.UpdateState ();														// frame updates from current state class

		// trigger timed stun
		if (stunned) {
			Stun ();
			stunTimer += Time.deltaTime;													// start timer
		} 

		if (shellShrinking || nucleusDeactivating) shrinkTimer += Time.deltaTime;			// start timer

		// checks for OVERLAY TEXT
		if (!uim.uI.GetComponent<StartOptions>().inMainMenu && timeCheck == true) {			// if game start (not in menu)
			sincePlaytimeBegin = Time.time;																// check time
			timeCheck = false;																		// check time only once
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		currentState.OnTriggerEnter (other);								// pass collider into state class
	}

	void OnDisable()
	{
		ParticleStateEvents.toZero -= TransitionToZero;						// untrigger transition event
		ParticleStateEvents.toFirst -= TransitionToFirst;					// untrigger transition event
		ParticleStateEvents.toSecond -= TransitionToSecond;					// untrigger transition event
		ParticleStateEvents.toThird -= TransitionToThird;					// untrigger transition event
		ParticleStateEvents.toFourth -= TransitionToFourth;					// untrigger transition event
		ParticleStateEvents.toFifth -= TransitionToFifth;					// untrigger transition event
		ParticleStateEvents.toSixth -= TransitionToSixth;					// untrigger transition event
		ParticleStateEvents.toSeventh -= TransitionToSeventh;				// untrigger transition event
		// new state
	}

	void OnDestroy()
	{
		ParticleStateEvents.toZero -= TransitionToZero;						// untrigger transition event
		ParticleStateEvents.toFirst -= TransitionToFirst;					// untrigger transition event
		ParticleStateEvents.toSecond -= TransitionToSecond;					// untrigger transition event
		ParticleStateEvents.toThird -= TransitionToThird;					// untrigger transition event
		ParticleStateEvents.toFourth -= TransitionToFourth;					// untrigger transition event
		ParticleStateEvents.toFifth -= TransitionToFifth;					// untrigger transition event
		ParticleStateEvents.toSixth -= TransitionToSixth;					// untrigger transition event
		ParticleStateEvents.toSeventh -= TransitionToSeventh;				// untrigger transition event
		// new state
	}

	// EVOL CHANGES \\

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
	public void SubtractDark(float changeAmount) {
		lightEvol -= changeAmount;											// subtract dark evol level
	}

	public void AddLight(float changeAmount) {
		darkEvol += changeAmount;											// add light evol level
	}
	public void SubtractLight(float changeAmount) {
		lightEvol -= changeAmount;											// subtract light evol level
	}

	// public float TotalEvol()
		// darkEvol + lightEvol

	// BEHAVIOURS \\

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

	public void TransitionTo(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		if (toState == 0)	{ 													// to zero
			// rb.mass = 0.2f;															// set mass
			Core (fromState, toState, fromLight, toLight, shape);					// CORE: to zero circle
		}
		else if (toState == 1) {												// to first
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);			// CAMERA: zoom to size 20
			Shell (fromState, toState, fromLight, toLight);							// SHELL: shrink, deactivate
		}
		else if (toState == 2) {												// to second
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);			// CAMERA: zoom to size 20
			Shell (fromState, toState, fromLight, toLight);							// SHELL: shrink, deactivate
			Nucleus (fromState, toState, fromLight, toLight);						// NUCLEUS: fade to white, deactivate
		}
		else if (toState == 3) {												// to third
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);			// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;													// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);								// level circle collider on world
			sc[1].radius = 0.51f;													// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			Core (fromState, toState, fromLight, toLight, shape);					// CORE: to zero circle
			Shell (fromState, toState, fromLight, toLight);							// SHELL: shrink, deactivate
			Nucleus (fromState, toState, fromLight, toLight);						// NUCLEUS: fade to white, deactivate
		}
		else if (toState == 4) {												// to fourth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);			// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;													// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;													// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			Core (fromState, toState, fromLight, toLight, shape);					// CORE: to zero circle
			Shell (fromState, toState, fromLight, toLight);							// SHELL: shrink, deactivate
			Nucleus (fromState, toState, fromLight, toLight);						// NUCLEUS: fade to white, deactivate
		}
		else if (toState == 5) {												// to fifth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);			// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;													// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;													// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			Core (fromState, toState, fromLight, toLight, shape);					// CORE: to zero circle
			Shell (fromState, toState, fromLight, toLight);							// SHELL: shrink, deactivate
			Nucleus (fromState, toState, fromLight, toLight);						// NUCLEUS: fade to white, deactivate
		}
		else if (toState == 6) {												// to sixth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);			// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;													// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;													// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			Core (fromState, toState, fromLight, toLight, shape);					// CORE: to zero circle
			Shell (fromState, toState, fromLight, toLight);							// SHELL: shrink, deactivate
			Nucleus (fromState, toState, fromLight, toLight);						// NUCLEUS: fade to white, deactivate
		}
		else if (toState == 7) {												// to seventh
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);			// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;													// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;													// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			Core (fromState, toState, fromLight, toLight, shape);					// CORE: to zero circle
			Shell (fromState, toState, fromLight, toLight);							// SHELL: shrink, deactivate
			Nucleus (fromState, toState, fromLight, toLight);						// NUCLEUS: fade to white, deactivate
		}
		//new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	/*public void TransitionToDead(int prevState, GameObject particle)
	{
		uim.Dead(true);														// trigger fade out/text

		GetComponent<SphereCollider> ().enabled = false;					// disable collider at start to prevent collisions during shrink
		GetComponent<Animator> ().SetTrigger("fadeout");					// enable core to black animation
		GetComponent<Animator>().SetBool("black", true);					// enable black core animation state
		GetComponent<Animator>().SetBool("dead", true);						// enable black core animation state

		if (evol > 2f) {
			//shell.GetComponent<SphereCollider> ().enabled = false;				// disable shell collider at start to prevent collisions during shrink
			shell.GetComponent<Animator> ().SetTrigger("shrink");				// enable shell shrink animation
			shell.GetComponent<Animator> ().SetBool("shell", false);			// reset shell animation state
		}

		lastStateChange = Time.time;										// reset time since last state change
	}

	public void TransitionToZero(bool light, int prevState, GameObject particle)
	{
		// devolving from dark/light zero
		if (prevState == 0)	{ 											
			// rb.mass = 0.2f;													// set mass
			Core (prevState, 0, light);										// CORE: to zero circle
		}
		// devolving from first
		else if (prevState == 1) {										
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "electron", true);						// CAMERA: zoom to size 20
			Core (prevState, 0, light);										// CORE: to zero circle
		}
		// devolving from second
		else if (prevState == 2) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "electron", true);						// CAMERA: zoom to size 20
			Core (prevState, 0, light);										// CORE: to zero circle
			Nucleus (prevState, 0, light);									// NUCLEUS: fade to white, deactivate
		}
		// devolving from third
		else if (prevState == 3) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "shell", true);							// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			Core (prevState, 0, light);										// CORE: shrink to zero, fade to white
			Shell (prevState, 0, light);									// SHELL: shrink, deactivate
			Nucleus (prevState, 0, light);									// NUCLEUS: fade to white, deactivate
		}
		// devolving from fourth
		else if (prevState == 4) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "shell", true);							// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			Core (prevState, 0, light);										// CORE: shrink to zero, fade to white
			Shell (prevState, 0, light);									// SHELL: shrink, deactivate
			Nucleus (prevState, 0, light);									// NUCLEUS: fade to white, deactivate
			CoreToWhite ();													// CORE: return to idle white
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable (); 												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from fifth
		else if (prevState == 5) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "atom", true);							// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from sixth
		else if (prevState == 6) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "atom", true);							// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable();												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from seventh
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "element", true);						// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
		}
		//new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	// public void TransitionToDarkZero( bool light, int prevState, GameObject particle)
	// public void TransitionToLightZero( bool light, int prevState, GameObject particle)

	public void TransitionToFirst(bool light, int prevState, GameObject particle)
	{
		// evolving from photon
		if (prevState == 0)	{ 											
			// rb.mass = 0.5f;													// set mass
			SetZoomCamera("electron", "photon", false);							// CAMERA: zoom to size 25
			CoreToElectron ();													// CORE: grow to electron size, is white
		}
		// devolving from electron2
		else if (prevState == 2) {										
			//rb.mass = 0.5f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from shell
		else if (prevState == 3) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("electron", "shell", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusToWhite ();												// NUCLEUS: fade to white
		}
		// devolving from shell2
		else if (prevState == 4) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("electron", "shell", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from atom
		else if (prevState == 5) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("electron", "atom", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("electron", "atom", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
			NucleusToWhite ();												// NUCLEUS: fade to white
		}
		// devolving from element
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("electron", "element", true);						// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
		}
		//new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToSecond(bool light, int prevState, GameObject particle)
	{
		// evolving from electron
		if (prevState == 1)	{ 											
			// rb.mass = 0.75f;													// set mass
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
		}
		// devolving from shell
		else if (prevState == 3) {										
			//rb.mass = 0.75f;													// set mass
			SetZoomCamera("electron", "shell", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from shell2
		else if (prevState == 4) {										
			//rb.mass = 0.75f;													// set mass
			SetZoomCamera("electron", "shell", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom
		else if (prevState == 5) {										
			//rb.mass = 0.75f;													// set mass
			SetZoomCamera("electron", "atom", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("electron", "atom", true);						// CAMERA: zoom to size 25
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from element
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("electron", "element", true);						// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToThird(bool light, int prevState, GameObject particle)
	{
		// evolving from electron
		if (prevState == 1)	{ 											
			// rb.mass = 0.5f;													// set mass
			SetZoomCamera("shell", "electron", false);						// CAMERA: zoom to size 40
			sc[0].radius = 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToBlack();													// CORE: fade to black
			ShellGrow ();													// SHELL: grow
		}
		// evolving from electron2
		else if (prevState == 2) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("shell", "electron", false);						// CAMERA: zoom to size 40
			sc[0].radius = 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToBlack ();													// CORE: fade to black
			ShellGrow ();													// SHELL: grow
		}
		// devolving from shell2
		else if (prevState == 4) {										
			//rb.mass = 0.5f;													// set mass
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom
		else if (prevState == 5) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("shell", "atom", true);							// CAMERA: zoom to size 40
			CoreToBlack ();													// CORE: fade to black
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("shell", "atom", true);							// CAMERA: zoom to size 25
			CoreToBlack ();													// CORE: fade to black
		}
		// devolving from element
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("shell", "element", true);						// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
			CoreToBlack();													// CORE: fade to black
			ShellGrow ();													// SHELL: grow
		}
		// new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToFourth(bool light, int prevState, GameObject particle)
	{
		// evolving from electron2
		if (prevState == 2)	{ 											
			// rb.mass = 0.75f;													// set mass
			SetZoomCamera("shell", "electron", false);						// CAMERA: zoom to size 40
			sc[0].radius= 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToBlack ();													// CORE: fade to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
			ShellGrow ();													// SHELL: grow
		}
		// evolving from shell
		else if (prevState == 3) {										
			//rb.mass = 0.75f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from atom
		else if (prevState == 5) {										
			//rb.mass = 0.75f;													// set mass
			SetZoomCamera("shell", "atom", true);							// CAMERA: zoom to size 40
			CoreToBlack ();													// CORE: fade to black
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.5f;													// set mass
			SetZoomCamera("shell", "atom", true);							// CAMERA: zoom to size 25
			CoreToBlack ();													// CORE: fade to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from element
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("shell", "element", true);						// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
			CoreToBlack();													// CORE: fade to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
			ShellGrow ();													// SHELL: grow
		}
		// new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToFifth(bool light, int prevState, GameObject particle)
	{
		// evolving from shell2
		if (prevState == 4)	{ 											
			// rb.mass = 0.5f;													// set mass
			SetZoomCamera("atom", "shell", false);						// CAMERA: zoom to size 40
			CoreToWhite ();													// CORE: fade to white
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.5f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from element
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("atom", "element", true);							// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
			ShellGrow ();													// SHELL: grow
		}
		// new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	// public void TransitionToFifthTriangle(bool light, int prevState, GameObject particle)
	// public void TransitionToFifthSquare(bool light, int prevState, GameObject particle)

	public void TransitionToSixth(bool light, int prevState, GameObject particle)
	{
		// evolving from atom
		if (prevState == 5)	{ 											
			// rb.mass = 0.5f;													// set mass
			NucleusToBlack ();												// NUCLEUS: fade to black--	
		}
		// devolving from element
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("atom", "element", true);							// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
			ShellGrow ();													// SHELL: grow
			NucleusToBlack ();												// NUCLEUS: fade to black
			ShellShrink ();													// SHELL: shrink
		}

		lastStateChange = Time.time;														// reset time since last state change
	}

	// public void TransitionToSixthTriangle(bool light, int prevState, GameObject particle)
	// public void TransitionToSixthSquare(bool light, int prevState, GameObject particle)

	public void TransitionToSeventh(bool light, int prevState, GameObject particle)
	{
		// evolving from atom2
		if (prevState == 6)	{ 											
			// rb.mass = 0.5f;													// set mass
			// SelectShape();
			// ToShape();
			NucleusToWhite ();												// NUCLEUS: fade to white
		}

		lastStateChange = Time.time;														// reset time since last state change
	}

	// public void TransitionToSeventhTriangle(bool light, int prevState, GameObject particle)
	// public void TransitionToSeventhSquare(bool light, int prevState, GameObject particle)*/

	// new state

	// transitions \\

	// camera 
	private void SetZoomCamera(string set, string reset, bool devol) 
	{
		transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetTrigger (set);								// set trigger state

		transform.FindChild ("Follow Camera").GetComponent<Animator> ().ResetTrigger (reset);								// reset trigger state

		if (devol == true)																								// if devol true
			transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", true);						// set devolve trigger
		else																											// else
			transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", false);						// reset devolve trigger

	}
	private void ResetZoomCamera(bool devol) 
	{
		if (devol == true)																									// if devol true
			transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", true);						// set devolve trigger
		else																											// else
			transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", false);						// reset devolve trigger
	}

	// core
	private void Core (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{
	// evolutions \\

		// zero
			// to dark zero (0.5)
				// from zero
		if (fromState == 0 && toState == 0 && fromLight && !toLight) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// to light zero (0.5) no change
			// to first
				// from dark zero (0.5)
		if (fromState == 0 && toState == 1 && !fromLight && !toLight) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && !fromLight && toLight) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light zero (0.5)
		if (fromState == 0 && toState == 1 && fromLight && !toLight) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && fromLight && toLight) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
	
		// first
			// to second
				// from dark first
		if (fromState == 1 && toState == 2 && !fromLight && !toLight) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && !fromLight && toLight) {	// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light first
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && fromLight && toLight) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// second
			// to third
				// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// third
			// to fourth
				// from dark third
		if (fromState == 3 && toState == 4 && !fromLight && !toLight) {							// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && !fromLight && toLight) {						// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {							// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && fromLight && toLight) {						// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
			// to fifth
				// from dark fourth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light fourth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		// to square fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
			// to sixth
				// from dark circle fifth
		if (fromState == 5 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle fifth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from triangle fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 1) {						// to dark triangle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from square fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 2) {						// to dark square sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
			// to seventh
				// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) {		// to light triangle seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) {		// to light square seventh
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
			// to eighth
				// from dark circle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 0) {			// to dark circle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) {		// to light circle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			// to dark circle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 0) {		// to light circle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark triangle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 1) {		// to light triangle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light triangle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 1) {		// to light triangle eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark square seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 2) {			// to dark square eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 2) {		// to light square eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light square seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 2) {			// to dark square eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 2) {		// to light square eighth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

	// devolutions \\

		// zero
				// to dead
		if (fromState == 0 && toState == -1) {													// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			
			// dark zero (0.5)
				// to zero
		if (fromState == 0 && toState == 0 && !fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// first
			// to dead
		if (fromState == 1 && toState == -1) {													// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark first
				// to zero
		if (fromState == 1 && toState == 0 && !fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light first
				// to zero
		if (fromState == 1 && toState == 0 && fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// second
			// to dead
		if (fromState == 2 && toState == -1) {													// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark second
				// to zero
		if (fromState == 2 && toState == 0 && !fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 2 && toState == 1 && !fromLight && !toLight) {							// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && !fromLight && toLight) {						// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light second
				// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) {							// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && fromLight && toLight) {						// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
	
		// third
			// to dead
		if (fromState == 3 && toState == -1) {									// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark third	
				// to zero
		if (fromState == 3 && toState == 0 && !fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {							// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light third	
				// to zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) {							// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
			// to dead
		if (fromState == 4 && toState == -1) {									// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark fourth	
				// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {							// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && !fromLight && toLight) {						// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 4 && toState == 3 && !fromLight && !toLight) {							// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && !fromLight && toLight) {						// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light fourth	
				// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) {							// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 4 && toState == 0 && fromLight && !toLight) {							// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && fromLight && toLight) {						// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {							// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && fromLight && toLight) {						// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
			// to dead
		if (fromState == 5 && toState == -1) {									// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle fifth
				// to zero
		if (fromState == 5 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
			if (fromState == 5 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from triangle fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {				// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from square fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
			// to dead
		if (fromState == 6 && toState == -1) {									// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle sixth
				// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from triangle sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 1) {			// to triangle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from square sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {			// to square fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
			// to dead
		if (fromState == 7 && toState == -1) {									// to dead
			GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// to fifth
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark triangle seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 1) {			// to triangle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light triangle seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {			// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}	
			// from dark square seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {			// to square fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark square sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light square seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {				// to square fifth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark square sixth
			GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

	}

/*	private void CoreToZero() {
		GetComponent<Animator> ().ResetTrigger ("scaleup");					// reset next stage
		GetComponent<Animator> ().SetTrigger("scaledown");					// enable core to black animation
		GetComponent<Animator>().SetBool("photon", true);					// enable black core animation state
	}
	private void CoreToFirst() {
		GetComponent<Animator>().SetTrigger ("scaleup");					// trigger core to white animation
		GetComponent<Animator>().SetBool("photon", false);					// disable black core animation state, returning to idle
	}
	private void CoreToWhite() {
		GetComponent<Animator> ().SetTrigger ("fadein");					// trigger core to white animation
		GetComponent<Animator>().SetBool("black", false);					// disable black core animation state, returning to idle

	}
	private void CoreToBlack() {
		GetComponent<Animator> ().ResetTrigger ("fadein");					// reset next stage
		GetComponent<Animator> ().SetTrigger("fadeout");					// enable core to black animation
		GetComponent<Animator>().SetBool("black", true);					// enable black core animation state	

	}
	// private void CoreToShape()*/

	// shell
	private void Shell (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{
		// evolutions \\

		// zero
			// to dark zero (0.5)
				// from zero
		if (fromState == 0 && toState == 0 && fromLight && !toLight) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// to first
				// from dark zero (0.5)
		if (fromState == 0 && toState == 1 && !fromLight && !toLight) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && !fromLight && toLight) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light zero (0.5)
		if (fromState == 0 && toState == 1 && fromLight && !toLight) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && fromLight && toLight) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// first
			// to second
				// from dark first
		if (fromState == 1 && toState == 2 && !fromLight && !toLight) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && !fromLight && toLight) {	// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light first
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && fromLight && toLight) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// second
			// to third
				// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// third
			// to fourth
				// from dark third
		if (fromState == 3 && toState == 4 && !fromLight && !toLight) {							// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && !fromLight && toLight) {						// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {							// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && fromLight && toLight) {						// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
			// to fifth
				// from dark fourth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light fourth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		// to square fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
			// to sixth
				// from dark circle fifth
		if (fromState == 5 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle fifth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from triangle fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 1) {						// to dark triangle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from square fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 2) {						// to dark square sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
			// to seventh
				// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) {		// to light triangle seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) {		// to light square seventh
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
			// to eighth
				// from dark circle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 0) {			// to dark circle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) {		// to light circle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			// to dark circle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 0) {		// to light circle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark triangle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 1) {		// to light triangle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light triangle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 1) {		// to light triangle eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark square seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 2) {			// to dark square eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 2) {		// to light square eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light square seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 2) {			// to dark square eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 2) {		// to light square eighth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// devolutions \\

		// zero
			// to dead
		if (fromState == 0 && toState == -1) {													// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

			// dark zero (0.5)
				// to zero
		if (fromState == 0 && toState == 0 && !fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// first
			// to dead
		if (fromState == 1 && toState == -1) {													// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark first
				// to zero
		if (fromState == 1 && toState == 0 && !fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light first
				// to zero
		if (fromState == 1 && toState == 0 && fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// second
			// to dead
		if (fromState == 2 && toState == -1) {													// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark second
				// to zero
		if (fromState == 2 && toState == 0 && !fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 2 && toState == 1 && !fromLight && !toLight) {							// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && !fromLight && toLight) {						// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light second
				// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) {							// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && fromLight && toLight) {						// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// third
			// to dead
		if (fromState == 3 && toState == -1) {									// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark third	
				// to zero
		if (fromState == 3 && toState == 0 && !fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {							// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light third	
				// to zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) {							// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
			// to dead
		if (fromState == 4 && toState == -1) {									// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark fourth	
				// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {							// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && !fromLight && toLight) {						// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 4 && toState == 3 && !fromLight && !toLight) {							// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && !fromLight && toLight) {						// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light fourth	
				// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) {							// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 4 && toState == 0 && fromLight && !toLight) {							// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && fromLight && toLight) {						// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {							// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && fromLight && toLight) {						// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
			// to dead
		if (fromState == 5 && toState == -1) {									// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle fifth
				// to zero
		if (fromState == 5 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from triangle fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {				// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from square fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
			// to dead
		if (fromState == 6 && toState == -1) {									// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle sixth
				// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from triangle sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 1) {			// to triangle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from square sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {			// to square fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
			// to dead
		if (fromState == 7 && toState == -1) {									// to dead
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark triangle seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 1) {			// to triangle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light triangle seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {			// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}	
			// from dark square seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 2) {			// to square fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {			// to dark square sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light square seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 2) {				// to square fifth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 2) {			// to dark square sixth
			shell.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			shell.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			shell.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

	}

/*	private void ShellDisable() {
		shell.GetComponent<Animator> ().SetTrigger ("shrink");				// trigger shell shrink animation
		shell.GetComponent<Animator>().SetBool("shell", false);				// enable black core animation state
		shellShrinking = true;												// activate timer
		if (shrinkTimer >= 2f) {											// if timer >= duration
			shell.SetActive(false);												// deactivate shell
			shellShrinking = false;												// reset timer flag
			shrinkTimer = 0f;													// reset timer
		}
		//GetComponent<SphereCollider>().enabled = true;						// enable core collider
	}
	// private void ShellToFirst()
	private void ShellToThird() {
		shell.SetActive(true);												// activate shell
		shell.GetComponent<Animator>().SetTrigger("grow");					// enable shell grow animation
		shell.GetComponent<Animator>().SetBool("shell", true);				// enable shell grown animation state
		//shell.GetComponent<SphereCollider> ().enabled = true;				// enable collider (enable here to prevent particles from entering shell to contact core electron)
		//GetComponent<SphereCollider>().enabled = false;						// disable core collider
	}*/

	// nucleus
	private void Nucleus (int fromState, int toState, bool fromLight, bool toLight, int shape) 
	{
		// evolutions \\

		// zero
			// to dark zero (0.5)
				// from zero
		if (fromState == 0 && toState == 0 && fromLight && !toLight) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// to first
				// from dark zero (0.5)
		if (fromState == 0 && toState == 1 && !fromLight && !toLight) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && !fromLight && toLight) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light zero (0.5)
		if (fromState == 0 && toState == 1 && fromLight && !toLight) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 0 && toState == 1 && fromLight && toLight) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// first
			// to second
				// from dark first
		if (fromState == 1 && toState == 2 && !fromLight && !toLight) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && !fromLight && toLight) {	// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light first
		if (fromState == 1 && toState == 2 && fromLight && !toLight) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 1 && toState == 2 && fromLight && toLight) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// second
			// to third
				// from dark second
		if (fromState == 2 && toState == 3 && !fromLight && !toLight) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && !fromLight && toLight) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
				// from light second
		if (fromState == 2 && toState == 3 && fromLight && !toLight) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
		else if (fromState == 2 && toState == 3 && fromLight && toLight) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}

		// third
			// to fourth
				// from dark third
		if (fromState == 3 && toState == 4 && !fromLight && !toLight) {							// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && !fromLight && toLight) {						// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light third
		if (fromState == 3 && toState == 4 && fromLight && !toLight) {							// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 4 && fromLight && toLight) {						// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
			// to fifth
				// from dark fourth
		if (fromState == 4 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light fourth
		if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 5 && fromLight && toLight && shape == 2) {		// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
			// to sixth
				// from dark circle fifth
		if (fromState == 5 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle fifth
		if (fromState == 5 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from triangle fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 1) {						// to dark triangle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from square fifth
		if (fromState == 5 && toState == 6 && fromLight && shape == 2) {						// to dark square sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
			// to seventh
				// from dark circle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 0) {			// to dark circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 0) {		// to light circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle sixth
		if (fromState == 6 && toState == 7 && fromLight && !toLight && shape == 0) {			// to dark circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && fromLight && toLight && shape == 0) {		// to light circle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark triangle sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 1) {			// to dark triangle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 1) {		// to light triangle seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark square sixth
		if (fromState == 6 && toState == 7 && !fromLight && !toLight && shape == 2) {			// to dark square seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 7 && !fromLight && toLight && shape == 2) {		// to light square seventh
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
			// to eighth
				// from dark circle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 0) {			// to dark circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 0) {		// to light circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light circle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 0) {			// to dark circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 0) {		// to light circle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark triangle seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 1) {		// to light triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light triangle seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 1) {			// to dark triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 1) {		// to light triangle eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from dark square seventh
		if (fromState == 7 && toState == 8 && !fromLight && !toLight && shape == 2) {			// to dark square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && !fromLight && toLight && shape == 2) {		// to light square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// from light square seventh
		if (fromState == 7 && toState == 8 && fromLight && !toLight && shape == 2) {			// to dark square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 8 && fromLight && toLight && shape == 2) {		// to light square eighth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// devolutions \\

		// zero
			// to dead
		if (fromState == 0 && toState == -1) {													// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// dark zero (0.5)
			// to zero
		if (fromState == 0 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// first
			// to dead
		if (fromState == 1 && toState == -1) {													// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark first
				// to zero
		if (fromState == 1 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light first
				// to zero
		if (fromState == 1 && toState == 0 && fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero (0.5)
		if (fromState == 1 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// second
			// to dead
		if (fromState == 2 && toState == -1) {													// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark second
				// to zero
		if (fromState == 2 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 2 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 2 && toState == 1 && !fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light second
				// to zero
		if (fromState == 2 && toState == 0 && fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 2 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 2 && toState == 1 && fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 2 && toState == 1 && fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// third
			// to dead
		if (fromState == 3 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark third	
				// to zero
		if (fromState == 3 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 3 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 3 && toState == 1 && !fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 3 && toState == 2 && !fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light third	
				// to zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 3 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 3 && toState == 1 && fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 3 && toState == 2 && fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 3 && toState == 2 && !fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fourth
			// to dead
		if (fromState == 4 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark fourth	
				// to zero
		if (fromState == 4 && toState == 0 && !fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 4 && toState == 0 && !fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 4 && toState == 1 && !fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && !fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 4 && toState == 2 && !fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && !fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 4 && toState == 3 && !fromLight && !toLight) {							// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && !fromLight && toLight) {						// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light fourth	
				// to zero
		if (fromState == 4 && toState == 0 && fromLight && toLight) {							// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 4 && toState == 0 && fromLight && !toLight) {							// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 4 && toState == 1 && fromLight && !toLight) {							// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 1 && fromLight && toLight) {						// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 4 && toState == 2 && fromLight && !toLight) {							// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 2 && fromLight && toLight) {						// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 4 && toState == 3 && fromLight && !toLight) {							// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 4 && toState == 3 && fromLight && toLight) {						// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// fifth
			// to dead
		if (fromState == 5 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle fifth
				// to zero
		if (fromState == 5 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from triangle fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 1) {				// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from square fifth
				// to zero
		if (fromState == 5 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 5 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 5 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 5 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 5 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 5 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 5 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// sixth
			// to dead
		if (fromState == 6 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle sixth
				// to zero
		if (fromState == 6 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from triangle sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 1) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && !toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from square sixth
				// to zero
		if (fromState == 6 && toState == 0 && !fromLight && toLight && shape == 2) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 6 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 6 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 6 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 6 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 6 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 6 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 6 && toState == 5 && !fromLight && toLight && shape == 0) {			// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

		// seventh
			// to dead
		if (fromState == 7 && toState == -1) {									// to dead
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");						// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");						// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);						// enable black core animation state
		}
			// from dark circle seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 0) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 0) {		// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && !fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light circle seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 0) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 0) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 0) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 0) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 0) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 0) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 0) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 0) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && !toLight && shape == 0) {			// to dark fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && !toLight && shape == 0) {			// to dark circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 0) {		// to light circle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 0) {			// to dark circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 6 && fromLight && toLight && shape == 0) {		// to light circle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from dark triangle seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 1) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 1) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light triangle seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 1) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 1) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator> ().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 1) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 1) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 1) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 1) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 1) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 1) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 1) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 1) {			// to triangle fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 1) {			// to dark triangle sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}	
			// from dark square seventh
				// to zero
		if (fromState == 7 && toState == 0 && !fromLight && toLight && shape == 2) {			// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && !fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && !fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && !fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && !fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && !fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && !fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && !fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && !fromLight && toLight && shape == 2) {			// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && !fromLight && toLight && shape == 2) {			// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && !fromLight && !toLight && shape == 2) {			// to dark square sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
			// from light square seventh
				// to zero
		if (fromState == 7 && toState == 0 && fromLight && toLight && shape == 2) {				// to zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to dark zero
		if (fromState == 7 && toState == 0 && fromLight && !toLight && shape == 2) {			// to dark zero
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to first
		if (fromState == 7 && toState == 1 && fromLight && !toLight && shape == 2) {			// to dark first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 1 && fromLight && toLight && shape == 2) {		// to light first
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to second
		if (fromState == 7 && toState == 2 && fromLight && !toLight && shape == 2) {			// to dark second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 2 && fromLight && toLight && shape == 2) {		// to light second
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to third
		if (fromState == 7 && toState == 3 && fromLight && !toLight && shape == 2) {			// to dark third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
		else if (fromState == 7 && toState == 3 && fromLight && toLight && shape == 2) {		// to light third
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fourth
		if (fromState == 7 && toState == 4 && fromLight && toLight && shape == 2) {				// to light fourth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to fifth
		if (fromState == 7 && toState == 5 && fromLight && toLight && shape == 2) {				// to square fifth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}
				// to sixth
		if (fromState == 7 && toState == 6 && fromLight && !toLight && shape == 2) {			// to dark square sixth
			nucleus.GetComponent<Animator> ().ResetTrigger ("scaleup");										// reset next stage
			nucleus.GetComponent<Animator> ().SetTrigger("scaledown");										// enable core to black animation
			nucleus.GetComponent<Animator>().SetBool("photon", true);										// enable black core animation state
		}

	}

	/*private void NucleusEnable() {
		nucleus.SetActive(true);											// enable nucleus	
	}
	private void NucleusDisable() {
		nucleus.GetComponent<Animator> ().ResetTrigger ("fadeblack");		// reset next stage
		nucleus.GetComponent<Animator> ().SetTrigger ("fadewhite");			// trigger nucleus to white animation
		nucleus.GetComponent<Animator>().SetBool("white", true);			// disable white nucleus animation state, returning to idle
		// deactivate
		nucleusDeactivating = true;											// activate timer
		if (shrinkTimer >= 1f) {											// if timer >= duration
			nucleus.SetActive(false);											// deactivate nucleus
			nucleusDeactivating = false;										// reset timer flag
			shrinkTimer = 0f;													// reset timer
		}
	}
	private void NucleusToWhite() {
		nucleus.GetComponent<Animator> ().ResetTrigger ("fadeblack");		// reset next stage
		nucleus.GetComponent<Animator> ().SetTrigger ("fadewhite");			// trigger nucleus to white animation
		nucleus.GetComponent<Animator>().SetBool("white", true);			// disable white nucleus animation state, returning to idle
	}
	private void NucleusToBlack() {
		nucleus.GetComponent<Animator> ().SetTrigger("fadeblack");			// enable nucleus to black animation
		nucleus.GetComponent<Animator>().SetBool("white", false);			// enable white nucleus animation state	

	}
*/
}