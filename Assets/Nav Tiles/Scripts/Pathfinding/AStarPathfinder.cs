using System.Collections.Generic;
using Nav_Tiles.Scripts;
using Nav_Tiles.Scripts.Utility;
using Priority_Queue;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NavigationTiles.Pathfinding
{
	/// <summary>
	/// A* pathfinding using (by default) a manhattan distance Heuristic that works on any square (or square-ish) map.
	/// </summary>
	public class AStarPathfinder : Pathfinder
	{
		private readonly Dictionary<NavNode, int> costSoFar = new Dictionary<NavNode, int>();
		private readonly SimplePriorityQueue<NavNode> frontier = new SimplePriorityQueue<NavNode>();
		public AStarPathfinder(TilemapNavigation tilemapNavigation) : base(tilemapNavigation)
		{
		}

		public override List<NavNode> FindPath(NavNode start, NavNode end)
		{
			costSoFar.Clear();
			costSoFar[start] = 0;
			cameFrom.Clear();
			cameFrom[start] = start;
			frontier.Clear();

			frontier.Enqueue(start, 0);
			
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();

				if (current == end)
				{
					break;
				}

				foreach (var next in tilemap.GetNeighborNodes(current))
				{
					int newCost = costSoFar[current] + next.WalkCost; //cost algorithm generalized somewhere

					if (!costSoFar.ContainsKey(next))
					{
						costSoFar[next] = newCost;
						int priority = newCost + Heuristic(end,next);
						cameFrom[next] = current;
						frontier.Enqueue(next, priority);
					}else if (newCost < costSoFar[next])
					{
						costSoFar[next] = newCost;
						int priority = newCost + Heuristic(end, next);
						cameFrom[next] = current;
						frontier.UpdatePriority(next, priority);
					}
				}
			}

			return GetPath(end);
		}

		

		//overload for convenience
		public virtual int Heuristic(NavNode a, NavNode b, int stepUpLayerCost = 1)
		{
			return Heuristic(a.NavPosition, b.NavPosition,stepUpLayerCost);
		}

		//Manhattan Distance. StepCost is multiplier for going up or down on z.
		public virtual int Heuristic(Vector3Int a, Vector3Int b, int stepUpLayerCost = 1)
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + stepUpLayerCost*Mathf.Abs(a.z - b.z);
		}
	}
}