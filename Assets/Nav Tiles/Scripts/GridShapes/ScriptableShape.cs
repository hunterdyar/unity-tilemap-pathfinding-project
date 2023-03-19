using System.Collections.Generic;
using UnityEngine;

namespace NavigationTiles.GridShapes
{ 
	public abstract class ScriptableShape : ScriptableObject
	{
		public abstract List<Vector2Int> Shape { get; }
	}
}