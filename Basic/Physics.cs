﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityUtilities {
	public static class PhysicsUtils {
		private static readonly Camera _mainCamera = Camera.main;
		
		/// <summary>
		/// Cast Ray r and return first GameObject that's hit.
		/// Checks if pointer/mouse is over UI before raycasting.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="ignoreUI"></param>
		/// <returns></returns>
		public static GameObject Raycast(Vector3 position, bool ignoreUI = false) {
			var hit = new RaycastHit();
			if (Physics.Raycast(_mainCamera.ScreenPointToRay(position), out hit) && (ignoreUI || !IsMouseOverUI())) {
				return hit.transform.gameObject;
			}

			return default;
		}

		public static Vector3 RaycastPoint(Vector3 position, bool ignoreUI = false) {
			var hit = new RaycastHit();
			if (Physics.Raycast(_mainCamera.ScreenPointToRay(position), out hit) && (ignoreUI || !IsMouseOverUI())) {
				return hit.point;
			}

			return default;
		}

		/// <summary>
		/// Determines if mouse is over UI elements.
		/// </summary>
		/// <returns></returns>
		private static bool IsMouseOverUI() {
			var pointerData = new PointerEventData(EventSystem.current) {
				position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
			};
			var results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
			return results.Count > 0;
		}
	}
}