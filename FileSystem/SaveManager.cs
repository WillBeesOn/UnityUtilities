using System.IO;
using System.Collections.Generic;
using UnityEngine;

/*
 * Improvements
 * Save on game quit
 * Use binary formatter. Does it really need to be put in JSON at all?
 * Can List<T> be serialized granted that T is serializable?
 * Create a separate serialization package
 */
namespace UnityUtilities.FileSystem {
	/// <summary>
	/// Manages multiple save files saved to local storage. Supports using a custom model for the save files.
	/// </summary>
	/// <typeparam name="TData">Model of save file</typeparam>
	public class SaveManager<TData> where TData : new() {
		/// <summary>
		/// Get a list of the file names of the save files managed by this instance of SaveManager.
		/// </summary>
		public List<string> fileNames => _fileNames;

		private readonly List<string> _fileNames;
		private readonly string _savePath = Application.persistentDataPath;
		private readonly string _fileTemplatePattern;
		private readonly string _fileNameTemplate;
		private readonly string _fileExtension;

		/// <summary>
		/// Create a SaveManager instance that manges save files.
		/// </summary>
		/// <param name="fileNameTemplate">Template for file names. This will be used to create save files
		/// and find save files to load. Please include file extension. Save files are numbered starting from 1.
		/// Numbers will be inserted where a pair of curly braces are found.
		/// (Example. "saveFile{example}.save", "save{}file.save")
		/// </param>
		/// <param name="savePath">Optional path to save files. Default is the persistent data path.</param>
		public SaveManager(string fileNameTemplate, string savePath = "") {
			var splitTemplate = fileNameTemplate.Split('.');
			_fileExtension = $".{splitTemplate[splitTemplate.Length - 1]}";
			_fileNameTemplate = fileNameTemplate.Substring(0, fileNameTemplate.IndexOf('.'));

			if (savePath.Length > 0) {
				_savePath = savePath;
			}

			var openBracketIndex = fileNameTemplate.IndexOf('{');
			var length = fileNameTemplate.IndexOf('}') - openBracketIndex + 1;
			_fileTemplatePattern = fileNameTemplate.Substring(openBracketIndex, length);

			var filesArray = Directory.GetFiles(_savePath,
			                                    fileNameTemplate.Replace(_fileTemplatePattern, "*"),
			                                    SearchOption.TopDirectoryOnly);
			_fileNames = new List<string>(filesArray);
		}

		/// <summary>
		/// Creates a new save file initialized with given data.
		/// </summary>
		/// <param name="initialData">Initial data to create save file.</param>
		/// <returns>If saving was successful</returns>
		public bool CreateNewSaveFile(TData initialData) {
			var newFileName =
				_fileNameTemplate.Replace(_fileTemplatePattern, $"{_fileNames.Count + 1}") +
				_fileExtension;
			var filePath = Path.Combine(_savePath, newFileName);
			if (File.Exists(filePath)) {
				return false;
			}

			Json<TData>.Serialize(filePath, initialData);
			_fileNames.Add(newFileName);
			return true;
		}

		/// <summary>
		/// Overwrites save file at index with data.
		/// </summary>
		/// <param name="index">Index of save file to overwrite</param>
		/// <param name="data">Data you want saved</param>
		/// <returns>If overwriting save was successful</returns>
		public bool OverwriteSaveFile(int index, TData data) {
			var filePath = Path.Combine(_savePath, _fileNames[index]);
			if (!File.Exists(filePath)) {
				return false;
			}

			Json<TData>.Serialize(filePath, data);
			return true;
		}

		/// <summary>
		/// Overwrites currently loaded save data with save file at index.
		/// </summary>
		/// <param name="index">Index of save file to load</param>
		/// <param name="currentData">Current save file loaded</param>
		/// <returns>If loading was successful</returns>
		public bool LoadSaveFile(int index, TData currentData) {
			return Json<TData>.Deserialize(Path.Combine(_savePath, _fileNames[index]), currentData);
		}

		/// <summary>
		/// Deletes save file at index.
		/// </summary>
		/// <param name="index">Index of file to delete</param>
		/// <returns>If deleting was successful</returns>
		public bool DeleteSaveFile(int index) {
			return FileHelper.DeleteFile(Path.Combine(_savePath, _fileNames[index]));
		}
	}
}