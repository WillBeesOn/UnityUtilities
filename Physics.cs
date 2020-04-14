using UnityEngine;

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
    }
}