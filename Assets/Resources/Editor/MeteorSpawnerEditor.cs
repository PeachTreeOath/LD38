using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeteorSpawner))]
public class MeteorSpawnerEditor : Editor {

	public override void OnInspectorGUI(){
		MeteorSpawner spawner = (MeteorSpawner)target;

		EditorGUILayout.LabelField("Meteor Speed", EditorStyles.boldLabel);
		spawner.speedMin = EditorGUILayout.FloatField("Min", spawner.speedMin);
		spawner.speedMax = EditorGUILayout.FloatField("Max", spawner.speedMax);
		EditorGUILayout.MinMaxSlider(ref spawner.speedMin, ref spawner.speedMax, 10, 50);

		EditorGUILayout.LabelField("Spawner Details", EditorStyles.boldLabel);
		spawner.spawnerWidth = EditorGUILayout.Slider("Width", spawner.spawnerWidth, 0, 20);
		spawner.secondsPerMeteor = EditorGUILayout.Slider("Seconds/Meteor", spawner.secondsPerMeteor, 0, 2);
		spawner.torqueRange = EditorGUILayout.Slider("Max Torque", spawner.torqueRange, 0, 50);
		spawner.spawnerAngleRange = EditorGUILayout.Slider("Max Angle", spawner.spawnerAngleRange, 0, 6);
		spawner.maxBlastSize = EditorGUILayout.Slider("Max Blast Size", spawner.maxBlastSize, 1, 5);

		EditorUtility.SetDirty(spawner);
	}
}
