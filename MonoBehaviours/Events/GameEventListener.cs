using UnityEngine;
using UnityEngine.Events;
using UnityUtilities.MonoBehaviours.Events;

namespace UnityUtilities.MonoBehaviours.Events {
	public class GameEventListener : MonoBehaviour {

		[System.Serializable] private class GameEventHandler : UnityEvent<GameEvent.GameEventArgs> { };
		
		[SerializeField] private GameEvent[] gameEvents;
		[SerializeField] private GameEventHandler _gameEventHandler;
		[SerializeField] private UnityEvent _unityEvent;

		private void Awake() {
			foreach (var ge in gameEvents) {
				ge.Subscribe(this);
			}
		}

		private void OnDestroy() {
			foreach (var ge in gameEvents) {
				ge.Unsubscribe(this);
			}
		}


		public void TriggerEvent() {
			_unityEvent.Invoke();
		}
		
		public void TriggerEvent(GameEvent.GameEventArgs args) {
			_gameEventHandler.Invoke(args);
		}
	}
}