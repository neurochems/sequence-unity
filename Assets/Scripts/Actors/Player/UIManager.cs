using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	// UI
	public GameObject uI;
	public GameObject deathFade;

	// player state pattern
	private PlayerStatePattern psp;

	// flags
	private bool dead = false; 
	private bool endgame = false; 

	// timers
	private float deathFadeTimer = 0.0f;
	private float endGameTimer = 0.0f;

	// endless
	public bool endless = false;

	// debug info
	//public UnityEngine.UI.Text numParticles;
	//public GameObject debugCollectables;
	//private int numTotal, numPhoton, numElectron, numElectron2, numShell, numShell2, numAtom, numAtom2;

	// ui text anim
	public GameObject[] overlayTextTrigger;



	void Start () 
	{
		psp = GetComponent<PlayerStatePattern> ();	
	}
	
	// Update is called once per frame
	void Update () 
	{

		/*////////////////////////      DEBUG particle counts      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\

		// total children
		numTotal = debugCollectables.transform.childCount;

		/ iterate particles, logging tag type
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

		*/

		//////////////////////////////////////////////////////////////////////////////////////

		if (dead) deathFadeTimer += Time.deltaTime;																		// start death timer
		if (deathFadeTimer >= 8f) {																					// if timer >= duration
			uI.gameObject.tag = "Destroy";																				// flag old ui for destroy
			SceneManager.LoadScene("Sequence1");																		// restart scene
			deathFadeTimer = 0f;																						// reset timer
		}

		if (endgame) endGameTimer += Time.deltaTime;																	// start endgame timer
		if (endGameTimer >= 10f) {																				// if timer >= duration
			uI.gameObject.tag = "Destroy";																				// flag old ui for destroy
			SceneManager.LoadScene("Sequence1");																		// restart scene
			endGameTimer = 0f;																						// reset timer
		}

		// checks for OVERLAY TEXT (FIRST)

		if (!uI.GetComponent<StartOptions> ().inMainMenu && GetPlaytimeElapsed () >= 5.0f) {							// 5 sec into playable/not-menu game	// OVERLAY TEXT
			overlayTextTrigger [0].SetActive (true);																	// activate text
			overlayTextTrigger [0].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
		}

		// checks for OVERLAY TEXT (SECOND)
		if (psp.currentState == psp.firstState && GetStateElapsed () > 2.0f) {																			// 2 sec into being electron				
			overlayTextTrigger [1].SetActive (true);																	// activate text
			overlayTextTrigger [1].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
		}	
	 	
		// checks for OVERLAY TEXT (THIRD)
		if (psp.currentState == psp.thirdState && GetStateElapsed () > 2.0f) {																			// 2 sec into being shell				
			overlayTextTrigger [2].SetActive (true);																	// activate text
			overlayTextTrigger [2].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
		}	

		// checks for OVERLAY TEXT (FOURTH)
		if (psp.currentState == psp.fifthState && GetStateElapsed () > 2.0f) {																			// 2 sec into being atom				
			overlayTextTrigger [3].SetActive (true);																	// activate text
			overlayTextTrigger [3].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
		}

		// checks for OVERLAY TEXT (ENDING)
		if (psp.currentState == psp.fifthState && GetStateElapsed () > 40.0f) {																			// 20 sec into being atom				
			overlayTextTrigger [4].SetActive (true);																	// activate text
			overlayTextTrigger [4].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text

			//overlayTextTrigger [4].SetActive (true);																	// activate text
			//overlayTextTrigger [4].GetComponent<Animator> ().SetTrigger ("fade");

			//overlayTextTrigger [4].SetActive (true);																	// activate text
			//overlayTextTrigger [4].GetComponent<Animator> ().SetTrigger ("fade");

			//overlayTextTrigger [4].SetActive (true);																	// activate text
			//overlayTextTrigger [4].GetComponent<Animator> ().SetTrigger ("fade");

			if (!endless) {																								// if not endless
				endgame = true;
				deathFade.SetActive(true);																					// fade screen to black
			}
		}	
	}

	public void Dead(bool isDead)
	{
		dead = isDead;
		deathFade.SetActive(true);																					// fade screen to black

		overlayTextTrigger [5].SetActive (true);																	// activate text
		overlayTextTrigger [5].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
	}

	public void IsEndless(bool toggle) {
		endless = toggle;
	}

	float GetStateElapsed() {
		return Time.time - psp.lastStateChange;											// returns time spent in current state
	}

	float GetPlaytimeElapsed() {
		return Time.time - psp.sincePlaytimeBegin;											// returns time since player control
	}

}
