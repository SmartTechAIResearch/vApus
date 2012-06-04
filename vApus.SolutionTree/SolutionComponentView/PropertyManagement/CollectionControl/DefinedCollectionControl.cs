﻿/*
 * Copyright 2010 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using vApus.Util;

namespace vApus.SolutionTree
{
    /// <summary>
    /// For selecting multiple base items with the same parent.
    /// </summary>
    public partial class DefinedCollectionControl : UserControl
    {
        public event EventHandler ValueChanged;

        private IEnumerable _value;

        public IEnumerable Value
        {
            get { return _value; }
        }
        public DataGridViewRowCollection Rows
        {
            get { return dataGridView.Rows; }
        }
        /// <summary>
        /// For selecting multiple base items with the same parent.
        /// </summary>
        public DefinedCollectionControl()
        {
            InitializeComponent();

            SetColumn();
        }
        private void SetColumn()
        {
            DataGridViewColumn column = new DataGridViewTextBoxColumn();

            dataGridView.Columns.Add(column);
            dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void SetValue(IEnumerable value)
        {
            try
            {
                var parent = (SolutionComponent)value.GetParent();

            }
            catch (Exception ex)
            {

                throw new Exception("value must be of type BaseItem (direct or indirect) and must have a parent of the type SolutionComponent (also direct or indirect type).", ex);
            }
            _value = value;

            dataGridView.CellValueChanged -= dataGridView_CellValueChanged;
            dataGridView.RowsRemoved -= dataGridView_RowsRemoved;
            dataGridView.Rows.Clear();

            IEnumerator enumerator = value.GetEnumerator();
            while (enumerator.MoveNext())
                if (enumerator.Current != null)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.Add(CreateDataGridViewCell(enumerator.Current));
                    dataGridView.Rows.Add(row);
                }
            dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
            dataGridView.RowsRemoved += new DataGridViewRowsRemovedEventHandler(dataGridView_RowsRemoved);
        }
        private DataGridViewCell CreateDataGridViewCell(object value)
        {
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            cell.Value = value.ToString();
            return cell;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {

            SelectBaseItemsDialog selectBaseItemsDialog = new SelectBaseItemsDialog();
            selectBaseItemsDialog.SetValue(_value);

            if (selectBaseItemsDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetValue(selectBaseItemsDialog.NewValue);
                }
                catch { }
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }
        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }
    }
}