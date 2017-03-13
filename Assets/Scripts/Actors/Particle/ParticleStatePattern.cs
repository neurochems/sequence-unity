using UnityEngine;
using System.Collections;

public class ParticleStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public bool light = true, toLight;										// is light flag, to light flag

	[HideInInspector] public IParticleState currentState;					// other object state
	//[HideInInspector] public int previousState;								// previous state index / Fn

	[HideInInspector] public DeadParticleState deadState;					// instance of dead state
	[HideInInspector] public ZeroParticleState zeroState;				// instance of photon state
	[HideInInspector] public FirstParticleState firstState;			// instance of electron state
	[HideInInspector] public SecondParticleState secondState;			// instance of electron2 state
	[HideInInspector] public ThirdParticleState thirdState;					// instance of shell state
	[HideInInspector] public FourthParticleState fourthState;				// instance of shell2 state
	[HideInInspector] public FifthParticleState fifthState;					// instance of atom state
	[HideInInspector] public SixthParticleState sixthState;					// instance of atom2 state
	[HideInInspector] public SeventhParticleState seventhState;					// instance of atom2 state
	// new state

	//public float attractionRange = 20f;										// particle sensing distance
	//[HideInInspector] public Transform attractionTarget;					// particle sensing target

	//private GameObject nucleus, shell;										// reference to nucleus and shell children
	[HideInInspector] public GameObject self;								// reference to this gameobject

	//component references
	private ParticleCoreManager pcm;										// particle core manager (animations)
	private ParticleShellManager psm;										// particle core manager (animations)
	private ParticleNucleusManager pnm;										// particle core manager (animations)
	//private ParticlePhysicsManager ppm;										// particle physics manager
	private SphereCollider[] sc;											// sphere colliders

	private int die;														// roll for collision conflicts
	public bool stunned;													// stunned?
	public float stunDuration = 3f;											// duration of post-hit invulnerability
	private float stunTimer = 0f;											// stun timer
	private bool shellShrinking, nucleusDeactivating;						// shell shrinking flag, nucleus deactivating flag
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer

	void Awake()
	{
		evol = 0f;															// initialize evol
		darkEvol = 0f;														// initialize dark evol
		lightEvol = 0f;														// initialize light evol

		deltaDark = 0f;														// initialize delta dark evol
		deltaLight = 0f;													// initialize delta light evol

		deadState = new DeadParticleState (this);							// initialize dead state
		zeroState = new ZeroParticleState (this);							// initialize zero state
		firstState = new FirstParticleState (this);							// initialize first state
		secondState = new SecondParticleState (this);						// initialize second state
		thirdState = new ThirdParticleState (this);							// initialize third state
		fourthState = new FourthParticleState (this);						// initialize fourth state
		fifthState = new FifthParticleState (this);							// initialize fifth state
		sixthState = new SixthParticleState (this);							// initialize sixth state
		seventhState = new SeventhParticleState (this);						// initialize seventh state
		// new state

		pcm = GetComponent<ParticleCoreManager> ();							// initialize core manager ref
		psm = transform.FindChild ("Shell")
			.gameObject.GetComponent<ParticleShellManager>();				// initialize shell manager ref
		pnm = transform.FindChild ("Nucleus")
			.gameObject.GetComponent<ParticleNucleusManager>();				// initialize nucleus manager ref

		//ppm = GetComponent<ParticlePhysicsManager> ();						// init particle physics manager ref
		sc = GetComponents<SphereCollider> ();								// init sphere colliders ref
	}

	void Start () 
	{
		currentState = zeroState;											// start at photon state
		TransitionTo(0, 0, light, toLight, 0);								// CORE: shrink to photon size, fade to white
	}

	void Update () 
	{
		currentState.UpdateState ();										// frame updates from current state class

		// stun duration timer
		if (stunned) {
			//Stun ();
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

	// BEHAVIOURS \\

	public void SpawnZero (int num)
	{
		GetComponent<SpawnParticle> ().SpawnPhoton (num);
	}

	public void SpawnFirst (int num)
	{
		GetComponent<SpawnParticle> ().SpawnElectron (num);
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
	public void SubDark(float changeAmount) {
		lightEvol -= changeAmount;											// subtract dark evol level
	}

	public void AddLight(float changeAmount) {
		darkEvol += changeAmount;											// add light evol level
	}
	public void SubLight(float changeAmount) {
		lightEvol -= changeAmount;											// subtract light evol level
	}

	// STATE TRANSTITIONS \\

	public void TransitionTo(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		if (toState == 0)	{ 														// to zero
			// rb.mass = 0.2f;															// set mass
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 1) {													// to first
			// rb.mass = 0.2f;															// set mass
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 2) {													// to second
			//rb.mass = 0.2f;															// set mass
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 3) {													// to third
			//rb.mass = 0.2f;															// set mass
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 4) {													// to fourth
			//rb.mass = 0.2f;															// set mass
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 5) {													// to fifth
			//rb.mass = 0.2f;															// set mass
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 6) {													// to sixth
			//rb.mass = 0.2f;															// set mass
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 7) {													// to seventh
			// rb.mass = 0.2f;															// set mass
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		//new state
	}
		
	// transitions \\

	// set player parts
	private void SetParts(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		pcm.Core (fromState, toState, fromLight, toLight, shape);					// change circle
		psm.Shell (fromState, toState, fromLight, toLight, shape);							// change shell
		pnm.Nucleus(fromState, toState, fromLight, toLight, shape);					// change nucleus
	}

	/* core
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
	*/
}
