using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hykudoru.Events
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

        public static void Add<TEventKey>(TEventKey eventKey, UnityAction listener)
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

        public static void Remove<TEventKey>(TEventKey eventKey, UnityAction listener)
        {
            UnityEvent e = null;
            if (instance.eventDictionary.TryGetValue(eventKey.GetHashCode(), out e))
            {
                e.RemoveListener(listener);
            }
        }

        public static void Trigger<TEventKey>(TEventKey eventKey)
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
}

namespace Hykudoru.Events.V2
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
        
        static void Add<TEventKey>(TEventKey eventKey, Action action)
        {
            List<Action> eventActionList;
            if (emptyEvents.TryGetValue(eventKey.GetHashCode(), out eventActionList))
            {
                eventActionList.Add(action);
            }
            else
            {
                emptyEvents.Add(eventKey.GetHashCode(), new List<Action>() { action });
            }
        }

        static void Add<TEventKey, TParam>(TEventKey eventKey, Action<TParam> action)
        {
            List<Action<object>> eventActionList;
            if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventActionList))
            {
                eventActionList.Add(action as Action<object>);
            }
            else
            {
                dataEvents.Add(eventKey.GetHashCode(), new List<Action<object>>() { action as Action<object> });
            }
        }

        public static void AddListener(string eventKey, Action action)
        {
            Add(eventKey, action);
        }

        public static void AddListener(int eventKey, Action action)
        {
            Add(eventKey, action);
        }

        public static void AddListener(string eventKey, Action<object> action)
        {
            Add(eventKey, action);
        }

        public static void AddListener(int eventKey, Action<object> action)
        {
            Add(eventKey, action);
        }

        // REMOVE LISTENER

        static void Remove<TEventKey>(TEventKey eventKey, Action action)
        {
            List<Action> eventActionList;
            if (emptyEvents.TryGetValue(eventKey.GetHashCode(), out eventActionList))
            {
                eventActionList.Remove(action);
            }
        }

        static void Remove<TEventKey, TParam>(TEventKey eventKey, Action<TParam> action)
        {
            List<Action<object>> eventActionList;
            if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventActionList))
            {
                eventActionList.Remove(action as Action<object>);
            }
        }

        public static void RemoveListener(string eventKey, Action action)
        {
            Remove(eventKey, action);
        }

        public static void RemoveListener(int eventKey, Action action)
        {
            Remove(eventKey, action);
        }

        public static void RemoveListener(string eventKey, Action<object> action)
        {
            Remove(eventKey, action);
        }

        public static void RemoveListener(int eventKey, Action<object> action)
        {
            Remove(eventKey, action);
        }

        public static void Invoke<TEventKey>(TEventKey eventKey)
        {
            List<Action> eventActionList;
            if (emptyEvents.TryGetValue(eventKey.GetHashCode(), out eventActionList))
            {
                int count = eventActionList.Count;
                for (int i = 0; i < count; i++)
                {
                    Action handler = eventActionList[i];
                    if (handler != null)
                    {
                        handler();
                    }
                }
            }
        }

        public static void Invoke<TEventKey, TData>(TEventKey eventKey, TData data)
        {
            List<Action<object>> eventActionList;
            if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventActionList))
            {
                int count = eventActionList.Count;
                for (int i = 0; i < count; i++)
                {
                    Action<object> handler = eventActionList[i];
                    if (handler != null)
                    {
                        handler(data);
                    }
                }
            }
        }
    }
    
    public class EventManager<TEventArgs>
    {
        static Dictionary<int, List<Action<TEventArgs>>> dataEvents;
        static List<Action<TEventArgs>> cachedEventActionList;
        static Action<TEventArgs> cachedAction;

        static EventManager()
        {
            dataEvents = new Dictionary<int, List<Action<TEventArgs>>>();
        }

        public static void AddListener<TEventKey>(TEventKey eventKey, Action<TEventArgs> action)
        {
            if (dataEvents.TryGetValue(eventKey.GetHashCode(), out cachedEventActionList))
            {
                cachedEventActionList.Add(action);
                cachedEventActionList = null;
            }
            else
            {
                dataEvents.Add(eventKey.GetHashCode(), new List<Action<TEventArgs>>() { action as Action<TEventArgs> });
            }
        }

        public static void RemoveListener<TEventKey>(TEventKey eventKey, Action<TEventArgs> action)
        {
            if (dataEvents.TryGetValue(eventKey.GetHashCode(), out cachedEventActionList))
            {
                cachedEventActionList.Remove(action);
                cachedEventActionList = null;
            }
        }

        public static void Invoke<TEventKey>(TEventKey eventKey, TEventArgs data)
        {
            if (dataEvents.TryGetValue(eventKey.GetHashCode(), out cachedEventActionList))
            {
                int count = cachedEventActionList.Count;
                for (int i = 0; i < count; i++)
                {
                    cachedAction = cachedEventActionList[i];
                    if (cachedAction != null)
                    {
                        cachedAction(data);
                    }
                }

                cachedAction = null;
                cachedEventActionList = null;
            }
        }
    }
}

namespace Hykudoru.Events.V3
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

        // ADD LISTENER
        public static void AddListener(Action<TData> action)
        {
            List<Action<TData>> eventActionList;
            if (dataEvents.TryGetValue(typeof(TData), out eventActionList))
            {
                eventActionList.Add(action);
            }
            else
            {
                dataEvents.Add(typeof(TData), new List<Action<TData>>() { action });
            }
        }

        // REMOVE LISTENER
        public static void RemoveListener(Action<TData> action)
        {
            List<Action<TData>> eventActionList;
            if (dataEvents.TryGetValue(typeof(TData), out eventActionList))
            {
                eventActionList.Remove(action);
            }
        }

        // INVOKE
        public static void Invoke(TData data)
        {
            List<Action<TData>> eventActionList;
            if (dataEvents.TryGetValue(typeof(TData), out eventActionList))
            {
                int count = eventActionList.Count;
                for (int i = 0; i < count; i++)
                {
                    Action<TData> handler = eventActionList[i];
                    if (handler != null)
                    {
                        handler(data);
                    }
                }
            }
        }
    }
}