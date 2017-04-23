using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldGenerator : MonoBehaviour {

    public enum PlanetType { Earth, Mars,Jupitor, Pluto}
    public PlanetType Type = PlanetType.Earth;
    public int depth;
    public int radius;
    public float scale;
    public float swissCheeseDensity;
    public float swissCheeseHoleWidth;
    public float swissCheeseHoleHeight;
    public float lavaPercent;
    public float stonePercent;
    public float dirtPercent;
    public float grassPercent;

    private bool swissCheeseXGenning = false;
    private bool swissCheeseYGenning = false;
    private HashSet<int> swissCheeseYValues = new HashSet<int>();

    private float playerStartYOffset = 11f;


    public GameObject dirt;
    public GameObject background;
    public GameObject cloudOverlay;
    public List<Sprite> EarthBlockTypes;
    public List<Sprite> MarsBlockTypes;
    public List<Sprite> PlutoBlockTypes;
    public List<Sprite> JupitorBlockTypes;
    
    [HideInInspector]
	public List<GameObject> worldTiles;
    [HideInInspector]
    public List<GameObject> worldEntities;
	Vector3 lastPos;
	PlayerController pController;

    // Use this for initialization
    void Start()
    {
        
        dirt.transform.localScale = new Vector3(dirt.transform.localScale.x * scale,
            dirt.transform.localScale.y * scale, 1.0f);
        // background = GameObject.Find("Background");
        cloudOverlay = GameObject.Find("CloudOverlay");

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
		//worldTiles.Add(GameObject.Find("Backpack"));
        worldEntities.Add(GameObject.Find("RocketTest"));

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
                    switch(Type)
                    {
                        case PlanetType.Earth:
                            AssignBlockType(EarthBlockTypes,y, goRight);
                            break;
                        case PlanetType.Mars:
                            AssignBlockType(MarsBlockTypes, y, goRight);
                            break;
                        case PlanetType.Jupitor:
                            AssignBlockType(JupitorBlockTypes, y, goRight);
                            break;
                        case PlanetType.Pluto:
                            AssignBlockType(PlutoBlockTypes, y, goRight);
                            break;
                            
                        default:
                            AssignBlockType(EarthBlockTypes, y, goRight);
                            break;
                    }
                }
            }
        }

        Sprite backgroundSprite = new Sprite();
        Sprite cloudSprite = new Sprite();
        switch (Type)
        {
            case PlanetType.Earth:
                backgroundSprite = EarthBlockTypes[4];
                cloudSprite = EarthBlockTypes[5];
                break;
            case PlanetType.Mars:
                backgroundSprite = MarsBlockTypes[4];
                cloudSprite = MarsBlockTypes[5];
                break;
            case PlanetType.Jupitor:
                backgroundSprite = JupitorBlockTypes[4];
                cloudSprite = JupitorBlockTypes[5];
                break;
            case PlanetType.Pluto:
                backgroundSprite = PlutoBlockTypes[4];
                cloudSprite = PlutoBlockTypes[5];
                break;

            default:
                backgroundSprite = EarthBlockTypes[4];
                cloudSprite = EarthBlockTypes[5];
                break;
        }
        //Background Sky
        background.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
        background.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = backgroundSprite;
        //Clouds
        cloudOverlay.GetComponent<SpriteRenderer>().sprite = cloudSprite;
        cloudOverlay.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cloudSprite;
    }

    Sprite AssignBlockType(List<Sprite> blockTypes, int index, GameObject go)
    {
        //Add y variation
        if (index != 0 && index < 18)
        {
            index += Random.Range(0, 5);
        }

        Sprite sprite = new Sprite();
        if( index == depth -1) 
        {
            //Grass
            sprite = blockTypes[3];
            go.GetComponent<Block>().Health = 1;
        }
        else if (index < lavaPercent * depth)
        {
            //Lava
            sprite = blockTypes[0];
            go.GetComponent<Block>().isIndestructable = true;
            go.GetComponent<Block>().Type = Block.BlockType.Damage;
            go.GetComponent<Block>().Health = 1;
        }
        else if(index < (lavaPercent + stonePercent) * depth)
        {
            //Stone
            sprite = blockTypes[1];
            go.GetComponent<Block>().Health = 3;
        }
        else if(index < (lavaPercent + stonePercent + dirtPercent) * depth)
        {
            //Dirt
            sprite = blockTypes[2];
            go.GetComponent<Block>().Health = 1;
        }
        else 
        {
            //Grass
            sprite = blockTypes[3];
            go.GetComponent<Block>().Health = 1;
        }

        

        go.GetComponent<SpriteRenderer>().sprite = sprite;
        return sprite;
    }

    void WrapEntities()
    {

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
				temp.transform.position += Vector3.right * ((rightDist + onDeckRadius) + .5f);
			}else
			{
				GameObject worldLeftMost = GetLeftMost(worldTiles);
				GameObject onDeckRightMost = GetRightMost(onDeck);
				float onDeckRadius = onDeckRightMost.transform.position.x - temp.transform.position.x;
				float leftDist = temp.transform.position.x - worldLeftMost.transform.position.x;
				temp.transform.position += Vector3.left * ((leftDist + onDeckRadius) + .5f) ;
			}

			for(int i = 0; i < onDeck.Count; i++)
			{
				onDeck[i].transform.parent = oldParent.transform;
			}

			Destroy(temp);

			lastPos = Globals.playerObj.transform.position;
		}
	}

	public GameObject GetRightMost(List<GameObject> tiles)
	{
		GameObject rMost = Globals.playerObj;
		for(int  i = 0; i < tiles.Count; i++)
		{
			if(tiles[i] != null &&
				tiles[i].tag.Equals("Ground"))
			{
				if(rMost.transform.position.x < tiles[i].transform.position.x)
				{
					rMost = tiles[i];
				}
			}
		}

		return rMost;
	}

	public GameObject GetLeftMost(List<GameObject> tiles)
	{
		GameObject lMost = Globals.playerObj;
		for(int  i = 0; i < tiles.Count; i++)
		{
			if(tiles[i] != null &&
				tiles[i].tag.Equals("Ground"))
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
