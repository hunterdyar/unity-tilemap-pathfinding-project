using System.Collections.Generic;
using NavigationTiles.Entities;
using NavigationTiles.Pathfinding;
using UnityEngine;

namespace NavigationTiles
{
	public class NavNode : INode
	{
		//Tile/Configuration things.
		
		public NavTile NavTile => _tile;
		private NavTile _tile;
		public TilemapNavigation TilemapNavigation => _navigation;
		private TilemapNavigation _navigation;

		public int WalkCost => _tile.WalkCost;
		public bool Walkable => _tile.Walkable;
		
		
		// Positions
		
		/// <summary>
		/// TilemapPosition is the cell position in the Grid component. NavPosition is different for hex grids, and internal.
		/// </summary>
		public Vector3Int NavPosition => _navPosition;
		private Vector3Int _navPosition;
		public Vector3Int TilemapPosition => _navigation.NavCellToGridCell(NavPosition);
		public Vector3 WorldPosition => _navigation.Tilemap.CellToWorld(TilemapPosition)+_navigation.Tilemap.tileAnchor;

		//Grid Entities
		private List<GridEntity> _entities;
		public NavNode(NavTile tile,Vector3Int navPosition,TilemapNavigation navigation)
		{
			this._tile = tile;
			_navPosition = navPosition;
			this._navigation = navigation;
		}

		public void AddGridEntity(GridEntity entity)
		{
			//Lazy init, because we probably don't need an empty list when not using entities.
			if (_entities == null)
			{
				_entities = new List<GridEntity>();
			}

			if (!_entities.Contains(entity))
			{
				_entities.Add(entity);
				entity.SetNode(this);
			}
			else
			{
				Debug.LogWarning("Trying to add entity to navnode but entity is already there.");
			}
		}

		public void RemoveGridEntity(GridEntity entity)
		{
			if (_entities.Contains(entity))
			{
				_entities.Remove(entity);
				entity.ClearNodeReference(this);
			}
			else
			{
				Debug.LogWarning($"Trying to remove {entity} from NavNode, but entity not at this navnode.");
			}
		}

		/// <summary>
		/// Get Entities here. Won't return null, will give empty list instead.
		/// </summary>
		public List<GridEntity> GetEntitiesHere()
		{
			if (_entities == null)
			{
				return new List<GridEntity>();
			}

			return _entities;
		}

		public bool HasEntity(GridEntity entity)
		{
			if (_entities == null)
			{
				return false;
			}

			return _entities.Contains(entity);
		}
	}
}