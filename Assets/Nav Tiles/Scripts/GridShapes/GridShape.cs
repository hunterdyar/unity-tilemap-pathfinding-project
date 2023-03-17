using System.Collections.Generic;
using NavigationTiles.Utility;
using UnityEditor;
using UnityEngine;

namespace NavigationTiles.GridShapes
{
	[CreateAssetMenu(fileName = "Grid Shape", menuName = "Nav Tiles/Grid Shape", order = 0)]
	public class GridShape : ScriptableObject
	{
		public List<Vector2Int> Shape => _shape;
		[SerializeField] private List<Vector2Int> _shape;
		public BoundsInt GridBounds => GetShapeBounds();

		private BoundsInt GetShapeBounds()
		{
			BoundsInt bounds = new BoundsInt();
			int minX = 0;
			int minY = 0;
			int maxX = 0;
			int maxY = 0;
			foreach (var pos in _shape)
			{
				minX = Mathf.Min(minX, pos.x);
				maxX = Mathf.Max(maxX, pos.x);
				
				minY = Mathf.Min(minY, pos.y);
				maxY = Mathf.Max(maxY, pos.y);
			}
			bounds.SetMinMax(new Vector3Int(minX,minY,0),new Vector3Int(maxX,maxY,0));
			return bounds;
		}

		public void ToggleCell(Vector2Int pos)
		{

#if UNITY_EDITOR
				Undo.RecordObject(this, "Toggle Cell");
#endif
				// reassemble
				if (_shape.Contains(pos))
				{
					_shape.Remove(pos);
				}
				else
				{
					_shape.Add(pos);
				}
#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
#endif
		}
		
	
		/// <summary>
		/// Calculates the shape rotated around the origin axis, as if it started facing up v2(0,1). 
		/// </summary>
		public List<Vector2Int> GetShapeInCardinalFacingDirection(Vector2Int facing)
		{
			int fx = Mathf.Clamp(facing.x, -1, 1);
			int fy = Mathf.Clamp(facing.y, -1, 1);
			
			if (fx == 0)
			{
				if (fy == 1)
				{
					return _shape;
				}else if (fy == -1)
				{
					return _shape.ConvertAll(v => v.Rotate180());
				}
			}

			if (fy == 0)
			{
				if (fx == 1)
				{
					return _shape.ConvertAll(v => v.RotateRight());
				}else if (fx == -1)
				{
					//return rotated left.
					return _shape.ConvertAll(v => v.RotateLeft());
				}
			}

			Debug.LogWarning("GetShapeInFacingDir requires input facing dir to be cardinal.");
			return _shape;
		}

		public List<Vector2Int> GetShapeFlippedVertically()
		{
			return _shape.ConvertAll(v => v.FlipVertically());
		}

		public List<Vector2Int> GetShapeFlippedHorizontally()
		{
			return _shape.ConvertAll(v => v.FlipHorizontally());
		}

		//Default shape looks up. Looking right, down, left, or up.
		[ContextMenu("Rotate Right")]
		void RR()
		{
			_shape = GetShapeInCardinalFacingDirection(Vector2Int.right);
		}

		[ContextMenu("Rotate Left")]
		void RL()
		{
			_shape = GetShapeInCardinalFacingDirection(Vector2Int.left);
		}

		[ContextMenu("Rotate 180")]
		void R180()
		{
			_shape = GetShapeInCardinalFacingDirection(Vector2Int.down);
		}

		[ContextMenu("Flip Vertically")]
		void FV()
		{
			_shape = GetShapeFlippedVertically();
		}

		[ContextMenu("Flip Horizontally")]
		void FH()
		{
			_shape = GetShapeFlippedHorizontally();
		}
	}
	
	
}