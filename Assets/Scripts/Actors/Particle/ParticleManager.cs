using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

	// movement/physics variables
	public float moveSpeed = 7;
	public float maxSpeed = 10;

	public float xMin = 0.5f, xMax = 1.0f, zMin = -1.0f, zMax = -0.5f;

	private Vector3 moveDir;
	private Rigidbody rb;
	[HideInInspector]
	public bool bump = false, hasCollided = false;
	public int bumpStrength = 100;

	// particle states
	public enum ParticleState {Dead, Photon, Electron, Electron2, Shell, Shell2, Atom, Atom2, StateEight, StateNine, StateTen}

	// particle state management
	public float evol = 0.0f;
	//public int nEvol = 0;
	public ParticleState initState;																							// init particle state
	public ParticleState currentState, previousState;																		// current particle state
	private bool isAtInit = true;																							// if particle was initialized at a particular state
																															// state change parameters
	private float /*deathThreshold, */photonThreshold, electronThreshold, electron2Threshold, shellThreshold, shell2Threshold, atomThreshold, atom2Threshold;	
	private float previousEvol = 0.0f, currentEvol = 0.0f;
	//private int previousNEvol, currentNEvol;
	private bool evolving = false, devolving = false, dead = false;

	private bool coreScale = true, coreColour = true;																		// core state checkers
	private bool shellChange = false;																	// shell state checkers
	private bool nucleusState = false, nucleusColour = true;																// nucleus state checkers


	// collision conflict resolution
	[HideInInspector]
	public int die;
	private bool rolling = false;

	// add-on gameobjects
	public GameObject nucleus, shell;
	public GameObject lostParticle;
	public Material particleMaterial, coreMaterial;

// INIT THINGS \\

	void Start () {
		// init components and properties
		rb = GetComponent<Rigidbody> ();

		xMin = Random.Range (-1.0f, 1.0f);												// randomize movement headings
		xMax = Random.Range (-1.0f, 1.0f);
		zMin = Random.Range (-1.0f, 1.0f);
		zMax = Random.Range (-1.0f, 1.0f);

		StartCoroutine (PreventParticleDamage (3));										// prevent damage for 3 sec at spawn

		// init thresholds
		//deathThreshold = -1;																// set death threshold
		photonThreshold = 0.0f;																// set photon threshold
		electronThreshold = 1.0f;																// set electron threshold
		electron2Threshold = 1.5f;																// set electron2 threshold
		shellThreshold = 2.0f;																	// set shell threshold
		shell2Threshold = 3.0f;																// set shell2 threshold
		atomThreshold = 5.0f;																	// set atom threshold
		atom2Threshold = 8.0f;																	// set atom2 threshold

		// init state
		if (initState == ParticleState.Photon) {
			evol = 0.0f;																	// evolution start
			//nEvol = 0;																			// set fn to 0n
			rb.mass = 0.25f;																	// set mass
		} else if (initState == ParticleState.Electron) {
			evol = 1.0f;
			//nEvol = 1;																			// set fn to 1n
			rb.mass = 0.5f;																		// set mass
		} else if (initState == ParticleState.Electron2) {
			evol = 1.5f;
			// nEvol = 2;																			// set fn to 2n
			rb.mass = 0.75f;																	// set mass
		} else if (initState == ParticleState.Shell) {
			evol = 2.0f;
			// nEvol = 3;																			// set fn to 3n
			rb.mass = 0.5f;																		// set mass
		} else if (initState == ParticleState.Shell2) {
			evol = 3.0f;
			// nEvol = 4;																			// set fn to 4n
			rb.mass = 0.75f;																	// set mass
		} else if (initState == ParticleState.Atom) {
			evol = 5.0f;
			// nEvol = 5;																			// set fn to 5n
			rb.mass = 1.0f;																		// set mass
		}

		// begin at init state
		SetCurrentState(initState);															
	}

// PHYSICS THINGS \\

	void FixedUpdate () {

		// movement
		//if (isAtInit) 
		moveDir = new Vector3 (Random.Range(xMin, xMax), 0.0f, Random.Range(zMin, zMax)).normalized;		// create move direction
		//else
			//moveDir.Set(rb.velocity.x, 0.0f, rb.velocity.z);		// create move direction
		
		rb.AddRelativeForce (moveDir * moveSpeed); 																// add force in movement direction

		if (rb.velocity.magnitude > maxSpeed) {																	// clamp at max speed
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
		}

		if (bump) {																								// bump away at collision
			rb.AddRelativeForce (Vector3.Slerp(rb.velocity, (rb.velocity * -bumpStrength), 1.0f));				// slerp force of velocity * a factor in the opposite direction
			//Debug.Log ("particle collision bump");
			bump = false;																						// reset collision trigger
		}
	}

// STATE MACHINE \\

	void Update() {

		currentEvol = evol;																		// set current evol (evol direction check)
		//currentEvol = evol;																		// set current evol (evol direction check)

		//currentFrameState = currentState;														// set current state this frame

	// switch start \\

		switch (currentState) {

		// dead state (evol -1 | f-1) \\

		case ParticleState.Dead:
			if (dead) {																			// if dead
				ParticleDeath();																	// death 
				dead = false;																		// reset death trigger
				//Debug.Log("particle dead");
			}
			break;

		// photon state (evol 0 | f0) \\

		case ParticleState.Photon:

			// checks for INIT STATE
			if ((evol == photonThreshold) && isAtInit) {																							// if init
				gameObject.tag = "Photon";																				// set gameobject tag
				CoreToPhoton();																							// scale core to photon
				isAtInit = false;																						// end init
			} 

			// checks for INCOMING TRIGGERS (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if ((currentEvol < electronThreshold) && (previousState == ParticleState.Shell2) && devolving) {			// if FROM SHELL2 and devolving
				// nEvol = 0;																									// set fn to 0n
				gameObject.tag = "Photon";																					// set gameobject tag
				rb.mass = 0.2f;																								// set mass
				CoreToWhite();																								// CORE: return to idle white
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				ShellShrink ();																								// SHELL: shrink
				NucleusDisable (); 																							// NUCLEUS: fade to white, then deactivate
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electronThreshold) && (previousState == ParticleState.Shell) && devolving) {		// if FROM SHELL and devolving
				// nEvol = 0;																									// set fn to 0
				gameObject.tag = "Photon";																					// set gameobject tag
				rb.mass = 0.2f;																								// set mass
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				ShellShrink ();																								// SHELL: shrink
				NucleusDisable ();																							// NUCLEUS: fade to white, then deactivate
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electronThreshold) && (previousState == ParticleState.Electron2) && devolving) {	// if FROM ELECTRON2 and devolving
				// nEvol = 0;																									// set fn to 0
				gameObject.tag = "Photon";																					// set gameobject tag
				rb.mass = 0.2f;																								// set mass
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				NucleusDisable ();																							// NUCLEUS: fade to white, then deactivate
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electronThreshold) && (previousState == ParticleState.Electron) && devolving) {		// if FROM ELECTRON and devolving
				// nEvol = 0;																									// set fn to 0
				gameObject.tag = "Photon";																					// set gameobject tag
				rb.mass = 0.2f;																								// set mass
				CoreToPhoton ();																							// CORE: shrink to photon size, fade to white
				devolving = false;																							// reset devolving trigger
			} 

			// checks for OUTGOING TRIGGERS (STATE CHANGES)
			if ((previousEvol < electronThreshold) && (currentEvol < photonThreshold)) {							// if electron -> dead
				dead = true;																							// dead trigger
				SetCurrentState (ParticleState.Dead);																	// kill player
			} 
			else if (currentEvol >= electronThreshold) {															// if evol electron
				evolving = true;																						// evolving trigger
				previousEvol = currentEvol;																				// previous evol value last frame
				// previousNEvol = currentNEvol;																			// previous // nEvol value last frame
				SetCurrentState (ParticleState.Electron);																// evolve to shell state
			}

			previousState = ParticleState.Photon;																	// set state last frame

			break;

		// electron state (evol 1 | f1) \\

		case ParticleState.Electron:

			// checks for INIT STATE
			if ((evol == electronThreshold) && isAtInit) {																							// if init
				gameObject.tag = "Electron";																			// set gameobject tag
				isAtInit = false;																						// end init
			} 

			// checks on INCOMING TRIGGERS (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if ((currentEvol >= electronThreshold) && evolving) {														// if FROM PHOTON and evolving
				// nEvol += 1;																									// increase fn
				gameObject.tag = "Electron";																				// set gameobject tag
				rb.mass = 0.5f;																								// set mass
				CoreToElectron ();																							// CORE: grow to electron size, is white
				evolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == ParticleState.Atom2) && devolving) {		// if FROM ATOM2 and devolving
				// nEvol -= 5;																									// set fn to 0n
				gameObject.tag = "Electron";																				// set gameobject tag
				rb.mass = 0.5f;																								// set mass
				ShellShrink ();																								// SHELL: shrink
				NucleusToWhite ();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == ParticleState.Atom) && devolving) {		// if FROM ATOM and devolving
				// nEvol -= 4;																									// set fn to 0n
				gameObject.tag = "Electron";																				// set gameobject tag
				rb.mass = 0.5f;																								// set mass
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == ParticleState.Shell2) && devolving) {		// if FROM SHELL2 and devolving
				// nEvol -= 3;																									// set fn to 0n
				gameObject.tag = "Electron";																				// set gameobject tag
				rb.mass = 0.5f;																								// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == ParticleState.Shell) && devolving) {		// if FROM SHELL and devolving
				// nEvol -= 2;																									// set fn to 0
				gameObject.tag = "Electron";																				// set gameobject tag
				rb.mass = 0.5f;																								// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				NucleusToWhite ();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < electron2Threshold) && (previousState == ParticleState.Electron2) && devolving) {	// if FROM ELECTRON2 and devolving
				// nEvol -= 1;																									// decrease fn
				gameObject.tag = "Electron";																				// set gameobject tag
				rb.mass = 0.5f;																								// set mass
				NucleusToWhite ();																							// NUCLEUS: fade out to white
				devolving = false;																							// reset devolving trigger
			} 

			// checks on OUTGOING TRIGGERS (METHODS & TRIGGERS)
			if ((previousEvol < electron2Threshold) && (currentEvol < photonThreshold)) {							// if evol electron -> dead
				SpawnParticle (1);																						// spawn 1 particle
				dead = true;																							// dead trigger
				SetCurrentState (ParticleState.Dead);																	// kill player
			} 
			else if ((previousEvol < electron2Threshold) && (currentEvol < electronThreshold)) {					// if evol electron -> photon
				SpawnParticle (1);																						// spawn 1 particle
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Photon);																	// devolve to photon
			} 
			else if (evol >= electron2Threshold) {																// if evol electron2
				evolving = true;																						// evolving trigger
				previousEvol = currentEvol;																				// previous evol value last frame
				// previousNEvol = currentNEvol;																			// previous // nEvol value last frame
				SetCurrentState (ParticleState.Electron2);																// evolve to electron2 state
			}

			previousState = ParticleState.Electron;																// set state last frame

			break;

		// electron2 state (evol 1 | f2) \\

		case ParticleState.Electron2:

			// checks for INIT STATE
			if ((evol == electron2Threshold) && isAtInit) {																							// if init
				gameObject.tag = "Electron2";																			// set gameobject tag
				NucleusEnable();																						// NUCLEUS: enable, fade in to black
				isAtInit = false;																						// end init
			} 

			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if ((currentEvol >= electron2Threshold) && evolving) {														// if FROM ELECTRON and evolving
				// nEvol += 1;																									// increase fn
				gameObject.tag = "Electron2";																				// set gameobject tag
				rb.mass = 0.75f;																							// set mass
				NucleusEnable ();																							// NUCLEUS: enable, fade in to black
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == ParticleState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				// nEvol -= 4;																									// decrease fn
				gameObject.tag = "Electron2";																				// set gameobject tag
				rb.mass = 0.75f;																							// set mass
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == ParticleState.Atom) && devolving) {			// if FROM ATOM and devolving
				// nEvol -= 3;																									// decrease fn
				gameObject.tag = "Electron2";																				// set gameobject tag
				rb.mass = 0.75f;																							// set mass
				ShellShrink ();																								// SHELL: shrink
				NucleusToBlack ();																							// NUCLEUS: fade to black
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == ParticleState.Shell2) && devolving) {			// if FROM SHELL2 and devolving
				// nEvol -= 2;																									// decrease fn
				gameObject.tag = "Electron2";																				// set gameobject tag
				rb.mass = 0.75f;																							// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				NucleusToBlack ();																							// NUCLEUS: fade to black
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < shellThreshold) && (previousState == ParticleState.Shell) && devolving) {			// if FROM SHELL and devolving
				// nEvol -= 1;																									// decrease fn
				gameObject.tag = "Electron2";																				// set gameobject tag
				rb.mass = 0.75f;																							// set mass
				CoreToWhite ();																								// CORE: fade to white
				ShellShrink ();																								// SHELL: shrink
				devolving = false;																							// reset devolving trigger
			}   		

			// checks on OUTGOING METHODS & TRIGGERS
			if ((previousEvol < shellThreshold) && (currentEvol < photonThreshold)) {								// if evol electron2 -> dead
				SpawnParticle (3);																						// spawn 3 particles
				dead = true;																							// dead trigger
				SetCurrentState (ParticleState.Dead);																	// kill player
			} 
			else if ((previousEvol < shellThreshold) && (currentEvol < electronThreshold)) {						// if evol electron2 -> photon
				SpawnParticle (2);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Photon);																	// devolve to photon
			} 
			else if ((previousEvol < shellThreshold) && (currentEvol < electron2Threshold)) {						// if evol electron2 -> electron
				SpawnParticle (1);																						// spawn 1 particle
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Electron);																// devolve to electron
			} 
			else if (evol >= shellThreshold) {																	// if evol shell
				evolving = true;																						// evolving trigger
				previousEvol = currentEvol;																				// previous evol value last frame
				// previousNEvol = currentNEvol;																			// previous // nEvol value last frame
				SetCurrentState (ParticleState.Shell);																	// evolve to shell state
			}

			previousState = ParticleState.Electron2;															// set state last frame

			break;

		// shell state (evol 2 | f3) \\

		case ParticleState.Shell:

			// checks for INIT STATE
			if ((evol == shellThreshold) && isAtInit) {																// if init @ evol 2 
				gameObject.tag = "Shell";																				// set gameobject tag
				CoreToBlack();																							// CORE: fade to black								
				ShellGrow();																							// SHELL: grow
				NucleusEnable();																						// NUCLEUS: enable, fade in to black
				isAtInit = false;																						// end init
			} 

			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if (currentEvol >= shellThreshold && evolving) {															// if FROM ELECTRON2 and evolving
				// nEvol += 1;																									// increase fn
				gameObject.tag = "Shell";																					// set gameobject tag
				rb.mass = 0.5f;																								// set mass
				CoreToBlack();																								// CORE: fade to black
				ShellGrow ();																								// SHELL: grow
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < shell2Threshold) && (previousState == ParticleState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				// nEvol -= 3;																									// decrease fn
				gameObject.tag = "Shell";																					// set gameobject tag
				shell.gameObject.tag = "Shell";																				// set shell gameobject tag
				rb.mass = 0.5f;																								// set mass
				CoreToBlack();																								// CORE: fade to black
				devolving = true;																							// devolving trigger
			} 
			else if ((currentEvol < shell2Threshold) && (previousState == ParticleState.Atom) && devolving) {			// if FROM ATOM and devolving
				// nEvol -= 2;																									// decrease fn
				gameObject.tag = "Shell";																					// set gameobject tag
				shell.gameObject.tag = "Shell";																				// set shell gameobject tag
				rb.mass = 0.5f;																								// set mass
				CoreToBlack();																								// CORE: fade to black
				NucleusToBlack();																							// NUCLEUS: fade to black
				devolving = true;																							// devolving trigger
			} 
			else if ((currentEvol < shell2Threshold) && (previousState == ParticleState.Shell2) && devolving) {			// if FROM SHELL2 and devolving
				// nEvol -= 1;																									// decrease fn
				gameObject.tag = "Shell";																					// set gameobject tag
				shell.gameObject.tag = "Shell";																				// set shell gameobject tag
				rb.mass = 0.5f;																								// set mass
				NucleusToBlack();																							// NUCLEUS: fade to black
				devolving = false;																							// reset devolving trigger
			}

			// checks for OUTGOING (STATE CHANGES)
			if ((previousEvol < shell2Threshold) && (currentEvol < photonThreshold)) {								// if evol shell -> dead
				SpawnParticle(4);																						// spawn 4 particles 
				dead = true;																							// dead trigger
				SetCurrentState (ParticleState.Dead);																	// dead
			} 
			else if ((previousEvol < shell2Threshold) && (currentEvol < electronThreshold)) {						// if evol shell -> photon
				SpawnParticle(3);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Photon);																	// devolve to photon state
			} 
			else if ((previousEvol < shell2Threshold) && (currentEvol < electron2Threshold)) {						// if evol shell -> electron
				SpawnParticle(2);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState(ParticleState.Electron);																// devolve to electron state
			} 
			else if ((previousEvol < shell2Threshold) && (currentEvol < shellThreshold)) {							// if evol shell -> electron2
				SpawnParticle(3);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Electron2);																// devolve to electron2 state
			} 
			else if (currentEvol >= shell2Threshold) {																// if evol shell2
				evolving = true;																						// evolving trigger
				previousEvol = currentEvol;																				// previous evol value last frame
				// previousNEvol = currentNEvol;																			// previous // nEvol value last frame
				SetCurrentState (ParticleState.Shell2);																	// evolve to shell2 state
			}

			previousState = ParticleState.Shell;																	// set state last frame

			break;

		// shell2 state (evol 3 | f4) \\

		case ParticleState.Shell2:												
			// checks for INIT STATE
			if ((evol == shell2Threshold) && isAtInit) {															// if init @ evol 2 
				gameObject.tag = "Shell2";																				// set gameobject tag
				shell.gameObject.tag = "Shell2";																		// set shell gameobject tag
				CoreToBlack();																							// CORE: fade to black
				ShellGrow();																							// SHELL: grow
				NucleusEnable();																						// NUCLEUS: enable, fade in to black
				NucleusToWhite ();																						// NUCLEUS: fade to white
				isAtInit = false;																						// end init
			} 

			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if (currentEvol >= shell2Threshold && evolving) {															// if FROM SHELL and evolving
				// nEvol += 1;																									// increase fn
				gameObject.tag = "Shell2";																					// set gameobject tag
				shell.gameObject.tag = "Shell2";																			// set shell gameobject tag
				rb.mass = 0.75f;																							// set mass
				NucleusToWhite();																							// NUCLEUS: fade to white
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < atomThreshold) && (previousState == ParticleState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				// nEvol -= 2;																									// decrease fn
				gameObject.tag = "Shell2";																					// set gameobject tag
				shell.gameObject.tag = "Shell2";																			// set shell gameobject tag
				rb.mass = 0.75f;																							// set mass
				CoreToBlack();																								// CORE: fade to black
				NucleusToWhite();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			} 
			else if ((currentEvol < atomThreshold) && (previousState == ParticleState.Atom) && devolving) {				// if FROM ATOM and devolving
				// nEvol -= 1;																									// decrease fn
				gameObject.tag = "Shell2";																					// set gameobject tag
				shell.gameObject.tag = "Shell2";																			// set shell gameobject tag
				rb.mass = 0.75f;																							// set mass
				CoreToBlack();																								// CORE: fade to black
				devolving = false;																							// reset devolving trigger
			}

			// checks for STATE CHANGES
			if ((previousEvol < atomThreshold) && (currentEvol < photonThreshold)) {								// if evol shell2 -> dead
				SpawnParticle(4);																						// spawn 4 particles
				
				dead = true;																							// dead trigger
				SetCurrentState (ParticleState.Dead);																	// dead
			} 
			else if (previousEvol < atomThreshold && currentEvol < electronThreshold) {							// if evol shell2 -> photon
				SpawnParticle(4);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Photon);																	// devolve to photon state
			} 
			else if (previousEvol < atomThreshold && currentEvol < electron2Threshold) {							// if evol shell2 -> electron
				SpawnParticle(3);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState(ParticleState.Electron);																// devolve to electron state
			} 
			else if (previousEvol < atomThreshold && currentEvol < shellThreshold) {								// if evol shell2 -> electron2
				SpawnParticle(2);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Electron2);																// devolve to electron2 state
			} 
			else if (previousEvol < atomThreshold && currentEvol < shell2Threshold) {								// if evol shell2 -> shell
				SpawnParticle(1);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Shell);																	// devolve to shell state
			} 
			else if (currentEvol >= atomThreshold) {																// if evolatom
				evolving = true;																						// evolving trigger 
				previousEvol = currentEvol;																				// previous evol value last frame
				// previousNEvol = currentNEvol;																			// previous // nEvol value last frame
				SetCurrentState (ParticleState.Atom);																	// evolve to atom state
			}

			previousState = ParticleState.Shell2;																// set state last frame

			break;

		// atom state (evol 5 | f5) \\
		
		case ParticleState.Atom:
			//Debug.Log ("atom state");

			if (evol == atomThreshold && isAtInit == true) {																	// if init @ evol 4
				gameObject.tag = "Atom";																				// set gameobject tag
				shell.gameObject.tag = "Atom";																		// set shell gameobject tag
				ShellGrow ();																							// grow shell
				NucleusEnable();																						// NUCLEUS: enable, fade in to black
				NucleusToWhite ();																						// NUCLEUS: fade to white
				isAtInit = false;																						// end init
			} 

			// checks for INCOMING (ANIMATIONS & STATE TRANSITION INTERACTIONS)
			if ((currentEvol >= atomThreshold) && evolving) {															// if FROM SHELL2 and evolving
				// nEvol += 1;																									// increase fn
				gameObject.tag = "Atom";																					// set gameobject tag
				shell.gameObject.tag = "Atom";																			// set shell gameobject tag
				rb.mass = 1.0f;																								// set mass
				CoreToWhite();																								// CORE: fade to white
				evolving = false;																							// reset evolving trigger
			} 
			else if ((currentEvol < atom2Threshold)  && (previousState == ParticleState.Atom2) && devolving) {			// if FROM ATOM2 and devolving
				// nEvol -= 1;																									// decrease fn
				gameObject.tag = "Atom";																					// set gameobject tag
				shell.gameObject.tag = "Atom";																			// set shell gameobject tag
				rb.mass = 1.0f;																								// set mass
				NucleusToWhite();																							// NUCLEUS: fade to white
				devolving = false;																							// reset devolving trigger
			}

			// checks for STATE CHANGES
			if ((previousEvol < atom2Threshold) && (currentEvol < photonThreshold)) {								// if evol atom -> dead
				SpawnParticle(4);																						// spawn 4 particles
				
				dead = true;																							// dead trigger
				SetCurrentState (ParticleState.Dead);																	// dead
			} 
			else if (previousEvol < atom2Threshold && currentEvol < electronThreshold) {							// if evol atom -> photon
				SpawnParticle(4);																						// spawn 3 particles 
				
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Photon);																	// devolve to photon state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < electron2Threshold) {							// if evol atom -> electron
				SpawnParticle(4);																						// spawn 2 particles
				devolving = true;																						// devolving trigger
				SetCurrentState(ParticleState.Electron);																// devolve to electron state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < shellThreshold) {								// if evol atom -> electron2
				SpawnParticle(3);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Electron2);																// devolve to electron2 state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < shell2Threshold) {							// if evol atom -> shell
				SpawnParticle(2);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Shell);																	// devolve to shell state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < atomThreshold) {								// if evol atom -> shell
				SpawnParticle(1);																						// spawn 3 particles 
				devolving = true;																						// devolving trigger
				SetCurrentState (ParticleState.Shell2);																	// devolve to shell2 state
			} 
			else if (previousEvol < atom2Threshold && currentEvol < atom2Threshold) {								// if same state
				int deltaEvol = Mathf.FloorToInt(previousEvol - currentEvol);											// check delta evol, rounding float down
				if (deltaEvol > 0)																					// if delta > 0 (losing evol within state)
					SpawnParticle(deltaEvol);																			// spawn delta particles 
			}
			else if (currentEvol >= atomThreshold) {																// if evol atom2
				evolving = true;																						// evolving trigger
				previousEvol = currentEvol;																				// previous evol value last frame
				// previousNEvol = currentNEvol;																			// previous // nEvol value last frame
				SetCurrentState (ParticleState.Atom2);																	// evolve to atom2 state
			}

			previousState = ParticleState.Atom;																	// set state last frame

			break;
		}

		//previousEvol = currentEvol;																// previous evol value last frame
		//previousNEvol = currentNEvol;															// previous // nEvol value last frame

		//previousFrameState = currentState;															// previous state last frame

	}

	// trigger methods \\

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

		hasCollided = true;																														// has collided trigger
		rolling = true;																															// roll die trigger

		if (other.gameObject.CompareTag ("Player") && hasCollided) {																			// colide with player
			StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
			bump = true;																															// collision bump trigger
			hasCollided = false;																													// reset has collided trigger	
			Debug.Log ("particle contact player");
		} 
		else if (other.gameObject.CompareTag ("Photon")) {																						// collide with photon
			if ((evol == other.gameObject.GetComponent<ParticleManager> ().evol) && rolling && hasCollided) {									// if = other photon
				Debug.Log("photon/photon");
				StartCoroutine(PreventParticleDamage (3));																								// prevent particle trigger collisions
				other.gameObject.GetComponent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				RollDie(other.gameObject.GetComponent<ParticleManager> (), 0.5f, 1.0f);															// roll die (win 0.5, lose 1)
			} 
			else if ((evol > other.gameObject.GetComponent<ParticleManager> ().evol) && hasCollided) {										// if > other photon
				StartCoroutine(PreventParticleDamage (3));																								// prevent particle trigger collisions
				other.gameObject.GetComponent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				AddEvol(0.5f);																															// add 0.5 evol
				other.gameObject.GetComponent<ParticleManager> ().SubtractEvol(1.0f);																// subtract 1 evol from other
			}
		} 
		else if (other.gameObject.CompareTag ("Electron")) {																					// collide with electron
			if ((evol == other.gameObject.GetComponent<ParticleManager> ().evol) && rolling && hasCollided) {									// if = other electron
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				other.gameObject.GetComponent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				RollDie(other.gameObject.GetComponent<ParticleManager> (), 1.0f, 1.0f);															// roll die (win 1, lose 1)
			}
			else if ((evol > other.gameObject.GetComponent<ParticleManager> ().evol) && hasCollided) {											// if > other electron 
				StartCoroutine(PreventParticleDamage (3));																								// prevent particle trigger collisions
				other.gameObject.GetComponent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				AddEvol(1.0f);																															// add 1 evol
				other.gameObject.GetComponent<ParticleManager> ().SubtractEvol(1.0f);																// subtract 1 evol from other
			}
			/*else if ((evol < other.gameObject.GetComponent<ParticleManager> ().evol) && hasCollided) {											// if < other electron, or photon
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				//SetCurrentState(ParticleState.Dead);
				//evol -= -1.0f;
				SubtractEvol(1.0f);																														// subtract 1 evol
			}*/
		} 
		else if (other.gameObject.CompareTag ("Electron2")) {																					// collide with electron2
			if ((evol == other.gameObject.GetComponent<ParticleManager> ().evol) && rolling && hasCollided) {									// if = other electron2
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				other.gameObject.GetComponent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				RollDie(other.gameObject.GetComponent<ParticleManager> (), 1.0f, 1.0f);															// roll die (win 1, lose 1)
			} 
			else if ((evol > other.gameObject.GetComponent<ParticleManager> ().evol) && hasCollided) {											// if > other electron2
				StartCoroutine(PreventParticleDamage (3));																								// prevent particle trigger collisions
				other.gameObject.GetComponent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				AddEvol(1.0f);																																// add 1 evol
				other.gameObject.GetComponent<ParticleManager> ().SubtractEvol(1.0f);																// subtract 1 evol from other
			}
			/*else if ((evol < other.gameObject.GetComponent<ParticleManager> ().evol) && hasCollided) {											// if < other electron2
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				SubtractEvol(1.0f);																														// subtract 1 evol
				//evol = -1.0f;
			}*/
		}
		else if (other.gameObject.CompareTag ("Shell")) {																						// collide with shell
			if ((evol == other.gameObject.GetComponentInParent<ParticleManager> ().evol) && rolling && hasCollided) {							// if = other shell
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				other.gameObject.GetComponentInParent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				RollDie(other.gameObject.GetComponentInParent<ParticleManager> (), 1.0f, 2.0f);													// roll die	(win 1, lose 2)																											// subtract evol
			} 
			else if ((evol > other.gameObject.GetComponentInParent<ParticleManager> ().evol) && hasCollided) {									// if > other shell
				StartCoroutine(PreventParticleDamage (3));																								// prevent particle trigger collisions
				other.gameObject.GetComponentInParent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				AddEvol(1.0f);																															// add 1 evol
				other.gameObject.GetComponentInParent<ParticleManager> ().SubtractEvol(2.0f);																// subtract 2 evol from other
			} 
			/*else if ((evol < 2.0f) && hasCollided) {																								// if < other shell
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				SubtractEvol(2.0f);																														// subtract 2 evol
			}*/
		}
		else if (other.gameObject.CompareTag ("Shell2")) {																						// collide with shell
			if ((evol == other.gameObject.GetComponentInParent<ParticleManager> ().evol) && rolling && hasCollided) {							// if = other shell2
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				other.gameObject.GetComponentInParent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				RollDie(other.gameObject.GetComponentInParent<ParticleManager> (), 1.0f, 2.0f);													// roll die	(win 1, lose 2)																											// subtract evol
			} 
			else if ((evol > other.gameObject.GetComponentInParent<ParticleManager> ().evol) && hasCollided) {									// if > other shell2
				StartCoroutine(PreventParticleDamage (3));																								// prevent particle trigger collisions
				other.gameObject.GetComponentInParent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				AddEvol(1.0f);																															// add 1 evol
				other.gameObject.GetComponentInParent<ParticleManager> ().SubtractEvol(2.0f);																// subtract 2 evol from other
			} 
			/*else if ((evol < 3.0f) && hasCollided) {																								// if < other shell2
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				SubtractEvol(2.0f);																														// subtract 2 evol
			}*/
		}
		else if (other.gameObject.CompareTag ("Atom")) {																						// collide with atom
			if ((evol == other.gameObject.GetComponentInParent<ParticleManager> ().evol) && rolling && hasCollided) { 							// if an atom of equal evol value
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				other.gameObject.GetComponentInParent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				RollDie(other.gameObject.GetComponentInParent<ParticleManager> (), 1.0f, 3.0f);													// roll die (win 1, lose 3)
			}
			else if ((evol > other.gameObject.GetComponentInParent<ParticleManager> ().evol) && hasCollided)	{								// if > other atom
				StartCoroutine(PreventParticleDamage (3));																								// prevent particle trigger collisions
				other.gameObject.GetComponentInParent<ParticleManager> ().StartCoroutine(PreventParticleDamage (3));										// prevent other particle trigger collisions
				AddEvol(1.0f);																															// add 1 evol
				if (other.gameObject.GetComponentInParent<ParticleManager> ().evol < 5.0f) 														// if other < atom
					other.gameObject.GetComponentInParent<ParticleManager> ().SubtractEvol(3.0f);																// subtract 3 evol from other
				else if (other.gameObject.GetComponentInParent<ParticleManager> ().evol < evol) 														// if other atom < this atom
					other.gameObject.GetComponentInParent<ParticleManager> ().SubtractEvol(2.0f);																// subtract 2 evol from other
			}
			/*else if ((evol < 5.0f) && hasCollided) {																								// if < other atom
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				SubtractEvol(3.0f);																														// subtract 3 evol
			}
			else if ((evol >= 5.0f) && (evol < other.gameObject.GetComponentInParent<ParticleManager> ().evol) && hasCollided)	{				// if atom < other atom
				StartCoroutine(PreventParticleDamage(3));																								// disable collider for 3 sec
				SubtractEvol(2.0f);																														// subtract 2 evol
			}*/
		}
	}

	// methods \\

		void RollDie(ParticleManager other, float addAmount, float subAmount) {
			do {
				die = Random.Range(1,6);														// roll die
				if (die > other.die) {															// if this die > other die
					AddEvol(addAmount);																// add evol level
					//Debug.Log("photon subtract");
					other.SubtractEvol(subAmount);													// other: remove evol level
					other.bump = true;																// other: collision bump
					rolling = false;																// exit roll
				}
			} while (rolling);																	// reroll if same die
		}

		void SubtractEvol(float changeAmount) {
			evol -= changeAmount;																							// add evol level
			//Debug.Log("photon -1");	
			//evol = -evol;																									// convert evol sign
			bump = true;																										// collision bump
			hasCollided = false;																								// reset has collided trigger	
		}

		void AddEvol(float changeAmount) {
			evol += changeAmount;																							// remove evol level
			hasCollided = false;																								// reset has collided trigger	
		}

// STATE CHANGES \\

	void SetCurrentState(ParticleState state) {
		currentState = state;														// change state
		//Debug.Log("particle set state: " + currentState);									// debug
	}

	// core \\

		// core scaling and colour change
		void Core (bool photon, bool white) {
		// core scaling
		if ((photon == true) && coreScale == true) {										// if photon and triggered
			// scale core down
			GetComponent<Animator> ().ResetTrigger ("scaleup");									// reset next stage
			GetComponent<Animator> ().SetTrigger("scaledown");									// enable core to black animation
			GetComponent<Animator>().SetBool("photon", true);									// enable black core animation state	
			coreColour = false;																	// reset core colour change trigger
		} 
		else if ((photon == false) && coreScale == true) {								// if electron and triggered
			// scale core up
			GetComponent<Animator>().SetTrigger ("scaleup");									// trigger core to white animation
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
		} 
		else if ((white == true) && coreColour == true) {									// if white and triggered
			// fade to white
			GetComponent<Animator> ().SetTrigger ("fadein");									// trigger core to white animation
			GetComponent<Animator>().SetBool("black", false);									// disable black core animation state, returning to idle	
			coreColour = false;																	// reset core colour change trigger
		} 
	}

	// shell \\

		void Shell (bool grow) {
			if ((grow == true) && shellChange == true) {										// if grow and triggered
				StartCoroutine(PreventParticleDamage(3));											// prevent collisions for 3 sec
				shell.SetActive(true);																// activate shell
				shell.GetComponent<Animator>().SetTrigger("grow");									// enable shell grow animation
				shell.GetComponent<Animator>().SetBool("shell", true);								// enable shell grown animation state
				shell.GetComponent<SphereCollider> ().enabled = true;								// enable collider (enable here to prevent particles from entering shell to contact core electron)
				GetComponent<SphereCollider>().enabled = false;										// disable core collider
				shellChange = false;																// reset shell change trigger
			} 
			else if ((grow == false) && shellChange == true) {								// if shrink and triggered
				StartCoroutine(PreventParticleDamage(3));												// prevent collisions for 3 sec
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
			} 
			else if ((white == false) && nucleusColour == true) {								// if black and triggered
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

// SPAWN PARTICLES \\

	void SpawnParticle(int lostPart) {
		// sfx.PlayOneShot (Death, 1.0f);
		int i = 0;
		do {
			if (i == 0){
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		

				particle.transform.parent = gameObject.transform.parent.transform;											// sort new particle under 'Collectables'
				particle.GetComponent<ParticlePhysicsManager> ().attractor = gameObject.GetComponent<ParticlePhysicsManager>().attractor;	// manually set new particle FauxGravityAttractor as World
				particle.GetComponent<Animator>().SetBool("black", false);													// set fade trigger
				particle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white

				if (lostPart >= 3) {																						// if losing 3 particles
					particle.GetComponent<ParticleManager>().initState = ParticleState.Electron;							// init electron (2x photon)
					i+=1;																										// spawn 1 less photon
				}
				else {
					particle.GetComponent<Animator>().SetBool("photon", true);													// photon: set photon condition
					particle.GetComponent<Animator>().SetTrigger("scaledown");													// scale to photon
					particle.GetComponent<ParticleManager>().initState = ParticleState.Photon;								// init photon
				}

				StartCoroutine(PreventParticleDamage(3));																	// prevent particle trigger collisions for 3 sec
			}
			else if (i == 1){
				GameObject particle = Instantiate																			// create new electron
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(0.5f, 2.5f)), Quaternion.identity) as GameObject;		

				particle.transform.parent = gameObject.transform.parent.transform;											// sort new electron under 'Collectables'
				particle.GetComponent<ParticlePhysicsManager> ().attractor = gameObject.GetComponent<ParticlePhysicsManager>().attractor;	// manually set new electron FauxGravityAttractor as World
				particle.GetComponent<Animator>().SetBool("black", false);													// set fade trigger
				particle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white

				if (lostPart >= 3) {																						// if losing 3 particles
					particle.GetComponent<ParticleManager>().initState = ParticleState.Electron;							// init electron (2x photon)
					i+=1;																										// spawn 1 less photon
				}
				else {
					particle.GetComponent<Animator>().SetBool("photon", true);													// photon: set photon condition
					particle.GetComponent<Animator>().SetTrigger("scaledown");													// scale to photon
					particle.GetComponent<ParticleManager>().initState = ParticleState.Photon;								// init photon
				}

				particle.GetComponent<ParticleManager>().initState = ParticleState.Photon;								// set particle to evol 0
				StartCoroutine(PreventParticleDamage(3));																	// prevent particle collisions for 3 sec
			}
			else if (i == 2){
				GameObject particle = Instantiate																			// create new electron
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(1.5f, -2.5f)), Quaternion.identity) as GameObject;		

				particle.transform.parent = gameObject.transform.parent.transform;											// sort new electron under 'Collectables'
				particle.GetComponent<ParticlePhysicsManager> ().attractor = gameObject.GetComponent<ParticlePhysicsManager>().attractor;	// manually set new electron FauxGravityAttractor as World
				particle.GetComponent<Animator>().SetBool("black", false);													// set fade trigger
				particle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white

				particle.GetComponent<Animator>().SetBool("photon", true);													// photon: set photon condition
				particle.GetComponent<Animator>().SetTrigger("scaledown");													// scale to photon
				particle.GetComponent<ParticleManager>().initState = ParticleState.Photon;								// set particle to evol 0

				StartCoroutine(PreventParticleDamage(3));																	// prevent particle collisions for 3 sec
			}
			else {
				GameObject particle = Instantiate																			// create new electron
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, 2.5f)), Quaternion.identity) as GameObject;		
				particle.transform.parent = gameObject.transform.parent.transform;											// sort new electron under 'Collectables'
				particle.GetComponent<ParticlePhysicsManager> ().attractor = gameObject.GetComponent<ParticlePhysicsManager>().attractor;	// manually set new electron FauxGravityAttractor as World
				particle.GetComponent<Animator>().SetBool("black", false);													// set fade trigger
				particle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white

				particle.GetComponent<Animator>().SetBool("photon", true);													// photon: set photon condition
				particle.GetComponent<Animator>().SetTrigger("scaledown");													// scale to photon
				particle.GetComponent<ParticleManager>().initState = ParticleState.Photon;								// set particle to evol 0

				StartCoroutine(PreventParticleDamage(3));																	// prevent particle collisions for 3 sec
			}

			i+=1;																											// # of particle spawn count

			//Debug.Log("particle lose electron");																				// debug
		} while (i != lostPart);
	}

// PARTICLE ABILITIES \\	

	IEnumerator PreventParticleDamage(float time) {
		//Debug.Log ("Particle collider off");
		GetComponent<SphereCollider> ().enabled = false;					// disable core collider
		shell.GetComponent<SphereCollider> ().enabled = false;				// disable shell collider
		yield return new WaitForSeconds(time); 								// wait 5 seconds
		GetComponent<SphereCollider> ().enabled = true;						// enable core collider
		shell.GetComponent<SphereCollider> ().enabled = true;				// enable shell collider
		//Debug.Log ("Particle collider on");
	}

// DEATH & DARK WORLD \\

	void ParticleDeath () {
		// sfx.PlayOneShot (Death, 1.0f);
		GetComponent<SphereCollider> ().enabled = false;									// disable collider at start to prevent collisions during shrink
		//GetComponent<Animator>().SetTrigger("fadeout");										// fade to black
		GetComponent<Animator>().SetBool("dead", true);									// enable black core animation state
		StopAllCoroutines();
		Destroy (gameObject, 2.0f);															// destroy object after animation
	}	


}