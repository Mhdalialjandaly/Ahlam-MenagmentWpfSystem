using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using EyeClinic.WPF.Components.Management;

namespace EyeClinic.WPF.AppServices.Print
{
    public class PrintServiceTemp
    {
        private int ReceiptWidth = 290;
        private int Padding = 10;
        private int LineHeight = 10;
        private string FontFamily = "Consolas";

        private int WindowsFontSize = 8;
        private int WindowsLargeFontSize = 10;

        public void Sample() {
            var document = new PrintDocument();
            document.PrintController = new StandardPrintController();
            document.PrintPage += ProvideContent;
            document.Print();
        }

        private void ProvideContent(object sender, PrintPageEventArgs e) {
            var g = e.Graphics;

            var brush = new SolidBrush(Color.Black);
            Pen blackPen = new Pen(Color.Black, 1);
            Pen whitePen = new Pen(Color.White, 1);
            Font normalFont = null;
            Font boldFont = null;
            Font titleFont = null;

            string additionalSettings = "Windows";
            if (additionalSettings == "Windows") {
                normalFont = new Font(FontFamily, WindowsFontSize);
                boldFont = new Font(FontFamily, WindowsFontSize, System.Drawing.FontStyle.Bold);
                titleFont = new Font(FontFamily, WindowsFontSize + 4, System.Drawing.FontStyle.Bold);
            } else if (additionalSettings == "Windows Large") {
                normalFont = new Font(FontFamily, WindowsLargeFontSize);
                boldFont = new Font(FontFamily, WindowsLargeFontSize, System.Drawing.FontStyle.Bold);
                titleFont = new Font(FontFamily, WindowsLargeFontSize + 4, System.Drawing.FontStyle.Bold);
            } else {
                normalFont = new Font(FontFamily, WindowsFontSize);
                boldFont = new Font(FontFamily, WindowsFontSize, System.Drawing.FontStyle.Bold);
                titleFont = new Font(FontFamily, WindowsFontSize + 4, System.Drawing.FontStyle.Bold);
            }

            int normalFontHeight = normalFont.Height;
            int boldFontHeight = boldFont.Height;


            StringFormat center_sf = new StringFormat();
            center_sf.LineAlignment = StringAlignment.Center;
            center_sf.Alignment = StringAlignment.Center;

            StringFormat far_sf = new StringFormat();
            far_sf.LineAlignment = StringAlignment.Far;
            far_sf.Alignment = StringAlignment.Far;

            StringFormat near_sf = new StringFormat();
            near_sf.LineAlignment = StringAlignment.Near;
            near_sf.Alignment = StringAlignment.Near;
            near_sf.Trimming = StringTrimming.None;

            int offset_x = Padding;
            int offset_y = Padding;

            int max_x = ReceiptWidth - Padding;
            int contentwidth = max_x - Padding;

            var second_3column_offset_x = contentwidth / 3;
            var third_3column_offset_x = second_3column_offset_x + contentwidth / 3;

            // Print Logo
            bool printLogo = false;
            if (printLogo) {
                System.Drawing.Image img = ConvertBase64Image("");
                var loc = new Point((contentwidth) / 2 - 36, offset_y);
                g.DrawImage(img, loc);
                offset_y = offset_y + 72 + LineHeight;
            }


            int rest_height = (int)g.MeasureString("DR. Mohammad AlChujaa", titleFont, contentwidth - 50, StringFormat.GenericTypographic).Height;
            g.DrawString("DR. Mohammad AlChujaa", titleFont, brush, new System.Drawing.Rectangle(new System.Drawing.Point(offset_x, offset_y), new System.Drawing.Size(contentwidth, rest_height)), center_sf);
            offset_y = offset_y + rest_height + LineHeight;

            g.DrawString(DateTime.Now.ToShortDateString(), normalFont, brush, offset_x, offset_y);
            g.DrawString(DateTime.Now.ToShortTimeString(), normalFont, brush, new System.Drawing.Rectangle(new System.Drawing.Point(offset_x, offset_y), new System.Drawing.Size(contentwidth, boldFontHeight)), far_sf);
            offset_y = offset_y + normalFontHeight + LineHeight;

            //Line
            g.DrawLine(blackPen, offset_x, offset_y, max_x, offset_y);
            offset_y = offset_y + LineHeight;


            g.DrawString("Count Down List", normalFont, brush, new System.Drawing.Rectangle(new System.Drawing.Point(offset_x, offset_y), new System.Drawing.Size(contentwidth, boldFontHeight)), center_sf);
            offset_y = offset_y + LineHeight + 10;

            g.DrawString("Item", normalFont, brush, offset_x, offset_y);
            g.DrawString("Available Qty", normalFont, brush, new System.Drawing.Rectangle(new System.Drawing.Point(offset_x, offset_y), new System.Drawing.Size(contentwidth, boldFontHeight)), far_sf);
            offset_y = offset_y + normalFontHeight + LineHeight;

            //Line
            g.DrawLine(blackPen, offset_x, offset_y, max_x, offset_y);
            offset_y = offset_y + LineHeight;

            var menuItems = new List<TestItem> {
                new (){Name = "Item 1", CountDown = 10},
                new (){Name = "Item 2", CountDown = 12},
                new (){Name = "Item 3", CountDown = 41},
                new (){Name = "Item 4", CountDown = 64},
            };

            foreach (var item in menuItems) {
                g.DrawString(item.Name, normalFont, brush, offset_x, offset_y);
                g.DrawString(item.CountDown.ToString(), normalFont, brush, new System.Drawing.Rectangle(new System.Drawing.Point(offset_x, offset_y), new System.Drawing.Size(contentwidth, boldFontHeight)), far_sf);
                offset_y = offset_y + normalFontHeight + LineHeight;
            }
        }

        public Image ConvertBase64Image(string base64) {
            byte[] bytes = Convert.FromBase64String(base64);

            Image image;
            using MemoryStream ms = new MemoryStream(bytes);
            image = Image.FromStream(ms);

            return image;
        }

        public List<Printer> GetAllPrinters() {
            var printers = new List<Printer>();

            foreach (string printer in PrinterSettings.InstalledPrinters) {
                printers.Add(new Printer {
                    Name = printer
                });
            }
            return printers;
        }
    }

    public class TestItem
    {
        public string Name { get; set; }
        public int CountDown { get; set; }
    }
}
