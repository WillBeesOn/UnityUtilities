using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities {
    namespace Collections {

        /// <summary>
        /// A collection of pre-instantiated GameObjects to be used when needed.
        /// </summary>
        public class ObjectPool {

            /// <summary>
            /// List of GameObjects that are currently unused and in the pool.
            /// </summary>
            public List<GameObject> pooledObjects { get; private set; }

            /// <summary>
            /// List of GameObjects that are currently in use.
            /// </summary>
            public List<GameObject> objectsInUse { get; private set; }

            /// <summary>
            /// Create an object pool. Deactivates all instantiated instances of GameObjects.
            /// </summary>
            /// <param name="objToPool">GameObject you want to have pre-allocated.</param>
            /// <param name="poolSize">Number of GameObjects to have allocated.</param>
            public ObjectPool(GameObject objToPool, int poolSize) {
                GameObject instantiatedObj;
                pooledObjects = new List<GameObject>();
                objectsInUse = new List<GameObject>();
                for (int i = 0; i < poolSize; i++) {
                    instantiatedObj = Object.Instantiate(objToPool);
                    instantiatedObj.SetActive(false);
                    pooledObjects.Add(instantiatedObj);
                }
            }

            /// <summary>
            /// Return GameObject back to the pool. It is marked as available for use and deactivated.
            /// </summary>
            /// <param name="returnedObj">GameObject to return to the pool.</param>
            public void ReturnToPool(GameObject returnedObj) {
                if (objectsInUse.Contains(returnedObj)) {
                    returnedObj.SetActive(false);
                    pooledObjects.Add(returnedObj);
                    objectsInUse.Remove(returnedObj);
                }
            }

            /// <summary>
            /// Returns one GameObject that is available. It is marked it as being used and is activated. 
            /// </summary>
            /// <returns>Next available unused GameObject.</returns>
            public GameObject UseObject() {
                GameObject objToUse = null;
                if (pooledObjects.Count > 0) {
                    objToUse = pooledObjects[0];
                    objectsInUse.Add(objToUse);
                    pooledObjects.Remove(objToUse);
                    objToUse.SetActive(true);
                }
                return objToUse;
            }
        }
    }
}
