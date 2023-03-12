using System.Collections.Generic;
using System.Linq;
using Nav_Tiles.Scripts.Utility;
using Unity.Mathematics;
using UnityEngine;

namespace Nav_Tiles.Scripts.Pathfinding
{
	public class AStarPathfinder : IPathfinder
	{

		private Dictionary<NavNode, NavNode> _cameFrom = new Dictionary<NavNode, NavNode>();
		private Dictionary<NavNode, int> _costSoFar = new Dictionary<NavNode, int>();
		private Dictionary<NavNode, int> _priorityCosts = new Dictionary<NavNode, int>();

		private TilemapNavigation _navMap;
		public AStarPathfinder(TilemapNavigation tilemapNavigation)
		{
			_navMap = tilemapNavigation;
		}
		
		public List<NavNode> FindPath(NavNode start, NavNode end)
		{
			//im not actually sure this will work for the heuristic.
			var frontier = new PriorityQueue<NavNode,int>();
			_cameFrom.Clear();
			_costSoFar.Clear();
			
			//Add initial node
			_costSoFar.Add(start,0);
			_cameFrom.Add(start,start);
			frontier.Enqueue(start,0);
			if (start == end)
			{
				return new List<NavNode>(new []{start});
			}
			
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current == end)
				{
					break;
				}

				foreach (var next in _navMap.GetNeighborNodes(current))
				{
					int newCost = _costSoFar[current] + (next.WalkCost);//just gonna use walkCost.
					if (!_costSoFar.ContainsKey(next) || newCost < _costSoFar[next])
					{
						_costSoFar[next] = newCost;
						int priority = newCost + Heuristic(next, end);
						frontier.Enqueue(next,priority);
						_cameFrom[next] = current;
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
			while (current != _cameFrom[current])
			{
				path.Add(current);
				current = _cameFrom[current];
			}

			path.Reverse();
			return path;
		}

		//overload for convenience
		public static int Heuristic(NavNode a, NavNode b, int stepCost = 1)
		{
			return Heuristic(a.location, b.location,stepCost);
		}

		//Manhattan Distance. StepCost is multiplier for going up or down on z.
		public static int Heuristic(Vector3Int a, Vector3Int b, int stepCost = 1)
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + stepCost*Mathf.Abs(a.z - b.z);
		}
	}
}