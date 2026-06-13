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
            this.btnOpenCoord = new System.Windows.Forms.Button();
            this.btnBulkImport = new System.Windows.Forms.Button();
            this.btnManualImport = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.Status = new System.Windows.Forms.Label();
            this.lblCoordPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOpenCoord
            // 
            this.btnOpenCoord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenCoord.Location = new System.Drawing.Point(12, 12);
            this.btnOpenCoord.Name = "btnOpenCoord";
            this.btnOpenCoord.Size = new System.Drawing.Size(376, 30);
            this.btnOpenCoord.TabIndex = 0;
            this.btnOpenCoord.Text = "Open coord_data.txt";
            this.btnOpenCoord.UseVisualStyleBackColor = true;
            this.btnOpenCoord.Click += new System.EventHandler(this.btnOpenCoord_Click);
            // 
            // lblCoordPath
            // 
            this.lblCoordPath.AutoSize = true;
            this.lblCoordPath.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCoordPath.ForeColor = System.Drawing.Color.Gray;
            this.lblCoordPath.Location = new System.Drawing.Point(12, 45);
            this.lblCoordPath.Name = "lblCoordPath";
            this.lblCoordPath.Size = new System.Drawing.Size(100, 13);
            this.lblCoordPath.TabIndex = 5;
            this.lblCoordPath.Text = "No file selected";
            // 
            // btnBulkImport
            // 
            this.btnBulkImport.Enabled = true;
            this.btnBulkImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBulkImport.Location = new System.Drawing.Point(12, 63);
            this.btnBulkImport.Name = "btnBulkImport";
            this.btnBulkImport.Size = new System.Drawing.Size(184, 30);
            this.btnBulkImport.TabIndex = 1;
            this.btnBulkImport.Text = "Bulk Import (Select Folder)";
            this.btnBulkImport.UseVisualStyleBackColor = true;
            this.btnBulkImport.Click += new System.EventHandler(this.btnBulkImport_Click);
            // 
            // btnManualImport
            // 
            this.btnManualImport.Enabled = false;
            this.btnManualImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualImport.Location = new System.Drawing.Point(204, 63);
            this.btnManualImport.Name = "btnManualImport";
            this.btnManualImport.Size = new System.Drawing.Size(184, 30);
            this.btnManualImport.TabIndex = 2;
            this.btnManualImport.Text = "Manual Import (Select Files)";
            this.btnManualImport.UseVisualStyleBackColor = true;
            this.btnManualImport.Click += new System.EventHandler(this.btnManualImport_Click);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Status.Location = new System.Drawing.Point(12, 100);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(38, 15);
            this.Status.TabIndex = 3;
            this.Status.Text = "Ready";
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(12, 120);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(376, 23);
            this.Progress.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 155);
            this.Controls.Add(this.btnOpenCoord);
            this.Controls.Add(this.lblCoordPath);
            this.Controls.Add(this.btnBulkImport);
            this.Controls.Add(this.btnManualImport);
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

        private System.Windows.Forms.Button btnOpenCoord;
        private System.Windows.Forms.Button btnBulkImport;
        private System.Windows.Forms.Button btnManualImport;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Label lblCoordPath;
    }
}
