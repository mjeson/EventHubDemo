using System;

namespace EventHubDemo
{
    public class ObjectEventArgs : EventArgs
    {
        public object Value { get; private set; }
        public ObjectEventArgs(object value)
        {
            this.Value = value;
        }
    }
}
