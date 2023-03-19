using System.Collections.Generic;
using NavigationTiles.Utility;
using UnityEditor;
using UnityEngine;

namespace NavigationTiles.GridShapes
{
	[CreateAssetMenu(fileName = "Grid Shape", menuName = "Nav Tiles/Shapes/Shape", order = 0)]
	public class GridShape : ScriptableShape
	{
		public override List<Vector2Int> Shape => _shape;
		[SerializeField] private List<Vector2Int> _shape;

		/// <summary>
		/// Calculates the bounds. While a shape may or may not contain the 'origin', the optional bool parameter can choose to include the origin, and will return a valid bounds with an empty shape.
		/// </summary>
		/// <returns></returns>
		public BoundsInt GetShapeBounds(bool includeOrigin = false)
		{
			var bounds = new BoundsInt();
			//starting at 0 basically means that the origin is included in bounds calculation. This is important for the editor tool when the list is empty.
			var minX = 0;
			var minY = 0;
			var maxX = 0;
			var maxY = 0;

			if (!includeOrigin && _shape.Count > 0)
			{
				minX = _shape[0].x;
				maxX = _shape[0].x;
				minY = _shape[0].y;
				maxY = _shape[0].y;
			}else if (!includeOrigin)//_shape is empty.
			{
				Debug.LogWarning("Can't get shape bounds. No items in shape and origin not included.",this);
			}
			
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
				Undo.RecordObject(this, "Toggle Shape Cell");
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
		/// Calculates and returns the shape rotated around the origin, as if it started facing up v2(0,1). Does not modify original shape.
		/// </summary>
		public List<Vector2Int> GetShapeInCardinalFacingDirection(Vector2Int facing)
		{
			// "normalize"
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

		//While these functions could be public, I consider it bad practice for there to be too many functions that modify a shape that might be used at runtime.
		//Basically, using this during runtime will have it's changes saved in the scriptableObject, and that's probably not what we want.
		//so, above for runtime - returns a copy, below just for editor usefulness.
		 
		#region Editor Utility Functions
#if UNITY_EDITOR

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
#endif
#endregion


	}
	
	
}