using System;
using System.Collections.Generic;

namespace UnityUtilities {
    namespace Collections {
        namespace Generic {

            /// <summary>
            /// A 1D representation of a 2D list.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            public class List2D<T> : List<T> {

                public bool IsReadOnly => false;
                protected int _columns;
                protected int _rows;

                /// <summary>
                /// Create an empty 2D 0 by 0 list
                /// </summary>
                public List2D() : this(0, 0) { }

                /// <summary>
                /// Create a 2D list with a number of rows and columns.
                /// </summary>
                /// <param name="rows"></param>
                /// <param name="columns"></param>
                public List2D(int rows, int columns) : this(new List<T>(columns * rows), columns, rows) { }

                /// <summary>
                /// Create a 2D rows by columns list from an initial list.
                /// </summary>
                /// <param name="initialList"></param>
                /// <param name="rows"></param>
                /// <param name="columns"></param>
                public List2D(IEnumerable<T> initialList, int rows, int columns) : base(initialList) {
                    _rows = rows;
                    _columns = columns;
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
                /// Inserts a range of elements from the list `into the front of an array.
                /// </summary>
                /// <param name="array"></param>
                /// <param name="start"></param>
                /// <param name="count"></param>
                public virtual void CopyRange(T[] array, int start, int count) {
                    CopyRange(array, 0, start, count);
                }

                /// <summary>
                /// Inserts a range of elements from the list into an array at a given index.
                /// </summary>
                /// <param name="array"></param>
                /// <param name="arrayInsertIndex"></param>
                /// <param name="start"></param>
                /// <param name="count"></param>
                public virtual void CopyRange(T[] array, int arrayInsertIndex, int start, int count) {
                    CopyTo(start, array, arrayInsertIndex, count);
                }

                /// <summary>
                /// Copies the entire list into the front of a 2D array.
                /// </summary>
                /// <param name="array"></param>
                public virtual void CopyTo2D(T[][] array) {
                    CopyTo2D(array, 0, 0);
                }

                /// <summary>
                /// Inserts the entire list into a 2D array at a given index.
                /// </summary>
                /// <param name="array"></param>
                /// <param name="rowInsertIndex"></param>
                /// <param name="colInsertIndex"></param>
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

                /// <summary>
                /// Inserts a range of elements from the list into the front of a 2D array.
                /// </summary>
                /// <param name="array"></param>
                /// <param name="rowStartIndex"></param>
                /// <param name="colStartIndex"></param>
                public virtual void CopyRange2D(T[][] array, int rowStartIndex, int colStartIndex, int count) {
                    CopyRange2D(array, rowStartIndex, colStartIndex, count, 0, 0);
                }

                /// <summary>
                /// Inserts a range of elements from the list into a 2D array at a given index.
                /// </summary>
                /// <param name="array"></param>
                /// <param name="rowStartIndex"></param>
                /// <param name="colStartIndex"></param>
                /// <param name="rowInsertIndex"></param>
                /// <param name="colInsertIndex"></param>
                public virtual void CopyRange2D(T[][] array, int rowStartIndex, int colStartIndex, int count, int rowInsertIndex, int colInsertIndex) {
                    int currentListElement = rowStartIndex * _columns + colStartIndex;

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

                /// <summary>
                /// Get 2D index of item (row, column).
                /// </summary>
                /// <param name="item"></param>
                /// <returns></returns>
                public virtual Tuple<int, int> IndexOf2D(T item) {
                    int targetIndex = IndexOf(item);
                    return new Tuple<int, int>(
                        (int)Math.Floor((double)(targetIndex / _columns)),
                        targetIndex % _columns);
                }

                /// <summary>
                /// Inserts item at specified row and column.
                /// </summary>
                /// <param name="row"></param>
                /// <param name="col"></param>
                /// <param name="item"></param>
                public virtual void Insert2D(int row, int col, T item) {
                    Insert(row * _columns + col, item);
                }

                /// <summary>
                /// Removes item at specified row and column.
                /// </summary>
                /// <param name="row"></param>
                /// <param name="col"></param>
                public virtual void RemoveAt2D(int row, int col) {
                    RemoveAt(row * _columns + col);
                }

                /// <summary>
                /// Removes range of elements at specified row and column from list.
                /// </summary>
                /// <param name="row"></param>
                /// <param name="col"></param>
                /// <param name="count"></param>
                public virtual void RemoveRange2D(int row, int col, int count) {
                    RemoveRange(row * _columns + col, count);
                }
            }
        }
    }
}
