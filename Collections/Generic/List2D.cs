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

		public T Get(int x, int y) => IsValidIndex(x, y) ? this[GetLinearPos(x, y)] : default;

		public void Set(T value, int x, int y) {
			if (!IsValidIndex(x, y)) return;
			base[GetLinearPos(x, y)] = value;
		}

		public virtual void Insert(T item, int x, int y) {
			if (IsValidIndex(x, y)) {
				Insert(GetLinearPos(x, y), item);
			}
		}

		public virtual void Remove(int x, int y) {
			if (IsValidIndex(x, y)) {
				RemoveAt(GetLinearPos(x, y));
			}
		}

		public virtual void RemoveRange(int x, int y, int count) => RemoveRange(GetLinearPos(x, y), count);

		public void IndexOf(T item, out int x, out int y) {
			var targetIndex = IndexOf(item);
			x = targetIndex % width;
			y = Mathf.FloorToInt(targetIndex / width);
		}

		protected bool IsValidIndex(int x, int y) => x < width && x >= 0 && y < height && y >= 0;

		private int GetLinearPos(int x, int y) => y * width + x;

		protected void GetXYForIndex(int index, out int x, out int y) {
			y = index / width;
			x = index - y * width;
		}
	}
}