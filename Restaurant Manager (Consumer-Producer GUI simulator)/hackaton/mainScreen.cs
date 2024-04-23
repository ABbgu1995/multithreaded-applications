using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace hackaton
{
    public partial class mainScreen : Form
    {

        displayStatistical displayStatistical;
        public mainScreen()
        {
            InitializeComponent();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (displayStatistical == null || displayStatistical.IsDisposed)
            {
                this.Hide();
                displayStatistical = new displayStatistical();
                displayStatistical.Show();
            }
            //displayStatistical.Visible = true;

            else
            {
                displayStatistical.Activate();
            }

            String s = producers.Text;
            int num_of_producers = int.Parse(s);

            String s1 = consumers.Text;
            int num_of_consumers = int.Parse(s1);

            displayStatistical.startRuninng(num_of_producers, num_of_consumers, int.Parse(producers_rate.Text), int.Parse(consumers_rate.Text));
        }

        private void mainScreen_Load(object sender, EventArgs e)
        {

        }

        private void consumers_TextChanged(object sender, EventArgs e)
        {

        }
    }

}