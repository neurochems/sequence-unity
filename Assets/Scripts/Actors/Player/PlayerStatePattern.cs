using UnityEngine;
using System.Collections;

public class PlayerStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public bool light = true, toLight;										// is light flag, to light flag

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

	//private GameObject nucleus, shell;								// reference to nucleus and shell children

	// component references
	private PlayerCoreManager pcm;											// player core manager (animations)
	private PlayerShellManager psm;											// player shell manager (animations)
	private PlayerNucleusManager pnm;										// player nucleus manager (animations)
	private UIManager uim;													// UI manager
	//private PlayerPhysicsManager ppm;										// player physics manager
	private SphereCollider[] sc;											// sphere colliders

	// timers & flags
	private int die;														// roll for collision conflicts
	public bool stunned;													// stunned?
	public float stunDuration = 5f;											// duration of post-hit invulnerability
	private float stunTimer = 0f;											// stun timer
	private bool shellShrinking, shell2Shrinking, nucleusDeactivating;		// shell/shell2 shrinking flag, nucleus deactivating flag
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer
	// UI
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

		pcm = GetComponent<PlayerCoreManager> ();							// initialize core manager ref
		psm = transform.FindChild ("Player Shell")
			.gameObject.GetComponent<PlayerShellManager>();					// initialize shell manager ref
		pnm = transform.FindChild ("Player Nucleus")
			.gameObject.GetComponent<PlayerNucleusManager>();				// initialize nucleus manager ref

		uim = GetComponent<UIManager> ();									// init ui manager ref
		//ppm = GetComponent<PlayerPhysicsManager> ();						// init player physics manager ref
		sc = GetComponents<SphereCollider> ();								// init sphere collider ref

		Destroy(GameObject.FindGameObjectWithTag("Destroy"));				// destroy old UI

	}

	void Start () 
	{
		currentState = zeroState;											// start at photon state
		TransitionTo(0, 0, light, toLight, 0);								// CORE: shrink to photon size, fade to white
	}

	void Update () 
	{
		if (uim.uI.GetComponent<StartOptions> ().inMainMenu) {								// if in menu
			evol = 0f;																			// prevent evol changes (no death in menu)
		}

		currentState.UpdateState ();														// frame updates from current state class

		// trigger timed stun
		if (stunned) {
			//Stun ();
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

	// public float TotalEvol()
		// darkEvol + lightEvol

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

	public void TransitionTo(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		if (toState == 0)	{ 														// to zero
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 1) {													// to first
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 2) {													// to second
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 3) {													// to third
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 4) {													// to fourth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 5) {													// to fifth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 6) {													// to sixth
			//rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 7) {													// to seventh
			// rb.mass = 0.2f;															// set mass
			SetZoomCamera(toState.ToString(), fromState.ToString(), light);				// CAMERA: zoom to size 20
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		//new state

		lastStateChange = Time.time;														// reset time since last state change
	}
		
	// camera - PUT IN SEPARATE SCRIPT
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

	// set player parts
	private void SetParts(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		pcm.Core (fromState, toState, fromLight, toLight, shape);					// change circle
		psm.Shell (fromState, toState, fromLight, toLight, shape);							// change shell
		pnm.Nucleus(fromState, toState, fromLight, toLight, shape);					// change nucleus
	}

}