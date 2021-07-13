using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.Collections.Generic {
	public class List2D<T> : List<T> {
		public int width { get; private set; }
		public int height { get; private set; }

		public List2D() { }

		public List2D(int x, int y) : this(x, y, new List<T>(x * y)) { }

		public List2D(int x, int y, IEnumerable<T> initialList) : base(initialList) {
			width = x;
			height = y;
		}

		public T Get(int x, int y) => IsValidIndex(x, y) ? this[IndexOf1D(x, y)] : default;

		public void Set(T value, int x, int y) {
			if (!IsValidIndex(x, y)) return;
			base[IndexOf1D(x, y)] = value;
		}

		public virtual void Insert(T item, int x, int y) {
			if (IsValidIndex(x, y)) {
				Insert(IndexOf1D(x, y), item);
			}
		}

		public virtual void Remove(int x, int y) {
			if (IsValidIndex(x, y)) {
				RemoveAt(IndexOf1D(x, y));
			}
		}

		public virtual void RemoveRange(int x, int y, int count) => RemoveRange(IndexOf1D(x, y), count);

		public void IndexOf(T item, out int x, out int y) {
			var targetIndex = IndexOf(item);
			x = targetIndex % width;
			y = Mathf.FloorToInt(targetIndex / width);
		}

		private bool IsValidIndex(int x, int y) => IndexOf1D(x, y) < Count;

		// Gets the 1D index of an item in the list given x,y position.
		private int IndexOf1D(int x, int y) => y * width + x;

		// Given a 1D index, return x,y position.
		protected void GetXYForIndex(int index, out int x, out int y) {
			y = index / width;
			x = index - y * width;
		}
	}
}