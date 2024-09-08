namespace EyeClinic.WPF.Components.PatientList.PatientFile.PDFMerger
{
    partial class PDFMergerWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TargetButton = new System.Windows.Forms.Button();
            textTarget = new System.Windows.Forms.TextBox();
            TextPathFile1 = new System.Windows.Forms.TextBox();
            TextPathFile2 = new System.Windows.Forms.TextBox();
            Path1 = new System.Windows.Forms.Button();
            Path2 = new System.Windows.Forms.Button();
            LastNameLbl = new System.Windows.Forms.Label();
            MergeButton = new System.Windows.Forms.Button();
            Close = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // TargetButton
            // 
            TargetButton.BackColor = System.Drawing.Color.Yellow;
            TargetButton.Location = new System.Drawing.Point(442, 311);
            TargetButton.Name = "TargetButton";
            TargetButton.Size = new System.Drawing.Size(217, 29);
            TargetButton.TabIndex = 1;
            TargetButton.Text = "مكان تخزين الملف المخرج";
            TargetButton.UseVisualStyleBackColor = false;
            TargetButton.Click += TargetButton_Click;
            // 
            // textTarget
            // 
            textTarget.Location = new System.Drawing.Point(442, 278);
            textTarget.Name = "textTarget";
            textTarget.Size = new System.Drawing.Size(217, 27);
            textTarget.TabIndex = 2;
            // 
            // TextPathFile1
            // 
            TextPathFile1.Location = new System.Drawing.Point(442, 360);
            TextPathFile1.Name = "TextPathFile1";
            TextPathFile1.Size = new System.Drawing.Size(217, 27);
            TextPathFile1.TabIndex = 3;
            TextPathFile1.TextChanged += TextPathFile1_TextChanged;
            // 
            // TextPathFile2
            // 
            TextPathFile2.Location = new System.Drawing.Point(205, 360);
            TextPathFile2.Name = "TextPathFile2";
            TextPathFile2.Size = new System.Drawing.Size(217, 27);
            TextPathFile2.TabIndex = 3;
            // 
            // Path1
            // 
            Path1.BackColor = System.Drawing.Color.Blue;
            Path1.ForeColor = System.Drawing.Color.White;
            Path1.Location = new System.Drawing.Point(491, 393);
            Path1.Name = "Path1";
            Path1.Size = new System.Drawing.Size(125, 29);
            Path1.TabIndex = 1;
            Path1.Text = "الملف الاول";
            Path1.UseVisualStyleBackColor = false;
            Path1.Click += Path1_Click;
            // 
            // Path2
            // 
            Path2.BackColor = System.Drawing.Color.Blue;
            Path2.ForeColor = System.Drawing.Color.White;
            Path2.Location = new System.Drawing.Point(257, 393);
            Path2.Name = "Path2";
            Path2.Size = new System.Drawing.Size(125, 29);
            Path2.TabIndex = 1;
            Path2.Text = "الملف الثاني";
            Path2.UseVisualStyleBackColor = false;
            Path2.Click += Path2_Click;
            // 
            // LastNameLbl
            // 
            LastNameLbl.AutoSize = true;
            LastNameLbl.Location = new System.Drawing.Point(205, 297);
            LastNameLbl.Name = "LastNameLbl";
            LastNameLbl.Size = new System.Drawing.Size(117, 20);
            LastNameLbl.TabIndex = 4;
            LastNameLbl.Text = "اخر ملف تم دمجه";
            // 
            // MergeButton
            // 
            MergeButton.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            MergeButton.ForeColor = System.Drawing.Color.Blue;
            MergeButton.Location = new System.Drawing.Point(371, 446);
            MergeButton.Name = "MergeButton";
            MergeButton.Size = new System.Drawing.Size(125, 29);
            MergeButton.TabIndex = 1;
            MergeButton.Text = "دمج";
            MergeButton.UseVisualStyleBackColor = false;
            MergeButton.Click += MergeButton_Click;
            // 
            // Close
            // 
            Close.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            Close.ForeColor = System.Drawing.Color.Red;
            Close.Location = new System.Drawing.Point(663, 446);
            Close.Name = "Close";
            Close.Size = new System.Drawing.Size(125, 29);
            Close.TabIndex = 1;
            Close.Text = "اغلاق";
            Close.UseVisualStyleBackColor = false;
            Close.Click += Close_Click;
            // 
            // PDFMergerWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 500);
            ControlBox = false;
            Controls.Add(LastNameLbl);
            Controls.Add(TextPathFile2);
            Controls.Add(TextPathFile1);
            Controls.Add(textTarget);
            Controls.Add(Path2);
            Controls.Add(Path1);
            Controls.Add(Close);
            Controls.Add(MergeButton);
            Controls.Add(TargetButton);
            Name = "PDFMergerWindow";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "PDFMergerWindow";
            Load += PDFMergerWindow_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF Pdfviewer;
        private System.Windows.Forms.Button TargetButton;
        private System.Windows.Forms.Button Path1;
        private System.Windows.Forms.Button Path2;
        private System.Windows.Forms.Label LastNameLbl;
        private System.Windows.Forms.Button MergeButton;
        private new System.Windows.Forms.Button Close;
        public System.Windows.Forms.TextBox textTarget;
        public System.Windows.Forms.TextBox TextPathFile1;
        public System.Windows.Forms.TextBox TextPathFile2;
    }
}