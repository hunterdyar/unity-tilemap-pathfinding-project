using NavigationTiles.Utility;
using UnityEngine;

namespace NavigationTiles.Pathfinding
{
	public class AstarHexPathfinder : AStarPathfinder
	{
		public AstarHexPathfinder(IGraph graph) : base(graph)
		{
		}
		//What is, performance-wise, the fastest way to change Heuristics? I think it's this.
		//how much overhead does a delegate add? Basically the same, if not faster, than interfaces I think I heard once (and just googled: yeah abouts)
		//
		public override int Heuristic(Vector3Int a, Vector3Int b, int stepUpLayerCost = 1)
		{
			return HexUtility.CubeDistance(a, b);
		}
	}
}