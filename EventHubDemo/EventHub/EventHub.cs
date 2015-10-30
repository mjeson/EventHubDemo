using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventHubDemo
{
    public class EventHub
    {
        private readonly static EventHub _singleton = new EventHub();
        public static EventHub Singleton { get { return _singleton; } }

        private readonly Dictionary<string, string> _topicMap = new Dictionary<string, string>();
        private readonly Dictionary<string, Action<object, ObjectEventArgs>> _actionMap = new Dictionary<string, Action<object, ObjectEventArgs>>();
        private readonly Dictionary<string, List<Action<object, ObjectEventArgs>>> _eventMap = new Dictionary<string, List<Action<object, ObjectEventArgs>>>();

        public void Publish(string eventTopic, object sender, ObjectEventArgs eventData)
        {
            if (!_eventMap.ContainsKey(eventTopic))
            {
                return;
            }

            foreach (Action<object, ObjectEventArgs> action in _eventMap[eventTopic])
            {
                action.Invoke(sender, eventData);
            }
        }

        public void SubscribeInstance(object instance)
        {
            MethodInfo[] methods = instance.GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(EventSubscriptionAttribute), false)
                    .OfType<EventSubscriptionAttribute>()
                    .ToList();
                foreach (var attribute in attributes)
                {
                    Action<object, ObjectEventArgs> action = (Action<object, ObjectEventArgs>)Delegate.CreateDelegate(typeof(Action<object, ObjectEventArgs>), instance, method);
                    string id = string.Format("{0}@{1}@{2}", attribute.Topic, method.MetadataToken, instance.GetHashCode());
                    subscribe(attribute.Topic, id, action);
                }
            }
        }

        public void UnsubscribeInstance(object instance)
        {
            MethodInfo[] methods = instance.GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(EventSubscriptionAttribute), false)
                    .OfType<EventSubscriptionAttribute>()
                    .ToList();
                foreach (var attribute in attributes)
                {
                    string id = string.Format("{0}@{1}@{2}", attribute.Topic, method.MetadataToken, instance.GetHashCode());
                    unsubscribe(id);
                }
            }
        }

        private void subscribe(string eventTopic, string id, Action<object, ObjectEventArgs> action)
        {
            if (_actionMap.ContainsKey(id))
            {
                throw new Exception(string.Format("Subscribing same ID twice: {0}", id));
            }

            if (!_eventMap.ContainsKey(eventTopic))
            {
                _eventMap[eventTopic] = new List<Action<object, ObjectEventArgs>>();
            }

            _topicMap.Add(id, eventTopic);
            _actionMap.Add(id, action);
            _eventMap[eventTopic].Add(action);
        }

        private void unsubscribe(string id)
        {
            if (!_actionMap.ContainsKey(id))
            {
                return;
            }
            var action = _actionMap[id];
            var topic = _topicMap[id];

            _eventMap[topic].Remove(action);
            if (_eventMap[topic].Count == 0)
            {
                _eventMap.Remove(topic);
            }

            _actionMap.Remove(id);
            _topicMap.Remove(id);
        }
    }

}
