﻿/*
 * Copyright 2015 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vApus.Util {
    /// <summary>
    /// Used in vApus.Util.OptionsDialog.
    /// </summary>
    public partial class ExportingResultsPanel : Panel {

        #region Constructor
        /// <summary>
        /// Used in vApus.Util.OptionsDialog.
        /// </summary>
        public ExportingResultsPanel() {
            InitializeComponent();
            if (IsHandleCreated) SetGui();
            else HandleCreated += ExportingResultsPanel_HandleCreated;

        }
        #endregion

        #region Functions
        private void ExportingResultsPanel_HandleCreated(object sender, EventArgs e) {
            HandleCreated -= ExportingResultsPanel_HandleCreated;
            SetGui();
        }

        private void SetGui() {
            if (ExportingResultsManager.Enabled) btnEnableDisable.PerformClick();
            txtFolder.Text = ExportingResultsManager.Folder;
        }
        private void btnBrowse_Click(object sender, EventArgs e) {
            if (fbd.ShowDialog() == DialogResult.OK)
                txtFolder.Text = fbd.SelectedPath;
        }

        private void txtFolder_TextChanged(object sender, EventArgs e) {
            ExportingResultsManager.Folder = txtFolder.Text;
        }

        private void btnEnableDisable_Click(object sender, EventArgs e) {
            if (btnEnableDisable.Text == "Disable") {
                btnEnableDisable.Text = "Enable";
                grp.Enabled = ExportingResultsManager.Enabled = false;
                txtFolder.BackColor = SystemColors.Control;
            } else {
                btnEnableDisable.Text = "Disable";
                grp.Enabled = ExportingResultsManager.Enabled = true;
                txtFolder.BackColor = Color.White;
            }
        }

        public override string ToString() {
            return "Exporting Test Results";
        }

        #endregion
    }
}