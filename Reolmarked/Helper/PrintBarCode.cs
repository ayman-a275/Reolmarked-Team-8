using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using ZXing;
using ZXing.Common;

namespace Reolmarked.Helper
{
    public static class PrintBarCode
    {

        /*
        Klasse til at printe barcoder. Skrevet af AI.
         */

        private static Bitmap _barcodeToPrint;

        public static void PrintBarcode(string serialNumber)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 100,
                    Width = 300,
                    Margin = 10,
                    PureBarcode = false
                }
            };

            var pixelData = writer.Write(serialNumber);
            _barcodeToPrint = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);

            var bmpData = _barcodeToPrint.LockBits(
                new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb
            );

            System.Runtime.InteropServices.Marshal.Copy(
                pixelData.Pixels,
                0,
                bmpData.Scan0,
                pixelData.Pixels.Length
            );

            _barcodeToPrint.UnlockBits(bmpData);

            var printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPage;
            printDocument.Print();

            _barcodeToPrint.Dispose();
        }

        private static void PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = (e.PageBounds.Width - _barcodeToPrint.Width) / 2;
            int y = (e.PageBounds.Height - _barcodeToPrint.Height) / 2;
            e.Graphics.DrawImage(_barcodeToPrint, x, y);
        }
    }
}