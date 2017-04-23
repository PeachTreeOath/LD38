using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeteorSpawner))]
public class MeteorSpawnerEditor : Editor {

	public override void OnInspectorGUI(){
		MeteorSpawner spawner = (MeteorSpawner)target;

		EditorGUILayout.LabelField("Meteor Details", EditorStyles.boldLabel);
		spawner.meteorFab = (GameObject) EditorGUILayout.ObjectField("Meteor Prefab", 
			spawner.meteorFab, typeof(GameObject), true);
		spawner.speedMin = EditorGUILayout.FloatField("Min Speed", spawner.speedMin);
		spawner.speedMax = EditorGUILayout.FloatField("Max Speed", spawner.speedMax);
		EditorGUILayout.MinMaxSlider(ref spawner.speedMin, ref spawner.speedMax, 0, 30);
		spawner.torqueRange = EditorGUILayout.Slider("Max Torque", spawner.torqueRange, 0, 100);
		spawner.spawnerAngleRange = EditorGUILayout.Slider("Max Angle (Deg)", spawner.spawnerAngleRange, 0, 90);
		spawner.maxBlastSize = EditorGUILayout.Slider("Max Blast Size", spawner.maxBlastSize, 1, 5);

		EditorGUILayout.LabelField("Spawner Details", EditorStyles.boldLabel);
		spawner.spawnerWidth = EditorGUILayout.Slider("Width", spawner.spawnerWidth, 0, 20);
		spawner.secondsPerMeteor = EditorGUILayout.Slider("Seconds/Meteor", spawner.secondsPerMeteor, 0, 2);


		EditorUtility.SetDirty(spawner);
	}
}
