using System.Collections.Generic;
using System.IO;

namespace NavigationTiles.Pathfinding
{
	public abstract class Pathfinder
	{
		protected readonly Dictionary<NavNode, NavNode> cameFrom = new Dictionary<NavNode, NavNode>();
		protected TilemapNavigation tilemap;
		public Pathfinder(TilemapNavigation navigation)
		{
			tilemap = navigation;
		}

		public abstract List<NavNode> FindPath(NavNode start, NavNode end);

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