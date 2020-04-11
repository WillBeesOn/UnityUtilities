using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.Collections.Generic;

namespace UnityUtilities {
    namespace Collections {
        namespace GameObjects {
            // TODO Add animation options when grid is updated.
            // Support GridMode. Look at EntityGrid2D for implementation.
            // Needs testing.

            /// <summary>
            /// A list of GameObjects that are arranged in a 2D grid.
            /// Handles positioning and updating GameObjects in the grid when
            /// ell size or padding are updated.
            /// </summary>
            public class ObjectGrid2D : List2D<GameObject> {
                /// <summary>
                /// How large each cell of the grid is in Unity units.
                /// </summary>
                public Vector2 CellSize { get; private set; }

                private const int _pixelsPerUnit = 100;

                /// <summary>
                /// Creates a 1x1 grid with cell size and padding of 0x0.
                /// </summary>
                public ObjectGrid2D() : this(1, 1) { }

                /// <summary>
                /// Creates a grid with rows and columns. Cell size and padding of 0x0.
                /// </summary>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                public ObjectGrid2D(int rows, int cols) : this(rows, cols, new Vector2(0, 0)) { }

                /// <summary>
                /// Creates a grid with rows, columns, and cell size. Padding of 0x0.
                /// </summary>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                /// <param name="cellSize"></param>
                public ObjectGrid2D(int rows, int cols, Vector2 cellSize) : this(rows, cols, cellSize, new Vector2(0, 0)) { }

                /// <summary>
                /// Creates a grid with rows, columns, cell size, and padding.
                /// </summary>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                /// <param name="cellSize"></param>
                /// <param name="padding"></param>
                public ObjectGrid2D(int rows, int cols, Vector2 cellSize, Vector2 padding) : this(new List<GameObject>(), rows, cols, cellSize, padding) { }

                /// <summary>
                /// Creates a grid from a list with rows and columns. Cell size and padding of 0x0.
                /// </summary>
                /// <param name="initialList"></param>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                public ObjectGrid2D(IEnumerable<GameObject> initialList, int rows, int cols) : this(initialList, rows, cols, new Vector2(0, 0)) { }

                /// <summary>
                /// Creates a grid from a list with rows, columns, and cell size. Padding of 0x0.
                /// </summary>
                /// <param name="initialList"></param>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                /// <param name="cellSize"></param>
                public ObjectGrid2D(IEnumerable<GameObject> initialList, int rows, int cols, Vector2 cellSize) : this(initialList, rows, cols, cellSize, new Vector2(0, 0)) { }

                /// <summary>
                /// Creates a grid from a list with rows, columns, cell size, and padding.
                /// </summary>
                /// <param name="initialList"></param>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                /// <param name="cellSize"></param>
                /// <param name="padding"></param>
                public ObjectGrid2D(IEnumerable<GameObject> initialList, int rows, int cols, Vector2 cellSize, Vector2 padding) : base(initialList, rows, cols) {
                    CellSize = cellSize;

                    if (CellSize == new Vector2(0, 0) && Count > 0) {
                        GameObject item = base[0];
                        float x = item.GetComponent<SpriteRenderer>().sprite.rect.width / _pixelsPerUnit;
                        float y = item.GetComponent<SpriteRenderer>().sprite.rect.height / _pixelsPerUnit;
                        CellSize = new Vector2(x, y);
                    }
                    UpdateGridItems();
                }

                /// <summary>
                /// Add an item to the grid and position it accordingly.
                /// </summary>
                /// <param name="item"></param>
                public new void Add(GameObject item) {
                    base.Add(item);
                    PositionItem(item);
                }

                /// <summary>
                /// Remove an object from the grid;
                /// </summary>
                /// <param name="item"></param>
                /// <returns></returns>
                public new bool Remove(GameObject item) {
                    bool removed = base.Remove(item);

                    if (removed) {
                        UpdateGridItems();
                    }
                    return removed;
                }

                /// <summary>
                /// Set the size each cell in the grid should be. Position of GameObjects are updated.
                /// </summary>
                /// <param name="cellSize"></param>
                public void SetCellSize(Vector2 cellSize) {
                    CellSize = cellSize;
                    UpdateGridItems();
                }

                /// <summary>
                /// Updates the position of each item in the grid.
                /// </summary>
                private void UpdateGridItems() {
                    foreach (GameObject g in this) {
                        PositionItem(g);
                    }
                }

                /// <summary>
                /// Position item based on cell size and padding.
                /// </summary>
                /// <param name="item"></param>
                private void PositionItem(GameObject item) {
                    Tuple<int, int> targetIndex = IndexOf2D(item);

                    float x = (targetIndex.Item2 * CellSize.x) - (_columns * CellSize.x / 2);
                    float y = (-targetIndex.Item1 * CellSize.y) - (_rows * CellSize.y / 2);

                    item.transform.localPosition = new Vector3(x, y, item.transform.localPosition.z);
                }
            }
        }
    }
}