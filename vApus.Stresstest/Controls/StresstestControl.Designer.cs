﻿namespace vApus.Stresstest
{
    partial class StresstestControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StresstestControl));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pnl = new System.Windows.Forms.Panel();
            this.pnlFastResultListing = new System.Windows.Forms.Panel();
            this.flpFastMetrics = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBorderDrillDown = new System.Windows.Forms.Panel();
            this.cboDrillDown = new System.Windows.Forms.ComboBox();
            this.lblStarted = new System.Windows.Forms.Label();
            this.lblMeasuredRuntime = new System.Windows.Forms.Label();
            this.btnRerunning = new System.Windows.Forms.Button();
            this.lblStopped = new System.Windows.Forms.Label();
            this.btnSaveDisplayedResults = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lvwFastResultsListing = new System.Windows.Forms.ListView();
            this.clmFRLStartedAt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLRuntimeLeft = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLMeasuredRuntime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLConcurrentUsers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLPrecision = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLRun = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLLogEntriesProcessed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLThroughput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLResponseTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLMaxResponseTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLDelay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmFRLErrors = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblUpdatesIn = new System.Windows.Forms.Label();
            this.flpConfiguration = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.kvpStresstest = new vApus.Util.KeyValuePairControl();
            this.kvpConnection = new vApus.Util.KeyValuePairControl();
            this.kvpConnectionProxy = new vApus.Util.KeyValuePairControl();
            this.kvpLog = new vApus.Util.KeyValuePairControl();
            this.kvpLogRuleSet = new vApus.Util.KeyValuePairControl();
            this.btnMonitor = new System.Windows.Forms.Button();
            this.kvpConcurrentUsers = new vApus.Util.KeyValuePairControl();
            this.kvpPrecision = new vApus.Util.KeyValuePairControl();
            this.kvpDynamicRunMultiplier = new vApus.Util.KeyValuePairControl();
            this.kvpDelay = new vApus.Util.KeyValuePairControl();
            this.kvpShuffle = new vApus.Util.KeyValuePairControl();
            this.kvpDistribute = new vApus.Util.KeyValuePairControl();
            this.epnlMessages = new vApus.Util.EventPanel();
            this.flpMetrics = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.kvmThreadsInUse = new vApus.Util.KeyValuePairControl();
            this.kvmCPUUsage = new vApus.Util.KeyValuePairControl();
            this.kvmContextSwitchesPerSecond = new vApus.Util.KeyValuePairControl();
            this.kvmMemoryUsage = new vApus.Util.KeyValuePairControl();
            this.kvmNicsSent = new vApus.Util.KeyValuePairControl();
            this.kvmNicsReceived = new vApus.Util.KeyValuePairControl();
            this.btnExport = new System.Windows.Forms.Button();
            this.kvpMonitorBefore = new vApus.Util.KeyValuePairControl();
            this.kvpMonitorAfter = new vApus.Util.KeyValuePairControl();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnl.SuspendLayout();
            this.pnlFastResultListing.SuspendLayout();
            this.flpFastMetrics.SuspendLayout();
            this.pnlBorderDrillDown.SuspendLayout();
            this.flpConfiguration.SuspendLayout();
            this.flpMetrics.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pnl);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.epnlMessages);
            this.splitContainer.Panel2.Controls.Add(this.flpMetrics);
            this.splitContainer.Size = new System.Drawing.Size(897, 639);
            this.splitContainer.SplitterDistance = 384;
            this.splitContainer.TabIndex = 2;
            // 
            // pnl
            // 
            this.pnl.Controls.Add(this.pnlFastResultListing);
            this.pnl.Controls.Add(this.flpConfiguration);
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(0, 0);
            this.pnl.Name = "pnl";
            this.pnl.Padding = new System.Windows.Forms.Padding(3);
            this.pnl.Size = new System.Drawing.Size(897, 384);
            this.pnl.TabIndex = 1;
            this.pnl.Text = "[Put title here]";
            // 
            // pnlFastResultListing
            // 
            this.pnlFastResultListing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFastResultListing.BackColor = System.Drawing.Color.White;
            this.pnlFastResultListing.Controls.Add(this.flpFastMetrics);
            this.pnlFastResultListing.Controls.Add(this.label4);
            this.pnlFastResultListing.Controls.Add(this.lvwFastResultsListing);
            this.pnlFastResultListing.Controls.Add(this.lblUpdatesIn);
            this.pnlFastResultListing.Location = new System.Drawing.Point(0, 104);
            this.pnlFastResultListing.Name = "pnlFastResultListing";
            this.pnlFastResultListing.Size = new System.Drawing.Size(897, 280);
            this.pnlFastResultListing.TabIndex = 1;
            this.pnlFastResultListing.Text = "Fast Results";
            // 
            // flpFastMetrics
            // 
            this.flpFastMetrics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpFastMetrics.Controls.Add(this.label1);
            this.flpFastMetrics.Controls.Add(this.pnlBorderDrillDown);
            this.flpFastMetrics.Controls.Add(this.lblStarted);
            this.flpFastMetrics.Controls.Add(this.lblMeasuredRuntime);
            this.flpFastMetrics.Controls.Add(this.btnRerunning);
            this.flpFastMetrics.Controls.Add(this.lblStopped);
            this.flpFastMetrics.Controls.Add(this.btnSaveDisplayedResults);
            this.flpFastMetrics.Location = new System.Drawing.Point(-1, 37);
            this.flpFastMetrics.Name = "flpFastMetrics";
            this.flpFastMetrics.Size = new System.Drawing.Size(899, 60);
            this.flpFastMetrics.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Drill down to";
            // 
            // pnlBorderDrillDown
            // 
            this.pnlBorderDrillDown.BackColor = System.Drawing.Color.Silver;
            this.pnlBorderDrillDown.Controls.Add(this.cboDrillDown);
            this.pnlBorderDrillDown.Location = new System.Drawing.Point(86, 3);
            this.pnlBorderDrillDown.Name = "pnlBorderDrillDown";
            this.pnlBorderDrillDown.Size = new System.Drawing.Size(127, 23);
            this.pnlBorderDrillDown.TabIndex = 0;
            // 
            // cboDrillDown
            // 
            this.cboDrillDown.BackColor = System.Drawing.Color.White;
            this.cboDrillDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDrillDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboDrillDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDrillDown.FormattingEnabled = true;
            this.cboDrillDown.Items.AddRange(new object[] {
            "Concurrent Users",
            "Precision",
            "Run"});
            this.cboDrillDown.Location = new System.Drawing.Point(1, 1);
            this.cboDrillDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.cboDrillDown.Name = "cboDrillDown";
            this.cboDrillDown.Size = new System.Drawing.Size(125, 21);
            this.cboDrillDown.TabIndex = 0;
            this.cboDrillDown.SelectedIndexChanged += new System.EventHandler(this.cboDrillDown_SelectedIndexChanged);
            // 
            // lblStarted
            // 
            this.lblStarted.AutoSize = true;
            this.lblStarted.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStarted.Location = new System.Drawing.Point(216, 6);
            this.lblStarted.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblStarted.Name = "lblStarted";
            this.lblStarted.Size = new System.Drawing.Size(0, 16);
            this.lblStarted.TabIndex = 0;
            // 
            // lblMeasuredRuntime
            // 
            this.lblMeasuredRuntime.AutoSize = true;
            this.lblMeasuredRuntime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMeasuredRuntime.Location = new System.Drawing.Point(216, 6);
            this.lblMeasuredRuntime.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblMeasuredRuntime.Name = "lblMeasuredRuntime";
            this.lblMeasuredRuntime.Size = new System.Drawing.Size(0, 16);
            this.lblMeasuredRuntime.TabIndex = 0;
            // 
            // btnRerunning
            // 
            this.btnRerunning.AutoSize = true;
            this.btnRerunning.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRerunning.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnRerunning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRerunning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRerunning.ForeColor = System.Drawing.Color.White;
            this.btnRerunning.Location = new System.Drawing.Point(219, 3);
            this.btnRerunning.MaximumSize = new System.Drawing.Size(89, 24);
            this.btnRerunning.MinimumSize = new System.Drawing.Size(89, 24);
            this.btnRerunning.Name = "btnRerunning";
            this.btnRerunning.Size = new System.Drawing.Size(89, 24);
            this.btnRerunning.TabIndex = 2;
            this.btnRerunning.Text = "Rerunning...";
            this.btnRerunning.UseVisualStyleBackColor = false;
            this.btnRerunning.Visible = false;
            this.btnRerunning.Click += new System.EventHandler(this.btnRerunning_Click);
            // 
            // lblStopped
            // 
            this.lblStopped.AutoSize = true;
            this.lblStopped.BackColor = System.Drawing.SystemColors.Control;
            this.lblStopped.Font = new System.Drawing.Font("Consolas", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStopped.Location = new System.Drawing.Point(317, 3);
            this.lblStopped.Margin = new System.Windows.Forms.Padding(6, 3, 6, 0);
            this.lblStopped.Name = "lblStopped";
            this.lblStopped.Size = new System.Drawing.Size(0, 20);
            this.lblStopped.TabIndex = 0;
            // 
            // btnSaveDisplayedResults
            // 
            this.btnSaveDisplayedResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveDisplayedResults.AutoSize = true;
            this.btnSaveDisplayedResults.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSaveDisplayedResults.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveDisplayedResults.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnSaveDisplayedResults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDisplayedResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDisplayedResults.Location = new System.Drawing.Point(326, 3);
            this.btnSaveDisplayedResults.MaximumSize = new System.Drawing.Size(165, 24);
            this.btnSaveDisplayedResults.Name = "btnSaveDisplayedResults";
            this.btnSaveDisplayedResults.Size = new System.Drawing.Size(165, 24);
            this.btnSaveDisplayedResults.TabIndex = 3;
            this.btnSaveDisplayedResults.Text = "Save Displayed Results...";
            this.btnSaveDisplayedResults.UseVisualStyleBackColor = false;
            this.btnSaveDisplayedResults.Click += new System.EventHandler(this.btnSaveDisplayedResults_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(2, 7);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "Fast Results Listing";
            // 
            // lvwFastResultsListing
            // 
            this.lvwFastResultsListing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFastResultsListing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwFastResultsListing.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmFRLStartedAt,
            this.clmFRLRuntimeLeft,
            this.clmFRLMeasuredRuntime,
            this.clmFRLConcurrentUsers,
            this.clmFRLPrecision,
            this.clmFRLRun,
            this.clmFRLLogEntriesProcessed,
            this.clmFRLThroughput,
            this.clmFRLResponseTime,
            this.clmFRLMaxResponseTime,
            this.clmFRLDelay,
            this.clmFRLErrors});
            this.lvwFastResultsListing.FullRowSelect = true;
            this.lvwFastResultsListing.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwFastResultsListing.Location = new System.Drawing.Point(-1, 97);
            this.lvwFastResultsListing.MultiSelect = false;
            this.lvwFastResultsListing.Name = "lvwFastResultsListing";
            this.lvwFastResultsListing.Size = new System.Drawing.Size(899, 184);
            this.lvwFastResultsListing.TabIndex = 2;
            this.lvwFastResultsListing.UseCompatibleStateImageBehavior = false;
            this.lvwFastResultsListing.View = System.Windows.Forms.View.Details;
            // 
            // clmFRLStartedAt
            // 
            this.clmFRLStartedAt.Text = "Started At";
            this.clmFRLStartedAt.Width = 165;
            // 
            // clmFRLRuntimeLeft
            // 
            this.clmFRLRuntimeLeft.Text = "Time Left";
            this.clmFRLRuntimeLeft.Width = 140;
            // 
            // clmFRLMeasuredRuntime
            // 
            this.clmFRLMeasuredRuntime.Text = "Measured Time";
            this.clmFRLMeasuredRuntime.Width = 140;
            // 
            // clmFRLConcurrentUsers
            // 
            this.clmFRLConcurrentUsers.Text = "Concurrent Users";
            this.clmFRLConcurrentUsers.Width = 94;
            // 
            // clmFRLPrecision
            // 
            this.clmFRLPrecision.Text = "Precision";
            this.clmFRLPrecision.Width = 55;
            // 
            // clmFRLRun
            // 
            this.clmFRLRun.Text = "Run";
            this.clmFRLRun.Width = 32;
            // 
            // clmFRLLogEntriesProcessed
            // 
            this.clmFRLLogEntriesProcessed.Text = "Log Entries Processed";
            this.clmFRLLogEntriesProcessed.Width = 130;
            // 
            // clmFRLThroughput
            // 
            this.clmFRLThroughput.Text = "Throughput / s";
            this.clmFRLThroughput.Width = 91;
            // 
            // clmFRLResponseTime
            // 
            this.clmFRLResponseTime.Text = "Response Time in ms";
            this.clmFRLResponseTime.Width = 113;
            // 
            // clmFRLMaxResponseTime
            // 
            this.clmFRLMaxResponseTime.Text = "Max. Response Time";
            this.clmFRLMaxResponseTime.Width = 112;
            // 
            // clmFRLDelay
            // 
            this.clmFRLDelay.Text = "Delay in ms";
            this.clmFRLDelay.Width = 80;
            // 
            // clmFRLErrors
            // 
            this.clmFRLErrors.Text = "Errors";
            // 
            // lblUpdatesIn
            // 
            this.lblUpdatesIn.AutoSize = true;
            this.lblUpdatesIn.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdatesIn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUpdatesIn.Location = new System.Drawing.Point(157, 9);
            this.lblUpdatesIn.Name = "lblUpdatesIn";
            this.lblUpdatesIn.Size = new System.Drawing.Size(0, 18);
            this.lblUpdatesIn.TabIndex = 0;
            this.lblUpdatesIn.Visible = false;
            // 
            // flpConfiguration
            // 
            this.flpConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpConfiguration.AutoScroll = true;
            this.flpConfiguration.BackColor = System.Drawing.Color.White;
            this.flpConfiguration.Controls.Add(this.label3);
            this.flpConfiguration.Controls.Add(this.kvpStresstest);
            this.flpConfiguration.Controls.Add(this.kvpConnection);
            this.flpConfiguration.Controls.Add(this.kvpConnectionProxy);
            this.flpConfiguration.Controls.Add(this.kvpLog);
            this.flpConfiguration.Controls.Add(this.kvpLogRuleSet);
            this.flpConfiguration.Controls.Add(this.btnMonitor);
            this.flpConfiguration.Controls.Add(this.kvpConcurrentUsers);
            this.flpConfiguration.Controls.Add(this.kvpPrecision);
            this.flpConfiguration.Controls.Add(this.kvpDynamicRunMultiplier);
            this.flpConfiguration.Controls.Add(this.kvpDelay);
            this.flpConfiguration.Controls.Add(this.kvpShuffle);
            this.flpConfiguration.Controls.Add(this.kvpDistribute);
            this.flpConfiguration.Controls.Add(this.kvpMonitorBefore);
            this.flpConfiguration.Controls.Add(this.kvpMonitorAfter);
            this.flpConfiguration.Location = new System.Drawing.Point(0, 0);
            this.flpConfiguration.Name = "flpConfiguration";
            this.flpConfiguration.Size = new System.Drawing.Size(897, 105);
            this.flpConfiguration.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Configuration";
            // 
            // kvpStresstest
            // 
            this.kvpStresstest.BackColor = System.Drawing.Color.LightBlue;
            this.kvpStresstest.Key = "Stresstest";
            this.kvpStresstest.Location = new System.Drawing.Point(116, 6);
            this.kvpStresstest.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpStresstest.Name = "kvpStresstest";
            this.kvpStresstest.Size = new System.Drawing.Size(69, 24);
            this.kvpStresstest.TabIndex = 14;
            this.kvpStresstest.TabStop = false;
            this.kvpStresstest.Tooltip = "";
            this.kvpStresstest.Value = "";
            // 
            // kvpConnection
            // 
            this.kvpConnection.BackColor = System.Drawing.Color.LightBlue;
            this.kvpConnection.Key = "Connection";
            this.kvpConnection.Location = new System.Drawing.Point(188, 6);
            this.kvpConnection.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpConnection.Name = "kvpConnection";
            this.kvpConnection.Size = new System.Drawing.Size(77, 24);
            this.kvpConnection.TabIndex = 8;
            this.kvpConnection.TabStop = false;
            this.kvpConnection.Tooltip = "The connection to the application to test.";
            this.kvpConnection.Value = "";
            // 
            // kvpConnectionProxy
            // 
            this.kvpConnectionProxy.BackColor = System.Drawing.SystemColors.Control;
            this.kvpConnectionProxy.Key = "Connection Proxy";
            this.kvpConnectionProxy.Location = new System.Drawing.Point(268, 6);
            this.kvpConnectionProxy.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpConnectionProxy.Name = "kvpConnectionProxy";
            this.kvpConnectionProxy.Size = new System.Drawing.Size(112, 24);
            this.kvpConnectionProxy.TabIndex = 15;
            this.kvpConnectionProxy.TabStop = false;
            this.kvpConnectionProxy.Tooltip = "This is used in and defines the connection.";
            this.kvpConnectionProxy.Value = "";
            // 
            // kvpLog
            // 
            this.kvpLog.BackColor = System.Drawing.Color.LightBlue;
            this.kvpLog.Key = "Log";
            this.kvpLog.Location = new System.Drawing.Point(383, 6);
            this.kvpLog.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpLog.Name = "kvpLog";
            this.kvpLog.Size = new System.Drawing.Size(34, 24);
            this.kvpLog.TabIndex = 8;
            this.kvpLog.TabStop = false;
            this.kvpLog.Tooltip = "The log used to test the application.";
            this.kvpLog.Value = "";
            // 
            // kvpLogRuleSet
            // 
            this.kvpLogRuleSet.BackColor = System.Drawing.SystemColors.Control;
            this.kvpLogRuleSet.Key = "Log Rule Set";
            this.kvpLogRuleSet.Location = new System.Drawing.Point(420, 6);
            this.kvpLogRuleSet.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpLogRuleSet.Name = "kvpLogRuleSet";
            this.kvpLogRuleSet.Size = new System.Drawing.Size(87, 24);
            this.kvpLogRuleSet.TabIndex = 17;
            this.kvpLogRuleSet.TabStop = false;
            this.kvpLogRuleSet.Tooltip = "This is used in and defines the log entries.";
            this.kvpLogRuleSet.Value = "";
            // 
            // btnMonitor
            // 
            this.btnMonitor.AutoSize = true;
            this.btnMonitor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMonitor.BackColor = System.Drawing.Color.LightBlue;
            this.btnMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMonitor.Location = new System.Drawing.Point(510, 5);
            this.btnMonitor.Margin = new System.Windows.Forms.Padding(3, 5, 0, 3);
            this.btnMonitor.MaximumSize = new System.Drawing.Size(1000, 24);
            this.btnMonitor.Name = "btnMonitor";
            this.btnMonitor.Size = new System.Drawing.Size(73, 24);
            this.btnMonitor.TabIndex = 18;
            this.btnMonitor.Text = "Monitor...";
            this.toolTip.SetToolTip(this.btnMonitor, "The monitors used to link stresstest results to performance counters. Maximum 5 a" +
        "llowed.");
            this.btnMonitor.UseVisualStyleBackColor = false;
            this.btnMonitor.Click += new System.EventHandler(this.btnMonitor_Click);
            // 
            // kvpConcurrentUsers
            // 
            this.kvpConcurrentUsers.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpConcurrentUsers.Key = "Concurrent Users";
            this.kvpConcurrentUsers.Location = new System.Drawing.Point(595, 6);
            this.kvpConcurrentUsers.Margin = new System.Windows.Forms.Padding(12, 6, 0, 3);
            this.kvpConcurrentUsers.Name = "kvpConcurrentUsers";
            this.kvpConcurrentUsers.Size = new System.Drawing.Size(111, 24);
            this.kvpConcurrentUsers.TabIndex = 8;
            this.kvpConcurrentUsers.TabStop = false;
            this.kvpConcurrentUsers.Tooltip = "The count(s) of the concurrent users generated, the minimum given value equals on" +
    "e.";
            this.kvpConcurrentUsers.Value = "";
            // 
            // kvpPrecision
            // 
            this.kvpPrecision.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpPrecision.Key = "Precision";
            this.kvpPrecision.Location = new System.Drawing.Point(709, 6);
            this.kvpPrecision.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpPrecision.Name = "kvpPrecision";
            this.kvpPrecision.Size = new System.Drawing.Size(65, 24);
            this.kvpPrecision.TabIndex = 8;
            this.kvpPrecision.TabStop = false;
            this.kvpPrecision.Tooltip = "A static multiplier of the runtime for each concurrency level. Must be greater th" +
    "an zero.";
            this.kvpPrecision.Value = "";
            // 
            // kvpDynamicRunMultiplier
            // 
            this.kvpDynamicRunMultiplier.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpDynamicRunMultiplier.Key = "Dynamic Run Multiplier";
            this.kvpDynamicRunMultiplier.Location = new System.Drawing.Point(3, 39);
            this.kvpDynamicRunMultiplier.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpDynamicRunMultiplier.Name = "kvpDynamicRunMultiplier";
            this.kvpDynamicRunMultiplier.Size = new System.Drawing.Size(143, 24);
            this.kvpDynamicRunMultiplier.TabIndex = 8;
            this.kvpDynamicRunMultiplier.TabStop = false;
            this.kvpDynamicRunMultiplier.Tooltip = resources.GetString("kvpDynamicRunMultiplier.Tooltip");
            this.kvpDynamicRunMultiplier.Value = "";
            // 
            // kvpDelay
            // 
            this.kvpDelay.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpDelay.Key = "Delay";
            this.kvpDelay.Location = new System.Drawing.Point(149, 39);
            this.kvpDelay.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpDelay.Name = "kvpDelay";
            this.kvpDelay.Size = new System.Drawing.Size(45, 24);
            this.kvpDelay.TabIndex = 9;
            this.kvpDelay.TabStop = false;
            this.kvpDelay.Tooltip = "The delay in milliseconds between the execution of log entries per user.\r\nKeep th" +
    "is zero to have an ASAP test.";
            this.kvpDelay.Value = "";
            // 
            // kvpShuffle
            // 
            this.kvpShuffle.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpShuffle.Key = "Shuffle";
            this.kvpShuffle.Location = new System.Drawing.Point(197, 39);
            this.kvpShuffle.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpShuffle.Name = "kvpShuffle";
            this.kvpShuffle.Size = new System.Drawing.Size(53, 24);
            this.kvpShuffle.TabIndex = 10;
            this.kvpShuffle.TabStop = false;
            this.kvpShuffle.Tooltip = "The actions and loose log entries will be shuffled for each concurrent user when " +
    "testing, creating unique usage patterns.";
            this.kvpShuffle.Value = "";
            // 
            // kvpDistribute
            // 
            this.kvpDistribute.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpDistribute.Key = "Distribute";
            this.kvpDistribute.Location = new System.Drawing.Point(253, 39);
            this.kvpDistribute.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpDistribute.Name = "kvpDistribute";
            this.kvpDistribute.Size = new System.Drawing.Size(67, 24);
            this.kvpDistribute.TabIndex = 10;
            this.kvpDistribute.TabStop = false;
            this.kvpDistribute.Tooltip = "Action and Loose Log Entry Distribution\nFast: The length of the log stays the sam" +
    "e, entries are picked by chance based on the occurance.\nFull: entries are execut" +
    "ed X times the occurance.";
            this.kvpDistribute.Value = "";
            // 
            // epnlMessages
            // 
            this.epnlMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.epnlMessages.BackColor = System.Drawing.SystemColors.Control;
            this.epnlMessages.BeginOfTimeFrame = new System.DateTime(((long)(0)));
            this.epnlMessages.Collapsed = false;
            this.epnlMessages.Cursor = System.Windows.Forms.Cursors.Default;
            this.epnlMessages.EndOfTimeFrame = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.epnlMessages.ExpandOnErrorEvent = true;
            this.epnlMessages.Location = new System.Drawing.Point(0, 61);
            this.epnlMessages.Margin = new System.Windows.Forms.Padding(0);
            this.epnlMessages.Name = "epnlMessages";
            this.epnlMessages.ProgressBarColor = System.Drawing.Color.SteelBlue;
            this.epnlMessages.Size = new System.Drawing.Size(897, 190);
            this.epnlMessages.TabIndex = 3;
            this.epnlMessages.CollapsedChanged += new System.EventHandler(this.epnlMessages_CollapsedChanged);
            // 
            // flpMetrics
            // 
            this.flpMetrics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpMetrics.BackColor = System.Drawing.Color.White;
            this.flpMetrics.Controls.Add(this.label2);
            this.flpMetrics.Controls.Add(this.kvmThreadsInUse);
            this.flpMetrics.Controls.Add(this.kvmCPUUsage);
            this.flpMetrics.Controls.Add(this.kvmContextSwitchesPerSecond);
            this.flpMetrics.Controls.Add(this.kvmMemoryUsage);
            this.flpMetrics.Controls.Add(this.kvmNicsSent);
            this.flpMetrics.Controls.Add(this.kvmNicsReceived);
            this.flpMetrics.Controls.Add(this.btnExport);
            this.flpMetrics.Location = new System.Drawing.Point(0, 0);
            this.flpMetrics.Margin = new System.Windows.Forms.Padding(0);
            this.flpMetrics.Name = "flpMetrics";
            this.flpMetrics.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.flpMetrics.Size = new System.Drawing.Size(897, 61);
            this.flpMetrics.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(312, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Client Monitoring and Stresstest Messages";
            // 
            // kvmThreadsInUse
            // 
            this.kvmThreadsInUse.BackColor = System.Drawing.Color.GhostWhite;
            this.kvmThreadsInUse.Key = "Threads in Use";
            this.kvmThreadsInUse.Location = new System.Drawing.Point(324, 9);
            this.kvmThreadsInUse.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.kvmThreadsInUse.Name = "kvmThreadsInUse";
            this.kvmThreadsInUse.Size = new System.Drawing.Size(112, 16);
            this.kvmThreadsInUse.TabIndex = 8;
            this.kvmThreadsInUse.TabStop = false;
            this.kvmThreadsInUse.Tooltip = "The number of threads in use should remain equal to the concurrent users just unt" +
    "ill the end of the test for tests with low delays.";
            this.kvmThreadsInUse.Value = "0";
            // 
            // kvmCPUUsage
            // 
            this.kvmCPUUsage.BackColor = System.Drawing.Color.GhostWhite;
            this.kvmCPUUsage.Key = "CPU Usage";
            this.kvmCPUUsage.Location = new System.Drawing.Point(439, 9);
            this.kvmCPUUsage.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.kvmCPUUsage.Name = "kvmCPUUsage";
            this.kvmCPUUsage.Size = new System.Drawing.Size(105, 16);
            this.kvmCPUUsage.TabIndex = 8;
            this.kvmCPUUsage.TabStop = false;
            this.kvmCPUUsage.Tooltip = "Try to keep this below 60 % to ensure that the client is not the bottleneck.";
            this.kvmCPUUsage.Value = "N/A";
            // 
            // kvmContextSwitchesPerSecond
            // 
            this.kvmContextSwitchesPerSecond.BackColor = System.Drawing.Color.GhostWhite;
            this.kvmContextSwitchesPerSecond.Key = "Context Switches / s";
            this.kvmContextSwitchesPerSecond.Location = new System.Drawing.Point(547, 9);
            this.kvmContextSwitchesPerSecond.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.kvmContextSwitchesPerSecond.Name = "kvmContextSwitchesPerSecond";
            this.kvmContextSwitchesPerSecond.Size = new System.Drawing.Size(158, 16);
            this.kvmContextSwitchesPerSecond.TabIndex = 8;
            this.kvmContextSwitchesPerSecond.TabStop = false;
            this.kvmContextSwitchesPerSecond.Tooltip = "";
            this.kvmContextSwitchesPerSecond.Value = "N/A";
            // 
            // kvmMemoryUsage
            // 
            this.kvmMemoryUsage.BackColor = System.Drawing.Color.GhostWhite;
            this.kvmMemoryUsage.Key = "Memory Usage";
            this.kvmMemoryUsage.Location = new System.Drawing.Point(708, 9);
            this.kvmMemoryUsage.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.kvmMemoryUsage.Name = "kvmMemoryUsage";
            this.kvmMemoryUsage.Size = new System.Drawing.Size(123, 16);
            this.kvmMemoryUsage.TabIndex = 8;
            this.kvmMemoryUsage.TabStop = false;
            this.kvmMemoryUsage.Tooltip = "Make sure you have sufficient memory to ensure that the client is not the bottlen" +
    "eck.";
            this.kvmMemoryUsage.Value = "N/A";
            // 
            // kvmNicsSent
            // 
            this.kvmNicsSent.BackColor = System.Drawing.Color.GhostWhite;
            this.kvmNicsSent.Key = "NIC Usage (Sent)";
            this.kvmNicsSent.Location = new System.Drawing.Point(3, 35);
            this.kvmNicsSent.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.kvmNicsSent.Name = "kvmNicsSent";
            this.kvmNicsSent.Size = new System.Drawing.Size(139, 16);
            this.kvmNicsSent.TabIndex = 10;
            this.kvmNicsSent.TabStop = false;
            this.kvmNicsSent.Tooltip = "Make sure that the NIC is not the bottleneck (Most used displayed).";
            this.kvmNicsSent.Value = "N/A";
            // 
            // kvmNicsReceived
            // 
            this.kvmNicsReceived.BackColor = System.Drawing.Color.GhostWhite;
            this.kvmNicsReceived.Key = "NIC Usage (Received)";
            this.kvmNicsReceived.Location = new System.Drawing.Point(145, 35);
            this.kvmNicsReceived.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.kvmNicsReceived.Name = "kvmNicsReceived";
            this.kvmNicsReceived.Size = new System.Drawing.Size(167, 16);
            this.kvmNicsReceived.TabIndex = 11;
            this.kvmNicsReceived.TabStop = false;
            this.kvmNicsReceived.Tooltip = "Make sure that the NIC is not the bottleneck (Most used displayed).";
            this.kvmNicsReceived.Value = "N/A";
            // 
            // btnExport
            // 
            this.btnExport.AutoSize = true;
            this.btnExport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExport.BackColor = System.Drawing.SystemColors.Control;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(324, 29);
            this.btnExport.Margin = new System.Windows.Forms.Padding(12, 0, 3, 3);
            this.btnExport.MaximumSize = new System.Drawing.Size(127, 24);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(127, 24);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export Messages...";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // kvpMonitorBefore
            // 
            this.kvpMonitorBefore.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpMonitorBefore.Key = "Monitor Before";
            this.kvpMonitorBefore.Location = new System.Drawing.Point(323, 39);
            this.kvpMonitorBefore.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpMonitorBefore.Name = "kvpMonitorBefore";
            this.kvpMonitorBefore.Size = new System.Drawing.Size(96, 24);
            this.kvpMonitorBefore.TabIndex = 19;
            this.kvpMonitorBefore.TabStop = false;
            this.kvpMonitorBefore.Tooltip = "Start monitoring before the test starts, expressed in minutes with a max of 60.";
            this.kvpMonitorBefore.Value = "";
            // 
            // kvpMonitorAfter
            // 
            this.kvpMonitorAfter.BackColor = System.Drawing.Color.GhostWhite;
            this.kvpMonitorAfter.Key = "Monitor After";
            this.kvpMonitorAfter.Location = new System.Drawing.Point(422, 39);
            this.kvpMonitorAfter.Margin = new System.Windows.Forms.Padding(3, 6, 0, 3);
            this.kvpMonitorAfter.Name = "kvpMonitorAfter";
            this.kvpMonitorAfter.Size = new System.Drawing.Size(86, 24);
            this.kvpMonitorAfter.TabIndex = 20;
            this.kvpMonitorAfter.TabStop = false;
            this.kvpMonitorAfter.Tooltip = "Continue monitoring after the test is finished, expressed in minutes with a max o" +
    "f 60.";
            this.kvpMonitorAfter.Value = "";
            // 
            // StresstestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer);
            this.Name = "StresstestControl";
            this.Size = new System.Drawing.Size(897, 639);
            this.SizeChanged += new System.EventHandler(this.StresstestControl_SizeChanged);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnl.ResumeLayout(false);
            this.pnlFastResultListing.ResumeLayout(false);
            this.pnlFastResultListing.PerformLayout();
            this.flpFastMetrics.ResumeLayout(false);
            this.flpFastMetrics.PerformLayout();
            this.pnlBorderDrillDown.ResumeLayout(false);
            this.flpConfiguration.ResumeLayout(false);
            this.flpConfiguration.PerformLayout();
            this.flpMetrics.ResumeLayout(false);
            this.flpMetrics.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.FlowLayoutPanel flpConfiguration;
        private System.Windows.Forms.Label label3;
        private vApus.Util.KeyValuePairControl kvpStresstest;
        private vApus.Util.KeyValuePairControl kvpConnection;
        private vApus.Util.KeyValuePairControl kvpConnectionProxy;
        private vApus.Util.KeyValuePairControl kvpLog;
        private vApus.Util.KeyValuePairControl kvpConcurrentUsers;
        private vApus.Util.KeyValuePairControl kvpPrecision;
        private vApus.Util.KeyValuePairControl kvpDynamicRunMultiplier;
        private vApus.Util.KeyValuePairControl kvpDelay;
        private vApus.Util.KeyValuePairControl kvpShuffle;
        private vApus.Util.KeyValuePairControl kvpDistribute;
        private System.Windows.Forms.Panel pnlFastResultListing;
        private System.Windows.Forms.FlowLayoutPanel flpFastMetrics;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDrillDown;
        private System.Windows.Forms.Label lblStarted;
        private System.Windows.Forms.Label lblMeasuredRuntime;
        private System.Windows.Forms.Label lblStopped;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lvwFastResultsListing;
        private System.Windows.Forms.ColumnHeader clmFRLStartedAt;
        private System.Windows.Forms.ColumnHeader clmFRLRuntimeLeft;
        private System.Windows.Forms.ColumnHeader clmFRLMeasuredRuntime;
        private System.Windows.Forms.ColumnHeader clmFRLConcurrentUsers;
        private System.Windows.Forms.ColumnHeader clmFRLPrecision;
        private System.Windows.Forms.ColumnHeader clmFRLRun;
        private System.Windows.Forms.ColumnHeader clmFRLLogEntriesProcessed;
        private System.Windows.Forms.ColumnHeader clmFRLThroughput;
        private System.Windows.Forms.ColumnHeader clmFRLResponseTime;
        private System.Windows.Forms.ColumnHeader clmFRLMaxResponseTime;
        private System.Windows.Forms.ColumnHeader clmFRLDelay;
        private System.Windows.Forms.ColumnHeader clmFRLErrors;
        private System.Windows.Forms.Button btnSaveDisplayedResults;
        private System.Windows.Forms.Label lblUpdatesIn;
        private System.Windows.Forms.FlowLayoutPanel flpMetrics;
        private System.Windows.Forms.Label label2;
        private vApus.Util.KeyValuePairControl kvmThreadsInUse;
        private vApus.Util.KeyValuePairControl kvmCPUUsage;
        private vApus.Util.KeyValuePairControl kvmContextSwitchesPerSecond;
        private vApus.Util.KeyValuePairControl kvmMemoryUsage;
        private vApus.Util.KeyValuePairControl kvmNicsSent;
        private vApus.Util.KeyValuePairControl kvmNicsReceived;
        private vApus.Util.KeyValuePairControl kvpLogRuleSet;
        private Util.EventPanel epnlMessages;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnMonitor;
        private System.Windows.Forms.Button btnRerunning;
        private System.Windows.Forms.Panel pnlBorderDrillDown;
        private Util.KeyValuePairControl kvpMonitorBefore;
        private Util.KeyValuePairControl kvpMonitorAfter;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
