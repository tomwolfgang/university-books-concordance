namespace application {
  partial class DocumentsQueryDialog {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentsQueryDialog));
      this.groupBoxDocProperties = new System.Windows.Forms.GroupBox();
      this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
      this.checkBoxReleaseDate = new System.Windows.Forms.CheckBox();
      this.textBoxAuthor = new System.Windows.Forms.TextBox();
      this.checkBoxAuthor = new System.Windows.Forms.CheckBox();
      this.textBoxTitle = new System.Windows.Forms.TextBox();
      this.checkBoxTitle = new System.Windows.Forms.CheckBox();
      this.textBoxGutenbergId = new System.Windows.Forms.TextBox();
      this.checkBoxGutenbergId = new System.Windows.Forms.CheckBox();
      this.groupBoxWords = new System.Windows.Forms.GroupBox();
      this.btnRemoveWord = new System.Windows.Forms.Button();
      this.btnAddWord = new System.Windows.Forms.Button();
      this.textBoxWord = new System.Windows.Forms.TextBox();
      this.listBoxWords = new System.Windows.Forms.ListBox();
      this.groupBoxResults = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.listViewDocuments = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnQuery = new System.Windows.Forms.Button();
      this.btnNext = new System.Windows.Forms.Button();
      this.lblWordsQueryInformation1 = new System.Windows.Forms.Label();
      this.lblWordsQueryInformation2 = new System.Windows.Forms.Label();
      this.lblWordsQueryInformation = new System.Windows.Forms.Label();
      this.groupBoxDocProperties.SuspendLayout();
      this.groupBoxWords.SuspendLayout();
      this.groupBoxResults.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxDocProperties
      // 
      this.groupBoxDocProperties.Controls.Add(this.dateTimePicker);
      this.groupBoxDocProperties.Controls.Add(this.checkBoxReleaseDate);
      this.groupBoxDocProperties.Controls.Add(this.textBoxAuthor);
      this.groupBoxDocProperties.Controls.Add(this.checkBoxAuthor);
      this.groupBoxDocProperties.Controls.Add(this.textBoxTitle);
      this.groupBoxDocProperties.Controls.Add(this.checkBoxTitle);
      this.groupBoxDocProperties.Controls.Add(this.textBoxGutenbergId);
      this.groupBoxDocProperties.Controls.Add(this.checkBoxGutenbergId);
      this.groupBoxDocProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxDocProperties.Location = new System.Drawing.Point(12, 12);
      this.groupBoxDocProperties.Name = "groupBoxDocProperties";
      this.groupBoxDocProperties.Size = new System.Drawing.Size(559, 252);
      this.groupBoxDocProperties.TabIndex = 0;
      this.groupBoxDocProperties.TabStop = false;
      this.groupBoxDocProperties.Text = "Document Properties:";
      // 
      // dateTimePicker
      // 
      this.dateTimePicker.Enabled = false;
      this.dateTimePicker.Location = new System.Drawing.Point(169, 175);
      this.dateTimePicker.Name = "dateTimePicker";
      this.dateTimePicker.Size = new System.Drawing.Size(384, 30);
      this.dateTimePicker.TabIndex = 7;
      // 
      // checkBoxReleaseDate
      // 
      this.checkBoxReleaseDate.AutoSize = true;
      this.checkBoxReleaseDate.Location = new System.Drawing.Point(6, 175);
      this.checkBoxReleaseDate.Name = "checkBoxReleaseDate";
      this.checkBoxReleaseDate.Size = new System.Drawing.Size(157, 29);
      this.checkBoxReleaseDate.TabIndex = 6;
      this.checkBoxReleaseDate.Text = "Release Date:";
      this.checkBoxReleaseDate.UseVisualStyleBackColor = true;
      this.checkBoxReleaseDate.CheckedChanged += new System.EventHandler(this.checkBoxReleaseDate_CheckedChanged);
      // 
      // textBoxAuthor
      // 
      this.textBoxAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAuthor.Enabled = false;
      this.textBoxAuthor.Location = new System.Drawing.Point(169, 124);
      this.textBoxAuthor.Name = "textBoxAuthor";
      this.textBoxAuthor.Size = new System.Drawing.Size(384, 30);
      this.textBoxAuthor.TabIndex = 5;
      // 
      // checkBoxAuthor
      // 
      this.checkBoxAuthor.AutoSize = true;
      this.checkBoxAuthor.Location = new System.Drawing.Point(6, 124);
      this.checkBoxAuthor.Name = "checkBoxAuthor";
      this.checkBoxAuthor.Size = new System.Drawing.Size(98, 29);
      this.checkBoxAuthor.TabIndex = 4;
      this.checkBoxAuthor.Text = "Author:";
      this.checkBoxAuthor.UseVisualStyleBackColor = true;
      this.checkBoxAuthor.CheckedChanged += new System.EventHandler(this.checkBoxAuthor_CheckedChanged);
      // 
      // textBoxTitle
      // 
      this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTitle.Enabled = false;
      this.textBoxTitle.Location = new System.Drawing.Point(169, 78);
      this.textBoxTitle.Name = "textBoxTitle";
      this.textBoxTitle.Size = new System.Drawing.Size(384, 30);
      this.textBoxTitle.TabIndex = 3;
      // 
      // checkBoxTitle
      // 
      this.checkBoxTitle.AutoSize = true;
      this.checkBoxTitle.Location = new System.Drawing.Point(6, 78);
      this.checkBoxTitle.Name = "checkBoxTitle";
      this.checkBoxTitle.Size = new System.Drawing.Size(77, 29);
      this.checkBoxTitle.TabIndex = 2;
      this.checkBoxTitle.Text = "Title:";
      this.checkBoxTitle.UseVisualStyleBackColor = true;
      this.checkBoxTitle.CheckedChanged += new System.EventHandler(this.checkBoxTitle_CheckedChanged);
      // 
      // textBoxGutenbergId
      // 
      this.textBoxGutenbergId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxGutenbergId.Enabled = false;
      this.textBoxGutenbergId.Location = new System.Drawing.Point(169, 32);
      this.textBoxGutenbergId.Name = "textBoxGutenbergId";
      this.textBoxGutenbergId.Size = new System.Drawing.Size(384, 30);
      this.textBoxGutenbergId.TabIndex = 1;
      // 
      // checkBoxGutenbergId
      // 
      this.checkBoxGutenbergId.AutoSize = true;
      this.checkBoxGutenbergId.Location = new System.Drawing.Point(6, 32);
      this.checkBoxGutenbergId.Name = "checkBoxGutenbergId";
      this.checkBoxGutenbergId.Size = new System.Drawing.Size(153, 29);
      this.checkBoxGutenbergId.TabIndex = 0;
      this.checkBoxGutenbergId.Text = "Gutenberg Id:";
      this.checkBoxGutenbergId.UseVisualStyleBackColor = true;
      this.checkBoxGutenbergId.CheckedChanged += new System.EventHandler(this.checkBoxGutenbergId_CheckedChanged);
      // 
      // groupBoxWords
      // 
      this.groupBoxWords.Controls.Add(this.btnRemoveWord);
      this.groupBoxWords.Controls.Add(this.btnAddWord);
      this.groupBoxWords.Controls.Add(this.textBoxWord);
      this.groupBoxWords.Controls.Add(this.listBoxWords);
      this.groupBoxWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxWords.Location = new System.Drawing.Point(706, 12);
      this.groupBoxWords.Name = "groupBoxWords";
      this.groupBoxWords.Size = new System.Drawing.Size(501, 254);
      this.groupBoxWords.TabIndex = 1;
      this.groupBoxWords.TabStop = false;
      this.groupBoxWords.Text = "Words Filter (0):";
      // 
      // btnRemoveWord
      // 
      this.btnRemoveWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnRemoveWord.Location = new System.Drawing.Point(386, 211);
      this.btnRemoveWord.Name = "btnRemoveWord";
      this.btnRemoveWord.Size = new System.Drawing.Size(109, 37);
      this.btnRemoveWord.TabIndex = 3;
      this.btnRemoveWord.Text = "Remove";
      this.btnRemoveWord.UseVisualStyleBackColor = true;
      this.btnRemoveWord.Click += new System.EventHandler(this.btnRemoveWord_Click);
      // 
      // btnAddWord
      // 
      this.btnAddWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnAddWord.Location = new System.Drawing.Point(290, 211);
      this.btnAddWord.Name = "btnAddWord";
      this.btnAddWord.Size = new System.Drawing.Size(90, 37);
      this.btnAddWord.TabIndex = 2;
      this.btnAddWord.Text = "Add";
      this.btnAddWord.UseVisualStyleBackColor = true;
      this.btnAddWord.Click += new System.EventHandler(this.btnAddWord_Click);
      // 
      // textBoxWord
      // 
      this.textBoxWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxWord.Location = new System.Drawing.Point(6, 213);
      this.textBoxWord.Name = "textBoxWord";
      this.textBoxWord.Size = new System.Drawing.Size(278, 30);
      this.textBoxWord.TabIndex = 1;
      this.textBoxWord.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxWord_KeyPress);
      // 
      // listBoxWords
      // 
      this.listBoxWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listBoxWords.FormattingEnabled = true;
      this.listBoxWords.ItemHeight = 25;
      this.listBoxWords.Location = new System.Drawing.Point(6, 26);
      this.listBoxWords.Name = "listBoxWords";
      this.listBoxWords.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.listBoxWords.Size = new System.Drawing.Size(489, 179);
      this.listBoxWords.TabIndex = 0;
      this.listBoxWords.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxWords_KeyUp);
      // 
      // groupBoxResults
      // 
      this.groupBoxResults.Controls.Add(this.label1);
      this.groupBoxResults.Controls.Add(this.listViewDocuments);
      this.groupBoxResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxResults.Location = new System.Drawing.Point(12, 270);
      this.groupBoxResults.Name = "groupBoxResults";
      this.groupBoxResults.Size = new System.Drawing.Size(1201, 444);
      this.groupBoxResults.TabIndex = 2;
      this.groupBoxResults.TabStop = false;
      this.groupBoxResults.Text = "Results (0):";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(1087, 2);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 18);
      this.label1.TabIndex = 2;
      this.label1.Text = "[Double Click]";
      // 
      // listViewDocuments
      // 
      this.listViewDocuments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
      this.listViewDocuments.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listViewDocuments.FullRowSelect = true;
      this.listViewDocuments.GridLines = true;
      this.listViewDocuments.Location = new System.Drawing.Point(7, 29);
      this.listViewDocuments.Name = "listViewDocuments";
      this.listViewDocuments.ShowItemToolTips = true;
      this.listViewDocuments.Size = new System.Drawing.Size(1188, 409);
      this.listViewDocuments.TabIndex = 0;
      this.listViewDocuments.UseCompatibleStateImageBehavior = false;
      this.listViewDocuments.View = System.Windows.Forms.View.Details;
      this.listViewDocuments.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewDocuments_MouseDoubleClick);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Gutenberg Id";
      this.columnHeader1.Width = 132;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Author";
      this.columnHeader2.Width = 163;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Title";
      this.columnHeader3.Width = 450;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Release Date";
      this.columnHeader4.Width = 170;
      // 
      // btnQuery
      // 
      this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnQuery.Location = new System.Drawing.Point(594, 105);
      this.btnQuery.Name = "btnQuery";
      this.btnQuery.Size = new System.Drawing.Size(95, 38);
      this.btnQuery.TabIndex = 3;
      this.btnQuery.Text = "Query";
      this.btnQuery.UseVisualStyleBackColor = true;
      this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
      // 
      // btnNext
      // 
      this.btnNext.Enabled = false;
      this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnNext.Location = new System.Drawing.Point(996, 728);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new System.Drawing.Size(179, 56);
      this.btnNext.TabIndex = 4;
      this.btnNext.Text = "Next...";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
      // 
      // lblWordsQueryInformation1
      // 
      this.lblWordsQueryInformation1.AutoSize = true;
      this.lblWordsQueryInformation1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsQueryInformation1.Location = new System.Drawing.Point(211, 728);
      this.lblWordsQueryInformation1.Name = "lblWordsQueryInformation1";
      this.lblWordsQueryInformation1.Size = new System.Drawing.Size(555, 20);
      this.lblWordsQueryInformation1.TabIndex = 5;
      this.lblWordsQueryInformation1.Text = "You may query documents that you\'d like to filter the Words Inspector for.";
      // 
      // lblWordsQueryInformation2
      // 
      this.lblWordsQueryInformation2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsQueryInformation2.Location = new System.Drawing.Point(16, 751);
      this.lblWordsQueryInformation2.Name = "lblWordsQueryInformation2";
      this.lblWordsQueryInformation2.Size = new System.Drawing.Size(862, 51);
      this.lblWordsQueryInformation2.TabIndex = 6;
      this.lblWordsQueryInformation2.Text = resources.GetString("lblWordsQueryInformation2.Text");
      // 
      // lblWordsQueryInformation
      // 
      this.lblWordsQueryInformation.AutoSize = true;
      this.lblWordsQueryInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWordsQueryInformation.Location = new System.Drawing.Point(16, 728);
      this.lblWordsQueryInformation.Name = "lblWordsQueryInformation";
      this.lblWordsQueryInformation.Size = new System.Drawing.Size(153, 20);
      this.lblWordsQueryInformation.TabIndex = 7;
      this.lblWordsQueryInformation.Text = "Words Inspector:";
      // 
      // DocumentsQueryDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1219, 797);
      this.Controls.Add(this.lblWordsQueryInformation);
      this.Controls.Add(this.lblWordsQueryInformation2);
      this.Controls.Add(this.lblWordsQueryInformation1);
      this.Controls.Add(this.btnNext);
      this.Controls.Add(this.btnQuery);
      this.Controls.Add(this.groupBoxResults);
      this.Controls.Add(this.groupBoxWords);
      this.Controls.Add(this.groupBoxDocProperties);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "DocumentsQueryDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Documents Query";
      this.groupBoxDocProperties.ResumeLayout(false);
      this.groupBoxDocProperties.PerformLayout();
      this.groupBoxWords.ResumeLayout(false);
      this.groupBoxWords.PerformLayout();
      this.groupBoxResults.ResumeLayout(false);
      this.groupBoxResults.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.GroupBox groupBoxDocProperties;
    public System.Windows.Forms.CheckBox checkBoxGutenbergId;
    public System.Windows.Forms.TextBox textBoxGutenbergId;
    public System.Windows.Forms.TextBox textBoxTitle;
    public System.Windows.Forms.CheckBox checkBoxTitle;
    public System.Windows.Forms.TextBox textBoxAuthor;
    public System.Windows.Forms.CheckBox checkBoxAuthor;
    public System.Windows.Forms.CheckBox checkBoxReleaseDate;
    public System.Windows.Forms.DateTimePicker dateTimePicker;
    public System.Windows.Forms.GroupBox groupBoxWords;
    public System.Windows.Forms.Button btnRemoveWord;
    public System.Windows.Forms.Button btnAddWord;
    public System.Windows.Forms.TextBox textBoxWord;
    public System.Windows.Forms.ListBox listBoxWords;
    public System.Windows.Forms.GroupBox groupBoxResults;
    public System.Windows.Forms.ListView listViewDocuments;
    public System.Windows.Forms.Button btnQuery;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    public System.Windows.Forms.Button btnNext;
    public System.Windows.Forms.Label lblWordsQueryInformation1;
    public System.Windows.Forms.Label lblWordsQueryInformation2;
    private System.Windows.Forms.Label lblWordsQueryInformation;
    private System.Windows.Forms.Label label1;
  }
}