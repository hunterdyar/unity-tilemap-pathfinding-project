using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace NavigationTiles.Entities
{
	/// <summary>
	/// Access point to spawning the same entity prefab on an entity map.
	/// </summary>
	public class GridEntityPool : MonoBehaviour
	{
		[Header("Pool Settings")] [SerializeField]
		private GridEntity _entityPrefab;
		[SerializeField] private EntityMap _entityMap;
		[SerializeField] private int maxPoolSize = 10;

		[Tooltip("Leave null for no parent.")]
		[SerializeField] private Transform _entityParent;

		//pool
		private IObjectPool<GridEntity> _pool;

		private void Awake()
		{
			_pool = new ObjectPool<GridEntity>(SpawnNewGridEntityInPool, OnTakeFromPool, OnReturnedToPool, OnDestroyedEntity, true,10, maxPoolSize);
		}

		public GridEntity CreateEntityOnNode(NavNode node)
		{
			var entity = _pool.Get();
			entity.Initiate(_pool);
			_entityMap.AddEntityToMap(node, entity);
			return entity;
		}
		private GridEntity SpawnNewGridEntityInPool()
		{
			return Instantiate(_entityPrefab,_entityParent);
			//still needs to be initiated
		}
		void OnReturnedToPool(GridEntity entity)
		{
			_entityMap.RemoveEntity(entity);
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