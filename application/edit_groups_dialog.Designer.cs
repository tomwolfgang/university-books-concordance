namespace application {
  partial class EditGroupsDialog {
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
      this.groupBoxGroups = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnRemoveGroup = new System.Windows.Forms.Button();
      this.btnAddGroup = new System.Windows.Forms.Button();
      this.textBoxGroup = new System.Windows.Forms.TextBox();
      this.listViewGroups = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxWordsInGroup = new System.Windows.Forms.GroupBox();
      this.btnRemoveWord = new System.Windows.Forms.Button();
      this.btnAddWord = new System.Windows.Forms.Button();
      this.textBoxWord = new System.Windows.Forms.TextBox();
      this.listViewWordsInGroup = new System.Windows.Forms.ListView();
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxWordsInDatabase = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btnLoadWords = new System.Windows.Forms.Button();
      this.listViewWordsInDatabase = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxGroups.SuspendLayout();
      this.groupBoxWordsInGroup.SuspendLayout();
      this.groupBoxWordsInDatabase.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxGroups
      // 
      this.groupBoxGroups.Controls.Add(this.label2);
      this.groupBoxGroups.Controls.Add(this.btnRemoveGroup);
      this.groupBoxGroups.Controls.Add(this.btnAddGroup);
      this.groupBoxGroups.Controls.Add(this.textBoxGroup);
      this.groupBoxGroups.Controls.Add(this.listViewGroups);
      this.groupBoxGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxGroups.Location = new System.Drawing.Point(12, 12);
      this.groupBoxGroups.Name = "groupBoxGroups";
      this.groupBoxGroups.Size = new System.Drawing.Size(348, 527);
      this.groupBoxGroups.TabIndex = 1;
      this.groupBoxGroups.TabStop = false;
      this.groupBoxGroups.Text = "Groups (0):";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(233, 2);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 18);
      this.label2.TabIndex = 4;
      this.label2.Text = "[Double Click]";
      // 
      // btnRemoveGroup
      // 
      this.btnRemoveGroup.Enabled = false;
      this.btnRemoveGroup.Location = new System.Drawing.Point(204, 480);
      this.btnRemoveGroup.Name = "btnRemoveGroup";
      this.btnRemoveGroup.Size = new System.Drawing.Size(138, 39);
      this.btnRemoveGroup.TabIndex = 3;
      this.btnRemoveGroup.Text = "Remove";
      this.btnRemoveGroup.UseVisualStyleBackColor = true;
      this.btnRemoveGroup.Click += new System.EventHandler(this.btnRemoveGroup_Click);
      // 
      // btnAddGroup
      // 
      this.btnAddGroup.Location = new System.Drawing.Point(7, 480);
      this.btnAddGroup.Name = "btnAddGroup";
      this.btnAddGroup.Size = new System.Drawing.Size(137, 39);
      this.btnAddGroup.TabIndex = 2;
      this.btnAddGroup.Text = "Add";
      this.btnAddGroup.UseVisualStyleBackColor = true;
      this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
      // 
      // textBoxGroup
      // 
      this.textBoxGroup.Location = new System.Drawing.Point(7, 443);
      this.textBoxGroup.Name = "textBoxGroup";
      this.textBoxGroup.Size = new System.Drawing.Size(335, 30);
      this.textBoxGroup.TabIndex = 1;
      // 
      // listViewGroups
      // 
      this.listViewGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.listViewGroups.FullRowSelect = true;
      this.listViewGroups.GridLines = true;
      this.listViewGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewGroups.HideSelection = false;
      this.listViewGroups.Location = new System.Drawing.Point(6, 29);
      this.listViewGroups.MultiSelect = false;
      this.listViewGroups.Name = "listViewGroups";
      this.listViewGroups.ShowItemToolTips = true;
      this.listViewGroups.Size = new System.Drawing.Size(336, 407);
      this.listViewGroups.TabIndex = 0;
      this.listViewGroups.UseCompatibleStateImageBehavior = false;
      this.listViewGroups.View = System.Windows.Forms.View.Details;
      this.listViewGroups.SelectedIndexChanged += new System.EventHandler(this.listViewGroups_SelectedIndexChanged);
      this.listViewGroups.DoubleClick += new System.EventHandler(this.listViewGroups_DoubleClick);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Group";
      this.columnHeader1.Width = 220;
      // 
      // groupBoxWordsInGroup
      // 
      this.groupBoxWordsInGroup.Controls.Add(this.btnRemoveWord);
      this.groupBoxWordsInGroup.Controls.Add(this.btnAddWord);
      this.groupBoxWordsInGroup.Controls.Add(this.textBoxWord);
      this.groupBoxWordsInGroup.Controls.Add(this.listViewWordsInGroup);
      this.groupBoxWordsInGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxWordsInGroup.Location = new System.Drawing.Point(386, 12);
      this.groupBoxWordsInGroup.Name = "groupBoxWordsInGroup";
      this.groupBoxWordsInGroup.Size = new System.Drawing.Size(379, 527);
      this.groupBoxWordsInGroup.TabIndex = 2;
      this.groupBoxWordsInGroup.TabStop = false;
      this.groupBoxWordsInGroup.Text = "Words in Group (0):";
      // 
      // btnRemoveWord
      // 
      this.btnRemoveWord.Enabled = false;
      this.btnRemoveWord.Location = new System.Drawing.Point(235, 479);
      this.btnRemoveWord.Name = "btnRemoveWord";
      this.btnRemoveWord.Size = new System.Drawing.Size(138, 39);
      this.btnRemoveWord.TabIndex = 3;
      this.btnRemoveWord.Text = "Remove";
      this.btnRemoveWord.UseVisualStyleBackColor = true;
      this.btnRemoveWord.Click += new System.EventHandler(this.btnRemoveWord_Click);
      // 
      // btnAddWord
      // 
      this.btnAddWord.Enabled = false;
      this.btnAddWord.Location = new System.Drawing.Point(7, 480);
      this.btnAddWord.Name = "btnAddWord";
      this.btnAddWord.Size = new System.Drawing.Size(137, 39);
      this.btnAddWord.TabIndex = 2;
      this.btnAddWord.Text = "Add";
      this.btnAddWord.UseVisualStyleBackColor = true;
      this.btnAddWord.Click += new System.EventHandler(this.btnAddWord_Click);
      // 
      // textBoxWord
      // 
      this.textBoxWord.Location = new System.Drawing.Point(7, 443);
      this.textBoxWord.Name = "textBoxWord";
      this.textBoxWord.Size = new System.Drawing.Size(366, 30);
      this.textBoxWord.TabIndex = 1;
      // 
      // listViewWordsInGroup
      // 
      this.listViewWordsInGroup.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
      this.listViewWordsInGroup.FullRowSelect = true;
      this.listViewWordsInGroup.GridLines = true;
      this.listViewWordsInGroup.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewWordsInGroup.HideSelection = false;
      this.listViewWordsInGroup.Location = new System.Drawing.Point(6, 29);
      this.listViewWordsInGroup.MultiSelect = false;
      this.listViewWordsInGroup.Name = "listViewWordsInGroup";
      this.listViewWordsInGroup.ShowItemToolTips = true;
      this.listViewWordsInGroup.Size = new System.Drawing.Size(367, 407);
      this.listViewWordsInGroup.TabIndex = 0;
      this.listViewWordsInGroup.UseCompatibleStateImageBehavior = false;
      this.listViewWordsInGroup.View = System.Windows.Forms.View.Details;
      this.listViewWordsInGroup.SelectedIndexChanged += new System.EventHandler(this.listViewWordsInGroup_SelectedIndexChanged);
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Word";
      this.columnHeader2.Width = 240;
      // 
      // groupBoxWordsInDatabase
      // 
      this.groupBoxWordsInDatabase.Controls.Add(this.label3);
      this.groupBoxWordsInDatabase.Controls.Add(this.btnLoadWords);
      this.groupBoxWordsInDatabase.Controls.Add(this.listViewWordsInDatabase);
      this.groupBoxWordsInDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxWordsInDatabase.Location = new System.Drawing.Point(924, 7);
      this.groupBoxWordsInDatabase.Name = "groupBoxWordsInDatabase";
      this.groupBoxWordsInDatabase.Size = new System.Drawing.Size(383, 532);
      this.groupBoxWordsInDatabase.TabIndex = 3;
      this.groupBoxWordsInDatabase.TabStop = false;
      this.groupBoxWordsInDatabase.Text = "Words in DB (0):";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(270, 4);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(100, 18);
      this.label3.TabIndex = 4;
      this.label3.Text = "[Double Click]";
      // 
      // btnLoadWords
      // 
      this.btnLoadWords.Location = new System.Drawing.Point(128, 481);
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
            this.columnHeader3});
      this.listViewWordsInDatabase.FullRowSelect = true;
      this.listViewWordsInDatabase.GridLines = true;
      this.listViewWordsInDatabase.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewWordsInDatabase.HideSelection = false;
      this.listViewWordsInDatabase.Location = new System.Drawing.Point(6, 29);
      this.listViewWordsInDatabase.MultiSelect = false;
      this.listViewWordsInDatabase.Name = "listViewWordsInDatabase";
      this.listViewWordsInDatabase.ShowItemToolTips = true;
      this.listViewWordsInDatabase.Size = new System.Drawing.Size(371, 446);
      this.listViewWordsInDatabase.TabIndex = 0;
      this.listViewWordsInDatabase.UseCompatibleStateImageBehavior = false;
      this.listViewWordsInDatabase.View = System.Windows.Forms.View.Details;
      this.listViewWordsInDatabase.DoubleClick += new System.EventHandler(this.listViewWordsInDatabase_DoubleClick);
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Word";
      this.columnHeader3.Width = 240;
      // 
      // EditGroupsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1310, 546);
      this.Controls.Add(this.groupBoxWordsInDatabase);
      this.Controls.Add(this.groupBoxWordsInGroup);
      this.Controls.Add(this.groupBoxGroups);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "EditGroupsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Edit Groups";
      this.Shown += new System.EventHandler(this.EditGroupsDialog_Shown);
      this.groupBoxGroups.ResumeLayout(false);
      this.groupBoxGroups.PerformLayout();
      this.groupBoxWordsInGroup.ResumeLayout(false);
      this.groupBoxWordsInGroup.PerformLayout();
      this.groupBoxWordsInDatabase.ResumeLayout(false);
      this.groupBoxWordsInDatabase.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.GroupBox groupBoxGroups;
    private System.Windows.Forms.Button btnRemoveGroup;
    private System.Windows.Forms.Button btnAddGroup;
    public System.Windows.Forms.TextBox textBoxGroup;
    public System.Windows.Forms.ListView listViewGroups;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    public System.Windows.Forms.GroupBox groupBoxWordsInGroup;
    private System.Windows.Forms.Button btnRemoveWord;
    public System.Windows.Forms.Button btnAddWord;
    public System.Windows.Forms.TextBox textBoxWord;
    public System.Windows.Forms.ListView listViewWordsInGroup;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    public System.Windows.Forms.GroupBox groupBoxWordsInDatabase;
    public System.Windows.Forms.ListView listViewWordsInDatabase;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    public System.Windows.Forms.Button btnLoadWords;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
  }
}