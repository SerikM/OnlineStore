using Model.EF;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebUI2.Models
{
   public class PDFFactory
    {
       public Order order;
       HttpServerUtilityBase server;

        public PDFFactory (Order order, HttpServerUtilityBase server)
        {
           this.order = order;
           this.server = server;
        }


      

       public byte[] BuildFile(HttpResponseBase response)
       {
          PdfDocument doc = new PdfDocument();
          doc.Info.Title = order.ShiptoName + "'s order";
          doc.Options.ColorMode = PdfColorMode.Cmyk;
                
          PdfPage page = doc.AddPage();
          XGraphics gr = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);
          DrawTitle(page, gr, order, doc);
                          
          DrawBody(ref gr,order, page);

          MemoryStream stream = new MemoryStream();
          doc.Save(stream, false);
          string fileName = order.ShiptoName + "s order.pdf";
          
          response.Headers.Add(" content-disposition", "inline; filename=" + fileName);
          return stream.ToArray();             
        }


 


        public void DrawTitle(PdfPage page, XGraphics gr,Order order, PdfDocument doc)
        {
            // build header
            gr.DrawRectangle(new XSolidBrush(XColors.MidnightBlue), new XRect(40, 05, page.Width - 80, 07));

            // logo image
            XImage image = XImage.FromFile(server.MapPath("/Content/MyImages/haze.png"));           
            gr.DrawImage(image, 40, 15, 100, 30);
            
            // transaction status
            XRect rect = new XRect(40,90,page.Width-80, 0);
            XFont font = new XFont("Verdana", 12, XFontStyle.Bold);
            gr.DrawString("Transaction receipt: " + order.RequestPaymentStatus, font, XBrushes.MidnightBlue, rect);

            // transaction time
            font = new XFont("Verdana", 10, XFontStyle.Regular);
            rect = new XRect(400, 90, page.Width - 80, 0);
            gr.DrawString(order.RequestPaymentDate, font, XBrushes.MidnightBlue, rect);
            
            // build footer
            XStringFormat format = new XStringFormat(); 
            format.Alignment = XStringAlignment.Near;
            font = new XFont("Verdana", 8);
            rect = new XRect(10, page.Height - 20, page.Width - 20, 0);
            gr.DrawString(getCompAddress(), font, XBrushes.MidnightBlue, rect, format);
        }






        private void DrawBody(ref XGraphics gr, Order ord, PdfPage page)
        {       
            XFont regularFont = new XFont("Times New Roman", 8, XFontStyle.Regular);                   
            XTextFormatter frmt = new XTextFormatter(gr);

            // print trans details
            gr.DrawLine(XPens.MidnightBlue, 40, 100, page.Width - 40, 100);
            XRect rect = new XRect(40, 100, page.Width-80, 100);
            gr.DrawRectangle(XBrushes.Beige, rect);
            frmt.DrawString(GetTransDetails(ord), regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);


            // print trans details
            rect = new XRect(40, 205, page.Width-80, 100);
            gr.DrawRectangle(XBrushes.Beige, rect);
            frmt.DrawString(GetCustDetails(ord), regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);

            // print order details
            rect = new XRect(40, 310, page.Width-80, 210);
            gr.DrawRectangle(XBrushes.Beige, rect);
            frmt.DrawString(GetOrderDetails(ord), regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);


            // two division lines
            gr.DrawLine(XPens.MidnightBlue, 40, 521, page.Width-40, 521);
            gr.DrawLine(XPens.MidnightBlue, 40, 535, page.Width - 40, 535);
           
            // bottom section text
            rect = new XRect(40, 536, page.Width - 80, 120);
            gr.DrawRectangle(XBrushes.Beige, rect);
            frmt.DrawString(getCompAddress(), regularFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            
            //bottom line
            gr.DrawLine(XPens.MidnightBlue, 40, 657, page.Width - 40, 657);

        }

       private string GetOrderDetails(Order ord)
       {
           StringBuilder bld = new StringBuilder();
           bld.Append("\n\r");
           bld.Append("    Order Details\n\r");
           bld.Append("    Order Number: " + ord.Id + "\n\r");
           bld.Append("    Items Purchased: " + ord.RequestListOfProducts + "\n\r");
           bld.Append("    Order Totals\n\r");
           bld.Append("    ____________________________________________\n\r");
           bld.Append("    Sub Total: " + ord.SubTotal  + "\n\r");
           bld.Append("    Shipping Costs: " + ord.ShippingCost  + "\n\r");
           bld.Append("    ____________________________________________\n\r");
           bld.Append("    Total: " + ord.Total + "\n\r");
           return bld.ToString();
       }

       private string GetCustDetails(Order ord)
       {
            StringBuilder bld = new StringBuilder();
            bld.Append("\n\r");
         	bld.Append("    Customer Details\n\r");
            bld.Append("    Name: " + ord.ShiptoName + "\n\r");
            bld.Append("    Email:" + ord.CspPayerEmail + "\n\r");
            bld.Append("    Address: " + ord.CspShippingAddress + "\n\r");

            return bld.ToString();
}



        private string getCompAddress()
        {
            StringBuilder bld = new StringBuilder();
            bld.Append("\n\r");
            bld.Append("    Stesha Pty Ltd\n\r");
            bld.Append("    2/26 Frances Street\n\r");
            bld.Append("    Lidcombe 2141\n\r");
            bld.Append("    NSW, Australia\n\r");
            bld.Append("    (+61)468627710\n\r");
            bld.Append("    ABN (19 731 703 340)");
            return bld.ToString();               
        }



        private string GetTransDetails(Order ord)
        {
            StringBuilder bld = new StringBuilder();
            bld.Append("\n\r");
            bld.Append("    Purchase Amount: AUD" + ord.Total + "\n\r");
            bld.Append("    Payment Status: " + ord.RequestPaymentStatus + "\n\r");
            bld.Append("    Paid To: Stesha Pty Ltd\n\r");
            bld.Append("    Transaction Number: " + ord.RequestTxnId + "\n\r");
           
            return bld.ToString();
        }














    }
}
