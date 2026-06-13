namespace Npcgen2CoordData
{
    partial class Form1
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
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.Status = new System.Windows.Forms.Label();
            this.btnImportAndSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImportAndSave
            // 
            this.btnImportAndSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportAndSave.Location = new System.Drawing.Point(12, 12);
            this.btnImportAndSave.Name = "btnImportAndSave";
            this.btnImportAndSave.Size = new System.Drawing.Size(376, 35);
            this.btnImportAndSave.TabIndex = 0;
            this.btnImportAndSave.Text = "Import npcgen.data && Save coord_data.txt";
            this.btnImportAndSave.UseVisualStyleBackColor = true;
            this.btnImportAndSave.Click += new System.EventHandler(this.btnImportAndSave_Click);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Status.Location = new System.Drawing.Point(12, 55);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(38, 15);
            this.Status.TabIndex = 1;
            this.Status.Text = "Ready";
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(12, 75);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(376, 23);
            this.Progress.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 110);
            this.Controls.Add(this.btnImportAndSave);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Progress);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Npcgen to coord_data Importer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImportAndSave;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Label Status;
    }
}
