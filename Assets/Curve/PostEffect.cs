using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour {

	public float curveIntensity;
	public Material mat;
	public float vignettePower;

    private Material matInstance;

    private void Start()
    {
        matInstance = new Material(mat);
    }
    
	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
        matInstance.SetFloat("_ScreenWidth", Screen.width);
        matInstance.SetFloat("_ScreenHeight", Screen.height);
        matInstance.SetFloat("_CurveIntensity", curveIntensity);
        matInstance.SetFloat("_VignettePower", vignettePower);
		Graphics.Blit(src, dest, matInstance);
	}
}
