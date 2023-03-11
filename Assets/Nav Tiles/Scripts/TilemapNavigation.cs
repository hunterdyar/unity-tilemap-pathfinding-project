using System;
using System.Collections;
using System.Collections.Generic;
using Nav_Tiles.Scripts;
using Nav_Tiles.Scripts.Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

//runtime only. We create a dictionary of NavNodes that is fast to operate on, which reference the tile data.

//There are a few structural things to know. One, Tilemap is a sealed class, so we can't make our own version of tilemap and extend it. This tool should just work with how unity works.
//That means we have to hook into tilemap and read it's data. For now, the asset will only work at Runtime, and we do this initialization on awake.
//That has some downsides - mostly editor convenience. But the upside is we can use a dictionary, as we won't be serializing any data.

//the downside is updates at runtime. This system doesn't support fully adding/removing tiles during play yet (use walkable flag for now), but my goal is to be able to.
//Perhaps more importantly, one can optimize a grid to remove redundant nodes, only leaving nodes where a user may turn (or stop). By having navnodes and navTiles be independent, that's a possibility.


[RequireComponent(typeof(Tilemap))]
public class TilemapNavigation : MonoBehaviour
{
	private NavTile[] GetNeighborCache = new NavTile[8];//"8" here needs to be the highest possible number of neighbors.
	public readonly Vector3Int[] CardinalDirections = new[]
	{
		new Vector3Int(1, 0, 0),
		new Vector3Int(0, 1, 0),
		new Vector3Int(-1,0,0),
		new Vector3Int(0,-1,0),
	};
	public readonly Vector3Int[] CardinalAndDiagonalDirections = new[]
	{
		new Vector3Int(1, 0, 0),
		new Vector3Int(0, 1, 0),
		new Vector3Int(-1, 0, 0),
		new Vector3Int(0, -1, 0),
		new Vector3Int(1, 1, 0),
		new Vector3Int(-1, 1, 0),
		new Vector3Int(-1, -1, 0),
		new Vector3Int(1, -1, 0),
	};
	public GridConnectionType ConnectionConnectionType => _connectionType = GridConnectionType.FlatCardinal;//default
	[FormerlySerializedAs("_type")] [SerializeField] private GridConnectionType _connectionType;
	public Tilemap Tilemap => _tilemap;
	private Tilemap _tilemap;
	
	//todo: dropdown selection for pathfinder types.
	public IPathfinder Pathfinder => _pathfinder;
	private IPathfinder _pathfinder;
	public Grid Grid => _tilemap.layoutGrid;
	
	//Dictionaries cannot be serialized by Unity. 
	private readonly Dictionary<Vector3Int, NavNode> _navMap = new Dictionary<Vector3Int, NavNode>();

	private void Awake()
	{
		_pathfinder = new DJPathfinder(this);
		_tilemap = GetComponent<Tilemap>();
		InitiateNavMap();
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
				_navMap.Add(location,new NavNode(tile,location,this));
			}
		} 
	}

	public NavNode[] GetNeighborNodes(NavNode node)
	{
		switch (_connectionType)
		{
			case GridConnectionType.FlatCardinalAndDiagonal:
				return GetNeighborNodesUsingDirectionList(node, CardinalAndDiagonalDirections);
			case GridConnectionType.FlatCardinal:
			default:
				return GetNeighborNodesUsingDirectionList(node, CardinalDirections);
		}
	}
	private NavNode[] GetNeighborNodesUsingDirectionList(NavNode node, Vector3Int[] directions)
	{
		NavNode[] nodeCache = new NavNode[12];
		int n = 0;
		foreach (var dir in directions)
		{
			if(_navMap.TryGetValue(node.location+dir,out var neighbor))
			{
				nodeCache[n] = neighbor;
				n++;
			}
		}

		if (n == 0)
		{
			return Array.Empty<NavNode>();
		}

		var output = new NavNode[n];
		Array.Copy(nodeCache, output, n);
		return output;
	}
	private void OnValidate()
	{
		_tilemap = GetComponent<Tilemap>();
		if (Grid.cellLayout == GridLayout.CellLayout.Hexagon && _connectionType != GridConnectionType.FlatHexagonal)
		{
			Debug.LogWarning("Hexagonal connection type is only valid option for grid.");
			_connectionType = GridConnectionType.FlatHexagonal;
		}

		if (Grid.cellLayout == GridLayout.CellLayout.Rectangle && _connectionType == GridConnectionType.FlatHexagonal)
		{
			Debug.LogWarning("Hexagonal connection type is not valid option for rectangular grid.");
			_connectionType = GridConnectionType.FlatCardinalAndDiagonal;
		}

		if (Grid.cellLayout == GridLayout.CellLayout.Isometric && _connectionType == GridConnectionType.FlatHexagonal)
		{
			Debug.LogWarning("Hexagonal connection type is not valid option for isometric grid.");
			_connectionType = GridConnectionType.FlatCardinal;
		}

		if (Grid.cellLayout == GridLayout.CellLayout.IsometricZAsY && _connectionType == GridConnectionType.FlatHexagonal)
		{
			Debug.LogWarning("Hexagonal connection type is not valid option for isometricZAsY grid.");
			_connectionType = GridConnectionType.FlatCardinal;
		}
	}

	public NavTile GetNavTile(Vector3Int cellPosition)
	{
		return _tilemap.GetTile<NavTile>(cellPosition);
	}

	//todo: make tryget pattern
	public NavNode GetNavNode(Vector3Int cellPosition)
	{
		if (_navMap.TryGetValue(cellPosition, out var node))
		{
			return node;
		}

		return null;
	}
}