using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UnityUtilities.Systems.InteractWithWorldSpaceUI {
	
	// Attach to a GameObject with a world space Canvas and a trigger Collider.
	// Toggles a world space UI GameObject when another GameObject enters this object's trigger Collider.
	// When UI is visible, allows user input to "interact" with the object and invokes an event that sends GameObject.
	public class InteractWithWorldSpaceUI : MonoBehaviour {
		// Events to run when something is interacted with
		public event Action<GameObject> OnEnter;
		public event Action<GameObject> OnInteract;

		[SerializeField] private UnityEvent<GameObject> uOnEnter;
		[SerializeField] private UnityEvent<GameObject> uOnInteract;
		[SerializeField] private Camera mainCamera;
		[SerializeField] private InputActionAsset controls;
		[SerializeField] private string targetActionMap;
		[SerializeField] private string targetAction;
		[SerializeField] private GameObject uiPopUp;
		[SerializeField] private GameObject uiPopUpTrigger;
		[SerializeField] private GameObject objectToSendOnEnterEvent;
		[SerializeField] private GameObject objectToSendOnInteractEvent;

		private bool _isVisible;
		private InputActionMap _actionMap;
		private InputAction _action;

		private void Awake() {
			_actionMap = controls.FindActionMap(targetActionMap);
			_action = _actionMap.FindAction(targetAction);
		}

		private void OnInteractEvent(InputAction.CallbackContext c) {
			OnInteract?.Invoke(objectToSendOnInteractEvent);
			uOnInteract?.Invoke(objectToSendOnInteractEvent);
		}

		// Orient the world space Canvas after all movement is completed this frame to avoid jittering
		private void LateUpdate() {
			if (_isVisible) {
				var cameraRotation = mainCamera.transform.rotation;
				transform.LookAt(transform.position + cameraRotation * Vector3.forward,
				                 cameraRotation * Vector3.up);
			}
		}

		private void ToggleUIPopUp(GameObject other, bool show) {
			if (uiPopUpTrigger != other) return;
			if (_action != null) {
				if (show) _action.performed += OnInteractEvent;
				else _action.performed -= OnInteractEvent;
			}

			uiPopUp.SetActive(show);
			_isVisible = show;
		}

		private void OnTriggerEnter(Collider other) {
			OnEnter?.Invoke(objectToSendOnEnterEvent);
			uOnEnter?.Invoke(objectToSendOnEnterEvent);
			ToggleUIPopUp(other.gameObject, true);
		}

		private void OnTriggerExit(Collider other) {
			ToggleUIPopUp(other.gameObject, false);
		}

		private void OnTriggerEnter2D(Collider2D other) {
			OnEnter?.Invoke(objectToSendOnEnterEvent);
			uOnEnter?.Invoke(objectToSendOnEnterEvent);
			ToggleUIPopUp(other.gameObject, true);
		}

		private void OnTriggerExit2D(Collider2D other) {
			ToggleUIPopUp(other.gameObject, false);
		}
	}
}