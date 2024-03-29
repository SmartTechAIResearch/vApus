﻿namespace vApus.StressTest {
    partial class EditScenarioPanel {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditScenarioPanel));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkClearScenarioBeforeImport = new System.Windows.Forms.CheckBox();
            this.btnRevertToImported = new System.Windows.Forms.Button();
            this.btnRedetermineParameterTokens = new System.Windows.Forms.Button();
            this.chkClearScenarioBeforeCapture = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tc = new vApus.Util.TabControlWithAdjustableBorders();
            this.tpCapture = new System.Windows.Forms.TabPage();
            this.btnOpenLupusTitanium = new System.Windows.Forms.Button();
            this.tpImport = new System.Windows.Forms.TabPage();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.fctxtxImport = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tpExtraTools = new System.Windows.Forms.TabPage();
            this.btnExportToTextFile = new System.Windows.Forms.Button();
            this.tc.SuspendLayout();
            this.tpCapture.SuspendLayout();
            this.tpImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctxtxImport)).BeginInit();
            this.tpExtraTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "capture.png");
            this.imageList.Images.SetKeyName(1, "import.png");
            // 
            // chkClearScenarioBeforeImport
            // 
            this.chkClearScenarioBeforeImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkClearScenarioBeforeImport.AutoSize = true;
            this.chkClearScenarioBeforeImport.Location = new System.Drawing.Point(6, 557);
            this.chkClearScenarioBeforeImport.Name = "chkClearScenarioBeforeImport";
            this.chkClearScenarioBeforeImport.Size = new System.Drawing.Size(157, 17);
            this.chkClearScenarioBeforeImport.TabIndex = 31;
            this.chkClearScenarioBeforeImport.Text = "Clear scenario before import";
            this.toolTip.SetToolTip(this.chkClearScenarioBeforeImport, "Check this if you want to clear the scenario before you import from text.");
            this.chkClearScenarioBeforeImport.UseVisualStyleBackColor = true;
            // 
            // btnRevertToImported
            // 
            this.btnRevertToImported.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRevertToImported.BackColor = System.Drawing.Color.White;
            this.btnRevertToImported.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevertToImported.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevertToImported.Location = new System.Drawing.Point(269, 279);
            this.btnRevertToImported.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnRevertToImported.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnRevertToImported.Name = "btnRevertToImported";
            this.btnRevertToImported.Size = new System.Drawing.Size(209, 24);
            this.btnRevertToImported.TabIndex = 1;
            this.btnRevertToImported.Text = "Revert to imported";
            this.toolTip.SetToolTip(this.btnRevertToImported, "Revert the whole scenario to how it was imported.");
            this.btnRevertToImported.UseVisualStyleBackColor = false;
            this.btnRevertToImported.Click += new System.EventHandler(this.btnRevertToImported_Click);
            // 
            // btnRedetermineParameterTokens
            // 
            this.btnRedetermineParameterTokens.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRedetermineParameterTokens.BackColor = System.Drawing.Color.White;
            this.btnRedetermineParameterTokens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedetermineParameterTokens.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRedetermineParameterTokens.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRedetermineParameterTokens.Location = new System.Drawing.Point(269, 309);
            this.btnRedetermineParameterTokens.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnRedetermineParameterTokens.Name = "btnRedetermineParameterTokens";
            this.btnRedetermineParameterTokens.Size = new System.Drawing.Size(209, 24);
            this.btnRedetermineParameterTokens.TabIndex = 2;
            this.btnRedetermineParameterTokens.Text = "Redetermine parameter tokens...";
            this.toolTip.SetToolTip(this.btnRedetermineParameterTokens, "Sometimes a set of parameter tokens can be used and is needed for a valid request" +
        ".\r\nIn the following dialog you can redetermine the tokens for the whole scenario" +
        " to avoid this problem.\r\n");
            this.btnRedetermineParameterTokens.UseVisualStyleBackColor = false;
            this.btnRedetermineParameterTokens.Click += new System.EventHandler(this.btnRedetermineParameterTokens_Click);
            // 
            // chkClearScenarioBeforeCapture
            // 
            this.chkClearScenarioBeforeCapture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkClearScenarioBeforeCapture.AutoSize = true;
            this.chkClearScenarioBeforeCapture.Location = new System.Drawing.Point(344, 309);
            this.chkClearScenarioBeforeCapture.Name = "chkClearScenarioBeforeCapture";
            this.chkClearScenarioBeforeCapture.Size = new System.Drawing.Size(165, 17);
            this.chkClearScenarioBeforeCapture.TabIndex = 32;
            this.chkClearScenarioBeforeCapture.Text = "Clear scenario before capture";
            this.chkClearScenarioBeforeCapture.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            this.openFileDialog.Multiselect = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Text File (*.txt)|*.txt";
            // 
            // tc
            // 
            this.tc.BottomVisible = false;
            this.tc.Controls.Add(this.tpCapture);
            this.tc.Controls.Add(this.tpImport);
            this.tc.Controls.Add(this.tpExtraTools);
            this.tc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc.ImageList = this.imageList;
            this.tc.LeftVisible = false;
            this.tc.Location = new System.Drawing.Point(0, 3);
            this.tc.Name = "tc";
            this.tc.RightVisible = false;
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(748, 606);
            this.tc.TabIndex = 0;
            this.tc.TopVisible = true;
            // 
            // tpCapture
            // 
            this.tpCapture.BackColor = System.Drawing.Color.White;
            this.tpCapture.Controls.Add(this.chkClearScenarioBeforeCapture);
            this.tpCapture.Controls.Add(this.btnOpenLupusTitanium);
            this.tpCapture.ImageIndex = 0;
            this.tpCapture.Location = new System.Drawing.Point(0, 23);
            this.tpCapture.Name = "tpCapture";
            this.tpCapture.Padding = new System.Windows.Forms.Padding(3);
            this.tpCapture.Size = new System.Drawing.Size(747, 582);
            this.tpCapture.TabIndex = 0;
            this.tpCapture.Text = "Capture HTTP(s)";
            // 
            // btnOpenLupusTitanium
            // 
            this.btnOpenLupusTitanium.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpenLupusTitanium.BackColor = System.Drawing.Color.White;
            this.btnOpenLupusTitanium.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenLupusTitanium.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenLupusTitanium.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenLupusTitanium.Location = new System.Drawing.Point(269, 279);
            this.btnOpenLupusTitanium.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnOpenLupusTitanium.Name = "btnOpenLupusTitanium";
            this.btnOpenLupusTitanium.Size = new System.Drawing.Size(240, 24);
            this.btnOpenLupusTitanium.TabIndex = 31;
            this.btnOpenLupusTitanium.Text = "Open Lupus-Titanium HTTP(s) proxy...";
            this.btnOpenLupusTitanium.UseVisualStyleBackColor = false;
            this.btnOpenLupusTitanium.Click += new System.EventHandler(this.btnOpenLupusTitanium_Click);
            // 
            // tpImport
            // 
            this.tpImport.BackColor = System.Drawing.Color.White;
            this.tpImport.Controls.Add(this.chkClearScenarioBeforeImport);
            this.tpImport.Controls.Add(this.btnImport);
            this.tpImport.Controls.Add(this.btnBrowse);
            this.tpImport.Controls.Add(this.fctxtxImport);
            this.tpImport.ImageIndex = 1;
            this.tpImport.Location = new System.Drawing.Point(0, 23);
            this.tpImport.Name = "tpImport";
            this.tpImport.Padding = new System.Windows.Forms.Padding(3);
            this.tpImport.Size = new System.Drawing.Size(747, 582);
            this.tpImport.TabIndex = 1;
            this.tpImport.Text = "Import from text";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnImport.AutoSize = true;
            this.btnImport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnImport.BackColor = System.Drawing.Color.White;
            this.btnImport.Enabled = false;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(305, 552);
            this.btnImport.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(54, 24);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnBrowse.AutoSize = true;
            this.btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowse.BackColor = System.Drawing.Color.White;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowse.Location = new System.Drawing.Point(365, 552);
            this.btnBrowse.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(72, 24);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // fctxtxImport
            // 
            this.fctxtxImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fctxtxImport.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctxtxImport.AutoScrollMinSize = new System.Drawing.Size(0, 14);
            this.fctxtxImport.BackBrush = null;
            this.fctxtxImport.CharHeight = 14;
            this.fctxtxImport.CharWidth = 8;
            this.fctxtxImport.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctxtxImport.DelayedTextChangedInterval = 200;
            this.fctxtxImport.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctxtxImport.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.fctxtxImport.IsReplaceMode = false;
            this.fctxtxImport.Location = new System.Drawing.Point(0, 0);
            this.fctxtxImport.Name = "fctxtxImport";
            this.fctxtxImport.Paddings = new System.Windows.Forms.Padding(0);
            this.fctxtxImport.PreferredLineWidth = 65536;
            this.fctxtxImport.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctxtxImport.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctxtxImport.ServiceColors")));
            this.fctxtxImport.Size = new System.Drawing.Size(747, 546);
            this.fctxtxImport.TabIndex = 0;
            this.fctxtxImport.WordWrap = true;
            this.fctxtxImport.Zoom = 100;
            this.fctxtxImport.TextChangedDelayed += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctxtxImport_TextChangedDelayed);
            // 
            // tpExtraTools
            // 
            this.tpExtraTools.BackColor = System.Drawing.Color.White;
            this.tpExtraTools.Controls.Add(this.btnRevertToImported);
            this.tpExtraTools.Controls.Add(this.btnRedetermineParameterTokens);
            this.tpExtraTools.Controls.Add(this.btnExportToTextFile);
            this.tpExtraTools.Location = new System.Drawing.Point(0, 23);
            this.tpExtraTools.Name = "tpExtraTools";
            this.tpExtraTools.Padding = new System.Windows.Forms.Padding(3);
            this.tpExtraTools.Size = new System.Drawing.Size(747, 582);
            this.tpExtraTools.TabIndex = 2;
            this.tpExtraTools.Text = "Extra tools";
            // 
            // btnExportToTextFile
            // 
            this.btnExportToTextFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExportToTextFile.BackColor = System.Drawing.Color.White;
            this.btnExportToTextFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportToTextFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportToTextFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportToTextFile.Location = new System.Drawing.Point(269, 249);
            this.btnExportToTextFile.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnExportToTextFile.Name = "btnExportToTextFile";
            this.btnExportToTextFile.Size = new System.Drawing.Size(209, 24);
            this.btnExportToTextFile.TabIndex = 0;
            this.btnExportToTextFile.Text = "Export to text file...";
            this.btnExportToTextFile.UseVisualStyleBackColor = false;
            this.btnExportToTextFile.Click += new System.EventHandler(this.btnExportToTextFile_Click);
            // 
            // EditScenarioPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tc);
            this.Name = "EditScenarioPanel";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(748, 609);
            this.tc.ResumeLayout(false);
            this.tpCapture.ResumeLayout(false);
            this.tpCapture.PerformLayout();
            this.tpImport.ResumeLayout(false);
            this.tpImport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctxtxImport)).EndInit();
            this.tpExtraTools.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Util.TabControlWithAdjustableBorders tc;
        private System.Windows.Forms.TabPage tpCapture;
        private System.Windows.Forms.TabPage tpImport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnBrowse;
        private FastColoredTextBoxNS.FastColoredTextBox fctxtxImport;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.CheckBox chkClearScenarioBeforeImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tpExtraTools;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button btnRevertToImported;
        private System.Windows.Forms.Button btnRedetermineParameterTokens;
        private System.Windows.Forms.Button btnExportToTextFile;
        private System.Windows.Forms.Button btnOpenLupusTitanium;
        private System.Windows.Forms.CheckBox chkClearScenarioBeforeCapture;
    }
}
