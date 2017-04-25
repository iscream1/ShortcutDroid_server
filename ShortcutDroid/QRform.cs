using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using ZXing;
using ZXing.Common;
using System.Threading;

namespace ShortcutDroid
{
    public partial class QRform : Form
    {
        Bitmap QRbitmap;

        public QRform()
        {
            InitializeComponent();
            
            string asd = "https://www.google.hu/webhp?sourceid=chrome-instant&ion=1&espv=2&ie=UTF-8#q=boolean+not+character&*";
            Thread.Sleep(1000);
            foreach (char c in asd) SendKeys.SendWait(c + "{ENTER}");
            makeQR();
        }

        private void makeQR()
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300
                }
            };
            QRbitmap = barcodeWriter.Write(getExtIP());
        }

        private string getExtIP()
        {
            string ip = new WebClient().DownloadString("http://icanhazip.com");
            //Console.WriteLine(ip);
            return ip;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Brush br = Brushes.Red;
            if (QRbitmap != null) e.Graphics.DrawImage(QRbitmap, 0, 0);
        }
    }
}
