﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
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
            string name = g.ToString();
            return name.Substring(0, name.IndexOf('(') - 1);
        }

        /// <summary>
        /// Determines if an array is full.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array to test.</param>
        /// <returns>Boolean.</returns>
        public static bool IsArrayFull<T>(T[] array) {
            foreach (T element in array) {
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
            List<int> levelSceneIndices = new List<int>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                string[] splitPath = SceneUtility.GetScenePathByBuildIndex(i).Split(new char[] { '/' });
                string sceneName = splitPath[splitPath.Length - 1].ToLower();

                if (pattern.IsMatch(sceneName)) {
                    levelSceneIndices.Add(i);
                }
            }

            return levelSceneIndices;
        }
    }
}