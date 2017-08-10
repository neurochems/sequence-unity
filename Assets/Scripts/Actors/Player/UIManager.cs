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
	public UnityEngine.UI.Text numParticles;
	public GameObject debugParticles;
	private int numTotal, numZero, numFirst, numSecond, numThird, numFourth, numFifth, numSixth, numSeventh, numEighth, numNinth;

	// ui text anim
	public GameObject[] overlayTextTrigger;



	void Start () 
	{
		psp = GetComponent<PlayerStatePattern> ();	
	}
	
	// Update is called once per frame
	void Update () 
	{

		////////////////////////      DEBUG particle counts      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\

		// iterate particles, logging tag type
		foreach (Transform child in debugParticles.transform) {
			if (child.CompareTag("Zero") && child.gameObject.activeInHierarchy) numZero += 1;
			else if (child.CompareTag("First") && child.gameObject.activeInHierarchy) numFirst += 1;
			else if (child.CompareTag("Second") && child.gameObject.activeInHierarchy) numSecond += 1;
			else if (child.CompareTag("Third") && child.gameObject.activeInHierarchy) numThird += 1;
			else if (child.CompareTag("Fourth") && child.gameObject.activeInHierarchy) numFourth += 1;
			else if (child.CompareTag("Fifth") && child.gameObject.activeInHierarchy) numFifth += 1;
			else if (child.CompareTag("Sixth") && child.gameObject.activeInHierarchy) numSixth += 1;
			else if (child.CompareTag("Seventh") && child.gameObject.activeInHierarchy) numSeventh += 1;
			else if (child.CompareTag("Eighth") && child.gameObject.activeInHierarchy) numEighth += 1;
			else if (child.CompareTag("Ninth") && child.gameObject.activeInHierarchy) numNinth += 1;
		}

		// total children
		numTotal = numZero + numFirst + numSecond + numThird + numFourth + numFifth + numSixth + numSeventh + numEighth + numNinth;

		// display info
		numParticles.text = "Total: " + numTotal + "\nZero: " + numZero + "\nFirst: " + numFirst + "\nSecond: " + numSecond + 
			"\nThird: " + numThird + "\nFourth: " + numFourth + "\nFifth: " + numFifth + "\nSixth: " + numSixth + "\nSeventh: " + numSeventh + 
			"\nEighth: " + numEighth + "\nNinth: " + numNinth;

		// reset numbers
		numTotal = 0;
		numZero = 0;
		numFirst = 0;
		numSecond = 0;
		numThird = 0;
		numFourth = 0;
		numFifth = 0;
		numSixth = 0;
		numSeventh = 0;
		numEighth = 0;
		numNinth = 0;

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
