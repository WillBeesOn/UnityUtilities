using System;
using System.Collections.Generic;

namespace UnityUtilities {
	/// <summary>
	/// Mange dispatching and subscribing to events using callbacks without arguments.
	/// </summary>
	public static class Events {
		private static readonly Dictionary<string, List<Action>> _callbacks = new Dictionary<string, List<Action>>();

		public static void Dispatch(string eventName) {
			if (!_callbacks.TryGetValue(eventName, out var list)) {
				return;
			}

			foreach (var func in list) {
				func();
			}
		}

		public static void Subscribe(string eventName, Action callbackFn) {
			var eventPublished = _callbacks.TryGetValue(eventName, out var list);
			if (eventPublished && list.Contains(callbackFn)) {
				return;
			}

			if (!eventPublished) {
				_callbacks.Add(eventName, new List<Action> {callbackFn});
			} else if (!list.Contains(callbackFn)) {
				list.Add(callbackFn);
			}
		}

		public static void Unsubscribe(string eventName, Action callbackFn) {
			if (!_callbacks.TryGetValue(eventName, out var list) || !list.Contains(callbackFn)) {
				return;
			}
			list.Remove(callbackFn);
		}

		public static void Clear(string eventName) {
			if (!_callbacks.ContainsKey(eventName)) {
				return;
			}
			_callbacks.Remove(eventName);
		}
	}

	/// <summary>
	/// Mange dispatching and subscribing to events using callbacks with arguments.
	/// </summary>
	public static class Events<T> {
		private static readonly List<Action<T>> _callbacks = new List<Action<T>>();

		public static void Dispatch(T args) {
			foreach (var func in _callbacks) {
				func(args);
			}
		}

		public static void Subscribe(Action<T> callbackFn) {
			if (_callbacks.Contains(callbackFn)) {
				return;
			}

			_callbacks.Add(callbackFn);
		}

		public static void Unsubscribe(Action<T> callbackFn) {
			_callbacks.Remove(callbackFn);
		}

		public static void Clear() {
			_callbacks.Clear();
		}
	}
}