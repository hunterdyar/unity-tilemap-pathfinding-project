using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace NavigationTiles.Agents
{
	/// <summary>
	/// Grid Entity Pool should be the primary way to get and return GridEntities that are repeated, like effects, highlighting tiles, and so on.
	/// </summary>
	public class GridEntityPool : MonoBehaviour
	{
		[Header("Pool Settings")] [SerializeField]
		private GridEntity _entityPrefab;
		[SerializeField] private int maxPoolSize = 10;

		[Tooltip("Leave null for no parent.")]
		[SerializeField] private Transform _entityParent;
		// public IObjectPool<GridEntity> Pool => _pool;
		private IObjectPool<GridEntity> _pool;

		private void Awake()
		{
			_pool = new ObjectPool<GridEntity>(SpawnNewGridEntityInPool, OnTakeFromPool, OnReturnedToPool, OnDestroyedEntity, true,10, maxPoolSize);
		}

		public GridEntity CreateEntityOnNode(NavNode node)
		{
			var entity = _pool.Get();
			entity.Initiate(_pool);
			node.AddGridEntity(entity);
			return entity;
		}
		private GridEntity SpawnNewGridEntityInPool()
		{
			return Instantiate(_entityPrefab,_entityParent);
			//still needs to be initiated
		}
		void OnReturnedToPool(GridEntity entity)
		{
			//this looks cursed but I promise it makes sense. We always go through the node to update the entity properties.
			entity.Node.RemoveGridEntity(entity);
			entity.gameObject.SetActive(false);	
		}
		void OnTakeFromPool(GridEntity entity)
		{
			entity.gameObject.SetActive(true);
		}

		void OnDestroyedEntity(GridEntity entity)
		{
			// entity.Node.ClearEntity(entity);
			Destroy(entity);
		}
	}
}