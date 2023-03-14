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
		public BreadthFirstPathfinding(IGraph graph) : base(graph)
		{
		}
		public override bool TryFindPath(NavNode start, NavNode end, out List<NavNode> path)
		{
			_pathStatus = PathStatus.Searching;
			cameFrom.Clear();
			frontier.Clear();
			
			frontier.Enqueue(start);
			cameFrom[start] = start;
			var reached = new HashSet<NavNode>();
			reached.Add(start);

			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();

				//NO early exit. If you wanted a faster one, use DJ or a*. I don't know why you are using breadth-first, I can only imagine you want to take advantage of the cameFrom dictionary as a vector field for something.
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
			
			if (reached.Contains(end))
			{
				_pathStatus = PathStatus.PathFound;
			}
			else
			{
				_pathStatus = PathStatus.NoPathFound;
			}

			path = GetPath(end);
			return _pathStatus == PathStatus.PathFound;
		}
		
	}
}