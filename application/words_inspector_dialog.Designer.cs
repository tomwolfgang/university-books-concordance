namespace application {
  partial class WordsInspectorDialog {
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
      this.groupBoxWords = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.listViewWords = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxLocations = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.listViewLocations = new System.Windows.Forms.ListView();
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxPreview = new System.Windows.Forms.GroupBox();
      this.richTextBoxContents = new System.Windows.Forms.RichTextBox();
      this.groupBoxWords.SuspendLayout();
      this.groupBoxLocations.SuspendLayout();
      this.groupBoxPreview.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxWords
      // 
      this.groupBoxWords.Controls.Add(this.label1);
      this.groupBoxWords.Controls.Add(this.listViewWords);
      this.groupBoxWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxWords.Location = new System.Drawing.Point(13, 13);
      this.groupBoxWords.Name = "groupBoxWords";
      this.groupBoxWords.Size = new System.Drawing.Size(375, 677);
      this.groupBoxWords.TabIndex = 0;
      this.groupBoxWords.TabStop = false;
      this.groupBoxWords.Text = "Words (0):";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(265, 2);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 18);
      this.label1.TabIndex = 3;
      this.label1.Text = "[Double Click]";
      // 
      // listViewWords
      // 
      this.listViewWords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.listViewWords.FullRowSelect = true;
      this.listViewWords.GridLines = true;
      this.listViewWords.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewWords.HideSelection = false;
      this.listViewWords.Location = new System.Drawing.Point(6, 29);
      this.listViewWords.MultiSelect = false;
      this.listViewWords.Name = "listViewWords";
      this.listViewWords.ShowItemToolTips = true;
      this.listViewWords.Size = new System.Drawing.Size(362, 642);
      this.listViewWords.TabIndex = 0;
      this.listViewWords.UseCompatibleStateImageBehavior = false;
      this.listViewWords.View = System.Windows.Forms.View.Details;
      this.listViewWords.DoubleClick += new System.EventHandler(this.listViewWords_DoubleClick);
      this.listViewWords.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewWords_KeyPress);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Word";
      this.columnHeader1.Width = 220;
      // 
      // groupBoxLocations
      // 
      this.groupBoxLocations.Controls.Add(this.label2);
      this.groupBoxLocations.Controls.Add(this.listViewLocations);
      this.groupBoxLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxLocations.Location = new System.Drawing.Point(394, 13);
      this.groupBoxLocations.Name = "groupBoxLocations";
      this.groupBoxLocations.Size = new System.Drawing.Size(1054, 270);
      this.groupBoxLocations.TabIndex = 1;
      this.groupBoxLocations.TabStop = false;
      this.groupBoxLocations.Text = "Locations (0):";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(944, 2);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 18);
      this.label2.TabIndex = 4;
      this.label2.Text = "[Double Click]";
      // 
      // listViewLocations
      // 
      this.listViewLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader8,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
      this.listViewLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listViewLocations.FullRowSelect = true;
      this.listViewLocations.GridLines = true;
      this.listViewLocations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewLocations.HideSelection = false;
      this.listViewLocations.Location = new System.Drawing.Point(6, 29);
      this.listViewLocations.MultiSelect = false;
      this.listViewLocations.Name = "listViewLocations";
      this.listViewLocations.ShowItemToolTips = true;
      this.listViewLocations.Size = new System.Drawing.Size(1042, 235);
      this.listViewLocations.TabIndex = 1;
      this.listViewLocations.UseCompatibleStateImageBehavior = false;
      this.listViewLocations.View = System.Windows.Forms.View.Details;
      this.listViewLocations.DoubleClick += new System.EventHandler(this.listViewLocations_DoubleClick);
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Id";
      this.columnHeader2.Width = 80;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Title";
      this.columnHeader3.Width = 210;
      // 
      // columnHeader8
      // 
      this.columnHeader8.Text = "Word";
      this.columnHeader8.Width = 100;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Line";
      this.columnHeader4.Width = 100;
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Page";
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Sentence";
      this.columnHeader6.Width = 100;
      // 
      // columnHeader7
      // 
      this.columnHeader7.Text = "Paragraph";
      this.columnHeader7.Width = 100;
      // 
      // groupBoxPreview
      // 
      this.groupBoxPreview.Controls.Add(this.richTextBoxContents);
      this.groupBoxPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxPreview.Location = new System.Drawing.Point(394, 289);
      this.groupBoxPreview.Name = "groupBoxPreview";
      this.groupBoxPreview.Size = new System.Drawing.Size(1054, 401);
      this.groupBoxPreview.TabIndex = 2;
      this.groupBoxPreview.TabStop = false;
      this.groupBoxPreview.Text = "Preview:";
      // 
      // richTextBoxContents
      // 
      this.richTextBoxContents.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.richTextBoxContents.Location = new System.Drawing.Point(0, 29);
      this.richTextBoxContents.Name = "richTextBoxContents";
      this.richTextBoxContents.ReadOnly = true;
      this.richTextBoxContents.Size = new System.Drawing.Size(1048, 366);
      this.richTextBoxContents.TabIndex = 0;
      this.richTextBoxContents.Text = "";
      // 
      // WordsInspectorDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1460, 697);
      this.Controls.Add(this.groupBoxPreview);
      this.Controls.Add(this.groupBoxLocations);
      this.Controls.Add(this.groupBoxWords);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "WordsInspectorDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Words Inspector";
      this.Shown += new System.EventHandler(this.WordsQuery_Shown);
      this.groupBoxWords.ResumeLayout(false);
      this.groupBoxWords.PerformLayout();
      this.groupBoxLocations.ResumeLayout(false);
      this.groupBoxLocations.PerformLayout();
      this.groupBoxPreview.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.GroupBox groupBoxWords;
    public System.Windows.Forms.ListView listViewWords;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    public System.Windows.Forms.GroupBox groupBoxLocations;
    public System.Windows.Forms.ListView listViewLocations;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    public System.Windows.Forms.GroupBox groupBoxPreview;
    public System.Windows.Forms.RichTextBox richTextBoxContents;
    private System.Windows.Forms.ColumnHeader columnHeader8;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}