using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {

	public static GameObject playerObj;
    public static GameObject Levels;

    //Level Variables
    public static int currentLevel = 0;
	public static int lastLevel = 0;

	public static bool ship1;
	public static bool ship2;
	public static bool ship3;

	public static int radarStat;
	public static int speedStat;
	public static int armorStat;
	public static int jumpStat;
	public static int magnetStat;
	public static int resourceStat;

	public static float startY;
}
