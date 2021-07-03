#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityUtilities.Systems.KeyDoor {
	[CustomEditor(typeof(Key))]
	public class KeyEditor : Editor {
		// Key properties
		[NonSerialized] private SerializedProperty _keyCollectionMethod;
		[NonSerialized] private SerializedProperty _consumeOnUse;
		[NonSerialized] private SerializedProperty _destroyDelay;
		[NonSerialized] private SerializedProperty _onCollectedAnimTrigger;

		// Animation rendered list
		[NonSerialized] private GUIContent[] _animatorTriggersRenderedList;

		// Keep track of Animation settings
		[NonSerialized] private int _currentAnimatorTrigger;
		[NonSerialized] private bool _manageAnimation;
		[NonSerialized] private bool _initializedAnimatorTriggers;
		[NonSerialized] private Animator _attachedAnimator;
		[NonSerialized] private List<AnimatorControllerParameter> _triggers;

		// Tooltip text
		[NonSerialized] private readonly GUIContent _manageAnimationTooltip = EditorGUIUtility.TrTextContent("Manage Animation", "Allow this to manage animations for the key. Requires an Animator component to be attached.");
		[NonSerialized] private GUIContent _keyCollectionMethodTooltip;
		[NonSerialized] private GUIContent _consumeOnUseTooltip;
		[NonSerialized] private GUIContent _destroyDelayTooltip;
		[NonSerialized] private GUIContent _onCollectedAnimTriggerTooltip;

		private void OnEnable() {
			_keyCollectionMethod = serializedObject.FindProperty("keyCollectionMethod");
			_consumeOnUse = serializedObject.FindProperty("consumeOnUse");
			_destroyDelay = serializedObject.FindProperty("destroyDelay");
			_onCollectedAnimTrigger = serializedObject.FindProperty("onCollectedAnimTrigger");

			_attachedAnimator = ((Key) serializedObject.targetObject).gameObject.GetComponent<Animator>();

			_keyCollectionMethodTooltip = EditorGUIUtility.TrTextContent(_keyCollectionMethod.displayName, "What triggers this Key to be obtained by a KeyHolder?");
			_consumeOnUseTooltip = EditorGUIUtility.TrTextContent(_consumeOnUse.displayName, "Should the key be removed from KeyHolder upon use?");
			_destroyDelayTooltip = EditorGUIUtility.TrTextContent(_destroyDelay.displayName, "Delay between obtaining the key and destroying the GameObject.");
			_onCollectedAnimTriggerTooltip = EditorGUIUtility.TrTextContent(_onCollectedAnimTrigger.displayName, "Name of Animator trigger parameter when key is collected.");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(_keyCollectionMethod, _keyCollectionMethodTooltip);
			EditorGUILayout.PropertyField(_consumeOnUse, _consumeOnUseTooltip);

			if (!_initializedAnimatorTriggers) UpdateAvailableAnimatorTriggers();

			EditorGUI.BeginDisabledGroup(_attachedAnimator == null);
			_manageAnimation = EditorGUILayout.Toggle(_manageAnimationTooltip, _manageAnimation);
			++EditorGUI.indentLevel;
			if (_manageAnimation) {
				EditorGUILayout.PropertyField(_destroyDelay, _destroyDelayTooltip);
				var selectedTrigger = EditorGUILayout.Popup(_onCollectedAnimTriggerTooltip, _currentAnimatorTrigger, _animatorTriggersRenderedList);
				if (selectedTrigger != _currentAnimatorTrigger) {
					_onCollectedAnimTrigger.intValue = selectedTrigger == 0 ? 0 : _triggers[selectedTrigger - 1].nameHash;
					_currentAnimatorTrigger = selectedTrigger;
				}
			} else {
				_destroyDelay.floatValue = 0;
				_onCollectedAnimTrigger.intValue = 0;
			}

			--EditorGUI.indentLevel;
			EditorGUI.EndDisabledGroup();
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

				if (_triggers[i].nameHash == _onCollectedAnimTrigger.intValue) {
					_currentAnimatorTrigger = i + 1;
				}
			}

			_manageAnimation = _onCollectedAnimTrigger.intValue != 0 || _destroyDelay.floatValue != 0;
			_initializedAnimatorTriggers = true;
		}
	}
}
#endif