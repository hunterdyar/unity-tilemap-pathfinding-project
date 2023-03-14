namespace NavigationTiles.Pathfinding
{
	public interface IGraph
	{
		public NavNode[] GetNeighborNodes(NavNode center, bool walkableOnly = true);
	}
}