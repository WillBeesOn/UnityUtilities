using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtilities.Systems {
	public enum KeyCollectionMethod {
		OnCollision,
		Custom
	}

	public class KeyHolder : MonoBehaviour {
		[SerializeField] private KeyCollectionMethod keyCollectionMethod;
		private readonly HashSet<Key> _keySet = new HashSet<Key>();

		public void AddKey(Key k) {
			_keySet.Add(k);
			k.KeyCollected(this);
		}

		public void RemoveKey(Key k) {
			_keySet.Remove(k);
		}

		// Check if the KeyHolder has the necessary keys to unlock something.
		public bool CanUnlock(IEnumerable<Key> requiredKeys) {
			return !requiredKeys.Except(_keySet).Any();
		}

		private void OnTriggerEnter(Collider other) {
			var key = other.GetComponent<Key>();
			if (key == null) return;
			if (keyCollectionMethod == KeyCollectionMethod.OnCollision) AddKey(key);
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var key = other.GetComponent<Key>();
			if (key == null) return;
			if (keyCollectionMethod == KeyCollectionMethod.OnCollision) AddKey(key);
		}
	}
}