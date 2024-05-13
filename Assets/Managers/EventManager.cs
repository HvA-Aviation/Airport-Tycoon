using System;
using System.Collections.Generic;
using System.Linq;
using EventManager;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        #region Variables

        private readonly Dictionary<EventId, List<Action<EventArgs>>> _eventDictionary =
            new Dictionary<EventId, List<Action<EventArgs>>>();

        private readonly Dictionary<EventId, List<Action<EventArgs>>> _flashEventDictionary =
            new Dictionary<EventId, List<Action<EventArgs>>>();

        #endregion

        public void CreateEvent(EventId eventId)
        {
            // If the event doesn't exist, create it
            if (!_eventDictionary.ContainsKey(eventId))
                _eventDictionary[eventId] = new List<Action<EventArgs>>();
        }

        public void Subscribe(EventId eventId, Action<EventArgs> eventHandler)
        {
            // If the event doesn't exist, create it
            if (!_eventDictionary.ContainsKey(eventId))
                _eventDictionary[eventId] = new List<Action<EventArgs>>();
            // Add the event handler to the event
            _eventDictionary[eventId].Add(eventHandler);
        }

        public void SubscribeFlash(EventId eventId, Action<EventArgs> eventHandler)
        {
            // If the event doesn't exist, create it
            if (!_flashEventDictionary.ContainsKey(eventId))
                _flashEventDictionary[eventId] = new List<Action<EventArgs>>();
            // Add the event handler to the event
            _flashEventDictionary[eventId].Add(eventHandler);
        }

        public void Unsubscribe(EventId eventId, Action<EventArgs> eventHandler)
        {
            // If the event doesn't exist, return
            if (!_eventDictionary.ContainsKey(eventId))
                return;
            // Remove the event handler from the event
            _eventDictionary[eventId].Remove(eventHandler);
        }

        private void Update()
        {
            //Debug.Log();
        }

        public void TriggerEvent(EventId eventId, EventArgs eventArgs = null)
        {
            if (eventArgs == null)
            {
                eventArgs = EventArgs.Empty;
            }

            if (_eventDictionary.TryGetValue(eventId, out var value))
            {
                foreach (var handler in value.ToList()) handler.Invoke(eventArgs);
            }

            if (_flashEventDictionary.TryGetValue(eventId, out var flashValue))
            {
                foreach (var handler in flashValue.ToList())
                {
                    handler.Invoke(eventArgs);
                    flashValue.Remove(handler);
                }
            }
        }
    }
}