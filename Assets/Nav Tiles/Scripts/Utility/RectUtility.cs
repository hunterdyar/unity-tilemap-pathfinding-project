using UnityEngine;

namespace NavigationTiles.Utility
{
	public static class RectUtility
	{
		public static readonly Vector3Int[] CardinalDirections = new[]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, -1, 0),
		};

		public static readonly Vector3Int[] CardinalAndDiagonalDirections = new[]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(1, 1, 0),
			new Vector3Int(-1, 1, 0),
			new Vector3Int(-1, -1, 0),
			new Vector3Int(1, -1, 0),
		};
	}
}