using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementShaderEffect : MonoBehaviour {

    public Shader ReplacementShader;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        if(ReplacementShader != null)
        {
            GetComponent<Camera>().SetReplacementShader(ReplacementShader, "Opaque");
        }
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
