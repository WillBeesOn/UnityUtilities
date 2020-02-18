using System.IO;
using UnityEngine;

namespace UnityUtilities {
    namespace FileSystem {

        /// <summary>
        /// Save system that creates save files as JSON.
        /// </summary>
        /// <typeparam name="T">Serializable type.</typeparam>
        public class SaveFile<T> where T : new() {

            /// <summary>
            /// Object that represents the data model for the save file.
            /// </summary>
            public T Model { get; private set; }

            /// <summary>
            /// Used to tell if a save file already exists at the file location of the SaveFile object.
            /// </summary>
            public bool existingSaveFile;

            private string _savePath = Application.persistentDataPath;

            /// <summary>
            /// Create SaveFile object.
            /// Default save location is at the persistent data path.
            /// </summary>
            /// <param name="fileName">Name of save file including extension.</param>
            /// <param name="optionalPath">Absolute path at which to save the file.</param>
            public SaveFile(string fileName, string optionalPath = "") {
                if (optionalPath.Length > 0) {
                    _savePath = optionalPath;
                }
                _savePath = Path.Combine(_savePath, fileName);
                Load();
            }

            /// <summary>
            /// Saves current model to local storage.
            /// </summary>
            public void Save() {
                Json.Serialize(_savePath, Model);
            }

            /// <summary>
            /// Saves given model to local storage.
            /// </summary>
            /// <param name="newModel">New model to replace the current one.</param>
            public void Save(T newModel) {
                Model = newModel;
                Save();
            }

            /// <summary>
            /// Loads JSON file and deserializes it.
            /// </summary>
            /// <returns>Object containing deserialized JSON data.</returns>
            public T Load() {
                T deserializedFile = Json.Deserialize<T>(_savePath);
                Model = new T();

                if (deserializedFile != null) {
                    Model = deserializedFile;
                    existingSaveFile = true;
                }

                return Model;
            }

            /// <summary>
            /// Deletes save file represented by this object.
            /// </summary>
            public void Delete() {
                FileHelper.DeleteFile(_savePath);
            }
        }
    }
}
