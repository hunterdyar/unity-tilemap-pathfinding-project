using System;
using System.Collections.Generic;

namespace Nav_Tiles.Scripts.Utility
{
	//This is https://www.redblobgames.com/pathfinding/a-star/implementation.html#csharp
	//its known not great implementation, but lets me test.
	public class PriorityQueue<TElement, TPriority>
	{
		private List<Tuple<TElement, TPriority>> elements = new List<Tuple<TElement, TPriority>>();

		public int Count
		{
			get { return elements.Count; }
		}

		public void Enqueue(TElement item, TPriority priority)
		{
			elements.Add(Tuple.Create(item, priority));
		}

		public TElement Dequeue()
		{
			Comparer<TPriority> comparer = Comparer<TPriority>.Default;
			int bestIndex = 0;

			for (int i = 0; i < elements.Count; i++)
			{
				if (comparer.Compare(elements[i].Item2, elements[bestIndex].Item2) < 0)
				{
					bestIndex = i;
				}
			}

			TElement bestItem = elements[bestIndex].Item1;
			elements.RemoveAt(bestIndex);
			return bestItem;
		}
	}

}