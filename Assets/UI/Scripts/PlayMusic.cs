using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayMusic : MonoBehaviour {

	public AudioClip zero;							// ref Audioclip for main 
	public AudioMixerSnapshot volumeDown;			// ref to mixer snapshot of main up 
	public AudioMixerSnapshot volumeUp;				// ref to mixer snapshot of main down

	private AudioSource musicSource;				// ref to audiosource which plays music
	private float resetTime = .01f;					// very short time used to fade in near instantly without a click


	void Awake () 
	{
		musicSource = GetComponent<AudioSource> ();					// init audiosource ref
		//Call the PlayLevelMusic function to start playing music (in startoptions)
	}


	public void PlayLevelMusic()
	{
		//This switch looks at the last loadedLevel number using the scene index in build settings to decide which music clip to play.
		//switch (SceneManager.GetActiveScene().buildIndex)
		//{

		//musicSource.clip = titleMusic;
		musicSource.clip = zero;
		//Fade up the volume very quickly, over resetTime seconds (.01 by default)
		FadeUp (resetTime);
		//Play the assigned music clip in musicSource
		musicSource.Play ();
	}
	
	//Used if running the game in a single scene, takes an integer music source allowing you to choose a clip by number and play.
	public void PlaySelectedMusic(int snapshotChoice)
	{

		//This switch looks at the integer parameter snapshotChoice to decide which snapshot to play.
		switch (snapshotChoice) {
		//if snapshotChoice is 0 assigns zero state snapshot
		case 0:
			musicSource.clip = zero;
			break;
			//if snapshotChoice is 0 assigns first state snapshot
		case 1:
			musicSource.clip = zero;
			break;
		}
		//Play the selected clip
		musicSource.Play ();
	}

	//Call this function to very quickly fade up the volume of master mixer
	public void FadeUp(float fadeTime)
	{
		//call the TransitionTo function of the audioMixerSnapshot volumeUp;
		volumeUp.TransitionTo (fadeTime);
	}

	//Call this function to fade the volume to silence over the length of fadeTime
	public void FadeDown(float fadeTime)
	{
		//call the TransitionTo function of the audioMixerSnapshot volumeDown;
		volumeDown.TransitionTo (fadeTime);
	}
}
