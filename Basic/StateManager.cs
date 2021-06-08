using System.Collections.Generic;

namespace UnityUtilities {
	public abstract class State {
		/// <summary>
		/// Handles entering a new state.
		/// </summary>
		public virtual void OnStateEnter() { }

		/// <summary>
		/// Handles exiting the current state.
		/// </summary>
		public virtual void OnStateExit() { }

		/// <summary>
		/// Handle behavior mean to run every frame.
		/// </summary>
		public virtual void Tick() { }
	}

	/// <summary>
	/// Manage entering and exiting new states
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class StateManager<T> where T : State {
		public T currentState => _stateStack.Count > 0 ? _stateStack[_stateStack.Count - 1] : default;
		public T previousState => _stateStack.Count > 1 ? _stateStack[_stateStack.Count - 2] : default;

		private readonly List<T> _stateStack = new List<T>();
		private readonly uint _maxHistory;

		public StateManager(T state, uint maxHistory = uint.MaxValue) {
			_stateStack.Add(state);
			_maxHistory = maxHistory;
		}

		public void UpdateState(T state) {
			_stateStack[_stateStack.Count - 1].OnStateExit();
			_stateStack.Add(state);
			_stateStack[_stateStack.Count - 1].OnStateEnter();

			if (_stateStack.Count > _maxHistory) {
				_stateStack.RemoveAt(0);
			}
		}

		public void RollBackState() {
			_stateStack[_stateStack.Count - 1].OnStateExit();
			_stateStack.RemoveAt(_stateStack.Count - 1);
			_stateStack[_stateStack.Count - 1].OnStateEnter();
		}

		public void Initialize() {
			_stateStack[_stateStack.Count - 1].OnStateEnter();
		}
	}
}