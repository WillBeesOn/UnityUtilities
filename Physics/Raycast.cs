using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities {

    public static class Physics {

        public static GameObject Raycast(Ray r) {
            RaycastHit hit = new RaycastHit();
            if (UnityEngine.Physics.Raycast(r, out hit)) {
                return hit.transform.gameObject;
            }
            return default;
        }
    }
}