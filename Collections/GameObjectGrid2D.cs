using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.Collections.Generic;

namespace UnityUtilities.Collections {
	
	public class GameObjectGrid2D : Grid2D<GameObject> {
		
		public GameObjectGrid2D(int x, int y, float cellSize, Vector3 origin) : this(x, y, cellSize, origin, GridOrientation.XY, new List<GameObject>(x * y)) { }
		
		public GameObjectGrid2D(int x, int y, float cellSize, Vector3 origin, GridOrientation orientation) : this(x, y, cellSize, origin, orientation, new List<GameObject>(x * y)) { }

		public GameObjectGrid2D(int x, int y, float cellSize, Vector3 origin, GridOrientation orientation, IEnumerable<GameObject> initialList) : base(x, y, cellSize, origin, orientation, initialList) {
			PositionAllObjects();
		}

		public new void Add(GameObject item) {
			base.Add(item);
			PositionObject(IndexOf(item));
		}

		public override void UpdateCellSize(float newCellSize) {
			base.UpdateCellSize(newCellSize);
			PositionAllObjects();
		}

		public override void UpdateOrigin(Vector3 newOrigin) {
			base.UpdateOrigin(newOrigin);
			PositionAllObjects();
		}

		private void PositionAllObjects() {
			for (var i = 0; i < Count; i++) {
				PositionObject(i);
			}
		}

		private void PositionObject(int index) {
			GetXYForIndex(index, out var x, out var y);
			base[index].transform.position = GetWorldPositionByIndex(x, y);
		}
	}
}