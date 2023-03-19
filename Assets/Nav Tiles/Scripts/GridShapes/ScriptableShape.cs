using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NavigationTiles.GridShapes
{ 
	public abstract class ScriptableShape : ScriptableObject
	{
		public abstract List<Vector2Int> Shape { get; }

		/// <summary>
		/// Returns the shape as NavNodes filtering out positions that the tilemap doesn't have, and absolute tilemap positions.
		/// Required when using procedural shapes, like "straight line until wall".
		/// </summary>

		//Todo: This should return List<NavNode> ?
		public virtual List<Vector3Int> GetShapeOnTilemap(Vector3Int center,TilemapNavigation navigation)
		{
			return Shape.ConvertAll<Vector3Int>(x => (Vector3Int)x + center).Where(navigation.HasNavCellLocation).ToList();
		}

		public virtual List<NavNode> GetNodesOnTilemap(NavNode center, TilemapNavigation navigation)
		{
			List<NavNode> nodes = new List<NavNode>();
			foreach (var offset in Shape)
			{
				if(navigation.TryGetNavNode(center.NavPosition+(Vector3Int)offset,out var node))
				{
					nodes.Add(node);	
				}
			}

			return nodes;
		}
	}
}