using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Pool;

namespace NavigationTiles.Agents
{
	/// <summary>
	/// Entities are the most basic thing that can exist on tiles (navnodes).
	/// Most of their API is private and internal. That's because I am using NavNodes as the single source of truth, containing their list of entities.
	/// Yet, it's very useful to ask an entity what node it is on. Turns out, pretty important. So We can read that from a cached reference, but updating it has to go through the NavNode.
	/// This way, we enforce that the entities reference to the navnode and the navnodes reference to the entity stay in sync. Which is pretty important.
	/// 
	/// In summary: doing something silly [multiple sources of truth (entity has node reference, node has entities reference)] but I am enforcing that you have to play nice.
	/// </summary>
	public class GridEntity : MonoBehaviour
	{
		[SerializeField] private Vector3 worldPositionOffset;
		public NavNode Node => _node;
		//This should always be updated by the NavNodes public functions.
		private NavNode _node;
		
		private IObjectPool<GridEntity> _pool;
		
		public void Initiate(IObjectPool<GridEntity> pool)
		{
			_pool = pool;
		}

		private void SnapToNode()
		{ 
			transform.position = _node.WorldPosition + worldPositionOffset;
		}

		internal void SetNode(NavNode node)
		{
			if (_node != null)
			{
				Debug.LogWarning("Can't set entity on node, entity already on node. Previous node should call clearnodereference");
			}
			_node = node;
			SnapToNode();
		}
		
		internal void ClearNodeReference(NavNode node)
		{
			if (_node == node)
			{
				_node = null;
			}
			else
			{
				Debug.LogWarning("Node trying to clear reference, from wrong node.");
			}
		}

		public void ReturnToPool()
		{
			_pool.Release(this);
		}
		
	}
}