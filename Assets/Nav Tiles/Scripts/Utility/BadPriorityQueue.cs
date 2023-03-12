// using System;
// using System.Collections.Generic;
// using Unity.VisualScripting;
//
// namespace Nav_Tiles.Scripts.Utility
// {
// 	public class PriorityQueue<T>
// 	{
// 		private List<T> _heap;
//
// 		public delegate int GetCostDelegate(T item);
//
// 		private GetCostDelegate GetCost;
// 		public int Count => _heap.Count;
// 		
// 		public PriorityQueue(GetCostDelegate getCost)
// 		{
// 			GetCost = getCost;
// 			_heap = new List<T>();
// 		}
//
// 		public void Enqueue(T newItem)
// 		{
// 			_heap.Add(newItem);
// 			int i = _heap.Count - 1;//index of newItem
// 			while (i != 0)
// 			{
// 				int p = (i - 1) / 2;
// 				if(GetCost(_heap[p]) > GetCost(_heap[i]))
// 				{
// 					Swap(i, p);
// 					i = p;
// 				}
// 				else
// 				{
// 					return;
// 				}
// 			}
// 		}
//
// 		public T Dequeue()
// 		{
// 			int i = _heap.Count - 1;
// 			T first = _heap[0];
// 			_heap[0] = _heap[i];
// 			_heap.RemoveAt(i);
// 			--i;
// 			int p = 0;
//
// 			while (true)
// 			{
// 				int left = p * 2 + 1;
// 				if (left > i)
// 				{
// 					break;
// 				}
//
// 				int right = left + 1;
// 				if ((right < p) && (GetCost(_heap[right]) < GetCost(_heap[left])))
// 				{
// 					left = right;
// 				}
// 				if (GetCost(_heap[p]) <= GetCost(_heap[left]))
// 				{
// 					break;
// 				}
//
// 				Swap(p, left);
// 				p = left;
// 			}
//
// 			return first;
// 		}
//
// 		public void Clear()
// 		{
// 			_heap.Clear();
// 		}
//
// 		public bool TryPeek(out T item)
// 		{
// 			if (_heap.Count > 0)
// 			{
// 				item = _heap[0];
// 				return true;
// 			}else
// 			{
// 				item = default;
// 				return false;
// 			}
// 		}
//
// 		public T Peek()
// 		{
// 			if (_heap.Count > 0)
// 			{
// 				return _heap[0];
// 			}
// 			else
// 			{
// 				return default;
// 			}
// 		}
// 		
// 		public bool Contains(T item)
// 		{
// 			return _heap.Contains(item);
// 		}
// 		
// 		public bool IsEmpty()
// 		{
// 			return _heap.Count == 0;
// 		}
//
// 		public void Remove(T item)
// 		{
// 			_heap.Remove(item);
// 		}
// 		private void Swap(int a,int b)
// 		{
// 			//Deconstruction Syntax for swapping
// 			(_heap[a], _heap[b]) = (_heap[b], _heap[a]);
// 		}
// 	}
// }