using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entityx
{
   internal  class EventSignal
    {
        private delegate void EventDelegate<T>(T e);
        private delegate void EventDelegate(object e);

        public void Link<Event, Receiver>(Receiver receiver)
        {
            var receiverMethodInfo = typeof(Receiver).GetMethod("Receive", new Type[]{ typeof(Event) });
            var types = receiverMethodInfo.GetParameters().Select(p => p.ParameterType);
            var eventDelegate = Delegate.CreateDelegate(typeof(EventDelegate), receiver, receiverMethodInfo.Name);
            links.Add(typeof(Receiver), (EventDelegate)eventDelegate);
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

        Dictionary<Type, EventDelegate> links;
	};
}
