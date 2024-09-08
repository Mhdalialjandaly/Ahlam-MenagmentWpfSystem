using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using EyeClinic.Core;
using EyeClinic.Model;
using EyeClinic.WPF.Components.Management;

namespace EyeClinic.WPF.AppServices.Print
{
    public partial class PrintService : IPrintService
    {
        public List<Printer> GetAllPrinters() {
            var printers = new List<Printer>();

            foreach (string printer in PrinterSettings.InstalledPrinters) {
                printers.Add(new Printer {
                    Name = printer
                });
            }
            return printers;
        }

        public void PrintPrescription(PatientDto patient, List<PatientVisitPrescriptionDto> prescription) {
            var document = new PrintDocument {
                PrintController = new StandardPrintController()
            };

            document.PrintPage += (_, args) =>
                ProvidePrescriptionContent(patient, prescription, args.Graphics);
            document.Print();
        }

        public void PrintPrescription(PatientDto patient, List<OldMedicineViewTableDto> prescription) {
            var document = new PrintDocument {
                PrintController = new StandardPrintController()
            };

            document.PrintPage += (_, args) =>
                ProvidePrescriptionContent(patient, prescription, args.Graphics);
            document.Print();
        }

        private void ProvidePrescriptionContent(PatientDto patient, List<PatientVisitPrescriptionDto> prescription,
                Graphics drawer) {
            int offsetX = Padding;
            int offsetY = Padding;

            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            offsetY += LineHeight * 9;

            WritePatientHeaderInfo(drawer, patient, ref offsetX, ref offsetY);

            offsetY += LineHeight;

            int index = 1;
            foreach (var item in prescription) {
                drawer.DrawString(index + " - " + item.Medicine.MedicineName, NormalFont, BlackBrush, offsetX, offsetY);
                drawer.DrawString(item.Medicine.MedicineType?.EnName ?? "", NormalFont, BlackBrush, new Rectangle(new Point(offsetX, offsetY), new Size(contentWidth, BoldFontHeight)), FarText);
                offsetY += LineHeight + 8;

                drawer.DrawString(item.MedicineUsage.UsageName, NormalSmallFont, BlackBrush, offsetX + 26, offsetY);
                offsetY = offsetY + NormalFontHeight + LineHeight;
                index++;
            }
        }

        private void ProvidePrescriptionContent(PatientDto patient, List<OldMedicineViewTableDto> prescription,
            Graphics drawer) {
            int offsetX = Padding;
            int offsetY = Padding;

            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            offsetY += LineHeight * 9;

            WritePatientHeaderInfo(drawer, patient, ref offsetX, ref offsetY);

            offsetY += LineHeight;

            int index = 1;
            foreach (var item in prescription) {
                if (!string.IsNullOrWhiteSpace(item.MedicineType)) {
                    drawer.DrawString(index + " - " + item.MedicineName, NormalFont, BlackBrush, offsetX, offsetY);
                    drawer.DrawString(item.MedicineType ?? "", NormalFont, BlackBrush, new Rectangle(new Point(offsetX, offsetY), new Size(contentWidth, BoldFontHeight)), FarText);
                    offsetY += LineHeight + 10;
                    index++;
                } else {
                    drawer.DrawString(item.MedicineName, NormalSmallFont, BlackBrush, offsetX + 26, offsetY);
                    offsetY += LineHeight + 8;
                }
            }
        }

        public void PrintGlass(PatientDto patient, PatientVisitGlassDto glass) {
            var document = new PrintDocument {
                PrintController = new StandardPrintController()
            };

            document.PrintPage += (_, args) =>
                ProvideGlassContent(patient, glass, args.Graphics);
            document.Print();
        }

        private void ProvideGlassContent(PatientDto patient, PatientVisitGlassDto glass, Graphics drawer) {
            int offsetX = Padding;
            int offsetY = Padding;

            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            offsetY += LineHeight * 10;

            WritePatientHeaderInfo(drawer, patient, ref offsetX, ref offsetY);

            offsetX += 4;
            drawer.DrawString("Right", NormalFont, BlackBrush, new PointF(offsetX, offsetY + 97));
            drawer.DrawString("Left", NormalFont, BlackBrush, new PointF(offsetX, offsetY + 125));
            offsetX += 60;

            var headers = new[] { "Sph", "Cyl", "Axis", "Prism", "Base", "IPD" };
            var rightValues = new[] { glass.RSph, glass.RCyl, glass.RAxis, glass.RPrism, glass.RBase, glass.RIpd };
            var leftValues = new[] { glass.LSph, glass.LCyl, glass.LAxis, glass.LPrism, glass.LBase, glass.LIpd };

            offsetY += LineHeight * 4;
            drawer.DrawRectangle(BlackPen, new Rectangle(offsetX, offsetY, 420, 70));
            drawer.DrawLine(BlackTinPen, offsetX, offsetY + 35, offsetX + 420, offsetY + 35);

            drawer.DrawString(headers[0], SmallFont, BlackBrush, new PointF(offsetX + 20, offsetY - 25));
            drawer.DrawString(rightValues[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 10));
            drawer.DrawString(leftValues[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 43));
            for (int i = 1; i <= 5; i++) {
                var pointX = offsetX + i * 70;
                int rightValuePadding = (rightValues[i] ?? "0").Length > 3 ? 11 : 15;
                int leftValuePadding = (leftValues[i] ?? "0").Length > 3 ? 11 : 15;

                drawer.DrawString(headers[i], SmallFont, BlackBrush, new PointF(pointX + 12, offsetY - 25));
                drawer.DrawString(rightValues[i], SmallFont, BlackBrush, new PointF(pointX + rightValuePadding, offsetY + 10));
                drawer.DrawString(leftValues[i], SmallFont, BlackBrush, new PointF(pointX + leftValuePadding, offsetY + 43));

                drawer.DrawLine(BlackTinPen, pointX, offsetY, pointX, offsetY + 70);
            }

            offsetY += LineHeight * 2;

            if (glass.AddVision) {
                offsetY += LineHeight * 2;
                drawer.DrawString("Add for near vision        أضف للرؤية القريبة", NormalFont, RedBrush, new PointF(offsetX + 10, offsetY));
            }

            var rightValues2 = new[] { glass.RSph2, glass.RCyl2, glass.RAxis2, glass.RPrism2, glass.RBase2, glass.RIpd2 };
            var leftValues2 = new[] { glass.LSph2, glass.LCyl2, glass.LAxis2, glass.LPrism2, glass.LBase2, glass.LIpd2 };

            var secondTableIsNotEmpty = rightValues2.Any(e => !string.IsNullOrWhiteSpace(e));
            if (secondTableIsNotEmpty == false)
                secondTableIsNotEmpty = leftValues2.Any(e => !string.IsNullOrWhiteSpace(e));

            offsetY += LineHeight * 2;

            if (secondTableIsNotEmpty) {
                drawer.DrawString("Right", NormalFont, BlackBrush, new PointF(16, 313 + (glass.AddVision ? 225 : 181)));
                drawer.DrawString("Left", NormalFont, BlackBrush, new PointF(16, 313 + (glass.AddVision ? 256 : 212)));

                drawer.DrawRectangle(BlackPen, new Rectangle(offsetX, offsetY, 420, 70));
                drawer.DrawLine(BlackTinPen, offsetX, offsetY + 35, offsetX + 420, offsetY + 35);

                drawer.DrawString(rightValues2[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 10));
                drawer.DrawString(leftValues2[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 43));
                for (int i = 1; i <= 5; i++) {
                    var pointX = offsetX + i * 70;
                    int rightValuePadding = (rightValues[i] ?? "0").Length > 3 ? 11 : 15;
                    int leftValuePadding = (leftValues[i] ?? "0").Length > 3 ? 11 : 15;

                    drawer.DrawString(rightValues2[i], SmallFont, BlackBrush, new PointF(pointX + rightValuePadding, offsetY + 10));
                    drawer.DrawString(leftValues2[i], SmallFont, BlackBrush, new PointF(pointX + leftValuePadding, offsetY + 43));

                    drawer.DrawLine(BlackTinPen, pointX, offsetY, pointX, offsetY + 70);
                }
                offsetY += LineHeight * 4;
            }

            drawer.DrawString("ملاحظات", NormalFont, BlackBrush, new Rectangle(new Point(offsetX - 100, offsetY), new Size(contentWidth, LineHeight)), FarText);
            offsetY += LineHeight;

            drawer.DrawString(glass.Notes, NormalFont, RedBrush, new Point(offsetX, offsetY));
            if (glass.ContactLenses)
            {

                //drawer.DrawString("عدسات لاصقة ", NormalFont, BlackBrush, new Rectangle(new Point(offsetX - 150, offsetY), new Size(contentWidth, LineHeight)), FarText);
                //offsetX += LineHeight;

                drawer.DrawString("عدسات لاصقة ", NormalFont, BlueBrush, new Point(offsetX - 50, offsetY-25));
            }
        }

        public void PrintGlass(PatientDto patient, PatientGlassDto glass) {
            var document = new PrintDocument() {
                PrintController = new StandardPrintController()
            };
            document.PrintPage += (_, args) =>
                ProvideGlassContent(patient, glass, args.Graphics);
            document.Print();
        }

        private void ProvideGlassContent(PatientDto patient, PatientGlassDto glass, Graphics drawer) {
            int offsetX = Padding;
            int offsetY = Padding;

            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            offsetY += LineHeight * 10;

            WritePatientHeaderInfo(drawer, patient, ref offsetX, ref offsetY);

            offsetX += 4;
            drawer.DrawString("Right", NormalFont, BlackBrush, new PointF(offsetX, offsetY + 97));
            drawer.DrawString("Right", NormalFont, BlackBrush, new PointF(offsetX, offsetY + (glass.AddVision ? 225 : 181)));
            drawer.DrawString("Left", NormalFont, BlackBrush, new PointF(offsetX, offsetY + 125));
            drawer.DrawString("Left", NormalFont, BlackBrush, new PointF(offsetX, offsetY + (glass.AddVision ? 256 : 212)));
            offsetX += 60;

            var headers = new[] { "Sph", "Cyl", "Axis", "Prism", "Base", "IPD" };
            var rightValues = new[] { glass.RSph, glass.RCyl, glass.RAxis, glass.RPrism, glass.RBase, glass.RIpd };
            var leftValues = new[] { glass.LSph, glass.LCyl, glass.LAxis, glass.LPrism, glass.LBase, glass.LIpd };

            offsetY += LineHeight * 4;
            drawer.DrawRectangle(BlackPen, new Rectangle(offsetX, offsetY, 420, 70));
            drawer.DrawLine(BlackTinPen, offsetX, offsetY + 35, offsetX + 420, offsetY + 35);

            drawer.DrawString(headers[0], SmallFont, BlackBrush, new PointF(offsetX + 20, offsetY - 25));
            drawer.DrawString(rightValues[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 10));
            drawer.DrawString(leftValues[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 43));
            for (int i = 1; i <= 5; i++) {
                var pointX = offsetX + i * 70;
                int rightValuePadding = (rightValues[i] ?? "0").Length > 3 ? 11 : 15;
                int leftValuePadding = (leftValues[i] ?? "0").Length > 3 ? 11 : 15;

                drawer.DrawString(headers[i], SmallFont, BlackBrush, new PointF(pointX + 12, offsetY - 25));
                drawer.DrawString(rightValues[i], SmallFont, BlackBrush, new PointF(pointX + rightValuePadding, offsetY + 10));
                drawer.DrawString(leftValues[i], SmallFont, BlackBrush, new PointF(pointX + leftValuePadding, offsetY + 43));

                drawer.DrawLine(BlackTinPen, pointX, offsetY, pointX, offsetY + 70);
            }

            offsetY += LineHeight * 2;

            if (glass.AddVision) {
                offsetY += LineHeight * 2;
                drawer.DrawString("Add for near vision", NormalFont, RedBrush, new PointF(offsetX + 10, offsetY));
            }

            var rightValues2 = new[] { glass.RSph2, glass.RCyl2, glass.RAxis2, glass.RPrism2, glass.RBase2, glass.RIpd2 };
            var leftValues2 = new[] { glass.LSph2, glass.LCyl2, glass.LAxis2, glass.LPrism2, glass.LBase2, glass.LIpd2 };

            offsetY += LineHeight * 2;
            drawer.DrawRectangle(BlackPen, new Rectangle(offsetX, offsetY, 420, 70));
            drawer.DrawLine(BlackTinPen, offsetX, offsetY + 35, offsetX + 420, offsetY + 35);

            drawer.DrawString(rightValues2[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 10));
            drawer.DrawString(leftValues2[0], SmallFont, BlackBrush, new PointF(offsetX + 11, offsetY + 43));
            for (int i = 1; i <= 5; i++) {
                var pointX = offsetX + i * 70;
                int rightValuePadding = (rightValues[i] ?? "0").Length > 3 ? 11 : 15;
                int leftValuePadding = (leftValues[i] ?? "0").Length > 3 ? 11 : 15;

                drawer.DrawString(rightValues2[i], SmallFont, BlackBrush, new PointF(pointX + rightValuePadding, offsetY + 10));
                drawer.DrawString(leftValues2[i], SmallFont, BlackBrush, new PointF(pointX + leftValuePadding, offsetY + 43));

                drawer.DrawLine(BlackTinPen, pointX, offsetY, pointX, offsetY + 70);
            }

            offsetY += LineHeight * 4;
            drawer.DrawString("ملاحظات", NormalFont, BlackBrush, new Rectangle(new Point(offsetX - 100, offsetY), new Size(contentWidth, LineHeight)), FarText);
            offsetY += LineHeight;

            drawer.DrawString(glass.Notes, NormalFont, RedBrush, new Point(offsetX, offsetY));
        }

        public void PrintNote(PatientDto patient, string note) {
            var document = new PrintDocument {
                PrintController = new StandardPrintController()
            };

            document.PrintPage += (_, args) =>
                ProvideNoteContent(patient, note, args.Graphics);
            document.Print();
        }

        private void ProvideNoteContent(PatientDto patient, string note, Graphics drawer) {
            int offsetX = Padding;
            int offsetY = Padding;

            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            offsetY += LineHeight * 9;

            WritePatientHeaderInfo(drawer, patient, ref offsetX, ref offsetY);

            offsetY += LineHeight;

            WriteFarLine(drawer, note, ref offsetX, ref offsetY);
        }

        public void PrintLabTests(PatientDto patient, List<PatientVisitLabTestDto> labTests) {
            var document = new PrintDocument {
                PrintController = new StandardPrintController()
            };

            document.PrintPage += (_, args) =>
                ProvideLabTestContent(patient, labTests, args.Graphics);
            document.Print();
        }

        private void ProvideLabTestContent(PatientDto patient,
                    List<PatientVisitLabTestDto> labTests, Graphics drawer) {
            int offsetX = Padding;
            int offsetY = Padding;

            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            offsetY += LineHeight * 10;

            WritePatientHeaderInfo(drawer, patient, ref offsetX, ref offsetY);

            offsetY += LineHeight * 2;

            int index = 1;
            foreach (var item in labTests) {
                drawer.DrawString(index + " - " + item.LabTest.LabTestName, NormalFont, BlackBrush, offsetX, offsetY);
                offsetY += LineHeight + 8;
                index++;
            }
        }

        public void PrintTests(PatientDto patient, List<PatientVisitsTestDto> tests) {
            var document = new PrintDocument {
                PrintController = new StandardPrintController()
            };

            document.PrintPage += (_, args) =>
                ProvideTestContent(patient, tests, args.Graphics);
            document.Print();
        }

        private void ProvideTestContent(PatientDto patient,
            List<PatientVisitsTestDto> tests, Graphics drawer) {
            int offsetX = Padding;
            int offsetY = Padding;

            int maxX = ReceiptWidth - Padding;
            int contentWidth = maxX - Padding;

            offsetY += LineHeight * 10;

            WritePatientHeaderInfo(drawer, patient, ref offsetX, ref offsetY);

            offsetY += LineHeight * 2;

            int index = 1;
            foreach (var item in tests) {
                drawer.DrawString($"{index} - {item.Test.EnName}", NormalFont, BlackBrush, offsetX, offsetY);
                offsetY += LineHeight + 8;
                if (item.RightEye) {
                    drawer.DrawString(/*"RE: "*/ /*+*/ item.RightEyeNote, NormalFont, BlackBrush, offsetX, offsetY);
                    int rightEyeResultHeight = (int)drawer.MeasureString(item.RightEyeNote, NormalFont, contentWidth - 50, StringFormat.GenericTypographic).Height;
                    offsetY += LineHeight + rightEyeResultHeight;
                }
                if (item.LeftEye) {
                    drawer.DrawString(/*"LE: "*/ /*+*/ item.LeftEyeNote, NormalFont, BlackBrush, offsetX, offsetY);
                    int leftEyeResultHeight = (int)drawer.MeasureString(item.LeftEyeNote, NormalFont, contentWidth - 50, StringFormat.GenericTypographic).Height;
                    offsetY += LineHeight + leftEyeResultHeight;
                }
                index++;
            }
        }
    }
}
