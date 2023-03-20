using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace NavigationTiles.Entities
{
	/// <summary>
	/// An Entity Map is like a layer on top of the tilemap. It's a dictionary of entities and their positions.
	/// An entity map cannot be used by multiple tilemaps at the same time.
	/// </summary>
	[CreateAssetMenu(fileName = "Entity Map", menuName = "Nav Tiles/Entities/Entity Map", order = 0)]
	public class EntityMap : ScriptableObject
	{
		//Keep a dictionary of GridEntities and NavNodes.
		
		//Todo: wrap data in bidirectional dictionary.
		private readonly Dictionary<NavNode, GridEntity> _entities = new Dictionary<NavNode, GridEntity>();
		private readonly Dictionary<GridEntity, NavNode> _inverseEntities = new Dictionary<GridEntity, NavNode>();
		public int Count => _entities.Count;

		//We don't actually need this injected! yet...
		//I think we will for listening to events, moving between maps, or so on.
		public TilemapNavigation TilemapNavNavigation => _tilemapNav;
		private TilemapNavigation _tilemapNav;

		public void Initiate(TilemapNavigation tilemapNavigation)
		{
			_entities.Clear();
			_inverseEntities.Clear();
			this._tilemapNav = tilemapNavigation;
		}

		public bool TryGetEntity(NavNode node, out GridEntity entity)
		{
			return _entities.TryGetValue(node, out entity);
		}

		public bool HasAnyEntity(NavNode node)
		{
			return _entities.ContainsKey(node);
		}
		public void AddEntityToMap(NavNode node, GridEntity entity, bool snapToPosition = true)
		{
			if (_entities.ContainsValue(entity))
			{
				if (_entities.ContainsKey(node) && _entities[node] != entity)
				{
					Debug.LogWarning("Trying to add entity to map, but that entity is already on map somewhere else.");
				}
			}
			if (!_entities.ContainsKey(node))
			{
				_entities.Add(node,entity);
				_inverseEntities.Add(entity,node);
				if (snapToPosition)
				{
					entity.SnapToNode(node);
				}
#if UNITY_EDITOR
				//The editor window contains an info box telling us how many items are in the dictionary, which is helpful for debugging.
				//This wouldn't update unless we mouse over it, which makes it far less helpful for debugging, so we force a repaint.
				InternalEditorUtility.RepaintAllViews();
#endif
			}
			else
			{
				Debug.LogError("Can't add entity, already an entity on this layer (map)");
			}
		}

		public void RemoveEntityOnNode(NavNode node)
		{
			if (_entities.ContainsKey(node))
			{
				var value = _entities[node];
				_entities.Remove(node);
				_inverseEntities.Remove(value);
			}

#if UNITY_EDITOR
			InternalEditorUtility.RepaintAllViews();
#endif
		}

		public void RemoveEntity(GridEntity entity)
		{
			if (_inverseEntities.ContainsKey(entity))
			{
				var value = _inverseEntities[entity];
				_inverseEntities.Remove(entity);
				_entities.Remove(value);
			}
#if UNITY_EDITOR
			InternalEditorUtility.RepaintAllViews();
#endif
		}

		public void MoveEntityToNode(GridEntity entity,NavNode node)
		{
			RemoveEntity(entity);
			AddEntityToMap(node,entity);
			
		}
	}
}