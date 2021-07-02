using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.Systems.KeyDoor {
	public enum UnlockMethod {
		OnHolderCollision,
		OnKeyCollect,
		Custom
	};

	// A locked door or object that can be unlocked if a KeyHolder is holding the required Key(s).
	public class KeyLock : MonoBehaviour {
		//Events to run when the KeyHolder tries to open the door and key requirement is not met.
		public event Action DoorLocked;

		// Events to run when the door is unlocked.
		public event Action OnDoorUnlocked;

		public bool isUnlocked { get; private set; }

		[Tooltip("How will the door unlock when the key is obtained?")]
		public UnlockMethod unlockMethod;

		[Tooltip("Key(s) needed to open the door.")]
		[SerializeField] private List<Key> requiredKeys;

		[Header("Animation")]
		[Tooltip("Name of Animator trigger parameter when the door is unlocked.")]
		[SerializeField] private string onUnlockedAnimTrigger;

		[Tooltip("Name of Animator trigger parameter when KeyHolder tries to open the door and key requirement is not met.")]
		[SerializeField] private string lockedAnimTrigger;

		[Tooltip("Delay between activating the unlock and actually unlocking the door.")]
		[SerializeField] private float unlockDelay;

		private Animator _animator;
		private int _onUnlockedId;
		private int _lockedId;

		public void TryToUnlock(KeyHolder holder) {
			if (unlockMethod == UnlockMethod.OnHolderCollision && holder.CanUnlock(requiredKeys)) {
				foreach (var key in requiredKeys) {
					if (key.consumeOnUse) holder.RemoveKey(key);
				}

				StartCoroutine(Unlock());
			} else PlayLockedAnimation();
		}

		private void Awake() {
			_animator = GetComponent<Animator>();
			_onUnlockedId = Animator.StringToHash(onUnlockedAnimTrigger);
			_lockedId = Animator.StringToHash(lockedAnimTrigger);
			if (unlockMethod == UnlockMethod.OnKeyCollect) {
				foreach (var k in requiredKeys) {
					k.OnKeyCollect += Unlock;
				}
			}
		}

		private void Unlock(Key k) {
			StartCoroutine(Unlock());
		}

		private IEnumerator Unlock() {
			yield return new WaitForSeconds(unlockDelay);
			isUnlocked = true;
			PlayUnlockedAnimation();
		}

		private void OnTriggerEnter(Collider other) {
			var holder = other.GetComponent<KeyHolder>();
			if (holder == null) return;
			TryToUnlock(holder);
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var holder = other.GetComponent<KeyHolder>();
			if (holder == null) return;
			TryToUnlock(holder);
		}

		private void PlayLockedAnimation() {
			if (_animator == null || lockedAnimTrigger == null) return;
			DoorLocked?.Invoke();
			_animator.SetTrigger(_lockedId);
		}

		private void PlayUnlockedAnimation() {
			if (_animator == null || onUnlockedAnimTrigger == null) return;
			OnDoorUnlocked?.Invoke();
			_animator.SetTrigger(_onUnlockedId);
		}
	}
}