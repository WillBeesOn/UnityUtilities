using UnityEngine;
using UnityUtilities.Collections.Generic;

namespace UnityUtilities.Collections.Generic {
	public class SquareGridXZ<T> : SquareGrid<T> {

		public SquareGridXZ(int width, int height, float cellSize, Vector3 worldPosition) : base(width, height, cellSize, worldPosition) { }

		public override Vector3 GetWorldPosition(int x, int y) {
			return (new Vector3(x, 0, y) + worldPosition) * cellSize + new Vector3(cellSize, cellSize) * 0.5f;
		}
		
		public override void GetIndexByWorldPosition(Vector3 position, out int x, out int z) {
			x = Mathf.FloorToInt((position.x - worldPosition.x) / cellSize);
			z = Mathf.FloorToInt((position.z - worldPosition.z) / cellSize);
		}
	}
}