using UnityEngine;
using System.Collections;

public class ParticleStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float darkEvolStart, lightEvolStart;								// last dark & light evolution level (for delta calc)
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public new bool light;													// is light flag
	public bool toLight;													// to light flag

	[HideInInspector] public IParticleState currentState;					// other object state
	//[HideInInspector] public int previousState;								// previous state index / Fn

	[HideInInspector] public DeadParticleState deadState;					// instance of dead state
	[HideInInspector] public ZeroParticleState zeroState;					// instance of zero state
	[HideInInspector] public FirstParticleState firstState;					// instance of first state
	[HideInInspector] public SecondParticleState secondState;				// instance of second state
	[HideInInspector] public ThirdParticleState thirdState;					// instance of third state
	[HideInInspector] public FourthParticleState fourthState;				// instance of fourth state
	[HideInInspector] public FifthParticleState fifthState;					// instance of fifth state
	[HideInInspector] public SixthParticleState sixthState;					// instance of sixth state
	[HideInInspector] public SeventhParticleState seventhState;				// instance of seventh state
	// new state

	public bool lightworld;													// is light world flag
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
	//private ParticlePhysicsManager ppm;										// particle physics manager
	private SphereCollider[] sc;											// sphere colliders

	private MeshRenderer rendCore, rendShell, rendNucleus;					// mesh renderers (for lightworld colour changes)

	public int die;															// collision conflict check
	public bool stunned;													// stunned?
	public float stunDuration = 3f;											// duration of post-hit invulnerability
	private float stunTimer = 0f;											// stun timer
	//private bool shellShrinking, nucleusDeactivating;						// shell shrinking flag, nucleus deactivating flag
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

		psp = GameObject.Find ("Player").
			gameObject.GetComponent<PlayerStatePattern> ();					// init player state pattern ref

		pcm = transform.FindChild("Core")
			.gameObject.GetComponent<ParticleCoreManager> ();				// initialize core manager ref
		psm = transform.FindChild ("Shell")
			.gameObject.GetComponent<ParticleShellManager>();				// initialize shell manager ref
		pnm = transform.FindChild ("Nucleus")
			.gameObject.GetComponent<ParticleNucleusManager>();				// initialize nucleus manager ref

		//ppm = GetComponent<ParticlePhysicsManager> ();						// init particle physics manager ref
		sc = GetComponents<SphereCollider> ();								// init sphere colliders ref

		rendCore = transform.FindChild("Core")
			.gameObject.GetComponent<MeshRenderer> ();						// init core mesh renderer ref
		rendShell = transform.FindChild("Shell")
			.gameObject.GetComponent<MeshRenderer> ();						// init shell mesh renderer ref
		rendNucleus = transform.FindChild("Nucleus")
			.gameObject.GetComponent<MeshRenderer> ();						// init nucleus mesh renderer ref
	}

	void Start () 
	{
		light = true;														// init light
		//lightEvol = 0.5f;													// init 0.5 evol
		currentState = zeroState;											// start at zero state
		//TransitionTo(0, 0, light, toLight, 0);								// CORE: shrink to zero size, fade to white
	}

	void Update () 
	{
		evol = lightEvol + darkEvol;										// update total evol value

		deltaDark = darkEvol - darkEvolStart;								// calculate deltaDark
		deltaLight = lightEvol - lightEvolStart;							// calculate deltaLight

		//if (toLightworld) ChangeWorld (true);
		//if (toDarkworld) ChangeWorld (false);

		lightworld = psp.lightworld;										// update if lightworld

		if (!changeParticles) psp.changeParticles = false;					// if changeParticles is false for one frame, reset in player state pattern
		if (changeParticles) changeParticles = psp.changeParticles;			// if changeParticles is true, update from player state pattern

		currentState.UpdateState ();										// frame updates from current state class

		// stun duration timer
		if (stunned) {
			//Stun ();
			stunTimer += Time.deltaTime;													// start timer
		}

		// deactivating shell/nucleus timer
		//if (shellShrinking || nucleusDeactivating) shrinkTimer += Time.deltaTime;			// start timer
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

	// BEHAVIOURS \\ - PUT IN SEPARATE SCRIPT

	public void SpawnZero (int num)
	{
		GetComponent<SpawnParticle> ().SpawnPhoton (num);
	}

	public void SpawnFirst (int num)
	{
		GetComponent<SpawnParticle> ().SpawnElectron (num);
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

	// STATE TRANSTITIONS \\

	public void TransitionTo(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		if (toState == 0)	{ 														// to zero
			// rb.mass = 0.2f;															// set mass
			gameObject.tag = "Zero";													// set tag
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 1) {													// to first
			// rb.mass = 0.2f;															// set mass
			gameObject.tag = "First";													// set tag
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 2) {													// to second
			//rb.mass = 0.2f;															// set mass
			gameObject.tag = "Second";													// set tag
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 3) {													// to third
			//rb.mass = 0.2f;															// set mass
			gameObject.tag = "Third";													// set tag
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 4) {													// to fourth
			//rb.mass = 0.2f;															// set mass
			gameObject.tag = "Fourth";													// set tag
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 5) {													// to fifth
			//rb.mass = 0.2f;															// set mass
			gameObject.tag = "Fifth";													// set tag
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 6) {													// to sixth
			//rb.mass = 0.2f;															// set mass
			gameObject.tag = "Sixth";													// set tag
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 7) {													// to seventh
			// rb.mass = 0.2f;															// set mass
			gameObject.tag = "Seventh";													// set tag
			sc[0].radius = 0.51f;														// shrink collision radius in
			//sc[0].center = new Vector3(0f, 0f, 0f);									// level circle collider on world
			sc[1].radius = 0.51f;														// shrink collision radius in
			sc[1].center = new Vector3(0f, 0f, 0f);										// level circle collider on world
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		//new state

		darkEvolStart = darkEvol;													// store dark evol at start of state
		lightEvolStart = lightEvol;													// store light evol at start of state
	}

	public void ChangeWorld(bool toLW, int fromState, int toState, bool toLight) 
	{
		pcm.ToOtherWorld (toLW, fromState, toState, toLight);								// change core
		//pcm.ScaleTo(false, "hidden", "zero");
		psm.ToOtherWorld (toLW, fromState, toState, toLight);								// change shell
		pnm.ToOtherWorld (toLW, fromState, toState, toLight);								// change nucleus
		if (toLW) toLightworld = false;												// if to light world, reset toLightworld flag
		else if (!toLW)	toDarkworld = false;										// if to dark world, reset toDarkworld flag

	}

	// set particle parts (normal state transitions)
	private void SetParts(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		pcm.Core (fromState, toState, fromLight, toLight, shape);					// change circle
		psm.Shell (fromState, toState, fromLight, toLight, shape);					// change shell
		pnm.Nucleus (fromState, toState, fromLight, toLight, shape);				// change nucleus
	}

}
