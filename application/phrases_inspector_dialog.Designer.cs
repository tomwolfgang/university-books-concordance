namespace application {
  partial class PhrasesInspectorDialog {
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
      this.groupBoxLocations = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.listViewLocations = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.richTextBoxContents = new System.Windows.Forms.RichTextBox();
      this.groupBoxPreview = new System.Windows.Forms.GroupBox();
      this.groupBoxPhrases = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.listViewPhrases = new System.Windows.Forms.ListView();
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxLocations.SuspendLayout();
      this.groupBoxPreview.SuspendLayout();
      this.groupBoxPhrases.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxLocations
      // 
      this.groupBoxLocations.Controls.Add(this.label2);
      this.groupBoxLocations.Controls.Add(this.listViewLocations);
      this.groupBoxLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxLocations.Location = new System.Drawing.Point(8, 263);
      this.groupBoxLocations.Name = "groupBoxLocations";
      this.groupBoxLocations.Size = new System.Drawing.Size(1125, 284);
      this.groupBoxLocations.TabIndex = 10;
      this.groupBoxLocations.TabStop = false;
      this.groupBoxLocations.Text = "Locations (0):";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(1009, 2);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 18);
      this.label2.TabIndex = 14;
      this.label2.Text = "[Double Click]";
      // 
      // listViewLocations
      // 
      this.listViewLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader2,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
      this.listViewLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listViewLocations.FullRowSelect = true;
      this.listViewLocations.GridLines = true;
      this.listViewLocations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewLocations.HideSelection = false;
      this.listViewLocations.Location = new System.Drawing.Point(17, 34);
      this.listViewLocations.MultiSelect = false;
      this.listViewLocations.Name = "listViewLocations";
      this.listViewLocations.ShowItemToolTips = true;
      this.listViewLocations.Size = new System.Drawing.Size(1096, 244);
      this.listViewLocations.TabIndex = 1;
      this.listViewLocations.UseCompatibleStateImageBehavior = false;
      this.listViewLocations.View = System.Windows.Forms.View.Details;
      this.listViewLocations.DoubleClick += new System.EventHandler(this.listViewLocations_DoubleClick);
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Doc Id";
      this.columnHeader3.Width = 100;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Title";
      this.columnHeader2.Width = 380;
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
      this.columnHeader7.Width = 122;
      // 
      // richTextBoxContents
      // 
      this.richTextBoxContents.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.richTextBoxContents.Location = new System.Drawing.Point(6, 29);
      this.richTextBoxContents.Name = "richTextBoxContents";
      this.richTextBoxContents.ReadOnly = true;
      this.richTextBoxContents.Size = new System.Drawing.Size(1096, 231);
      this.richTextBoxContents.TabIndex = 0;
      this.richTextBoxContents.Text = "";
      // 
      // groupBoxPreview
      // 
      this.groupBoxPreview.Controls.Add(this.richTextBoxContents);
      this.groupBoxPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxPreview.Location = new System.Drawing.Point(8, 553);
      this.groupBoxPreview.Name = "groupBoxPreview";
      this.groupBoxPreview.Size = new System.Drawing.Size(1125, 274);
      this.groupBoxPreview.TabIndex = 11;
      this.groupBoxPreview.TabStop = false;
      this.groupBoxPreview.Text = "Preview:";
      // 
      // groupBoxPhrases
      // 
      this.groupBoxPhrases.Controls.Add(this.label1);
      this.groupBoxPhrases.Controls.Add(this.listViewPhrases);
      this.groupBoxPhrases.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxPhrases.Location = new System.Drawing.Point(8, 12);
      this.groupBoxPhrases.Name = "groupBoxPhrases";
      this.groupBoxPhrases.Size = new System.Drawing.Size(1125, 236);
      this.groupBoxPhrases.TabIndex = 12;
      this.groupBoxPhrases.TabStop = false;
      this.groupBoxPhrases.Text = "Phrases (0):";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(1013, 2);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 18);
      this.label1.TabIndex = 13;
      this.label1.Text = "[Double Click]";
      // 
      // listViewPhrases
      // 
      this.listViewPhrases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9});
      this.listViewPhrases.FullRowSelect = true;
      this.listViewPhrases.GridLines = true;
      this.listViewPhrases.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewPhrases.HideSelection = false;
      this.listViewPhrases.Location = new System.Drawing.Point(17, 29);
      this.listViewPhrases.MultiSelect = false;
      this.listViewPhrases.Name = "listViewPhrases";
      this.listViewPhrases.ShowItemToolTips = true;
      this.listViewPhrases.Size = new System.Drawing.Size(1096, 188);
      this.listViewPhrases.TabIndex = 0;
      this.listViewPhrases.UseCompatibleStateImageBehavior = false;
      this.listViewPhrases.View = System.Windows.Forms.View.Details;
      this.listViewPhrases.DoubleClick += new System.EventHandler(this.listViewPhrases_DoubleClick);
      // 
      // columnHeader9
      // 
      this.columnHeader9.Text = "Phrase";
      this.columnHeader9.Width = 740;
      // 
      // PhrasesInspectorDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1145, 836);
      this.Controls.Add(this.groupBoxPhrases);
      this.Controls.Add(this.groupBoxLocations);
      this.Controls.Add(this.groupBoxPreview);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "PhrasesInspectorDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Phrases Inspector";
      this.Shown += new System.EventHandler(this.PhrasesInspectorDialog_Shown);
      this.groupBoxLocations.ResumeLayout(false);
      this.groupBoxLocations.PerformLayout();
      this.groupBoxPreview.ResumeLayout(false);
      this.groupBoxPhrases.ResumeLayout(false);
      this.groupBoxPhrases.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.GroupBox groupBoxLocations;
    public System.Windows.Forms.ListView listViewLocations;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    public System.Windows.Forms.RichTextBox richTextBoxContents;
    public System.Windows.Forms.GroupBox groupBoxPreview;
    public System.Windows.Forms.GroupBox groupBoxPhrases;
    public System.Windows.Forms.ListView listViewPhrases;
    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
  }
}