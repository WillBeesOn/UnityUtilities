using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.Collections.Generic;

namespace UnityUtilities.Systems.Grid {
	public class GridController : MonoBehaviour {
		[SerializeField] protected int width;
		[SerializeField] protected int height;
		[SerializeField] protected float cellSize;
		[SerializeField] protected GridOrientation gridOrientation;
		[SerializeField] private bool drawDebug;
		[SerializeField] private Color debugDrawColor;

		protected virtual void Awake() {
			if (drawDebug) DrawDebug();
		}

		// Draw debug lines for grid cells.
		protected virtual void DrawDebug() {
			for (var x = 0; x < width; x++) {
				for (var y = 0; y < height; y++) {
					Debug.DrawLine(GetGridCornerWorldPosition(x, y), GetGridCornerWorldPosition(x, y + 1), debugDrawColor, 1000f);
					Debug.DrawLine(GetGridCornerWorldPosition(x, y), GetGridCornerWorldPosition(x + 1, y), debugDrawColor, 1000f);
				}
			}

			Debug.DrawLine(GetGridCornerWorldPosition(0, height), GetGridCornerWorldPosition(width, height), debugDrawColor, 1000f);
			Debug.DrawLine(GetGridCornerWorldPosition(width, 0), GetGridCornerWorldPosition(width, height), debugDrawColor, 1000f);
		}

		// Get the location of the bottom left corner of grid cell.
		private Vector3 GetGridCornerWorldPosition(int x, int y) => (gridOrientation == GridOrientation.XZ ? new Vector3(x, 0, y) : new Vector3(x, y)) * cellSize + transform.position;
	}
}