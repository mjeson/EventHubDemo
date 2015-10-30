using System;

namespace EventHubDemo
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EventSubscriptionAttribute : Attribute
    {
        public EventSubscriptionAttribute(string topic)
        {
            Topic = topic;
        }

        public string Topic { get; private set; }
    }
}
