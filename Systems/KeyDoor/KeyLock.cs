using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtilities.Systems.KeyDoor {
	public enum UnlockMethod {
		OnHolderCollision,
		OnKeyCollect,
		Custom
	};

	// A locked door or object that can be unlocked if a KeyHolder is holding the required Key(s).
	public class KeyLock : MonoBehaviour {
		//Event to run when the KeyHolder tries to open the door and key requirement is not met.
		public event Action DoorLocked;
		public event Action OnDoorUnlocked;

		public bool isUnlocked { get; private set; }

		public UnlockMethod unlockMethod;

		[SerializeField] private UnityEvent uDoorLocked;
		[SerializeField] private UnityEvent uOnDoorUnlocked;
		[SerializeField] private List<Key> requiredKeys;
		[SerializeField] private int onUnlockedAnimTrigger;
		[SerializeField] private int lockedAnimTrigger;
		[SerializeField] private float unlockDelay;

		private Animator _animator;

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
			if (_animator == null || lockedAnimTrigger == 0) return;
			DoorLocked?.Invoke();
			_animator.SetTrigger(lockedAnimTrigger);
		}

		private void PlayUnlockedAnimation() {
			if (_animator == null || onUnlockedAnimTrigger == 0) return;
			OnDoorUnlocked?.Invoke();
			_animator.SetTrigger(onUnlockedAnimTrigger);
		}
	}
}