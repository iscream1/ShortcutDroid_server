using System.Drawing;
using System.Windows.Forms;
using System.Net;
using ZXing;
using ZXing.Common;

namespace ShortcutDroid
{
    public partial class QRform : Form
    {
        Bitmap QRbitmap;

        public QRform()
        {
            InitializeComponent();
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
