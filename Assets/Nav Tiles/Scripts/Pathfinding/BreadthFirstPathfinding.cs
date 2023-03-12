using System.Collections.Generic;

namespace Nav_Tiles.Scripts.Pathfinding
{
	public class BreadthFirstPathfinding : IPathfinder
	{
		private TilemapNavigation _tilemap;
		private Dictionary<NavNode, NavNode> cameFrom;
		public BreadthFirstPathfinding(TilemapNavigation tilemapNavigation)
		{
			_tilemap = tilemapNavigation;
		}
		public List<NavNode> FindPath(NavNode start, NavNode end)
		{
			 cameFrom = new Dictionary<NavNode, NavNode>();
			
			var frontier = new Queue<NavNode>();
			frontier.Enqueue(start);
			cameFrom[start] = start;
			var reached = new HashSet<NavNode>();
			reached.Add(start);

			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();

				foreach (var next in _tilemap.GetNeighborNodes(current))
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

		public List<NavNode> GetPath(NavNode end)
		{
			var path = new List<NavNode>();
			bool getting = true;
			var current = end;
			//we set start=start.
			while (current != cameFrom[current])
			{
				path.Add(current);
				current = cameFrom[current];
			}

			path.Reverse();
			return path;
		}
	}
}