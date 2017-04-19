namespace application {
  partial class EditPhrasesDialog {
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
      this.groupBoxPhrases = new System.Windows.Forms.GroupBox();
      this.richTextBoxContents = new System.Windows.Forms.RichTextBox();
      this.listViewPhrases = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnRemove = new System.Windows.Forms.Button();
      this.groupBoxAdd = new System.Windows.Forms.GroupBox();
      this.btnAdd = new System.Windows.Forms.Button();
      this.richTextBoxAdd = new System.Windows.Forms.RichTextBox();
      this.groupBoxPhrases.SuspendLayout();
      this.groupBoxAdd.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxPhrases
      // 
      this.groupBoxPhrases.Controls.Add(this.richTextBoxContents);
      this.groupBoxPhrases.Controls.Add(this.listViewPhrases);
      this.groupBoxPhrases.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxPhrases.Location = new System.Drawing.Point(12, 12);
      this.groupBoxPhrases.Name = "groupBoxPhrases";
      this.groupBoxPhrases.Size = new System.Drawing.Size(852, 442);
      this.groupBoxPhrases.TabIndex = 2;
      this.groupBoxPhrases.TabStop = false;
      this.groupBoxPhrases.Text = "Phrases (0):";
      // 
      // richTextBoxContents
      // 
      this.richTextBoxContents.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.richTextBoxContents.Location = new System.Drawing.Point(6, 290);
      this.richTextBoxContents.Name = "richTextBoxContents";
      this.richTextBoxContents.ReadOnly = true;
      this.richTextBoxContents.Size = new System.Drawing.Size(837, 146);
      this.richTextBoxContents.TabIndex = 1;
      this.richTextBoxContents.Text = "";
      // 
      // listViewPhrases
      // 
      this.listViewPhrases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.listViewPhrases.FullRowSelect = true;
      this.listViewPhrases.GridLines = true;
      this.listViewPhrases.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewPhrases.HideSelection = false;
      this.listViewPhrases.Location = new System.Drawing.Point(6, 29);
      this.listViewPhrases.MultiSelect = false;
      this.listViewPhrases.Name = "listViewPhrases";
      this.listViewPhrases.ShowItemToolTips = true;
      this.listViewPhrases.Size = new System.Drawing.Size(837, 255);
      this.listViewPhrases.TabIndex = 0;
      this.listViewPhrases.UseCompatibleStateImageBehavior = false;
      this.listViewPhrases.View = System.Windows.Forms.View.Details;
      this.listViewPhrases.SelectedIndexChanged += new System.EventHandler(this.listViewGroups_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Phrase";
      this.columnHeader1.Width = 600;
      // 
      // btnRemove
      // 
      this.btnRemove.Enabled = false;
      this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnRemove.Location = new System.Drawing.Point(703, 469);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(152, 34);
      this.btnRemove.TabIndex = 10;
      this.btnRemove.Text = "Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // groupBoxAdd
      // 
      this.groupBoxAdd.Controls.Add(this.btnAdd);
      this.groupBoxAdd.Controls.Add(this.richTextBoxAdd);
      this.groupBoxAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxAdd.Location = new System.Drawing.Point(12, 509);
      this.groupBoxAdd.Name = "groupBoxAdd";
      this.groupBoxAdd.Size = new System.Drawing.Size(852, 191);
      this.groupBoxAdd.TabIndex = 11;
      this.groupBoxAdd.TabStop = false;
      this.groupBoxAdd.Text = "Add Phrase:";
      // 
      // btnAdd
      // 
      this.btnAdd.Enabled = false;
      this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnAdd.Location = new System.Drawing.Point(691, 145);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(152, 34);
      this.btnAdd.TabIndex = 11;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // richTextBoxAdd
      // 
      this.richTextBoxAdd.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.richTextBoxAdd.Location = new System.Drawing.Point(6, 29);
      this.richTextBoxAdd.Name = "richTextBoxAdd";
      this.richTextBoxAdd.Size = new System.Drawing.Size(837, 110);
      this.richTextBoxAdd.TabIndex = 1;
      this.richTextBoxAdd.Text = "";
      this.richTextBoxAdd.TextChanged += new System.EventHandler(this.richTextBoxAdd_TextChanged);
      // 
      // EditPhrasesDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(873, 704);
      this.Controls.Add(this.groupBoxAdd);
      this.Controls.Add(this.btnRemove);
      this.Controls.Add(this.groupBoxPhrases);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "EditPhrasesDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Edit Phrases";
      this.Shown += new System.EventHandler(this.EditPhrasesDialog_Shown);
      this.groupBoxPhrases.ResumeLayout(false);
      this.groupBoxAdd.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.GroupBox groupBoxPhrases;
    public System.Windows.Forms.ListView listViewPhrases;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    public System.Windows.Forms.RichTextBox richTextBoxContents;
    private System.Windows.Forms.Button btnRemove;
    public System.Windows.Forms.GroupBox groupBoxAdd;
    private System.Windows.Forms.Button btnAdd;
    public System.Windows.Forms.RichTextBox richTextBoxAdd;
  }
}