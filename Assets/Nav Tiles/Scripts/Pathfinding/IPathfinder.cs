using System.Collections.Generic;

namespace Nav_Tiles.Scripts.Pathfinding
{
	public interface IPathfinder
	{
		public List<NavNode> FindPath(NavNode start, NavNode end);
	}
}