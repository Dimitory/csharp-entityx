using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eventsx
{
    internal class EventSignal
    {
        private delegate void EventDelegate<T>(T e);
        private delegate void EventDelegate(object e);

        Dictionary<Type, EventDelegate> links = new Dictionary<Type, EventDelegate>();

        public void Link<Event, Receiver>(Receiver receiver)
        {
            var receiverMethodInfo = typeof(Receiver).GetMethod("Receive", new Type[] { typeof(Event) });
            var eventDelegate = receiverMethodInfo.CreateDelegate(typeof(EventDelegate<Event>),receiver) as EventDelegate<Event>;
            EventDelegate internalDelegate = (e) => eventDelegate((Event)e);
            links[typeof(Receiver)] = internalDelegate;
        }

        public void Unlink<Receiver>(Receiver receiver)
        {
            links.Remove(typeof(Receiver));
        }

        public void Emit<Event>(Event ev)
        {
            foreach (var link in links)
            {
                link.Value.Invoke(ev);
            }
        }
    };
}
