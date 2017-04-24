using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    [Header("Planet Gen")]
    public string planetName;
    public WorldGenerator.PlanetType WorldType;
    public int depth = 25;
    public int radius = 60;
    public float holeDensity = 0.01f;
    public float holeWidth = 0.6f;
    public float holeHeight = 0.97f;
    public float lavaPercent = 0.3f;
    public float stonePercent = 0.3f;
    public float dirtPercent = 0.35f;
    public float grassPercent = 0.35f;

    [Header("Asteroid Gen")]
    public float speedMin = 3;
    public float speedMax = 8;
    public float secondsPerMeteor = 0.5f;
    public float torqueRange = 25;
    //public float spawnerWidth = 80; // Doing equation now
    public float spawnerAngleRange = 45;
    public float maxBlastRadius = 3;

    [Header("Level Gen")]
    public int shipPartCost = 100;
}
