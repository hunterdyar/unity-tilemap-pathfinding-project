// using System.Collections.Generic;
// using System.IO;
// using Nav_Tiles.Scripts.Utility;
// using UnityEngine;
//
// namespace Nav_Tiles.Scripts.Pathfinding
// {
// 	//http://idm-lab.org/bib/abstracts/papers/aaai07a.pdf
// 	public class ThetaStarPathfinding : IPathfinder
// 	{
// 		private TilemapNavigation _tilemapNavigation;
// 		private PriorityQueue<NavNode,int> openList = new PriorityQueue<NavNode,int>();
// 		private List<NavNode> closedList;
// 		private List<NavNode> path;
//
// 		public ThetaStarPathfinding(TilemapNavigation navigation)
// 		{
// 			_tilemapNavigation = navigation;
// 		}
// 		public List<NavNode> FindPath(NavNode start, NavNode end)
// 		{
// 			openList = new PriorityQueue<NavNode, int>();
// 			closedList = new List<NavNode>();
// 			path = new List<NavNode>();
// 			NavNode current;
// 			
// 			openList.Enqueue(end);
//
// 			while (!openList.IsEmpty())
// 			{
// 				//current frontier.
// 				current = openList.Dequeue();
// 				closedList.Add(current);
//
// 				if (current == start)
// 				{
// 					return path;
// 				}
//
// 				foreach (var neighbor in _tilemapNavigation.GetNeighborNodes(current))
// 				{
// 					if (!closedList.Contains(neighbor))
// 					{
// 						if (!openList.Contains(neighbor))
// 						{
// 							//uhguhdg
// 						}
// 					}
// 				}
// 			}
// 			
// 			return new List<NavNode>();
// 		}
// 		
// 	}
// }