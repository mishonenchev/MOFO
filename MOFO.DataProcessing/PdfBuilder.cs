using iTextSharp.text;
using iTextSharp.text.pdf;
using MOFO.DataProcessing.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZXing;

namespace MOFO.DataProcessing
{
    public class PdfBuilder:IPdfBuilder
    {
        private static Font f12;
        private static Font f14;
        private static Font f14Bold;
        private static Font f12Bold;
        private static Font f7;
        public PdfBuilder()
        {
            var basePath = System.AppDomain.CurrentDomain.RelativeSearchPath;
            string ARIALUNI_TFF = basePath + @"\ARIALUNI.TTF";
            BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            f12 = new Font(bf, 9, Font.NORMAL);
            f14 = new Font(bf, 11, Font.NORMAL);
            f14Bold = new Font(bf, 11, Font.BOLD);
            f12Bold = new Font(bf, 9, Font.BOLD);
            f7 = new Font(bf, 7, Font.NORMAL);
        }
        public FileContentResult GetCardsPdf(List<Card> cards)
        {
            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 50, 50, 70, 70);
                var writer = PdfWriter.GetInstance(doc, ms);
                doc.Open(); 
                PdfContentByte cb = writer.DirectContent;
                cb.SetColorStroke(BaseColor.BLACK);
                cb.SetLineWidth(1.8);
                var rowCount = 0;
                var currentX = 60;
                var currentY = (int)doc.PageSize.Height - 182;
                for (int i = 0; i < cards.Count; i++)
                {
                    if (i != 0 && i % 2 == 0)
                    {
                        //3.12 * milimeters to make it in pixels
                        rowCount++;
                        currentX = 60;
                        currentY= (int)doc.PageSize.Height - 182 - rowCount * 172;
                        if (currentY < 40)
                        {
                            doc.NewPage();
                            currentY = (int)doc.PageSize.Height - 182;
                            rowCount = 0;
                        }
                    }

                    var rect = new iTextSharp.text.Rectangle(currentX, currentY, 230, 162);
                    rect.Border = Rectangle.BOX;
                    cb.Rectangle(currentX, currentY, 230, 162);
                    cb.Stroke();
                    var barcodeWriter = new BarcodeWriter
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new ZXing.Common.EncodingOptions
                        {
                            Height = 140,
                            Width = 140,
                            Margin = 1,
                            PureBarcode = true
                        }
                    };
                    var barcodeBitmap = barcodeWriter.Write(cards[i].QRCode);
                    var barcodeMemoryStream = new MemoryStream();
                    barcodeBitmap.Save(barcodeMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    barcodeMemoryStream.Seek(0, SeekOrigin.Begin);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(barcodeMemoryStream);
                    image.SetAbsolutePosition(currentX + 45, currentY + 15);
                    cb.AddImage(image);
                    ColumnText ct = new ColumnText(cb);
                    Phrase myText = new Phrase("Ref " + cards[i].ReferenceNumber, f12);
                    ct.SetSimpleColumn(myText, currentX + 55, currentY+25, currentX + 175, currentY+5, 15, Element.ALIGN_CENTER);
                    ct.Go();

                    currentX += 240;

                   

                }
                doc.Close();
                bytes = ms.ToArray();
            }
            return new FileContentResult(bytes, "application/pdf");
        }
    }
}
