using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtilities.Systems.KeyDoor {
	public enum UnlockMethod {
		OnHolderCollision,
		OnKeyCollect,
		Custom
	};

	public class KeyLock : MonoBehaviour {
		[Tooltip("How will the door unlock when the key is obtained?")]
		public UnlockMethod unlockMethod;

		[Tooltip("Key(s) needed to open the door.")]
		[SerializeField] private List<Key> requiredKeys;

		[Header("Events")]
		[Tooltip("Events to run when the door is unlocked.")]
		[SerializeField] private UnityEvent onDoorUnlocked;

		[Tooltip("Events to run when the KeyHolder tries to open the door and key requirement is not met.")]
		[SerializeField] private UnityEvent doorLocked;

		[Header("Animation")]
		[Tooltip("Name of Animator trigger parameter when the door is unlocked.")]
		[SerializeField] private string onUnlockedAnimTrigger;

		[Tooltip("Name of Animator trigger parameter when KeyHolder tries to open the door and key requirement is not met.")]
		[SerializeField] private string lockedAnimTrigger;

		private Animator _animator;
		private int _onUnlockedId;
		private int _lockedId;

		public void Unlock() {
			PlayUnlockedAnimation();
		}

		private void Awake() {
			_animator = GetComponent<Animator>();
			_onUnlockedId = Animator.StringToHash(onUnlockedAnimTrigger);
			_lockedId = Animator.StringToHash(lockedAnimTrigger);
			if (unlockMethod == UnlockMethod.OnKeyCollect) {
				foreach (var k in requiredKeys) {
					k.onKeyCollect.AddListener(Unlock);
				}
			}
		}

		private void OnTriggerEnter(Collider other) {
			var holder = other.GetComponent<KeyHolder>();
			if (unlockMethod == UnlockMethod.OnHolderCollision && holder != null && holder.CanUnlock(requiredKeys)) Unlock();
			else PlayLockedAnimation();
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var holder = other.GetComponent<KeyHolder>();
			if (unlockMethod == UnlockMethod.OnHolderCollision && holder != null && holder.CanUnlock(requiredKeys)) Unlock();
			else PlayLockedAnimation();
		}

		private void PlayLockedAnimation() {
			if (_animator == null || lockedAnimTrigger == null) return;
			doorLocked.Invoke();
			_animator.SetTrigger(_lockedId);
		}

		private void PlayUnlockedAnimation() {
			if (_animator == null || onUnlockedAnimTrigger == null) return;
			onDoorUnlocked.Invoke();
			_animator.SetTrigger(_onUnlockedId);
		}
	}
}