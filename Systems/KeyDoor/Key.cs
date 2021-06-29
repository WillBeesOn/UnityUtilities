using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtilities.Systems {
	public class Key : MonoBehaviour {
		[Tooltip("Events to run when key is collected.")]
		public UnityEvent onKeyCollect;

		[Tooltip("Should the key be removed upon use?")]
		[SerializeField] private bool consumeOnUse;

		[Tooltip("Delay between obtaining the key and destroying the GameObject.")]
		[SerializeField] private float destroyDelay;

		[Tooltip("Name of Animator trigger parameter when key is collected.")]
		[SerializeField] private string onCollectedAnimTrigger;

		private Animator _animator;
		private Collider _collider;
		private Collider2D _collider2D;
		private int _onCollectedId;

		public void KeyCollected(KeyHolder holder) {
			onKeyCollect.Invoke();
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
			if (_collider != null) _collider.enabled = false;
			if (_collider2D != null) _collider.enabled = false;
			PlayGetKeyAnimation();

			yield return new WaitForSeconds(destroyDelay);
			gameObject.SetActive(false);
			if (consumeOnUse) holder.RemoveKey(this);
			Destroy(gameObject);
		}
	}
}