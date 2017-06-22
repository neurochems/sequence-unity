﻿using UnityEngine;
using System.Collections;

public class ParticleStatePattern : MonoBehaviour {

	public float evol;														// evolution level
	public int state;														// state indicator for inspector
	public new bool isLight;													// is light flag
	public float darkEvol, lightEvol;										// dark & light evolution level
	public float darkEvolStart, lightEvolStart;								// last dark & light evolution level (for delta calc)
	public float deltaDark, deltaLight;										// delta dark & light evolution level

	public float evolC, darkEvolC, lightEvolC;								// evol values at start of collision

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
	private Rigidbody rb;													// particle rigidbody
	//private ParticlePhysicsManager ppm;										// particle physics manager
	[HideInInspector] public SphereCollider[] sc;							// sphere colliders

	private MeshRenderer rendCore, rendShell, rendNucleus;					// mesh renderers (for lightworld colour changes)

	public int die;															// collision conflict check
	public bool stunned;													// stunned?
	public float stunDuration = 3f;											// duration of post-hit invulnerability
	private float stunTimer = 0f;											// stun timer
	//private bool shellShrinking, nucleusDeactivating;						// shell shrinking flag, nucleus deactivating flag
	[HideInInspector] public float shrinkTimer = 0f;						// shell deactivation timer

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
		// new state

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

		//ppm = GetComponent<ParticlePhysicsManager> ();						// init particle physics manager ref
		sc = GetComponents<SphereCollider> ();								// init sphere colliders ref

		rb = GetComponent<Rigidbody> ();									// init rigidbody ref

		rendCore = transform.FindChild("Core")
			.gameObject.GetComponent<MeshRenderer> ();						// init core mesh renderer ref
		rendShell = transform.FindChild("Shell")
			.gameObject.GetComponent<MeshRenderer> ();						// init shell mesh renderer ref
		rendNucleus = transform.FindChild("Nucleus")
			.gameObject.GetComponent<MeshRenderer> ();						// init nucleus mesh renderer ref

		//light = true;														// init light
		//lightEvol = 0.5f;													// init 0.5 evol
		currentState = zeroState;											// start at zero state
		//TransitionTo(0, 0, light, toLight, 0);								// CORE: shrink to zero size, fade to white
	}

	void Update () 
	{
		evol = lightEvol + darkEvol;										// update total evol value

		deltaDark = darkEvol - darkEvolStart;								// calculate deltaDark
		deltaLight = lightEvol - lightEvolStart;							// calculate deltaLight

		lightworld = psp.lightworld;										// update if lightworld
		changeParticles = psp.changeParticles;								// if changeParticles is true, update from player state pattern

		currentState.UpdateState ();										// frame updates from current state class

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
			//else if (currentState == ninthState) state = 9;
			//else if (currentState == tenthState) state = 10;
			updateStateIndicator = false;
		}

		// stun duration timer
		if (stunned) {
			//Stun ();
			stunTimer += Time.deltaTime;													// start timer
		}
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

		updateStateIndicator = true;

		if (toState == 0)	{ 														// to zero
			rb.mass = 1.0f;																// set mass
			gameObject.tag = "Zero";													// set tag
			sc[0].radius = 0.205f;														// update collision radius
			sc[1].radius = 0.200f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 1) {													// to first
			rb.mass = 2.0f;																// set mass
			gameObject.tag = "First";													// set tag
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 2) {													// to second
			rb.mass = 2.5f;																// set mass
			gameObject.tag = "Second";													// set tag
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 3) {													// to third
			rb.mass = 3.0f;																// set mass
			gameObject.tag = "Third";													// set tag
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 1.00f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 4) {													// to fourth
			rb.mass = 3.5f;																// set mass
			gameObject.tag = "Fourth";													// set tag
			sc[0].radius = 1.02f;														// update collision radius
			sc[1].radius = 1.00f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 5) {													// to fifth
			rb.mass = 4.0f;																// set mass
			gameObject.tag = "Fifth";													// set tag
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 6) {													// to sixth
			rb.mass = 4.5f;																// set mass
			gameObject.tag = "Sixth";													// set tag
			sc[0].radius = 0.51f;														// update collision radius
			sc[1].radius = 0.50f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 7) {													// to seventh
			rb.mass = 5.5f;																// set mass
			gameObject.tag = "Seventh";													// set tag
			sc[0].radius = 1.53f;														// update collision radius
			sc[1].radius = 1.50f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		else if (toState == 8) {													// to eighth
			rb.mass = 7.0f;																// set mass
			gameObject.tag = "Eighth";													// set tag
			sc[0].radius = 1.53f;														// update collision radius
			sc[1].radius = 1.50f;														// update collision radius
			SetParts(fromState, toState, fromLight, toLight, shape);					// set player parts
		}
		//new state 9 mass = 8.5
		//new state 10 mass = 10

		isLight = toLight;															// set light flag

		darkEvolStart = darkEvol;													// store dark evol at start of state
		lightEvolStart = lightEvol;													// store light evol at start of state
	}

	public void ChangeWorld(bool toLW, int fromState, int toState, bool toLight) 
	{
		Debug.Log ("particle change world");
		if (toLW) inLightworld = true;												// if to lightworld, set inLightworld
		if (!toLW) inLightworld = false;											// if not to lightworld, reset inLightworld

		pcm.ToOtherWorld (toLW, fromState, toState, toLight);						// change core
		psm.ToOtherWorld (toLW, fromState, toState, toLight);						// change shell
		pnm.ToOtherWorld (toLW, fromState, toState, toLight);						// change nucleus

		if (toLW) toLightworld = false;												// if to light world, reset toLightworld flag
		else if (!toLW)	toDarkworld = false;										// if to dark world, reset toDarkworld flag

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
	}

	/*private void LightWorldNucleus()
	{
		rendNucleus.material.SetColor("_Color", Color.white);						// change nucleus to white
		//changeParticles = false;													// reset change particles flag
	}*/

	// set particle parts (normal state transitions)
	private void SetParts(int fromState, int toState, bool fromLight, bool toLight, int shape)
	{
		pcm.Core (fromState, toState, fromLight, toLight, shape);					// change circle
		psm.Shell (fromState, toState, fromLight, toLight, shape);					// change shell
		pnm.Nucleus (fromState, toState, fromLight, toLight, shape);				// change nucleus
	}

}
