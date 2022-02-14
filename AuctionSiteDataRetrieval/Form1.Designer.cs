namespace AuctionSiteDataRetrieval
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.setupBrowser = new System.Windows.Forms.Button();
            this.lblToken = new System.Windows.Forms.Label();
            this.btnRetrieveToken = new System.Windows.Forms.Button();
            this.txtSecurityCode = new System.Windows.Forms.TextBox();
            this.lblSecurityCode = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // setupBrowser
            // 
            this.setupBrowser.Location = new System.Drawing.Point(414, 140);
            this.setupBrowser.Name = "setupBrowser";
            this.setupBrowser.Size = new System.Drawing.Size(200, 29);
            this.setupBrowser.TabIndex = 0;
            this.setupBrowser.Text = "Setup Browser";
            this.setupBrowser.UseVisualStyleBackColor = true;
            this.setupBrowser.Click += new System.EventHandler(this.setupBrowser_Click);
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(359, 271);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(0, 20);
            this.lblToken.TabIndex = 1;
            // 
            // btnRetrieveToken
            // 
            this.btnRetrieveToken.Location = new System.Drawing.Point(12, 114);
            this.btnRetrieveToken.Name = "btnRetrieveToken";
            this.btnRetrieveToken.Size = new System.Drawing.Size(219, 45);
            this.btnRetrieveToken.TabIndex = 2;
            this.btnRetrieveToken.Text = "Retrieve Token";
            this.btnRetrieveToken.UseVisualStyleBackColor = true;
            this.btnRetrieveToken.Click += new System.EventHandler(this.btnRetrieveToken_Click);
            // 
            // txtSecurityCode
            // 
            this.txtSecurityCode.Location = new System.Drawing.Point(12, 67);
            this.txtSecurityCode.Name = "txtSecurityCode";
            this.txtSecurityCode.Size = new System.Drawing.Size(219, 27);
            this.txtSecurityCode.TabIndex = 3;
            this.txtSecurityCode.Text = "";
            // 
            // lblSecurityCode
            // 
            this.lblSecurityCode.AutoSize = true;
            this.lblSecurityCode.Location = new System.Drawing.Point(12, 34);
            this.lblSecurityCode.Name = "lblSecurityCode";
            this.lblSecurityCode.Size = new System.Drawing.Size(100, 20);
            this.lblSecurityCode.TabIndex = 4;
            this.lblSecurityCode.Text = "Security Code";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(323, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 5;
            this.button1.Text = "Encrypt";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblSecurityCode);
            this.Controls.Add(this.txtSecurityCode);
            this.Controls.Add(this.btnRetrieveToken);
            this.Controls.Add(this.lblToken);
            this.Controls.Add(this.setupBrowser);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setupBrowser;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.Button btnRetrieveToken;
        private System.Windows.Forms.TextBox txtSecurityCode;
        private System.Windows.Forms.Label lblSecurityCode;
        private System.Windows.Forms.Button button1;
    }
}

