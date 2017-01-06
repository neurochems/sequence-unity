using UnityEngine;
using System.Collections;

public class PlayerStatePattern : MonoBehaviour {

	public float evol;														// evolution level

	[HideInInspector] public IParticleState currentState;					// other object state
	[HideInInspector] public int previousState = 0;							// previous state index / Fn

	[HideInInspector] public DeadPlayerState deadState;						// instance of dead state
	[HideInInspector] public PhotonPlayerState photonState;					// instance of photon state
	[HideInInspector] public ElectronPlayerState electronState;				// instance of electron state
	[HideInInspector] public Electron2PlayerState electron2State;			// instance of electron2 state
	[HideInInspector] public ShellPlayerState shellState;					// instance of shell state
	[HideInInspector] public Shell2PlayerState shell2State;					// instance of shell2 state
	[HideInInspector] public AtomPlayerState atomState;						// instance of atom state
	//[HideInInspector] public Atom2PlayerState atom2State;					// instance of atom2 state

	private int die;														// roll for collision conflicts
	private GameObject nucleus, shell;										// reference to nucleus and shell children
	[HideInInspector] public GameObject self;								// reference to this gameobject

	// manager references
	private UIManager uim;													// UI manager
	private PlayerPhysicsManager ppm;										// player physics manager

	// timers
	public float stunDuration = 5f;											// duration of post-hit invulnerability
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer
	[HideInInspector] public float lastStateChange = 0.0f;					// since last state change
	[HideInInspector] public float sincePlaytimeBegin = 0.0f;				// since game start
	private bool timeCheck = true;											// time check flag

	void Awake()
	{
		evol = 0f;															// initialize evol

		deadState = new DeadPlayerState (this);								// initialize dead state
		photonState = new PhotonPlayerState (this);							// initialize photon state
		electronState = new ElectronPlayerState (this);						// initialize electron state
		electron2State = new Electron2PlayerState (this);					// initialize electron2 state
		shellState = new ShellPlayerState (this);							// initialize shell state
		shell2State = new Shell2PlayerState (this);							// initialize shell2 state
		atomState = new AtomPlayerState (this);								// initialize atom state
		//atom2State = new Atom2PlayerState (this);							// initialize atom2 state

		nucleus = transform.FindChild ("Player Nucleus").gameObject;		// initialize nucleus
		shell = transform.FindChild ("Player Shell").gameObject;			// initialize shell

		uim = GetComponent<UIManager> ();									// init ui manager
		ppm = GetComponent<PlayerPhysicsManager> ();						// init player physics manager

		self = gameObject;													// init self reference

	}

	void OnDisable()
	{
		ParticleStateEvents.toPhoton -= TransitionToPhoton;					// untrigger transition event
		ParticleStateEvents.toElectron -= TransitionToElectron;				// untrigger transition event
		ParticleStateEvents.toElectron2 -= TransitionToElectron2;			// untrigger transition event
		ParticleStateEvents.toShell -= TransitionToShell;					// untrigger transition event
		ParticleStateEvents.toShell2 -= TransitionToShell2;					// untrigger transition event
		ParticleStateEvents.toAtom -= TransitionToAtom;						// untrigger transition event
	}

	void OnDestroy()
	{
		ParticleStateEvents.toPhoton -= TransitionToPhoton;					// untrigger transition event
		ParticleStateEvents.toElectron -= TransitionToElectron;				// untrigger transition event
		ParticleStateEvents.toElectron2 -= TransitionToElectron2;			// untrigger transition event
		ParticleStateEvents.toShell -= TransitionToShell;					// untrigger transition event
		ParticleStateEvents.toShell2 -= TransitionToShell2;					// untrigger transition event
		ParticleStateEvents.toAtom -= TransitionToAtom;						// untrigger transition event
	}

	void Start () 
	{
		currentState = photonState;											// start at photon state
		TransitionToPhoton(gameObject);										// CORE: shrink to photon size, fade to white
		// stun here if stun in SpawnParticle doesn't work
	}

	void Update () 
	{
		currentState.UpdateState ();										// frame updates from current state class

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

	// EVOL CHANGES \\

	public void SubtractEvol(float changeAmount) 
	{
		evol -= changeAmount;												// subtract evol level
		//bump = true;															// collision bump
	}

	public void AddEvol(float changeAmount) 
	{
		evol += changeAmount;												// add evol level
	}

	// BEHAVIOURS \\

	public void Stun (bool stun)											// post-hit invulnerability
	{
		if (stun) {																// if stun
			GetComponent<SphereCollider> ().enabled = false;						// disable collider
			ppm.Bump(true);															// enable bump
		}
		else 																	// if not stun
			GetComponent<SphereCollider> ().enabled = true;							// enable collider

	}

	public void SpawnPhoton (int num)
	{
		GetComponent<SpawnParticle> ().SpawnPhoton (num);
	}

	public void SpawnElectron (int num)
	{
		GetComponent<SpawnParticle> ().SpawnElectron (num);
	}

	// STATE TRANSTITIONS \\

	public void TransitionToDead(GameObject particle)
	{
		//GetComponent<SphereCollider> ().enabled = false;					// disable collider at start to prevent collisions during shrink
		shell.GetComponent<Animator> ().SetTrigger("shrink");									// enable shell shrink animation
		shell.GetComponent<Animator> ().SetBool("shell", false);								// reset shell animation state
		GetComponent<Animator> ().SetTrigger("fadeout");									// enable core to black animation
		GetComponent<Animator>().SetBool("black", true);									// enable black core animation state
		GetComponent<Animator>().SetBool("dead", true);									// enable black core animation state

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToPhoton(GameObject particle)
	{
		// devolving from electron
		if (previousState == 0 || previousState == 1)	{ 											
			// rb.mass = 0.2f;													// set mass
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
		}
		// devolving from electron2
		else if (previousState == 2) {										
			//rb.mass = 0.2f;													// set mass
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			NucleusDisable ();												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from shell
		else if (previousState == 3) {										
			//rb.mass = 0.2f;													// set mass
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable ();												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from shell2
		else if (previousState == 4) {										
			//rb.mass = 0.2f;													// set mass
			CoreToWhite ();													// CORE: return to idle white
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable (); 												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from atom
		else if (previousState == 5) {										
			//rb.mass = 0.2f;													// set mass
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink

		}

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToElectron(GameObject particle)
	{
		// evolving from photon
		if (previousState == 0)	{ 											
			// rb.mass = 0.5f;													// set mass
			CoreToElectron ();												// CORE: grow to electron size, is white
		}
		// devolving from electron2
		else if (previousState == 2) {										
			//rb.mass = 0.5f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from shell
		else if (previousState == 3) {										
			//rb.mass = 0.5f;													// set mass
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusToWhite ();												// NUCLEUS: fade to white
		}
		// devolving from shell2
		else if (previousState == 4) {										
			//rb.mass = 0.5f;													// set mass
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from atom
		else if (previousState == 5) {										
			//rb.mass = 0.5f;													// set mass
			ShellShrink ();													// SHELL: shrink

		}

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToElectron2(GameObject particle)
	{
		// evolving from electron
		if (previousState == 1)	{ 											
			// rb.mass = 0.75f;													// set mass
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
		}
		// devolving from shell
		else if (previousState == 3) {										
			//rb.mass = 0.75f;													// set mass
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from shell2
		else if (previousState == 4) {										
			//rb.mass = 0.75f;													// set mass
			CoreToWhite ();													// CORE: fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom
		else if (previousState == 5) {										
			//rb.mass = 0.75f;													// set mass
			ShellShrink ();													// SHELL: shrink
			NucleusToBlack ();												// NUCLEUS: fade to black
		}

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToShell(GameObject particle)
	{
		// evolving from electron
		if (previousState == 1)	{ 											
			// rb.mass = 0.5f;													// set mass
			CoreToBlack();													// CORE: fade to black
			ShellGrow ();													// SHELL: grow
		}
		// evolving from electron2
		else if (previousState == 2) {										
			//rb.mass = 0.5f;													// set mass
			CoreToBlack ();													// CORE: fade to black
			ShellGrow ();													// SHELL: grow
		}
		// devolving from shell2
		else if (previousState == 4) {										
			//rb.mass = 0.5f;													// set mass
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom
		else if (previousState == 5) {										
			//rb.mass = 0.5f;													// set mass
			CoreToBlack ();													// CORE: fade to black
			NucleusToBlack ();												// NUCLEUS: fade to black
		}

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToShell2(GameObject particle)
	{
		// evolving from electron2
		if (previousState == 2)	{ 											
			// rb.mass = 0.75f;													// set mass
			CoreToBlack ();													// CORE: fade to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
			ShellGrow ();													// SHELL: grow
		}
		// evolving from shell
		else if (previousState == 2) {										
			//rb.mass = 0.75f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from atom
		else if (previousState == 5) {										
			//rb.mass = 0.75f;													// set mass
			CoreToBlack ();													// CORE: fade to black
		}

		lastStateChange = Time.time;														// reset time since last state change
	}

	public void TransitionToAtom(GameObject particle)
	{
		// evolving from shell2
		if (previousState == 0)	{ 											
			// rb.mass = 0.5f;													// set mass
			CoreToWhite ();													// CORE: fade to white
		}	

		lastStateChange = Time.time;														// reset time since last state change
	}

	// transitions \\

	// core
	private void CoreToPhoton() {
		GetComponent<Animator> ().ResetTrigger ("scaleup");					// reset next stage
		GetComponent<Animator> ().SetTrigger("scaledown");					// enable core to black animation
		GetComponent<Animator>().SetBool("photon", true);					// enable black core animation state
	}
	private void CoreToElectron() {
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

	// shell
	private void ShellGrow() {
		shell.SetActive(true);												// activate shell
		shell.GetComponent<Animator>().SetTrigger("grow");					// enable shell grow animation
		shell.GetComponent<Animator>().SetBool("shell", true);				// enable shell grown animation state
		shell.GetComponent<SphereCollider> ().enabled = true;				// enable collider (enable here to prevent particles from entering shell to contact core electron)
		GetComponent<SphereCollider>().enabled = false;						// disable core collider
	}
	private void ShellShrink() {
		shell.GetComponent<Animator> ().SetTrigger ("shrink");				// trigger shell shrink animation
		shell.GetComponent<Animator>().SetBool("shell", false);				// enable black core animation state
		shrinkTimer += Time.deltaTime;										// start timer
		if (shrinkTimer >= 2f) {											// if timer >= duration
			shell.SetActive(false);												// deactivate shell
			shrinkTimer = 0f;													// reset timer
		}
		GetComponent<SphereCollider>().enabled = true;						// enable core collider
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
		shrinkTimer += Time.deltaTime;										// start timer
		if (shrinkTimer >= 1f) {											// if timer >= duration
			nucleus.SetActive(false);											// deactivate shell
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
