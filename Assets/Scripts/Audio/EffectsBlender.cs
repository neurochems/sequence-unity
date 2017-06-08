using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class EffectsBlender : MonoBehaviour {

	public AudioMixer mixer;						// ref to audio mixer
	public AudioClip[] stems;						// ref for audio stems
	public AudioMixerSnapshot[] snapshots;			// ref to mixer snapshots for each state + fade in/out
	public float[] weights;							// weight of snapshots in each blend

	///<summary>
	///<para>0 = circle</para>
	///<para>1 = triangle</para>
	///<para>2 = square</para>
	///<para>3 = light world</para>
	///</summary>
	public void BlendSnapshot(int trigger) 
	{
		switch (trigger)
		{
		case 1:
			weights [0] = 1.0f;
			weights [1] = 0.0f;
			mixer.TransitionToSnapshots(snapshots, weights, 2.0f);
			break;
		case 2:
			break;
		}
	}

}
