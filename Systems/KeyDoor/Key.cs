using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtilities.Systems.KeyDoor {
	public enum KeyCollectionMethod {
		OnCollision,
		Custom
	}
	
	// A Key used to open a KeyLock when obtained by a KeyHolder.
	public class Key : MonoBehaviour {
		// Events to run when this Key gets collected by a KeyHolder.
		public event Action<Key> OnKeyCollect;
		public KeyCollectionMethod keyCollectionMethod;
		public bool consumeOnUse;

		[SerializeField] private UnityEvent<Key> onKeyCollected;
		[SerializeField] private float destroyDelay;
		[SerializeField] private int onCollectedAnimTrigger;

		private Animator _animator;
		private Collider _collider;
		private Collider2D _collider2D;

		public void KeyCollected(KeyHolder holder) {
			OnKeyCollect?.Invoke(this);
			onKeyCollected?.Invoke(this);
			if (_collider != null) _collider.enabled = false;
			if (_collider2D != null) _collider.enabled = false;
			PlayGetKeyAnimation();

			StartCoroutine(DelayedDestroy(holder));
		}

		private void Awake() {
			_collider = GetComponent<Collider>();
			_collider2D = GetComponent<Collider2D>();
			_animator = GetComponent<Animator>();
		}

		private void PlayGetKeyAnimation() {
			if (_animator == null || onCollectedAnimTrigger == 0) return;
			_animator.SetTrigger(onCollectedAnimTrigger);
		}

		private IEnumerator DelayedDestroy(KeyHolder holder) {
			yield return new WaitForSeconds(destroyDelay);
			Destroy(gameObject);
		}
	}
}