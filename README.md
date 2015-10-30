# EventHubDemo
A simple Event Aggregator that uses attributes.

Usage:

To subsribe and unsubscribe

    public partial class Form1 : Form
    {
        private int _count;

        public Form1()
        {
            InitializeComponent();
            EventHub.Singleton.SubscribeInstance(this);
        }

        [EventSubscription("AddCount2Requested")]
        public void AddCount2(object sender, ObjectEventArgs e)
        {
            _count = _count + ((int) e.Value);
            label2.InvokeIfRequired(c => { c.Text = _count.ToString(); });
        }

        private void destroy()
        {
            EventHub.Singleton.UnsubscribeInstance(this);
        }
    }

To publish

        private void addButton_Click(object sender, EventArgs e)
        {
            EventHub.Singleton.Publish("AddCount2Requested", this, new ObjectEventArgs(10));
        }

TODO:
-    Make it to allow generics, i.e. using DataEventArgs&lt;T&gt; instead of ObjectEventArgs


