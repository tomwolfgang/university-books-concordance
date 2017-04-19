namespace application {
  partial class LoadingScreenDialog {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.lblLoadingMessage = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // lblLoadingMessage
      // 
      this.lblLoadingMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLoadingMessage.Location = new System.Drawing.Point(0, 20);
      this.lblLoadingMessage.Name = "lblLoadingMessage";
      this.lblLoadingMessage.Size = new System.Drawing.Size(534, 30);
      this.lblLoadingMessage.TabIndex = 0;
      this.lblLoadingMessage.Text = "Please wait while loading...";
      this.lblLoadingMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // btnCancel
      // 
      this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(207, 138);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(125, 43);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // progressBar
      // 
      this.progressBar.Location = new System.Drawing.Point(39, 87);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(463, 23);
      this.progressBar.TabIndex = 2;
      this.progressBar.Visible = false;
      // 
      // LoadingScreenDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(535, 224);
      this.ControlBox = false;
      this.Controls.Add(this.progressBar);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.lblLoadingMessage);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "LoadingScreenDialog";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Book Concordance - Loading...";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadingScreen_FormClosing);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblLoadingMessage;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ProgressBar progressBar;
  }
}