using System;
using System.Collections.Generic;
using Doozy.Engine.Utils.ColorModels;
using Unity.Mathematics;
using UnityEngine;

namespace UnityUtilities.Collections.Generic {
	public class SquareGrid<T> : List2D<T> {
		public float cellSize { get; private set; }
		public Vector3 worldPosition { get; private set; }
		
		public List<int2> edgeIndices { get; private set; }

		public SquareGrid(int width, int height, float cellSize, Vector3 worldPosition) : base(width, height) {
			this.cellSize = cellSize;
			this.worldPosition = worldPosition;
			edgeIndices = new List<int2>();
			for (var i = 0; i < width; i++) {
				for (var j = 0; j < height; j++) {
					var a = GetWorldPosition(i, j) - new Vector3(cellSize, worldPosition.y, cellSize) * 0.5f;
					var b = GetWorldPosition(i + 1, j) - new Vector3(cellSize, worldPosition.y, cellSize) * 0.5f;
					var c = GetWorldPosition(i, j + 1) - new Vector3(cellSize, worldPosition.y, cellSize) * 0.5f;
					Debug.DrawLine(a, b, Color.black, 100f);
					Debug.DrawLine(a, c, Color.black, 100f);
				}
			}
		}

		public void UpdateCellSize(float newCellSize) {
			if (newCellSize < 0) {
				return;
			}
			cellSize = newCellSize;
		}

		// Returns the Vector3 position of an item in the grid, centered in the cell it inhabits.
		public virtual Vector3 GetWorldPosition(int x, int y) {
			return (new Vector3(x, y) + worldPosition) * cellSize + new Vector3(cellSize, cellSize) * 0.5f;
		}

		// Sets references for the x and y index of a cell by a Vector3 world position.
		public virtual void GetIndexByWorldPosition(Vector3 position, out int x, out int y) {
			x = Mathf.FloorToInt((position.x - worldPosition.x) / cellSize);
			y = Mathf.FloorToInt((position.y - worldPosition.y) / cellSize);
		}

		public IEnumerable<int2> UpdateEdgeIndices (T root) {
			if (root == null) {
				return default;
			}
			IndexOf(root, out var originX, out var originY);
			DepthFirstSearch(root, originX, originY, new List<T>(),
			                 item => item == null,
			                 (item, x, y) => {
				                 if (IsOutOfBounds(x, y)) {
					                 return;
				                 } 
				                 edgeIndices.Add(new int2(x, y));
			                 });
			return edgeIndices;
		}

		public IEnumerable<T> GetAdjacentItems(T root) {
			if (root == null) {
				return default;
			}
			IndexOf(root, out var x, out var y);
			var adjacentItems =  new List<T> {
				Get(x + 1 ,y),
				Get(x - 1, y),
				Get(x, y + 1),
				Get(x, y - 1)
			};
			adjacentItems.RemoveAll(item => item == null);
			return adjacentItems;
		}

		private void DepthFirstSearch(T root, int x, int y, ICollection<T> searched, Func<T, bool> testPositiveBaseCase, Action<T, int, int> positiveBaseCaseReached) {
			if (testPositiveBaseCase(root)) {
				positiveBaseCaseReached(root, x, y);
			}
			
			if (root == null || searched.Contains(root)) {
				return;
			}

			searched.Add(root);
			
			for (var i = x - 1; i <= x + 1; i++) {
				for (var j = y - 1; j <= y + 1; j++) {
					var nextItem = Get(i, j);
					DepthFirstSearch(nextItem, i, j, searched, testPositiveBaseCase, positiveBaseCaseReached);
				}
			}
		}
	}
}