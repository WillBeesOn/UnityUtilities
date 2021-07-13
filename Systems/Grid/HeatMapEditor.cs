using System;
using UnityEditor;
using UnityEngine;

namespace UnityUtilities.Systems.Grid {
	[CustomEditor(typeof(HeatMap))]
	public class HeatMapEditor : Editor {
		// HeatMap and GridController fields
		[NonSerialized] private SerializedProperty _width;
		[NonSerialized] private SerializedProperty _height;
		[NonSerialized] private SerializedProperty _cellSize;
		[NonSerialized] private SerializedProperty _gridOrientation;
		[NonSerialized] private SerializedProperty _activityRadius;
		[NonSerialized] private SerializedProperty _activityValue;
		[NonSerialized] private SerializedProperty _drawDebug;
		[NonSerialized] private SerializedProperty _debugDrawColor;
		[NonSerialized] private SerializedProperty _debugTextColor;
		[NonSerialized] private SerializedProperty _debugTextFontSize;

		private void OnEnable() {
			_width = serializedObject.FindProperty("width");
			_height = serializedObject.FindProperty("height");
			_cellSize = serializedObject.FindProperty("cellSize");
			_gridOrientation = serializedObject.FindProperty("gridOrientation");
			_activityRadius = serializedObject.FindProperty("activityRadius");
			_activityValue = serializedObject.FindProperty("activityValue");
			_drawDebug = serializedObject.FindProperty("drawDebug");
			_debugDrawColor = serializedObject.FindProperty("debugDrawColor");
			_debugTextColor = serializedObject.FindProperty("debugTextColor");
			_debugTextFontSize = serializedObject.FindProperty("debugTextFontSize");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(_width, new GUIContent("Width", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_height, new GUIContent("Height", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_cellSize, new GUIContent("Cell Size", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_gridOrientation, new GUIContent("Grid Orientation", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_activityRadius, new GUIContent("Activity Radius", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_activityValue, new GUIContent("Activity Value", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_drawDebug, new GUIContent("Draw Debug", "Width of the heatmap"));
			
			EditorGUI.BeginDisabledGroup(!_drawDebug.boolValue);
			EditorGUILayout.PropertyField(_debugDrawColor, new GUIContent("Debug Grid Color", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_debugTextColor, new GUIContent("Debug Text Color", "Width of the heatmap"));
			EditorGUILayout.PropertyField(_debugTextFontSize, new GUIContent("Debug Text Font Size", "Width of the heatmap"));
			EditorGUI.EndChangeCheck();

			serializedObject.ApplyModifiedProperties();
		}
	}
}