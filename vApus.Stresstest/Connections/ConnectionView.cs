﻿/*
 * Copyright 2010 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.CodeDom.Compiler;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vApus.SolutionTree;
using vApus.Util;

namespace vApus.Stresstest {
    /// <summary>
    /// Editting a connection happens here. Gui is generated based on the connection rule set in the chosen connection proxy.
    /// </summary>
    public partial class ConnectionView : BaseSolutionComponentView {

        #region Fields
        private readonly Connection _connection;
        /// <summary>
        ///     For endless loops.
        /// </summary>
        private bool _canUpdateGui = true;
        private bool _testing, _tracing;
        #endregion

        #region Constructors
        /// <summary>
        /// Design time constructor.
        /// </summary>
        public ConnectionView() { InitializeComponent(); }
        /// <summary>
        /// Editting a connection happens here. Gui is generated based on the connection rule set in the chosen connection proxy.
        /// </summary>
        /// <param name="solutionComponent"></param>
        public ConnectionView(SolutionComponent solutionComponent)
            : base(solutionComponent) {
            InitializeComponent();
            _connection = solutionComponent as Connection;

            if (IsHandleCreated)
                SetGui();
            else
                HandleCreated += ConnectionView_HandleCreated;
        }
        #endregion

        #region Functions
        private void ConnectionView_HandleCreated(object sender, EventArgs e) {
            HandleCreated -= ConnectionView_HandleCreated;
            SetGui();
        }
        private void SetGui() {
            solutionComponentPropertyPanel.SolutionComponent = _connection;
            ruleSetSyntaxItemPanel.SetRuleSetAndInput(_connection.ConnectionProxy.ConnectionProxyRuleSet, _connection.ConnectionString);
            ruleSetSyntaxItemPanel.InputChanged += ruleSetSyntaxItemPanel_InputChanged;
        }
        
        private void ruleSetSyntaxItemPanel_InputChanged(object sender, EventArgs e) {
            _connection.ConnectionString = ruleSetSyntaxItemPanel.Input;
            _canUpdateGui = false;
            _connection.InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
        }
    
        async private void btnTestConnection_Click(object sender, EventArgs e) {
            _testing = true;
            split.Enabled = false;
            btnTestConnection.Enabled = false;
            btnTestConnection.Text = "Testing...";

            await Task.Run(() => TestConnection());
        }
        private void TestConnection() {
            _testing = false;
            if (_connection.ConnectionProxy.IsEmpty) {
                SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                    btnTestConnection.Text = "Test Connection";
                    if (!_tracing)
                        split.Enabled = true;
                    btnTestConnection.Enabled = true;

                    string error = "This connection has no connection proxy assigned to!";
                    MessageBox.Show(this, error, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    LogWrapper.LogByLevel("[" + _connection + "] " + error, LogLevel.Warning);
                }, null);
                return;
            }

            var connectionProxyPool = new ConnectionProxyPool(_connection);
            CompilerResults compilerResults = connectionProxyPool.CompileConnectionProxyClass(false);

            if (compilerResults.Errors.HasErrors) {
                string error = "Failed at compiling the connection proxy class";
                var sb = new StringBuilder(error + ": ");
                sb.AppendLine();
                foreach (CompilerError ce in compilerResults.Errors) {
                    sb.AppendFormat("Error number {0}, Line {1}, Column {2}: {3}", ce.ErrorNumber, ce.Line, ce.Column,
                                    ce.ErrorText);
                    sb.AppendLine();
                }

                SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                    btnTestConnection.Text = "Test Connection";
                    if (!_tracing)
                        split.Enabled = true;
                    btnTestConnection.Enabled = true;

                    MessageBox.Show(this, error, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    LogWrapper.LogByLevel("[" + _connection + "] " + sb, LogLevel.Warning);
                }, null);
            } else {
                string errorMessage;
                connectionProxyPool.TestConnection(out errorMessage);

                SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                    btnTestConnection.Text = "Test Connection";
                    if (!_tracing)
                        split.Enabled = true;
                    btnTestConnection.Enabled = true;

                    if (errorMessage == null) {
                        MessageBox.Show(this, "The connection has been established! and closed again successfully.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    } else {
                        string error =
                            "The connection could not be made, please make sure everything is filled in correctly.";
                        MessageBox.Show(this, error, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        LogWrapper.LogByLevel("[" + _connection + "] " + error + "\n" + errorMessage, LogLevel.Warning);
                    }
                }, null);
            }
            connectionProxyPool.Dispose();
            connectionProxyPool = null;
        }

        private void tracertControl_BeforeTrace(object sender, EventArgs e) {
            _tracing = true;
            split.Enabled = false;

            string[] sp = _connection.ConnectionString.Split(new[] { _connection.ConnectionProxy.ConnectionProxyRuleSet.ChildDelimiter }, StringSplitOptions.None);
            string tracertField = sp[_connection.ConnectionProxy.ConnectionProxyRuleSet.TracertField - 1];

            if (tracertField.ContainsChars('/')) {
                if (tracertField.StartsWith("http://"))
                    tracertField = tracertField.Substring("http://".Length);

                tracertField = tracertField.Split('/')[0];
            }

            tracertControl.SetToTrace(tracertField);
        }
        private void tracertControl_Done(object sender, EventArgs e) {
            _tracing = false;
            if (!_testing)
                split.Enabled = true;
        }

        public override void Refresh() {
            if (_canUpdateGui) {
                base.Refresh();
                solutionComponentPropertyPanel.Refresh();
                ruleSetSyntaxItemPanel.SetRuleSetAndInput(_connection.ConnectionProxy.ConnectionProxyRuleSet,
                                                          _connection.ConnectionString);
            }
            _canUpdateGui = true;
        }
        #endregion
    }
}