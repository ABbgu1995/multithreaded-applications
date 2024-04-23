using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace hackaton
{
    public partial class displayStatistical : Form
    {
        Resrestaurant rst;
        private SynchronizationContext _context;
        private int totalWaitingTime;



        public displayStatistical()
        {
            InitializeComponent();
            rst = new Resrestaurant(12);
            totalWaitingTime = 0;
        }

        private void UpdateLabel(int nop, int noc, int pr, int cr)
        {
            while (true)
            {
                if (label1.InvokeRequired)
                {
                    label1.Invoke(new Action(() =>
                    {
                        label1.Text = CalculateCapacity().ToString();
                    }));
                }
                else
                {
                    label1.Text = CalculateCapacity().ToString();
                }

                if (label3.InvokeRequired)
                {
                    label3.Invoke(new Action(() =>
                    {
                        label3.Text = DateTime.Now.ToString();
                    }));
                }
                else
                {
                    label3.Text = DateTime.Now.ToString();
                }

                if (label5.InvokeRequired)
                {
                    label5.Invoke(new Action(() =>
                    {
                        label5.Text = CalculateWaiting(nop, noc, pr, cr).ToString();
                    }));
                }
                else
                {
                    label5.Text = CalculateWaiting(nop, noc, pr, cr).ToString();
                }

                if (label7.InvokeRequired)
                {
                    label7.Invoke(new Action(() =>
                    {
                        label7.Text = $"Average Waiting Time: {AverageWaitingTimeLabel(rst.getToatalWaitingTime(), rst.getCount()):F2} seconds";
                    }));
                }
                else
                {
                    label7.Text = $"Average Waiting Time: {AverageWaitingTimeLabel(rst.getToatalWaitingTime(), rst.getCount()):F2} seconds";
                }
                if (pictureBoxFreeChairs.InvokeRequired)
                {
                    int numoffree = rst.getFreeChairs();
                    pictureBoxFreeChairs.Invoke(new Action(() =>
                    {
                        // Set the image based on the number of free chairs
                        
                        if (numoffree >= 0 && numoffree <= 12)
                        {
                            string imageName = Path.Combine(System.Windows.Forms.Application.StartupPath, $"{rst.getFreeChairs()}.png");
                            pictureBoxFreeChairs.Image = System.Drawing.Image.FromFile(imageName);
                        }
                    }));
                }
                else
                {
                    int numoffree = rst.getFreeChairs();
                    if (numoffree >= 0 && numoffree <= 12)
                    {
                        // Set the image based on the number of free chairs
                        string imageName = Path.Combine(System.Windows.Forms.Application.StartupPath, $"{rst.getFreeChairs()}.png");
                        pictureBoxFreeChairs.Image = System.Drawing.Image.FromFile(imageName);
                    }
                }

            }
        }





        public int CalculateCapacity()
        {
            if (rst.getCount() >= rst.getSize()) { return rst.getSize(); }


            return rst.getCount();


        }

        public int CalculateWaiting(int nop, int noc, int pr, int cr)
        {


            if (rst.getCount() < rst.getSize()) { return 0; }
            int waiting = nop - rst.getEmptyPlaces();
            return waiting;



        }

        public double AverageWaitingTimeLabel(int totalWaitingTime, int count)
        {
            return (double)totalWaitingTime / count;
        }

        public void startRuninng(int nop, int noc, int pr, int cr)
        {
            Thread UpdateThread = new Thread(() => this.UpdateLabel(nop, noc, pr, cr));
            UpdateThread.Start();

            Thread addGroupThread = new Thread(() => rst.AddGroup(pr, nop));
            addGroupThread.Start();

            Thread removeGroupThread = new Thread(() => rst.RemoveGroup(cr, noc));
            removeGroupThread.Start();

            //updateThread.Join();
            //addGroupThread.Join();



        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
        }
    }
}
