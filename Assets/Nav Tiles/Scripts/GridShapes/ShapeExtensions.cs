using System.Collections.Generic;
using UnityEngine;

namespace NavigationTiles.GridShapes
{
	public static class ShapeExtensions
	{
		public static BoundsInt GetShapeBounds(this List<Vector2Int> shape, bool includeOrigin = false)
		{
			var bounds = new BoundsInt();
			//starting at 0 basically means that the origin is included in bounds calculation. This is important for the editor tool when the list is empty.
			var minX = 0;
			var minY = 0;
			var maxX = 0;
			var maxY = 0;

			if (!includeOrigin && shape.Count > 0)
			{
				minX = shape[0].x;
				maxX = shape[0].x;
				minY = shape[0].y;
				maxY = shape[0].y;
			}
			else if (!includeOrigin) //_shape is empty.
			{
				Debug.LogWarning("Can't get shape bounds. No items in shape and origin not included.");
			}

			foreach (var pos in shape)
			{
				minX = Mathf.Min(minX, pos.x);
				maxX = Mathf.Max(maxX, pos.x);

				minY = Mathf.Min(minY, pos.y);
				maxY = Mathf.Max(maxY, pos.y);
			}

			bounds.SetMinMax(new Vector3Int(minX, minY, 0), new Vector3Int(maxX, maxY, 0));
			return bounds;
		}

		public static void Toggle(this List<Vector2Int> shape, Vector2Int pos)
		{
			if (shape.Contains(pos))
			{
				shape.Remove(pos);
			}
			else
			{
				shape.Add(pos);
			}
		}
	}
}