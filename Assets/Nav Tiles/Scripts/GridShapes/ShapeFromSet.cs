using UnityEngine;
using UnityEngine.Scripting;

namespace NavigationTiles.GridShapes
{
	[System.Serializable]
	public class ShapeFromSet
	{
		public ShapeSetOperation Operation;
		public ScriptableShape Shape => _shape;
		[SerializeField] private ScriptableShape _shape;
	}
}