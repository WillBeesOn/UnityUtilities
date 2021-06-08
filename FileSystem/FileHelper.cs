using System.IO;

namespace UnityUtilities.FileSystem {
	/// <summary>
	/// Basic file loader class to load all kinds of files.
	/// </summary>
	public static class FileHelper {
		/// <summary>
		/// Loads text file as string.
		/// </summary>
		/// <param name="filePath">Absolute path to file.</param>
		/// <returns>Returns full text from text file.</returns>
		public static string OpenTextFile(string filePath) {
			return File.Exists(filePath) ? File.ReadAllText(filePath) : default;
		}

		/// <summary>
		/// Save string as text file.
		/// </summary>
		/// <param name="filePath">Absolute path at which to save file.</param>
		/// <param name="textData">String to be saved into a file.</param>
		public static bool SaveTextFile(string filePath, string textData) {
			File.WriteAllText(filePath, textData);
			return File.Exists(filePath);
		}

		/// <summary>
		/// Loads file as bytes.
		/// </summary>
		/// <param name="filePath">Absolute path to file.</param>
		/// <returns>Returns all bytes of file</returns>
		public static byte[] OpenByteFile(string filePath) {
			return File.Exists(filePath) ? File.ReadAllBytes(filePath) : default;
		}

		/// <summary>
		/// Saves file as bytes
		/// </summary>
		/// <param name="filePath">Absolute path of file to save to</param>
		/// <param name="bytesToSave">Bytes to be saved into file</param>
		/// <returns></returns>
		public static bool SaveByteFile(string filePath, byte[] bytesToSave) {
			File.WriteAllBytes(filePath, bytesToSave);
			return File.Exists(filePath);
		}


		/// <summary>
		/// Deletes file.
		/// </summary>
		/// <param name="filePath">Absolute path at which file is to be deleted.</param>
		public static bool DeleteFile(string filePath) {
			if (File.Exists(filePath)) {
				File.Delete(filePath);
				return true;
			}

			return false;
		}
	}
}