using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gemukodo.Events
{
    public class EventManager
    {
        protected static EventManager instance;
        protected Dictionary<int, UnityEvent> eventDictionary;

        static EventManager()
        {
            instance = new EventManager();
        }

        protected EventManager()
        {
            eventDictionary = new Dictionary<int, UnityEvent>(50);
        }

        public static void StartListening<TEventKey>(TEventKey eventKey, UnityAction listener)
        {
            UnityEvent e = null;
            if (instance.eventDictionary.TryGetValue(eventKey.GetHashCode(), out e))
            {
                e.AddListener(listener);
            }
            else
            {
                e = new UnityEvent();
                e.AddListener(listener);
                instance.eventDictionary[eventKey.GetHashCode()] = e;
            }
        }

        public static void StopListening<TEventKey>(TEventKey eventKey, UnityAction listener)
        {
            UnityEvent e = null;
            if (instance.eventDictionary.TryGetValue(eventKey.GetHashCode(), out e))
            {
                e.RemoveListener(listener);
            }
        }

        public static void TriggerEvent<TEventKey>(TEventKey eventKey)
        {
            UnityEvent e = null;
            if (instance.eventDictionary.TryGetValue(eventKey.GetHashCode(), out e))
            {
                if (e != null)
                {
                    e.Invoke();
                }
            }
        }
    }

    namespace V2
    {
        public class EventManager
        {
            private static Dictionary<int, List<Action>> emptyEvents;
            private static Dictionary<int, List<Action<object>>> dataEvents;

            static EventManager()
            {
                emptyEvents = new Dictionary<int, List<Action>>();
                dataEvents = new Dictionary<int, List<Action<object>>>();
            }

            // ADD LISTENER

            static void Add<TEventKey>(TEventKey eventKey, Action eventHandler)
            {
                List<Action> eventHandlerList;
                if (emptyEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Add(eventHandler);
                }
                else
                {
                    emptyEvents.Add(eventKey.GetHashCode(), new List<Action>() { eventHandler });
                }
            }

            static void Add<TEventKey, TParam>(TEventKey eventKey, Action<TParam> eventHandler)
            {
                List<Action<object>> eventHandlerList;
                if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Add(eventHandler as Action<object>);
                }
                else
                {
                    dataEvents.Add(eventKey.GetHashCode(), new List<Action<object>>() { eventHandler as Action<object> });
                }
            }

            public static void AddListener(string eventKey, Action eventHandler)
            {
                Add(eventKey, eventHandler);
            }

            public static void AddListener(int eventKey, Action eventHandler)
            {
                Add(eventKey, eventHandler);
            }

            public static void AddListener(string eventKey, Action<object> eventHandler)
            {
                Add(eventKey, eventHandler);
            }

            public static void AddListener(int eventKey, Action<object> eventHandler)
            {
                Add(eventKey, eventHandler);
            }

            // REMOVE LISTENER

            static void Remove<TEventKey>(TEventKey eventKey, Action eventHandler)
            {
                List<Action> eventHandlerList;
                if (emptyEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Remove(eventHandler);
                }
            }

            static void Remove<TEventKey, TParam>(TEventKey eventKey, Action<TParam> eventHandler)
            {
                List<Action<object>> eventHandlerList;
                if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Remove(eventHandler as Action<object>);
                }
            }

            public static void RemoveListener(string eventKey, Action eventHandler)
            {
                Remove(eventKey, eventHandler);
            }

            public static void RemoveListener(int eventKey, Action eventHandler)
            {
                Remove(eventKey, eventHandler);
            }

            public static void RemoveListener(string eventKey, Action<object> eventHandler)
            {
                Remove(eventKey, eventHandler);
            }

            public static void RemoveListener(int eventKey, Action<object> eventHandler)
            {
                Remove(eventKey, eventHandler);
            }

            public static void Invoke<TEventKey>(TEventKey eventKey)
            {
                List<Action> eventHandlerList;
                if (emptyEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    int count = eventHandlerList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        eventHandlerList[i]?.Invoke();
                    }
                }
            }

            public static void Invoke<TEventKey, TData>(TEventKey eventKey, TData data)
            {
                List<Action<object>> eventHandlerList;
                if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    int count = eventHandlerList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        eventHandlerList[i]?.Invoke(data);
                    }
                }
            }
        }

        public class EventManager<TEventArgs>
        {
            static Dictionary<int, List<Action<TEventArgs>>> dataEvents;

            static EventManager()
            {
                dataEvents = new Dictionary<int, List<Action<TEventArgs>>>();
            }

            public static void AddListener<TEventKey>(TEventKey eventKey, Action<TEventArgs> eventHandler)
            {
                List<Action<TEventArgs>> eventHandlerList;
                if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Add(eventHandler);
                }
                else
                {
                    dataEvents.Add(eventKey.GetHashCode(), new List<Action<TEventArgs>>() { eventHandler as Action<TEventArgs> });
                }
            }

            public static void RemoveListener<TEventKey>(TEventKey eventKey, Action<TEventArgs> eventHandler)
            {
                List<Action<TEventArgs>> eventHandlerList;
                if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Remove(eventHandler);
                }
            }

            public static void Invoke<TEventKey>(TEventKey eventKey, TEventArgs data)
            {
                List<Action<TEventArgs>> eventHandlerList;
                if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    int count = eventHandlerList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        eventHandlerList[i]?.Invoke(data);
                    }
                }
            }
        }
    }

    namespace V3
    {
        public class EventType
        {
            public const EventType Empty = null;
        }

        public class EventManager<TEvent, TData> where TEvent : EventType
        {
            private static Dictionary<Type, List<Action<TData>>> dataEvents;

            static EventManager()
            {
                dataEvents = new Dictionary<Type, List<Action<TData>>>();
            }

            public static void AddListener(Action<TData> eventHandler)
            {
                List<Action<TData>> eventHandlerList;
                if (dataEvents.TryGetValue(typeof(TData), out eventHandlerList))
                {
                    eventHandlerList.Add(eventHandler);
                }
                else
                {
                    dataEvents.Add(typeof(TData), new List<Action<TData>>() { eventHandler });
                }
            }

            public static void RemoveListener(Action<TData> eventHandler)
            {
                List<Action<TData>> eventHandlerList;
                if (dataEvents.TryGetValue(typeof(TData), out eventHandlerList))
                {
                    eventHandlerList.Remove(eventHandler);
                }
            }

            public static void Invoke(TData data)
            {
                List<Action<TData>> eventHandlerList;
                if (dataEvents.TryGetValue(typeof(TData), out eventHandlerList))
                {
                    int count = eventHandlerList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        eventHandlerList[i]?.Invoke(data);
                    }
                }
            }
        }
    }
}