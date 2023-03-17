using NavigationTiles.GridShapes;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GridShape))]
public class GridShapeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var grid = (GridShape)target;

		EditorGUILayout.BeginVertical();
		
		var bounds = grid.GetShapeBounds(true);
		
		for (int y = bounds.yMax+1; y >= bounds.yMin-1; y--)
		{
			GUILayout.BeginHorizontal();
			for (int x = bounds.xMin-1; x <= bounds.xMax+1; x++)
			{
				bool has = grid.Shape.Contains(new Vector2Int(x, y));
				bool isOrigin = false;
				var cell = has ? "X" : " ";
				GUI.color = Color.gray;
				
				if (has)
				{
					GUI.color = Color.red;
				}

				if (x == 0 && y == 0)
				{
					isOrigin = true;
					GUI.color = Color.green;
					cell = has ? "X" : "0";
				}

				if (GUILayout.Button(cell, GUILayout.Width(20)))
				{
					GUI.enabled = !isOrigin;
					grid.ToggleCell(new Vector2Int(x,y));
					GUI.enabled = true;
				}
			}

			GUILayout.EndHorizontal();
		}
		GUI.color = Color.white;

		EditorGUILayout.EndVertical();

		DrawDefaultInspector();
	}
}
