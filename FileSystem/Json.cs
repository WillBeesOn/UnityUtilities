using UnityEngine;

namespace UnityUtilities {
    namespace FileSystem {

        /// <summary>
        /// A collection of functions to assist in JSON processing.
        /// </summary>
        public static class Json {
            /// <summary>
            /// Deserialize JSON string into serializable object T.
            /// </summary>
            /// <typeparam name="T">Serializable type.</typeparam>
            /// <param name="filePath">Absolute path of file to read.</param>
            /// <returns></returns>
            public static T Deserialize<T>(string filePath) {
                string jsonString = FileHelper.OpenTextFile(filePath);
                return JsonUtility.FromJson<T>(jsonString);
            }

            /// <summary>
            /// Serializes object data model into JSON.
            /// </summary>
            /// <typeparam name="T">Serializable type.</typeparam>
            /// <param name="filePath">Absolute path at which to save file.</param>
            /// <param name="data">Object to be serialized into JSON.</param>
            public static void Serialize<T>(string filePath, T data) {
                FileHelper.SaveTextFile(filePath, JsonUtility.ToJson(data));
            }
        }
    }
}
