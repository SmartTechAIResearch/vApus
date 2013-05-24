﻿/*
 * Copyright 2013 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using vApus.Results;
using WeifenLuo.WinFormsUI.Docking;

namespace vApus.DetailedResultsViewer {
    public partial class ResultsPanel : DockablePanel {

        public event EventHandler ResultsDeleted;
        
        private ResultsHelper _resultsHelper;

        public ResultsHelper ResultsHelper {
            get { return _resultsHelper; }
            set { _resultsHelper = value; }
        }
        /// <summary>
        /// Don't forget to set ResultsHelper.
        /// </summary>
        public ResultsPanel() {
            InitializeComponent();
        }
        public void ClearReport() {
            this.Enabled = false;
            detailedResultsControl.ClearResults();
            this.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stresstestId">0 for all</param>
        public void RefreshReport(ulong stresstestId) {
            this.Enabled = false;
            if (stresstestId == 0) detailedResultsControl.RefreshResults(_resultsHelper); else detailedResultsControl.RefreshResults(_resultsHelper, stresstestId);
            this.Enabled = true;
        }

        private void detailedResultsControl_ResultsDeleted(object sender, System.EventArgs e) {
            if (ResultsDeleted != null) ResultsDeleted(this, null);
        }
    }
}