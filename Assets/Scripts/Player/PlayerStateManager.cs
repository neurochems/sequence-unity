using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerStateManager : MonoBehaviour {

	// player states
	public enum PlayerState {Dead, Photon, Electron, Electron2, Shell, Shell2, Atom, Atom2, StateEight, StateNine, StateTen}

	public bool godMode = false;

	// player state management
	public float evol;																										// current evol level
	public int nEvol;																										// current nEvol level
	public PlayerState currentState, previousState;																			// current player state
		// state change parameters
	private float /*deathThreshold, */photonThreshold, electronThreshold, electron2Threshold, shellThreshold, shell2Threshold, atomThreshold, atom2Threshold;		
	private float previousEvol = 0.0f, currentEvol = 0.0f;																	// state change checks (evol)
	private int previousNEvol, currentNEvol;																				// state change checks (nEvol)
	private bool start, evolving, devolving, dead;																			// state transition checkers
												// dead = dark world
	private bool coreScale = true, coreColour = true;																		// core state checkers
	private bool shellActive = false, shellChange = false;																	// shell state checkers
	private bool nucleusState = false, nucleusColour = true;																// nucleus state checkers

	// gameobject references & utility
	public GameObject nucleus, shell;
	public GameObject lostParticle, lostParticleParent;
	public FauxGravityAttractor lostParticleWorld;

	// physics
	private Rigidbody rb;
	private bool bump = false, hasCollided = false;
	public int bumpStrength = 100;

	// UI
	public Canvas uI;
	public GameObject deathFade;

	// endless
	public bool endless = false;

	// debug info
	public UnityEngine.UI.Text numParticles;
	public GameObject debugCollectables;
	private int numTotal, numPhoton, numElectron, numElectron2, numShell, numShell2, numAtom, numAtom2;

	// ui text anim
	public GameObject[] overlayTextTrigger;

	// timers
	private float lastStateChange = 0.0f;
	[HideInInspector]
	public float sinceGameBegin = 0.0f;
	private bool timeCheck = true;

// INIT THINGS \\

	void Start () {																		
		// init debug
		//numParticles = uI.gameObject.transform.FindChild("Debug Text").GetComponent<UnityEngine.UI.Text>();

		// destroy old UI
		Destroy(GameObject.FindGameObjectWithTag("Destroy"));

		// init components
		rb = GetComponent<Rigidbody> ();
		rb.mass = 0.2f;

		// init thresholds
		//deathThreshold = -1;																// set death threshold
		photonThreshold = 0.0f;																// set photon threshold
		electronThreshold = 1.0f;															// set electron threshold
		electron2Threshold = 1.5f;															// set electron2 threshold
		shellThreshold = 2.0f;																// set shell threshold
		shell2Threshold = 3.0f;																// set shell2 threshold
		atomThreshold = 5.0f;																// set atom threshold
		atom2Threshold = 8.0f;																// set atom2 threshold

		// prevent player trigger collisions at game start
		//StartCoroutine (PreventPlayerDamage (7.0f));											

		// init player state
		SetCurrentState(PlayerState.Photon);												// begin as photon
		evol = 0.0f;																		// evolution start
		nEvol = 0;																			// evolution start
		start = true;																		// evolution start

	}

// PHYSICS THINGS \\

	void FixedUpdate() {
		if (bump) {																							// bump away at collision
			rb.AddRelativeForce (Vector3.Slerp(rb.velocity, (rb.velocity * -bumpStrength), 1.0f));					// slerp force of velocity * a factor in the opposite direction
			//Debug.Log ("collision bump");
			bump = false;																						// reset collision trigger
		}
	}

// STATE MACHINE \\

	void Update () {

	// DEBUG particle counts \\
		
		// total children
		numTotal = debugCollectables.transform.childCount;
		
		// iterate particles, logging tag type
		foreach (Transform child in debugCollectables.transform) {
			if (child.CompareTag("Photon")) numPhoton += 1;
			else if (child.CompareTag("Electron")) numElectron += 1;
			else if (child.CompareTag("Electron2")) numElectron2 += 1;
			else if (child.CompareTag("Shell")) numShell += 1;
			else if (child.CompareTag("Shell2")) numShell2 += 1;
			else if (child.CompareTag("Atom")) numAtom += 1;
			else if (child.CompareTag("Atom2")) numAtom2 += 1;
		}
		
		// display info
		numParticles.text = "Total: " + numTotal + "\nPhotons: " + numPhoton + "\nElectrons: " + numElectron + "\nElectron2s: " + numElectron2 + 
			"\nShells: " + numShell + "\nShell2s: " + numShell2 + "\nAtoms: " + numAtom + "\nAtom2s: " + numAtom2;
		
		// reset numbers
		numTotal = 0;
		numPhoton = 0;
		numElectron = 0;
		numElectron2 = 0;
		numShell = 0;
		numShell2 = 0;
		numAtom = 0;
		numAtom2 = 0;

		// set player attributes and appearance

		currentEvol = evol;																	// set current evol (evol direction check)
		currentNEvol = nEvol;																	// set current evol (evol direction check)

		// checks for OVERLAY TEXT
		if (!uI.GetComponent<StartOptions>().inMainMenu && timeCheck == true) {				// if game start (not in menu)
			sinceGameBegin = Time.time;															// check time
			timeCheck = false;																	// check time only once
		}


	// SWITCH START \\
		switch (currentState) {															

		// dead state (evol -1) \\

		case PlayerState.Dead:
			if (dead) {																								// if dead
				StartCoroutine (PlayerDeath ());																		// death sequence
				dead = false;																							// reset dead trigger
				Debug.Log ("dead");
			}
			break;
		
		// photon state (evol 0 | f0) \\

		case PlayerState.Photon:
			// checks for OVERLAY TEXT (FIRST)
			if (!uI.GetComponent<StartOptions> ().inMainMenu && GetGameElapsed () >= 5.0f) {							// 5 sec into playable/not-menu game	// OVERLAY TEXT
				overlayTextTrigger [0].SetActive (true);																	// activate text
				overlayTextTrigger [0].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
			}

			// checks for ANIMATIONS & STATE TRANSITION INTERACTIONS
			if ((currentEvol == photonThreshold) && start) {
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				start = false;																								// reset start sequence trigger
			}

			if ((currentEvol < electronThreshold) && (previousState == PlayerState.Shell2) && devolving) {				// if FROM SHELL2 and devolving
				//StartCoroutine(ResetZoomCamera("shell", false, 3.0f));														// CAMERA: reset anim triggers after delay
				nEvol = 0;																									// set fn to 0n
				rb.mass = 0.25f;																							// set mass
				CoreToWhite();																								// CORE: return to idle white
				CoreToPhoton();																								// CORE: shrink to photon size, fade to white
				ShellShrink();																								// SHELL: shrink
				NucleusDisable(); 																							// NUCLEUS: fade to white, then deactivate
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electronThreshold) && (previousState == PlayerState.Shell) && devolving) {		// if FROM SHELL and devolving
				//StartCoroutine(ResetZoomCamera("shell", false, 3.0f));														// CAMERA: reset anim triggers after delay
				nEvol = 0;																									// set fn to 0
				rb.mass = 0.25f;																							// set mass
				CoreToWhite();																								// CORE: return to idle white
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				ShellShrink ();																								// SHELL: shrink
				NucleusDisable ();																							// NUCLEUS: fade to white, then deactivate
				devolving = false;																							// reset devolving trigger
			}
			else if ((currentEvol < electronThreshold) && (previousState == PlayerState.Electron2) && devolving) {	// if FROM ELECTRON2 and devolving
				//StartCoroutine(ResetZoomCamera("electron", false, 2.0f));														// CAMERA: reset anim triggers after delay
				nEvol = 0;																									// set fn to 0
				rb.mass = 0.25f;																							// set mass
				NucleusDisable ();																							// NUCLEUS: fade to white, then deactivate
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electronThreshold) && (previousState == PlayerState.Electron) && devolving) {		// if FROM ELECTRON and devolving
				//SetZoomCamera("photon", false);																				// CAMERA: reset anim triggers
				nEvol = 0;																									// set fn to 0
				rb.mass = 0.25f;																							// set mass
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				devolving = false;																							// reset devolving trigger
			} 

			// checks for STATE CHANGES
			if ((previousEvol < electronThreshold) && (currentEvol < photonThreshold)) {							// if electron -> dead
				dead = true;																							// dead trigger
				SetCurrentState (PlayerState.Dead);																		// kill player
			} 
			else if (currentEvol >= electronThreshold) {															// if evol electron
				evolving = true;																						// evolving trigger (GrowShell once)
				SetZoomCamera("electron", "photon", false);																// CAMERA: zoom to size 25
				SetCurrentState (PlayerState.Electron);																	// evolve to shell state
			}

			previousState = PlayerState.Photon;																		// set state last frame 

			break;

		// electron state (evol 1 | f1) \\

		case PlayerState.Electron:

			//transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("zoom", false);

			// checks for OVERLAY TEXT (SECOND)
			if (GetStateElapsed () > 2.0f) {																			// 2 sec into being electron				
				overlayTextTrigger [1].SetActive (true);																	// activate text
				overlayTextTrigger [1].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
			}	

			// checks on INCOMING TRIGGERS
			if ((currentEvol >= electronThreshold) && evolving) {														// if FROM PHOTON and evolving
				//StartCoroutine(ResetZoomCamera("photon", false, 2.0f));														// CAMERA: reset anim triggers after delay
				nEvol += 1;																									// increase fn
				rb.mass = 0.50f;																							// set mass
				CoreToElectron ();																							// CORE: grow to electron size, is white
				evolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == PlayerState.Atom2) && devolving) {		// if FROM ATOM2 and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 5;																									// set fn to 0n
				rb.mass = 0.50f;																							// set mass
				ShellShrink ();																								// SHELL: shrink
				NucleusToWhite ();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == PlayerState.Atom) && devolving) {		// if FROM ATOM and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 4;																									// set fn to 0n
				rb.mass = 0.50f;																							// set mass
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == PlayerState.Shell2) && devolving) {		// if FROM SHELL2 and devolving
				//StartCoroutine(ResetZoomCamera("shell", false, 3.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 3;																									// set fn to 0n
				rb.mass = 0.50f;																							// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == PlayerState.Shell) && devolving) {		// if FROM SHELL and devolving
				//StartCoroutine(ResetZoomCamera("shell", false, 3.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 2;																									// set fn to 0
				rb.mass = 0.50f;																							// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				NucleusToWhite ();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == PlayerState.Electron2) && devolving) {	// if FROM ELECTRON2 and devolving
				nEvol -= 1;																									// decrease fn
				rb.mass = 0.50f;																							// set mass
				NucleusToWhite ();																							// NUCLEUS: fade out to white
				devolving = false;																							// reset devolving trigger
			} 

			// checks on OUTGOING METHODS & TRIGGERS
			if ((previousEvol < electron2Threshold) && (currentEvol < photonThreshold)) {								// if evol electron -> dead
				SpawnParticle (2);																						// spawn 1 particle
				dead = true;																							// dead trigger
				SetCurrentState (PlayerState.Dead);																		// kill player
			} 
			else if ((previousEvol < electron2Threshold) && (currentEvol < electronThreshold)) {					// if evol electron -> photon
				SetZoomCamera("photon", "electron", true);																// CAMERA: set anim triggers
				SpawnParticle (1);																						// spawn 1 particle
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Photon);																	// devolve to photon
			} 
			else if (evol >= electron2Threshold) {																// if evol electron2
				evolving = true;																						// evolving trigger
				SetCurrentState (PlayerState.Electron2);																// evolve to electron2 state
			}

			previousState = PlayerState.Electron;																		// set state last frame

			break;

		// electron2 state (evol 1 | f2) \\

		case PlayerState.Electron2:
			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if ((currentEvol >= electron2Threshold) && evolving) {														// if FROM ELECTRON and evolving
				nEvol += 1;																									// increase fn
				rb.mass = 0.75f;																							// set mass
				NucleusEnable();																							// NUCLEUS: enable, fade in to black
				NucleusToBlack();																							// NUCLEUS: fade to black
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == PlayerState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 4;																									// decrease fn
				rb.mass = 0.75f;																							// set mass
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == PlayerState.Atom) && devolving) {			// if FROM ATOM and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 3;																									// decrease fn
				rb.mass = 0.75f;																							// set mass
				ShellShrink ();																								// SHELL: shrink
				NucleusToBlack ();																							// NUCLEUS: fade to black
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == PlayerState.Shell2) && devolving) {			// if FROM SHELL2 and devolving
				//StartCoroutine(ResetZoomCamera("shell", false, 3.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 2;																									// decrease fn
				rb.mass = 0.75f;																							// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				NucleusToBlack ();																							// NUCLEUS: fade to black
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == PlayerState.Shell) && devolving) {			// if FROM SHELL and devolving
				//StartCoroutine(ResetZoomCamera("shell", false, 3.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 1;																									// decrease fn
				rb.mass = 0.75f;																							// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			}   		
			
			// checks on OUTGOING METHODS & TRIGGERS
			if ((previousEvol < shellThreshold) && (currentEvol < photonThreshold)) {								// if evol electron2 -> dead
				SpawnParticle (3);																						// spawn 3 particles
				dead = true;																							// dead trigger
				SetCurrentState (PlayerState.Dead);																		// kill player
			} 
			else if ((previousEvol < shellThreshold) && (currentEvol < electronThreshold)) {						// if evol electron2 -> photon
				SetZoomCamera("photon", "electron", true);																// CAMERA: set anim triggers
				SpawnParticle (2);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Photon);																	// devolve to photon
			} 
			else if ((previousEvol < shellThreshold) && (currentEvol < electron2Threshold)) {						// if evol electron2 -> electron
				SpawnParticle (1);																						// spawn 1 particle
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Electron);																	// devolve to electron
			} 
			else if (evol >= shellThreshold) {																		// if evol shell
				evolving = true;																						// evolving trigger
				SetZoomCamera("shell", "electron", false);																// CAMERA: zoom to size 40
				SetCurrentState (PlayerState.Shell);																	// evolve to shell state
			}

			previousState = PlayerState.Electron2;																		// set state last frame

			break;

		// shell state (evol 2 | f3) \\

		case PlayerState.Shell:
			//transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("zoom", false);

			// checks for OVERLAY TEXT (THIRD)
			if (GetStateElapsed () > 2.0f) {																			// 2 sec into being shell				
				overlayTextTrigger [2].SetActive (true);																	// activate text
				overlayTextTrigger [2].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
			}													

			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if (currentEvol >= shellThreshold && evolving) {															// if FROM ELECTRON2 and evolving
				//StartCoroutine(ResetZoomCamera("electron", false, 3.0f));														// CAMERA: reset anim triggers after delay
				nEvol += 1;																									// increase fn
				rb.mass = 0.50f;																							// set mass
				CoreToBlack();																								// CORE: fade to black
				ShellGrow ();																								// SHELL: grow
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < shell2Threshold) && (previousState == PlayerState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 3;																									// decrease fn
				rb.mass = 0.50f;																							// set mass
				CoreToBlack();																								// CORE: fade to black
				devolving = true;																							// devolving trigger
			} 
			else if ((currentEvol < shell2Threshold) && (previousState == PlayerState.Atom) && devolving) {			// if FROM ATOM and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 2;																									// decrease fn
				rb.mass = 0.50f;																							// set mass
				CoreToBlack();																								// CORE: fade to black
				NucleusToBlack();																							// NUCLEUS: fade to black
				devolving = true;																							// devolving trigger
			} 
			else if ((currentEvol < shell2Threshold) && (previousState == PlayerState.Shell2) && devolving) {			// if FROM SHELL2 and devolving
				nEvol -= 1;																									// decrease fn
				rb.mass = 0.50f;																							// set mass
				NucleusToBlack();																							// NUCLEUS: fade to black
				devolving = false;																							// reset devolving trigger
			}

			// checks for STATE CHANGES
			if ((previousEvol < shell2Threshold) && (currentEvol < photonThreshold)) {								// if evol shell -> dead
				SpawnParticle(4);																						// spawn 4 particles 
				dead = true;																							// dead trigger
				SetCurrentState (PlayerState.Dead);																		// dead
			} 
			else if ((previousEvol < shell2Threshold) && (currentEvol < electronThreshold)) {							// if evol shell -> photon
				SetZoomCamera("photon", "shell", true);																		// CAMERA: set anim triggers
				SpawnParticle(3);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Photon);																	// devolve to photon state
			}
			else if ((previousEvol < shell2Threshold) && (currentEvol < electron2Threshold)) {						// if evol shell -> electron
				SetZoomCamera("electron", "shell", true);																// CAMERA: Set anim triggers
				SpawnParticle(2);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState(PlayerState.Electron);																	// devolve to electron state
			} 
			else if ((previousEvol < shell2Threshold) && (currentEvol < shellThreshold)) {							// if evol shell -> electron2
				SetZoomCamera("electron", "shell", true);																// CAMERA: Set anim triggers
				SpawnParticle(3);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Electron2);																// devolve to electron2 state
			} 
			else if (currentEvol >= shell2Threshold) {															// if evol shell2
				evolving = true;																						// evolving trigger
				SetCurrentState (PlayerState.Shell2);																	// evolve to shell2 state
			}

			previousState = PlayerState.Shell;																		// set state last frame

			break;
		
		// shell2 state (evol 3 | f4) \\												

		case PlayerState.Shell2:
			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if (currentEvol >= shell2Threshold && evolving) {															// if FROM SHELL and evolving
				nEvol += 1;																									// increase fn
				rb.mass = 0.75f;																							// set mass
				NucleusToWhite();																							// NUCLEUS: fade to white
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < atomThreshold) && (previousState == PlayerState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 2;																									// decrease fn
				rb.mass = 0.75f;																							// set mass
				CoreToBlack();																								// CORE: fade to black
				NucleusToWhite();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < atomThreshold) && (previousState == PlayerState.Atom) && devolving) {				// if FROM ATOM and devolving
				//StartCoroutine(ResetZoomCamera("atom", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol -= 1;																									// decrease fn
				rb.mass = 0.75f;																							// set mass
				CoreToBlack();																								// CORE: fade to black
				devolving = false;																							// reset devolving trigger
			}

			// checks for STATE CHANGES
			if ((previousEvol < atomThreshold) && (currentEvol < photonThreshold)) {								// if evol shell2 -> dead
				SpawnParticle(4);																						// spawn 4 particles
				dead = true;																							// dead trigger
				SetCurrentState (PlayerState.Dead);																		// dead
			} 
			else if (previousEvol < atomThreshold && currentEvol < electronThreshold) {							// if evol shell2 -> photon
				SetZoomCamera("photon", "shell", true);																	// CAMERA: zoom to size 20
				SpawnParticle(4);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Photon);																	// devolve to photon state
			} 
			else if (previousEvol < atomThreshold && currentEvol < electron2Threshold) {							// if evol shell2 -> electron
				SetZoomCamera("electron", "shell", true);																		// CAMERA: zoom to size 25
				SpawnParticle(3);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState(PlayerState.Electron);																	// devolve to electron state
			} 
			else if (previousEvol < atomThreshold && currentEvol < shellThreshold) {								// if evol shell2 -> electron2
				SetZoomCamera("electron", "shell", true);																		// CAMERA: zoom to size 25
				SpawnParticle(2);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Electron2);																// devolve to electron2 state
			} 
			else if (previousEvol < atomThreshold && currentEvol < shell2Threshold) {								// if evol shell2 -> shell
				SpawnParticle(1);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Shell);																	// devolve to shell state
			} 
			else if (currentEvol >= atomThreshold) {																// if evolatom
				SetZoomCamera("atom", "shell", false);																			// CAMERA: zoom to size 60
				evolving = true;																						// evolving trigger (GrowShell once)
				SetCurrentState (PlayerState.Atom);																		// evolve to atom state
			}

			previousState = PlayerState.Shell2;																		// set state last frame

			break;

		// atom state (evol 5 | f5) \\

		case PlayerState.Atom:
			// checks for OVERLAY TEXT (FOURTH)
			if (GetStateElapsed () > 2.0f) {																			// 2 sec into being atom				
				overlayTextTrigger [3].SetActive (true);																	// activate text
				overlayTextTrigger [3].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
			}

			// checks for OVERLAY TEXT (ENDING)
			if (GetStateElapsed () > 40.0f) {																			// 20 sec into being atom				
				overlayTextTrigger [4].SetActive (true);																	// activate text
				overlayTextTrigger [4].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
				if (!endless) 																								// if not endless
					StartCoroutine(Endgame());																					// ENDGAME
			}	

			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if ((currentEvol >= atomThreshold) && evolving) {															// if FROM SHELL2 and evolving
				//StartCoroutine(ResetZoomCamera("shell", false, 5.0f));														// CAMERA: reset anim triggers after delay
				nEvol += 1;																									// increase fn
				rb.mass = 1.0f;																								// set mass
				CoreToWhite();																								// CORE: fade to white
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < atom2Threshold)  && (previousState == PlayerState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				nEvol -= 1;																									// decrease fn
				rb.mass = 1.0f;																								// set mass
				NucleusToWhite();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			}

			// checks for STATE CHANGES
			if ((previousEvol < atom2Threshold) && (currentEvol < photonThreshold)) {								// if evol atom -> dead
				SpawnParticle(4);																						// spawn 4 particles
				dead = true;																							// dead trigger
				SetCurrentState (PlayerState.Dead);																		// dead
			} 
			else if (previousEvol < atom2Threshold && currentEvol < electronThreshold) {							// if evol atom -> photon
				SetZoomCamera("photon", "atom", true);																			// CAMERA: zoom to size 20
				SpawnParticle(4);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Photon);																	// devolve to photon state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < electron2Threshold) {							// if evol atom -> electron
				SetZoomCamera("electron", "atom", true);																		// CAMERA: zoom to size 25
				SpawnParticle(4);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState(PlayerState.Electron);																	// devolve to electron state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < shellThreshold) {								// if evol atom -> electron2
				SetZoomCamera("electron", "atom", true);																		// CAMERA: zoom to size 25
				SpawnParticle(3);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Electron2);																// devolve to electron2 state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < shell2Threshold) {							// if evol atom -> shell
				SetZoomCamera("shell", "atom", true);																			// CAMERA: zoom to size 40
				SpawnParticle(2);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Shell);																	// devolve to shell state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < atomThreshold) {								// if evol atom -> shell2
				SetZoomCamera("shell", "atom", true);																			// CAMERA: zoom to size 40
				SpawnParticle(1);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (PlayerState.Shell2);																	// devolve to shell2 state
			} 

			if (previousEvol < atom2Threshold && currentEvol < atom2Threshold) {								// if same state
				int deltaEvol = Mathf.FloorToInt(previousEvol - currentEvol);											// check delta evol, rounding float down
				if (deltaEvol > 0)																					// if delta > 0 (losing evol within state)
					SpawnParticle(deltaEvol);																				// spawn delta particles 
			}
			else if (currentEvol >= atomThreshold) {																// if evol atom2
				evolving = true;																						// evolving trigger (GrowShell once)
				SetCurrentState (PlayerState.Atom2);																	// evolve to atom2 state
			}

			previousState = PlayerState.Atom;																		// set state last frame

			break;

		}

		previousEvol = currentEvol;															// previous evol value last frame
		previousNEvol = currentNEvol;														// previous nEvol value last frame
	}

	// trigger methods \\

		// zoom camera
		void SetZoomCamera(string set, string reset, bool devol) {

			transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetTrigger (set);								// set trigger state

//		if (set == "photon") {
//			
//		}

			transform.FindChild ("Follow Camera").GetComponent<Animator> ().ResetTrigger (reset);								// reset trigger state

			if (devol == true)																								// if devol true
				transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", true);						// set devolve trigger
			else																											// else
				transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", false);						// reset devolve trigger

		}

		void ResetZoomCamera(bool devol) {
			
			//transform.FindChild ("Follow Camera").GetComponent<Animator> ().ResetTrigger (state);								// reset trigger state

			//yield return new WaitForSeconds (time);

			if (devol == true)																									// if devol true
				transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", true);						// set devolve trigger
			else																											// else
				transform.FindChild ("Follow Camera").GetComponent<Animator> ().SetBool ("devolve", false);						// reset devolve trigger
		}

		// core
		void CoreToPhoton() {
			coreScale = true;																	// photon core trigger
			coreColour = true;																	// white core trigger
			Core(true, true);																	// CORE: shrink to photon size, is white
		}
		void CoreToElectron() {
			coreScale = true;																	// reset photon core trigger
			Core(false, true);																	// CORE: grow to normal size, is white
		}
		void CoreToWhite() {
			coreColour = true;																	// white core trigger
			Core(true);																			// CORE: fade to white
		}
		void CoreToBlack() {
			coreColour = true;																	// reset white core trigger
			Core(false);																		// CORE: fade to black
		}

		// shell
		void ShellGrow() {
			shellChange = true;																	// grow shell trigger
			Shell(true);																		// SHELL: grow
		}
		void ShellShrink() {
			shellChange = true;																	// reset shell grow trigger		
			Shell(false);																		// SHELL: shrink
		}

		// nucleus
		void NucleusEnable() {
			nucleusState = true;																// nucleus state trigger
			Nucleus(false, true);																// NUCLEUS: fade in to black, enable
		}
		void NucleusDisable() {
			nucleusState = true;																// reset active nucleus trigger																					
			Nucleus (true, false);																// NUCLEUS: fade to white, then deactivate
		}
		void NucleusToWhite() {
			nucleusColour = true;																// white nucleus trigger																					
			Nucleus(true);																		// NUCLEUS: fade to white
		}
		void NucleusToBlack() {
			nucleusColour = true;																// reset white nucleus trigger
			Nucleus (false);																	// NUCLEUS: fade to black
		}

// COLLISION RULES & SCORING \\

	void OnTriggerEnter(Collider other) {

		if (godMode) hasCollided = false;																												// has collided trigger
		else hasCollided = true;																													// has collided trigger

		if (other.gameObject.CompareTag ("Photon")) {																					// collide with photon
			StartCoroutine (PreventPlayerDamage (2.0f));																						// prevent player trigger collisions
			if ((evol >= other.gameObject.GetComponent<ParticleStateManager> ().evol) && hasCollided) {										// if any state >= other photon
				AddEvol(other.gameObject.GetComponent<ParticleStateManager>(), 0.5f, 1.0f);														// add 0.5 evol, take 1 evol
			}
		} 
		else if (other.gameObject.CompareTag ("Electron")) {																			// collide with electron
			StartCoroutine (PreventPlayerDamage (2.0f));																						// prevent player trigger collisions
			if ((evol < other.gameObject.GetComponent<ParticleStateManager> ().evol) && hasCollided) {										// if < other electron
				SubtractEvol(other.gameObject.GetComponent<ParticleStateManager>(), 1.0f, 0.5f);												// subtract 1 evol, give 1 evol
			} 
			else if ((evol >= other.gameObject.GetComponent<ParticleStateManager> ().evol) && hasCollided) {								// if any state >= other electron
				AddEvol(other.gameObject.GetComponent<ParticleStateManager>(), 1.0f, 1.0f);														// add 1 evol, take 1 evol
			}
		}
		else if (other.gameObject.CompareTag ("Electron2")) {																			// collide with electron2
			StartCoroutine (PreventPlayerDamage (2.0f));																						// prevent player trigger collisions
			if ((evol < other.gameObject.GetComponent<ParticleStateManager> ().evol) && hasCollided) {										// if < other electron2
				SubtractEvol(other.gameObject.GetComponent<ParticleStateManager>(), 1.0f, 1.0f);												// subtract 1 evol, give 1 evol
			} 
			else if ((evol >= other.gameObject.GetComponent<ParticleStateManager> ().evol) && hasCollided) {								// if any state >= other electron2
				AddEvol(other.gameObject.GetComponent<ParticleStateManager>(), 1.0f, 1.0f);														// add 1 evol, take 1 evol
			}
		}
		else if (other.gameObject.CompareTag ("Shell")) {																				// collide with shell
			StartCoroutine(PreventPlayerDamage(2.0f));																							// prevent player trigger collisions
			if ((evol < 2.0f) && hasCollided) {																								// if < shell
				SubtractEvol(other.gameObject.GetComponentInParent<ParticleStateManager>(), 1.0f, 1.0f);										// subtract 2 evol, give 1 evol
			} 
			else if ((evol >= 2.0f) && (evol >= other.gameObject.GetComponentInParent<ParticleStateManager> ().evol) && hasCollided) {	// if any state >= other shell											
				AddEvol(other.gameObject.GetComponentInParent<ParticleStateManager>(), 1.0f, 2.0f);												// add 1 evol, take 2 evol
			}
		}
		else if (other.gameObject.CompareTag ("Shell2")) {																				// collide with shell
			StartCoroutine(PreventPlayerDamage(2.0f));																							// prevent player trigger collisions
			if ((evol < 3.0f) && hasCollided) {																								// if < shell2
				SubtractEvol(other.gameObject.GetComponentInParent<ParticleStateManager>(), 1.0f, 1.0f);										// subtract 2 evol, give 1 evol
			} 
			else if ((evol >= 3.0f) && (evol >= other.gameObject.GetComponentInParent<ParticleStateManager> ().evol) && hasCollided) {	// if any state >= other shell2											
				AddEvol(other.gameObject.GetComponentInParent<ParticleStateManager>(), 1.0f, 2.0f);												// add 1 evol, take 2
			}
		}
		else if (other.gameObject.CompareTag ("Atom")) {																				// collide with atom
			StartCoroutine(PreventPlayerDamage(2.0f));																							// prevent player trigger collisions
			if ((evol < 5.0f) && hasCollided) {																								// if < atom
				SubtractEvol(other.gameObject.GetComponentInParent<ParticleStateManager>(), 3.0f, 1.0f);										// subtract 3 evol, give 1 evol
			} 
			else if ((evol >= 5.0f) && (evol < other.gameObject.GetComponentInParent<ParticleStateManager> ().evol) && hasCollided) {		// if atom < other atom
				SubtractEvol(other.gameObject.GetComponentInParent<ParticleStateManager>(), 1.0f, 1.0f);										// subtract 1 evol, give 1 evol
			} 
			else if ((evol >= 5.0f) && (evol >= other.gameObject.GetComponentInParent<ParticleStateManager> ().evol) && hasCollided) {	// if any state >= other atom
				AddEvol(other.gameObject.GetComponentInParent<ParticleStateManager>(), 1.0f, 3.0f);												// add 1 evol, take 3 evol
			}
		}
	}

	// methods \\

		void SubtractEvol(ParticleStateManager other, float playerSubAmount, float otherAddAmount) {
			evol -= playerSubAmount;																							// remove evol level
			other.evol += otherAddAmount;																						// +1 evol to other shell
			bump = true;																										// collision bump
			hasCollided = false;																								// reset has collided trigger	
			Debug.Log ("player contact " + other.gameObject.tag + ": evol -" + playerSubAmount);
			Debug.Log ("evol: " + evol);
		}

		void AddEvol(ParticleStateManager other, float playerAddAmount, float otherSubAmount) {
			evol += playerAddAmount;																							// remove evol level
			other.evol -= otherSubAmount;																						// +1 evol to other shell
			hasCollided = false;																								// reset has collided trigger	
			Debug.Log ("player contact " + other.gameObject.tag + ": evol +" + playerAddAmount);
			Debug.Log ("evol: " + evol);
	}

// STATE CHANGES \\
	
	void SetCurrentState(PlayerState state) {
		currentState = state;																// change state
		lastStateChange = Time.time;														// reset time since last state change
		Debug.Log("set state: " + currentState);											// debug
	}

	// core \\

		// core scaling and colour change
		void Core (bool photon, bool white) {
			// core scaling
			if ((photon == true) && coreScale == true) {										// if photon and triggered
				// scale core down
				GetComponent<Animator> ().ResetTrigger ("scaleup");									// reset next stage
				GetComponent<Animator> ().SetTrigger("scaledown");									// enable core to black animation
				GetComponent<Animator>().SetBool("black", false);									// enable black core animation state	
				GetComponent<Animator>().SetBool("photon", true);									// enable black core animation state	
				coreColour = false;																	// reset core colour change trigger
			} 
			else if ((photon == false) && coreScale == true) {								// if electron and triggered
				// scale core up
				GetComponent<Animator> ().SetTrigger ("scaleup");									// trigger core to white animation
				GetComponent<Animator>().SetBool("photon", false);									// disable black core animation state, returning to idle
				coreColour = false;																	// reset core colour change trigger
			}
			
			// fade to colour
			if ((white == false) && coreColour == true) {										// if black and triggered
				// fade to black
				GetComponent<Animator> ().ResetTrigger ("fadein");									// reset next stage
				GetComponent<Animator> ().SetTrigger("fadeout");									// enable core to black animation
				GetComponent<Animator>().SetBool("black", true);									// enable black core animation state	
				coreColour = false;																	// reset core colour change trigger
			} 
			else if ((white == true) && coreColour == true) {									// if white and triggered
				// fade to white
				GetComponent<Animator> ().SetTrigger ("fadein");									// trigger core to white animation
				GetComponent<Animator>().SetBool("black", false);									// disable black core animation state, returning to idle
				coreColour = false;																	// reset core colour change trigger
			} 
		}
		
		// core colour change only
		void Core (bool white) {
		// fade to colour
		if ((white == false) && coreColour == true) {										// if black and triggered
			// fade to black
			GetComponent<Animator> ().ResetTrigger ("fadein");									// reset next stage
			GetComponent<Animator> ().SetTrigger("fadeout");									// enable core to black animation
			GetComponent<Animator>().SetBool("black", true);									// enable black core animation state
			coreColour = false;																	// reset core colour change trigger
		} else if ((white == true) && coreColour == true) {									// if white and triggered
			// fade to white
			GetComponent<Animator> ().SetTrigger ("fadein");									// trigger core to white animation
			GetComponent<Animator>().SetBool("black", false);									// disable black core animation state, returning to idle	
			coreColour = false;																	// reset core colour change trigger
		} 
	}

	// shell \\

		void Shell (bool grow) {
			
			if ((grow == true) && shellChange == true) {										// if grow and triggered
				StartCoroutine(PreventPlayerDamage(3.0f));												// prevent collisions for 3 sec
				shell.SetActive(true);																// activate shell
				shell.GetComponent<Animator>().SetTrigger("grow");									// enable shell grow animation
				shell.GetComponent<Animator>().SetBool("shell", true);								// enable shell grown animation state
				shell.GetComponent<SphereCollider> ().enabled = true;								// enable collider (enable here to prevent particles from entering shell to contact core electron)
				GetComponent<SphereCollider>().enabled = false;										// disable core collider
				shellChange = false;																// reset shell change trigger
			} else if ((grow == false) && shellChange == true) {								// if shrink and triggered
				StartCoroutine(PreventPlayerDamage(3.0f));												// prevent collisions for 3 sec
				shell.GetComponent<Animator> ().SetTrigger ("shrink");								// trigger shell shrink animation
				shell.GetComponent<Animator>().SetBool("shell", false);								// enable black core animation state
				StartCoroutine(DeactivateShell());													// deactivate shell after 1 sec (for animation)
				GetComponent<SphereCollider>().enabled = true;										// enable core collider
				shellChange = false;																	// reset shell change trigger
			}

		}

		IEnumerator DeactivateShell(){
		yield return new WaitForSeconds(1); 												// wait 1 second
		shell.SetActive(false);																// deactivate shell
	}

	// nucleus \\

		void Nucleus (bool white, bool enabled) {
			// nucleus enabled
			if ((enabled == true) && nucleusState == true) {									// if enabled and triggered
				// scale core down
				nucleus.SetActive(true);															// enable nucleus	
				nucleusState = false;																// reset nucleus state trigger
			}

			// fade to colour and deactivate
			if ((white == true) && (enabled == false) && nucleusState == true) {				// if deactivated and triggered
				// fade to white
				nucleus.GetComponent<Animator> ().ResetTrigger ("fadeblack");						// reset next stage
				nucleus.GetComponent<Animator> ().SetTrigger ("fadewhite");							// trigger nucleus to white animation
				nucleus.GetComponent<Animator>().SetBool("white", true);							// disable white nucleus animation state, returning to idle
				// deactivate
				StartCoroutine(DeactivateNucleus());												// deactivate nucleus after 1 sec (for animation)
				nucleusState = false;																// reset nucleus state trigger
			}
		}
		
		void Nucleus (bool white) {
			// fade to colour
			if ((white == true) && nucleusColour == true) {										// if white and triggered
				// fade to white
				nucleus.GetComponent<Animator> ().ResetTrigger ("fadeblack");						// reset next stage
				nucleus.GetComponent<Animator> ().SetTrigger ("fadewhite");							// trigger nucleus to white animation
				nucleus.GetComponent<Animator>().SetBool("white", true);							// disable white nucleus animation state, returning to idle
				nucleusColour = false;																// reset nucleus colour change trigger
			} else if ((white == false) && nucleusColour == true) {								// if black and triggered
				// fade to black
				nucleus.GetComponent<Animator> ().SetTrigger("fadeblack");							// enable nucleus to black animation
				nucleus.GetComponent<Animator>().SetBool("white", false);							// enable white nucleus animation state	
				nucleusColour = false;																// reset nucleus colour change trigger
			}
		}

		IEnumerator DeactivateNucleus() {
		yield return new WaitForSeconds(1); 													// wait 1 second
		nucleus.SetActive(false);																// deactivate shell
	}
	
// SPAWN PARTICLEs \\

	void SpawnParticle(int lostPart) {
		// sfx.PlayOneShot (Death, 1.0f);
		int i = 0;
		do {
			if (i == 0){
				GameObject particle = Instantiate																				// create new particle at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		

				particle.transform.parent = lostParticleParent.transform;														// sort new electron under 'Collectables'
				particle.GetComponent<FauxGravityBody> ().attractor = GetComponent<FauxGravityBody> ().attractor;				// set new electron FauxGravityAttractor as World

				particle.GetComponent<Animator>().SetBool("black", false);														// fade core to white: reset black condition
				particle.GetComponent<Animator>().SetTrigger("fadein");															// fade core to white

				if (lostPart >= 3) {																							// if losing 3 particles
					particle.GetComponent<ParticleStateManager>().initState = 
						particle.GetComponent<ParticleStateManager>().initState = ParticleStateManager.ParticleState.Electron;		// init electron (2x photon)
					i+=1;																											// spawn 1 less photon
				}
				else {																											// else
					particle.GetComponent<Animator>().SetBool("photon", true);														// photon: set photon condition
					particle.GetComponent<Animator>().SetTrigger("scaledown");														// scale to photon
					particle.GetComponent<ParticleStateManager>().initState = 
						particle.GetComponent<ParticleStateManager>().initState = ParticleStateManager.ParticleState.Electron;		// init photon
				}

				// particle collisions prevented in particle start()

			} 
			else if (i == 1) {
				GameObject particle = Instantiate																				// create new electron at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(0.5f, 2.5f)), Quaternion.identity) as GameObject;		

				particle.transform.parent = lostParticleParent.transform;														// sort new electron under 'Collectables'
				particle.GetComponent<FauxGravityBody> ().attractor = GetComponent<FauxGravityBody> ().attractor;				// set new electron FauxGravityAttractor as World

				particle.GetComponent<Animator>().SetBool("black", false);														// fade core to white: reset black condition
				particle.GetComponent<Animator>().SetTrigger("fadein");															// fade core to white

				if (lostPart >= 3) {																							// if losing 3 particles
					particle.GetComponent<ParticleStateManager>().initState = 
						particle.GetComponent<ParticleStateManager>().initState = ParticleStateManager.ParticleState.Electron;		// init electron (2x photon)
					i+=1;																											// spawn 1 less photon
				}
				else {																											// else
					particle.GetComponent<Animator>().SetBool("photon", true);														// photon: set photon condition
					particle.GetComponent<Animator>().SetTrigger("scaledown");														// scale to photon
					particle.GetComponent<ParticleStateManager>().initState = 
						particle.GetComponent<ParticleStateManager>().initState = ParticleStateManager.ParticleState.Electron;		// init photon
				}

				// particle collisions prevented in particle start()
			} 
			else if (i == 2) {
				GameObject particle = Instantiate																		// create new electron at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(1.5f, -2.5f)), Quaternion.identity) as GameObject;		
				particle.transform.parent = lostParticleParent.transform;												// sort new electron under 'Collectables'
				particle.GetComponent<FauxGravityBody> ().attractor = GetComponent<FauxGravityBody> ().attractor;		// set new electron FauxGravityAttractor as World
				particle.GetComponent<Animator>().SetBool("black", false);												// fade core to white: reset black condition
				particle.GetComponent<Animator>().SetTrigger("fadein");													// fade core to white
				particle.GetComponent<Animator>().SetBool("photon", true);												// photon: set photon condition
				particle.GetComponent<Animator>().SetTrigger("scaledown");												// scale to photon
				particle.GetComponent<ParticleStateManager>().evol = 0;													// set evol to 0
			} 
			else {
				GameObject particle = Instantiate																		// create new electron at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, 2.5f)), Quaternion.identity) as GameObject;		
				particle.transform.parent = lostParticleParent.transform;												// sort new electron under 'Collectables'
				particle.GetComponent<FauxGravityBody> ().attractor = GetComponent<FauxGravityBody> ().attractor;		// set new electron FauxGravityAttractor as World
				particle.GetComponent<Animator>().SetBool("black", false);												// fade core to white: reset black condition
				particle.GetComponent<Animator>().SetTrigger("fadein");													// fade core to white
				particle.GetComponent<Animator>().SetBool("photon", true);												// photon: set photon condition
				particle.GetComponent<Animator>().SetTrigger("scaledown");												// scale to photon
				particle.GetComponent<ParticleStateManager>().evol = 0;													// set evol to 0
			}
			
			i+=1;																										// # of particle spawn count
			
			Debug.Log("player particle lost");																			// debug

		} while (i != lostPart);
	}

// PLAYER ABILITIES \\

	IEnumerator PreventPlayerDamage(float time) {
		Debug.Log ("Player collider off");
		GetComponent<SphereCollider> ().enabled = false;					// disable core collider
		shell.GetComponent<SphereCollider> ().enabled = false;				// disable shell collider
		yield return new WaitForSeconds(time); 								// wait 5 seconds
		GetComponent<SphereCollider> ().enabled = true;						// enable core collider
		shell.GetComponent<SphereCollider> ().enabled = true;				// enable shell collider
		Debug.Log ("Player collider on");
	}

// ENDGAME/DEATH \\

	IEnumerator Endgame() {
		// sfx.PlayOneShot (Death, 1.0f);
		deathFade.SetActive(true);											// fade screen to black

		yield return new WaitForSeconds(10.0f); 								// wait 5 seconds
		// wait for longer

		uI.gameObject.tag = "Destroy";										// flag old ui for destroy
		SceneManager.LoadScene("Sequence1");								// restart scene
	}

	IEnumerator PlayerDeath() {
		// sfx.PlayOneShot (Death, 1.0f);
		// dead = false;														// reset trigger to call this coroutine once
		//GetComponent<SphereCollider> ().enabled = false;					// disable core collider
		shell.GetComponent<Animator> ().SetTrigger("shrink");									// enable shell shrink animation
		shell.GetComponent<Animator> ().SetBool("shell", false);								// reset shell animation state
		GetComponent<Animator> ().SetTrigger("fadeout");									// enable core to black animation
		GetComponent<Animator>().SetBool("black", true);									// enable black core animation state
		GetComponent<Animator>().SetBool("dead", true);									// enable black core animation state
		//deathFade.GetComponent<Animator>().SetTrigger("black");
		deathFade.SetActive(true);

		// fade in death text
		// checks for OVERLAY TEXT (DEATH)
		overlayTextTrigger [5].SetActive (true);																	// activate text
		overlayTextTrigger [5].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text

		yield return new WaitForSeconds(7.0f); 								// wait 5 seconds
		// wait for longer

		uI.gameObject.tag = "Destroy";										// flag old ui for destroy
		SceneManager.LoadScene("Sequence1");								// restart scene
	}

// UI METHODS \\

	float GetStateElapsed() {
		return Time.time - lastStateChange;											// returns time spent in current state
	}

	float GetGameElapsed() {
		return Time.time - sinceGameBegin;											// returns time since player control
	}

	public void IsEndless(bool toggle) {
		endless = toggle;
	}

}