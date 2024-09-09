using AxAcroPDFLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.PDFMerger
{
    public partial class PDFMergerWindow : Form
    {
        public PDFMergerWindow()
        {
            InitializeComponent();
        }
     
        string targetPath = @"C:\Users\MajR\Desktop\", uploadedfile1name, uploadedfile2name;
        public List<string> Paths = new List<string>();
        public List<string> Paths2 = new List<string>();
        public List<string> Paths3 = new List<string>();
        public List<string> PathsPDf3 = new List<string>();
        string p1, p2, p3, r;
        private void TargetButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            // Set the initial directory (optional)

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textTarget.Text = openFileDialog.SelectedPath.ToString();
            }
        }

        private void Path1_Click(object sender, EventArgs e)
        {
            string filePath = TextPathFile1.Text;
            if (string.IsNullOrEmpty(TextPathFile1.Text))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = @"C:\Users\MajR\Desktop"; // Set the initial directory (optional)

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    TextPathFile1.Text = filePath;
                }
                uploadedfile1name = Path.GetFileName(filePath);              
                if (!string.IsNullOrEmpty(filePath))
                {
                    LastNameLbl.Text = Path.GetFileName(filePath);
                }
            }
            else
            {
                
            }
        }

        private void Path2_Click(object sender, EventArgs e)
        {

            string filePath = TextPathFile2.Text;
            if (string.IsNullOrEmpty(TextPathFile2.Text))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = @"C:\Users\MajR\Desktop"; // Set the initial directory (optional)

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    TextPathFile2.Text = filePath;
                }
                uploadedfile2name = Path.GetFileName(filePath);
            }
            else
            {
                
            }
        }

        public void MergeButton_Click(object sender, EventArgs e)
        {
            MergPDFActionOutBound();
        }

        private void MergPDFActionOutBound()
        {
            string TheResult = "";
            try
            {
                if (!string.IsNullOrEmpty(textTarget.Text))
                {
                    targetPath = @"" + textTarget.Text;
                    TheResult = targetPath + "\\" + "(" + uploadedfile2name + ")" + "-" + uploadedfile1name;
                    MergePDFs(TheResult, TextPathFile1.Text, TextPathFile2.Text);

                    
                }
                else
                {
                    TheResult = targetPath + "\\" + "(" + uploadedfile2name + ")" + "-" + uploadedfile1name;
                    MergePDFs(TheResult, TextPathFile1.Text, TextPathFile2.Text);
                  
                }
            }
            catch (Exception)
            {

                
            }
        }

        public void MergPDFAction(PatientVisitsTestDto testDto) 
        {
            string TheResult = "";
            uploadedfile2name = Path.GetFileName(testDto.Test.ImagePath2);
            uploadedfile1name = Path.GetFileName(TextPathFile1.Text);
            try
            {
                if (!string.IsNullOrEmpty(textTarget.Text))
                {
                    targetPath = @"" + textTarget.Text;
                    TheResult = targetPath + "\\" + "(" + uploadedfile2name + ")" + "-" + uploadedfile1name;
                    MergePDFs(TheResult, TextPathFile1.Text, TextPathFile2.Text);
                  
                    
                }
                else
                {
                    TheResult = targetPath + "\\" + "(" + uploadedfile2name + ")" + "-" + uploadedfile1name;
                    MergePDFs(TheResult, TextPathFile1.Text, TextPathFile2.Text);                    
                  
                    this.Close();
                }
            }
            catch (Exception)
            {
               
                this.Close();
            }
        }
        public static void MergePDFs(string targetPath, params string[] pdfPaths)
        {
            using (var targetDoc = new PdfDocument())
            {
                foreach (var pdfPath in pdfPaths)
                {
                    using (var pdfDoc = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import))
                    {
                        for (var i = 0; i < pdfDoc.PageCount; i++)
                        {
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                        }
                    }
                }
                targetDoc.Save(targetPath);
            }
        }

        private void PDFMergerWindow_Load(object sender, EventArgs e)
        {

        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextPathFile1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
