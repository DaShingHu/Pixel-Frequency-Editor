namespace Pixel_Frequecny_Editor
{
    partial class frmMain
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
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnMod = new System.Windows.Forms.Button();
            this.lstInputImages = new System.Windows.Forms.ListView();
            this.lstOutput = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radRed = new System.Windows.Forms.RadioButton();
            this.radHue = new System.Windows.Forms.RadioButton();
            this.radSat = new System.Windows.Forms.RadioButton();
            this.radValue = new System.Windows.Forms.RadioButton();
            this.radBlue = new System.Windows.Forms.RadioButton();
            this.radGreen = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFile
            // 
            this.openFile.FileName = "openFileDialog1";
            this.openFile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFile_FileOk);
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(268, 12);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(169, 23);
            this.btnInput.TabIndex = 2;
            this.btnInput.Text = "Open file";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnMod
            // 
            this.btnMod.Location = new System.Drawing.Point(268, 41);
            this.btnMod.Name = "btnMod";
            this.btnMod.Size = new System.Drawing.Size(169, 23);
            this.btnMod.TabIndex = 3;
            this.btnMod.Text = "Modify";
            this.btnMod.UseVisualStyleBackColor = true;
            this.btnMod.Click += new System.EventHandler(this.btnMod_Click);
            // 
            // lstInputImages
            // 
            this.lstInputImages.Location = new System.Drawing.Point(12, 12);
            this.lstInputImages.Name = "lstInputImages";
            this.lstInputImages.Size = new System.Drawing.Size(250, 198);
            this.lstInputImages.TabIndex = 4;
            this.lstInputImages.UseCompatibleStateImageBehavior = false;
            // 
            // lstOutput
            // 
            this.lstOutput.Location = new System.Drawing.Point(443, 12);
            this.lstOutput.Name = "lstOutput";
            this.lstOutput.Size = new System.Drawing.Size(250, 198);
            this.lstOutput.TabIndex = 5;
            this.lstOutput.UseCompatibleStateImageBehavior = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.radGreen);
            this.panel1.Controls.Add(this.radBlue);
            this.panel1.Controls.Add(this.radRed);
            this.panel1.Controls.Add(this.radHue);
            this.panel1.Controls.Add(this.radSat);
            this.panel1.Controls.Add(this.radValue);
            this.panel1.Location = new System.Drawing.Point(271, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(168, 145);
            this.panel1.TabIndex = 6;
            // 
            // radRed
            // 
            this.radRed.AutoSize = true;
            this.radRed.Location = new System.Drawing.Point(4, 76);
            this.radRed.Name = "radRed";
            this.radRed.Size = new System.Drawing.Size(45, 17);
            this.radRed.TabIndex = 3;
            this.radRed.TabStop = true;
            this.radRed.Text = "Red";
            this.radRed.UseVisualStyleBackColor = true;
            // 
            // radHue
            // 
            this.radHue.AutoSize = true;
            this.radHue.Location = new System.Drawing.Point(4, 52);
            this.radHue.Name = "radHue";
            this.radHue.Size = new System.Drawing.Size(45, 17);
            this.radHue.TabIndex = 2;
            this.radHue.TabStop = true;
            this.radHue.Text = "Hue";
            this.radHue.UseVisualStyleBackColor = true;
            this.radHue.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radSat
            // 
            this.radSat.AutoSize = true;
            this.radSat.Location = new System.Drawing.Point(4, 28);
            this.radSat.Name = "radSat";
            this.radSat.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radSat.Size = new System.Drawing.Size(73, 17);
            this.radSat.TabIndex = 1;
            this.radSat.TabStop = true;
            this.radSat.Text = "Saturation";
            this.radSat.UseVisualStyleBackColor = true;
            // 
            // radValue
            // 
            this.radValue.AutoSize = true;
            this.radValue.Checked = true;
            this.radValue.Location = new System.Drawing.Point(4, 4);
            this.radValue.Name = "radValue";
            this.radValue.Size = new System.Drawing.Size(52, 17);
            this.radValue.TabIndex = 0;
            this.radValue.TabStop = true;
            this.radValue.Text = "Value";
            this.radValue.UseVisualStyleBackColor = true;
            // 
            // radBlue
            // 
            this.radBlue.AutoSize = true;
            this.radBlue.Location = new System.Drawing.Point(4, 97);
            this.radBlue.Name = "radBlue";
            this.radBlue.Size = new System.Drawing.Size(46, 17);
            this.radBlue.TabIndex = 4;
            this.radBlue.TabStop = true;
            this.radBlue.Text = "Blue";
            this.radBlue.UseVisualStyleBackColor = true;
            // 
            // radGreen
            // 
            this.radGreen.AutoSize = true;
            this.radGreen.Location = new System.Drawing.Point(3, 120);
            this.radGreen.Name = "radGreen";
            this.radGreen.Size = new System.Drawing.Size(54, 17);
            this.radGreen.TabIndex = 5;
            this.radGreen.TabStop = true;
            this.radGreen.Text = "Green";
            this.radGreen.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 227);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstOutput);
            this.Controls.Add(this.lstInputImages);
            this.Controls.Add(this.btnMod);
            this.Controls.Add(this.btnInput);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.Button btnMod;
        private System.Windows.Forms.ListView lstInputImages;
        private System.Windows.Forms.ListView lstOutput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radRed;
        private System.Windows.Forms.RadioButton radHue;
        private System.Windows.Forms.RadioButton radSat;
        private System.Windows.Forms.RadioButton radValue;
        private System.Windows.Forms.RadioButton radGreen;
        private System.Windows.Forms.RadioButton radBlue;
    }
}

