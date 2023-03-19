using System;
using System.Collections.Generic;
using NavigationTiles.Entities;
using NavigationTiles.GridShapes;
using UnityEngine;

namespace NavigationTiles.Highlight
{
	[RequireComponent(typeof(GridEntityPool))]
	public class HighlighterShapeUnderMouse : MonoBehaviour
	{
		[SerializeField] private TilemapNavigation _map;
		private GridEntityPool _pool;
		[SerializeField] private ScriptableShape _shape;

		private Camera _camera;
		private readonly List<GridEntity> _highlights = new List<GridEntity>();

		private NavNode _highlightCenter;

		private void Awake()
		{
			_pool = GetComponent<GridEntityPool>();
		}

		private void Start()
		{
			_camera = Camera.main;
		}

		private void Update()
		{
			//refactoring update functions into their own calls allows us to early exit them conveniently, but also have multiple pieces of tick logic. 
			HighlightTick();
		}

		void HighlightTick()
		{
			if (_shape == null)
			{
				return;
			}

			if (TryGetNodeUnderMouse(out var hoverNode))
			{
				if (_highlightCenter != hoverNode)
				{
					_highlightCenter = hoverNode;
					HighlightNode(hoverNode);
				}
				return;
			}
			else
			{
				if (_highlightCenter != null)
				{
					ClearCurrent();
					_highlightCenter = null;
				}
			}
		}

		public void HighlightNode(NavNode node)
		{
			ClearCurrent();
			foreach (var space in _shape.GetNodesOnTilemap(node, _map))
			{
				var item = _pool.CreateEntityOnNode(space);
				_highlights.Add(item);
			}
		}

		private bool TryGetNodeUnderMouse(out NavNode node)
		{
			//world->cell->nav
			var mouseHover = _map.Grid.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
			return _map.TryGetNavNodeAtWorldPos(mouseHover, out node);
		}

		private void ClearCurrent()
		{
			foreach (var item in _highlights)
			{
				item.ReturnToPool();
			}
			_highlights.Clear();
		}
	}
}