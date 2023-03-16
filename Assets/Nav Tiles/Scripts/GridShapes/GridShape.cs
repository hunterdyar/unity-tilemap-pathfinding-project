using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NavigationTiles.GridShapes
{
	[CreateAssetMenu(fileName = "Grid Shape", menuName = "Nav Tiles/Grid Shape", order = 0)]
	public class GridShape : ScriptableObject
	{
		public List<Vector2Int> Shape => _shape;
		[SerializeField] private List<Vector2Int> _shape;
		public BoundsInt GridBounds => GetShapeBounds();

		private BoundsInt GetShapeBounds()
		{
			BoundsInt bounds = new BoundsInt();
			int minX = 0;
			int minY = 0;
			int maxX = 0;
			int maxY = 0;
			foreach (var pos in _shape)
			{
				minX = Mathf.Min(minX, pos.x);
				maxX = Mathf.Max(maxX, pos.x);
				
				minY = Mathf.Min(minY, pos.y);
				maxY = Mathf.Max(maxY, pos.y);
			}
			bounds.SetMinMax(new Vector3Int(minX,minY,0),new Vector3Int(maxX,maxY,0));
			return bounds;
		}

		public void ToggleCell(Vector2Int pos)
		{

#if UNITY_EDITOR
				Undo.RecordObject(this, "Toggle Cell");
#endif
				// reassemble
				if (_shape.Contains(pos))
				{
					_shape.Remove(pos);
				}
				else
				{
					_shape.Add(pos);
				}
#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
#endif
		}
		
		
		//Default shape looks up. Looking right, down, left, or up.
		// public static List<Vector2Int> GetShapeInFacingDirection(Vector2Int facing, bool flipInsteadOfRotate)
		// {
		// 	//
		// 	
		// }
	}
}