using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityUtilities {

    /// <summary>
    /// Contains utility functions for a variety of purposes.
    /// </summary>
    public static class Utils {

        /// <summary>
        /// Trims the name of the GameObject to reflect its name in the editor.
        /// </summary>
        /// <param name="g">GameObject of which you want the name.</param>
        /// <returns>Formatted GameObject name.</returns>
        public static string TrimGameObjectName(GameObject g) {
            var name = g.ToString();
            return name.Substring(0, name.IndexOf('(') - 1);
        }

        /// <summary>
        /// Determines if an array is full.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array to test.</param>
        /// <returns>Boolean.</returns>
        public static bool IsArrayFull<T>(T[] array) {
            foreach (var element in array) {
                if (element == null) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Finds the scene indices of all scenes that match the given Regex pattern.
        /// </summary>
        /// <param name="pattern">Pattern used to search for scenes.</param>
        /// <returns>List of scene indices.</returns>
        public static List<int> GetScenesBuildIndexByRegex(Regex pattern) {
            var levelSceneIndices = new List<int>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                var splitPath = SceneUtility.GetScenePathByBuildIndex(i).Split(new char[] { '/' });
                var sceneName = splitPath[splitPath.Length - 1].ToLower();

                if (pattern.IsMatch(sceneName)) {
                    levelSceneIndices.Add(i);
                }
            }

            return levelSceneIndices;
        }

        /// <summary>
        /// Gets all children of a GameObject with a given tag.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static GameObject[] GetChildrenWithTag(GameObject parent, string tag) {
            return (from child in GetAllChildren(parent)
                    where child.CompareTag(tag)
                    select child).ToArray();
        }

        /// <summary>
        /// Gets all children of a GameObject, including nested children.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<GameObject> GetAllChildren(GameObject parent) {
            return from transform in parent.GetComponentsInChildren<Transform>()
                    where transform && transform != parent.transform
                    select transform.gameObject;
        }
        
        /// <summary>
        /// Gets all children of a GameObject with component T, including nested children.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject[] GetAllChildren<T>(GameObject parent) where T : Component {
            return (from transform in parent.GetComponentsInChildren<T>()
                    where transform
                    select transform.gameObject).ToArray();
        }

        public static bool IsNumericType(object o) {
            switch (Type.GetTypeCode(o.GetType())) {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;   
            }
        }
    }
}
