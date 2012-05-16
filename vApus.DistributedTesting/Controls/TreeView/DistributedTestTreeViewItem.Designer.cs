﻿namespace vApus.DistributedTesting
{
    partial class DistributedTestTreeViewItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistributedTestTreeViewItem));
            this.pnlRunSync = new System.Windows.Forms.Panel();
            this.cboRunSync = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.picAddTile = new System.Windows.Forms.PictureBox();
            this.pnlRunSync.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddTile)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRunSync
            // 
            this.pnlRunSync.BackColor = System.Drawing.Color.Silver;
            this.pnlRunSync.Controls.Add(this.cboRunSync);
            this.pnlRunSync.Location = new System.Drawing.Point(66, 6);
            this.pnlRunSync.Name = "pnlRunSync";
            this.pnlRunSync.Size = new System.Drawing.Size(127, 23);
            this.pnlRunSync.TabIndex = 18;
            // 
            // cboRunSync
            // 
            this.cboRunSync.BackColor = System.Drawing.Color.White;
            this.cboRunSync.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRunSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboRunSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRunSync.FormattingEnabled = true;
            this.cboRunSync.Items.AddRange(new object[] {
            "None",
            "Break on First",
            "Break on Last"});
            this.cboRunSync.Location = new System.Drawing.Point(1, 1);
            this.cboRunSync.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.cboRunSync.Name = "cboRunSync";
            this.cboRunSync.Size = new System.Drawing.Size(125, 21);
            this.cboRunSync.TabIndex = 0;
            this.toolTip.SetToolTip(this.cboRunSync, resources.GetString("cboRunSync.ToolTip"));
            this.cboRunSync.Enter += new System.EventHandler(this._Enter);
            this.cboRunSync.KeyDown += new System.Windows.Forms.KeyEventHandler(this._KeyDown);
            this.cboRunSync.KeyUp += new System.Windows.Forms.KeyEventHandler(this._KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Run Sync:";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // picAddTile
            // 
            this.picAddTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picAddTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAddTile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAddTile.Image = ((System.Drawing.Image)(resources.GetObject("picAddTile.Image")));
            this.picAddTile.Location = new System.Drawing.Point(251, 6);
            this.picAddTile.Name = "picAddTile";
            this.picAddTile.Size = new System.Drawing.Size(23, 23);
            this.picAddTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAddTile.TabIndex = 20;
            this.picAddTile.TabStop = false;
            this.toolTip.SetToolTip(this.picAddTile, "Add Tile Stresstest <ctrl+i>");
            this.picAddTile.Click += new System.EventHandler(this.picAddTile_Click);
            // 
            // DistributedTestTreeViewItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picAddTile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlRunSync);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DistributedTestTreeViewItem";
            this.Size = new System.Drawing.Size(274, 35);
            this.Enter += new System.EventHandler(this._Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this._KeyUp);
            this.pnlRunSync.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAddTile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRunSync;
        private System.Windows.Forms.ComboBox cboRunSync;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox picAddTile;

    }
}
