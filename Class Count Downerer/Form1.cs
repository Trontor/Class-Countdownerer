using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Class_Count_Downerer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BaseClass();
        }
        TimeSpan timespan = new TimeSpan();
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        List<DateTime> times = new List<DateTime>();
        bool FoundClass = false;

        private void BaseClass()
        {
            DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 20, 0);
            for (int i = 0; i < 11; i++)
            {
                if (i == 3)
                    time = time.AddMinutes(20);
                else
                    time = time.AddMinutes(40);
                times.Add(time);
            }


            t.Tick += t_Tick;
            t.Interval = 1;
            t.Start();
            icon.ShowBalloonTip(1000, "Class Countdown", "Timer initialised...", ToolTipIcon.Info);
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        Color txtColour()
        {
            if (timespan.TotalSeconds > 1600)
                return Color.FromArgb(0, (int)(((2400 - timespan.TotalSeconds) / 3.1372549)), 0);
            else if (timespan.TotalSeconds > 800)
                return Color.FromArgb(255, (int)((1600 - timespan.TotalSeconds) / 3.1372549), 0);
            else
                return Color.FromArgb((int)(((800 - timespan.TotalSeconds) / 3.1372549)), 0, 0);
        }

        double NthRoot(double A, double N)
        {
            return Math.Pow(A, 1.0 / N);
        }

        void t_Tick(object sender, EventArgs e)
        {
            Window();
        }
        void Window()
        {
            FloatingOSDWindow window = new FloatingOSDWindow();
            DateTime nxtPer = getNextPeriod();
            if (!FoundClass)
                return;

            timespan = (nxtPer - DateTime.Now);
            float StartSize = 1f;
            float EndSize = 40;
            float Time = 60 * 40;
            float R = (float)(NthRoot(EndSize / StartSize, Time) - 1);
            float Size = (float)(StartSize * Math.Pow((1 + R), 2400 - timespan.TotalSeconds));
            Debug.WriteLine(Size);
            Debug.WriteLine(timespan.TotalSeconds);
            Debug.WriteLine(txtColour());
            Point pt_Mouse = new Point(System.Windows.Forms.Cursor.Position.X + 10, System.Windows.Forms.Cursor.Position.Y + 10);
            window.Show(pt_Mouse,
                    (byte)255,
                    txtColour(),
                    new System.Drawing.Font("Segoe UI", Size),
                    20,
                    FloatingWindow.AnimateMode.SlideTopToBottom,
                    0,
                    timespan.Minutes + ":" + (timespan.Seconds.ToString().Length == 1 ? "0" + timespan.Seconds.ToString() : timespan.Seconds.ToString()));

        }

        DateTime getNextPeriod()
        {
            foreach (DateTime t in times)
            {
                if (DateTime.Now < t)
                {
                    FoundClass = true;
                    return t;
                }
            }
            FoundClass = false;
            return DateTime.Now; ;
        }

        private void icon_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit the class countdownerer?", "???", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                this.Close();
        }
    }
}
