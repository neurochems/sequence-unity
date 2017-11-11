// from: https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/game-jam-template?playlist=17111

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartOptions : MonoBehaviour {

	public bool inMainMenu = true;										//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	public Animator animColorFade; 										//Reference to animator which will fade to and from black when starting game.
	public Animator animStartFade; 										//Reference to animator which will fade to and from black when starting game.
	public Animator animMenuAlpha;										//Reference to animator that will fade out alpha of MenuPanel canvas group
	public AnimationClip fadeColorAnimationClip;						//Animation clip fading to color (black default) when changing scenes
	public AnimationClip startFadeColorAnimationClip;					//Animation clip fading to color (black default) when changing scenes
	public AnimationClip fadeAlphaAnimationClip;						//Animation clip fading out UI elements alpha

	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels

	public bool disableInput = true;									// disable input until fade in is complete
	private GameObject pb;												// play button ref
	private Button pbButton;											// play button Button ref
	private float disableInputTimer = 0.0f;								// disable input timer
	
	void Awake()
	{
		inMainMenu = true;
		//Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<ShowPanels> ();

		// begin fade in
		animStartFade.SetTrigger("fade");
		//animStartFade.SetBool("visible", true);

	}

	void Start() 
	{
		pb = GameObject.Find("Play Button");
		pbButton = GameObject.Find("Play Button").GetComponent<Button>();
	}

	void Update() {

		// disable input until fade in (4 sec+0.2s buffer) is complete
		if (inMainMenu && disableInput) {														// if main menu and during fade in
			pbButton.interactable = false;															// enable play button
			disableInputTimer += Time.deltaTime;													// start timer
			if (disableInputTimer >= 4.20f) {														// if timer = 4.20 sec
				pbButton.interactable = true;															// enable play button
				EventSystem.current.SetSelectedGameObject(pb, null);									// give focus to play button
				disableInput = false;																	// reset is init flag
				disableInputTimer = 0f;																	// reset timer
			}
		}

	}

	public void StartButtonClicked()
	{
		StartGameInScene();
	}

	public void HideDelayed()
	{
		//Hide the main menu UI element after fading out menu for start game in scene
		showPanels.HideMenu();
	}

	public void StartGameInScene()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;
		//Set trigger for animator to start animation fading out Menu UI
		animMenuAlpha.SetTrigger ("fade");												// fade menu to black
		animMenuAlpha.SetBool ("visible", false);										// fade menu to black
		animColorFade.SetTrigger ("fade");												// fade menu tint to black
		animColorFade.SetBool ("visible", false);										// fade menu tint to black
		Invoke("HideDelayed", fadeAlphaAnimationClip.length);							// hide menu after fade out
		Debug.Log ("Game started in same scene! Put your game starting stuff here.");
	}

}
