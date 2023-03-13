using Priority_Queue;
using UnityEngine;

namespace NavigationTiles
{
	public class NavNode
	{
		public NavTile NavTile => tile;
		private NavTile tile;
		public TilemapNavigation TilemapNavigation => navigation;
		private TilemapNavigation navigation;

		public int WalkCost => tile.WalkCost;
		public bool Walkable => tile.Walkable;
		//todo: private setter
		/// <summary>
		/// TilemapPosition is the cell position in the Grid component. NavPosition is different for hex grids, and internal.
		/// </summary>
		public Vector3Int NavPosition;
		public Vector3Int TilemapPosition => navigation.NavCellToGridCell(NavPosition);
		public NavNode(NavTile tile,Vector3Int navPosition,TilemapNavigation navigation)
		{
			this.tile = tile;
			this.NavPosition = navPosition;
			this.navigation = navigation;
		}
	}
}