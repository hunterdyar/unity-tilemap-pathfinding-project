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
		public Vector3Int location;
		public NavNode(NavTile tile,Vector3Int location,TilemapNavigation navigation)
		{
			this.location = location;
			this.tile = tile;
			this.navigation = navigation;
		}
	}
}