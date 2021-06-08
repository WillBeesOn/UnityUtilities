using UnityEngine;

namespace UnityUtilities.FileSystem {
	/// <summary>
	/// A collection of functions to assist in JSON processing.
	/// </summary>
	public static class Json<T> where T : new() {
		/// <summary>
		/// Deserialize JSON string into serializable object T and overwrites passed data.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool Deserialize(string filePath, T data) {
			var jsonString = FileHelper.OpenTextFile(filePath);

			if (jsonString == null) {
				return false;
			}

			JsonUtility.FromJsonOverwrite(jsonString, data);
			return true;
		}

		/// <summary>
		/// Deserialize JSON string into serializable object T and returns deserialized data.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static T Deserialize(string filePath) {
			var jsonString = FileHelper.OpenTextFile(filePath);
			return jsonString == null ? default : JsonUtility.FromJson<T>(jsonString);
		}

		/// <summary>
		/// Serializes object data model into JSON and save it to local storage.
		/// </summary>
		/// <typeparam name="T">Serializable type.</typeparam>
		/// <param name="filePath">Absolute path at which to save file.</param>
		/// <param name="data">Object to be serialized into JSON.</param>
		public static void Serialize(string filePath, T data) {
			FileHelper.SaveTextFile(filePath, JsonUtility.ToJson(data));
		}
	}
}