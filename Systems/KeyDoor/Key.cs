using System;
using System.Collections;
using UnityEngine;

namespace UnityUtilities.Systems.KeyDoor {
	public enum KeyCollectionMethod {
		OnCollision,
		Custom
	}

	public class Key : MonoBehaviour {
		// Events to run when this Key gets collected by a KeyHolder.
		public event Action<Key> OnKeyCollect;

		[Tooltip("What triggers this Key to be obtained by a KeyHolder?")]
		public KeyCollectionMethod keyCollectionMethod;

		[Tooltip("Should the key be removed from KeyHolder upon use?")]
		public bool consumeOnUse;

		[Tooltip("Delay between obtaining the key and destroying the GameObject.")]
		[SerializeField] private float destroyDelay;

		[Tooltip("Name of Animator trigger parameter when key is collected.")]
		[SerializeField] private string onCollectedAnimTrigger;

		private Animator _animator;
		private Collider _collider;
		private Collider2D _collider2D;
		private int _onCollectedId;

		public void KeyCollected(KeyHolder holder) {
			OnKeyCollect?.Invoke(this);
			if (_collider != null) _collider.enabled = false;
			if (_collider2D != null) _collider.enabled = false;
			PlayGetKeyAnimation();

			StartCoroutine(DelayedDestroy(holder));
		}

		private void Awake() {
			_collider = GetComponent<Collider>();
			_collider2D = GetComponent<Collider2D>();
			_animator = GetComponent<Animator>();
			_onCollectedId = Animator.StringToHash(onCollectedAnimTrigger);
		}

		private void PlayGetKeyAnimation() {
			if (_animator == null || onCollectedAnimTrigger == null) return;
			_animator.SetTrigger(_onCollectedId);
		}

		private IEnumerator DelayedDestroy(KeyHolder holder) {
			yield return new WaitForSeconds(destroyDelay);
			Destroy(gameObject);
		}
	}
}