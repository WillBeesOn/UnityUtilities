using System;
using System.Collections.Generic;

namespace UnityUtilities {
    /// <summary>
    /// Mange dispatching and subscribing to events.
    /// </summary>
    public static class Events {

        /// <summary>
        /// A collection of all events that have been subscribed to or have been
        /// dispatched.
        /// </summary>
        private static readonly Dictionary<string, EventData> allEvents = new Dictionary<string, EventData>();

        /// <summary>
        /// Delegate to be called when event is triggered.
        /// </summary>
        /// <param name="args"></param>
        public delegate void EventCallback(EventArgs args);

        /// <summary>
        /// Contains the event handler that will dispatch an event. Also records
        /// the number of subscribers, adds, and removes them.
        /// </summary>
        private class EventData {
            public int numOfSubscribers;
            public event EventCallback Handler;

            public void AddSubscriber(EventCallback callbackFn) {
                numOfSubscribers++;
                Handler += callbackFn;
            }

            public void RemoveSubscriber(EventCallback callbackFn) {
                numOfSubscribers--;
                Handler -= callbackFn;
            }

            public bool Dispatch(EventArgs args) {
                EventCallback h = Handler;
                if (h != null) {
                    h(args);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Remove event from list of published events.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static bool Unpublish(string eventName) {
            if (allEvents.ContainsKey(eventName)) {
                allEvents.Remove(eventName);
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
        public static bool Dispatch(string eventName, EventArgs args) {
            if (allEvents.ContainsKey(eventName) || Publish(eventName)) {
                return allEvents[eventName].Dispatch(args);
            }
            return false;
        }

        /// <summary>
        /// Subscribes the given function to the event. The function will be
        /// called whenever the event is dispatched. A single event may only
        /// be subscribed to by one function.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callbackFn"></param>
        /// <returns></returns>
        public static bool Subscribe(string eventName, EventCallback callbackFn) {
            if (allEvents.ContainsKey(eventName) || Publish(eventName)) {
                allEvents[eventName].AddSubscriber(callbackFn);
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
        public static bool Unsubscribe(string eventName, EventCallback callbackFn) {
            if (allEvents.ContainsKey(eventName)) {
                allEvents[eventName].RemoveSubscriber(callbackFn);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates an event if it doesn't exist. If it does exist, doesn't do
        /// anything.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private static bool Publish(string eventName) {
            if (!allEvents.ContainsKey(eventName)) {
                allEvents.Add(eventName, new EventData());
                return true;
            }
            return false;
        }
    }
}