namespace application {
  partial class StatisticsDialog {
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
      this.groupBoxTopWords = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.listViewWordFrequencies = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lblWordsPerSentence = new System.Windows.Forms.Label();
      this.lblWordsPerLine = new System.Windows.Forms.Label();
      this.lblWordsPerParagraph = new System.Windows.Forms.Label();
      this.lblWordsPerPage = new System.Windows.Forms.Label();
      this.lblWordsPerDocument = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lblCharsPerLine = new System.Windows.Forms.Label();
      this.lblCharsPerDocument = new System.Windows.Forms.Label();
      this.lblCharsPerSentence = new System.Windows.Forms.Label();
      this.lblCharsPerPage = new System.Windows.Forms.Label();
      this.lblCharsPerParagraph = new System.Windows.Forms.Label();
      this.groupBoxTopWords.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxTopWords
      // 
      this.groupBoxTopWords.Controls.Add(this.label1);
      this.groupBoxTopWords.Controls.Add(this.listViewWordFrequencies);
      this.groupBoxTopWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxTopWords.Location = new System.Drawing.Point(75, 268);
      this.groupBoxTopWords.Name = "groupBoxTopWords";
      this.groupBoxTopWords.Size = new System.Drawing.Size(650, 373);
      this.groupBoxTopWords.TabIndex = 0;
      this.groupBoxTopWords.TabStop = false;
      this.groupBoxTopWords.Text = "Top Words (0):";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(536, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 18);
      this.label1.TabIndex = 1;
      this.label1.Text = "[Double Click]";
      // 
      // listViewWordFrequencies
      // 
      this.listViewWordFrequencies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.listViewWordFrequencies.FullRowSelect = true;
      this.listViewWordFrequencies.Location = new System.Drawing.Point(7, 30);
      this.listViewWordFrequencies.MultiSelect = false;
      this.listViewWordFrequencies.Name = "listViewWordFrequencies";
      this.listViewWordFrequencies.Size = new System.Drawing.Size(637, 337);
      this.listViewWordFrequencies.TabIndex = 0;
      this.listViewWordFrequencies.UseCompatibleStateImageBehavior = false;
      this.listViewWordFrequencies.View = System.Windows.Forms.View.Details;
      this.listViewWordFrequencies.DoubleClick += new System.EventHandler(this.listViewWordFrequencies_DoubleClick);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Word";
      this.columnHeader1.Width = 340;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Appearance";
      this.columnHeader2.Width = 100;
      // 
      // lblWordsPerSentence
      // 
      this.lblWordsPerSentence.AutoSize = true;
      this.lblWordsPerSentence.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsPerSentence.Location = new System.Drawing.Point(22, 76);
      this.lblWordsPerSentence.Name = "lblWordsPerSentence";
      this.lblWordsPerSentence.Size = new System.Drawing.Size(243, 25);
      this.lblWordsPerSentence.TabIndex = 1;
      this.lblWordsPerSentence.Text = "Per Sentence: Retreiving...";
      // 
      // lblWordsPerLine
      // 
      this.lblWordsPerLine.AutoSize = true;
      this.lblWordsPerLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsPerLine.Location = new System.Drawing.Point(69, 41);
      this.lblWordsPerLine.Name = "lblWordsPerLine";
      this.lblWordsPerLine.Size = new System.Drawing.Size(196, 25);
      this.lblWordsPerLine.TabIndex = 2;
      this.lblWordsPerLine.Text = "Per Line: Retreiving...";
      // 
      // lblWordsPerParagraph
      // 
      this.lblWordsPerParagraph.AutoSize = true;
      this.lblWordsPerParagraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsPerParagraph.Location = new System.Drawing.Point(15, 113);
      this.lblWordsPerParagraph.Name = "lblWordsPerParagraph";
      this.lblWordsPerParagraph.Size = new System.Drawing.Size(250, 25);
      this.lblWordsPerParagraph.TabIndex = 3;
      this.lblWordsPerParagraph.Text = "Per Paragraph: Retreiving...";
      // 
      // lblWordsPerPage
      // 
      this.lblWordsPerPage.AutoSize = true;
      this.lblWordsPerPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsPerPage.Location = new System.Drawing.Point(60, 153);
      this.lblWordsPerPage.Name = "lblWordsPerPage";
      this.lblWordsPerPage.Size = new System.Drawing.Size(205, 25);
      this.lblWordsPerPage.TabIndex = 4;
      this.lblWordsPerPage.Text = "Per Page: Retreiving...";
      // 
      // lblWordsPerDocument
      // 
      this.lblWordsPerDocument.AutoSize = true;
      this.lblWordsPerDocument.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsPerDocument.Location = new System.Drawing.Point(17, 193);
      this.lblWordsPerDocument.Name = "lblWordsPerDocument";
      this.lblWordsPerDocument.Size = new System.Drawing.Size(248, 25);
      this.lblWordsPerDocument.TabIndex = 5;
      this.lblWordsPerDocument.Text = "Per Document: Retreiving...";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.lblWordsPerLine);
      this.groupBox1.Controls.Add(this.lblWordsPerDocument);
      this.groupBox1.Controls.Add(this.lblWordsPerSentence);
      this.groupBox1.Controls.Add(this.lblWordsPerPage);
      this.groupBox1.Controls.Add(this.lblWordsPerParagraph);
      this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox1.Location = new System.Drawing.Point(12, 11);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(392, 234);
      this.groupBox1.TabIndex = 6;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Average Words:";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.lblCharsPerLine);
      this.groupBox2.Controls.Add(this.lblCharsPerDocument);
      this.groupBox2.Controls.Add(this.lblCharsPerSentence);
      this.groupBox2.Controls.Add(this.lblCharsPerPage);
      this.groupBox2.Controls.Add(this.lblCharsPerParagraph);
      this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox2.Location = new System.Drawing.Point(410, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(392, 234);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Average Characters:";
      // 
      // lblCharsPerLine
      // 
      this.lblCharsPerLine.AutoSize = true;
      this.lblCharsPerLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCharsPerLine.Location = new System.Drawing.Point(69, 41);
      this.lblCharsPerLine.Name = "lblCharsPerLine";
      this.lblCharsPerLine.Size = new System.Drawing.Size(196, 25);
      this.lblCharsPerLine.TabIndex = 2;
      this.lblCharsPerLine.Text = "Per Line: Retreiving...";
      // 
      // lblCharsPerDocument
      // 
      this.lblCharsPerDocument.AutoSize = true;
      this.lblCharsPerDocument.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCharsPerDocument.Location = new System.Drawing.Point(17, 193);
      this.lblCharsPerDocument.Name = "lblCharsPerDocument";
      this.lblCharsPerDocument.Size = new System.Drawing.Size(248, 25);
      this.lblCharsPerDocument.TabIndex = 5;
      this.lblCharsPerDocument.Text = "Per Document: Retreiving...";
      // 
      // lblCharsPerSentence
      // 
      this.lblCharsPerSentence.AutoSize = true;
      this.lblCharsPerSentence.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCharsPerSentence.Location = new System.Drawing.Point(22, 76);
      this.lblCharsPerSentence.Name = "lblCharsPerSentence";
      this.lblCharsPerSentence.Size = new System.Drawing.Size(243, 25);
      this.lblCharsPerSentence.TabIndex = 1;
      this.lblCharsPerSentence.Text = "Per Sentence: Retreiving...";
      // 
      // lblCharsPerPage
      // 
      this.lblCharsPerPage.AutoSize = true;
      this.lblCharsPerPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCharsPerPage.Location = new System.Drawing.Point(60, 153);
      this.lblCharsPerPage.Name = "lblCharsPerPage";
      this.lblCharsPerPage.Size = new System.Drawing.Size(205, 25);
      this.lblCharsPerPage.TabIndex = 4;
      this.lblCharsPerPage.Text = "Per Page: Retreiving...";
      // 
      // lblCharsPerParagraph
      // 
      this.lblCharsPerParagraph.AutoSize = true;
      this.lblCharsPerParagraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCharsPerParagraph.Location = new System.Drawing.Point(15, 113);
      this.lblCharsPerParagraph.Name = "lblCharsPerParagraph";
      this.lblCharsPerParagraph.Size = new System.Drawing.Size(250, 25);
      this.lblCharsPerParagraph.TabIndex = 3;
      this.lblCharsPerParagraph.Text = "Per Paragraph: Retreiving...";
      // 
      // StatisticsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(817, 647);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.groupBoxTopWords);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "StatisticsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Statistics";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticsDialog_FormClosing);
      this.Shown += new System.EventHandler(this.StatisticsDialog_Shown);
      this.groupBoxTopWords.ResumeLayout(false);
      this.groupBoxTopWords.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.GroupBox groupBoxTopWords;
    public System.Windows.Forms.ListView listViewWordFrequencies;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    public System.Windows.Forms.Label lblWordsPerSentence;
    public System.Windows.Forms.Label lblWordsPerLine;
    public System.Windows.Forms.Label lblWordsPerParagraph;
    public System.Windows.Forms.Label lblWordsPerPage;
    public System.Windows.Forms.Label lblWordsPerDocument;
    public System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label1;
    public System.Windows.Forms.GroupBox groupBox2;
    public System.Windows.Forms.Label lblCharsPerLine;
    public System.Windows.Forms.Label lblCharsPerDocument;
    public System.Windows.Forms.Label lblCharsPerSentence;
    public System.Windows.Forms.Label lblCharsPerPage;
    public System.Windows.Forms.Label lblCharsPerParagraph;
  }
}