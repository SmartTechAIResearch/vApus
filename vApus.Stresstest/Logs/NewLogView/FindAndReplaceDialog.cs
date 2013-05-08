﻿/*
 * Copyright 2013 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Windows.Forms;

namespace vApus.Stresstest {
    public partial class FindAndReplaceDialog : Form {
        public event EventHandler<FindEventArgs> FindClicked;
        public event EventHandler<ReplaceEventArgs> ReplaceClicked;

        public FindAndReplaceDialog() {
            InitializeComponent();
        }
        public void SetFind(string find) {
            txtFind.Text = find;
        }

        private void btnFind_Click(object sender, EventArgs e) {
            if(FindClicked != null) FindClicked(this, new FindEventArgs(txtFind.Text, !chkMatchCase.Checked));
        }

        private void btnReplaceWith_Click(object sender, EventArgs e) {
            if(ReplaceClicked != null) ReplaceClicked(this, new ReplaceEventArgs(txtFind.Text, !chkMatchCase.Checked, txtReplace.Text, chkReplaceAll.Checked));
        }
        private void txtFind_TextChanged(object sender, EventArgs e) {
            btnFind.Enabled = txtFind.Text.Length != 0;
        }

        private void txtReplace_TextChanged(object sender, EventArgs e) {
            btnReplaceWith.Enabled = txtFind.Text.Length != 0 && txtReplace.Text.Length != 0;
        }

        private void txtFind_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && btnFind.Enabled) btnFind.PerformClick();
        }

        private void txtReplace_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && btnReplaceWith.Enabled) btnReplaceWith.PerformClick();
        }
        public class FindEventArgs : EventArgs {
            public string Find { get; private set; }
            public bool IgnoreCase { get; private set; }
            public FindEventArgs(string find, bool ignoreCase) {
                Find = find;
                IgnoreCase = ignoreCase;
            }
        }
        public class ReplaceEventArgs : EventArgs {
            public string Find { get; private set; }
            public bool IgnoreCase { get; private set; }
            public string With { get; private set; }
            public bool All { get; private set; }
            public ReplaceEventArgs(string find, bool ignoreCase, string with, bool all) {
                Find = find;
                IgnoreCase = ignoreCase;
                With = with;
                All = all;
            }
        }
    }
}
