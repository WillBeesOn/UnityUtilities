using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.MonoBehaviours.Events {
	[CreateAssetMenu(menuName = "Game Event", fileName = "NewGameEvent")]
	public class GameEvent : ScriptableObject {
		public class GameEventArgs { }

		private readonly HashSet<GameEventListener> _listeners = new HashSet<GameEventListener>();

		public void Dispatch() {
			foreach (var listener in _listeners) {
				listener.TriggerEvent();
			}
		}

		public void Dispatch(GameEventArgs args) {
			foreach (var listener in _listeners) {
				listener.TriggerEvent(args);
			}
		}

		public void Subscribe(GameEventListener l) => _listeners.Add(l);
		public void Unsubscribe(GameEventListener l) => _listeners.Remove(l);
	}
}