using System.Collections.Generic;
using System.IO;
using UnityEngine.Assertions;

namespace NavigationTiles.Pathfinding
{
	public abstract class Pathfinder
	{
		protected readonly Dictionary<NavNode, NavNode> cameFrom = new Dictionary<NavNode, NavNode>();
		
		
		protected IGraph tilemap;
		public PathStatus PathStatus => _pathStatus;
		protected PathStatus _pathStatus = PathStatus.Initiated;
		/// <summary>
		/// A copy of the most recently calculated path.
		/// </summary>
		public List<NavNode> LatestPath => _latestPath;
		private List<NavNode> _latestPath;
		
		/// <summary>
		/// Construct a new pathfinder
		/// </summary>
		/// <param name="graph">Graph is probably the TilemapNavigation component, but you can implement your own IGraph and use the pathfinder (with NavNodes)</param>
		public Pathfinder(IGraph graph)
		{
			tilemap = graph;
		}

		public List<NavNode> FindPath(NavNode start, NavNode end)
		{
			if (TryFindPath(start, end, out var path))
			{
				return path;
			}
			else
			{
				//return null or return new empty list?
				//As a rule of thumb, one shouldn't rely on using !=null as case handling. 
				//If you are just calling this function and then doing a null check, you should probably use TryFindPath instead.
				return null;
			}
		}
		public abstract bool TryFindPath(NavNode start, NavNode end, out List<NavNode> path);

		public List<NavNode> GetPath(NavNode end)
		{
			if (_pathStatus != PathStatus.PathFound)
			{
				return new List<NavNode>();
			}
			
			var path = new List<NavNode>();
			var current = end;
			//we set start=start.
			while (current != cameFrom[current])
			{
				path.Add(current);
				current = cameFrom[current];
			}

			path.Reverse();
			_latestPath = path;
			//we could reset pathStatus back to idle/initiated here, but it would be helpful to read last pathstatus.
			return path;
		}
	}
}