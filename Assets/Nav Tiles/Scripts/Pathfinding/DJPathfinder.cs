using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

namespace NavigationTiles.Pathfinding
{
	//Dijkstra's Algorthim
	public class DJPathfinder : Pathfinder
	{
		private Dictionary<NavNode,int> _costSoFar;

		public DJPathfinder(TilemapNavigation navigation) : base(navigation)
		{
		}

		public override List<NavNode> FindPath(NavNode start, NavNode end)
		{
			_costSoFar = new Dictionary<NavNode, int>();
			_costSoFar[start] = 0;
			cameFrom.Clear();
			cameFrom[start] = start;
			
			
			var frontier = new SimplePriorityQueue<NavNode>();
			frontier.Enqueue(start,0);

			var reached = new HashSet<NavNode>();
			reached.Add(start);

			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				
				if (current == end)
				{
					break;
				}

				foreach (var next in tilemap.GetNeighborNodes(current))
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