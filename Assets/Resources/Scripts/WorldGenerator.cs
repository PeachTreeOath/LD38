using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public int depth;
    public int radius;

    public GameObject dirt;

    // Use this for initialization
    void Start()
    {
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateWorld()
    {
        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < radius; x++)
            {
                GameObject goRight = Instantiate<GameObject>(dirt, transform);
                goRight.transform.position = new Vector3(dirt.transform.position.x + x, dirt.transform.position.y + y);

                GameObject goLeft = Instantiate<GameObject>(dirt, transform);
                goLeft.transform.position = new Vector3(dirt.transform.position.x - x, dirt.transform.position.y + y);
            }
        }
    }
}
