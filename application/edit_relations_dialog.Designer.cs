namespace application {
  partial class EditRelationsDialog {
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
      this.groupBoxRelations = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnRemoveRelation = new System.Windows.Forms.Button();
      this.btnAddRelation = new System.Windows.Forms.Button();
      this.textBoxRelation = new System.Windows.Forms.TextBox();
      this.listViewRelations = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxWordPairs = new System.Windows.Forms.GroupBox();
      this.lblSecondWord = new System.Windows.Forms.Label();
      this.lblFirstWord = new System.Windows.Forms.Label();
      this.textBoxFirstWord = new System.Windows.Forms.TextBox();
      this.btnRemoveWordPair = new System.Windows.Forms.Button();
      this.btnAddWordPair = new System.Windows.Forms.Button();
      this.textBoxSecondWord = new System.Windows.Forms.TextBox();
      this.listViewWordPairsInRelation = new System.Windows.Forms.ListView();
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxWordsInDatabase = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnLoadWords = new System.Windows.Forms.Button();
      this.listViewWordsInDatabase = new System.Windows.Forms.ListView();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxRelations.SuspendLayout();
      this.groupBoxWordPairs.SuspendLayout();
      this.groupBoxWordsInDatabase.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxRelations
      // 
      this.groupBoxRelations.Controls.Add(this.label2);
      this.groupBoxRelations.Controls.Add(this.btnRemoveRelation);
      this.groupBoxRelations.Controls.Add(this.btnAddRelation);
      this.groupBoxRelations.Controls.Add(this.textBoxRelation);
      this.groupBoxRelations.Controls.Add(this.listViewRelations);
      this.groupBoxRelations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxRelations.Location = new System.Drawing.Point(12, 12);
      this.groupBoxRelations.Name = "groupBoxRelations";
      this.groupBoxRelations.Size = new System.Drawing.Size(348, 527);
      this.groupBoxRelations.TabIndex = 2;
      this.groupBoxRelations.TabStop = false;
      this.groupBoxRelations.Text = "Relations (0):";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(237, 1);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 18);
      this.label2.TabIndex = 5;
      this.label2.Text = "[Double Click]";
      // 
      // btnRemoveRelation
      // 
      this.btnRemoveRelation.Enabled = false;
      this.btnRemoveRelation.Location = new System.Drawing.Point(204, 480);
      this.btnRemoveRelation.Name = "btnRemoveRelation";
      this.btnRemoveRelation.Size = new System.Drawing.Size(138, 39);
      this.btnRemoveRelation.TabIndex = 3;
      this.btnRemoveRelation.Text = "Remove";
      this.btnRemoveRelation.UseVisualStyleBackColor = true;
      this.btnRemoveRelation.Click += new System.EventHandler(this.btnRemoveRelation_Click);
      // 
      // btnAddRelation
      // 
      this.btnAddRelation.Location = new System.Drawing.Point(7, 480);
      this.btnAddRelation.Name = "btnAddRelation";
      this.btnAddRelation.Size = new System.Drawing.Size(137, 39);
      this.btnAddRelation.TabIndex = 2;
      this.btnAddRelation.Text = "Add";
      this.btnAddRelation.UseVisualStyleBackColor = true;
      this.btnAddRelation.Click += new System.EventHandler(this.btnAddRelation_Click);
      // 
      // textBoxRelation
      // 
      this.textBoxRelation.Location = new System.Drawing.Point(7, 443);
      this.textBoxRelation.Name = "textBoxRelation";
      this.textBoxRelation.Size = new System.Drawing.Size(335, 30);
      this.textBoxRelation.TabIndex = 1;
      // 
      // listViewRelations
      // 
      this.listViewRelations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.listViewRelations.FullRowSelect = true;
      this.listViewRelations.GridLines = true;
      this.listViewRelations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewRelations.HideSelection = false;
      this.listViewRelations.Location = new System.Drawing.Point(6, 29);
      this.listViewRelations.MultiSelect = false;
      this.listViewRelations.Name = "listViewRelations";
      this.listViewRelations.ShowItemToolTips = true;
      this.listViewRelations.Size = new System.Drawing.Size(336, 407);
      this.listViewRelations.TabIndex = 0;
      this.listViewRelations.UseCompatibleStateImageBehavior = false;
      this.listViewRelations.View = System.Windows.Forms.View.Details;
      this.listViewRelations.SelectedIndexChanged += new System.EventHandler(this.listViewRelations_SelectedIndexChanged);
      this.listViewRelations.DoubleClick += new System.EventHandler(this.listViewRelations_DoubleClick);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Relation";
      this.columnHeader1.Width = 220;
      // 
      // groupBoxWordPairs
      // 
      this.groupBoxWordPairs.Controls.Add(this.lblSecondWord);
      this.groupBoxWordPairs.Controls.Add(this.lblFirstWord);
      this.groupBoxWordPairs.Controls.Add(this.textBoxFirstWord);
      this.groupBoxWordPairs.Controls.Add(this.btnRemoveWordPair);
      this.groupBoxWordPairs.Controls.Add(this.btnAddWordPair);
      this.groupBoxWordPairs.Controls.Add(this.textBoxSecondWord);
      this.groupBoxWordPairs.Controls.Add(this.listViewWordPairsInRelation);
      this.groupBoxWordPairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxWordPairs.Location = new System.Drawing.Point(384, 12);
      this.groupBoxWordPairs.Name = "groupBoxWordPairs";
      this.groupBoxWordPairs.Size = new System.Drawing.Size(615, 527);
      this.groupBoxWordPairs.TabIndex = 3;
      this.groupBoxWordPairs.TabStop = false;
      this.groupBoxWordPairs.Text = "Word Pairs in Relation (0):";
      // 
      // lblSecondWord
      // 
      this.lblSecondWord.AutoSize = true;
      this.lblSecondWord.Location = new System.Drawing.Point(7, 440);
      this.lblSecondWord.Name = "lblSecondWord";
      this.lblSecondWord.Size = new System.Drawing.Size(139, 25);
      this.lblSecondWord.TabIndex = 6;
      this.lblSecondWord.Text = "Second Word:";
      // 
      // lblFirstWord
      // 
      this.lblFirstWord.AutoSize = true;
      this.lblFirstWord.Location = new System.Drawing.Point(7, 404);
      this.lblFirstWord.Name = "lblFirstWord";
      this.lblFirstWord.Size = new System.Drawing.Size(108, 25);
      this.lblFirstWord.TabIndex = 5;
      this.lblFirstWord.Text = "First Word:";
      // 
      // textBoxFirstWord
      // 
      this.textBoxFirstWord.AllowDrop = true;
      this.textBoxFirstWord.Location = new System.Drawing.Point(254, 404);
      this.textBoxFirstWord.Name = "textBoxFirstWord";
      this.textBoxFirstWord.Size = new System.Drawing.Size(355, 30);
      this.textBoxFirstWord.TabIndex = 4;
      this.textBoxFirstWord.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxFirstWord_DragDrop);
      this.textBoxFirstWord.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxFirstWord_DragEnter);
      // 
      // btnRemoveWordPair
      // 
      this.btnRemoveWordPair.Enabled = false;
      this.btnRemoveWordPair.Location = new System.Drawing.Point(471, 480);
      this.btnRemoveWordPair.Name = "btnRemoveWordPair";
      this.btnRemoveWordPair.Size = new System.Drawing.Size(138, 39);
      this.btnRemoveWordPair.TabIndex = 7;
      this.btnRemoveWordPair.Text = "Remove";
      this.btnRemoveWordPair.UseVisualStyleBackColor = true;
      this.btnRemoveWordPair.Click += new System.EventHandler(this.btnRemoveWordPair_Click);
      // 
      // btnAddWordPair
      // 
      this.btnAddWordPair.Enabled = false;
      this.btnAddWordPair.Location = new System.Drawing.Point(254, 480);
      this.btnAddWordPair.Name = "btnAddWordPair";
      this.btnAddWordPair.Size = new System.Drawing.Size(137, 39);
      this.btnAddWordPair.TabIndex = 6;
      this.btnAddWordPair.Text = "Add";
      this.btnAddWordPair.UseVisualStyleBackColor = true;
      this.btnAddWordPair.Click += new System.EventHandler(this.btnAddWordPair_Click);
      // 
      // textBoxSecondWord
      // 
      this.textBoxSecondWord.AllowDrop = true;
      this.textBoxSecondWord.Location = new System.Drawing.Point(254, 440);
      this.textBoxSecondWord.Name = "textBoxSecondWord";
      this.textBoxSecondWord.Size = new System.Drawing.Size(355, 30);
      this.textBoxSecondWord.TabIndex = 5;
      this.textBoxSecondWord.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxSecondWord_DragDrop);
      this.textBoxSecondWord.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxSecondWord_DragEnter);
      // 
      // listViewWordPairsInRelation
      // 
      this.listViewWordPairsInRelation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
      this.listViewWordPairsInRelation.FullRowSelect = true;
      this.listViewWordPairsInRelation.GridLines = true;
      this.listViewWordPairsInRelation.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewWordPairsInRelation.HideSelection = false;
      this.listViewWordPairsInRelation.Location = new System.Drawing.Point(6, 29);
      this.listViewWordPairsInRelation.MultiSelect = false;
      this.listViewWordPairsInRelation.Name = "listViewWordPairsInRelation";
      this.listViewWordPairsInRelation.ShowItemToolTips = true;
      this.listViewWordPairsInRelation.Size = new System.Drawing.Size(603, 369);
      this.listViewWordPairsInRelation.TabIndex = 0;
      this.listViewWordPairsInRelation.UseCompatibleStateImageBehavior = false;
      this.listViewWordPairsInRelation.View = System.Windows.Forms.View.Details;
      this.listViewWordPairsInRelation.SelectedIndexChanged += new System.EventHandler(this.listViewWordPairsInRelation_SelectedIndexChanged);
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "First";
      this.columnHeader2.Width = 200;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Second";
      this.columnHeader3.Width = 200;
      // 
      // groupBoxWordsInDatabase
      // 
      this.groupBoxWordsInDatabase.Controls.Add(this.label1);
      this.groupBoxWordsInDatabase.Controls.Add(this.btnLoadWords);
      this.groupBoxWordsInDatabase.Controls.Add(this.listViewWordsInDatabase);
      this.groupBoxWordsInDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxWordsInDatabase.Location = new System.Drawing.Point(1108, 12);
      this.groupBoxWordsInDatabase.Name = "groupBoxWordsInDatabase";
      this.groupBoxWordsInDatabase.Size = new System.Drawing.Size(383, 532);
      this.groupBoxWordsInDatabase.TabIndex = 4;
      this.groupBoxWordsInDatabase.TabStop = false;
      this.groupBoxWordsInDatabase.Text = "Words in DB (0):";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(191, 2);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(176, 18);
      this.label1.TabIndex = 5;
      this.label1.Text = "[Drag to textboxes on left]";
      // 
      // btnLoadWords
      // 
      this.btnLoadWords.Location = new System.Drawing.Point(128, 480);
      this.btnLoadWords.Name = "btnLoadWords";
      this.btnLoadWords.Size = new System.Drawing.Size(145, 39);
      this.btnLoadWords.TabIndex = 4;
      this.btnLoadWords.Text = "Load Words";
      this.btnLoadWords.UseVisualStyleBackColor = true;
      this.btnLoadWords.Click += new System.EventHandler(this.btnLoadWords_Click);
      // 
      // listViewWordsInDatabase
      // 
      this.listViewWordsInDatabase.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
      this.listViewWordsInDatabase.FullRowSelect = true;
      this.listViewWordsInDatabase.GridLines = true;
      this.listViewWordsInDatabase.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewWordsInDatabase.HideSelection = false;
      this.listViewWordsInDatabase.Location = new System.Drawing.Point(6, 29);
      this.listViewWordsInDatabase.MultiSelect = false;
      this.listViewWordsInDatabase.Name = "listViewWordsInDatabase";
      this.listViewWordsInDatabase.ShowItemToolTips = true;
      this.listViewWordsInDatabase.Size = new System.Drawing.Size(371, 441);
      this.listViewWordsInDatabase.TabIndex = 0;
      this.listViewWordsInDatabase.UseCompatibleStateImageBehavior = false;
      this.listViewWordsInDatabase.View = System.Windows.Forms.View.Details;
      this.listViewWordsInDatabase.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewWordsInDatabase_ItemDrag);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Word";
      this.columnHeader4.Width = 240;
      // 
      // EditRelationsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1496, 544);
      this.Controls.Add(this.groupBoxWordsInDatabase);
      this.Controls.Add(this.groupBoxWordPairs);
      this.Controls.Add(this.groupBoxRelations);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "EditRelationsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Edit Relations";
      this.Shown += new System.EventHandler(this.EditRelationsDialog_Shown);
      this.groupBoxRelations.ResumeLayout(false);
      this.groupBoxRelations.PerformLayout();
      this.groupBoxWordPairs.ResumeLayout(false);
      this.groupBoxWordPairs.PerformLayout();
      this.groupBoxWordsInDatabase.ResumeLayout(false);
      this.groupBoxWordsInDatabase.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.GroupBox groupBoxRelations;
    private System.Windows.Forms.Button btnRemoveRelation;
    private System.Windows.Forms.Button btnAddRelation;
    public System.Windows.Forms.TextBox textBoxRelation;
    public System.Windows.Forms.ListView listViewRelations;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    public System.Windows.Forms.GroupBox groupBoxWordPairs;
    private System.Windows.Forms.Button btnRemoveWordPair;
    public System.Windows.Forms.Button btnAddWordPair;
    public System.Windows.Forms.TextBox textBoxSecondWord;
    public System.Windows.Forms.ListView listViewWordPairsInRelation;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.Label lblSecondWord;
    private System.Windows.Forms.Label lblFirstWord;
    public System.Windows.Forms.TextBox textBoxFirstWord;
    public System.Windows.Forms.GroupBox groupBoxWordsInDatabase;
    public System.Windows.Forms.Button btnLoadWords;
    public System.Windows.Forms.ListView listViewWordsInDatabase;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
  }
}