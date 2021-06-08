using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace UnityUtilities.Collections.Generic {

	public class List2D<T> : List<T> {
		public int width { get; private set; }
		public int height { get; private set; }
		
		public List2D() : this(0, 0) { }
		
		public List2D(int x, int y) : this(new List<T>(new T[x * y]), x, y) { }
		
		public List2D(IEnumerable<T> initialList, int x, int y) : base(initialList) {
			width = x;
			height = y;
		}
		
		public T Get(int x, int y) {
			return !IsOutOfBounds(x, y) ? this[y * width + x] : default;
		}

		public virtual void Insert(int x, int y, T item) {
			if (!IsOutOfBounds(x ,y)) {
				Insert(y * width + x, item);
			}
		}

		public virtual void Remove(int x, int y) {
			if (!IsOutOfBounds(x ,y)) {
				RemoveAt(y * width + x);
			}
		}
		
		public void IndexOf(T item, out int x, out int y) {
			var targetIndex = IndexOf(item);
			x = targetIndex % width;
			y = Mathf.FloorToInt(targetIndex / width);
		}

		public void CopyRange(T[] array, int start, int count) {
			CopyRange(array, 0, start, count);
		}
		
		public void CopyRange(T[] array, int arrayInsertIndex, int start, int count) {
			CopyTo(start, array, arrayInsertIndex, count);
		}
		
		public virtual void CopyTo2D(T[][] array) {
			CopyTo2D(array, 0, 0);
		}
		
		public virtual void CopyTo2D(T[][] array, int rowInsertIndex, int colInsertIndex) {
			int currentListElement = 0;

			for (int i = rowInsertIndex; i < array.Length; ++i) {
				for (int j = colInsertIndex; j < array[i].Length; ++j) {
					if (currentListElement >= Count) {
						return;
					}

					array[i][j] = base[currentListElement];
					++currentListElement;
				}

				colInsertIndex = 0;
			}
		}
		
		public virtual void CopyRange2D(T[][] array, int rowStartIndex, int colStartIndex, int count) {
			CopyRange2D(array, rowStartIndex, colStartIndex, count, 0, 0);
		}
		
		public virtual void CopyRange2D(T[][] array, int rowStartIndex, int colStartIndex, int count, int rowInsertIndex, int colInsertIndex) {
			int currentListElement = rowStartIndex * width + colStartIndex;

			for (int i = rowInsertIndex; i < array.Length; ++i) {
				for (int j = colInsertIndex; j < array[i].Length; ++j) {
					if (currentListElement >= Count || count == 0) {
						return;
					}

					array[i][j] = base[currentListElement];
					++currentListElement;
					--count;
				}

				colInsertIndex = 0;
			}
		}

		public virtual void RemoveRange2D(int x, int y, int count) {
			RemoveRange(y * width + x, count);
		}
		
		protected bool IsOutOfBounds(int x, int y) {
			return x > width - 1 || x < 0 || y > height - 1 || y < 0;
		}
	}
}