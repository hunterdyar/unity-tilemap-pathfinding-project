using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Nav_Tiles.Scripts.Example
{
	public class PathfindingTest : MonoBehaviour
	{
		[SerializeField] private TilemapNavigation _navigation;

		public Vector3Int end;
		public Vector3Int start;

		private List<NavNode> tiles = new List<NavNode>();
		
		void Update()
		{
			var hover = _navigation.Grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (Input.GetMouseButtonDown(0))
			{
				_navigation.Tilemap.SetColor(start, Color.white);
				start = hover;
			}

			if (end != hover)
			{
				_navigation.Tilemap.SetColor(end,Color.white);
				
				end = hover;
				var startNode = _navigation.GetNavNode(start);
				var endNode = _navigation.GetNavNode(end);
				
				//this is for me in a month when I forget again
				if (startNode.NavTile.flags == TileFlags.LockColor)
				{
					Debug.LogWarning("Note: pathfinder testing uses setColor. NavTile flags need to not be set to 'lock color'",startNode.NavTile);
				}

				if (startNode != null && endNode != null)
				{
					SetColors(Color.white);
					tiles = _navigation.Pathfinder.FindPath(startNode, endNode);
					SetColors(Color.blue);
					//set start and end
					_navigation.Tilemap.SetColor(start, Color.magenta);
					_navigation.Tilemap.SetColor(end, Color.green);
				}
			}
			
		}

		
		private void SetColors(Color color)
		{
			foreach (var t in tiles)
			{
				_navigation.Tilemap.SetColor(t.location,color);
			}
		}

	}
}