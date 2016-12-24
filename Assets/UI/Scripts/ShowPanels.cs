using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject creditsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 
	public GameObject pauseTint;							//Store a reference to the Game Object OptionsTint 

	public Animator animCreditsAlpha;						//Reference to animator that will fade in alpha of Credits Panel canvas group

	//Call this function to activate and display the Credits panel during the main menu
	public void ShowCreditsPanel()
	{
		creditsPanel.SetActive(true);
		animCreditsAlpha.SetTrigger ("fade");
		//creditsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Credits panel during the main menu
	public void HideCreditsPanel()
	{
		creditsPanel.SetActive(false);
		//creditsTint.SetActive(false);
	}

	//Call this function to activate and display the main menu panel during the main menu
	public void ShowMenu()
	{
		menuPanel.SetActive (true);
	}

	//Call this function to deactivate and hide the main menu panel during game play
	public void HideMenu()
	{
		menuPanel.SetActive (false);
	}
	
	//Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel()
	{
		pausePanel.SetActive (true);
		pauseTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel()
	{
		pausePanel.SetActive (false);
		pauseTint.SetActive(false);

	}
}
