using System;
using System.Collections.Generic;

namespace eventsx
{
    public class EventSystem
    {
        Dictionary<Type, EventSignal> connections = new Dictionary<Type, EventSignal>();
        public void Subscribe<Event, Receiver>(Receiver receiver)
        {
            var signal = GetSignal<Event>();
            signal.Link<Event, Receiver>(receiver);
        }

        public void Unsubscribe<Event, Receiver>(Receiver receiver)
        {
            var signal = GetSignal<Event>();
            signal.Unlink(receiver);
        }

        public void Emit<Event>(Event ev)
        {
            var signal = GetSignal<Event>();
            signal.Emit(ev);
        }

        public void Emit<Event>(params object[] args)
        {
            var eventType = typeof(Event);
            var ev = Activator.CreateInstance(eventType, args);
            var signal = GetSignal<Event>();
            signal.Emit(ev);
        }

        private EventSignal GetSignal<Event>()
        {
            var eventType = typeof(Event);
            EventSignal signal = null;
            connections.TryGetValue(eventType, out signal);
            if (signal == null)
            {
                signal = new EventSignal();
                connections.Add(eventType, signal);
            }
            return signal;
        }
    }
}
