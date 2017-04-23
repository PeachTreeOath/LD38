using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{

    WorldGenerator world;
    MeteorSpawner meteorSpawner;

    // Use this for initialization
    void Awake()
    {
        Globals.Levels = gameObject;
        world = GameObject.Find("World").GetComponent<WorldGenerator>();
        meteorSpawner = GameObject.Find("MeteorSpawner").GetComponent<MeteorSpawner>();
    }

    public void GetCurrentLevel()
    {
        Level level = transform.GetChild(Globals.currentLevel).GetComponent<Level>();
        MapValues(level);
    }

    public void GetNextLevel()
    {
        Globals.currentLevel += 1;
        Level level = transform.GetChild(Globals.currentLevel).GetComponent<Level>();
        MapValues(level);
    }

    void MapValues(Level level)
    {
        meteorSpawner.speedMin = level.speedMin;
        meteorSpawner.speedMax = level.speedMax;
        meteorSpawner.secondsPerMeteor = level.secondsPerMeteor;
        meteorSpawner.torqueRange = level.torqueRange;
        meteorSpawner.spawnerWidth = level.radius * world.scale;
        meteorSpawner.spawnerAngleRange = level.spawnerAngleRange;
        meteorSpawner.maxBlastSize = level.maxBlastRadius;

        world.Type = level.WorldType;
        world.depth = level.depth;
        world.radius = level.radius;
        world.swissCheeseDensity = level.holeDensity;
        world.swissCheeseHoleWidth = level.holeWidth;
        world.swissCheeseHoleHeight = level.holeHeight;
        world.lavaPercent = level.lavaPercent;
        world.stonePercent = level.stonePercent;
        world.dirtPercent = level.dirtPercent;
        world.grassPercent = level.grassPercent;

        GameObject.Find("WorldNameText").GetComponent<Text>().text = "-" + level.WorldType.ToString() + "-";
        GameObject.Find("WorldCountText").GetComponent<Text>().text = level.planetName + " of 5";

        if (level.planetName.Equals("World 1"))
        {
            GameObject.Find("Tutorial").GetComponent<Image>().enabled = true;
        }
    }
}
