using System;
using System.Drawing;
using EyeClinic.Model;

namespace EyeClinic.WPF.AppServices.Print
{
    public partial class PrintService
    {
        private const int ReceiptWidth = 540;
        private const int Padding = 12;
        private const int LineHeight = 22;
        private const string FontFamily = "Times New Roman";
        private const int WindowsFontSize = 15;

        public SolidBrush BlackBrush = new(Color.Black);
        public SolidBrush BlueBrush = new(Color.Blue);
        public SolidBrush RedBrush = new(Color.Red);
        public Pen BlackPen = new(Color.Black, 2);
        public Pen BlackTinPen = new(Color.Black, float.Parse("0.2"));
        public Pen DashedPen = new Pen(Color.Gray, 2) {
            DashCap = System.Drawing.Drawing2D.DashCap.Round,
            DashPattern = new[] { 4.0F, 2.0F },
        };
        public Font SmallFont = new(FontFamily, 12);
        public Font SmallFontForName = new(FontFamily, 13);
        public Font NormalSmallFont = new(FontFamily, 14);
        public Font NormalFont = new(FontFamily, WindowsFontSize);
        public Font BoldFont = new(FontFamily, WindowsFontSize, FontStyle.Bold);
        public Font TitleFont = new(FontFamily, WindowsFontSize + 4, FontStyle.Bold);

        public int NormalFontHeight => NormalFont.Height;
        public int BoldFontHeight => BoldFont.Height;
        public int TitleFontHeight => TitleFont.Height;

        public StringFormat CenterText = new() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
        public StringFormat NearText = new() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near, Trimming = StringTrimming.None };
        public StringFormat FarText = new() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Far };


        public void WriteFarLine(Graphics drawer, string text, ref int offsetX, ref int offsetY) {
            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            int patientNameHeight = (int)drawer.MeasureString(text, NormalFont, contentWidth - 50, StringFormat.GenericTypographic).Height;
            drawer.DrawString(text, NormalFont, BlackBrush, new Rectangle(new Point(offsetX, offsetY), new Size(contentWidth, patientNameHeight)), FarText);
            offsetY = offsetY + patientNameHeight + LineHeight;
        }

        public void WriteSmallFarLine(Graphics drawer, string text, ref int offsetX, ref int offsetY) {
            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            int patientNameHeight = (int)drawer.MeasureString(text, NormalFont, contentWidth - 50, StringFormat.GenericTypographic).Height;
            drawer.DrawString(text, SmallFontForName, BlackBrush, new Rectangle(new Point(offsetX, offsetY), new Size(contentWidth, patientNameHeight)), FarText);
            offsetY = offsetY + patientNameHeight + 4;
        }

        public void WritePatientName(Graphics drawer, string text, ref int offsetX, ref int offsetY) {
            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            int patientNameHeight = (int)drawer.MeasureString(text, NormalFont, contentWidth - 50, StringFormat.GenericTypographic).Height;
            drawer.DrawString("الاسم", SmallFontForName, BlackBrush, new Rectangle(new Point(offsetX, offsetY), new Size(contentWidth, patientNameHeight)), FarText);
            drawer.DrawString(" : ", SmallFontForName, BlackBrush, new Rectangle(new Point(offsetX - 40, offsetY), new Size(contentWidth, patientNameHeight)), FarText);
            drawer.DrawString(text, SmallFontForName, BlueBrush, new Rectangle(new Point(offsetX - 50, offsetY), new Size(contentWidth, patientNameHeight)), FarText);
            offsetY = offsetY + patientNameHeight + 4;
        }

        public void WriteLine(Graphics drawer, string text, ref int offsetX, ref int offsetY) {
            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            int patientNameHeight = (int)drawer.MeasureString(text, NormalFont, contentWidth - 50, StringFormat.GenericTypographic).Height;
            drawer.DrawString(text, NormalFont, BlackBrush, new Rectangle(new Point(offsetX, offsetY), new Size(contentWidth, patientNameHeight)), NearText);
            offsetY = offsetY + patientNameHeight + LineHeight;
        }

        public void WriteCenterLine(Graphics drawer, string text, ref int offsetX, ref int offsetY) {
            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            int patientNameHeight = (int)drawer.MeasureString(text, NormalFont, contentWidth - 50, StringFormat.GenericTypographic).Height;
            drawer.DrawString(text, NormalFont, BlackBrush, new Rectangle(new Point(offsetX, offsetY), new Size(contentWidth, patientNameHeight)), CenterText);
            offsetY = offsetY + patientNameHeight + LineHeight;
        }

        public void DrawHorizontalLine(Graphics drawer, ref int offsetX, ref int offsetY) {
            int maxX = ReceiptWidth - Padding;

            drawer.DrawLine(BlackPen, offsetX, offsetY, maxX, offsetY);
            offsetY += LineHeight;
        }

        public void WritePatientHeaderInfo(Graphics drawer, PatientDto patient, ref int offsetX, ref int offsetY) {
            WritePatientName(drawer, $"{ patient.FirstName+" ("+patient.FatherName+" - "+patient.MotherName+") "+patient.LastName} / { patient.Number}", ref offsetX, ref offsetY);

            WriteSmallFarLine(drawer, $"التاريخ : {DateTime.Now:dd/MM/yyyy}", ref offsetX, ref offsetY);
            //WriteSmallFarLine(drawer, $"العمر : {patient.AgeDisplayName}", ref offsetX, ref offsetY);
        }

        public void NewLine(ref int offsetY) {
            offsetY = offsetY + NormalFontHeight + LineHeight;
        }
    }
}
