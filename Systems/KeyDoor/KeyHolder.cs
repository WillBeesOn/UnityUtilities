using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtilities.Systems.KeyDoor {

	public class KeyHolder : MonoBehaviour {
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
			if (key == null || key.keyCollectionMethod == KeyCollectionMethod.OnCollision) return;
			AddKey(key);
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var key = other.GetComponent<Key>();
			if (key == null || key.keyCollectionMethod == KeyCollectionMethod.OnCollision) return;
			AddKey(key);
		}
	}
}