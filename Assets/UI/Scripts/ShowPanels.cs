using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject creditsPanel;							//Store a reference to the Game Object creditsPanel 
	public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject title;								//Store a reference to the Game Object title 
	public GameObject subtitle;								//Store a reference to the Game Object subtitle
	public GameObject playButton;							//Store a reference to the Game Object play button 
	public GameObject pausePanel;						//Store a reference to the Game Object SettingsPanel 
	public GameObject pauseTint;							//Store a reference to the Game Object OptionsTint 

	public Animator animMenuAlpha;										//Reference to animator that will fade out alpha of MenuPanel canvas group
	public Animator animCreditsAlpha;						//Reference to animator that will fade in alpha of Credits Panel canvas group

	private bool show = true, hide;
	private float hideTimer;

	void Update() 
	{
		// initialization period timer
		if (hide) {																		// if init
			hideTimer += Time.deltaTime;														// start timer
			if (hideTimer >= 2.0f) {															// if timer = 1.2 sec
				creditsPanel.SetActive(false);														// deactivate panel
				show = true;																		// show panel next click
				hide = false;																		// reset is init flag
				hideTimer = 0f;																		// reset timer
			}
		}
	}

	//Call this function to activate and display the Credits panel during the main menu
	public void CreditsPanel()
	{
		if (show) {
			creditsPanel.SetActive(true);
			animCreditsAlpha.SetTrigger ("fade");
			show = false;
		}
		else if (!show) {
			animCreditsAlpha.SetTrigger ("fadeout");
			hide = true;
		}
	}

	//Call this function to activate and display the main menu panel during the main menu
	public void ShowMenu()
	{
		//menuPanel.SetActive (true);
	}

	//Call this function to deactivate and hide the main menu panel during game play
	public void HideMenu()
	{
		title.SetActive (false);
		subtitle.SetActive (false);
		playButton.SetActive (false);
		//settingsPanel.SetActive (false);
		menuPanel.SetActive (false);
	}
	
	//Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel()
	{
		pauseTint.SetActive(true);
		//animMenuAlpha.SetTrigger ("fade");												// fade menu to visible
		//animMenuAlpha.SetBool ("visible", true);										// fade menu to visible
		pausePanel.SetActive (true);
		//menuPanel.SetActive (true);
	}

	//Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel()
	{
		//menuPanel.SetActive (false);
		//animMenuAlpha.SetTrigger ("fade");												// fade menu to black
		//animMenuAlpha.SetBool ("visible", false);										// fade menu to black
		pausePanel.SetActive (false);
		pauseTint.SetActive(false);

	}
}
