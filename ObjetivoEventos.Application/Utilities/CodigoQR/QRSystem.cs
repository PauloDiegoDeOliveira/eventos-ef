using QRCoder;
using System.Drawing;
using System.IO;

namespace ObjetivoEventos.Application.Utilities.CodigoQR
{
    public static class QRSystem
    {
        public static Bitmap GenerateImage(string QRText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);
            return qrCodeImage;
        }

        public static byte[] GenerateByteArray(string QRText)
        {
            var image = GenerateImage(QRText);
            return ImageToByte(image);
        }

        private static byte[] ImageToByte(Image img)
        {
            using var stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}