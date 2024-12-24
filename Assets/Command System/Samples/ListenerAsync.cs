using UnityEngine;

namespace CSoft
{
    public class ListenerAsync : MonoBehaviour
    {
        private async void Start()
        {
            await CommandSystem.ListenToAsync((EventOne e) => new ReturningEvent { Count = 10 });
            var returningEvents = await CommandSystem.TriggerAsync<EventOne, ReturningEvent>(new EventOne());
            foreach (var returning in returningEvents)
                Debug.Log(returning.Count);
        }

    }
}
