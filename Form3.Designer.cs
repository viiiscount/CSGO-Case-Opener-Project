
namespace Project
{
    partial class FrmSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearch));
            this.TxtSearch = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.ListResults = new System.Windows.Forms.ListBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.BtnGo = new System.Windows.Forms.Button();
            this.lblFilesLoaded = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TxtSearch
            // 
            this.TxtSearch.Location = new System.Drawing.Point(16, 32);
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.Size = new System.Drawing.Size(359, 20);
            this.TxtSearch.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(152, 20);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Enter Search Value:";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(13, 418);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(443, 39);
            this.lblInfo.TabIndex = 7;
            this.lblInfo.Text = resources.GetString("lblInfo.Text");
            // 
            // ListResults
            // 
            this.ListResults.FormattingEnabled = true;
            this.ListResults.Location = new System.Drawing.Point(15, 64);
            this.ListResults.Name = "ListResults";
            this.ListResults.Size = new System.Drawing.Size(441, 342);
            this.ListResults.TabIndex = 8;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(381, 30);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 23);
            this.BtnSearch.TabIndex = 10;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // BtnGo
            // 
            this.BtnGo.Location = new System.Drawing.Point(381, 383);
            this.BtnGo.Name = "BtnGo";
            this.BtnGo.Size = new System.Drawing.Size(75, 23);
            this.BtnGo.TabIndex = 11;
            this.BtnGo.Text = "Go To Case";
            this.BtnGo.UseVisualStyleBackColor = true;
            // 
            // lblFilesLoaded
            // 
            this.lblFilesLoaded.AutoSize = true;
            this.lblFilesLoaded.Location = new System.Drawing.Point(362, 9);
            this.lblFilesLoaded.Name = "lblFilesLoaded";
            this.lblFilesLoaded.Size = new System.Drawing.Size(94, 13);
            this.lblFilesLoaded.TabIndex = 12;
            this.lblFilesLoaded.Text = "Loaded X/36 Files";
            this.lblFilesLoaded.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FrmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 471);
            this.Controls.Add(this.lblFilesLoaded);
            this.Controls.Add(this.BtnGo);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.ListResults);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.TxtSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(487, 510);
            this.MinimumSize = new System.Drawing.Size(487, 510);
            this.Name = "FrmSearch";
            this.Text = "Search Skins";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtSearch;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Button BtnGo;
        private System.Windows.Forms.ListBox ListResults;
        private System.Windows.Forms.Label lblFilesLoaded;
    }
}