using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtilities.Collections.Generic {
	/// <summary>
	/// A 1D representation of a 3D list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class List3D<T> : IList<T> {
		public T this[int index] {
			get => _list[index];
			set => _list[index] = value;
		}

		public int Count => _list.Count;
		public bool IsReadOnly => false;

		private List<T> _list;
		protected int _columns;
		protected int _rows;
		protected int _depth;

		/// <summary>
		/// Create an empty 3D 0x0x0 list
		/// </summary>
		public List3D() : this(0, 0, 0) { }

		/// <summary>
		/// Create a 3D list with a number of rows and columns.
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		public List3D(int rows, int columns, int depth) : this(new List<T>(columns * rows * depth), columns, rows, depth) { }

		/// <summary>
		/// Create a 3D rows by columns list from an initial list.
		/// </summary>
		/// <param name="initialList"></param>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		public List3D(IEnumerable<T> initialList, int rows, int columns, int depth) {
			_list = new List<T>(initialList);
			_rows = rows;
			_columns = columns;
			_depth = depth;
		}

		/// <summary>
		/// Add item to the list.
		/// </summary>
		/// <param name="item"></param>
		public virtual void Add(T item) {
			_list.Add(item);
		}

		/// <summary>
		/// Set rows and columns of the list.
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		public virtual void SetDimensions(int rows, int columns) {
			_rows = rows;
			_columns = columns;
		}

		/// <summary>
		/// Remove all elements from list.
		/// </summary>
		public virtual void Clear() {
			_list.Clear();
		}

		/// <summary>
		/// Determines if item is in the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Contains(T item) {
			return _list.Contains(item);
		}

		/// <summary>
		/// Copies the entire list into the front of an array.
		/// </summary>
		/// <param name="array"></param>
		public virtual void CopyTo(T[] array) {
			CopyTo(array, 0);
		}

		/// <summary>
		/// Copies the entire list into an array at a given index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayInsertIndex"></param>
		public virtual void CopyTo(T[] array, int arrayInsertIndex) {
			_list.CopyTo(array, arrayInsertIndex);
		}

		/// <summary>
		/// Copies range of elements from the list into the front of an array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="start"></param>
		/// <param name="count"></param>
		public virtual void CopyRange(T[] array, int start, int count) {
			CopyRange(array, 0, start, count);
		}

		/// <summary>
		/// Copies range of elements from the list into an array at a given index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayInsertIndex"></param>
		/// <param name="start"></param>
		/// <param name="count"></param>
		public virtual void CopyRange(T[] array, int arrayInsertIndex, int start, int count) {
			_list.CopyTo(start, array, arrayInsertIndex, count);
		}

		/// <summary>
		/// Copies the entire list into the front of a 3D array.
		/// </summary>
		/// <param name="array"></param>
		public virtual void CopyTo3D(T[][] array) {
			CopyTo3D(array, 0, 0);
		}

		/// <summary>
		/// Copies the entire list into a 3D array at a given index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="rowInsertIndex"></param>
		/// <param name="colInsertIndex"></param>
		public virtual void CopyTo3D(T[][] array, int rowInsertIndex, int colInsertIndex) {
			int currentListElement = 0;

			for (int i = rowInsertIndex; i < array.Length; ++i) {
				for (int j = colInsertIndex; j < array[i].Length; ++j) {
					if (currentListElement >= Count) {
						return;
					}

					array[i][j] = _list[currentListElement];
					++currentListElement;
				}

				colInsertIndex = 0;
			}
		}

		/// <summary>
		/// Copies range of elements from the list into the front of a 3D array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="rowStartIndex"></param>
		/// <param name="colStartIndex"></param>
		public virtual void CopyRange3D(T[][] array, int rowStartIndex, int colStartIndex, int count) {
			CopyRange3D(array, rowStartIndex, colStartIndex, count, 0, 0);
		}

		/// <summary>
		/// Copes range of elements from the list into a 3D array at a given index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="rowStartIndex"></param>
		/// <param name="colStartIndex"></param>
		/// <param name="rowInsertIndex"></param>
		/// <param name="colInsertIndex"></param>
		public virtual void CopyRange3D(T[][] array, int rowStartIndex, int colStartIndex, int count, int rowInsertIndex, int colInsertIndex) {
			int currentListElement = rowStartIndex * _columns + colStartIndex;

			for (int i = rowInsertIndex; i < array.Length; ++i) {
				for (int j = colInsertIndex; j < array[i].Length; ++j) {
					if (currentListElement >= Count || count == 0) {
						return;
					}

					array[i][j] = _list[currentListElement];
					++currentListElement;
					--count;
				}

				colInsertIndex = 0;
			}
		}

		public IEnumerator<T> GetEnumerator() {
			return _list.GetEnumerator();
		}

		/// <summary>
		/// Get 1D index of item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual int IndexOf(T item) {
			return _list.IndexOf(item);
		}

		/// <summary>
		/// Get 3D index of item (row, column).
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Tuple<int, int> IndexOf3D(T item) {
			int targetIndex = _list.IndexOf(item);
			return new Tuple<int, int>(
				(int) Math.Floor((double) (targetIndex / _columns)),
				targetIndex % _columns);
		}

		/// <summary>
		/// Inserts item at index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public virtual void Insert(int index, T item) {
			_list.Insert(index, item);
		}

		/// <summary>
		/// Inserts item at specified row and column.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="item"></param>
		public virtual void Insert3D(int row, int col, T item) {
			_list.Insert(row * _columns + col, item);
		}

		/// <summary>
		/// Removes item from list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Remove(T item) {
			return _list.Remove(item);
		}

		/// <summary>
		/// Removes item at index.
		/// </summary>
		/// <param name="index"></param>
		public virtual void RemoveAt(int index) {
			_list.RemoveAt(index);
		}

		/// <summary>
		/// Removes item at specified row and column.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		public virtual void RemoveAt3D(int row, int col) {
			_list.RemoveAt(row * _columns + col);
		}

		/// <summary>
		/// Removes range of elements at specified index from list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public virtual void RemoveRange(int index, int count) {
			_list.RemoveRange(index, count);
		}

		/// <summary>
		/// Removes range of elements at specified row and column from list.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="count"></param>
		public virtual void RemoveRange3D(int row, int col, int count) {
			_list.RemoveRange(row * _columns + col, count);
		}

		/// <summary>
		/// Returns an enumerator in index order that can be used to iterate over the list.
		/// </summary>
		/// <returns>IEnumerator</returns>
		IEnumerator IEnumerable.GetEnumerator() {
			return _list.GetEnumerator();
		}
	}
}