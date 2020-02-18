using System.IO;

namespace UnityUtilities {
    /// <summary>
    /// sdgdhbdhbd
    /// </summary>
    namespace FileSystem {

        /// <summary>
        /// Basic file loader class to load all kinds of files.
        /// </summary>
        public static class FileHelper {

            /// <summary>
            /// Loads text file.
            /// </summary>
            /// <param name="filePath">Absolute path of file.</param>
            /// <returns>Returns full text from text file.</returns>
            public static string OpenTextFile(string filePath) {
                if (File.Exists(filePath)) {
                    return File.ReadAllText(filePath);
                }
                return "";
            }

            /// <summary>
            /// Save string as text file.
            /// </summary>
            /// <param name="filePath">Absolute path at which to save file.</param>
            /// <param name="textData">String to be saved into a file.</param>
            public static void SaveTextFile(string filePath, string textData) {
                File.WriteAllText(filePath, textData);
            }

            /// <summary>
            /// Deletes file.
            /// </summary>
            /// <param name="filePath">Absolute path at which file is to be deleted.</param>
            public static void DeleteFile(string filePath) {
                if(File.Exists(filePath)) {
                    File.Delete(filePath);
                }
            }
        }
    }
}
