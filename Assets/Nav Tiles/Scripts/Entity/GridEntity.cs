using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Pool;

namespace NavigationTiles.Entities
{
	public class GridEntity : MonoBehaviour
	{
		[SerializeField] private Vector3 worldPositionOffset;

		private IObjectPool<GridEntity> _pool;
		
		public void Initiate(IObjectPool<GridEntity> pool)
		{
			_pool = pool;
		}

		public void SnapToNode(NavNode node)
		{ 
			transform.position = node.WorldPosition + worldPositionOffset;
		}
		
		public void ReturnToPool()
		{
			_pool.Release(this);
		}
		
	}
}