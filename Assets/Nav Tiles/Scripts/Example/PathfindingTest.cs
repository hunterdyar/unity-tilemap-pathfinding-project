using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Nav_Tiles.Scripts.Example
{
	public class PathfindingTest : MonoBehaviour
	{
		[SerializeField] private TilemapNavigation _navigation;

		public NavNode endNode;
		public NavNode startNode;

		private Vector3Int hover;
		
		private List<NavNode> tiles = new List<NavNode>();
		
		void Update()
		{
			var mouseHover = _navigation.Grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (Input.GetMouseButtonDown(0))
			{
				if (startNode != null)
				{
					_navigation.Tilemap.SetColor(startNode.location, startNode.NavTile.color);
				}

				startNode = _navigation.GetNavNode(hover);
			}

			//if we move the mouse to a new space, update.
			if (hover != mouseHover)
			{
				hover = mouseHover;
				
				//reset color
				if (endNode != null)
				{
					_navigation.Tilemap.SetColor(endNode.location, endNode.NavTile.color);
				}
				
				//set end to current hover
				endNode = _navigation.GetNavNode(hover);
				
				//if we have start and end nodes.
				if (startNode is { Walkable: true } && endNode is { Walkable: true })
				{
					//this is for me in a month when I forget again
					if (startNode.NavTile.flags == TileFlags.LockColor)
					{
						Debug.LogWarning("Note: pathfinder testing uses setColor. NavTile flags need to not be set to 'lock color'", startNode.NavTile);
					}
					
					ResetColors();
					
					//the actual pathfinding. the rest of this script is just faffing about with colors.
					tiles = _navigation.Pathfinder.FindPath(startNode, endNode);
					
					//set all path colors
					SetColors(Color.blue);
					//set start and end
					_navigation.Tilemap.SetColor(startNode.location, Color.magenta);
					_navigation.Tilemap.SetColor(this.endNode.location, Color.green);
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

		private void ResetColors()
		{
			foreach (var t in tiles)
			{
				_navigation.Tilemap.SetColor(t.location, t.NavTile.color);
			}
		}
	}
}