using System;
using UnityEngine;

namespace CSoft
{
    public class Listener : MonoBehaviour
    {
        readonly Action<EventOne> eventOneAction = (EventOne) => Debug.Log("Event One");
        readonly Action<EventTwo> eventTwoAction = (EventTwo) => Debug.Log("Event Two");

        void Start()
        {
            CommandSystem.ListenTo(eventOneAction);
            CommandSystem.ListenTo(eventTwoAction);

            CommandSystem.Trigger(new EventOne());
            CommandSystem.Trigger(new EventTwo());

            CommandSystem.ListenTo((EventOne e) => new ReturningEvent { Count = 10 });
            var returningEvents = CommandSystem.Trigger<EventOne, ReturningEvent>(new EventOne());
            foreach (var returning in returningEvents)
                Debug.Log(returning.Count);
        }

        private void OnDestroy()
        {
            CommandSystem.UnlistenTo(eventOneAction);
            CommandSystem.UnlistenTo(eventTwoAction);
        }
    }

    public struct EventOne { }
    public struct EventTwo { }
    public struct ReturningEvent
    {
        public int Count;
    }
}
