using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.Collections.Generic {
	public enum GridOrientation {
		XY,
		XZ
	}

	public class Grid2D<T> : List2D<T> {
		private Vector3 _origin;
		private float _cellSize;
		private GridOrientation _gridOrientation;

		public Grid2D(int x, int y, float cellSize, Vector3 origin, GridOrientation orientation = GridOrientation.XZ) : this(x, y, cellSize, origin, new List<T>(x * y), orientation) { }

		public Grid2D(int x, int y, float cellSize, Vector3 origin, IEnumerable<T> initialList, GridOrientation orientation = GridOrientation.XY) : base(x, y, initialList) {
			_origin = origin;
			_cellSize = cellSize;
			_gridOrientation = orientation;
		}

		public T Get(Vector3 position) {
			IndexOf(position, out var x, out var y);
			return Get(x, y);
		}

		public virtual void Set(T value, Vector3 position) {
			IndexOf(position, out var x, out var y);
			Set(value, x, y);
		}

		public virtual void UpdateCellSize(float newCellSize) {
			if (newCellSize <= 0) return;
			_cellSize = newCellSize;
		}

		public virtual void UpdateOrigin(Vector3 newOrigin) {
			_origin = newOrigin;
		}

		public virtual void ChangeGridOrientation(GridOrientation newOrientation) {
			_gridOrientation = newOrientation;
		}

		// Returns world position of center of a grid cell.
		public Vector3 GetWorldPosition(int x, int y) {
			var xPos = x * _cellSize + _cellSize / 2;
			var yPos = y * _cellSize + _cellSize / 2;
			return (_gridOrientation == GridOrientation.XZ ? new Vector3(xPos, 0, yPos) : new Vector3(xPos, yPos, 0)) + _origin;
		}

		public void IndexOf(Vector3 position, out int x, out int y) {
			x = Mathf.FloorToInt((position.x - _origin.x) / _cellSize);
			y = Mathf.FloorToInt((_gridOrientation == GridOrientation.XZ ? position.z - _origin.z : position.y - _origin.y) / _cellSize);
		}
	}
}