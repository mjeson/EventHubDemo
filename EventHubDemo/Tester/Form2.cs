using System;
using System.Drawing;
using System.Windows.Forms;

namespace EventHubDemo
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private Form1 form1;

        private void Form2_Load(object sender, EventArgs e)
        {
            form1 = new Form1();
            form1.Show();
            form1.Location = new Point(Width + Location.X, Location.Y);

            var form11 = new Form1();
            form11.Show();
            form11.Location = new Point(form1.Width + form1.Location.X, form1.Location.Y);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            EventHub.Singleton.Publish("AddCount2Requested", this, new ObjectEventArgs(10));
        }

        private void gcButton_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }
    }
}
