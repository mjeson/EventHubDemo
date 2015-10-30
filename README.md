# EventHubDemo
A simple attribute-based Event Aggregator.

Usage:

To subsribe and unsubscribe

    public class Form1
    {
        public Form1()
        {
            //To subscribe this instance
            EventHub.Singleton.SubscribeInstance(this);
        }

        [EventSubscription("AddCount2Requested")]
        public void AddCount2(object sender, ObjectEventArgs e)
        {
            int count = ((int) e.Value);
        }

        private void destroy()
        {
            //To unsubscribe
            EventHub.Singleton.UnsubscribeInstance(this);
        }
    }

To publish

        EventHub.Singleton.Publish("AddCount2Requested", this, new ObjectEventArgs(10));

TODO:
-    Make it to allow generics, i.e. using DataEventArgs&lt;T&gt; instead of ObjectEventArgs


