#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityUtilities.Systems.KeyDoor {
	[CustomEditor(typeof(KeyLock))]
	public class KeyLockEditor : Editor {
		// Key properties
		[NonSerialized] private SerializedProperty _unlockMethod;
		[NonSerialized] private SerializedProperty _requiredKeys;
		[NonSerialized] private SerializedProperty _unlockDelay;
		[NonSerialized] private SerializedProperty _onUnlockedAnimTrigger;
		[NonSerialized] private SerializedProperty _lockedAnimTrigger;
		[NonSerialized] private SerializedProperty _uDoorLocked;
		[NonSerialized] private SerializedProperty _uDoorUnlocked;

		// Animation rendered list
		[NonSerialized] private GUIContent[] _animatorTriggersRenderedList;

		// Keep track of Animation settings
		[NonSerialized] private int _currentUnlockedAnimatorTrigger;
		[NonSerialized] private int _currentLockedAnimatorTrigger;
		[NonSerialized] private bool _manageAnimation;
		[NonSerialized] private bool _initializedAnimatorTriggers;
		[NonSerialized] private Animator _attachedAnimator;
		[NonSerialized] private List<AnimatorControllerParameter> _triggers;

		// Event fields
		[NonSerialized] private bool _showEvents;

		// Tooltip text
		[NonSerialized] private readonly GUIContent _manageAnimationTooltip = EditorGUIUtility.TrTextContent("Manage Animation", "Allow this to manage animations for the key. Requires an Animator component to be attached.");
		[NonSerialized] private readonly GUIContent _openDelayTooltip = EditorGUIUtility.TrTextContent("Open Door Delay", "Delay between activating the door and actually unlocking it.");
		[NonSerialized] private readonly GUIContent _onUnlockedAnimTriggerTooltip = EditorGUIUtility.TrTextContent("Unlocked Animation Trigger", "Name of Animator trigger parameter when door is unlocked.");
		[NonSerialized] private readonly GUIContent _onLockedAnimTriggerTooltip = EditorGUIUtility.TrTextContent("Locked Animation Trigger", "Name of Animator trigger parameter when door is locked and KeyHolder tries to unlock it.");
		[NonSerialized] private readonly GUIContent _unlockMethodTooltip = EditorGUIUtility.TrTextContent("Unlock Method", "How will the door unlock when the key is obtained?");
		[NonSerialized] private readonly GUIContent _requiredKeysTooltip = EditorGUIUtility.TrTextContent("Required Keys", "Key(s) needed to open the door.");

		private void OnEnable() {
			_unlockMethod = serializedObject.FindProperty("unlockMethod");
			_requiredKeys = serializedObject.FindProperty("requiredKeys");
			_unlockDelay = serializedObject.FindProperty("unlockDelay");
			_onUnlockedAnimTrigger = serializedObject.FindProperty("onUnlockedAnimTrigger");
			_lockedAnimTrigger = serializedObject.FindProperty("lockedAnimTrigger");
			_uDoorUnlocked = serializedObject.FindProperty("uOnDoorUnlocked");
			_uDoorLocked = serializedObject.FindProperty("uDoorLocked");

			_attachedAnimator = ((KeyLock) serializedObject.targetObject).gameObject.GetComponent<Animator>();
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(_unlockMethod, _unlockMethodTooltip);
			EditorGUILayout.PropertyField(_requiredKeys, _requiredKeysTooltip);

			if (!_initializedAnimatorTriggers) UpdateAvailableAnimatorTriggers();

			EditorGUI.BeginDisabledGroup(_attachedAnimator == null);
			_manageAnimation = EditorGUILayout.Toggle(_manageAnimationTooltip, _manageAnimation);
			++EditorGUI.indentLevel;
			if (_manageAnimation) {
				EditorGUILayout.PropertyField(_unlockDelay, _openDelayTooltip);
				
				// Popup for being unlocked
				var selectedUnlockTrigger = EditorGUILayout.Popup(_onUnlockedAnimTriggerTooltip, _currentUnlockedAnimatorTrigger, _animatorTriggersRenderedList);
				if (selectedUnlockTrigger != _currentUnlockedAnimatorTrigger) {
					_onUnlockedAnimTrigger.intValue = selectedUnlockTrigger == 0 ? 0 : _triggers[selectedUnlockTrigger - 1].nameHash;
					_currentUnlockedAnimatorTrigger = selectedUnlockTrigger;
				}

				// Popup for being locked
				var selectedLockedTrigger = EditorGUILayout.Popup(_onLockedAnimTriggerTooltip, _currentLockedAnimatorTrigger, _animatorTriggersRenderedList);
				if (selectedLockedTrigger != _currentLockedAnimatorTrigger) {
					_lockedAnimTrigger.intValue = selectedLockedTrigger == 0 ? 0 : _triggers[selectedLockedTrigger - 1].nameHash;
					_currentLockedAnimatorTrigger = selectedLockedTrigger;
				}
			} else {
				_unlockDelay.floatValue = 0;
				_onUnlockedAnimTrigger.intValue = 0;
				_lockedAnimTrigger.intValue = 0;
			}
			--EditorGUI.indentLevel;

			_showEvents = EditorGUILayout.Foldout(_showEvents, "Unity Events");
			if (_showEvents) {
				EditorGUILayout.PropertyField(_uDoorLocked, new GUIContent("Door Locked"));
				EditorGUILayout.PropertyField(_uDoorUnlocked, new GUIContent("On Door Unlocked"));
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void UpdateAvailableAnimatorTriggers() {
			if (_attachedAnimator == null) {
				_animatorTriggersRenderedList = null;
				return;
			}

			_triggers = _attachedAnimator.parameters.Where(p => p.type == AnimatorControllerParameterType.Trigger).ToList();
			_animatorTriggersRenderedList = new GUIContent[_triggers.Count + 1];
			_animatorTriggersRenderedList[0] = new GUIContent("<None>");

			for (var i = 0; i < _triggers.Count; i++) {
				_animatorTriggersRenderedList[i + 1] = new GUIContent(_triggers[i].name);

				if (_triggers[i].nameHash == _onUnlockedAnimTrigger.intValue) {
					_currentUnlockedAnimatorTrigger = i + 1;
				}

				if (_triggers[i].nameHash == _lockedAnimTrigger.intValue) {
					_currentLockedAnimatorTrigger = i + 1;
				}
			}

			_manageAnimation = _onUnlockedAnimTrigger.intValue != 0 || _lockedAnimTrigger.intValue != 0 || _unlockDelay.floatValue != 0;
			_initializedAnimatorTriggers = true;
		}
	}
}
#endif