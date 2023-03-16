using System.Collections;
using System.Collections.Generic;
using NavigationTiles.PriorityQueue;
using UnityEngine;

namespace NavigationTiles.Pathfinding
{
	//Dijkstra's Algorithm
	public class DJPathfinder<T> : Pathfinder<T> where T : INode
	{
		private Dictionary<T,int> _costSoFar;

		public DJPathfinder(IGraph graph) : base(graph)
		{
		}

		public override bool TryFindPath(T start, T end, out List<T> path)
		{
			_pathStatus = PathStatus.Searching;
			_costSoFar = new Dictionary<T, int>();
			_costSoFar[start] = 0;
			cameFrom.Clear();
			cameFrom[start] = start;
			
			
			var frontier = new SimplePriorityQueue<T>();
			frontier.Enqueue(start,0);

			var reached = new HashSet<T>();
			reached.Add(start);

			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				
				if (Equals(current, end))
				{
					_pathStatus = PathStatus.PathFound;
					break;
				}

				foreach (T next in tilemap.GetNeighborNodes(current))
				{
					int newCost = _costSoFar[current] + next.WalkCost;//cost algorithm generalized somewhere
					//reached is only used because our temp priority queue implementation doesn't have "contains" for checking the frontier. We could check costsofar tho. 
					if (!reached.Contains(next) || newCost < _costSoFar[next])
					{
						_costSoFar[next] = newCost;
						int priority = newCost;
						frontier.Enqueue(next,priority);
						reached.Add(next);
						cameFrom[next] = current;
					}
				}
			}

			if (_pathStatus == PathStatus.Searching)
			{
				_pathStatus = PathStatus.NoPathFound;
			}
			path = GetPath(end);
			return _pathStatus == PathStatus.PathFound;
		}
	}
}