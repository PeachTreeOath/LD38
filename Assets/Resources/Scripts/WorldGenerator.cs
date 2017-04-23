using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldGenerator : MonoBehaviour {

    public int depth;
    public int radius;
    public float scale;
    public float swissCheeseDensity;
    public float swissCheeseHoleWidth;
    public float swissCheeseHoleHeight;

    private bool swissCheeseXGenning = false;
    private bool swissCheeseYGenning = false;
    private HashSet<int> swissCheeseYValues = new HashSet<int>();

    private float playerStartYOffset = 11f;


    public GameObject dirt;
    public List<Sprite> BlockTypes;
	List<GameObject> worldTiles;
	Vector3 lastPos;
	PlayerController pController;

    // Use this for initialization
    void Start()
    {
        
        dirt.transform.localScale = new Vector3(dirt.transform.localScale.x * scale,
            dirt.transform.localScale.y * scale, 1.0f);
        GenerateWorld();
		lastPos = Globals.playerObj.transform.position;
		pController = Globals.playerObj.GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
		WrapWorld();
    }

    void GenerateWorld()
    {
        float xOffset = -scale * radius;
        float coreYOffset = playerStartYOffset - scale * depth;
        worldTiles = new List<GameObject>();
		worldTiles.Add(GameObject.Find("Backpack"));
        for (int y = 0; y < depth; y++)
        {
            
            for (int x = 0; x < radius*2; x++)
            {
                bool newHole = Random.Range(0f, 1f) < swissCheeseDensity;
                bool test = swissCheeseYValues.Contains(x);
                if (swissCheeseYValues.Contains(x) || newHole)
                {
                    
                    swissCheeseYValues.Remove(x);
                    //gen holes
                    swissCheeseXGenning = true;
                    swissCheeseYGenning = true;
                    if (newHole || Random.Range(0f, 1f) < swissCheeseHoleHeight)
                        swissCheeseYValues.Add(x);
                }

                bool genTile = true;
                if (swissCheeseXGenning && Random.Range(0f, 1f) < swissCheeseHoleWidth)
                {
                    genTile = false;
                }
                else
                {
                    swissCheeseYGenning = false;
                    swissCheeseXGenning = false;
                    
                }

                if (genTile == true || y < 3 || x < 2)
                {
                    Vector3 xOffsetPos = new Vector3(xOffset + transform.position.x, transform.position.y);
                    GameObject goRight = Instantiate<GameObject>(dirt, transform);
                    goRight.transform.position = new Vector3(dirt.transform.position.x + ((x-radius) * scale), dirt.transform.position.y + (coreYOffset + y * scale));
                    worldTiles.Add(goRight);


                    //Changes the texture ultilized based on hieght: Lava, dirt, rock, etc.
                    AssignBlockType(y, goRight);
                }
            }
        }
    }

    Sprite AssignBlockType(int index, GameObject go)
    {
        //Add y variation
        if (index != 0 && index < 18)
        {
            index += Random.Range(0, 5);
        }

        Sprite sprite = new Sprite();
        if(index < 5)
        {
            //Lava
            sprite = BlockTypes[0];
            go.GetComponent<Block>().isIndestructable = true;
            go.GetComponent<Block>().Type = Block.BlockType.Damage;
            go.GetComponent<Block>().Health = 1;
        }
        else if(index < 15)
        {
            //Stone
            sprite = BlockTypes[1];
            go.GetComponent<Block>().Health = 3;
        }
        else if(index < 19)
        {
            //Dirt
            sprite = BlockTypes[2];
            go.GetComponent<Block>().Health = 1;
        }
        else 
        {
            //Grass
            sprite = BlockTypes[3];
            go.GetComponent<Block>().Health = 1;
        }

        go.GetComponent<SpriteRenderer>().sprite = sprite;
        return sprite;
    }

	void WrapWorld()
	{
		float wrapDist = 30;
		float wrapUpdateDist = 5;
		if(Vector3.Distance(lastPos, Globals.playerObj.transform.position) > wrapUpdateDist)
		{
			bool left = true;
			List<GameObject> onDeck = new List<GameObject>();
			for(int i = worldTiles.Count-1; i >= 0; i--)
			{
				if(worldTiles[i] != null)
				{
					//positive dist tile is right else it's left
					float dist = worldTiles[i].transform.position.x - Globals.playerObj.transform.position.x;
					if(Mathf.Abs(dist) > wrapDist)
					{
						if((pController.GetFacing() == PlayerController.FacingEnum.LEFT && dist > 0))
						{
							onDeck.Add(worldTiles[i]);
						}
						else
						{
							if((pController.GetFacing() == PlayerController.FacingEnum.RIGHT && dist < 0))
							{
								left = false;
								onDeck.Add(worldTiles[i]);
							}
						}
					}
				}else
				{
					worldTiles.RemoveAt(i);
				}
			}

			GameObject oldParent = onDeck[0].transform.parent.gameObject;
			GameObject temp = new GameObject();
			GameObject leftMost = GetLeftMost(onDeck);
			GameObject rightMost = GetRightMost(onDeck);

			float xPos = leftMost.transform.position.x + (rightMost.transform.position.x - leftMost.transform.position.x)/2f;
			temp.transform.position = new Vector3( xPos, 0, leftMost.transform.position.z );
			for(int i = 0; i < onDeck.Count; i++)
			{
				onDeck[i].transform.parent = temp.transform;
			}

			if(!left)
			{
				GameObject worldRightMost = GetRightMost(worldTiles);
				GameObject onDeckLeftMost = GetLeftMost(onDeck);
				float onDeckRadius = temp.transform.position.x - onDeckLeftMost.transform.position.x;
				float rightDist = worldRightMost.transform.position.x - temp.transform.position.x;
				temp.transform.position += Vector3.right * (rightDist + onDeckRadius);
			}else
			{
				GameObject worldLeftMost = GetLeftMost(worldTiles);
				GameObject onDeckRightMost = GetRightMost(onDeck);
				float onDeckRadius = onDeckRightMost.transform.position.x - temp.transform.position.x;
				float leftDist = temp.transform.position.x - worldLeftMost.transform.position.x;
				temp.transform.position += Vector3.left * (leftDist + onDeckRadius);
			}

			for(int i = 0; i < onDeck.Count; i++)
			{
				onDeck[i].transform.parent = oldParent.transform;
			}

			Destroy(temp);

			lastPos = Globals.playerObj.transform.position;
		}
	}

	GameObject GetRightMost(List<GameObject> tiles)
	{
		GameObject rMost = Globals.playerObj;
		for(int  i = 0; i < tiles.Count; i++)
		{
			if(tiles[i] != null)
			{
				if(rMost.transform.position.x < tiles[i].transform.position.x)
				{
					rMost = tiles[i];
				}
			}
		}

		return rMost;
	}

	GameObject GetLeftMost(List<GameObject> tiles)
	{
		GameObject lMost = Globals.playerObj;
		for(int  i = 0; i < tiles.Count; i++)
		{
			if(tiles[i] != null)
			{
				if(lMost.transform.position.x > tiles[i].transform.position.x)
				{
					lMost = tiles[i];
				}
			}
		}

		return lMost;
	}
}
