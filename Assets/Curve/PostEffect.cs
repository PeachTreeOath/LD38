using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour {

	public float curveIntensity;
	public Material mat;
	public float vignettePower;

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		mat.SetFloat("_ScreenWidth", Screen.width);
		mat.SetFloat("_ScreenHeight", Screen.height);
		mat.SetFloat("_CurveIntensity", curveIntensity);
		mat.SetFloat("_VignettePower", vignettePower);
		Graphics.Blit(src, dest, mat);
	}
}
