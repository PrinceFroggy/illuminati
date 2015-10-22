using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace illuminati
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Form2 form2;

        CSHARPWEBCAM.WebCam webcam;

        public Form1()
        {
            InitializeComponent();

            form2 = new Form2(this);
            form2.Visible = true;

            webcam = new CSHARPWEBCAM.WebCam();
            webcam.InitializeWebCam(ref pictureBox1);

            webcam.Start();
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            form2.Location = new Point(this.Location.X + 103, this.Location.Y - 130);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            webcam.AdvanceSetting();
            pictureBox1.Focus();
        }
    }
}
