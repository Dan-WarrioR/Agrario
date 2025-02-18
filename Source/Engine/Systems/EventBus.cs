using Source.Engine.Tools;
using Source.Engine.Tools.ProjectUtilities;

namespace Source.Engine.Systems
{
    public class EventBus
    {
        private Dictionary<string, Action> _events = new();
        
        public EventBus()
        {
            Dependency.Register(this);
        }

        public void Register(string eventName, Action listener)
        {
            if (!_events.ContainsKey(eventName))
            {
                _events[eventName] = listener;
            }
            else
            {
                _events[eventName] += listener;
            }
        }

        public void Unregister(string eventName, Action listener)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName] -= listener;
            }
        }

        public void Invoke(string eventName)
        {
            if (!_events.TryGetValue(eventName, out var action))
            {
                Debug.LogWarning($"No event in eventBus with name: {eventName}!");
                
                return;
            }
            
            action?.Invoke();
        }
    }
}