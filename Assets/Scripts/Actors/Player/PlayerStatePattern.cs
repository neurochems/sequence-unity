using UnityEngine;
using System.Collections;

public class PlayerStatePattern : MonoBehaviour {

	public float evol;														// evolution level

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

	private int die;														// roll for collision conflicts
	private GameObject nucleus, shell;										// reference to nucleus and shell children
	[HideInInspector] public GameObject self;								// reference to this gameobject

	// component references
	private UIManager uim;													// UI manager
	//private PlayerPhysicsManager ppm;										// player physics manager
	private SphereCollider[] sc;											// sphere colliders

	// timers & flags
	public bool stunned;													// stunned?
	public float stunDuration = 5f;											// duration of post-hit invulnerability
	private float stunTimer = 0f;											// stun timer
	private bool shellShrinking, nucleusDeactivating;						// shell shrinking flag, nucleus deactivating flag
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer
	[HideInInspector] public float lastStateChange = 0.0f;					// since last state change
	[HideInInspector] public float sincePlaytimeBegin = 0.0f;				// since game start
	private bool timeCheck = true;											// time check flag

	void Awake()
	{
		evol = 0f;															// initialize evol

		deadState = new DeadPlayerState (this);								// initialize dead state
		photonState = new ZeroPlayerState (this);							// initialize photon state
		electronState = new FirstPlayerState (this);						// initialize electron state
		electron2State = new SecondPlayerState (this);					// initialize electron2 state
		shellState = new ThirdPlayerState (this);							// initialize shell state
		shell2State = new FourthPlayerState (this);							// initialize shell2 state
		atomState = new FifthPlayerState (this);								// initialize atom state
		atom2State = new SixthPlayerState (this);							// initialize atom2 state
		elementState = new SeventhPlayerState (this);						// initialize element state
		// new state

		nucleus = transform.FindChild ("Player Nucleus").gameObject;		// initialize nucleus
		shell = transform.FindChild ("Player Shell").gameObject;			// initialize shell

		uim = GetComponent<UIManager> ();									// init ui manager ref
		//ppm = GetComponent<PlayerPhysicsManager> ();						// init player physics manager ref
		sc = GetComponents<SphereCollider> ();								// init sphere collider ref

		self = gameObject;													// init self reference

		Destroy(GameObject.FindGameObjectWithTag("Destroy"));				// destroy old UI

	}

	void Start () 
	{
		currentState = photonState;											// start at photon state
		TransitionToPhoton(0, gameObject);										// CORE: shrink to photon size, fade to white
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

	public void SubtractEvol(float changeAmount) 
	{
		evol -= changeAmount;												// subtract evol level
	}

	public void AddEvol(float changeAmount) 
	{
		evol += changeAmount;												// add evol level
	}

	// public void AddDark()
	// public void SubtractDark()
	// public void AddLight()
	// public void SubtractLight()

	// public float TotalEvol()
		// darkEvol + lightEvol

	// BEHAVIOURS \\

	public void Stun ()											// post-hit invulnerability
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
	}

	public void SpawnZero (int num)
	{
		GetComponent<SpawnParticle> ().SpawnPhoton (num);
	}

	public void SpawnFirst (int num)
	{
		GetComponent<SpawnParticle> ().SpawnElectron (num);
	}

	// STATE TRANSTITIONS \\

	public void TransitionToDead(int prevState, GameObject particle)
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

	public void TransitionToZero(int prevState, GameObject particle)
	{
		// devolving from electron
		if (prevState == 0 || prevState == 1)	{ 											
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "electron", true);						// CAMERA: zoom to size 20
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
		}
		// devolving from electron2
		else if (prevState == 2) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "electron", true);						// CAMERA: zoom to size 20
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			NucleusDisable ();												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from shell
		else if (prevState == 3) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "shell", true);							// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable ();												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from shell2
		else if (prevState == 4) {										
			//rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "shell", true);							// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToWhite ();													// CORE: return to idle white
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable (); 												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from atom
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
		// devolving from atom2
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
		// devolving from element
		else if (prevState == 7) {
			// rb.mass = 0.2f;													// set mass
			SetZoomCamera("photon", "element", true);						// CAMERA: zoom to size 20
			// ToCircle ();													// CORE: make mesh circle
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
		}
		//new state

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToFirst(int prevState, GameObject particle)
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

	public void TransitionToSecond(int prevState, GameObject particle)
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

	public void TransitionToThird(int prevState, GameObject particle)
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

	public void TransitionToFourth(int prevState, GameObject particle)
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

	public void TransitionToFifth(int prevState, GameObject particle)
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

	public void TransitionToSixth(int prevState, GameObject particle)
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

	public void TransitionToSeventh(int prevState, GameObject particle)
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

	// shape
	// private void SelectShape()
	// logic on lightCollected (+1 in state OnTriggerEnter; electron, electron2, square) and darkCollected (+1 in state OnTriggerEnter; shell, shell2, doubleShell, doubleShell2) vars

	// core
	private void CoreToZero() {
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
	// private void CoreToShape()

	// shell
	private void ShellDisable() {
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
	}

	// nucleus
	private void NucleusEnable() {
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

}
