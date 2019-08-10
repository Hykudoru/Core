using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gemukodo.Events
{
    namespace v1
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
                eventDictionary = new Dictionary<int, UnityEvent>(10);
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
    }

    namespace v2
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

    namespace v3
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

    namespace v4
    {
        public partial class EventManager
        {
            private static Dictionary<int, List<Action>> emptyEvents;
            private static Dictionary<int, List<Action<object>>> dataEvents;

            static EventManager()
            {
                emptyEvents = new Dictionary<int, List<Action>>();
                dataEvents = new Dictionary<int, List<Action<object>>>();
            }

            // ADD LISTENER

            public static void Register<TEventKey>(TEventKey eventKey, Action eventHandler)
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

            public static void Register<TEventKey, TParam>(TEventKey eventKey, Action<TParam> eventHandler)
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
            
            public static void Register<TParam>(object eventKey, Action<TParam> eventHandler)
            {
                Register<object, TParam>(eventKey, eventHandler);
            }

            public static void Register<TParam>(String eventKey, Action<TParam> eventHandler)
            {
                Register<string, TParam>(eventKey, eventHandler);
            }

            public static void Register<TParam>(int eventKey, Action<TParam> eventHandler)
            {
                Register<int, TParam>(eventKey, eventHandler);
            }
            
            // REMOVE LISTENER

            public static void Unregister<TEventKey>(TEventKey eventKey, Action eventHandler)
            {
                List<Action> eventHandlerList;
                if (emptyEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Remove(eventHandler);
                }
            }

            public static void Unregister<TEventKey, TParam>(TEventKey eventKey, Action<TParam> eventHandler)
            {
                List<Action<object>> eventHandlerList;
                if (dataEvents.TryGetValue(eventKey.GetHashCode(), out eventHandlerList))
                {
                    eventHandlerList.Remove(eventHandler as Action<object>);
                }
            }
            
            public static void Unregister<TParam>(object eventKey, Action<TParam> eventHandler)
            {
                Unregister<object, TParam>(eventKey, eventHandler);
            }

            public static void Unregister<TParam>(String eventKey, Action<TParam> eventHandler)
            {
                Unregister<string, TParam>(eventKey, eventHandler);
            }

            public static void Unregister<TParam>(int eventKey, Action<TParam> eventHandler)
            {
                Unregister<int, TParam>(eventKey, eventHandler);
            }
            
            public static void Trigger<TEventKey>(TEventKey eventKey)
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

            public static void Trigger<TEventKey, TData>(TEventKey eventKey, TData data)
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

        /*
        * A single event type 'TEvent' can have several delegate signature variations that invoke only when 
        * the eventtype AND the event handler's delegate signature matches with TArgs.
        *
        * Examples: 
        * ---------------------------------
        * Event<OnPlayerSpawned>.Trigger(); 
        *  - Invokes "Action" signature type event handlers listening for "OnPlayerSpawned".)
        *---------------------------------
        * Event<OnPlayerSpawned>.Trigger<Vector3>(player.position); 
        *  - Invokes 'Action<Vector3>' signature type event handlers listening for "OnPlayerSpawned".)
        *  ---------------------------------
        * Event<OnPlayerSpawned>.Trigger<Player>(player.position);
        *  - Invokes "Action<Player>' signature type event handlers for "OnPlayerSpawned) 
        */
        public partial class EventManager<TEvent>
        {
            public static event Action OnEvent;
            static Dictionary<Type, Delegate> eventSignatureHandlers;

            static EventManager()
            {
                eventSignatureHandlers = new Dictionary<Type, Delegate>();
                OnEvent = () => { };
            }

            protected static Delegate GetEventHandlers<TArgs>()
            {
                Action<TArgs> ev;
                if (!eventSignatureHandlers.ContainsKey(typeof(TArgs)))
                {
                    ev = arg => { };
                    eventSignatureHandlers.Add(typeof(TArgs), ev);
                }
                else
                {
                    ev = (Action<TArgs>)eventSignatureHandlers[typeof(TArgs)];
                }

                return eventSignatureHandlers[typeof(TArgs)];
            }

            public static void Register(Action handler)
            {
                OnEvent += handler;
            }

            public static void Unregister(Action handler)
            {
                OnEvent -= handler;
            }

            public static void Trigger()
            {
                OnEvent?.Invoke();
            }

            public static void Register<TArgs>(Action<TArgs> handler)
            {
                Action<TArgs> ev = GetEventHandlers<TArgs>() as Action<TArgs>;
                eventSignatureHandlers[typeof(TArgs)] = ev += handler;
            }

            public static void Unregister<TArgs>(Action<TArgs> handler)
            {
                Action<TArgs> action = GetEventHandlers<TArgs>() as Action<TArgs>;
                eventSignatureHandlers[typeof(TArgs)] = action -= handler;
            }

            public static void Trigger<TArgs>(TArgs args)
            {
                ((Action<TArgs>)GetEventHandlers<TArgs>())?.Invoke(args);
                OnEvent?.Invoke();
            }
        }
    }

    namespace v5
    {
        public delegate void Void();
        public delegate void Callback();
        public delegate void Callback<T>(T param);
        public delegate void Callback<T1, T2>(T1 param, T2 param);
        public delegate void Callback<T1, T2, T3>(T1 param, T2 param, T3 param);
        public delegate void Callback<T1, T2, T3, T4>(T1 param, T2 param, T3 param, T4 param);

        public partial class EventManager
        {
            public static void Register<TEvent, TArg>(Action<TArg> handler)
            {
                Event<TArg>.Register<TEvent>(handler);
            }

            public static void Register<TEvent, TArg, TArg2>(Action<TArg, TArg2> handler)
            {
                Event<TArg, TArg2>.Register<TEvent>(handler);
            }

            public static void Unregister<TEvent, TArg>(Action<TArg> handler)
            {
                Event<TArg>.Unregister<TEvent>(handler);
            }

            public static void Unregister<TEvent, TArg, TArg2>(Action<TArg, TArg2> handler)
            {
                Event<TArg, TArg2>.Unregister<TEvent>(handler);
            }

            public static void Trigger<TEvent, TArg>(TArg arg)
            {
                Event<TArg>.Trigger<TEvent>(arg);
            }

            public static void Trigger<TEvent, TArg, TArg2>(TArg arg, TArg2 arg2)
            {
                Event<TArg, TArg2>.Trigger<TEvent>(arg, arg2);
            }
        }

        public partial class EventManager<TEvent>
        {
            public static void Register<TArg, TArg2>(Action<TArg, TArg2> handler)
            {
                Event<TArg, TArg2>.Register<TEvent>(handler);
            }

            public static void Unregister<TArg, TArg2>(Action<TArg, TArg2> handler)
            {
                Event<TArg, TArg2>.Unregister<TEvent>(handler);
            }

            public static void Trigger<TArg, TArg2>(TArg arg, TArg2 arg2)
            {
                Event<TArg, TArg2>.Trigger<TEvent>(arg, arg2);
            }
        }

        public class Event<T1>
        {
            protected static Dictionary<object, Action<T1>> events;
            static protected Action<T1> e;

            static Event()
            {
                events = new Dictionary<object, Action<T1>>();
            }

            public static void Register<TEvent>(Action<T1> handler)
            {
                if (events.TryGetValue(typeof(TEvent), out e))
                {
                    e += handler;
                }
                else
                {
                    events.Add(typeof(TEvent), handler);
                }
            }

            public static void Register<TEvent>(TEvent eventKey, Action<T1> handler)
            {
                if (events.TryGetValue(eventKey, out e))
                {
                    e += handler;
                }
                else
                {
                    events.Add(typeof(TEvent), handler);
                }
            }

            public static void Unregister<TEvent>(Action<T1> handler)
            {
                if (events.TryGetValue(typeof(TEvent), out e))
                {
                    e -= handler;
                }
            }

            public static void Unregister<TEvent>(TEvent eventKey, Action<T1> handler)
            {
                if (events.TryGetValue(eventKey, out e))
                {
                    e -= handler;
                }
            }

            public static void Trigger<TEvent>(T1 p1)
            {
                if (events.TryGetValue(typeof(TEvent), out e))
                {
                    e?.Invoke(p1);
                }
            }

            public static void Trigger<TEvent>(TEvent eventKey, T1 p1)
            {
                if (events.TryGetValue(eventKey, out e))
                {
                    e?.Invoke(p1);
                }
            }
        }

        public class Event<T1, T2> : Event<T1>
        {
            static Dictionary<object, Action<T1, T2>> events;
            static Action<T1, T2> e;

            static Event()
            {
                events = new Dictionary<object, Action<T1, T2>>();
            }

            public static void Register<TEvent>(Action<T1, T2> handler)
            {
                if (events.TryGetValue(typeof(TEvent), out e))
                {
                    e += handler;
                }
                else
                {
                    events.Add(typeof(TEvent), handler);
                }
            }

            public static void Register<TEvent>(TEvent eventKey, Action<T1, T2> handler)
            {
                if (events.TryGetValue(eventKey, out e))
                {
                    e += handler;
                }
                else
                {
                    events.Add(eventKey, handler);
                }
            }

            public static void Unregister<TEvent>(Action<T1, T2> handler)
            {
                if (events.TryGetValue(typeof(TEvent), out e))
                {
                    e -= handler;
                }
            }

            public static void Unregister<TEvent>(TEvent eventKey, Action<T1, T2> handler)
            {
                if (events.TryGetValue(eventKey, out e))
                {
                    e -= handler;
                }
            }

            public static void Trigger<TEvent>(T1 p1, T2 p2)
            {
                if (events.TryGetValue(typeof(TEvent), out e))
                {
                    e?.Invoke(p1, p2);
                }
            }

            public static void Trigger<TEvent>(TEvent eventKey, T1 p1, T2 p2)
            {
                if (events.TryGetValue(eventKey, out e))
                {
                    e?.Invoke(p1, p2);
                }
            }
        }
    }
}