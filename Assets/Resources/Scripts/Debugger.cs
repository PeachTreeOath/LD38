using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour
{

    public bool infiniteMoney;
    public float secsPerMeteor;

    void Start()
    {
        ShopManager.Instance.debugOn = infiniteMoney;
        if(secsPerMeteor != 0)
        {
            GameObject.Find("MeteorSpawner").GetComponent<MeteorSpawner>().secondsPerMeteor = secsPerMeteor;
        }
    }
}