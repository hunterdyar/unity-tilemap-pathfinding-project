// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Nav_Tiles.Scripts.Pathfinding
// {
// 	public class AStarPathfinder : IPathfinder
// 	{
// 		private readonly List<NavTile> open = new List<NavTile>();
// 		private readonly List<NavTile> closed = new List<NavTile>();
//
// 		private Dictionary<NavTile, NavTile> cameFrom = new Dictionary<NavTile, NavTile>();
// 		private Dictionary<NavTile, int> costSoFar = new Dictionary<NavTile, int>();
//
// 		private TilemapNavigation _navMap;
// 		public AStarPathfinder(TilemapNavigation tilemapNavigation)
// 		{
// 			_navMap = tilemapNavigation;
// 		}
// 		
// 		public List<NavTile> FindPath(NavTile start, NavTile finish)
// 		{
// 			//reset cached data
// 			open.Clear();
// 			closed.Clear();
// 			cameFrom.Clear();
// 			costSoFar.Clear();
// 			
// 			//Add initial node
// 			open.Add(start);
//
// 			//we add neighbors to the list, and make lists of out of them.
// 			//
// 			while (open.Count > 0)
// 			{
// 				
// 			}
// 		}
// 	}
// }