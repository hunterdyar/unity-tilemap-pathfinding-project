using UnityEngine;

namespace NavigationTiles.Utility
{
	public static class Extension
	{
		//Vector2Int and Vector3Int extensions
		public static Vector2Int FlipVertically(this Vector2Int point)
		{
			return new Vector2Int(point.x, -point.y);
		}
		public static Vector3Int FlipVertically(this Vector3Int point)
		{
			return new Vector3Int(point.x, -point.y,point.z);
		}

		public static Vector2Int FlipHorizontally(this Vector2Int point)
		{
			return new Vector2Int(-point.x, point.y);
		}

		public static Vector3Int FlipHorizontally(this Vector3Int point)
		{
			return new Vector3Int(-point.x, point.y, point.z);
		}

		/// <summary>
		/// Returns a Vector2Int as if Rotated 90 degrees counter clockwise around the origin.
		/// </summary>
		public static Vector2Int RotateLeft(this Vector2Int point)
		{
			//90 degrees = pi/2
			//x*cos(90d)-y*sin(90d)
			//xsin(90d) +y*cos(90d)
			//x = 0 -1y
			//y = x + 0
			return new Vector2Int(-point.y, point.x);
		}

		/// <summary>
		/// Returns a Vector3Int as if Rotated 90 degrees counter clockwise around the origin, around the z axis
		/// </summary>
		public static Vector3Int RotateLeft90AroundZ(this Vector3Int point)
		{
			return new Vector3Int(-point.y, point.x,point.z);
		}

		/// <summary>
		/// Returns a Vector2Int as if Rotated 90 degrees clockwise around the origin.
		/// </summary>
		public static Vector2Int RotateRight(this Vector2Int point)
		{
			return new Vector2Int(point.y, -point.x);
		}

		/// <summary>
		/// Returns a Vector3Int as if Rotated 90 degrees clockwise around the origin.
		/// </summary>
		public static Vector3Int RotateRight90AroundZ(this Vector3Int point)
		{
			return new Vector3Int(point.y, point.x,point.z);
		}

		/// <summary>
		/// Returns a Vector2Int as if Rotated 180 degrees around the origin.
		/// </summary>
		public static Vector2Int Rotate180(this Vector2Int point)
		{
			return new Vector2Int(-point.x, -point.y);
		}

		/// <summary>
		/// Returns a Vector3Int as if Rotated 180 degrees around the origin, around the z axis.
		/// </summary>
		public static Vector3Int Rotate180AroundZ(this Vector3Int point)
		{
			return new Vector3Int(-point.x, -point.y,point.z);
		}
	}
}