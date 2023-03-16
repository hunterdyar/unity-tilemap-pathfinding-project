using Unity.VisualScripting;
using UnityEngine;

namespace NavigationTiles.Pathfinding
{
	public interface INode
	{
		public bool Walkable { get; }
		public int WalkCost {get;}
		public Vector3Int NavPosition { get; }
	}
}