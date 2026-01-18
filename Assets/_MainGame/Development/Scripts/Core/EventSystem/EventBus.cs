using System;
using System.Collections.Generic;

namespace VertigoCase.Runtime
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _handlers = new();
        public static void Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) return;

            var type = typeof(TEvent);

            if (_handlers.TryGetValue(type, out var existing))
                _handlers[type] = (Action<TEvent>)existing + handler;
            else
                _handlers[type] = handler;
        }

        public static void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            if (handler == null) return;

            var type = typeof(TEvent);

            if (!_handlers.TryGetValue(type, out var existing))
                return;

            var newDel = (Action<TEvent>)existing - handler;

            if (newDel == null)
                _handlers.Remove(type);
            else
                _handlers[type] = newDel;
        }

        public static void Fire<TEvent>(TEvent evt)
        {
            var type = typeof(TEvent);

            if (_handlers.TryGetValue(type, out var existing))
                ((Action<TEvent>)existing)?.Invoke(evt);
        }
        public static void Fire<TEvent>() where TEvent : struct
        {
            Fire(default(TEvent));
        }

        public static void Subscribe<TEvent>(Action handler)
        {
            if (handler == null) return;

            Action<TEvent> wrapped = _ => handler();
            Subscribe(wrapped);
            SignalHandlerCache<TEvent>.Map(handler, wrapped);
        }

        public static void Unsubscribe<TEvent>(Action handler)
        {
            if (handler == null) return;

            if (SignalHandlerCache<TEvent>.TryGet(handler, out var wrapped))
            {
                Unsubscribe(wrapped);
                SignalHandlerCache<TEvent>.Remove(handler);
            }
        }

        public static void ClearAll()
        {
            _handlers.Clear();
            SignalHandlerCache.ClearAll();
        }
    }

    internal static class SignalHandlerCache<TEvent>
    {
        private static readonly Dictionary<Action, Action<TEvent>> _map = new();

        public static void Map(Action original, Action<TEvent> wrapped)
        {
            _map[original] = wrapped;
        }

        public static bool TryGet(Action original, out Action<TEvent> wrapped)
            => _map.TryGetValue(original, out wrapped);

        public static void Remove(Action original)
        {
            _map.Remove(original);
        }
    }

    internal static class SignalHandlerCache
    {
        private static readonly List<IDictionary<Action, Delegate>> _all = new();

        public static void Register(IDictionary<Action, Delegate> dict)
        {
            _all.Add(dict);
        }

        public static void ClearAll()
        {
            _all.Clear();
        }
    }
}


