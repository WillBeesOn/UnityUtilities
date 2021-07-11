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

		public Grid2D(int x, int y, float cellSize, Vector3 origin) : this(x, y, cellSize, origin, GridOrientation.XY, new List<T>(x * y)) { }

		public Grid2D(int x, int y, float cellSize, Vector3 origin, GridOrientation orientation, IEnumerable<T> initialList) : base(x, y, initialList) {
			_origin = origin;
			_cellSize = cellSize;
			_gridOrientation = orientation;
			DrawDebug();
		}

		public T Get(Vector3 position) {
			GetIndexByWorldPosition(position, out var x, out var y);
			return Get(x, y);
		}

		public virtual void Set(T value, Vector3 position) {
			GetIndexByWorldPosition(position, out var x, out var y);
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

		protected Vector3 GetWorldPositionByIndex(int x, int y) {
			var xPos = x * _cellSize + _cellSize / 2;
			var yPos = y * _cellSize + _cellSize / 2;
			return (_gridOrientation == GridOrientation.XZ ? new Vector3(xPos, 0, yPos) : new Vector3(xPos, yPos, 0)) + _origin;
		}

		private void GetIndexByWorldPosition(Vector3 position, out int x, out int y) {
			x = Mathf.FloorToInt((position.x - _origin.x) / _cellSize);
			y = Mathf.FloorToInt((_gridOrientation == GridOrientation.XZ ? position.z - _origin.z : position.y - _origin.y) / _cellSize);
		}

		private void DrawDebug() {
			for (var x = 0; x < width; x++) {
				for (var y = 0; y < height; y++) {
					Debug.DrawLine(GetGridCornerWorldPosition(x, y), GetGridCornerWorldPosition(x, y + 1), Color.red, 1000f);
					Debug.DrawLine(GetGridCornerWorldPosition(x, y), GetGridCornerWorldPosition(x + 1, y), Color.red, 1000f);
				}
			}

			Debug.DrawLine(GetGridCornerWorldPosition(0, height), GetGridCornerWorldPosition(width, height), Color.red, 1000f);
			Debug.DrawLine(GetGridCornerWorldPosition(width, 0), GetGridCornerWorldPosition(width, height), Color.red, 1000f);
		}

		private Vector3 GetGridCornerWorldPosition(int x, int y) => (_gridOrientation == GridOrientation.XZ ? new Vector3(x, 0, y) : new Vector3(x, y)) * _cellSize + _origin;
	}
}