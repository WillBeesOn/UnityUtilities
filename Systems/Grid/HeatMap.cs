using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtilities.Collections.Generic;

namespace UnityUtilities.Systems.Grid {
	public class HeatMap : GridController {
		[SerializeField] private int activityRadius;
		[SerializeField] private int activityValue;
		[SerializeField] private int debugTextFontSize;

		private Grid2D<int> _heatMapItems;
		private List2D<TextMeshPro> _gridText;

		// Callback to be used by new Unity input system.
		public void IndicateActivityOnClick(InputAction.CallbackContext context) {
			if (!context.performed) return;
			IndicateActivity(PhysicsUtils.GetMouseWorldPoint());
		}

		// Create a diamond pattern to indicate activity on the heat map. Diamond size is adjustable via activityRadius.
		private void IndicateActivity(Vector3 worldPoint) {
			_heatMapItems.IndexOf(worldPoint, out var x, out var y);
			if (x < 0 || y < 0) return;

			// Loop through top half of radius, including activated row.
			for (var vertical = 0; vertical <= activityRadius; vertical++) {
				var rowLimit = activityRadius - vertical;
				UpdateHorizontalHeatMapItems(x, y, vertical, rowLimit);
			}
			
			// Loop through bottom half of radius, excluding activated row.
			for (var vertical = -1; vertical >= -activityRadius; vertical--) {
				var rowLimit = activityRadius + vertical;
				UpdateHorizontalHeatMapItems(x, y, vertical, rowLimit);
			}
		}

		// Updates the values of heat map cells horizontally, updating 1 fewer each iteration to create a triangle pattern.
		private void UpdateHorizontalHeatMapItems(int x, int y, int vertical, int rowLimit) {
			var newY = y + vertical;
			for (var horizontal = Math.Abs(vertical) - activityRadius; horizontal <= rowLimit; horizontal++) {
				var newX = x + horizontal;
				var textMesh = _gridText.Get(newX, newY);
				if (textMesh == null) continue;

				var value = _heatMapItems.Get(newX, newY) + activityValue;
				textMesh.text = value.ToString();
				_heatMapItems.Set(value, newX, newY);
			}
		}

		protected override void Awake() {
			_gridText = new List2D<TextMeshPro>(width, height);
			
			// Initialize heat map with 0s.
			_heatMapItems = new Grid2D<int>(width, height, cellSize, transform.position, gridOrientation);
			for (var i = 0; i < width; i++) {
				for (var j = 0; j < height; j++) {
					_heatMapItems.Add(0);
				}
			}

			base.Awake();
		}

		// Create world text to display values of each heat map cell.
		protected override void DrawDebug() {
			base.DrawDebug();
			for (var y = 0; y < width; y++) {
				for (var x = 0; x < height; x++) {
					var newTextMesh = Utils.CreateWorldText("0", transform, _heatMapItems.GetWorldPosition(x, y), Color.white, debugTextFontSize);
					newTextMesh.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
					_gridText.Add(newTextMesh);
				}
			}
		}
	}
}