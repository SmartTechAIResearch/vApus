﻿namespace vApus.StressTest {
    partial class EditUserActionPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditUserActionPanel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblLabel = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.lblMove = new System.Windows.Forms.Label();
            this.lblSteps = new System.Windows.Forms.Label();
            this.nudMoveSteps = new System.Windows.Forms.NumericUpDown();
            this.lblConnection = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnShowHideParameterTokens = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.picDelay = new System.Windows.Forms.PictureBox();
            this.picCopy = new System.Windows.Forms.PictureBox();
            this.chkUseEditView = new System.Windows.Forms.CheckBox();
            this.splitParameterTokens = new System.Windows.Forms.SplitContainer();
            this.tc = new vApus.Util.TabControlWithAdjustableBorders();
            this.tpStructured = new System.Windows.Forms.TabPage();
            this.splitStructured = new System.Windows.Forms.SplitContainer();
            this.dgvRequests = new System.Windows.Forms.DataGridView();
            this.btnApplyEditView = new System.Windows.Forms.Button();
            this.fctxteditView = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tpPlainText = new System.Windows.Forms.TabPage();
            this.fctxtxPlainText = new FastColoredTextBoxNS.FastColoredTextBox();
            this.pnlBorderTokens = new System.Windows.Forms.Panel();
            this.cboParameterScope = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flpTokens = new System.Windows.Forms.FlowLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.flpConfiguration = new System.Windows.Forms.FlowLayoutPanel();
            this.lbtnAsImported = new vApus.Util.LinkButton();
            this.lbtnEditable = new vApus.Util.LinkButton();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnRevertToImported = new System.Windows.Forms.Button();
            this.lblLinkTo = new System.Windows.Forms.Label();
            this.flpLink = new System.Windows.Forms.FlowLayoutPanel();
            this.picMoveDown = new System.Windows.Forms.PictureBox();
            this.picMoveUp = new System.Windows.Forms.PictureBox();
            this.lblRequestCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudMoveSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitParameterTokens)).BeginInit();
            this.splitParameterTokens.Panel1.SuspendLayout();
            this.splitParameterTokens.Panel2.SuspendLayout();
            this.splitParameterTokens.SuspendLayout();
            this.tc.SuspendLayout();
            this.tpStructured.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitStructured)).BeginInit();
            this.splitStructured.Panel1.SuspendLayout();
            this.splitStructured.Panel2.SuspendLayout();
            this.splitStructured.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fctxteditView)).BeginInit();
            this.tpPlainText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctxtxPlainText)).BeginInit();
            this.pnlBorderTokens.SuspendLayout();
            this.flpConfiguration.SuspendLayout();
            this.flpLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMoveDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMoveUp)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new System.Drawing.Point(13, 39);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(36, 13);
            this.lblLabel.TabIndex = 26;
            this.lblLabel.Text = "Label:";
            // 
            // txtLabel
            // 
            this.txtLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLabel.Location = new System.Drawing.Point(56, 36);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(935, 20);
            this.txtLabel.TabIndex = 25;
            this.txtLabel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtLabel_KeyUp);
            this.txtLabel.Leave += new System.EventHandler(this.txtLabel_Leave);
            // 
            // lblMove
            // 
            this.lblMove.AutoSize = true;
            this.lblMove.Location = new System.Drawing.Point(13, 67);
            this.lblMove.Name = "lblMove";
            this.lblMove.Size = new System.Drawing.Size(37, 13);
            this.lblMove.TabIndex = 27;
            this.lblMove.Text = "Move:";
            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.Location = new System.Drawing.Point(98, 67);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(37, 13);
            this.lblSteps.TabIndex = 30;
            this.lblSteps.Text = "Steps:";
            // 
            // nudMoveSteps
            // 
            this.nudMoveSteps.Location = new System.Drawing.Point(141, 65);
            this.nudMoveSteps.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudMoveSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMoveSteps.Name = "nudMoveSteps";
            this.nudMoveSteps.Size = new System.Drawing.Size(42, 20);
            this.nudMoveSteps.TabIndex = 31;
            this.nudMoveSteps.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnection.Location = new System.Drawing.Point(6, 10);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(72, 13);
            this.lblConnection.TabIndex = 37;
            this.lblConnection.Text = "User action";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Requests";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 100;
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 50;
            this.toolTip.ReshowDelay = 20;
            // 
            // btnShowHideParameterTokens
            // 
            this.btnShowHideParameterTokens.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnShowHideParameterTokens.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnShowHideParameterTokens.BackColor = System.Drawing.Color.White;
            this.btnShowHideParameterTokens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowHideParameterTokens.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowHideParameterTokens.Location = new System.Drawing.Point(515, 609);
            this.btnShowHideParameterTokens.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnShowHideParameterTokens.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnShowHideParameterTokens.Name = "btnShowHideParameterTokens";
            this.btnShowHideParameterTokens.Size = new System.Drawing.Size(157, 24);
            this.btnShowHideParameterTokens.TabIndex = 72;
            this.btnShowHideParameterTokens.Text = "Show parameter tokens";
            this.toolTip.SetToolTip(this.btnShowHideParameterTokens, "Show parameter tokens.");
            this.btnShowHideParameterTokens.UseVisualStyleBackColor = false;
            this.btnShowHideParameterTokens.Click += new System.EventHandler(this.btnShowHideParameterTokens_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.AutoSize = true;
            this.btnMerge.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMerge.BackColor = System.Drawing.Color.White;
            this.btnMerge.Enabled = false;
            this.btnMerge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMerge.Location = new System.Drawing.Point(3, 3);
            this.btnMerge.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnMerge.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(54, 24);
            this.btnMerge.TabIndex = 71;
            this.btnMerge.Text = "Merge";
            this.toolTip.SetToolTip(this.btnMerge, "Merge all linked user actions into a new one.");
            this.btnMerge.UseVisualStyleBackColor = false;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnSplit
            // 
            this.btnSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplit.AutoSize = true;
            this.btnSplit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSplit.BackColor = System.Drawing.Color.White;
            this.btnSplit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSplit.Location = new System.Drawing.Point(947, 62);
            this.btnSplit.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnSplit.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(44, 24);
            this.btnSplit.TabIndex = 77;
            this.btnSplit.Text = "Split";
            this.toolTip.SetToolTip(this.btnSplit, "Split all requests in seperate user actions.");
            this.btnSplit.UseVisualStyleBackColor = false;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // picDelay
            // 
            this.picDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picDelay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDelay.Image = global::vApus.StressTest.Properties.Resources.Delay;
            this.picDelay.Location = new System.Drawing.Point(925, 67);
            this.picDelay.Name = "picDelay";
            this.picDelay.Size = new System.Drawing.Size(16, 16);
            this.picDelay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDelay.TabIndex = 74;
            this.picDelay.TabStop = false;
            this.toolTip.SetToolTip(this.picDelay, "Click to NOT use delay after this user action.\r\nDelay is determined in the stress" +
        "test settings.");
            this.picDelay.Click += new System.EventHandler(this.picDelay_Click);
            // 
            // picCopy
            // 
            this.picCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCopy.Image = ((System.Drawing.Image)(resources.GetObject("picCopy.Image")));
            this.picCopy.Location = new System.Drawing.Point(903, 67);
            this.picCopy.Name = "picCopy";
            this.picCopy.Size = new System.Drawing.Size(16, 16);
            this.picCopy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCopy.TabIndex = 73;
            this.picCopy.TabStop = false;
            this.toolTip.SetToolTip(this.picCopy, "Copy this user action.");
            this.picCopy.Click += new System.EventHandler(this.picCopy_Click);
            // 
            // chkUseEditView
            // 
            this.chkUseEditView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkUseEditView.AutoSize = true;
            this.chkUseEditView.Location = new System.Drawing.Point(8, 400);
            this.chkUseEditView.Name = "chkUseEditView";
            this.chkUseEditView.Size = new System.Drawing.Size(90, 17);
            this.chkUseEditView.TabIndex = 34;
            this.chkUseEditView.Text = "Use edit view";
            this.toolTip.SetToolTip(this.chkUseEditView, "Show an edit view when a cell is selected, if checked.");
            this.chkUseEditView.UseVisualStyleBackColor = true;
            this.chkUseEditView.CheckedChanged += new System.EventHandler(this.chkUseEditView_CheckedChanged);
            // 
            // splitParameterTokens
            // 
            this.splitParameterTokens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitParameterTokens.BackColor = System.Drawing.SystemColors.Control;
            this.splitParameterTokens.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitParameterTokens.Location = new System.Drawing.Point(9, 192);
            this.splitParameterTokens.Name = "splitParameterTokens";
            // 
            // splitParameterTokens.Panel1
            // 
            this.splitParameterTokens.Panel1.BackColor = System.Drawing.Color.White;
            this.splitParameterTokens.Panel1.Controls.Add(this.tc);
            // 
            // splitParameterTokens.Panel2
            // 
            this.splitParameterTokens.Panel2.BackColor = System.Drawing.Color.White;
            this.splitParameterTokens.Panel2.Controls.Add(this.pnlBorderTokens);
            this.splitParameterTokens.Panel2.Controls.Add(this.label4);
            this.splitParameterTokens.Panel2.Controls.Add(this.flpTokens);
            this.splitParameterTokens.Panel2.Controls.Add(this.label5);
            this.splitParameterTokens.Panel2.Controls.Add(this.label6);
            this.splitParameterTokens.Panel2Collapsed = true;
            this.splitParameterTokens.Size = new System.Drawing.Size(988, 449);
            this.splitParameterTokens.SplitterDistance = 588;
            this.splitParameterTokens.TabIndex = 46;
            this.splitParameterTokens.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.split_SplitterMoved);
            // 
            // tc
            // 
            this.tc.BottomVisible = false;
            this.tc.Controls.Add(this.tpStructured);
            this.tc.Controls.Add(this.tpPlainText);
            this.tc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc.LeftVisible = false;
            this.tc.Location = new System.Drawing.Point(0, 0);
            this.tc.Name = "tc";
            this.tc.RightVisible = false;
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(988, 449);
            this.tc.TabIndex = 33;
            this.tc.TopVisible = true;
            this.tc.SelectedIndexChanged += new System.EventHandler(this.tc_SelectedIndexChanged);
            // 
            // tpStructured
            // 
            this.tpStructured.BackColor = System.Drawing.Color.White;
            this.tpStructured.Controls.Add(this.chkUseEditView);
            this.tpStructured.Controls.Add(this.splitStructured);
            this.tpStructured.Location = new System.Drawing.Point(0, 22);
            this.tpStructured.Name = "tpStructured";
            this.tpStructured.Padding = new System.Windows.Forms.Padding(3);
            this.tpStructured.Size = new System.Drawing.Size(987, 426);
            this.tpStructured.TabIndex = 0;
            this.tpStructured.Text = "Structured";
            // 
            // splitStructured
            // 
            this.splitStructured.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitStructured.BackColor = System.Drawing.SystemColors.Control;
            this.splitStructured.Location = new System.Drawing.Point(0, 0);
            this.splitStructured.Name = "splitStructured";
            this.splitStructured.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitStructured.Panel1
            // 
            this.splitStructured.Panel1.BackColor = System.Drawing.Color.White;
            this.splitStructured.Panel1.Controls.Add(this.dgvRequests);
            // 
            // splitStructured.Panel2
            // 
            this.splitStructured.Panel2.Controls.Add(this.btnApplyEditView);
            this.splitStructured.Panel2.Controls.Add(this.fctxteditView);
            this.splitStructured.Panel2Collapsed = true;
            this.splitStructured.Size = new System.Drawing.Size(985, 387);
            this.splitStructured.SplitterDistance = 193;
            this.splitStructured.TabIndex = 33;
            // 
            // dgvRequests
            // 
            this.dgvRequests.AllowDrop = true;
            this.dgvRequests.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvRequests.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRequests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRequests.BackgroundColor = System.Drawing.Color.White;
            this.dgvRequests.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRequests.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvRequests.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRequests.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRequests.EnableHeadersVisualStyles = false;
            this.dgvRequests.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.dgvRequests.Location = new System.Drawing.Point(0, 0);
            this.dgvRequests.Name = "dgvRequests";
            this.dgvRequests.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRequests.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRequests.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dgvRequests.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRequests.Size = new System.Drawing.Size(985, 387);
            this.dgvRequests.TabIndex = 32;
            this.dgvRequests.VirtualMode = true;
            this.dgvRequests.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequests_CellEnter);
            this.dgvRequests.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvRequests_CellValueNeeded);
            this.dgvRequests.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvRequests_CellValuePushed);
            this.dgvRequests.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRequests_ColumnHeaderMouseClick);
            this.dgvRequests.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRequests_RowPostPaint);
            this.dgvRequests.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvRequests_DragDrop);
            this.dgvRequests.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvRequests_DragOver);
            this.dgvRequests.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvRequests_KeyUp);
            this.dgvRequests.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvRequests_MouseDown);
            this.dgvRequests.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvRequests_MouseMove);
            // 
            // btnApplyEditView
            // 
            this.btnApplyEditView.AutoSize = true;
            this.btnApplyEditView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApplyEditView.BackColor = System.Drawing.Color.White;
            this.btnApplyEditView.Enabled = false;
            this.btnApplyEditView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyEditView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyEditView.Location = new System.Drawing.Point(467, 161);
            this.btnApplyEditView.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnApplyEditView.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnApplyEditView.Name = "btnApplyEditView";
            this.btnApplyEditView.Size = new System.Drawing.Size(50, 24);
            this.btnApplyEditView.TabIndex = 72;
            this.btnApplyEditView.Text = "Apply";
            this.btnApplyEditView.UseVisualStyleBackColor = false;
            this.btnApplyEditView.Click += new System.EventHandler(this.btnApplyEditView_Click);
            // 
            // fctxteditView
            // 
            this.fctxteditView.AutoCompleteBracketsList = new char[] {
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
            this.fctxteditView.AutoScrollMinSize = new System.Drawing.Size(0, 22);
            this.fctxteditView.BackBrush = null;
            this.fctxteditView.CharHeight = 22;
            this.fctxteditView.CharWidth = 8;
            this.fctxteditView.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctxteditView.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctxteditView.Enabled = false;
            this.fctxteditView.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.fctxteditView.IsReplaceMode = false;
            this.fctxteditView.LineInterval = 8;
            this.fctxteditView.Location = new System.Drawing.Point(6, 0);
            this.fctxteditView.Name = "fctxteditView";
            this.fctxteditView.Paddings = new System.Windows.Forms.Padding(0);
            this.fctxteditView.PreferredLineWidth = 65536;
            this.fctxteditView.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctxteditView.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctxteditView.ServiceColors")));
            this.fctxteditView.Size = new System.Drawing.Size(973, 155);
            this.fctxteditView.TabIndex = 2;
            this.fctxteditView.WordWrap = true;
            this.fctxteditView.Zoom = 100;
            this.fctxteditView.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctxteditView_TextChanged);
            // 
            // tpPlainText
            // 
            this.tpPlainText.BackColor = System.Drawing.Color.White;
            this.tpPlainText.Controls.Add(this.fctxtxPlainText);
            this.tpPlainText.Location = new System.Drawing.Point(0, 22);
            this.tpPlainText.Name = "tpPlainText";
            this.tpPlainText.Padding = new System.Windows.Forms.Padding(3);
            this.tpPlainText.Size = new System.Drawing.Size(987, 426);
            this.tpPlainText.TabIndex = 1;
            this.tpPlainText.Text = "Plain text";
            // 
            // fctxtxPlainText
            // 
            this.fctxtxPlainText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fctxtxPlainText.AutoCompleteBracketsList = new char[] {
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
            this.fctxtxPlainText.AutoScrollMinSize = new System.Drawing.Size(2, 22);
            this.fctxtxPlainText.BackBrush = null;
            this.fctxtxPlainText.CharHeight = 22;
            this.fctxtxPlainText.CharWidth = 8;
            this.fctxtxPlainText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctxtxPlainText.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctxtxPlainText.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.fctxtxPlainText.IsReplaceMode = false;
            this.fctxtxPlainText.LineInterval = 8;
            this.fctxtxPlainText.Location = new System.Drawing.Point(3, 3);
            this.fctxtxPlainText.Name = "fctxtxPlainText";
            this.fctxtxPlainText.Paddings = new System.Windows.Forms.Padding(0);
            this.fctxtxPlainText.PreferredLineWidth = 65536;
            this.fctxtxPlainText.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctxtxPlainText.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctxtxPlainText.ServiceColors")));
            this.fctxtxPlainText.Size = new System.Drawing.Size(981, 386);
            this.fctxtxPlainText.TabIndex = 1;
            this.fctxtxPlainText.Zoom = 100;
            this.fctxtxPlainText.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctxtxPlainText_TextChanged);
            // 
            // pnlBorderTokens
            // 
            this.pnlBorderTokens.BackColor = System.Drawing.Color.Silver;
            this.pnlBorderTokens.Controls.Add(this.cboParameterScope);
            this.pnlBorderTokens.Location = new System.Drawing.Point(9, 47);
            this.pnlBorderTokens.Name = "pnlBorderTokens";
            this.pnlBorderTokens.Size = new System.Drawing.Size(323, 23);
            this.pnlBorderTokens.TabIndex = 49;
            // 
            // cboParameterScope
            // 
            this.cboParameterScope.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboParameterScope.BackColor = System.Drawing.Color.White;
            this.cboParameterScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParameterScope.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboParameterScope.FormattingEnabled = true;
            this.cboParameterScope.Items.AddRange(new object[] {
            "<All>",
            "once in the scenario",
            "once in the parent user action",
            "once in this request",
            "once in the leaf node",
            "for every call"});
            this.cboParameterScope.Location = new System.Drawing.Point(1, 1);
            this.cboParameterScope.Name = "cboParameterScope";
            this.cboParameterScope.Size = new System.Drawing.Size(321, 21);
            this.cboParameterScope.TabIndex = 7;
            this.cboParameterScope.SelectedIndexChanged += new System.EventHandler(this.cboParameterScope_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Parameter tokens:";
            // 
            // flpTokens
            // 
            this.flpTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpTokens.AutoScroll = true;
            this.flpTokens.BackColor = System.Drawing.Color.White;
            this.flpTokens.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpTokens.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.flpTokens.Location = new System.Drawing.Point(9, 104);
            this.flpTokens.Name = "flpTokens";
            this.flpTokens.Size = new System.Drawing.Size(323, 387);
            this.flpTokens.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "This is for every user executing this scenario.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(271, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Show tokens for parameter values that are redetermined";
            // 
            // flpConfiguration
            // 
            this.flpConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flpConfiguration.Controls.Add(this.lbtnAsImported);
            this.flpConfiguration.Controls.Add(this.lbtnEditable);
            this.flpConfiguration.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpConfiguration.Location = new System.Drawing.Point(844, 604);
            this.flpConfiguration.Name = "flpConfiguration";
            this.flpConfiguration.Size = new System.Drawing.Size(150, 31);
            this.flpConfiguration.TabIndex = 48;
            // 
            // lbtnAsImported
            // 
            this.lbtnAsImported.Active = false;
            this.lbtnAsImported.ActiveLinkColor = System.Drawing.Color.DimGray;
            this.lbtnAsImported.AutoSize = true;
            this.lbtnAsImported.BackColor = System.Drawing.Color.Transparent;
            this.lbtnAsImported.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lbtnAsImported.ForeColor = System.Drawing.Color.DimGray;
            this.lbtnAsImported.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lbtnAsImported.LinkColor = System.Drawing.Color.DimGray;
            this.lbtnAsImported.Location = new System.Drawing.Point(71, 6);
            this.lbtnAsImported.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.lbtnAsImported.Name = "lbtnAsImported";
            this.lbtnAsImported.Padding = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.lbtnAsImported.RadioButtonBehavior = true;
            this.lbtnAsImported.Size = new System.Drawing.Size(79, 20);
            this.lbtnAsImported.TabIndex = 35;
            this.lbtnAsImported.TabStop = true;
            this.lbtnAsImported.Text = "As imported";
            this.lbtnAsImported.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbtnAsImported.VisitedLinkColor = System.Drawing.Color.DimGray;
            this.lbtnAsImported.ActiveChanged += new System.EventHandler(this.lbtn_ActiveChanged);
            // 
            // lbtnEditable
            // 
            this.lbtnEditable.Active = true;
            this.lbtnEditable.ActiveLinkColor = System.Drawing.Color.Black;
            this.lbtnEditable.AutoSize = true;
            this.lbtnEditable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbtnEditable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lbtnEditable.ForeColor = System.Drawing.Color.Black;
            this.lbtnEditable.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lbtnEditable.LinkColor = System.Drawing.Color.Black;
            this.lbtnEditable.Location = new System.Drawing.Point(7, 6);
            this.lbtnEditable.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.lbtnEditable.Name = "lbtnEditable";
            this.lbtnEditable.Padding = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.lbtnEditable.RadioButtonBehavior = true;
            this.lbtnEditable.Size = new System.Drawing.Size(61, 22);
            this.lbtnEditable.TabIndex = 34;
            this.lbtnEditable.TabStop = true;
            this.lbtnEditable.Text = "Editable";
            this.lbtnEditable.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbtnEditable.VisitedLinkColor = System.Drawing.Color.Black;
            this.lbtnEditable.ActiveChanged += new System.EventHandler(this.lbtn_ActiveChanged);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnApply.AutoSize = true;
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.BackColor = System.Drawing.Color.White;
            this.btnApply.Enabled = false;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(328, 609);
            this.btnApply.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnApply.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(50, 24);
            this.btnApply.TabIndex = 70;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Visible = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnRevertToImported
            // 
            this.btnRevertToImported.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRevertToImported.AutoSize = true;
            this.btnRevertToImported.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRevertToImported.BackColor = System.Drawing.Color.White;
            this.btnRevertToImported.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevertToImported.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevertToImported.Location = new System.Drawing.Point(384, 609);
            this.btnRevertToImported.MaximumSize = new System.Drawing.Size(9999, 24);
            this.btnRevertToImported.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnRevertToImported.Name = "btnRevertToImported";
            this.btnRevertToImported.Size = new System.Drawing.Size(124, 24);
            this.btnRevertToImported.TabIndex = 71;
            this.btnRevertToImported.Text = "Revert to imported";
            this.btnRevertToImported.UseVisualStyleBackColor = false;
            this.btnRevertToImported.Click += new System.EventHandler(this.btnRevertToImported_Click);
            // 
            // lblLinkTo
            // 
            this.lblLinkTo.AutoSize = true;
            this.lblLinkTo.Location = new System.Drawing.Point(13, 101);
            this.lblLinkTo.Name = "lblLinkTo";
            this.lblLinkTo.Size = new System.Drawing.Size(30, 13);
            this.lblLinkTo.TabIndex = 75;
            this.lblLinkTo.Text = "Link:";
            // 
            // flpLink
            // 
            this.flpLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpLink.AutoScroll = true;
            this.flpLink.Controls.Add(this.btnMerge);
            this.flpLink.Location = new System.Drawing.Point(56, 94);
            this.flpLink.Name = "flpLink";
            this.flpLink.Size = new System.Drawing.Size(935, 70);
            this.flpLink.TabIndex = 70;
            // 
            // picMoveDown
            // 
            this.picMoveDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMoveDown.Image = global::vApus.StressTest.Properties.Resources.MoveDown;
            this.picMoveDown.Location = new System.Drawing.Point(76, 67);
            this.picMoveDown.Name = "picMoveDown";
            this.picMoveDown.Size = new System.Drawing.Size(16, 16);
            this.picMoveDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMoveDown.TabIndex = 29;
            this.picMoveDown.TabStop = false;
            this.picMoveDown.Click += new System.EventHandler(this.picMoveDown_Click);
            // 
            // picMoveUp
            // 
            this.picMoveUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMoveUp.Image = global::vApus.StressTest.Properties.Resources.MoveUp;
            this.picMoveUp.Location = new System.Drawing.Point(56, 67);
            this.picMoveUp.Name = "picMoveUp";
            this.picMoveUp.Size = new System.Drawing.Size(16, 16);
            this.picMoveUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMoveUp.TabIndex = 28;
            this.picMoveUp.TabStop = false;
            this.picMoveUp.Click += new System.EventHandler(this.picMoveUp_Click);
            // 
            // lblRequestCount
            // 
            this.lblRequestCount.AutoSize = true;
            this.lblRequestCount.Location = new System.Drawing.Point(78, 173);
            this.lblRequestCount.Name = "lblRequestCount";
            this.lblRequestCount.Size = new System.Drawing.Size(19, 13);
            this.lblRequestCount.TabIndex = 78;
            this.lblRequestCount.Text = "[0]";
            // 
            // EditUserActionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblRequestCount);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.flpLink);
            this.Controls.Add(this.lblLinkTo);
            this.Controls.Add(this.picDelay);
            this.Controls.Add(this.picCopy);
            this.Controls.Add(this.btnShowHideParameterTokens);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnRevertToImported);
            this.Controls.Add(this.flpConfiguration);
            this.Controls.Add(this.splitParameterTokens);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.nudMoveSteps);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.picMoveDown);
            this.Controls.Add(this.picMoveUp);
            this.Controls.Add(this.lblMove);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.txtLabel);
            this.Name = "EditUserActionPanel";
            this.Size = new System.Drawing.Size(1000, 641);
            ((System.ComponentModel.ISupportInitialize)(this.nudMoveSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopy)).EndInit();
            this.splitParameterTokens.Panel1.ResumeLayout(false);
            this.splitParameterTokens.Panel2.ResumeLayout(false);
            this.splitParameterTokens.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitParameterTokens)).EndInit();
            this.splitParameterTokens.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tpStructured.ResumeLayout(false);
            this.tpStructured.PerformLayout();
            this.splitStructured.Panel1.ResumeLayout(false);
            this.splitStructured.Panel2.ResumeLayout(false);
            this.splitStructured.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitStructured)).EndInit();
            this.splitStructured.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fctxteditView)).EndInit();
            this.tpPlainText.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fctxtxPlainText)).EndInit();
            this.pnlBorderTokens.ResumeLayout(false);
            this.flpConfiguration.ResumeLayout(false);
            this.flpConfiguration.PerformLayout();
            this.flpLink.ResumeLayout(false);
            this.flpLink.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMoveDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMoveUp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Label lblMove;
        private System.Windows.Forms.PictureBox picMoveUp;
        private System.Windows.Forms.PictureBox picMoveDown;
        private System.Windows.Forms.Label lblSteps;
        private System.Windows.Forms.NumericUpDown nudMoveSteps;
        private System.Windows.Forms.DataGridView dgvRequests;
        private Util.TabControlWithAdjustableBorders tc;
        private System.Windows.Forms.TabPage tpStructured;
        private System.Windows.Forms.TabPage tpPlainText;
        private FastColoredTextBoxNS.FastColoredTextBox fctxtxPlainText;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer splitParameterTokens;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flpTokens;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FlowLayoutPanel flpConfiguration;
        private Util.LinkButton lbtnAsImported;
        private Util.LinkButton lbtnEditable;
        private System.Windows.Forms.Panel pnlBorderTokens;
        private System.Windows.Forms.ComboBox cboParameterScope;
        private System.Windows.Forms.Button btnShowHideParameterTokens;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnRevertToImported;
        private System.Windows.Forms.PictureBox picCopy;
        private System.Windows.Forms.PictureBox picDelay;
        private System.Windows.Forms.Label lblLinkTo;
        private System.Windows.Forms.FlowLayoutPanel flpLink;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Label lblRequestCount;
        private System.Windows.Forms.CheckBox chkUseEditView;
        private System.Windows.Forms.SplitContainer splitStructured;
        private FastColoredTextBoxNS.FastColoredTextBox fctxteditView;
        private System.Windows.Forms.Button btnApplyEditView;
    }
}
