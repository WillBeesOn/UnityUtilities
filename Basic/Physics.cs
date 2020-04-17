using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityUtilities {

    public static class PhysicsUtils {

        /// <summary>
        /// Cast Ray r and return first GameObject that's hit.
        /// Checks if pointer/mouse is over UI before raycasting.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static GameObject Raycast(Ray r) {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(r, out hit) && !Utils.IsMouseOverUI()) {
                return hit.transform.gameObject;
            }
            return default;
        }

        /// <summary>
        /// Determines if mouse is over UI elements.
        /// </summary>
        /// <returns></returns>
        public static bool IsMouseOverUI() {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            return results.Count > 0;
        }
    }
}