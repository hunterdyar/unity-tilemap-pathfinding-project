using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NavigationTiles.GridShapes
{
	[CreateAssetMenu(fileName = "Combined Grid Shape", menuName = "Nav Tiles/Shape/Combined Shape", order = 0)]
	public class CombinedGridShape : ScriptableShape
	{
		[Tooltip("Included shapes be a set union. Excluded spaces will take priority, and will always be excluded, regardless of order in this list. In other words, it is not a sequence of adds/subtracts, but taking a union of all the 'includes' and excluding from that any excludes.")]
		[SerializeField] private List<ShapeFromSet> _shapes;
		public override List<Vector2Int> Shape => GetCombinedShape();
		private List<Vector2Int> GetCombinedShape()
		{
			//Dare me to do this without linq? I'd rather not!
			var exclude = _shapes.Where(x => x.Operation == ShapeSetOperation.Exclude).SelectMany(x => x.Shape.Shape).Distinct();
			return _shapes.Where(x=>x.Operation == ShapeSetOperation.Include).SelectMany(x=>x.Shape.Shape).Where(x=>!exclude.Contains(x)).Distinct().ToList();
		}

		private void OnValidate()
		{
			var me = _shapes.Find(x => x.Shape == this);
			if (me != null)
			{
				//If this wasn't open source, I would be using Odin validator and inspector to do this up. If you have Odin, you should use it.
				Debug.LogWarning("Combined shape must not contain itself.");
				_shapes.Remove(me);
			}
		}
	}
}