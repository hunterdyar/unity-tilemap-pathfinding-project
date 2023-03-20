using System.Collections.Generic;
using UnityEngine;

namespace NavigationTiles.GridShapes
{
	[CreateAssetMenu(fileName = "Repeat Offset Shape", menuName = "Nav Tiles/Shapes/Repeat Offset Shape", order = 5)]

	public class RepeatInDirection : ScriptableShape
	{
		//todo:  make Shape private and use a GetShape(Tilemap) that can be more consistently overridden by the children.
		//so move Shape into just GridShape child, and let combined and repeatindirection use tilemap, don't need _shape lists that it doesnt make sense for them to have.
		public override List<Vector2Int> Shape => GetMaximumShape();
		
		[SerializeField] private bool includeLocalOrigin;
		[SerializeField] private bool stopAtNonWalkable;
		[SerializeField] private Vector2Int offset;
		[Min(0)]
		[SerializeField] private int maxRepeats = 100;

		/// <summary>
		/// Get all of the values up to MaxRepeats, centered on zero.
		/// </summary>
		/// <returns></returns>
		private List<Vector2Int> GetMaximumShape()
		{
			List<Vector2Int> shape = new List<Vector2Int>();
			var last = Vector2Int.zero;
			if (includeLocalOrigin)
			{
				shape.Add(last);
			}
			for (int i = 0; i < maxRepeats; i++)
			{
				last = last + offset;
				shape.Add(last);
			}

			return shape;
		}
		
		public override List<NavNode> GetNodesOnTilemap(NavNode center, TilemapNavigation navigation)
		{
			List<NavNode> shape = new List<NavNode>();
			var last = center;
			if (includeLocalOrigin)
			{
				shape.Add(last);
			}

			for (int i = 0; i < maxRepeats; i++)
			{
				var nextPos = last.NavPosition + (Vector3Int)offset;
				if (navigation.TryGetNavNode(nextPos, out last))
				{
					//if we ignore non-walkable or must be walkable
					if (!stopAtNonWalkable || last.Walkable)
					{
						shape.Add(last);
					}
					else
					{
						//break
						return shape;
					}
				}
				else
				{
					break;
				}
			}

			return shape;
		}
	}
}