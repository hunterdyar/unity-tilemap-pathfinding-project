using NavigationTiles.Utility;
using UnityEngine;

namespace NavigationTiles.Pathfinding
{
	public class AstarHexPathfinder : AStarPathfinder
	{
		public AstarHexPathfinder(TilemapNavigation tilemapNavigation) : base(tilemapNavigation)
		{
		}

		public override int Heuristic(Vector3Int a, Vector3Int b, int stepUpLayerCost = 1)
		{
			return HexUtility.CubeDistance(a, b);
		}
	}
}