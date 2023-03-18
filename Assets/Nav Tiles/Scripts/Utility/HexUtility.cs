using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NavigationTiles.Utility
{
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
		
		//Getting Positions
		public static Vector3Int[] GetPositionsInRange(Vector3Int center, int range)
		{
			List<Vector3Int> total = new List<Vector3Int>();
			for (int x = -range; x <= range; x++)
			{
				for (int y = Mathf.Max(-range, -x - range); y <= Mathf.Min(range, -x + range); y++)
				{
					var z = -x - y;
					var pos = new Vector3Int(x, y, z);
					total.Add(center + pos);
				}
			}

			return total.ToArray();
		}

		public static List<Vector3Int> GetPositionsInRing(Vector3Int center, int minRadius, int maxRadius)
		{
			//This is certainly not the most efficient way to do this. But it is 3 lines of code.
			var all = GetPositionsInRange(center, maxRadius);
			var hub = GetPositionsInRange(center, minRadius - 1);
			return all.Where(x => !hub.Contains(x)).ToList();
		}

		public static Vector3Int[] GetPositionsOnLine(Vector3Int a, Vector3Int b)
		{
			int N = CubeDistance(a, b);
			List<Vector3Int> results = new List<Vector3Int>();
			for (int i = 0; i <= N; i++)
			{
				results.Add(CubeRound(CubeLerp(a, b, 1.0f / N * i)));
			}

			return results.ToArray();
		}

		public static Vector3Int CubeLerp(Vector3Int a, Vector3Int b, float t)
		{
			t = Mathf.Clamp01(t);
			var unroundedCubePos = new Vector3(
				Mathf.Lerp(a.x, b.x, t),
				Mathf.Lerp(a.y, b.y, t),
				Mathf.Lerp(a.z, b.z, t)
			);
			return CubeRound(unroundedCubePos);
		}

		public static Vector3Int CubeRound(Vector3 pos)
		{
			var rx = Mathf.RoundToInt(pos.x);
			var ry = Mathf.RoundToInt(pos.y);
			var rz = Mathf.RoundToInt(pos.z);

			var x_diff = Mathf.Abs(rx - pos.x);
			var y_diff = Mathf.Abs(ry - pos.y);
			var z_diff = Mathf.Abs(rz - pos.z);

			if (x_diff > y_diff && x_diff > z_diff)
			{
				rx = -ry - rz;
			}
			else if (y_diff > z_diff)
			{
				ry = -rx - rz;
			}
			else
			{
				rz = -rx - ry;
			}

			return new Vector3Int(rx, ry, rz);
		}
	}
}