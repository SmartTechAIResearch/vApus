﻿namespace vApus.Stresstest
{
    partial class StresstestView
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
            Stop();
            StopMonitors();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StresstestView));
            this.solutionComponentPropertyPanel = new vApus.SolutionTree.SolutionComponentPropertyPanel();
            this.tc = new vApus.Util.TabControlWithAdjustableBorders();
            this.tpConfigure = new System.Windows.Forms.TabPage();
            this.tpStresstest = new System.Windows.Forms.TabPage();
            this.fastResultsControl = new vApus.Stresstest.FastResultsControl();
            this.tpReport = new System.Windows.Forms.TabPage();
            this.detailedResultsControl = new vApus.Stresstest.Controls.DetailedResultsControl();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnSchedule = new System.Windows.Forms.ToolStripButton();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.tmrProgress = new System.Windows.Forms.Timer(this.components);
            this.tmrProgressDelayCountDown = new System.Windows.Forms.Timer(this.components);
            this.tmrSchedule = new System.Windows.Forms.Timer(this.components);
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.tc.SuspendLayout();
            this.tpConfigure.SuspendLayout();
            this.tpStresstest.SuspendLayout();
            this.tpReport.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // solutionComponentPropertyPanel
            // 
            this.solutionComponentPropertyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionComponentPropertyPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.solutionComponentPropertyPanel.Location = new System.Drawing.Point(3, 3);
            this.solutionComponentPropertyPanel.Margin = new System.Windows.Forms.Padding(0);
            this.solutionComponentPropertyPanel.Name = "solutionComponentPropertyPanel";
            this.solutionComponentPropertyPanel.Size = new System.Drawing.Size(784, 488);
            this.solutionComponentPropertyPanel.SolutionComponent = null;
            this.solutionComponentPropertyPanel.TabIndex = 1;
            // 
            // tc
            // 
            this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc.BottomVisible = false;
            this.tc.Controls.Add(this.tpConfigure);
            this.tc.Controls.Add(this.tpStresstest);
            this.tc.Controls.Add(this.tpReport);
            this.tc.LeftVisible = false;
            this.tc.Location = new System.Drawing.Point(0, 43);
            this.tc.Name = "tc";
            this.tc.RightVisible = false;
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(791, 517);
            this.tc.TabIndex = 2;
            this.tc.TopVisible = true;
            // 
            // tpConfigure
            // 
            this.tpConfigure.BackColor = System.Drawing.Color.White;
            this.tpConfigure.Controls.Add(this.solutionComponentPropertyPanel);
            this.tpConfigure.Location = new System.Drawing.Point(0, 22);
            this.tpConfigure.Name = "tpConfigure";
            this.tpConfigure.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfigure.Size = new System.Drawing.Size(790, 494);
            this.tpConfigure.TabIndex = 0;
            this.tpConfigure.Text = "Configure";
            // 
            // tpStresstest
            // 
            this.tpStresstest.BackColor = System.Drawing.Color.White;
            this.tpStresstest.Controls.Add(this.fastResultsControl);
            this.tpStresstest.Location = new System.Drawing.Point(0, 19);
            this.tpStresstest.Name = "tpStresstest";
            this.tpStresstest.Size = new System.Drawing.Size(790, 497);
            this.tpStresstest.TabIndex = 1;
            this.tpStresstest.Text = "Stresstest";
            // 
            // fastResultsControl
            // 
            this.fastResultsControl.BackColor = System.Drawing.Color.Transparent;
            this.fastResultsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastResultsControl.Location = new System.Drawing.Point(0, 0);
            this.fastResultsControl.Margin = new System.Windows.Forms.Padding(0);
            this.fastResultsControl.MonitorConfigurationControlAndLinkButtonsVisible = true;
            this.fastResultsControl.Name = "fastResultsControl";
            this.fastResultsControl.Size = new System.Drawing.Size(790, 497);
            this.fastResultsControl.TabIndex = 0;
            this.fastResultsControl.MonitorClicked += new System.EventHandler(this.stresstestControl_MonitorClicked);
            // 
            // tpReport
            // 
            this.tpReport.BackColor = System.Drawing.Color.White;
            this.tpReport.Controls.Add(this.detailedResultsControl);
            this.tpReport.Location = new System.Drawing.Point(0, 19);
            this.tpReport.Name = "tpReport";
            this.tpReport.Size = new System.Drawing.Size(790, 497);
            this.tpReport.TabIndex = 2;
            this.tpReport.Text = "Report";
            // 
            // detailedResultsControl
            // 
            this.detailedResultsControl.BackColor = System.Drawing.SystemColors.Control;
            this.detailedResultsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailedResultsControl.Location = new System.Drawing.Point(0, 0);
            this.detailedResultsControl.Name = "detailedResultsControl";
            this.detailedResultsControl.Size = new System.Drawing.Size(790, 497);
            this.detailedResultsControl.TabIndex = 0;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSchedule,
            this.btnStart,
            this.btnStop});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.MinimumSize = new System.Drawing.Size(0, 40);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(791, 40);
            this.toolStrip.TabIndex = 1;
            // 
            // btnSchedule
            // 
            this.btnSchedule.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSchedule.Image = ((System.Drawing.Image)(resources.GetObject("btnSchedule.Image")));
            this.btnSchedule.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(23, 37);
            this.btnSchedule.Tag = "";
            this.btnSchedule.ToolTipText = "Schedule...";
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            this.btnSchedule.MouseEnter += new System.EventHandler(this.btnSchedule_MouseEnter);
            this.btnSchedule.MouseLeave += new System.EventHandler(this.btnSchedule_MouseLeave);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 37);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(90, 37);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tmrProgress
            // 
            this.tmrProgress.Tick += new System.EventHandler(this.tmrProgress_Tick);
            // 
            // tmrProgressDelayCountDown
            // 
            this.tmrProgressDelayCountDown.Interval = 1000;
            this.tmrProgressDelayCountDown.Tick += new System.EventHandler(this.tmrProgressDelayCountDown_Tick);
            // 
            // tmrSchedule
            // 
            this.tmrSchedule.Tick += new System.EventHandler(this.tmrSchedule_Tick);
            // 
            // StresstestView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 562);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.tc);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StresstestView";
            this.Text = "StresstestView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StresstestView_FormClosing);
            this.tc.ResumeLayout(false);
            this.tpConfigure.ResumeLayout(false);
            this.tpStresstest.ResumeLayout(false);
            this.tpReport.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private vApus.SolutionTree.SolutionComponentPropertyPanel solutionComponentPropertyPanel;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStrip toolStrip;
        private vApus.Util.TabControlWithAdjustableBorders tc;
        private System.Windows.Forms.TabPage tpConfigure;
        private System.Windows.Forms.TabPage tpStresstest;
        private System.Windows.Forms.Timer tmrProgress;
        private System.Windows.Forms.Timer tmrProgressDelayCountDown;
        private System.Windows.Forms.ToolStripButton btnSchedule;
        private System.Windows.Forms.Timer tmrSchedule;
        private System.Windows.Forms.TabPage tpReport;
        private System.Windows.Forms.SaveFileDialog sfd;
        private FastResultsControl fastResultsControl;
        private Controls.DetailedResultsControl detailedResultsControl;

    }
}