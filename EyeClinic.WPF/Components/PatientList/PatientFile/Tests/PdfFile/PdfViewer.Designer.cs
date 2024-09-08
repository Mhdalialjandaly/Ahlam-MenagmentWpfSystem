namespace EyeClinic.WPF.Components.PatientList.PatientFile.Tests.PdfFile
{
    partial class PdfViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PdfViewer));
            PdfViewerView = new AxAcroPDFLib.AxAcroPDF();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            CodeLabel = new System.Windows.Forms.Label();
            ProductNameLabel = new System.Windows.Forms.Label();
            axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
            ((System.ComponentModel.ISupportInitialize)PdfViewerView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)axWebBrowser1).BeginInit();
            SuspendLayout();
            // 
            // PdfViewerView
            // 
            PdfViewerView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PdfViewerView.Enabled = true;
            PdfViewerView.Location = new System.Drawing.Point(347, 82);
            PdfViewerView.Name = "PdfViewerView";
            PdfViewerView.OcxState = (System.Windows.Forms.AxHost.State)resources.GetObject("PdfViewerView.OcxState");
            PdfViewerView.Size = new System.Drawing.Size(731, 531);
            PdfViewerView.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox1.InitialImage = null;
            pictureBox1.Location = new System.Drawing.Point(12, 619);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(1066, 424);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.SystemColors.Control;
            label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(238, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(107, 38);
            label1.TabIndex = 2;
            label1.Text = "PdfFile";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.SystemColors.Control;
            label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(871, 3);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(83, 38);
            label2.TabIndex = 2;
            label2.Text = "Code";
            // 
            // CodeLabel
            // 
            CodeLabel.AutoSize = true;
            CodeLabel.BackColor = System.Drawing.SystemColors.Control;
            CodeLabel.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            CodeLabel.Location = new System.Drawing.Point(960, 9);
            CodeLabel.Name = "CodeLabel";
            CodeLabel.Size = new System.Drawing.Size(83, 38);
            CodeLabel.TabIndex = 2;
            CodeLabel.Text = "Code";
            // 
            // ProductNameLabel
            // 
            ProductNameLabel.AutoSize = true;
            ProductNameLabel.BackColor = System.Drawing.SystemColors.Control;
            ProductNameLabel.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ProductNameLabel.Location = new System.Drawing.Point(375, 9);
            ProductNameLabel.Name = "ProductNameLabel";
            ProductNameLabel.Size = new System.Drawing.Size(95, 38);
            ProductNameLabel.TabIndex = 2;
            ProductNameLabel.Text = "Name";
            // 
            // axWebBrowser1
            // 
            axWebBrowser1.Enabled = true;
            axWebBrowser1.Location = new System.Drawing.Point(12, 82);
            axWebBrowser1.OcxState = (System.Windows.Forms.AxHost.State)resources.GetObject("axWebBrowser1.OcxState");
            axWebBrowser1.Size = new System.Drawing.Size(329, 531);
            axWebBrowser1.TabIndex = 3;
            // 
            // PdfViewer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1090, 1055);
            Controls.Add(axWebBrowser1);
            Controls.Add(CodeLabel);
            Controls.Add(label2);
            Controls.Add(ProductNameLabel);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(PdfViewerView);
            Name = "PdfViewer";
            Text = "PdfViewer";
            ((System.ComponentModel.ISupportInitialize)PdfViewerView).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)axWebBrowser1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public AxAcroPDFLib.AxAcroPDF PdfViewerView;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label CodeLabel;
        public System.Windows.Forms.Label ProductNameLabel;
        public AxSHDocVw.AxWebBrowser axWebBrowser1;
    }
}