namespace application {
  partial class MainDialog {
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
      this.btnInsertDocuments = new System.Windows.Forms.Button();
      this.btnQueryDocuments = new System.Windows.Forms.Button();
      this.btnImport = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.lblStatsGroups = new System.Windows.Forms.Label();
      this.lblStatsIndexedWords = new System.Windows.Forms.Label();
      this.lblStatsUniqueWords = new System.Windows.Forms.Label();
      this.lblStatsDocuments = new System.Windows.Forms.Label();
      this.btnWordsInspector = new System.Windows.Forms.Button();
      this.btnWordsQuery = new System.Windows.Forms.Button();
      this.btnEditGroups = new System.Windows.Forms.Button();
      this.btnGroupsInspector = new System.Windows.Forms.Button();
      this.btnEditRelations = new System.Windows.Forms.Button();
      this.btnEditPhrases = new System.Windows.Forms.Button();
      this.btnPhrasesInspector = new System.Windows.Forms.Button();
      this.btnExport = new System.Windows.Forms.Button();
      this.btnResetDB = new System.Windows.Forms.Button();
      this.lblStatsRelations = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnInsertDocuments
      // 
      this.btnInsertDocuments.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnInsertDocuments.Location = new System.Drawing.Point(24, 23);
      this.btnInsertDocuments.Name = "btnInsertDocuments";
      this.btnInsertDocuments.Size = new System.Drawing.Size(181, 59);
      this.btnInsertDocuments.TabIndex = 0;
      this.btnInsertDocuments.Text = "Insert Documents...";
      this.btnInsertDocuments.UseVisualStyleBackColor = true;
      this.btnInsertDocuments.Click += new System.EventHandler(this.btnInsertDocuments_Click);
      // 
      // btnQueryDocuments
      // 
      this.btnQueryDocuments.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnQueryDocuments.Location = new System.Drawing.Point(24, 245);
      this.btnQueryDocuments.Name = "btnQueryDocuments";
      this.btnQueryDocuments.Size = new System.Drawing.Size(181, 59);
      this.btnQueryDocuments.TabIndex = 1;
      this.btnQueryDocuments.Text = "Documents Query...";
      this.btnQueryDocuments.UseVisualStyleBackColor = true;
      this.btnQueryDocuments.Click += new System.EventHandler(this.btnQueryDocuments_Click);
      // 
      // btnImport
      // 
      this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnImport.Location = new System.Drawing.Point(664, 320);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(169, 59);
      this.btnImport.TabIndex = 2;
      this.btnImport.Text = "Import DB...";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.lblStatsRelations);
      this.groupBox1.Controls.Add(this.linkLabel1);
      this.groupBox1.Controls.Add(this.lblStatsGroups);
      this.groupBox1.Controls.Add(this.lblStatsIndexedWords);
      this.groupBox1.Controls.Add(this.lblStatsUniqueWords);
      this.groupBox1.Controls.Add(this.lblStatsDocuments);
      this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox1.Location = new System.Drawing.Point(631, 23);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(377, 200);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Stats:";
      // 
      // linkLabel1
      // 
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.linkLabel1.Location = new System.Drawing.Point(272, 1);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new System.Drawing.Size(91, 18);
      this.linkLabel1.TabIndex = 13;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "More Stats";
      this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      // 
      // lblStatsGroups
      // 
      this.lblStatsGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatsGroups.Location = new System.Drawing.Point(76, 135);
      this.lblStatsGroups.Name = "lblStatsGroups";
      this.lblStatsGroups.Size = new System.Drawing.Size(287, 25);
      this.lblStatsGroups.TabIndex = 3;
      this.lblStatsGroups.Text = "Groups: 0";
      // 
      // lblStatsIndexedWords
      // 
      this.lblStatsIndexedWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatsIndexedWords.Location = new System.Drawing.Point(7, 101);
      this.lblStatsIndexedWords.Name = "lblStatsIndexedWords";
      this.lblStatsIndexedWords.Size = new System.Drawing.Size(356, 25);
      this.lblStatsIndexedWords.TabIndex = 2;
      this.lblStatsIndexedWords.Text = "Indexed Words: 0";
      // 
      // lblStatsUniqueWords
      // 
      this.lblStatsUniqueWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatsUniqueWords.Location = new System.Drawing.Point(15, 67);
      this.lblStatsUniqueWords.Name = "lblStatsUniqueWords";
      this.lblStatsUniqueWords.Size = new System.Drawing.Size(348, 25);
      this.lblStatsUniqueWords.TabIndex = 1;
      this.lblStatsUniqueWords.Text = "Unique Words: 0";
      // 
      // lblStatsDocuments
      // 
      this.lblStatsDocuments.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatsDocuments.Location = new System.Drawing.Point(41, 34);
      this.lblStatsDocuments.Name = "lblStatsDocuments";
      this.lblStatsDocuments.Size = new System.Drawing.Size(322, 25);
      this.lblStatsDocuments.TabIndex = 0;
      this.lblStatsDocuments.Text = "Documents: 0";
      // 
      // btnWordsInspector
      // 
      this.btnWordsInspector.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnWordsInspector.Location = new System.Drawing.Point(24, 321);
      this.btnWordsInspector.Name = "btnWordsInspector";
      this.btnWordsInspector.Size = new System.Drawing.Size(181, 59);
      this.btnWordsInspector.TabIndex = 4;
      this.btnWordsInspector.Text = "Words Inspector...";
      this.btnWordsInspector.UseVisualStyleBackColor = true;
      this.btnWordsInspector.Click += new System.EventHandler(this.btnWordsInspector_Click);
      // 
      // btnWordsQuery
      // 
      this.btnWordsQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnWordsQuery.Location = new System.Drawing.Point(219, 245);
      this.btnWordsQuery.Name = "btnWordsQuery";
      this.btnWordsQuery.Size = new System.Drawing.Size(181, 59);
      this.btnWordsQuery.TabIndex = 5;
      this.btnWordsQuery.Text = "Words Query...";
      this.btnWordsQuery.UseVisualStyleBackColor = true;
      this.btnWordsQuery.Click += new System.EventHandler(this.btnWordsQuery_Click);
      // 
      // btnEditGroups
      // 
      this.btnEditGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnEditGroups.Location = new System.Drawing.Point(219, 23);
      this.btnEditGroups.Name = "btnEditGroups";
      this.btnEditGroups.Size = new System.Drawing.Size(181, 59);
      this.btnEditGroups.TabIndex = 6;
      this.btnEditGroups.Text = "Edit Groups...";
      this.btnEditGroups.UseVisualStyleBackColor = true;
      this.btnEditGroups.Click += new System.EventHandler(this.btnEditGroups_Click);
      // 
      // btnGroupsInspector
      // 
      this.btnGroupsInspector.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnGroupsInspector.Location = new System.Drawing.Point(219, 321);
      this.btnGroupsInspector.Name = "btnGroupsInspector";
      this.btnGroupsInspector.Size = new System.Drawing.Size(181, 59);
      this.btnGroupsInspector.TabIndex = 7;
      this.btnGroupsInspector.Text = "Groups Inspector...";
      this.btnGroupsInspector.UseVisualStyleBackColor = true;
      this.btnGroupsInspector.Click += new System.EventHandler(this.btnGroupsInspector_Click);
      // 
      // btnEditRelations
      // 
      this.btnEditRelations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnEditRelations.Location = new System.Drawing.Point(414, 23);
      this.btnEditRelations.Name = "btnEditRelations";
      this.btnEditRelations.Size = new System.Drawing.Size(181, 59);
      this.btnEditRelations.TabIndex = 8;
      this.btnEditRelations.Text = "Edit Relations...";
      this.btnEditRelations.UseVisualStyleBackColor = true;
      this.btnEditRelations.Click += new System.EventHandler(this.btnEditRelations_Click);
      // 
      // btnEditPhrases
      // 
      this.btnEditPhrases.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnEditPhrases.Location = new System.Drawing.Point(219, 96);
      this.btnEditPhrases.Name = "btnEditPhrases";
      this.btnEditPhrases.Size = new System.Drawing.Size(181, 59);
      this.btnEditPhrases.TabIndex = 9;
      this.btnEditPhrases.Text = "Edit Phrases...";
      this.btnEditPhrases.UseVisualStyleBackColor = true;
      this.btnEditPhrases.Click += new System.EventHandler(this.btnEditPhrases_Click);
      // 
      // btnPhrasesInspector
      // 
      this.btnPhrasesInspector.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnPhrasesInspector.Location = new System.Drawing.Point(414, 321);
      this.btnPhrasesInspector.Name = "btnPhrasesInspector";
      this.btnPhrasesInspector.Size = new System.Drawing.Size(181, 59);
      this.btnPhrasesInspector.TabIndex = 10;
      this.btnPhrasesInspector.Text = "Phrases Inspector...";
      this.btnPhrasesInspector.UseVisualStyleBackColor = true;
      this.btnPhrasesInspector.Click += new System.EventHandler(this.btnPhrasesInspector_Click);
      // 
      // btnExport
      // 
      this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnExport.Location = new System.Drawing.Point(664, 245);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(169, 59);
      this.btnExport.TabIndex = 11;
      this.btnExport.Text = "Export DB...";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // btnResetDB
      // 
      this.btnResetDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnResetDB.Location = new System.Drawing.Point(839, 320);
      this.btnResetDB.Name = "btnResetDB";
      this.btnResetDB.Size = new System.Drawing.Size(169, 59);
      this.btnResetDB.TabIndex = 12;
      this.btnResetDB.Text = "Reset DB...";
      this.btnResetDB.UseVisualStyleBackColor = true;
      this.btnResetDB.Click += new System.EventHandler(this.btnDropDB_Click);
      // 
      // lblStatsRelations
      // 
      this.lblStatsRelations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblStatsRelations.Location = new System.Drawing.Point(60, 168);
      this.lblStatsRelations.Name = "lblStatsRelations";
      this.lblStatsRelations.Size = new System.Drawing.Size(303, 25);
      this.lblStatsRelations.TabIndex = 14;
      this.lblStatsRelations.Text = "Relations: 0";
      // 
      // MainDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1018, 391);
      this.Controls.Add(this.btnResetDB);
      this.Controls.Add(this.btnExport);
      this.Controls.Add(this.btnPhrasesInspector);
      this.Controls.Add(this.btnEditPhrases);
      this.Controls.Add(this.btnEditRelations);
      this.Controls.Add(this.btnGroupsInspector);
      this.Controls.Add(this.btnEditGroups);
      this.Controls.Add(this.btnWordsQuery);
      this.Controls.Add(this.btnWordsInspector);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnImport);
      this.Controls.Add(this.btnQueryDocuments);
      this.Controls.Add(this.btnInsertDocuments);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "MainDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Book Concordance - Main Menu";
      this.Shown += new System.EventHandler(this.Main_Shown);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnInsertDocuments;
    private System.Windows.Forms.Button btnQueryDocuments;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label lblStatsIndexedWords;
    private System.Windows.Forms.Label lblStatsUniqueWords;
    private System.Windows.Forms.Label lblStatsDocuments;
    private System.Windows.Forms.Button btnWordsInspector;
    private System.Windows.Forms.Button btnWordsQuery;
    private System.Windows.Forms.Label lblStatsGroups;
    private System.Windows.Forms.Button btnEditGroups;
    private System.Windows.Forms.Button btnGroupsInspector;
    private System.Windows.Forms.Button btnEditRelations;
    private System.Windows.Forms.Button btnEditPhrases;
    private System.Windows.Forms.Button btnPhrasesInspector;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnResetDB;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.Label lblStatsRelations;
  }
}

