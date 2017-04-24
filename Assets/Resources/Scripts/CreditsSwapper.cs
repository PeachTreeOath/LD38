using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSwapper : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GameObject psFab = Resources.Load("Prefabs/CreditsPS") as GameObject;

		for(int i = 1; i <= 13; i++)
		{
			Material mat = Resources.Load("Materials/Credits/Credits"+i) as Material;
			GameObject go = Instantiate(psFab);
			ParticleSystem ps = go.GetComponent<ParticleSystem>();
			if(i > 1)
			{
				ps.emissionRate = Mathf.Max(.3f, (i/10f) * (.5f));
			}else
			{
				ps.emissionRate = .85f;
			}
			ps.startRotation3D = new Vector3(0, 0, 4);
			go.transform.position = gameObject.transform.position;
			go.transform.rotation = transform.rotation;
			ps.GetComponent<Renderer>().material = mat;
			ps.startDelay = i*.45f;
			ps.startSpeed = Mathf.Max(2.5f, i * .35f);
		}
	}
}
