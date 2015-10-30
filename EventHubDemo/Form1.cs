using System;
using System.Windows.Forms;

namespace EventHubDemo
{
    public partial class Form1 : Form
    {
        private int _count;

        public Form1()
        {
            InitializeComponent();
            EventHub.Singleton.SubscribeInstance(this);
        }

        public void AddCount1(object sender, EventArgs e)
        {
            ++_count;
            label1.InvokeIfRequired(c => { c.Text = _count.ToString(); });
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
}
