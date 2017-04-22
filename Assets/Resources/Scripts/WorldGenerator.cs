using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public int depth;
    public int radius;
    public float scale;
    public float swissCheeseDensity;
    public float swissCheeseHoleSizes;

    private bool swissCheeseXGenning = false;
    private bool swissCheeseYGenning = false;
    private int swissCheeseYValue = -1;

    public GameObject dirt;

    // Use this for initialization
    void Start()
    {
        dirt.transform.localScale = new Vector3(dirt.transform.localScale.x * scale,
            dirt.transform.localScale.y * scale, 1.0f);
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
                if (Random.Range(0f, 1f) < swissCheeseDensity)
                {
                    //gen holes
                    swissCheeseXGenning = true;
                    swissCheeseYGenning = true;
                    swissCheeseYValue = y;
                }

                bool genTile = true;
                if (swissCheeseXGenning && Random.Range(0f, 1f) < swissCheeseHoleSizes)
                {
                    genTile = false;
                }
                else
                {
                    swissCheeseYGenning = false;
                    swissCheeseXGenning = false;
                    swissCheeseYValue = -1;
                }

                if (genTile == true)
                {
                    GameObject goRight = Instantiate<GameObject>(dirt, transform);
                    goRight.transform.position = new Vector3(dirt.transform.position.x + (x * scale), dirt.transform.position.y + (y * scale));

                    GameObject goLeft = Instantiate<GameObject>(dirt, transform);
                    goLeft.transform.position = new Vector3(dirt.transform.position.x - (x * scale), dirt.transform.position.y + (y * scale));
                }
            }
        }
    }
}
