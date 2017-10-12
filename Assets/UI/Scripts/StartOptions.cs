// from: https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/game-jam-template?playlist=17111

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartOptions : MonoBehaviour {

	//public int sceneToStart = 0;										//Index number in build settings of scene to load if changeScenes is true
	//public bool changeScenes;											//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	//public bool changeMusicOnStart;										//Choose whether to continue playing menu music or start a new music clip


	public bool inMainMenu = true;										//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	public Animator animColorFade; 										//Reference to animator which will fade to and from black when starting game.
	public Animator animStartFade; 										//Reference to animator which will fade to and from black when starting game.
	public Animator animMenuAlpha;										//Reference to animator that will fade out alpha of MenuPanel canvas group
	public AnimationClip fadeColorAnimationClip;						//Animation clip fading to color (black default) when changing scenes
	public AnimationClip startFadeColorAnimationClip;					//Animation clip fading to color (black default) when changing scenes
	public AnimationClip fadeAlphaAnimationClip;						//Animation clip fading out UI elements alpha


	private PlayMusic playMusic;										//Reference to PlayMusic script
	//private float fadeIn = 2.5f;										//Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels

	
	void Awake()
	{
		inMainMenu = true;
		//Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<ShowPanels> ();

		// begin fade in
		animStartFade.SetTrigger("fade");
		//animStartFade.SetBool("visible", true);

		//Get a reference to PlayMusic attached to UI object
		playMusic = GetComponent<PlayMusic> ();
	}


	public void StartButtonClicked()
	{
		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		//To change fade time, change length of animation "FadeToColor"
		/*if (changeMusicOnStart) 
		{
			playMusic.FadeDown(fadeColorAnimationClip.length);
		}*/

		//If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
		/*if (changeScenes) 
		{
			//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke ("LoadDelayed", fadeColorAnimationClip.length * .5f);

			//Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
			animColorFade.SetTrigger ("fade");
		} */

		//If changeScenes is false, call StartGameInScene
		/*else 
		{*/
			//Call the StartGameInScene function to start game without loading a new scene.
			StartGameInScene();
		//}

	}

	//Once the level has loaded, check if we want to call PlayLevelMusic
	/*void OnLevelWasLoaded()
	{
		//if changeMusicOnStart is true, call the PlayLevelMusic function of playMusic
		if (changeMusicOnStart)
		{
			playMusic.PlayLevelMusic ();
		}	
	}*/


	/*public void LoadDelayed()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//Hide the main menu UI element
		showPanels.HideMenu ();

		//Load the selected scene, by scene index number in build settings
		SceneManager.LoadScene (sceneToStart);
	}*/

	public void HideDelayed()
	{
		//Hide the main menu UI element after fading out menu for start game in scene
		showPanels.HideMenu();
	}

	public void StartGameInScene()
	{

		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;
		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		//To change fade time, change length of animation "FadeToColor"
		/*	if (changeMusicOnStart) 
		{
			//Wait until game has started, then play new music
			Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
		}*/
		//Set trigger for animator to start animation fading out Menu UI
		animMenuAlpha.SetTrigger ("fade");												// fade menu to black
		animMenuAlpha.SetBool ("visible", false);										// fade menu to black
		animColorFade.SetTrigger ("fade");												// fade menu tint to black
		animColorFade.SetBool ("visible", false);										// fade menu tint to black
		Invoke("HideDelayed", fadeAlphaAnimationClip.length);							// hide menu after fade out
		Debug.Log ("Game started in same scene! Put your game starting stuff here.");
	}


	/*public void PlayNewMusic()
	{
		//Fade up music nearly instantly without a click 
		playMusic.FadeUp (fadeIn);
		//Play music clip assigned to mainMusic in PlayMusic script
		//Debug.Log("start new music");
		playMusic.PlaySelectedMusic (1);
	}*/
}
