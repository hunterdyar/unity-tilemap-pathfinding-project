namespace NavigationTiles.Pathfinding
{
	public interface IGraph
	{
		public INode[] GetNeighborNodes(INode center, bool walkableOnly = true);
	}
}