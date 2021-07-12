using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
		public static GameObject RaycastToGameObject(Vector3 position) {
			if (!IsMouseOverUI() && Physics.Raycast(_mainCamera.ScreenPointToRay(position), out var hit)) {
				return hit.transform.gameObject;
			}

			return default;
		}

		public static Vector3 RaycastToPoint(Vector3 position) {
			if (!IsMouseOverUI() && Physics.Raycast(_mainCamera.ScreenPointToRay(position), out var hit)) {
				return hit.point;
			}

			return default;
		}

		public static Vector3 GetMouseWorldPoint() {
			#if ENABLE_INPUT_SYSTEM
			var mousePos = Mouse.current.position.ReadValue();
			#else
			var mousePos = Input.mousePosition;
			#endif
			if (!IsMouseOverUI() && Physics.Raycast(_mainCamera.ScreenPointToRay(mousePos), out var hit)) {
				return hit.point;
			}

			return default;
		}

		public static Vector3 GetMouseGameObject() {
			#if ENABLE_INPUT_SYSTEM
			var mousePos = Mouse.current.position.ReadValue();
			#else
			var mousePos = Input.mousePosition;
			#endif
			if (!IsMouseOverUI() && Physics.Raycast(_mainCamera.ScreenPointToRay(mousePos), out var hit)) {
				return hit.point;
			}

			return default;
		}

		/// <summary>
		/// Determines if mouse is over UI elements.
		/// </summary>
		/// <returns></returns>
		private static bool IsMouseOverUI() {
			#if ENABLE_INPUT_SYSTEM
			var mousePos = Mouse.current.position.ReadValue();
			#else
			var mousePos = Input.mousePosition;
			#endif
			var pointerData = new PointerEventData(EventSystem.current) {
				position = mousePos
			};
			var results = new List<RaycastResult>();

			if (EventSystem.current == null) return false;

			EventSystem.current.RaycastAll(pointerData, results);
			return results.Count > 0;
		}
	}
}