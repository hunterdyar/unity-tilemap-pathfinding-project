using System.Collections.Generic;
using NavigationTiles.Agents;
using NavigationTiles.Pathfinding;
using UnityEngine;

namespace NavigationTiles
{
	public class NavNode : INode
	{
		public NavTile NavTile => _tile;
		private NavTile _tile;
		public TilemapNavigation TilemapNavigation => _navigation;
		private TilemapNavigation _navigation;
		
		public int WalkCost => _tile.WalkCost;
		public bool Walkable => _tile.Walkable;
		//todo: private setter
		/// <summary>
		/// TilemapPosition is the cell position in the Grid component. NavPosition is different for hex grids, and internal.
		/// </summary>
		public Vector3Int NavPosition => _navPosition;
		private Vector3Int _navPosition;
		public Vector3Int TilemapPosition => _navigation.NavCellToGridCell(NavPosition);
		public NavNode(NavTile tile,Vector3Int navPosition,TilemapNavigation navigation)
		{
			this._tile = tile;
			_navPosition = navPosition;
			this._navigation = navigation;
		}
	}
}