namespace CIMProfileLoader
{
    partial class CIMProfileLoaderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelCIMProfile = new System.Windows.Forms.Label();
            this.textBoxCIMProfile = new System.Windows.Forms.TextBox();
            this.richTextBoxReport = new System.Windows.Forms.RichTextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonLoadProfile = new System.Windows.Forms.Button();
            this.comboBoxRDFGraphs = new System.Windows.Forms.ComboBox();
            this.labelRDFGraph = new System.Windows.Forms.Label();
            this.comboBoxRDFDatasets = new System.Windows.Forms.ComboBox();
            this.labelRDFDataset = new System.Windows.Forms.Label();
            this.buttonApplyRDFSInferenceOverDataset = new System.Windows.Forms.Button();
            this.labelRDFXMLFile = new System.Windows.Forms.Label();
            this.textBoxRDFFile = new System.Windows.Forms.TextBox();
            this.buttonBrowseRDF = new System.Windows.Forms.Button();
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.buttonListGraphs = new System.Windows.Forms.Button();
            this.buttonStartJenaStorage = new System.Windows.Forms.Button();
            this.buttonStopJenaStorage = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.labelSPO = new System.Windows.Forms.Label();
            this.buttonLoadGraph = new System.Windows.Forms.Button();
            this.textBoxTripleSubject = new System.Windows.Forms.TextBox();
            this.textBoxTriplePredicate = new System.Windows.Forms.TextBox();
            this.textBoxTripleObject = new System.Windows.Forms.TextBox();
            this.checkBoxAddTriple = new System.Windows.Forms.CheckBox();
            this.buttonUpdateGraph = new System.Windows.Forms.Button();
            this.labelQuery = new System.Windows.Forms.Label();
            this.buttonDeleteGraph = new System.Windows.Forms.Button();
            this.checkBoxDeleteTriple = new System.Windows.Forms.CheckBox();
            this.buttonSaveGraph = new System.Windows.Forms.Button();
            this.labelQueryClassAttInfo = new System.Windows.Forms.Label();
            this.buttonQueryNumOfClasses = new System.Windows.Forms.Button();
            this.textBoxQueryClassAttInfo = new System.Windows.Forms.TextBox();
            this.labelGetConductingEquipment = new System.Windows.Forms.Label();
            this.buttonQueryAllClassesAttr = new System.Windows.Forms.Button();
            this.comboBoxQueryTypeOfEquipment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonQueryGetTypeOfEquipment = new System.Windows.Forms.Button();
            this.textBoxQueryConnectivityNode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonQueryConnectivityNodeComponents = new System.Windows.Forms.Button();
            this.textBoxQueryComponentID = new System.Windows.Forms.TextBox();
            this.labelFilePreview = new System.Windows.Forms.Label();
            this.buttonQueryGetComponentInfo = new System.Windows.Forms.Button();
            this.labelApplyResetConfig = new System.Windows.Forms.Label();
            this.buttonResetConfig = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelCIMProfile, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxCIMProfile, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxReport, 1, 18);
            this.tableLayoutPanel1.Controls.Add(this.buttonBrowse, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonLoadProfile, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxRDFGraphs, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelRDFGraph, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxRDFDatasets, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelRDFDataset, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.buttonApplyRDFSInferenceOverDataset, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelRDFXMLFile, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRDFFile, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonBrowseRDF, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonLoadData, 5, 6);
            this.tableLayoutPanel1.Controls.Add(this.buttonListGraphs, 5, 7);
            this.tableLayoutPanel1.Controls.Add(this.buttonStartJenaStorage, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonStopJenaStorage, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonExit, 5, 21);
            this.tableLayoutPanel1.Controls.Add(this.labelSPO, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.buttonLoadGraph, 5, 8);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTripleSubject, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTriplePredicate, 3, 9);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTripleObject, 4, 9);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAddTriple, 3, 10);
            this.tableLayoutPanel1.Controls.Add(this.buttonUpdateGraph, 5, 9);
            this.tableLayoutPanel1.Controls.Add(this.labelQuery, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.buttonDeleteGraph, 5, 11);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxDeleteTriple, 4, 10);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveGraph, 5, 12);
            this.tableLayoutPanel1.Controls.Add(this.labelQueryClassAttInfo, 1, 14);
            this.tableLayoutPanel1.Controls.Add(this.buttonQueryNumOfClasses, 5, 13);
            this.tableLayoutPanel1.Controls.Add(this.textBoxQueryClassAttInfo, 2, 14);
            this.tableLayoutPanel1.Controls.Add(this.labelGetConductingEquipment, 1, 15);
            this.tableLayoutPanel1.Controls.Add(this.buttonQueryAllClassesAttr, 5, 14);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxQueryTypeOfEquipment, 2, 15);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 16);
            this.tableLayoutPanel1.Controls.Add(this.buttonQueryGetTypeOfEquipment, 5, 15);
            this.tableLayoutPanel1.Controls.Add(this.textBoxQueryConnectivityNode, 2, 16);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 17);
            this.tableLayoutPanel1.Controls.Add(this.buttonQueryConnectivityNodeComponents, 5, 16);
            this.tableLayoutPanel1.Controls.Add(this.textBoxQueryComponentID, 2, 17);
            this.tableLayoutPanel1.Controls.Add(this.labelFilePreview, 0, 18);
            this.tableLayoutPanel1.Controls.Add(this.buttonQueryGetComponentInfo, 5, 17);
            this.tableLayoutPanel1.Controls.Add(this.labelApplyResetConfig, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonResetConfig, 4, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, -1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 22;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(863, 1062);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelTitle, 2);
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(5, 10);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(5, 10, 3, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(369, 13);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Select CIM Profile definition in RDFS/RDFS-Augmented format :";
            // 
            // labelCIMProfile
            // 
            this.labelCIMProfile.AutoSize = true;
            this.labelCIMProfile.Location = new System.Drawing.Point(20, 45);
            this.labelCIMProfile.Margin = new System.Windows.Forms.Padding(20, 12, 3, 5);
            this.labelCIMProfile.Name = "labelCIMProfile";
            this.labelCIMProfile.Size = new System.Drawing.Size(64, 13);
            this.labelCIMProfile.TabIndex = 1;
            this.labelCIMProfile.Text = "CIM Profile :";
            this.labelCIMProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxCIMProfile
            // 
            this.textBoxCIMProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxCIMProfile, 4);
            this.textBoxCIMProfile.Location = new System.Drawing.Point(126, 43);
            this.textBoxCIMProfile.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.textBoxCIMProfile.Name = "textBoxCIMProfile";
            this.textBoxCIMProfile.Size = new System.Drawing.Size(625, 20);
            this.textBoxCIMProfile.TabIndex = 2;
            this.textBoxCIMProfile.WordWrap = false;
            // 
            // richTextBoxReport
            // 
            this.richTextBoxReport.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBoxReport, 4);
            this.richTextBoxReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxReport.Location = new System.Drawing.Point(126, 774);
            this.richTextBoxReport.Name = "richTextBoxReport";
            this.richTextBoxReport.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.richTextBoxReport, 3);
            this.richTextBoxReport.Size = new System.Drawing.Size(625, 247);
            this.richTextBoxReport.TabIndex = 9;
            this.richTextBoxReport.Text = "";
            this.richTextBoxReport.WordWrap = false;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBrowse.Location = new System.Drawing.Point(764, 43);
            this.buttonBrowse.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(84, 23);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "Browse..";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonLoadProfile
            // 
            this.buttonLoadProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonLoadProfile.Location = new System.Drawing.Point(764, 81);
            this.buttonLoadProfile.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonLoadProfile.Name = "buttonLoadProfile";
            this.buttonLoadProfile.Size = new System.Drawing.Size(84, 23);
            this.buttonLoadProfile.TabIndex = 4;
            this.buttonLoadProfile.Text = "Load Profile";
            this.buttonLoadProfile.UseVisualStyleBackColor = true;
            this.buttonLoadProfile.Click += new System.EventHandler(this.buttonLoadProfile_Click);
            // 
            // comboBoxRDFGraphs
            // 
            this.comboBoxRDFGraphs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRDFGraphs.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxRDFGraphs, 4);
            this.comboBoxRDFGraphs.Cursor = System.Windows.Forms.Cursors.Default;
            this.comboBoxRDFGraphs.Enabled = false;
            this.comboBoxRDFGraphs.ForeColor = System.Drawing.SystemColors.Window;
            this.comboBoxRDFGraphs.FormattingEnabled = true;
            this.comboBoxRDFGraphs.Location = new System.Drawing.Point(126, 359);
            this.comboBoxRDFGraphs.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.comboBoxRDFGraphs.Name = "comboBoxRDFGraphs";
            this.comboBoxRDFGraphs.Size = new System.Drawing.Size(625, 21);
            this.comboBoxRDFGraphs.TabIndex = 13;
            this.comboBoxRDFGraphs.SelectedIndexChanged += new System.EventHandler(this.comboBoxRDFGraphs_SelectedIndexChanged);
            // 
            // labelRDFGraph
            // 
            this.labelRDFGraph.AutoSize = true;
            this.labelRDFGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRDFGraph.Location = new System.Drawing.Point(20, 361);
            this.labelRDFGraph.Margin = new System.Windows.Forms.Padding(20, 12, 3, 5);
            this.labelRDFGraph.Name = "labelRDFGraph";
            this.labelRDFGraph.Size = new System.Drawing.Size(78, 13);
            this.labelRDFGraph.TabIndex = 14;
            this.labelRDFGraph.Text = "RDF Graph :";
            // 
            // comboBoxRDFDatasets
            // 
            this.comboBoxRDFDatasets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRDFDatasets.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxRDFDatasets, 4);
            this.comboBoxRDFDatasets.ForeColor = System.Drawing.SystemColors.Window;
            this.comboBoxRDFDatasets.FormattingEnabled = true;
            this.comboBoxRDFDatasets.Location = new System.Drawing.Point(126, 319);
            this.comboBoxRDFDatasets.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.comboBoxRDFDatasets.Name = "comboBoxRDFDatasets";
            this.comboBoxRDFDatasets.Size = new System.Drawing.Size(625, 21);
            this.comboBoxRDFDatasets.TabIndex = 27;
            this.comboBoxRDFDatasets.SelectedIndexChanged += new System.EventHandler(this.comboBoxRDFDatasets_SelectedIndexChanged);
            // 
            // labelRDFDataset
            // 
            this.labelRDFDataset.AutoSize = true;
            this.labelRDFDataset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRDFDataset.Location = new System.Drawing.Point(20, 321);
            this.labelRDFDataset.Margin = new System.Windows.Forms.Padding(20, 12, 3, 5);
            this.labelRDFDataset.Name = "labelRDFDataset";
            this.labelRDFDataset.Size = new System.Drawing.Size(100, 13);
            this.labelRDFDataset.TabIndex = 26;
            this.labelRDFDataset.Text = "Fuseki Dataset :";
            // 
            // buttonApplyRDFSInferenceOverDataset
            // 
            this.buttonApplyRDFSInferenceOverDataset.Location = new System.Drawing.Point(764, 119);
            this.buttonApplyRDFSInferenceOverDataset.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonApplyRDFSInferenceOverDataset.Name = "buttonApplyRDFSInferenceOverDataset";
            this.buttonApplyRDFSInferenceOverDataset.Size = new System.Drawing.Size(84, 60);
            this.buttonApplyRDFSInferenceOverDataset.TabIndex = 29;
            this.buttonApplyRDFSInferenceOverDataset.Text = "Apply loaded RDFS ontology over fuseki dataset";
            this.buttonApplyRDFSInferenceOverDataset.UseVisualStyleBackColor = true;
            this.buttonApplyRDFSInferenceOverDataset.Click += new System.EventHandler(this.buttonApplyRDFSInferenceOverDataset_Click);
            // 
            // labelRDFXMLFile
            // 
            this.labelRDFXMLFile.AutoSize = true;
            this.labelRDFXMLFile.Location = new System.Drawing.Point(20, 245);
            this.labelRDFXMLFile.Margin = new System.Windows.Forms.Padding(20, 12, 3, 5);
            this.labelRDFXMLFile.Name = "labelRDFXMLFile";
            this.labelRDFXMLFile.Size = new System.Drawing.Size(78, 13);
            this.labelRDFXMLFile.TabIndex = 5;
            this.labelRDFXMLFile.Text = "RDF/XML file :";
            // 
            // textBoxRDFFile
            // 
            this.textBoxRDFFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxRDFFile, 4);
            this.textBoxRDFFile.Location = new System.Drawing.Point(126, 243);
            this.textBoxRDFFile.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.textBoxRDFFile.Name = "textBoxRDFFile";
            this.textBoxRDFFile.Size = new System.Drawing.Size(625, 20);
            this.textBoxRDFFile.TabIndex = 6;
            this.textBoxRDFFile.WordWrap = false;
            // 
            // buttonBrowseRDF
            // 
            this.buttonBrowseRDF.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBrowseRDF.Location = new System.Drawing.Point(764, 243);
            this.buttonBrowseRDF.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonBrowseRDF.Name = "buttonBrowseRDF";
            this.buttonBrowseRDF.Size = new System.Drawing.Size(84, 23);
            this.buttonBrowseRDF.TabIndex = 7;
            this.buttonBrowseRDF.Text = "Browse..";
            this.buttonBrowseRDF.UseVisualStyleBackColor = true;
            this.buttonBrowseRDF.Click += new System.EventHandler(this.buttonBrowseRDF_Click);
            // 
            // buttonLoadData
            // 
            this.buttonLoadData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonLoadData.Location = new System.Drawing.Point(764, 281);
            this.buttonLoadData.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonLoadData.Name = "buttonLoadData";
            this.buttonLoadData.Size = new System.Drawing.Size(84, 23);
            this.buttonLoadData.TabIndex = 8;
            this.buttonLoadData.Text = "Load Data";
            this.buttonLoadData.UseVisualStyleBackColor = true;
            this.buttonLoadData.Click += new System.EventHandler(this.buttonLoadRDF_Click);
            // 
            // buttonListGraphs
            // 
            this.buttonListGraphs.Location = new System.Drawing.Point(764, 321);
            this.buttonListGraphs.Margin = new System.Windows.Forms.Padding(10, 12, 15, 5);
            this.buttonListGraphs.Name = "buttonListGraphs";
            this.buttonListGraphs.Size = new System.Drawing.Size(84, 23);
            this.buttonListGraphs.TabIndex = 28;
            this.buttonListGraphs.Text = "List Graphs";
            this.buttonListGraphs.UseVisualStyleBackColor = true;
            this.buttonListGraphs.Click += new System.EventHandler(this.buttonListGraphs_Click);
            // 
            // buttonStartJenaStorage
            // 
            this.buttonStartJenaStorage.Location = new System.Drawing.Point(655, 194);
            this.buttonStartJenaStorage.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonStartJenaStorage.Name = "buttonStartJenaStorage";
            this.buttonStartJenaStorage.Size = new System.Drawing.Size(84, 34);
            this.buttonStartJenaStorage.TabIndex = 12;
            this.buttonStartJenaStorage.Text = "Start Jena Storage";
            this.buttonStartJenaStorage.UseVisualStyleBackColor = true;
            this.buttonStartJenaStorage.Click += new System.EventHandler(this.buttonStartJenaAndApplyReasoner_Click);
            // 
            // buttonStopJenaStorage
            // 
            this.buttonStopJenaStorage.Location = new System.Drawing.Point(764, 194);
            this.buttonStopJenaStorage.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonStopJenaStorage.Name = "buttonStopJenaStorage";
            this.buttonStopJenaStorage.Size = new System.Drawing.Size(84, 34);
            this.buttonStopJenaStorage.TabIndex = 30;
            this.buttonStopJenaStorage.Text = "Stop Jena Storage";
            this.buttonStopJenaStorage.UseVisualStyleBackColor = true;
            this.buttonStopJenaStorage.Click += new System.EventHandler(this.buttonStopJenaStorage_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonExit.Location = new System.Drawing.Point(764, 1034);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(84, 23);
            this.buttonExit.TabIndex = 10;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelSPO
            // 
            this.labelSPO.AutoSize = true;
            this.labelSPO.Location = new System.Drawing.Point(143, 402);
            this.labelSPO.Margin = new System.Windows.Forms.Padding(20, 15, 3, 5);
            this.labelSPO.Name = "labelSPO";
            this.labelSPO.Size = new System.Drawing.Size(233, 13);
            this.labelSPO.TabIndex = 18;
            this.labelSPO.Text = "Subject (Qname) / Predicate (Qname) / Object :";
            // 
            // buttonLoadGraph
            // 
            this.buttonLoadGraph.Location = new System.Drawing.Point(764, 359);
            this.buttonLoadGraph.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonLoadGraph.Name = "buttonLoadGraph";
            this.buttonLoadGraph.Size = new System.Drawing.Size(84, 23);
            this.buttonLoadGraph.TabIndex = 16;
            this.buttonLoadGraph.Text = "Load Graph";
            this.buttonLoadGraph.UseVisualStyleBackColor = true;
            this.buttonLoadGraph.Click += new System.EventHandler(this.buttonLoadGraph_Click);
            // 
            // textBoxTripleSubject
            // 
            this.textBoxTripleSubject.Location = new System.Drawing.Point(436, 397);
            this.textBoxTripleSubject.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.textBoxTripleSubject.Name = "textBoxTripleSubject";
            this.textBoxTripleSubject.Size = new System.Drawing.Size(100, 20);
            this.textBoxTripleSubject.TabIndex = 19;
            // 
            // textBoxTriplePredicate
            // 
            this.textBoxTriplePredicate.Location = new System.Drawing.Point(542, 397);
            this.textBoxTriplePredicate.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.textBoxTriplePredicate.Name = "textBoxTriplePredicate";
            this.textBoxTriplePredicate.Size = new System.Drawing.Size(100, 20);
            this.textBoxTriplePredicate.TabIndex = 20;
            // 
            // textBoxTripleObject
            // 
            this.textBoxTripleObject.Location = new System.Drawing.Point(648, 397);
            this.textBoxTripleObject.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.textBoxTripleObject.Name = "textBoxTripleObject";
            this.textBoxTripleObject.Size = new System.Drawing.Size(100, 20);
            this.textBoxTripleObject.TabIndex = 21;
            // 
            // checkBoxAddTriple
            // 
            this.checkBoxAddTriple.AutoSize = true;
            this.checkBoxAddTriple.Checked = true;
            this.checkBoxAddTriple.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddTriple.Location = new System.Drawing.Point(544, 428);
            this.checkBoxAddTriple.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.checkBoxAddTriple.Name = "checkBoxAddTriple";
            this.checkBoxAddTriple.Size = new System.Drawing.Size(74, 17);
            this.checkBoxAddTriple.TabIndex = 22;
            this.checkBoxAddTriple.Text = "Add Triple";
            this.checkBoxAddTriple.UseVisualStyleBackColor = true;
            this.checkBoxAddTriple.CheckedChanged += new System.EventHandler(this.checkBoxAddTriple_CheckedChanged);
            // 
            // buttonUpdateGraph
            // 
            this.buttonUpdateGraph.Location = new System.Drawing.Point(764, 397);
            this.buttonUpdateGraph.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonUpdateGraph.Name = "buttonUpdateGraph";
            this.buttonUpdateGraph.Size = new System.Drawing.Size(84, 23);
            this.buttonUpdateGraph.TabIndex = 15;
            this.buttonUpdateGraph.Text = "Update Graph";
            this.buttonUpdateGraph.UseVisualStyleBackColor = true;
            this.buttonUpdateGraph.Click += new System.EventHandler(this.buttonUpdateGraph_Click);
            // 
            // labelQuery
            // 
            this.labelQuery.AutoSize = true;
            this.labelQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuery.Location = new System.Drawing.Point(20, 536);
            this.labelQuery.Margin = new System.Windows.Forms.Padding(20, 12, 3, 5);
            this.labelQuery.Name = "labelQuery";
            this.labelQuery.Size = new System.Drawing.Size(44, 13);
            this.labelQuery.TabIndex = 24;
            this.labelQuery.Text = "Query:";
            // 
            // buttonDeleteGraph
            // 
            this.buttonDeleteGraph.Location = new System.Drawing.Point(764, 458);
            this.buttonDeleteGraph.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonDeleteGraph.Name = "buttonDeleteGraph";
            this.buttonDeleteGraph.Size = new System.Drawing.Size(84, 23);
            this.buttonDeleteGraph.TabIndex = 17;
            this.buttonDeleteGraph.Text = "Delete Graph";
            this.buttonDeleteGraph.UseVisualStyleBackColor = true;
            this.buttonDeleteGraph.Click += new System.EventHandler(this.buttonDeleteGraph_Click);
            // 
            // checkBoxDeleteTriple
            // 
            this.checkBoxDeleteTriple.AutoSize = true;
            this.checkBoxDeleteTriple.Location = new System.Drawing.Point(650, 428);
            this.checkBoxDeleteTriple.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.checkBoxDeleteTriple.Name = "checkBoxDeleteTriple";
            this.checkBoxDeleteTriple.Size = new System.Drawing.Size(86, 17);
            this.checkBoxDeleteTriple.TabIndex = 23;
            this.checkBoxDeleteTriple.Text = "Delete Triple";
            this.checkBoxDeleteTriple.UseVisualStyleBackColor = true;
            this.checkBoxDeleteTriple.CheckedChanged += new System.EventHandler(this.checkBoxDeleteTriple_CheckedChanged);
            // 
            // buttonSaveGraph
            // 
            this.buttonSaveGraph.Location = new System.Drawing.Point(764, 496);
            this.buttonSaveGraph.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonSaveGraph.Name = "buttonSaveGraph";
            this.buttonSaveGraph.Size = new System.Drawing.Size(84, 23);
            this.buttonSaveGraph.TabIndex = 0;
            this.buttonSaveGraph.Text = "Save Graph";
            this.buttonSaveGraph.UseVisualStyleBackColor = true;
            this.buttonSaveGraph.Click += new System.EventHandler(this.buttonSaveGraph_Click);
            // 
            // labelQueryClassAttInfo
            // 
            this.labelQueryClassAttInfo.AutoSize = true;
            this.labelQueryClassAttInfo.Location = new System.Drawing.Point(143, 596);
            this.labelQueryClassAttInfo.Margin = new System.Windows.Forms.Padding(20, 22, 3, 5);
            this.labelQueryClassAttInfo.Name = "labelQueryClassAttInfo";
            this.labelQueryClassAttInfo.Size = new System.Drawing.Size(224, 13);
            this.labelQueryClassAttInfo.TabIndex = 32;
            this.labelQueryClassAttInfo.Text = "Class name ( leave empty field for all classes ):";
            // 
            // buttonQueryNumOfClasses
            // 
            this.buttonQueryNumOfClasses.Location = new System.Drawing.Point(764, 534);
            this.buttonQueryNumOfClasses.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonQueryNumOfClasses.Name = "buttonQueryNumOfClasses";
            this.buttonQueryNumOfClasses.Size = new System.Drawing.Size(84, 35);
            this.buttonQueryNumOfClasses.TabIndex = 31;
            this.buttonQueryNumOfClasses.Text = "Number of classes";
            this.buttonQueryNumOfClasses.UseVisualStyleBackColor = true;
            this.buttonQueryNumOfClasses.Click += new System.EventHandler(this.buttonQueryNumOfClasses_Click);
            // 
            // textBoxQueryClassAttInfo
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxQueryClassAttInfo, 3);
            this.textBoxQueryClassAttInfo.Location = new System.Drawing.Point(436, 594);
            this.textBoxQueryClassAttInfo.Margin = new System.Windows.Forms.Padding(3, 20, 3, 5);
            this.textBoxQueryClassAttInfo.Name = "textBoxQueryClassAttInfo";
            this.textBoxQueryClassAttInfo.Size = new System.Drawing.Size(315, 20);
            this.textBoxQueryClassAttInfo.TabIndex = 33;
            // 
            // labelGetConductingEquipment
            // 
            this.labelGetConductingEquipment.AutoSize = true;
            this.labelGetConductingEquipment.Location = new System.Drawing.Point(143, 644);
            this.labelGetConductingEquipment.Margin = new System.Windows.Forms.Padding(20, 20, 3, 5);
            this.labelGetConductingEquipment.Name = "labelGetConductingEquipment";
            this.labelGetConductingEquipment.Size = new System.Drawing.Size(101, 13);
            this.labelGetConductingEquipment.TabIndex = 34;
            this.labelGetConductingEquipment.Text = "Type of equipment: ";
            // 
            // buttonQueryAllClassesAttr
            // 
            this.buttonQueryAllClassesAttr.Location = new System.Drawing.Point(764, 584);
            this.buttonQueryAllClassesAttr.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonQueryAllClassesAttr.Name = "buttonQueryAllClassesAttr";
            this.buttonQueryAllClassesAttr.Size = new System.Drawing.Size(84, 35);
            this.buttonQueryAllClassesAttr.TabIndex = 25;
            this.buttonQueryAllClassesAttr.Text = "Class-attribute information";
            this.buttonQueryAllClassesAttr.UseVisualStyleBackColor = true;
            this.buttonQueryAllClassesAttr.Click += new System.EventHandler(this.buttonQueryAllClassesAttr_Click);
            // 
            // comboBoxQueryTypeOfEquipment
            // 
            this.comboBoxQueryTypeOfEquipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxQueryTypeOfEquipment.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxQueryTypeOfEquipment, 3);
            this.comboBoxQueryTypeOfEquipment.ForeColor = System.Drawing.SystemColors.Window;
            this.comboBoxQueryTypeOfEquipment.FormattingEnabled = true;
            this.comboBoxQueryTypeOfEquipment.Location = new System.Drawing.Point(436, 639);
            this.comboBoxQueryTypeOfEquipment.Margin = new System.Windows.Forms.Padding(3, 15, 3, 5);
            this.comboBoxQueryTypeOfEquipment.Name = "comboBoxQueryTypeOfEquipment";
            this.comboBoxQueryTypeOfEquipment.Size = new System.Drawing.Size(315, 21);
            this.comboBoxQueryTypeOfEquipment.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 693);
            this.label1.Margin = new System.Windows.Forms.Padding(20, 20, 3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Enter component ID :";
            // 
            // buttonQueryGetTypeOfEquipment
            // 
            this.buttonQueryGetTypeOfEquipment.Location = new System.Drawing.Point(764, 634);
            this.buttonQueryGetTypeOfEquipment.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonQueryGetTypeOfEquipment.Name = "buttonQueryGetTypeOfEquipment";
            this.buttonQueryGetTypeOfEquipment.Size = new System.Drawing.Size(83, 34);
            this.buttonQueryGetTypeOfEquipment.TabIndex = 35;
            this.buttonQueryGetTypeOfEquipment.Text = "Get equipment";
            this.buttonQueryGetTypeOfEquipment.UseVisualStyleBackColor = true;
            this.buttonQueryGetTypeOfEquipment.Click += new System.EventHandler(this.buttonQueryGetTypeOfEquipment_Click);
            // 
            // textBoxQueryConnectivityNode
            // 
            this.textBoxQueryConnectivityNode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxQueryConnectivityNode, 3);
            this.textBoxQueryConnectivityNode.Location = new System.Drawing.Point(436, 688);
            this.textBoxQueryConnectivityNode.Margin = new System.Windows.Forms.Padding(3, 15, 3, 5);
            this.textBoxQueryConnectivityNode.Name = "textBoxQueryConnectivityNode";
            this.textBoxQueryConnectivityNode.Size = new System.Drawing.Size(315, 20);
            this.textBoxQueryConnectivityNode.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 742);
            this.label2.Margin = new System.Windows.Forms.Padding(20, 20, 3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Enter ID:";
            // 
            // buttonQueryConnectivityNodeComponents
            // 
            this.buttonQueryConnectivityNodeComponents.Location = new System.Drawing.Point(764, 683);
            this.buttonQueryConnectivityNodeComponents.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonQueryConnectivityNodeComponents.Name = "buttonQueryConnectivityNodeComponents";
            this.buttonQueryConnectivityNodeComponents.Size = new System.Drawing.Size(83, 34);
            this.buttonQueryConnectivityNodeComponents.TabIndex = 39;
            this.buttonQueryConnectivityNodeComponents.Text = "Get belonging equipment";
            this.buttonQueryConnectivityNodeComponents.UseVisualStyleBackColor = true;
            this.buttonQueryConnectivityNodeComponents.Click += new System.EventHandler(this.buttonQueryConnectivityNodeComponents_Click);
            // 
            // textBoxQueryComponentID
            // 
            this.textBoxQueryComponentID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxQueryComponentID, 3);
            this.textBoxQueryComponentID.Location = new System.Drawing.Point(436, 737);
            this.textBoxQueryComponentID.Margin = new System.Windows.Forms.Padding(3, 15, 3, 5);
            this.textBoxQueryComponentID.Name = "textBoxQueryComponentID";
            this.textBoxQueryComponentID.Size = new System.Drawing.Size(315, 20);
            this.textBoxQueryComponentID.TabIndex = 41;
            // 
            // labelFilePreview
            // 
            this.labelFilePreview.AutoSize = true;
            this.labelFilePreview.Location = new System.Drawing.Point(20, 781);
            this.labelFilePreview.Margin = new System.Windows.Forms.Padding(20, 10, 3, 5);
            this.labelFilePreview.Name = "labelFilePreview";
            this.labelFilePreview.Size = new System.Drawing.Size(45, 13);
            this.labelFilePreview.TabIndex = 11;
            this.labelFilePreview.Text = "Report :";
            // 
            // buttonQueryGetComponentInfo
            // 
            this.buttonQueryGetComponentInfo.Location = new System.Drawing.Point(764, 732);
            this.buttonQueryGetComponentInfo.Margin = new System.Windows.Forms.Padding(10, 10, 15, 5);
            this.buttonQueryGetComponentInfo.Name = "buttonQueryGetComponentInfo";
            this.buttonQueryGetComponentInfo.Size = new System.Drawing.Size(83, 34);
            this.buttonQueryGetComponentInfo.TabIndex = 42;
            this.buttonQueryGetComponentInfo.Text = "Get data";
            this.buttonQueryGetComponentInfo.UseVisualStyleBackColor = true;
            this.buttonQueryGetComponentInfo.Click += new System.EventHandler(this.buttonQueryGetComponentInfo_Click);
            // 
            // labelApplyResetConfig
            // 
            this.labelApplyResetConfig.AutoSize = true;
            this.labelApplyResetConfig.Location = new System.Drawing.Point(20, 121);
            this.labelApplyResetConfig.Margin = new System.Windows.Forms.Padding(20, 12, 3, 5);
            this.labelApplyResetConfig.Name = "labelApplyResetConfig";
            this.labelApplyResetConfig.Size = new System.Drawing.Size(76, 13);
            this.labelApplyResetConfig.TabIndex = 43;
            this.labelApplyResetConfig.Text = "Fuseki config :";
            // 
            // buttonResetConfig
            // 
            this.buttonResetConfig.Location = new System.Drawing.Point(657, 134);
            this.buttonResetConfig.Margin = new System.Windows.Forms.Padding(12, 25, 3, 3);
            this.buttonResetConfig.Name = "buttonResetConfig";
            this.buttonResetConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonResetConfig.TabIndex = 44;
            this.buttonResetConfig.Text = "Reset config";
            this.buttonResetConfig.UseVisualStyleBackColor = true;
            this.buttonResetConfig.Click += new System.EventHandler(this.buttonResetConfig_Click);
            // 
            // CIMProfileLoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(884, 911);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CIMProfileLoaderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CIM File Loader";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelCIMProfile;
        private System.Windows.Forms.TextBox textBoxCIMProfile;
        private System.Windows.Forms.Label labelRDFXMLFile;
        private System.Windows.Forms.TextBox textBoxRDFFile;
        private System.Windows.Forms.RichTextBox richTextBoxReport;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label labelFilePreview;
        private System.Windows.Forms.Button buttonSaveGraph;
        private System.Windows.Forms.ComboBox comboBoxRDFGraphs;
        private System.Windows.Forms.Label labelRDFGraph;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonLoadProfile;
        private System.Windows.Forms.Button buttonBrowseRDF;
        private System.Windows.Forms.Button buttonLoadData;
        private System.Windows.Forms.Button buttonStartJenaStorage;
        private System.Windows.Forms.Button buttonDeleteGraph;
        private System.Windows.Forms.Button buttonLoadGraph;
        private System.Windows.Forms.Button buttonUpdateGraph;
        private System.Windows.Forms.Label labelSPO;
        private System.Windows.Forms.TextBox textBoxTripleSubject;
        private System.Windows.Forms.TextBox textBoxTriplePredicate;
        private System.Windows.Forms.TextBox textBoxTripleObject;
        private System.Windows.Forms.CheckBox checkBoxAddTriple;
        private System.Windows.Forms.CheckBox checkBoxDeleteTriple;
        private System.Windows.Forms.Label labelQuery;
        private System.Windows.Forms.Button buttonQueryAllClassesAttr;
        private System.Windows.Forms.Label labelRDFDataset;
        private System.Windows.Forms.ComboBox comboBoxRDFDatasets;
        private System.Windows.Forms.Button buttonListGraphs;
        private System.Windows.Forms.Button buttonApplyRDFSInferenceOverDataset;
        private System.Windows.Forms.Button buttonStopJenaStorage;
        private System.Windows.Forms.Button buttonQueryNumOfClasses;
        private System.Windows.Forms.Label labelQueryClassAttInfo;
        private System.Windows.Forms.TextBox textBoxQueryClassAttInfo;
        private System.Windows.Forms.Label labelGetConductingEquipment;
        private System.Windows.Forms.Button buttonQueryGetTypeOfEquipment;
        private System.Windows.Forms.ComboBox comboBoxQueryTypeOfEquipment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxQueryConnectivityNode;
        private System.Windows.Forms.Button buttonQueryConnectivityNodeComponents;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxQueryComponentID;
        private System.Windows.Forms.Button buttonQueryGetComponentInfo;
        private System.Windows.Forms.Label labelApplyResetConfig;
        private System.Windows.Forms.Button buttonResetConfig;
    }
}

