using UnityEngine;
using Unity.Mathematics;
using UnityUtilities.Collections.Generic;

namespace UnityUtilities.Collections {

	public class GridItem {
		public int width;
		public int height;
		public GameObject gameObject;
	}

	public class GridItemProxy : GridItem {
		public GridItem associatedGridItem;
		public int2 localProxyIndex;
	}

	public class ObjectGrid : SquareGridXZ<GridItem> {

		public ObjectGrid(int width, int height, float cellSize, Vector3 worldPosition) : base(width, height, cellSize, worldPosition){ }

		public override void Insert(int x, int y, GridItem item) {
			base.Insert(x, y, item);
			PositionItem(item);
		}

		private void UpdateGridItems() {
			foreach (var g in this) {
				PositionItem(g);
			}
		}

		private void PositionItem(GridItem item) {
			IndexOf(item, out var x, out var y);
			item.gameObject.transform.position = GetWorldPosition(x, y) +  new Vector3(item.width / 2, 0, item.height / 2) * cellSize;
		}
	}
}