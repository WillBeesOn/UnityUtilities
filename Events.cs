using System;
using System.Collections.Generic;

namespace UnityUtilities {

    /// <summary>
    /// Contains the event handler that will dispatch an event. Also records
    /// the number of subscribers, adds, and removes them.
    /// </summary>
    internal class EventData {
        private int numOfSubscribers;
        private event EventHandler Handler;

        internal void AddSubscriber(EventHandler callbackFn) {
            numOfSubscribers++;
            Handler += callbackFn;
        }

        internal void RemoveSubscriber(EventHandler callbackFn) {
            numOfSubscribers--;
            Handler -= callbackFn;
        }

        internal bool Dispatch(object sender, EventArgs args) {
            EventHandler h = Handler;
            if (h != null) {
                h(sender, args);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// A store for all events unique events that have been published.
    /// </summary>
    internal struct EventStore {
        internal static Dictionary<string, EventData> allEvents = new Dictionary<string, EventData>();
    }

    /// <summary>
    /// Manage publishing of events.
    /// </summary>
    public class EventPublisher {
        private List<string> _myEvents = new List<string>();

        /// <summary>
        /// Creates an event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool Publish(string eventName) {
            if (!EventStore.allEvents.ContainsKey(eventName)) {
                EventStore.allEvents.Add(eventName, new EventData());
                _myEvents.Add(eventName);
                return true;
            } else if (!_myEvents.Contains(eventName) && EventStore.allEvents.ContainsKey(eventName)) {
                _myEvents.Add(eventName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prevent dispatching of given event until it is published again
        /// in this instance.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool Unpublish(string eventName) {
            if (_myEvents.Contains(eventName)) {
                _myEvents.Remove(eventName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Dispatches the event to any subscribers. EventPublisher may only
        /// dispatch events that it has published itself.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Dispatch(string eventName, EventArgs args) {
            if (_myEvents.Contains(eventName)) {
                return EventStore.allEvents[eventName].Dispatch(this, args);
            }
            return false;
        }
    }

    /// <summary>
    /// Manage subscriptions to published events.
    /// </summary>
    public class EventSubscriber {
        private Dictionary<string, EventHandler> _mySubscriptions = new Dictionary<string, EventHandler>();

        /// <summary>
        /// Subscribes the given function to the event. The function will be
        /// called whenever the event is dispatched. A single event may only
        /// be subscribed to by one function.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callbackFn"></param>
        /// <returns></returns>
        public bool Subscribe(string eventName, EventHandler callbackFn) {
            if (EventStore.allEvents.ContainsKey(eventName) && !_mySubscriptions.ContainsKey(eventName)) {
                EventData data = EventStore.allEvents[eventName];
                data.AddSubscriber(callbackFn);
                EventStore.allEvents[eventName] = data;
                _mySubscriptions.Add(eventName, callbackFn);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stops the function bound to the given event from being called
        /// when the event is dispatched.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool Unsubscribe(string eventName) {
            if (_mySubscriptions.ContainsKey(eventName) && EventStore.allEvents.ContainsKey(eventName)) {
                EventData data = EventStore.allEvents[eventName];
                data.AddSubscriber(_mySubscriptions[eventName]);
                EventStore.allEvents[eventName] = data;
                _mySubscriptions.Remove(eventName);
                return true;
            }
            return false;
        }
    }
}