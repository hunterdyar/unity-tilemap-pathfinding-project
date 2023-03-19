using NavigationTiles.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(EntityMap))]
public class EntityMapEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var map = (EntityMap)target;

		if (EditorApplication.isPlaying)
		{
			EditorGUILayout.HelpBox($"There are {map.Count} entities on the map.", MessageType.Info);
		}else if (EditorApplication.isPaused)
		{
			EditorGUILayout.HelpBox($"There are {map.Count} entities on the map.", MessageType.Info);

		}
		base.OnInspectorGUI();
	}
}
