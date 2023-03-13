using UnityEngine;

namespace NavigationTiles.Utility
{
	//todo: i've done this before, copy over: https://raw.githubusercontent.com/hunterdyar/HopLiteClone/master/Assets/Scripts/Hex/HexUtil.cs
	public static class HexUtility
	{
		public static readonly Vector3Int[] CubeHexDirections = new[]
		{
			new Vector3Int(1, 0, -1),
			new Vector3Int(1, -1, 0),
			new Vector3Int(0, -1, 1),
			new Vector3Int(-1, 0, 1),
			new Vector3Int(-1, 1, 0),
			new Vector3Int(0, 1, -1),
		};

		public static int CubeDistance(Vector3Int a, Vector3Int b)
		{
			return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
		}

		public static int CubeDistance(NavNode a, NavNode b)
		{
			return CubeDistance(a.NavPosition, b.NavPosition);
		}

		public static int CubeDistance(Vector3Int a, NavNode b)
		{
			return CubeDistance(a, b.NavPosition);
		}
		
		//x = q, y == r, z == -q-r
		
		/// <summary>
		/// Unity Grid, Point-Top uses OddR 
		/// </summary>
		public static Vector3Int CubeToOddR(Vector3Int cube)
		{
			if (cube.x + cube.y + cube.z != 0)
			{
				Debug.LogWarning("Invalid Cube Coordinate Given");
			}
			var col = cube.x + (cube.y - (cube.y & 1)) / 2;
			var row = cube.y;
			return new Vector3Int(col, row, 0);
		}

		public static Vector3Int OddRToCube(Vector3Int hex)
		{
			var q = hex.x - (hex.y - (hex.y & 1)) / 2;
			var r = hex.y;
			return new Vector3Int(q, r, -q - r);
		}
		

		/// <summary>
		/// Unity Grid, Flat-Top hex grids use OddQ coordinates
		/// </summary>
		public static Vector3Int CubeToOddQ(Vector3Int cubePos)
		{
			var col = cubePos.x;
			var row = cubePos.y + (cubePos.x - (cubePos.x & 1)) / 2;
			return new Vector3Int(col, row,0);
		}
		public static Vector3Int OddQToCube(Vector3Int pos)
		{
			var q = pos.x;
			var r = pos.y - (pos.x - (pos.x & 1)) / 2;
			return new Vector3Int(q, r, -q - r);
		}
	}
}