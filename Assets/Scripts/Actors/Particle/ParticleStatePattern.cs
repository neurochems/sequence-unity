using UnityEngine;
using System.Collections;

public class ParticleStatePattern : MonoBehaviour {

	public float evol;														// evolution level

	[HideInInspector] public IParticleState currentState;					// other object state
	//[HideInInspector] public int previousState;								// previous state index / Fn

	[HideInInspector] public DeadParticleState deadState;					// instance of dead state
	[HideInInspector] public PhotonParticleState photonState;				// instance of photon state
	[HideInInspector] public ElectronParticleState electronState;			// instance of electron state
	[HideInInspector] public Electron2ParticleState electron2State;			// instance of electron2 state
	[HideInInspector] public ShellParticleState shellState;					// instance of shell state
	[HideInInspector] public Shell2ParticleState shell2State;				// instance of shell2 state
	[HideInInspector] public AtomParticleState atomState;					// instance of atom state
	[HideInInspector] public Atom2ParticleState atom2State;					// instance of atom2 state
	// new state

	//public float attractionRange = 20f;										// particle sensing distance
	//[HideInInspector] public Transform attractionTarget;					// particle sensing target

	private int die;														// roll for collision conflicts
	private GameObject nucleus, shell;										// reference to nucleus and shell children
	[HideInInspector] public GameObject self;								// reference to this gameobject

	//component references
	//private ParticlePhysicsManager ppm;										// particle physics manager
	private SphereCollider[] sc;											// sphere colliders

	public bool stunned;													// stunned?
	private bool shellShrinking, nucleusDeactivating;						// shell shrinking flag, nucleus deactivating flag
	public float stunDuration = 3f;											// duration of post-hit invulnerability
	private float stunTimer;												// stun timer
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer

	void Awake()
	{
		//evol = 0f;															// initialize evol

		deadState = new DeadParticleState (this);							// initialize dead state
		photonState = new PhotonParticleState (this);						// initialize photon state
		electronState = new ElectronParticleState (this);					// initialize electron state
		electron2State = new Electron2ParticleState (this);					// initialize electron2 state
		shellState = new ShellParticleState (this);							// initialize shell state
		shell2State = new Shell2ParticleState (this);						// initialize shell2 state
		atomState = new AtomParticleState (this);							// initialize atom state
		atom2State = new Atom2ParticleState (this);							// initialize atom2 state
		// new state

		nucleus = transform.FindChild("Nucleus").gameObject;				// initialize nucleus ref
		shell = transform.FindChild ("Shell").gameObject;					// initialize shell ref

		//ppm = GetComponent<ParticlePhysicsManager> ();						// init particle physics manager ref
		sc = GetComponents<SphereCollider> ();								// init sphere colliders ref

		self = gameObject;													// init this gameobject ref

	}

	void Start () 
	{
		currentState = photonState;											// start at photon state
		TransitionToPhoton(0, gameObject);										// CORE: shrink to photon size, fade to white
	}

	void Update () 
	{
		currentState.UpdateState ();										// frame updates from current state class

		// stun duration timer
		if (stunned) {
			Stun ();
			stunTimer += Time.deltaTime;													// start timer
		}

		// deactivating shell/nucleus timer
		if (shellShrinking || nucleusDeactivating) shrinkTimer += Time.deltaTime;			// start timer
	}

	private void OnTriggerEnter(Collider other)
	{
		currentState.OnTriggerEnter (other);								// pass collider into state class
	}

	void OnDisable()
	{
		ParticleStateEvents.toPhoton -= TransitionToPhoton;					// untrigger transition event
		ParticleStateEvents.toElectron -= TransitionToElectron;				// untrigger transition event
		ParticleStateEvents.toElectron2 -= TransitionToElectron2;			// untrigger transition event
		ParticleStateEvents.toShell -= TransitionToShell;					// untrigger transition event
		ParticleStateEvents.toShell2 -= TransitionToShell2;					// untrigger transition event
		ParticleStateEvents.toAtom -= TransitionToAtom;						// untrigger transition event
		ParticleStateEvents.toAtom2 -= TransitionToAtom2;					// untrigger transition event
		// new state
	}

	void OnDestroy()
	{
		ParticleStateEvents.toPhoton -= TransitionToPhoton;					// untrigger transition event
		ParticleStateEvents.toElectron -= TransitionToElectron;				// untrigger transition event
		ParticleStateEvents.toElectron2 -= TransitionToElectron2;			// untrigger transition event
		ParticleStateEvents.toShell -= TransitionToShell;					// untrigger transition event
		ParticleStateEvents.toShell2 -= TransitionToShell2;					// untrigger transition event
		ParticleStateEvents.toAtom -= TransitionToAtom;						// untrigger transition event
		ParticleStateEvents.toAtom2 -= TransitionToAtom2;					// untrigger transition event
		// new state
	}

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

	public void SpawnPhoton (int num)
	{
		GetComponent<SpawnParticle> ().SpawnPhoton (num);
	}

	public void SpawnElectron (int num)
	{
		GetComponent<SpawnParticle> ().SpawnElectron (num);
	}

	// EVOL CHANGES \\
	public void RollDie(ParticleStatePattern other, float addAmount, float subAmount) {
		bool rolling = true;
		do {
			die = Random.Range(1,6);														// roll die
			if (die > other.die) {															// if this die > other die
				AddEvol(addAmount);																// add evol level
				other.SubtractEvol(subAmount);													// other: remove evol level
				rolling = false;																// exit roll
			}
		} while (rolling);																	// reroll if same die
	}

	public void SubtractEvol(float changeAmount) 
	{
		evol -= changeAmount;												// subtract evol level
	}

	public void AddEvol(float changeAmount) 
	{
		evol += changeAmount;												// add evol level
	}

	// STATE TRANSTITIONS \\

	public void TransitionToDead(int prevState, GameObject particle)
	{
		gameObject.tag = "Dead";											// set gameobject tag
		//GetComponent<SphereCollider> ().enabled = false;					// disable collider at start to prevent collisions during shrink
		//GetComponent<Animator>().SetTrigger("fadeout");					// fade to black
		GetComponent<Animator>().SetBool("dead", true);						// enable black core animation state
		StopAllCoroutines();
		Destroy (gameObject, 2.0f);											// destroy object after animation
	}

	public void TransitionToPhoton(int prevState, GameObject particle)
	{
		gameObject.tag = "Photon";											// set gameobject tag
		//GetComponent<SphereCollider>().enabled = true;			// enable sphere collider
		// devolving from electron
		if (prevState == 0 || prevState == 1)	{ 											
			// rb.mass = 0.2f;													// set mass
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
		}
		// devolving from electron2
		else if (prevState == 2) {										
			//rb.mass = 0.2f;													// set mass
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			NucleusDisable ();												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from shell
		else if (prevState == 3) {										
			//rb.mass = 0.2f;													// set mass
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
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToWhite ();													// CORE: return to idle white
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			NucleusDisable (); 												// NUCLEUS: fade to white, then deactivate
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from atom
		else if (prevState == 5) {										
			//rb.mass = 0.2f;													// set mass
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable ();												// NUCLEUS: fade to white, then deactivate
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.2f;													// set mass
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			CoreToPhoton ();												// CORE: shrink to photon size, fade to white
			ShellShrink ();													// SHELL: shrink
			NucleusDisable ();												// NUCLEUS: fade to white, then deactivate
		}
		//new state
	}

	public void TransitionToElectron(int prevState, GameObject particle)
	{
		gameObject.tag = "Electron";											// set gameobject tag
		// evolving from photon
		if (prevState == 0)	{ 											
			// rb.mass = 0.5f;													// set mass
			CoreToElectron ();												// CORE: grow to electron size, is white
		}
		// devolving from electron2
		else if (prevState == 2) {										
			//rb.mass = 0.5f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from shell
		else if (prevState == 3) {										
			//rb.mass = 0.5f;													// set mass
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
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.2f;													// set mass
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// new state
	}

	public void TransitionToElectron2(int prevState, GameObject particle)
	{
		gameObject.tag = "Electron2";										// set gameobject tag
		// init from photon
		if (prevState == 0)	{ 											
			// rb.mass = 0.75f;													// set mass
			CoreToElectron ();												// CORE: grow to electron size, is white
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
		}
		// evolving from electron
		else if (prevState == 1)	{ 											
			// rb.mass = 0.75f;													// set mass
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
		}
		// devolving from shell
		else if (prevState == 3) {										
			//rb.mass = 0.75f;													// set mass
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
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.2f;													// set mass
			sc[0].radius = 0.51f;											// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			sc[1].radius = 0.51f;											// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);							// level circle collider on world
			ShellShrink ();													// SHELL: shrink
		}
		// new state
	}

	public void TransitionToShell(int prevState, GameObject particle)
	{
		gameObject.tag = "Shell";											// set gameobject tag
		// init from photon
		if (prevState == 0)	{ 											
			// rb.mass = 0.75f;													// set mass
			sc[0].radius = 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToElectron ();												// CORE: grow to electron size, is white
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
			CoreToBlack();													// CORE: fade to black
			ShellGrow ();													// SHELL: grow
		}
		// evolving from electron
		else if (prevState == 1)	{ 											
			// rb.mass = 0.5f;													// set mass
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
			CoreToBlack ();													// CORE: fade to black
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.2f;													// set mass
			CoreToBlack ();													// CORE: fade to black
		}
		// new state
	}

	public void TransitionToShell2(int prevState, GameObject particle)
	{
		gameObject.tag = "Shell2";											// set gameobject tag
		shell.gameObject.tag = "Shell2";									// set shell gameobject tag
		// init from photon
		if (prevState == 0)	{ 											
			// rb.mass = 0.75f;													// set mass
			sc[0].radius = 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToElectron ();												// CORE: grow to electron size, is white
			CoreToBlack();													// CORE: fade to black
			ShellGrow ();													// SHELL: grow
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// evolving from electron2
		if (prevState == 2)	{ 											
			// rb.mass = 0.75f;													// set mass
			sc[0].radius = 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToBlack ();													// CORE: fade to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
			ShellGrow ();													// SHELL: grow
		}
		// evolving from shell
		else if (prevState == 2) {										
			//rb.mass = 0.75f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from atom
		else if (prevState == 5) {										
			//rb.mass = 0.75f;													// set mass
			CoreToBlack ();													// CORE: fade to black
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.2f;													// set mass
			CoreToBlack ();													// CORE: fade to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// new state
	}

	public void TransitionToAtom(int prevState, GameObject particle)
	{
		gameObject.tag = "Atom2";											// set gameobject tag
		shell.gameObject.tag = "Atom2";									// set shell gameobject tag
		// init from photon
		if (prevState == 0)	{ 											
			// rb.mass = 0.75f;													// set mass
			sc[0].radius = 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToElectron ();												// CORE: grow to electron size, is white
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
			ShellGrow ();													// SHELL: grow
		}
		// evolving from shell2
		else if (prevState == 5) { 											
			// rb.mass = 0.5f;													// set mass
			CoreToWhite ();													// CORE: fade to white
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// devolving from atom2
		else if (prevState == 6) {										
			//rb.mass = 0.2f;													// set mass
			NucleusToWhite ();												// NUCLEUS: fade out to white
		}
		// new state
	}

	public void TransitionToAtom2(int prevState, GameObject particle)
	{
		gameObject.tag = "Atom";											// set gameobject tag
		shell.gameObject.tag = "Atom";									// set shell gameobject tag
		// init from photon
		if (prevState == 0)	{ 											
			// rb.mass = 0.75f;													// set mass
			sc[0].radius = 1.575f;											// grow collision radius out
			//sc[0].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			sc[1].radius = 1.575f;											// grow collision radius out
			sc[1].center = new Vector3(0f, 0.3f, 0f);						// level circle collider on world
			CoreToElectron ();												// CORE: grow to electron size, is white
			NucleusEnable ();												// NUCLEUS: enable, fade in to black
			NucleusToWhite ();												// NUCLEUS: fade out to white
			ShellGrow ();													// SHELL: grow
		}
		// evolving from atom
		else if (prevState == 5) { 											
			// rb.mass = 0.5f;													// set mass
			NucleusToBlack ();												// NUCLEUS: fade to black
		}
		// new state
	}

	// transitions \\

	// core
	void CoreToPhoton() {
		GetComponent<Animator> ().ResetTrigger ("scaleup");					// reset next stage
		GetComponent<Animator> ().SetTrigger("scaledown");					// enable core to black animation
		GetComponent<Animator>().SetBool("photon", true);					// enable black core animation state
	}
	void CoreToElectron() {
		GetComponent<Animator>().SetTrigger ("scaleup");					// trigger core to white animation
		GetComponent<Animator>().SetBool("photon", false);					// disable black core animation state, returning to idle
	}
	void CoreToWhite() {
		GetComponent<Animator> ().SetTrigger ("fadein");					// trigger core to white animation
		GetComponent<Animator>().SetBool("black", false);					// disable black core animation state, returning to idle

	}
	void CoreToBlack() {
		GetComponent<Animator> ().ResetTrigger ("fadein");					// reset next stage
		GetComponent<Animator> ().SetTrigger("fadeout");					// enable core to black animation
		GetComponent<Animator>().SetBool("black", true);					// enable black core animation state	

	}

	// shell
	void ShellGrow() {
		shell.SetActive(true);												// activate shell
		shell.GetComponent<Animator>().SetTrigger("grow");					// enable shell grow animation
		shell.GetComponent<Animator>().SetBool("shell", true);				// enable shell grown animation state
		//shell.GetComponent<SphereCollider> ().enabled = true;				// enable collider (enable here to prevent particles from entering shell to contact core electron)
		//GetComponent<SphereCollider>().enabled = false;						// disable core collider
	}
	void ShellShrink() {
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

	// nucleus
	void NucleusEnable() {
		nucleus.SetActive(true);											// enable nucleus	
	}
	void NucleusDisable() {
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
	void NucleusToWhite() {
		nucleus.GetComponent<Animator> ().ResetTrigger ("fadeblack");		// reset next stage
		nucleus.GetComponent<Animator> ().SetTrigger ("fadewhite");			// trigger nucleus to white animation
		nucleus.GetComponent<Animator>().SetBool("white", true);			// disable white nucleus animation state, returning to idle
	}
	void NucleusToBlack() {
		nucleus.GetComponent<Animator> ().SetTrigger("fadeblack");			// enable nucleus to black animation
		nucleus.GetComponent<Animator>().SetBool("white", false);			// enable white nucleus animation state	

	}

}
