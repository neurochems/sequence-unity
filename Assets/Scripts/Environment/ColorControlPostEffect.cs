using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class ColorControlPostEffect : ImageEffectBase {

	private PlayerStatePattern psp;

	//public bool lightworld, toDarkworld;

	public float invert = 0;
	public AnimationCurve invertCurve;
	private float invertTime = 0f;
	public float invertFactor = 1f;

	public float falloff;

	void Start()
	{
		psp = GetComponentInParent<PlayerStatePattern> ();
		//toDarkworld = GetComponentInParent<PlayerStatePattern> ().toDarkworld;
	}

	void Update()
	{
		if (psp.toLightworld) {
			invertTime += Time.fixedDeltaTime * invertFactor;
		} 
		else if (psp.toDarkworld) {
			Debug.Log ("reverse invert to dark world");
			invertTime -= Time.fixedDeltaTime * (invertFactor + 0.25f);
		}

		invert = invertCurve.Evaluate (invertTime);
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		material.SetFloat ("_Invert", invert);
		material.SetFloat ("_Falloff", falloff);
		Graphics.Blit (source, destination, material);
	}
}
