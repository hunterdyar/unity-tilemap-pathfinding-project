using System;
using System.Collections.Generic;
using NavigationTiles.Pathfinding;
using NavigationTiles.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

//runtime only. We create a dictionary of NavNodes that is fast to operate on, which reference the tile data.

//There are a few structural things to know. One, Tilemap is a sealed class, so we can't make our own version of tilemap and extend it. This tool should just work with how unity works.
//That means we have to hook into tilemap and read it's data. For now, the asset will only work at Runtime, and we do this initialization on awake.
//That has some downsides - mostly editor convenience. But the upside is we can use a dictionary, as we won't be serializing any data.

//the downside is updates at runtime. This system doesn't support fully adding/removing tiles during play yet (use walkable flag for now), but my goal is to be able to.
//Perhaps more importantly, one can optimize a grid to remove redundant nodes, only leaving nodes where a user may turn (or stop). By having navnodes and navTiles be independent, that's a possibility.

namespace NavigationTiles
{
	[RequireComponent(typeof(Tilemap))]
	public class TilemapNavigation : MonoBehaviour, IGraph
	{
		private NavNode[] _getNeighborCache = new NavNode[8]; //"8" here needs to be the highest possible number of neighbors.
		public GridConnectionType ConnectionConnectionType => _connectionType = GridConnectionType.FlatCardinal; //default
		
		//todo: Editor script to set this to hexagon and readonly when appropriate
		[SerializeField]
		private GridConnectionType _connectionType;

		public Tilemap Tilemap => _tilemap;
		private Tilemap _tilemap;
		public Pathfinder<NavNode> Pathfinder => _pathfinder;
		private Pathfinder<NavNode> _pathfinder;
		public Grid Grid => _tilemap.layoutGrid;
		public int MaxNodeCount => _navMap.Count + 1; //used by the pathfinder.

		//Dictionaries cannot be serialized by Unity. 
		private readonly Dictionary<Vector3Int, NavNode> _navMap = new Dictionary<Vector3Int, NavNode>();

		private void Awake()
		{
			_tilemap = GetComponent<Tilemap>();
			InitiateNavMap();
			
			//We can select different pathfinders.
			if (Grid.cellLayout == GridLayout.CellLayout.Hexagon)
			{
				_pathfinder = new AstarHexPathfinder<NavNode>(this);
			}
			else
			{
				_pathfinder = new AStarPathfinder<NavNode>(this);
			}
		}

		private void InitiateNavMap()
		{
			//we don't know if bounds have been reasonably set or not.
			_tilemap.CompressBounds();

			var bounds = _tilemap.cellBounds;
			//I was about to write an extension method to give me allPositionsWithin, until taking the 1/4 second to actually read documentation and go "oh, wait, that already exists"
			//This will work for all layouts (rectangular, hex, grid, etc)
			foreach (var location in bounds.allPositionsWithin)
			{
				var tile = _tilemap.GetTile<NavTile>(location);
				if (tile != null)
				{
					//we use grid cellposition for rectangular and isometric maps, but for hex, we convert to Cube.
					//This makes all the math and pathfinding much easier, at the inconvenience of these wrapper functions to do the conversion when needed.
					var loc = GridCellToNavCell(location);
					_navMap.Add(loc, new NavNode(tile, loc, this));
				}
			}
		}

		/// <summary>
		/// This is only necessary when using hex grids.
		/// </summary>
		public Vector3Int GridCellToNavCell(Vector3Int location)
		{
			if (_tilemap.cellLayout == GridLayout.CellLayout.Hexagon)
			{
				if (_tilemap.cellSwizzle == GridLayout.CellSwizzle.XYZ)
				{
					//Unity is using Pointy Top, which uses offset odd-row coords. We will use Cube coordinates.
					return HexUtility.OddRToCube(location);
				}
				else if (_tilemap.cellSwizzle == GridLayout.CellSwizzle.YXZ)
				{
					//Unity is using Flat Top, which uses offset odd-col coords. Again, we will use cube for both cases.
					return HexUtility.OddQToCube(location);
				}
			}

			return location;
		}

		public Vector3Int NavCellToGridCell(Vector3Int location)
		{
			if (_tilemap.cellLayout == GridLayout.CellLayout.Hexagon)
			{
				if (_tilemap.cellSwizzle == GridLayout.CellSwizzle.XYZ)
				{
					//Unity is using Pointy Top, which uses offset odd-row coords. We will use Cube coordinates.
					return HexUtility.CubeToOddR(location);
				}
				else if (_tilemap.cellSwizzle == GridLayout.CellSwizzle.YXZ)
				{
					//Unity is using Flat Top, which uses offset odd-col coords. Again, we will use cube for both cases.
					return HexUtility.CubeToOddQ(location);
				}
			}

			return location;
		}
		public INode[] GetNeighborNodes(INode node, bool walkableOnly = true)
		{
			switch (_connectionType)
			{
				case GridConnectionType.FlatCardinalAndDiagonal:
					return GetNeighborNodesUsingDirectionList(node, RectUtility.CardinalAndDiagonalDirections, walkableOnly);
				case GridConnectionType.Hexagonal:
					return GetNeighborNodesUsingDirectionList(node, HexUtility.CubeHexDirections, walkableOnly);
				case GridConnectionType.FlatCardinal:
				default:
					return GetNeighborNodesUsingDirectionList(node, RectUtility.CardinalDirections, walkableOnly);
			}
		}

		private INode[] GetNeighborNodesUsingDirectionList(INode node, Vector3Int[] directions, bool walkableOnly = true)
		{
			// NavNode[] nodeCache = new NavNode[12];
			int n = 0;
			foreach (var dir in directions)
			{
				if (_navMap.TryGetValue(node.NavPosition + dir, out var neighbor))
				{
					if (!walkableOnly || node.Walkable)
					{
						_getNeighborCache[n] = neighbor;
						n++;
					}
				}
			}

			if (n == 0)
			{
				return Array.Empty<NavNode>();
			}

			var output = new NavNode[n];
			Array.Copy(_getNeighborCache, output, n);
			return output;
		}
		private void OnValidate()
		{
			_tilemap = GetComponent<Tilemap>();
			if (Grid.cellLayout == GridLayout.CellLayout.Hexagon && _connectionType != GridConnectionType.Hexagonal)
			{
				Debug.LogWarning("Hexagonal connection type is only valid option for grid.");
				_connectionType = GridConnectionType.Hexagonal;
			}

			if (Grid.cellLayout == GridLayout.CellLayout.Rectangle && _connectionType == GridConnectionType.Hexagonal)
			{
				Debug.LogWarning("Hexagonal connection type is not valid option for rectangular grid.");
				_connectionType = GridConnectionType.FlatCardinalAndDiagonal;
			}

			if (Grid.cellLayout == GridLayout.CellLayout.Isometric && _connectionType == GridConnectionType.Hexagonal)
			{
				Debug.LogWarning("Hexagonal connection type is not valid option for isometric grid.");
				_connectionType = GridConnectionType.FlatCardinal;
			}

			if (Grid.cellLayout == GridLayout.CellLayout.IsometricZAsY && _connectionType == GridConnectionType.Hexagonal)
			{
				Debug.LogWarning("Hexagonal connection type is not valid option for isometricZAsY grid.");
				_connectionType = GridConnectionType.FlatCardinal;
			}
		}


		public NavTile GetNavTile(Vector3Int gridCellPosition)
		{
			if (_connectionType != GridConnectionType.Hexagonal)
			{
				return _tilemap.GetTile<NavTile>(GridCellToNavCell(gridCellPosition));
			}
			else
			{
				return _tilemap.GetTile<NavTile>(gridCellPosition);
			}
		}

		
		public bool TryGetNavNode(Vector3Int gridCellPosition, out NavNode node)
		{
			return _navMap.TryGetValue(gridCellPosition, out node);
		}
		
		public NavNode GetNavNode(Vector3Int gridCellPosition)
		{
			if (_navMap.TryGetValue(GridCellToNavCell(gridCellPosition), out var node))
			{
				return node;
			}

			return null;
		}


		public bool HasNavCellLocation(Vector3Int navPosition)
		{
			return _navMap.ContainsKey(navPosition);
		}
	}
}