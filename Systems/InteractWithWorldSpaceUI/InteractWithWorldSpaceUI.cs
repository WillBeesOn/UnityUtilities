using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityUtilities.Systems.InteractWithWorldSpaceUI {
	// Attach to a GameObject with a world space Canvas and a trigger Collider.
	// Toggles a world space UI GameObject when another GameObject enters this object's trigger Collider.
	// When UI is visible, allows user input to "interact" with the object and invokes an event that sends GameObject.
	public class InteractWithWorldSpaceUI : MonoBehaviour {
		// Events to run when something is interacted with
		public static event Action<GameObject> OnInteract;

		[Tooltip("Camera for UI popup to look at")]
		[SerializeField] private Camera mainCamera;

		[Header("Input Parameters")]
		[Tooltip("Unity Input System asset you are using")]
		[SerializeField] private InputActionAsset controls;

		[Tooltip("Name of Action Map that contains the target Action that should activate an \"interaction\"")]
		[SerializeField] private string targetActionMap;

		[Tooltip("Name of the target Action that should activate an \"interaction\"")]
		[SerializeField] private string targetAction;

		[Header("GameObjects")]
		[Tooltip("The UI GameObject to toggle when triggered")]
		[SerializeField] private GameObject uiPopUp;

		[Tooltip("Trigger toggling the UI popup when this GameObject enters the attached trigger Collider")]
		[SerializeField] private GameObject uiPopUpTrigger;

		[Tooltip("GameObject to send to subscribers of OnInteract event")]
		[SerializeField] private GameObject objectToSendOnEvent;

		private bool _isVisible;
		private InputActionMap _actionMap;
		private InputAction _action;

		private void Awake() {
			// TODO safe to forgo specifying the control scheme? If I just set the callback to an Action inside an Action map, will it apply to all schemes?
			_actionMap = controls.FindActionMap(targetActionMap);
			_action = _actionMap.FindAction(targetAction);
		}

		private void OnInteractEvent(InputAction.CallbackContext c) {
			OnInteract?.Invoke(objectToSendOnEvent);
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
			ToggleUIPopUp(other.gameObject, true);
		}

		private void OnTriggerExit(Collider other) {
			ToggleUIPopUp(other.gameObject, false);
		}

		private void OnTriggerEnter2D(Collider2D other) {
			ToggleUIPopUp(other.gameObject, true);
		}

		private void OnTriggerExit2D(Collider2D other) {
			ToggleUIPopUp(other.gameObject, false);
		}
	}
}