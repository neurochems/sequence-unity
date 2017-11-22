using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class ColorControlPostEffect : ImageEffectBase {

	private PlayerStatePattern psp;

	public float invert = 0;
	public AnimationCurve invertCurve;
	public float invertTime = 0f;
	public float invertFactor = 0.5f;

	public float falloff;

	private bool resetInvert;

	new void Start()
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
			invertTime -= Time.fixedDeltaTime * invertFactor;
			resetInvert = true;
		}

		invert = invertCurve.Evaluate (invertTime);

		if (!psp.lightworld && !psp.toDarkworld && resetInvert) {
			invertTime = 0f;
			resetInvert = false;
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		material.SetFloat ("_Invert", invert);
		material.SetFloat ("_Falloff", falloff);
		Graphics.Blit (source, destination, material);
	}
}
