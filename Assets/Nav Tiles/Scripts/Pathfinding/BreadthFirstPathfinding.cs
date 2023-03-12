using System.Collections.Generic;

namespace NavigationTiles.Pathfinding
{
	/// <summary>
	/// Because one would only bother using this if they wanted to do something _else_ with the pathfinding (use astar instead), I've given the CameFrom dictionary a getter, and removed the early-exit case.
	/// </summary>
	public class BreadthFirstPathfinding : Pathfinder
	{
		public Dictionary<NavNode, NavNode> CameFrom => cameFrom;
		private Queue<NavNode> frontier = new Queue<NavNode>();
		public BreadthFirstPathfinding(TilemapNavigation tilemapNavigation) : base(tilemapNavigation)
		{
		}
		public override List<NavNode> FindPath(NavNode start, NavNode end)
		{
			cameFrom.Clear();
			frontier.Clear();
			
			frontier.Enqueue(start);
			cameFrom[start] = start;
			var reached = new HashSet<NavNode>();
			reached.Add(start);

			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();

				foreach (var next in tilemap.GetNeighborNodes(current))
				{
					if (!reached.Contains(next))
					{
						frontier.Enqueue(next);
						reached.Add(next);
						cameFrom[next] = current;
					}
				}
			}

			return GetPath(end);
		}
		
	}
}