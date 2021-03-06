﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public GameObject ui;																							// UI
	public float textVisibleTime = 10.0f, chillTextVisibleTime, fastTextVisibleTime;								// amount of time text is visible
	public GameObject[] text;																						// ui text objects (for anim)

	private PlayerStatePattern psp;																					// player state pattern
	private StartOptions so;																						// refs
	private PlayerController pc;																						// refs

	private int currentPhilosophy, currentPhilosophy2, currentPhilosophy3;											// current philosophy text index
	private int currentTip, currentTip2, currentTip3;																// current tip text index
	private bool hidePhilosophy, hidePhilosophy2, hidePhilosophy3;													// hide philosophy text flag
	private bool hideTip, hideTip2, hideTip3;																		// hide tip text flag
	private float hidePhilosophyTimer = 0.0f, hidePhilosophyTimer2 = 0.0f, hidePhilosophyTimer3 = 0.0f;				// hide philosophy text timer
	private float hideTipTimer = 0.0f, hideTipTimer2 = 0.0f, hideTipTimer3 = 0.0f;									// hide tip text timer

	private bool quickStart;																						// prevent start text from reappearing

	public bool philosophyVisible = true, tipsVisible;																// menu toggles

	// debug info
	/*public UnityEngine.UI.Text numParticles;
	public GameObject debugParticles;
	private int numTotal, numZero, numFirst, numSecond, numThird, numFourth, numFifth, numSixth, numSeventh, numEighth, numNinth;*/

	void Start () 
	{
		/*if (ui.gameObject.tag == "UI") 
			ui.gameObject.tag = "DontDestroy";*/

		psp = GetComponent<PlayerStatePattern> ();
		so = ui.GetComponent<StartOptions> ();
		pc = GetComponent<PlayerController> ();

		if (pc.chill) {
			fastTextVisibleTime = textVisibleTime;
			chillTextVisibleTime = textVisibleTime * 1.5f;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

		////////////////////////      DEBUG particle counts      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\

/*		// iterate particles, logging tag type
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
*/

		/////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

		if (pc.chill) textVisibleTime = chillTextVisibleTime;
		else if (!pc.chill) textVisibleTime = fastTextVisibleTime;
		else if (psp.state == 10) textVisibleTime = fastTextVisibleTime;

	// timers \\

		if (hidePhilosophy) {																					// if philo
			hidePhilosophyTimer += Time.deltaTime;																	// start timer
			if (hidePhilosophyTimer >= textVisibleTime) {															// if timer = inspector value
				text [currentPhilosophy].GetComponent<Animator> ().SetTrigger ("fade");									// trigger text fade
				text [currentPhilosophy].GetComponent<Animator> ().SetBool ("visible", false);							// trigger text fade
				hidePhilosophy = false;																					// reset is init flag
				hidePhilosophyTimer = 0f;																				// reset timer
			}
		}
		if (hidePhilosophy2) {																					// if philo
			hidePhilosophyTimer2 += Time.deltaTime;																	// start timer
			if (hidePhilosophyTimer2 >= textVisibleTime) {															// if timer = inspector value
				text [currentPhilosophy2].GetComponent<Animator> ().SetTrigger ("fade");								// trigger text fade
				text [currentPhilosophy2].GetComponent<Animator> ().SetBool ("visible", false);							// trigger text fade
				hidePhilosophy2 = false;																				// reset is init flag
				hidePhilosophyTimer2 = 0f;																				// reset timer
			}
		}
		if (hidePhilosophy3) {																					// if philo
			hidePhilosophyTimer3 += Time.deltaTime;																	// start timer
			if (hidePhilosophyTimer3 >= textVisibleTime) {															// if timer = inspector value
				text [currentPhilosophy3].GetComponent<Animator> ().SetTrigger ("fade");								// trigger text fade
				text [currentPhilosophy3].GetComponent<Animator> ().SetBool ("visible", false);							// trigger text fade
				hidePhilosophy3 = false;																				// reset is init flag
				hidePhilosophyTimer3 = 0f;																				// reset timer
			}
		}

		if (hideTip) {																							// if tip
			hideTipTimer += Time.deltaTime;																			// start timer
			if (hideTipTimer >= (fastTextVisibleTime - 5f)) {																// if timer = inspector value - 5
				text [currentTip].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
				text [currentTip].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
				hideTip = false;																						// reset is init flag
				hideTipTimer = 0f;																						// reset timer
			}
		}
		if (hideTip2) {																							// if tip2
			hideTipTimer2 += Time.deltaTime;																		// start timer
			if (hideTipTimer2 >= (fastTextVisibleTime - 5f)) {															// if timer = inspector value - 5
				text [currentTip2].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
				text [currentTip2].GetComponent<Animator> ().SetBool ("visible", false);								// trigger text fade
				hideTip2 = false;																						// reset is init flag
				hideTipTimer2 = 0f;																						// reset timer
			}
		}
		if (hideTip3) {																							// if tip2
			hideTipTimer3 += Time.deltaTime;																		// start timer
			if (hideTipTimer3 >= (fastTextVisibleTime - 5f)) {															// if timer = inspector value - 5
				text [currentTip3].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
				text [currentTip3].GetComponent<Animator> ().SetBool ("visible", false);								// trigger text fade
				hideTip3 = false;																						// reset is init flag
				hideTipTimer3 = 0f;																						// reset timer
			}
		}

	// philosophy \\

		if (philosophyVisible) {
			// philosophy start
			if (!psp.timeCheck && !so.inMainMenu && GetPlaytimeElapsed () >= 10.0f) {						// 5 sec into playable/not-menu game
				if (!text[0].activeInHierarchy && !quickStart) {												// if text is inactive (do nothing if already active) and 1st instance
					text [0].SetActive (true);																		// activate text
					text [0].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
					text [0].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
					currentPhilosophy = 0;																			// set current text index
					hidePhilosophy = true;																			// start hide text timer
				}
			}

			// in dark world 
			if (!psp.lightworld) {

				// philosophy half zero state
				if ((psp.state == 0 && psp.evol == 0.5f) && GetStateElapsed () > 2.0f) {						// 2 sec into being dark world half zero
					// hide previous text
					if (text[0].activeInHierarchy) {																// if text is inactive (do nothing if already active)
						if (text [0].GetComponent<Animator> ().GetBool ("visible")) {										// if anim stuck on visible
							text [0].SetActive (false);																		// deactivate text
							quickStart = true;																				// prevent start text from reappearing
						}
						text [0].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
						text [0].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
						text [0].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
					}
					// activate text
					if (!text[1].activeInHierarchy) {																// if text is inactive (do nothing if already active)
						text [1].SetActive (true);																		// activate text
						text [1].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
						text [1].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
						currentPhilosophy2 = 1;																			// set current text index
						hidePhilosophy2 = true;																			// start hide text timer
					}

				}

				// philosophy first state
				if (psp.state == 1 && GetStateElapsed () > 2.0f) {												// 2 sec into being dark world first state
					if (text[1].activeInHierarchy) {																// if text is inactive (do nothing if already active)
						if (text [1].GetComponent<Animator> ().GetBool("visible"))										// if anim stuck on visible
							text [1].SetActive(false);																		// deactivate text
						text [1].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
						text [1].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
						text [1].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
					}
					if (!text[2].activeInHierarchy) {																// if text is inactive (do nothing if already active)
						text [2].SetActive (true);																		// activate text
						text [2].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
						text [2].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
						currentPhilosophy3 = 2;																			// set current text index
						hidePhilosophy3 = true;																			// start hide text timer
					}
				}

				// philosophy third state
				if (psp.state == 3 && GetStateElapsed () > 2.0f) {												// 2 sec into being dark world third state
					if (text[2].activeInHierarchy) {																// if text is inactive (do nothing if already active)
						text [2].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
						text [2].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
						text [2].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
					}
					if (!text[3].activeInHierarchy) {																// if text is inactive (do nothing if already active)
						text [3].SetActive (true);																		// activate text
						text [3].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
						text [3].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
						currentPhilosophy = 3;																			// set current text index
						hidePhilosophy = true;																			// start hide text timer
					}
				}

				// philosophy fifth state
				if (psp.state == 5) {
					// philosophy fifth state 1
					if (GetStateElapsed () > 2.0f) {																// 2 sec into being dark world fifth state
						// hide previous text
						if (text[3].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [3].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
							text [3].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
							text [3].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
						}
						// activate text
						if (!text[4].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [4].SetActive (true);																		// activate text
							text [4].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
							text [4].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
							currentPhilosophy = 4;																			// set current text index
							hidePhilosophy = true;																			// start hide text timer
						}
					}
					// philosophy fifth state 2
					if (GetStateElapsed () > 20.0f) {																// 20 sec into being dark world fifth state
						// hide previous text
						if (text[4].activeInHierarchy) {																// if text is active (do nothing if already inactive)
							text [4].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
							text [4].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
							text [4].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
						}
						// activate text
						if (!text[5].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [5].SetActive (true);																		// activate text
							text [5].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
							text [5].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
							currentPhilosophy2 = 5;																			// set current text index
							hidePhilosophy2 = true;																			// start hide text timer
						}
					}
				}

				// philosophy sixth state
				if (psp.state == 6 || psp.state == 7) {
					// philosophy sixth state 1
					if (GetStateElapsed () > 2.0f) {																// 2 sec into being dark world sixth or seventh state
						// hide previous text
						if (text[4].activeInHierarchy) {																// if text is active (do nothing if already inactive)
							text [4].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
							text [4].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
							text [4].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
						}
						if (text[5].activeInHierarchy) {																// if text is active (do nothing if already inactive)
							text [5].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
							text [5].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
							text [5].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
						}
						// activate text
						if (!text[6].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [6].SetActive (true);																		// activate text
							text [6].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
							text [6].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
							currentPhilosophy = 6;																			// set current text index
							hidePhilosophy = true;																			// start hide text timer
						}
					}
					// philosophy sixth state 2
					if (GetStateElapsed () > 20.0f) {																// 12 sec into being dark world sixth or seventh state
						if (!text[7].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [7].SetActive (true);																		// activate text
							text [7].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
							text [7].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
							currentPhilosophy2 = 7;																			// set current text index
							hidePhilosophy2 = true;																			// start hide text timer
						}
					}
				}

				// philosophy eighth state
				if (psp.state == 8 || psp.state == 9) {
					// philosophy eighth state 1
					if (GetStateElapsed () > 2.0f) {							// 2 sec into being dark world eighth or ninth state
						// hide previous text
						if (text[7].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [7].GetComponent<Animator> ().SetTrigger ("fade");											// trigger text fade
							text [7].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
							text [7].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
						}
						// activate text
						if (!text[8].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [8].SetActive (true);																		// activate text
							text [8].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
							text [8].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
							currentPhilosophy = 8;																			// set current text index
							hidePhilosophy = true;																			// start hide text timer
						}
					}
					// philosophy eighth state 2
					if (GetStateElapsed () > 20.0f) {							// 12 sec into being dark world eighth or ninth state
						if (!text[9].activeInHierarchy) {																// if text is inactive (do nothing if already active)
							text [9].SetActive (true);																		// activate text
							text [9].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
							text [9].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
							currentPhilosophy2 = 9;																			// set current text index
							hidePhilosophy2 = true;																			// start hide text timer
						}
					}
				}

				// philosophy tenth state
				if (psp.state == 10) {
					// philosophy tenth state 1
					if (GetStateElapsed () > 10.0f) {															// 10 sec into being tenth state
						if (!text[10].activeInHierarchy) {															// if text is inactive (do nothing if already active)
							text [10].SetActive (true);																	// activate text
							text [10].GetComponent<Animator> ().SetTrigger ("fade");									// trigger overlay text
							text [10].GetComponent<Animator> ().SetBool ("visible", true);								// trigger text fade
							currentPhilosophy = 10;																		// set current text index
							hidePhilosophy = true;																		// start hide text timer
						}
					}
					// philosophy tenth state 2
					if (GetStateElapsed () > 20.0f) {															// 20 sec into being tenth state
						if (!text[11].activeInHierarchy) {															// if text is inactive (do nothing if already active)
							text [11].SetActive (true);																	// activate text
							text [11].GetComponent<Animator> ().SetTrigger ("fade");									// trigger overlay text
							text [11].GetComponent<Animator> ().SetBool ("visible", true);								// trigger text fade
							currentPhilosophy2 = 11;																	// set current text index
							hidePhilosophy2 = true;																		// start hide text timer
						}
					}
					// philosophy tenth state 3
					if (GetStateElapsed () > 35.0f) {															// 35 sec into being tenth state
						if (!text[12].activeInHierarchy) {															// if text is inactive (do nothing if already active)
							text [12].SetActive (true);																	// activate text
							text [12].GetComponent<Animator> ().SetTrigger ("fade");									// trigger overlay text
							text [12].GetComponent<Animator> ().SetBool ("visible", true);								// trigger text fade
							currentPhilosophy3 = 12;																	// set current text index
							hidePhilosophy3 = true;																		// start hide text timer
						}
					}
					// philosophy tenth state 4
					if (GetStateElapsed () > 50.0f) {															// 50 sec into being tenth state
						if (!text[13].activeInHierarchy) {															// if text is inactive (do nothing if already active)
							text [13].SetActive (true);																	// activate text
							text [13].GetComponent<Animator> ().SetTrigger ("fade");									// trigger overlay text
							text [13].GetComponent<Animator> ().SetBool ("visible", true);								// trigger text fade
							currentPhilosophy = 13;																		// set current text index
							hidePhilosophy = true;																		// start hide text timer
						}
					}
					// philosophy tenth state 5
					if (GetStateElapsed () > 60.0f) {															// 60 sec into being tenth state
						if (!text[14].activeInHierarchy) {															// if text is inactive (do nothing if already active)
							text [14].SetActive (true);																	// activate text
							text [14].GetComponent<Animator> ().SetTrigger ("fade");									// trigger overlay text
							text [14].GetComponent<Animator> ().SetBool ("visible", true);								// trigger text fade
							currentPhilosophy2 = 14;																	// set current text index
							hidePhilosophy2 = true;																		// start hide text timer
						}
					}
				}
			}

			// in light world
			if (psp.lightworld) {

				// philosophy light world 1
				if (GetStateElapsed () > 20.0f) {																// 20 sec into being light world
					if (!text [15].activeInHierarchy) {																// if inactive	
						text [15].SetActive (true);																		// activate text
						text [15].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
						text [15].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
						currentPhilosophy3 = 15;																		// set current text index
						hidePhilosophy3 = true;																			// start hide text timer
					}
				}

				// philosophy light world 2
				if (GetStateElapsed () > 45.0f) {																// 45 sec into being light world
					if (!text [16].activeInHierarchy) {																// if inactive
						text [16].SetActive (true);																		// activate text
						text [16].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
						text [16].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
						currentPhilosophy2 = 16;																		// set current text index
						hidePhilosophy2 = true;																			// start hide text timer
					}
				}
			}
		}

	// tips \\

		if (tipsVisible) {
			// tip wasd
			if (!psp.timeCheck && !so.inMainMenu && GetPlaytimeElapsed () >= 5.0f) {							// 5 sec into playable/not-menu game
				if (!text[17].activeInHierarchy) {																	// if text is inactive (do nothing if already active)
					text [17].SetActive (true);																			// activate text
					if (!psp.lightworld) 																				// if dark world
						text [17].GetComponent<Text>().color = Color.white;												// set text colour 
					else if (psp.lightworld) 																			// if light world
						text [17].GetComponent<Text>().color = Color.black;												// set text colour 
					text [17].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
					text [17].GetComponent<Animator> ().SetBool ("visible", true);										// trigger text fade
					currentTip = 17;																					// set current text index
					hideTip = true;																						// start hide text timer
				}
			}
			// tip wasd 2
			if (!psp.timeCheck && !so.inMainMenu && GetPlaytimeElapsed () >= 25.0f) {							// 15 sec into playable/not-menu game
				if (!text[18].activeInHierarchy) {																	// if text is inactive (do nothing if already active)
					text [18].SetActive (true);																			// activate text
					if (!psp.lightworld) 																				// if dark world
						text [18].GetComponent<Text>().color = Color.white;												// set text colour 
					else if (psp.lightworld) 																			// if light world
						text [18].GetComponent<Text>().color = Color.black;												// set text colour 
					text [18].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
					text [18].GetComponent<Animator> ().SetBool ("visible", true);										// trigger text fade
					currentTip2 = 18;																					// set current text index
					hideTip2 = true;																					// start hide text timer
				}
			}
			// tip propel
			if (!psp.timeCheck && !so.inMainMenu && GetPlaytimeElapsed () >= 50.0f) {							// 45 sec into playable/not-menu game
				if (!text[19].activeInHierarchy) {																	// if text is inactive (do nothing if already active)
					text [19].SetActive (true);																			// activate text
					if (!psp.lightworld) 																				// if dark world
						text [19].GetComponent<Text>().color = Color.white;												// set text colour 
					else if (psp.lightworld) 																			// if light world
						text [19].GetComponent<Text>().color = Color.black;												// set text colour 
					text [19].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
					text [19].GetComponent<Animator> ().SetBool ("visible", true);										// trigger text fade
					currentTip3 = 19;																					// set current text index
					hideTip3 = true;																					// start hide text timer
				}
			}
			// tip propel 2
			if (!psp.timeCheck && !so.inMainMenu && GetPlaytimeElapsed () >= 80.0f) {							// 80 sec into playable/not-menu game
				if (!text[20].activeInHierarchy) {																	// if text is inactive (do nothing if already active)
					text [20].SetActive (true);																			// activate text
					if (!psp.lightworld) 																				// if dark world
						text [20].GetComponent<Text>().color = Color.white;												// set text colour 
					else if (psp.lightworld) 																			// if light world
						text [20].GetComponent<Text>().color = Color.black;												// set text colour 
					text [20].GetComponent<Animator> ().SetTrigger ("fade");											// trigger overlay text
					text [20].GetComponent<Animator> ().SetBool ("visible", true);										// trigger text fade
					currentTip = 20;																					// set current text index
					hideTip = true;																					// start hide text timer
				}
			}
			// tip density
			if (psp.state == 4 && GetStateElapsed () > 5.0f) {												// 5 sec into being fourth state
				// hide previous tips
				if (text[19].activeInHierarchy) {																// if text is active (do nothing if alreadyin active)
					text [19].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
					text [19].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
					text [19].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
				}
				if (text[20].activeInHierarchy) {																// if text is active (do nothing if already inactive)
					text [20].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
					text [20].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
					text [20].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
				}
				// activate tip
				if (!text[21].activeInHierarchy) {																// if text is inactive (do nothing if already active)
					text [21].SetActive (true);																		// activate text
					if (!psp.lightworld) 																			// if dark world
						text [21].GetComponent<Text>().color = Color.white;											// set text colour 
					else if (psp.lightworld)																		// if light world
						text [21].GetComponent<Text>().color = Color.black;											// set text colour 
					text [21].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
					text [21].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
					currentTip2 = 21;																				// set current text index
					hideTip2 = true;																				// start hide text timer
				}
			}
			// tip entropy
			if (psp.state == 5 && GetStateElapsed () > 15.0f) {												// 15 sec into being fifth state
				// hide previous tips
				if (text[20].activeInHierarchy) {																// if text is active (do nothing if already inactive)
					text [20].GetComponent<Animator> ().SetTrigger ("fade");										// trigger text fade
					text [20].GetComponent<Animator> ().SetBool ("visible", false);									// trigger text fade
					text [20].GetComponent<Animator> ().speed = 2.0f;												// double animation speed
				}
				if (!text[22].activeInHierarchy) {																// if text is inactive (do nothing if already active)
					text [22].SetActive (true);																		// activate text
					if (!psp.lightworld)																			// if dark world
						text [22].GetComponent<Text>().color = Color.white;											// set text colour 
					else if (psp.lightworld)																		// if light world
						text [22].GetComponent<Text>().color = Color.black;											// set text colour 
					text [22].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
					text [22].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
					currentTip3 = 22;																				// set current text index
					hideTip3 = true;																				// start hide text timer
				}
			}
			// tip diversity
			if (psp.state == 7 && GetStateElapsed () > 10.0f) {												// 15 sec into being seventh state
				if (!text[23].activeInHierarchy) {																// if text is inactive (do nothing if already active)
					text [23].SetActive (true);																		// activate text
					if (!psp.lightworld)																			// if dark world
						text [23].GetComponent<Text>().color = Color.white;											// set text colour 
					else if (psp.lightworld)																		// if light world
						text [23].GetComponent<Text>().color = Color.black;											// set text colour 
					text [23].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
					text [23].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
					currentTip = 23;																				// set current text index
					hideTip = true;																				// start hide text timer
				}
			}
		}

	// reset tip \\

		// tip reset
		if (GetPlaytimeElapsed() >= 420.0f) {															// playtime >= 7 mins
			if (!psp.lightworld)																			// if dark world
				text [24].GetComponent<Text>().color = Color.white;											// set text colour 
			else if (psp.lightworld)																		// if light world
				text [24].GetComponent<Text>().color = Color.black;											// set text colour 
			text [24].SetActive (true);																		// activate text
			text [24].GetComponent<Animator> ().SetTrigger ("fade");										// trigger overlay text
			text [24].GetComponent<Animator> ().SetBool ("visible", true);									// trigger text fade
		}

		if ((text [24].activeInHierarchy) && (psp.state == 10)) {										// if active and tenth state
			currentTip2 = 24;																				// set current text index
			hideTip2 = true;																				// start hide text timer
		}

	// endgame \\

		if (psp.state == 10 && GetStateElapsed () > 65.0f) {											// 65 sec into being tenth state
			ui.gameObject.tag = "Destroy";																	// flag old ui for destroy
			SceneManager.LoadScene("Sequence1");															// restart scene
		}

	}

	// timers \\

	public void PhilosophyVisible(bool toggle) {
		philosophyVisible = toggle;
	}

	public void TipsVisible(bool toggle) {
		tipsVisible = toggle;
	}

	float GetStateElapsed() {
		return Time.time - psp.lastStateChange;												// returns time spent in current state
	}
	float GetPlaytimeElapsed() {
		return Time.time - psp.sincePlaytimeBegin;											// returns time since player control
	}

}
