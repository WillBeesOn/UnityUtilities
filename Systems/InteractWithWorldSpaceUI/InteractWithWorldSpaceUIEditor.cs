#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

namespace UnityUtilities.Systems.InteractWithWorldSpaceUI {
	[CustomEditor(typeof(InteractWithWorldSpaceUI))]
	public class InteractWithWorldSpaceUIEditor : Editor {
		// InteractWithWorldSpaceUI properties
		[NonSerialized] private SerializedProperty _mainCamera;
		[NonSerialized] private SerializedProperty _controls;
		[NonSerialized] private SerializedProperty _targetActionMap;
		[NonSerialized] private SerializedProperty _targetAction;
		[NonSerialized] private SerializedProperty _uiPopUp;
		[NonSerialized] private SerializedProperty _uiPopUpTrigger;
		[NonSerialized] private SerializedProperty _objectToSendOnEnterEvent;
		[NonSerialized] private SerializedProperty _objectToSendOnInteractEvent;
		[NonSerialized] private SerializedProperty _uOnEnter;
		[NonSerialized] private SerializedProperty _uOnInteract;

		// Render ActionMap and Action lists in the editor.
		[NonSerialized] private GUIContent[] _actionMapRenderedList;
		[NonSerialized] private GUIContent[] _actionRenderedList;

		// Fields to keep track of selected Input system values
		[NonSerialized] private int _currentActionMap;
		[NonSerialized] private int _currentAction;
		[NonSerialized] private bool _showActions;
		[NonSerialized] private bool _inputAssetInitialized;

		// Fields for Events
		[NonSerialized] private bool _showUnityEvents;

		// Tooltip text
		[NonSerialized] private readonly GUIContent _actionMapSelectTooltip =
			EditorGUIUtility.TrTextContent("Target Action Map", "Action Map that contains the target Action that should activate an \"interaction\"");

		[NonSerialized] private readonly GUIContent _actionSelectTooltip =
			EditorGUIUtility.TrTextContent("Target Action", "Name of the target Action that should activate an \"interaction\"");

		[NonSerialized] private readonly GUIContent _eventsExpandedTooltip =
			EditorGUIUtility.TrTextContent("Events", "Put event listeners here");

		[NonSerialized] private readonly GUIContent _onEnterEventTooltip =
			EditorGUIUtility.TrTextContent("On Enter", "Invoked when UI popup is triggered");

		[NonSerialized] private readonly GUIContent _onInteractEventTooltip =
			EditorGUIUtility.TrTextContent("On Interact", "Invoked when an \"interaction\" is triggered");

		[NonSerialized] private readonly GUIContent _eventListenerPopupTooltip =
			EditorGUIUtility.TrTextContent("", "Event listener function");

		[NonSerialized] private GUIContent _controlsTooltip;
		[NonSerialized] private GUIContent _mainCameraTooltip;
		[NonSerialized] private GUIContent _uiPopUpTooltip;
		[NonSerialized] private GUIContent _uiPopUpTriggerTooltip;
		[NonSerialized] private GUIContent _objectToSendOnEnterEventToolTip;
		[NonSerialized] private GUIContent _objectToSendOnInteractEventToolTip;

		private void OnEnable() {
			// Get properties from associated serializedObject
			_mainCamera = serializedObject.FindProperty("mainCamera");
			_controls = serializedObject.FindProperty("controls");
			_targetActionMap = serializedObject.FindProperty("targetActionMap");
			_targetAction = serializedObject.FindProperty("targetAction");
			_uiPopUp = serializedObject.FindProperty("uiPopUp");
			_uiPopUpTrigger = serializedObject.FindProperty("uiPopUpTrigger");
			_objectToSendOnEnterEvent = serializedObject.FindProperty("objectToSendOnEnterEvent");
			_objectToSendOnInteractEvent = serializedObject.FindProperty("objectToSendOnInteractEvent");
			_uOnEnter = serializedObject.FindProperty("uOnEnter");
			_uOnInteract = serializedObject.FindProperty("uOnInteract");

			_controlsTooltip = EditorGUIUtility.TrTextContent(_controls.displayName, "Unity Input System asset you are using");
			_mainCameraTooltip = EditorGUIUtility.TrTextContent(_mainCamera.displayName, "Camera for UI popup to look at");
			_uiPopUpTooltip = EditorGUIUtility.TrTextContent(_uiPopUp.displayName, "The UI GameObject to toggle when triggered");
			_uiPopUpTriggerTooltip = EditorGUIUtility.TrTextContent(_uiPopUpTrigger.displayName, "Trigger toggling the UI popup when this GameObject enters the attached trigger Collider");
			_objectToSendOnEnterEventToolTip = EditorGUIUtility.TrTextContent(_objectToSendOnEnterEvent.displayName, "GameObject to send to subscribers of OnEnter event");
			_objectToSendOnInteractEventToolTip = EditorGUIUtility.TrTextContent(_objectToSendOnInteractEvent.displayName, "GameObject to send to subscribers of OnInteract event");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(_mainCamera, _mainCameraTooltip);

			// Check if a new InputActionAsset was added.
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Input Settings", EditorStyles.boldLabel);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_controls, _controlsTooltip);
			var inputAsset = (InputActionAsset) _controls.objectReferenceValue;

			// If a valid InputActionAsset was entered, display the selection for ActionMaps and Actions
			++EditorGUI.indentLevel;
			if (inputAsset != null) {
				// If a new asset was added or user has switched inspectors, update currently selected ActionMaps and Actions.
				if (EditorGUI.EndChangeCheck() || !_inputAssetInitialized) {
					UpdateActionMaps(inputAsset);
					UpdateActionsInMap(inputAsset);
					_inputAssetInitialized = true;
				}

				// Render dropdown list of ActionMaps and change selected map when user changes the dropdown.
				if (_actionMapRenderedList != null && _actionMapRenderedList.Length > 0) {
					var selectedActionMap = EditorGUILayout.Popup(_actionMapSelectTooltip, _currentActionMap, _actionMapRenderedList);

					// If newly selected map in this render iteration doesn't equal the previously selected one, update the selected ActionMap;
					if (selectedActionMap != _currentActionMap) {
						_currentActionMap = selectedActionMap;
						if (selectedActionMap == 0) {
							// No ActionMap is selected. Don't show Actions since there are none.
							_targetActionMap.stringValue = null;
							_showActions = false;
						} else {
							// An actual ActionMap is selected. Find its ID and save it to the serializedObject. Get updated list of Actions from selected map.
							var actionMap = inputAsset.FindActionMap(_actionMapRenderedList[selectedActionMap].text);
							if (actionMap != null) {
								_targetActionMap.stringValue = actionMap.id.ToString();
								UpdateActionsInMap(inputAsset);
							}
						}
					}

					// Render dropdown list of Actions within selected ActionMap.
					if (_showActions) {
						var selectedAction = EditorGUILayout.Popup(_actionSelectTooltip, _currentAction, _actionRenderedList);
						if (selectedAction != _currentAction) {
							if (selectedAction == 0) _targetAction.stringValue = null;
							else {
								var action = inputAsset.FindActionMap(_actionMapRenderedList[_currentActionMap].text).FindAction(_actionRenderedList[selectedAction].text);
								if (action != null) {
									_targetAction.stringValue = action.id.ToString();
								}
							}

							_currentAction = selectedAction;
						}
					}
				}
			}

			--EditorGUI.indentLevel;

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_uiPopUp, _uiPopUpTooltip);
			EditorGUILayout.PropertyField(_uiPopUpTrigger, _uiPopUpTriggerTooltip);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(_objectToSendOnEnterEvent, _objectToSendOnEnterEventToolTip);
			EditorGUILayout.PropertyField(_objectToSendOnInteractEvent, _objectToSendOnInteractEventToolTip);

			EditorGUILayout.Separator();

			_showUnityEvents = EditorGUILayout.Foldout(_showUnityEvents, "Unity Events");
			if (_showUnityEvents) {
				EditorGUILayout.PropertyField(_uOnEnter, new GUIContent("On Enter"));
				EditorGUILayout.PropertyField(_uOnInteract, new GUIContent("On Interact"));
			}


			// Update serialized fields in associated serializedObject.
			serializedObject.ApplyModifiedProperties();
		}

		// Update list of ActionMaps to render as a list from an InputActionAsset
		private void UpdateActionMaps(InputActionAsset inputAsset) {
			if (inputAsset == null) {
				_actionMapRenderedList = null;
				return;
			}

			var actionMaps = inputAsset.actionMaps;
			_actionMapRenderedList = new GUIContent[actionMaps.Count + 1];
			_actionMapRenderedList[0] = new GUIContent(EditorGUIUtility.TrTextContent("<None>"));

			// Build list of ActionMaps to render and update currently selected one based on what the serializedObject already has stored.
			for (var i = 0; i < actionMaps.Count; i++) {
				_actionMapRenderedList[i + 1] = new GUIContent(actionMaps[i].name);
				if (_targetActionMap != null && actionMaps[i].id.ToString() == _targetActionMap.stringValue) {
					_currentActionMap = i + 1;
				}
			}
		}

		// Update list of Actions to render if an ActionMap is selected.
		private void UpdateActionsInMap(InputActionAsset inputAsset) {
			if (inputAsset == null || _currentActionMap == 0) {
				_actionRenderedList = null;
				return;
			}

			var actions = inputAsset.actionMaps[_currentActionMap - 1].actions;
			_actionRenderedList = new GUIContent[actions.Count + 1];
			_actionRenderedList[0] = new GUIContent(EditorGUIUtility.TrTextContent("<None>"));

			// Build list of Actions to render and update currently selected one based on what the serializedObject already has stored.
			for (var i = 0; i < actions.Count; i++) {
				_actionRenderedList[i + 1] = new GUIContent(actions[i].name);
				if (_targetActionMap != null && actions[i].id.ToString() == _targetAction.stringValue) {
					_currentAction = i + 1;
				}
			}

			_showActions = true;
		}
	}
}
#endif