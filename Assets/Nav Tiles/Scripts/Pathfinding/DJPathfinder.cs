using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nav_Tiles.Scripts.Pathfinding
{
	public class DJPathfinder : IPathfinder
	{
		public List<NavNode> FindPath(NavNode start, NavNode finish)
		{
			Search(start,finish,10000000);//testing
			return path;
		}
		//Dijkstra's-ish algorithm basically does a blind flood-fill from the destination until the start tile is reached
		//keeping track of which tile flooded into the 'current' allows us to then reverse those directions and get the shortest path from start to end.

		private TilemapNavigation _tilemapNavigation;
		NavNode _cachedStart;
		public readonly List<NavNode> path = new List<NavNode>();
		public Dictionary<NavNode, int> Distances { get; set; } = new Dictionary<NavNode, int>();
		Dictionary<NavNode, NavNode> _cameFrom = new Dictionary<NavNode, NavNode>();
		
		public bool Running { get; private set; }

		public DJPathfinder(TilemapNavigation tilemapNavigation)
		{
			_tilemapNavigation = tilemapNavigation;
		}

		public void Search(NavNode start, NavNode end, int iterationsPerFrame)
		{
			_tilemapNavigation.StartCoroutine(FindAllPaths(start,end, iterationsPerFrame));
		}

		public IEnumerator FindAllPaths(NavNode start, NavNode end, int iterationsPerFrame)
		{
			Running = true;
			var frontier = new Queue<NavNode>();
			_cachedStart = start;
			frontier.Enqueue(start);
			_cameFrom = new Dictionary<NavNode, NavNode>();
			Distances = new Dictionary<NavNode, int> {[start] = 0};
			var iterations = 0;
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				foreach (var next in _tilemapNavigation.GetNeighborNodes(current))
				{
					if (Distances.ContainsKey(next) || !next.Walkable)
					{
						continue;
					}

					frontier.Enqueue(next);
					Distances[next] = Distances[current] + 1;
					_cameFrom[next] = current;
				}

				//performance things
				iterations++;
				// ReSharper disable once InvertIf
				if (iterations >= iterationsPerFrame)
				{
					iterations = 0;
					yield return null;
				}
			}
			
			SetPathList(start,end);
		}

		private void SetPathList(NavNode start, NavNode end)
		{
			Running = false;
			var search = end;
			path.Clear();
			while (search != start)
			{
				if (_cameFrom.ContainsKey(search))
				{
					path.Add(search);
					search = _cameFrom[search];
				}
				else
				{
					return;
				}
			}

			path.Add(start);
			path.Reverse();
		}

		public List<NavNode> GetPath()
		{
			return path;
		}
	}
}