﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using vApus.Monitor;
using vApus.REST.Convert;
using vApus.Results;
using vApus.SolutionTree;
using vApus.Stresstest;
using vApus.Util;

namespace vApus.DistributedTesting {
    public partial class DistributedTestView : BaseSolutionComponentView {
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int LockWindowUpdate(int hWnd);

        #region Fields

        private readonly object _lock = new object();

        private Schedule _schedule = null;

        private ITreeViewItem _selectedTestTreeViewItem;

        private DistributedTest _distributedTest = new DistributedTest();
        private DistributedTestCore _distributedTestCore;
        private DistributedTestMode _distributedTestMode;

        private ResultsHelper _resultsHelper = new ResultsHelper();
        /// <summary>
        /// For adding and getting results.
        /// </summary>
        private readonly Dictionary<TileStresstest, ulong> _tileStresstestsWithDbIds = new Dictionary<TileStresstest, ulong>();

        /// <summary>
        ///     Countdown for the update.
        /// </summary>
        private int _countDown;

        private Countdown _monitorBeforeCountDown;
        /// <summary>
        ///     The test can only start when this == 0.
        /// </summary>
        private int _pendingMonitorViewInitializations;

        /// <summary>
        ///     The monitors for the tests if any.
        /// </summary>
        private readonly Dictionary<TileStresstest, List<MonitorView>> _monitorViews = new Dictionary<TileStresstest, List<MonitorView>>();
        private readonly Dictionary<TileStresstest, MonitorMetricsCache> _monitorMetricsCaches = new Dictionary<TileStresstest, MonitorMetricsCache>();

        private readonly AutoResetEvent _monitorViewsInitializedWaitHandle = new AutoResetEvent(false);
        #endregion

        #region Properties

        private int TileStresstestCount {
            get {
                int count = 0;
                foreach (Tile t in _distributedTest.Tiles) count += t.Count;
                return count;
            }
        }
        private int UsedTileStresstestCount {
            get {
                int count = 0;
                foreach (Tile t in _distributedTest.Tiles)
                    foreach (TileStresstest ts in t)
                        if (ts.Use) ++count;
                return count;
            }
        }
        private int SlaveCount {
            get {
                int count = 0;
                foreach (Client c in _distributedTest.Clients) count += c.Count;
                return count;
            }
        }
        private int UsedSlaveCount {
            get {
                int count = 0;
                foreach (Client c in _distributedTest.Clients)
                    foreach (Slave s in c)
                        if (s.TileStresstest != null) ++count;
                return count;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        ///     Desing time constructor
        /// </summary>
        public DistributedTestView() {
            InitializeComponent();
        }

        public DistributedTestView(SolutionComponent solutionComponent, params object[] args)
            : base(solutionComponent, args) {
            InitializeComponent();

            SetDistributedTest(solutionComponent as DistributedTest);

            SolutionComponent.SolutionComponentChanged += SolutionComponent_SolutionComponentChanged;

            RemoteDesktopClient.RdpException += RemoteDesktopClient_RdpException;

            //Jumpstart the slaves when starting the test
            JumpStart.Done += JumpStart_Done;

            Shown += DistributedTestView_Shown; //if the test is empty, show the wizard.
        }

        #endregion

        #region General Functions

        private void SetDistributedTest(DistributedTest distributedTest) {
            _distributedTest = distributedTest;
            testTreeView.SetDistributedTest(_distributedTest);
            slaveTreeView.SetDistributedTest(_distributedTest);
            configureSlaves.SetDistributedTest(_distributedTest);
        }

        private void SolutionComponent_SolutionComponentChanged(object sender, SolutionComponentChangedEventArgs e) {
            if (sender is DistributedTest)
                SetDistributedTest(sender as DistributedTest);
            else if (sender is Tile || sender is TileStresstest || sender is Client || sender is Slave)
                RefreshGui();
        }

        /// <summary>
        /// Show the wizard if this is a new (empty) distributed test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistributedTestView_Shown(object sender, EventArgs e) {
            Shown -= DistributedTestView_Shown;
            if (_distributedTest.Tiles.Count == 0 && _distributedTest.Clients.Count == 0)
                ShowWizard();
        }
        private void btnWizard_Click(object sender, EventArgs e) { ShowWizard(); }
        private void ShowWizard() {
            using (var wizard = new Wizard()) {
                wizard.SetDistributedTest(_distributedTest);
                wizard.ShowDialog();
            }
        }

        /// <summary>
        ///     Show the gui to be able to schedule the test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSchedule_Click(object sender, EventArgs e) {
            _schedule = (btnSchedule.Tag != null && btnSchedule.Tag is DateTime) ? new Schedule((DateTime)btnSchedule.Tag) : new Schedule();
            if (_schedule.ShowDialog() == DialogResult.OK) {
                if (_schedule.ScheduledAt > DateTime.Now) {
                    btnSchedule.Tag = _schedule.ScheduledAt;
                } else {
                    btnSchedule.Text = string.Empty;
                    btnSchedule.Tag = null;
                }
                _schedule = null;
                btnStart_Click(this, null);
            } else {
                btnSchedule.Text = string.Empty;
            }
            _schedule = null;
        }
        private void btnSchedule_MouseEnter(object sender, EventArgs e) {
            btnSchedule.Text = btnSchedule.ToolTipText;
        }
        private void btnSchedule_MouseLeave(object sender, EventArgs e) {
            if (!btnSchedule.Text.StartsWith("Scheduled") && _schedule == null) btnSchedule.Text = string.Empty;
        }

        private void tpTree_SelectedIndexChanged(object sender, EventArgs e) {
            if (tcTree.SelectedIndex == 0) {
                configureTileStresstest.Visible = true;
                configureSlaves.Visible = false;
            } else {
                configureTileStresstest.Visible = false;
                configureSlaves.Visible = true;
            }
        }

        /// <summary>
        /// Set the gui according to the selected test tree view item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testTreeView_AfterSelect(object sender, EventArgs e) {
            _selectedTestTreeViewItem = sender as ITreeViewItem;
            if (sender is TileStresstestTreeViewItem) {
                var tstvi = sender as TileStresstestTreeViewItem;
                configureTileStresstest.SetTileStresstest(tstvi.TileStresstest);

                fastResultsControl.Visible = true;
                distributedStresstestControl.Visible = false;

                fastResultsControl.SetConfigurationControlsAndMonitorLinkButtons(tstvi.TileStresstest.ToString(), tstvi.TileStresstest.BasicTileStresstest.Connection,
                   tstvi.TileStresstest.BasicTileStresstest.ConnectionProxy, tstvi.TileStresstest.AdvancedTileStresstest.Log, tstvi.TileStresstest.AdvancedTileStresstest.LogRuleSet,
                   tstvi.TileStresstest.BasicTileStresstest.Monitors, tstvi.TileStresstest.AdvancedTileStresstest.Concurrencies, tstvi.TileStresstest.AdvancedTileStresstest.Runs,
                   tstvi.TileStresstest.AdvancedTileStresstest.MinimumDelay, tstvi.TileStresstest.AdvancedTileStresstest.MaximumDelay, tstvi.TileStresstest.AdvancedTileStresstest.Shuffle,
                   tstvi.TileStresstest.AdvancedTileStresstest.Distribute, tstvi.TileStresstest.AdvancedTileStresstest.MonitorBefore, tstvi.TileStresstest.AdvancedTileStresstest.MonitorAfter);

                if (_distributedTestCore != null) {
                    if (_distributedTestCore.TestProgressMessages.ContainsKey(tstvi.TileStresstest)) {
                        SetSlaveProgress(tstvi.TileStresstest, _distributedTestCore.TestProgressMessages[tstvi.TileStresstest]);
                    } else {
                        fastResultsControl.ClearFastResults();
                        detailedResultsControl.ClearResults();
                        detailedResultsControl.Enabled = false;
                    }
                }
            } else {
                bool showDescriptions = false;
                if (sender is DistributedTestTreeViewItem) {
                    var dttvi = sender as DistributedTestTreeViewItem;
                    foreach (Control ctrl in dttvi.Controls)
                        if ((ctrl is CheckBox && ctrl.Focused) || (ctrl is Panel && ctrl.Controls.Count != 0 && ctrl.Controls[0].Focused)) {
                            showDescriptions = true;
                            break;
                        }
                }
                configureTileStresstest.ClearTileStresstest(showDescriptions);

                fastResultsControl.Visible = false;
                distributedStresstestControl.Visible = true;

                detailedResultsControl.ClearResults();
                detailedResultsControl.Enabled = false;

                SetOverallProgress();
            }

            //Update the detailed results in the gui if any.
            RefreshDetailedResults();
        }
        private void slaveTreeView_AfterSelect(object sender, EventArgs e) {
            if (sender is ClientTreeViewItem) {
                var ctvi = sender as ClientTreeViewItem;
                ctvi.ConfigureSlaves = configureSlaves;
                configureSlaves.SetClient(ctvi);
            } else {
                configureSlaves.ClearClient();
            }
        }
        private void configureSlaves_GoToAssignedTest(object sender, EventArgs e) {
            TileStresstest ts = (sender as SlaveTile).Slave.TileStresstest;
            if (ts != null) {
                tcTree.SelectedIndex = 0;
                testTreeView.SelectTileStresstest(ts);
            }
        }

        private void tmrRefreshGui_Tick(object sender, EventArgs e) { RefreshGui(); }
        private void RefreshGui() {
            string tests = "Tests (" + UsedTileStresstestCount + "/" + TileStresstestCount + ")";
            if (tpTests.Text != tests) tpTests.Text = tests;

            string slaves = "Slaves (" + UsedSlaveCount + "/" + SlaveCount + ")";
            if (tpSlaves.Text != slaves) tpSlaves.Text = slaves;

            testTreeView.SetGui();
            slaveTreeView.SetGui();

            if (_distributedTestMode == DistributedTestMode.Edit) btnStart.Enabled = !testTreeView.Exclamation;
        }
        /// <summary>
        ///     Refresh some properties that are overriden in code.
        /// </summary>
        public override void Refresh() {
            base.Refresh();
            configureTileStresstest.Refresh();
        }

        /// <summary>
        ///     Set the gui for the different modes
        /// </summary>
        /// <param name="distributedTestMode"></param>
        /// <param name="scheduled">only for distributedTestMode.TestAndReport</param>
        private void SetMode(DistributedTestMode distributedTestMode, bool canEnableStop = false, bool scheduled = false) {
            if (IsDisposed) return;

            _distributedTestMode = distributedTestMode;

            if (_distributedTestMode == DistributedTestMode.Test) {
                btnStop.Enabled = canEnableStop;
                btnStart.Enabled = btnSchedule.Enabled = btnWizard.Enabled = false;
                if (scheduled) tmrSchedule.Start(); else btnSchedule.Text = string.Empty;
                //tcTree.SelectedTab = tpTests;
            } else {
                btnStop.Enabled = false;
                btnStart.Enabled = btnSchedule.Enabled = !testTreeView.Exclamation;
                btnWizard.Enabled = true;

                tmrSchedule.Stop();
                tmrProgress.Stop();
                tmrProgressDelayCountDown.Stop();

                btnSchedule.Text = string.Empty;
                btnSchedule.Tag = null;
            }

            testTreeView.SetMode(_distributedTestMode, scheduled);
            slaveTreeView.SetMode(_distributedTestMode);
            configureTileStresstest.SetMode(_distributedTestMode);
            configureSlaves.SetMode(_distributedTestMode);
        }
        #endregion

        #region Start & Schedule

        private void btnStart_Click(object sender, EventArgs e) {
            if (_distributedTestCore != null && _distributedTestCore.HasResults &&
                MessageBox.Show("Do you want to clear the previous results, before starting the test (at the scheduled date / time)?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            if (_distributedTest.RunSynchronization != RunSynchronization.None && !CheckNumberOfRuns()) {
                MessageBox.Show("Could not start the distributed test because the number of runs for the different single stresstests are not equal to each other.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (InitDatabaseBeforeStart()) {
                SetvApusInstancesAndStresstestsInDb();
                if (btnSchedule.Tag != null && btnSchedule.Tag is DateTime && (DateTime)btnSchedule.Tag > DateTime.Now)
                    ScheduleTest();
                else
                    Start();
            }
        }
        /// <summary>
        ///     Check if the number of runs for the different single stresstests are equal to each other.
        ///     Use this when using run synchronization.
        /// </summary>
        /// <returns></returns>
        private bool CheckNumberOfRuns() {
            int numberOfRuns = -1;
            foreach (Tile t in _distributedTest.Tiles)
                if (t.Use) {
                    foreach (TileStresstest ts in t)
                        if (ts.Use) {
                            if (numberOfRuns == -1)
                                numberOfRuns = ts.AdvancedTileStresstest.Concurrencies.Length * ts.AdvancedTileStresstest.Runs;
                            else if (numberOfRuns != ts.AdvancedTileStresstest.Concurrencies.Length * ts.AdvancedTileStresstest.Runs)
                                return false;
                        }
                }
            return true;
        }

        /// <summary>
        ///     True on success or if user said there can be proceed without database.
        /// </summary>
        /// <returns></returns>
        private bool InitDatabaseBeforeStart() {
            var dialog = new DescriptionAndTagsInputDialog { Description = _distributedTest.Description, Tags = _distributedTest.Tags, ResultsHelper = _resultsHelper };
            if (dialog.ShowDialog() == DialogResult.Cancel) {
                RemoveDatabase(false);
                return false;
            }

            bool edited = false;
            if (_distributedTest.Description != dialog.Description) {
                _distributedTest.Description = dialog.Description;
                edited = true;
            }
            if (_distributedTest.Tags.Combine(", ") != dialog.Tags.Combine(", ")) {
                _distributedTest.Tags = dialog.Tags;
                edited = true;
            }

            if (edited) _distributedTest.InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
            return true;
        }
        private void SetvApusInstancesAndStresstestsInDb() {
            _tileStresstestsWithDbIds.Clear();
            foreach (Tile t in _distributedTest.Tiles)
                if (t.Use)
                    foreach (TileStresstest ts in t)
                        if (ts.Use && ts.BasicTileStresstest.Slaves.Length != 0) {
                            var slave = ts.BasicTileStresstest.Slaves[0];
                            _resultsHelper.SetvApusInstance(slave.HostName, slave.IP, slave.Port, string.Empty, string.Empty, false);
                            ulong id = _resultsHelper.SetStresstest(ts.ToString(), _distributedTest.RunSynchronization.ToString(), ts.BasicTileStresstest.Connection.ToString(), ts.BasicTileStresstest.ConnectionProxy,
                                      ts.BasicTileStresstest.Connection.ConnectionString, ts.AdvancedTileStresstest.Log.ToString(), ts.AdvancedTileStresstest.LogRuleSet, ts.AdvancedTileStresstest.Concurrencies,
                                      ts.AdvancedTileStresstest.Runs, ts.AdvancedTileStresstest.MinimumDelay, ts.AdvancedTileStresstest.MaximumDelay, ts.AdvancedTileStresstest.Shuffle, ts.AdvancedTileStresstest.Distribute.ToString(),
                                      ts.AdvancedTileStresstest.MonitorBefore, ts.AdvancedTileStresstest.MonitorAfter);
                            _tileStresstestsWithDbIds.Add(ts, id);
                        }

            _resultsHelper.SetvApusInstance(NamedObjectRegistrar.Get<string>("HostName"), NamedObjectRegistrar.Get<string>("IP"), NamedObjectRegistrar.Get<int>("Port"),
                    NamedObjectRegistrar.Get<string>("vApusVersion") ?? string.Empty, NamedObjectRegistrar.Get<string>("vApusChannel") ?? string.Empty,
                    true);
        }

        private void ScheduleTest() { SetMode(DistributedTestMode.Test, true, true); }
        private void tmrSchedule_Tick(object sender, EventArgs e) {
            var scheduledAt = (DateTime)btnSchedule.Tag;
            if (scheduledAt <= DateTime.Now) {
                btnSchedule.Text = string.Empty;
                btnSchedule.Tag = null;
                tmrSchedule.Stop();
                Start();
            } else {
                TimeSpan dt = scheduledAt - DateTime.Now;
                if (dt.Milliseconds != 0) {
                    dt = new TimeSpan(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond));
                    dt += new TimeSpan(0, 0, 1);
                }
                btnSchedule.Text = "Scheduled in " + dt.ToLongFormattedString();
            }
        }

        async private void Start() {
            try {
                Cursor = Cursors.WaitCursor;
                //Otherwise a handle problem can arise
                SynchronizationContextWrapper.SynchronizationContext.Send((state) => {
                    SetMode(DistributedTestMode.Test);
                }, null);

                tcTest.SelectedIndex = 1;

                if (_distributedTest.UseRDP) ShowRemoteDesktop();

                distributedStresstestControl.ClearFastResults();

                //Smart update
                UpdateNotifier.Refresh();
                string host, username, password;
                int port, channel;
                bool smartUpdate;
                UpdateNotifier.GetCredentials(out host, out port, out username, out password, out channel, out smartUpdate);

                if (smartUpdate) {
                    if (UpdateNotifier.UpdateNotifierState == UpdateNotifierState.NewUpdateFound) {
                        MessageBox.Show("In order to be able to update the used slaves the master must be up to date as well.\nPlease update vApus, then you can start the test.", "Smart Update Slaves", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        throw new Exception();
                    } else if (UpdateNotifier.UpdateNotifierState == UpdateNotifierState.UpToDate) {
                        distributedStresstestControl.AppendMessages("Updating the slaves, this can take a while...");
                        var exceptions = await JumpStart.SmartUpdate(_distributedTest);

                        if (exceptions.Length != 0) {
                            foreach (Exception ex in exceptions)
                                distributedStresstestControl.AppendMessages(ex.Message, LogLevel.Error);

                            throw new Exception();
                        }
                    }
                }

                distributedStresstestControl.AppendMessages("Jump Starting the slaves...");
                try {
                    //Jumpstart the slaves first, an event will be fired when this is done and the test will start
                    JumpStart.Do(_distributedTest);
                } catch {
                    //Only one test can run at the same time.
                    distributedStresstestControl.AppendMessages("Failed to Jump Start one or more slaves.", LogLevel.Error);
                    throw;
                }

            } catch {
                RemoveDatabase(false);
                Stop();
            }
        }

        /// <summary>
        ///     A remote desktop is needed in order for the distributed test to work.
        /// </summary>
        private void ShowRemoteDesktop() {
            distributedStresstestControl.AppendMessages("Opening remote desktop connection(s) to the client(s)...");

            var rdc = SolutionComponentViewManager.Show(_distributedTest, typeof(RemoteDesktopClient)) as RemoteDesktopClient;
            rdc.Text = "Remote Desktop Client";
            Show();
            rdc.ClearRemoteDesktops();
            foreach (Client client in _distributedTest.Clients)
                if (client.UsedSlaveCount != 0)
                    rdc.ShowRemoteDesktop(client.HostName, client.IP, client.UserName, client.Password, client.Domain);
        }
        private void RemoteDesktopClient_RdpException(object sender, Util.RemoteDesktopClient.RdpExceptionEventArgs e) {
            string message = "Cannot open a remote desktop connection to " + e.IP + ". (error code: " + e.ErrorCode + ") ";
            distributedStresstestControl.AppendMessages(message, LogLevel.Error);
            LogWrapper.LogByLevel(message, LogLevel.Error);
        }

        private void JumpStart_Done(object sender, JumpStart.DoneEventArgs e) {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                try {
                    if (e.Exceptions.Length == 0) {
                        if (_distributedTestCore != null && !_distributedTestCore.IsDisposed) {
                            _distributedTestCore.Dispose();
                            _distributedTestCore = null;
                        }

                        _distributedTestCore = new DistributedTestCore(_distributedTest, _resultsHelper, _tileStresstestsWithDbIds);
                        _distributedTestCore.Message += _distributedTestCore_Message;
                        _distributedTestCore.OnTestProgressMessageReceived += _distributedTestCore_TestProgressMessageReceivedEventArgs;
                        _distributedTestCore.OnListeningError += _distributedTestCore_OnListeningError;
                        _distributedTestCore.OnFinished += _distributedTestCore_OnFinished;


                        var t = new Thread(InitializeAndStartTest);
                        t.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                        t.IsBackground = true;
                        t.Start();
                    } else {
                        //Failed jump starting slaves
                        foreach (Exception ex in e.Exceptions) {
                            string message = ex.ToString();
                            distributedStresstestControl.AppendMessages(message, LogLevel.Error);
                            LogWrapper.LogByLevel(message, LogLevel.Error);
                        }

                        RemoveDatabase();
                        Stop();
                    }
                } catch {
                    //Only one test can run at the same time.
                    string message = "Cannot start this test because another one is still running.";
                    distributedStresstestControl.AppendMessages(message, LogLevel.Error);
                    LogWrapper.LogByLevel(message, LogLevel.Error);
                    Stop();
                }
            }, null);
        }

        private void InitializeAndStartTest() {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate { btnStop.Enabled = true; }, null);
            if (InitializeTest() && _pendingMonitorViewInitializations == 0) StartTest();
        }
        private bool InitializeTest() {
            _pendingMonitorViewInitializations = 0;

            try {
                _distributedTestCore.Initialize();
                SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                    fastResultsControl.SetStresstestInitialized();

                    //Initialize the monitors.
                    _monitorViews.Clear();
                    _monitorMetricsCaches.Clear();
                    detailedResultsControl.ClearResults();
                    detailedResultsControl.Enabled = false;

                    foreach (TileStresstest tileStresstest in _distributedTestCore.UsedTileStresstests)
                        for (int i = 0; i != tileStresstest.BasicTileStresstest.Monitors.Length; i++)
                            ShowAndInitMonitorView(tileStresstest, tileStresstest.BasicTileStresstest.Monitors[i]);
                }, null);

                if (_pendingMonitorViewInitializations != 0) _monitorViewsInitializedWaitHandle.WaitOne();
            } catch (Exception ex) {
                HandleInitializeOrStartException(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        ///     Can only start after that all monitor views are initialized.
        /// </summary>
        private void StartTest() {
            try {
                SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                    try { LocalMonitor.StartMonitoring(Stresstest.Stresstest.ProgressUpdateDelay * 1000); } catch { fastResultsControl.AppendMessages("Could not initialize the local monitor, something is wrong with your WMI service.", LogLevel.Error); }
                    tmrProgress.Interval = Stresstest.Stresstest.ProgressUpdateDelay * 1000;
                    tmrProgress.Start();

                    tmrProgressDelayCountDown.Start();

                    _countDown = Stresstest.Stresstest.ProgressUpdateDelay - 1;

                    StartMonitors();

                    Cursor = Cursors.Default;
                }, null);
            } catch (Exception ex) { HandleInitializeOrStartException(ex); }
        }

        private void HandleInitializeOrStartException(Exception ex) {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                string message = string.Format("The stresstest threw an exception:{0}{1}", Environment.NewLine, ex.Message);
                distributedStresstestControl.AppendMessages(message, LogLevel.Error);
                if (_distributedTestCore != null && !_distributedTestCore.IsDisposed) {
                    _distributedTestCore.Dispose();
                    _distributedTestCore = null;
                }

                RemoveDatabase();
                Stop();
                Cursor = Cursors.Default;
            }, null);
        }

        #endregion

        #region Progress

        private void _distributedTestCore_Message(object sender, MessageEventArgs e) { distributedStresstestControl.AppendMessages(e.Message); }

        private void _distributedTestCore_TestProgressMessageReceivedEventArgs(object sender, TestProgressMessageReceivedEventArgs e) {
            Handle_distributedTestCore_TestProgressMessageReceivedEventArgs(e.TileStresstest, e.TestProgressMessage);
        }
        private void _distributedTestCore_OnListeningError(object sender, ListeningErrorEventArgs e) {
            //Stop the distributed test (it is not valid anymore if a slave fails)
            btnStop_Click(btnStop, null);

            //Update the stresstest result for the failed test and set the gui.
            foreach (TileStresstest tileStresstest in _distributedTestCore.TestProgressMessages.Keys) {
                bool found = false;
                foreach (Slave slave in tileStresstest.BasicTileStresstest.Slaves)
                    if (slave.IP == e.SlaveIP && slave.Port == e.SlavePort) {
                        if (_distributedTestCore.TestProgressMessages.ContainsKey(tileStresstest)) {
                            TestProgressMessage testProgressMessage = _distributedTestCore.TestProgressMessages[tileStresstest];
                            testProgressMessage.StresstestStatus = StresstestStatus.Error;
                            _distributedTestCore.TestProgressMessages[tileStresstest] = testProgressMessage;

                            Handle_distributedTestCore_TestProgressMessageReceivedEventArgs(tileStresstest, testProgressMessage);
                        }
                        found = true;
                        break;
                    }
                if (found) break;
            }
        }
        private void Handle_distributedTestCore_TestProgressMessageReceivedEventArgs(TileStresstest tileStresstest, TestProgressMessage testProgressMessage) {
            if (_selectedTestTreeViewItem != null && _selectedTestTreeViewItem is TileStresstestTreeViewItem && (_selectedTestTreeViewItem as TileStresstestTreeViewItem).TileStresstest == tileStresstest) {
                SetSlaveProgress(tileStresstest, testProgressMessage);

                if (testProgressMessage.StresstestStatus == StresstestStatus.Busy) {
                    tmrProgressDelayCountDown.Stop();
                    _countDown = Stresstest.Stresstest.ProgressUpdateDelay;
                    fastResultsControl.SetCountDownProgressDelay(_countDown);
                    tmrProgressDelayCountDown.Start();
                }
            }

            UpdateMonitorMetricsCaches(tileStresstest, testProgressMessage);

            SetOverallProgress();
            SetSlaveProgressInTreeView(tileStresstest, testProgressMessage);

            //Notify by mail if set in the options panel.
            if (testProgressMessage.StresstestStatus == StresstestStatus.Busy) {
                if (testProgressMessage.RunFinished) {
                    var l = testProgressMessage.StresstestMetricsCache.GetRunMetrics();
                    var runMetrics = l[l.Count - 1];
                    string message = string.Concat(tileStresstest.ToString(), " - Run ", runMetrics.Run, " of concurrency ", runMetrics.Concurrency, " finished.");
                    TestProgressNotifier.Notify(TestProgressNotifier.What.RunFinished, message);
                } else if (testProgressMessage.ConcurrencyFinished) {
                    var l = testProgressMessage.StresstestMetricsCache.GetConcurrencyMetrics();
                    var concurrencyMetrics = l[l.Count - 1];
                    string message = string.Concat(tileStresstest.ToString(), " - Concurrency ", concurrencyMetrics.Concurrency, " finished.");
                    TestProgressNotifier.Notify(TestProgressNotifier.What.ConcurrencyFinished, message);
                }
            } else {
                TestProgressNotifier.Notify(TestProgressNotifier.What.TestFinished, string.Concat(tileStresstest.ToString(), " finished. Status: ", testProgressMessage.StresstestStatus, "."));
            }

#if EnableBetaFeature
            WriteRestProgress();
#endif
        }

        private void UpdateMonitorMetricsCaches(TileStresstest tileStresstest, TestProgressMessage testProgressMessage) {
            if (_monitorViews.ContainsKey(tileStresstest)) {
                _monitorMetricsCaches[tileStresstest] = new MonitorMetricsCache();
                var monitorMetricsCache = _monitorMetricsCaches[tileStresstest];

                foreach (var monitorResultCache in GetMonitorResultCaches(tileStresstest)) {
                    foreach (var concurrencyMetrics in testProgressMessage.StresstestMetricsCache.GetConcurrencyMetrics()) {
                        var monitorMetrics = MonitorMetricsHelper.GetConcurrencyMetrics(monitorResultCache.Monitor, concurrencyMetrics, monitorResultCache);
                        monitorMetricsCache.Add(monitorMetrics);
                    }
                    foreach (var runMetrics in testProgressMessage.StresstestMetricsCache.GetRunMetrics()) {
                        var monitorMetrics = MonitorMetricsHelper.GetRunMetrics(monitorResultCache.Monitor, runMetrics, monitorResultCache);
                        monitorMetricsCache.Add(monitorMetrics);
                    }
                }
            }
        }

        private void SetOverallProgress() {
            if (_selectedTestTreeViewItem != null)
                if (_selectedTestTreeViewItem is DistributedTestTreeViewItem) {
                    distributedStresstestControl.SetTitle("Distributed Test");
                    if (_distributedTestCore != null && !_distributedTestCore.IsDisposed) {
                        var progress = new Dictionary<TileStresstest, StresstestMetricsCache>(_distributedTestCore.TestProgressMessages.Count);
                        foreach (TileStresstest tileStresstest in _distributedTestCore.TestProgressMessages.Keys) {
                            var tpm = _distributedTestCore.TestProgressMessages[tileStresstest].StresstestMetricsCache;
                            if (tpm != null) progress.Add(tileStresstest, tpm);
                        }

                        distributedStresstestControl.SetOverallFastResults(progress);
                    }
                } else if (_selectedTestTreeViewItem is TileTreeViewItem) {
                    var ttvi = _selectedTestTreeViewItem as TileTreeViewItem;
                    distributedStresstestControl.SetTitle(ttvi.Tile.Name + " " + ttvi.Tile.Index);
                    if (_distributedTestCore != null && !_distributedTestCore.IsDisposed) {
                        var progress = new Dictionary<TileStresstest, StresstestMetricsCache>();
                        foreach (TileStresstest tileStresstest in _distributedTestCore.TestProgressMessages.Keys)
                            if (ttvi.Tile.Contains(tileStresstest)) {
                                var tpm = _distributedTestCore.TestProgressMessages[tileStresstest].StresstestMetricsCache;
                                if (tpm != null) progress.Add(tileStresstest, tpm);
                            }

                        distributedStresstestControl.SetOverallFastResults(progress);
                    }
                }
        }
        /// <summary>
        /// </summary>
        /// <param name="tileStresstest"></param>
        /// <param name="testProgressMessage"></param>
        /// <param name="overalEndOfTimeFrame">The end of time frame for the full test.</param>
        private void SetSlaveProgress(TileStresstest tileStresstest, TestProgressMessage testProgressMessage) {
            lock (_lock) {
                //Build and add fast results.
                fastResultsControl.ClearFastResults();
                if (testProgressMessage.StresstestMetricsCache != null) {
                    fastResultsControl.UpdateFastConcurrencyResults(testProgressMessage.StresstestMetricsCache.GetConcurrencyMetrics());
                    fastResultsControl.UpdateFastRunResults(testProgressMessage.StresstestMetricsCache.GetRunMetrics());
                }
                var monitorResultCaches = GetMonitorResultCaches(tileStresstest);
                foreach (var monitorResultCache in monitorResultCaches) {
                    if (_monitorMetricsCaches.ContainsKey(tileStresstest)) {
                        var monitorMetricsCache = _monitorMetricsCaches[tileStresstest];
                        fastResultsControl.UpdateFastConcurrencyResults(monitorResultCache.Monitor, monitorMetricsCache.GetConcurrencyMetrics(monitorResultCache.Monitor));
                        fastResultsControl.UpdateFastRunResults(monitorResultCache.Monitor, monitorMetricsCache.GetRunMetrics(monitorResultCache.Monitor));
                    }
                }

                if (testProgressMessage.Events == null) fastResultsControl.ClearEvents();
                else fastResultsControl.SetEvents(testProgressMessage.Events);

                fastResultsControl.SetStresstestStarted(testProgressMessage.StartedAt);
                if (testProgressMessage.StresstestStatus == StresstestStatus.Busy)
                    fastResultsControl.SetMeasuredRuntime(testProgressMessage.MeasuredRuntime);
                else {
                    fastResultsControl.SetStresstestStopped(testProgressMessage.StresstestStatus, testProgressMessage.MeasuredRuntime);
                }

                fastResultsControl.SetClientMonitoring(testProgressMessage.ThreadsInUse, testProgressMessage.CPUUsage, testProgressMessage.ContextSwitchesPerSecond,
                    (int)testProgressMessage.MemoryUsage, (int)testProgressMessage.TotalVisibleMemory, testProgressMessage.NicsSent, testProgressMessage.NicsReceived);
            }
        }
        private void SetSlaveProgressInTreeView(TileStresstest tileStresstest, TestProgressMessage testProgressMessage) {
            lock (_lock) {
                DistributedTestTreeViewItem distributedTestTreeViewItem = null;
                TileStresstestTreeViewItem tileStresstestTreeViewItem = null;
                foreach (ITreeViewItem item in testTreeView.Items) {
                    if (item is DistributedTestTreeViewItem)
                        distributedTestTreeViewItem = item as DistributedTestTreeViewItem;
                    else if (item is TileStresstestTreeViewItem) {
                        var tstvi = item as TileStresstestTreeViewItem;
                        if (tstvi.TileStresstest == tileStresstest) {
                            tileStresstestTreeViewItem = tstvi;
                            break;
                        }
                    }
                }

                if (tileStresstestTreeViewItem != null) {
                    tileStresstestTreeViewItem.SetStresstestStatus(testProgressMessage.StresstestStatus);

                    //Build and add fast results.
                    if (testProgressMessage.StresstestMetricsCache != null) {
                        tileStresstestTreeViewItem.SetStresstestStarted(testProgressMessage.StartedAt);
                        tileStresstestTreeViewItem.SetEstimatedRunTimeLeft(testProgressMessage.MeasuredRuntime, testProgressMessage.EstimatedRuntimeLeft);

                        //Set the distributed test tree view item
                        distributedTestTreeViewItem.SetStresstestStarted();
                    }

                    if (testProgressMessage.Events == null) tileStresstestTreeViewItem.ClearEvents();
                    else tileStresstestTreeViewItem.SetEvents(testProgressMessage.Events);
                }
            }
        }

        private void testTreeView_ProgressEventClicked(object sender, EventProgressChart.ProgressEventEventArgs e) {
            if (sender == _selectedTestTreeViewItem && _selectedTestTreeViewItem is TileStresstestTreeViewItem) {
                tpStresstest.Select();
                fastResultsControl.ShowEvent(e.ProgressEvent.At);
            }
        }

        private void tmrProgressDelayCountDown_Tick(object sender, EventArgs e) {
            bool setCountDown = true;
            if (_selectedTestTreeViewItem != null && _selectedTestTreeViewItem is TileStresstestTreeViewItem)
                setCountDown = (_selectedTestTreeViewItem as TileStresstestTreeViewItem).StresstestResult == StresstestStatus.Busy;
            if (--_countDown > 0 && setCountDown) fastResultsControl.SetCountDownProgressDelay(_countDown);

#if EnableBetaFeature
            WriteMonitorRestProgress();
#endif
        }
        private void tmrProgress_Tick(object sender, EventArgs e) {
            try {
                distributedStresstestControl.SetMasterMonitoring(_distributedTestCore.Running, _distributedTestCore.OK, _distributedTestCore.Cancelled, _distributedTestCore.Failed,
                    LocalMonitor.CPUUsage, LocalMonitor.ContextSwitchesPerSecond, (int)LocalMonitor.MemoryUsage, (int)LocalMonitor.TotalVisibleMemory, LocalMonitor.NicsSent,
                    LocalMonitor.NicsReceived);
            } catch { } //Exception on false WMI. 
            _countDown = Stresstest.Stresstest.ProgressUpdateDelay;
        }
        #endregion

        #region Stop


        private void btnStop_Click(object sender, EventArgs e) {
            distributedStresstestControl.AppendMessages("Stopping the test...");
            if (_monitorBeforeCountDown != null) {
                _monitorBeforeCountDown.Stop();
                _monitorBeforeCountDown.Dispose();
                _monitorBeforeCountDown = null;
            }

            if (_distributedTestCore != null) {
                Cursor = Cursors.WaitCursor;

                SetMode(DistributedTestMode.Edit);
                btnStart.Enabled = btnSchedule.Enabled = btnWizard.Enabled = false;

                try {
                    _distributedTestCore.Stop();
                } catch (Exception ex) {
                    string message = string.Format("The stresstest threw an exception:{0}{1}", Environment.NewLine, ex.Message);
                    distributedStresstestControl.AppendMessages(message, LogLevel.Error);

                    btnStart.Enabled = btnSchedule.Enabled = btnWizard.Enabled = true;
                }

                Cursor = Cursors.Default;
                distributedStresstestControl.AppendMessages("Test Cancelled!", LogLevel.Warning);
            }
        }
        private void _distributedTestCore_OnFinished(object sender, FinishedEventArgs e) {
            _distributedTestCore.OnFinished -= _distributedTestCore_OnFinished;

            Stop(e.Cancelled == 0 && e.Error == 0);
        }

        private void DistributedTestView_FormClosing(object sender, FormClosingEventArgs e) {
            if (_distributedTestMode == DistributedTestMode.Edit ||
                MessageBox.Show("Are you sure you want to close a running test?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                tmrProgress.Stop();
                tmrProgressDelayCountDown.Stop();
                tmrSchedule.Stop();
                tmrRefreshGui.Stop();

                StopMonitorsUpdateDetailedResultsAndSetMode();

                if (_distributedTestCore != null)
                    try { _distributedTestCore.Stop(); } catch { }
            } else {
                Solution.ExplicitCancelFormClosing = true;
                e.Cancel = true;
            }
        }
        private void RemoveDatabase(bool confirm = true) {
            if (_resultsHelper != null && _resultsHelper.DatabaseName != null)
                if (!confirm || MessageBox.Show("Do you want to remove the result database?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    == DialogResult.Yes)
                    try { _resultsHelper.RemoveDatabase(); } catch { }
        }
        private void Stop(bool monitorAfter = false) {
            Cursor = Cursors.WaitCursor;

            SetMode(DistributedTestMode.Edit);

            if (_distributedTestCore != null)
                try {
                    _distributedTestCore.Stop();
                    try {
                        distributedStresstestControl.SetMasterMonitoring(_distributedTestCore.Running, _distributedTestCore.OK,
                                                                         _distributedTestCore.Cancelled, _distributedTestCore.Failed,
                                                                         LocalMonitor.CPUUsage, LocalMonitor.ContextSwitchesPerSecond,
                                                                         (int)LocalMonitor.MemoryUsage, (int)LocalMonitor.TotalVisibleMemory,
                                                                         LocalMonitor.NicsSent, LocalMonitor.NicsReceived);
                    } catch { } //Exception on false WMI. 
                } catch (Exception ex) {
                    string message = string.Format("The stresstest threw an exception:{0}{1}", Environment.NewLine, ex.Message);
                    distributedStresstestControl.AppendMessages(message, LogLevel.Error);
                    monitorAfter = false;
                }

            Cursor = Cursors.Default;

            int runningMonitors = 0;
            int monitorAfterTime = 0;
            foreach (TileStresstest ts in _monitorViews.Keys) {
                if (ts.AdvancedTileStresstest.MonitorAfter > monitorAfterTime &&
                    ts.BasicTileStresstest.Monitors.Length != 0)
                    monitorAfterTime = ts.AdvancedTileStresstest.MonitorAfter;
                foreach (MonitorView view in _monitorViews[ts])
                    if (view != null && !view.IsDisposed)
                        ++runningMonitors;
            }
            if (monitorAfter && monitorAfterTime != 0 && runningMonitors != 0) {
                int countdownTime = monitorAfterTime * 60000;
                var monitorAfterCountdown = new Countdown(countdownTime, 5000);
                monitorAfterCountdown.Tick += monitorAfterCountdown_Tick;
                monitorAfterCountdown.Stopped += monitorAfterCountdown_Stopped;
                monitorAfterCountdown.Start();
            } else { StopMonitorsUpdateDetailedResultsAndSetMode(); }
        }
        private void RefreshDetailedResults() {
            ulong[] stresstestIds = null;
            if (_resultsHelper != null && _resultsHelper.DatabaseName != null && _distributedTestMode == DistributedTestMode.Edit && _selectedTestTreeViewItem != null) {
                if (_selectedTestTreeViewItem is TileStresstestTreeViewItem) {
                    var tstvi = _selectedTestTreeViewItem as TileStresstestTreeViewItem;
                    if (_tileStresstestsWithDbIds.ContainsKey(tstvi.TileStresstest))
                        stresstestIds = new ulong[] { _tileStresstestsWithDbIds[tstvi.TileStresstest] };
                } else if (_selectedTestTreeViewItem is TileTreeViewItem) {
                    var l = new List<ulong>();
                    var ttvi = _selectedTestTreeViewItem as TileTreeViewItem;
                    foreach (var ctrl in ttvi.ChildControls)
                        if (ctrl is TileStresstestTreeViewItem) {
                            var tstvi = ctrl as TileStresstestTreeViewItem;
                            if (_tileStresstestsWithDbIds.ContainsKey(tstvi.TileStresstest))
                                l.Add(_tileStresstestsWithDbIds[tstvi.TileStresstest]);
                        }
                    stresstestIds = l.ToArray();
                } else if (_selectedTestTreeViewItem is DistributedTestTreeViewItem) {
                    stresstestIds = new ulong[] { };
                }
            }
            if (stresstestIds == null) {
                detailedResultsControl.ClearResults();
                detailedResultsControl.Enabled = false;
            } else {
                detailedResultsControl.Enabled = true;
                detailedResultsControl.RefreshResults(_resultsHelper, stresstestIds);
            }
        }
        #endregion

        #region Monitors

        private void ShowAndInitMonitorView(TileStresstest tileStresstest, Monitor.Monitor monitor) {
            //show the monitorview
            MonitorView monitorView;
            if (!MonitorViewAlreadyInited(monitor, out monitorView)) {
                ++_pendingMonitorViewInitializations;

                monitorView = SolutionComponentViewManager.Show(monitor) as MonitorView;
                this.Show();

                distributedStresstestControl.AppendMessages("Initializing " + monitorView.Text + "...");
                //For each view initialized, the distributed test view takes care of starting the test.
                monitorView.MonitorInitialized += new EventHandler<MonitorView.MonitorInitializedEventArgs>(monitorView_MonitorInitialized);
                monitorView.OnHandledException += new EventHandler<ErrorEventArgs>(monitorView_OnHandledException);
                monitorView.OnUnhandledException += new EventHandler<ErrorEventArgs>(monitorView_OnUnhandledException);
                monitorView.InitializeForStresstest();
            }

            if (!_monitorViews.ContainsKey(tileStresstest))
                _monitorViews.Add(tileStresstest, new List<MonitorView>());
            _monitorViews[tileStresstest].Add(monitorView);

            if (!_monitorMetricsCaches.ContainsKey(tileStresstest))
                _monitorMetricsCaches.Add(tileStresstest, new MonitorMetricsCache());

#if EnableBetaFeature
            WriteMonitorRestConfig();
#endif
        }

        /// <summary>
        ///     To init it only once.
        /// </summary>
        /// <param name="monitor"></param>
        /// <param name="monitorView">Out this if found.</param>
        /// <returns></returns>
        private bool MonitorViewAlreadyInited(Monitor.Monitor monitor, out MonitorView monitorView) {
            monitorView = null;
            foreach (var l in _monitorViews.Values)
                foreach (MonitorView mv in l)
                    if (mv.Monitor == monitor) {
                        monitorView = mv;
                        return true;
                    }
            return false;
        }

        private void monitorView_MonitorInitialized(object sender, MonitorView.MonitorInitializedEventArgs e) {
            var view = sender as MonitorView;
            view.MonitorInitialized -= monitorView_MonitorInitialized;
            if (--_pendingMonitorViewInitializations == 0) _monitorViewsInitializedWaitHandle.Set();
        }

        private void monitorView_OnHandledException(object sender, ErrorEventArgs e) { var view = sender as MonitorView; }

        private void monitorView_OnUnhandledException(object sender, ErrorEventArgs e) { var view = sender as MonitorView; }

        /// <summary>
        /// Get all monitor result caches for al the running monitors.
        /// </summary>
        /// <returns></returns>
        private List<MonitorResultCache> GetMonitorResultCaches(TileStresstest tileStresstest) {
            var l = new List<MonitorResultCache>();
            if (_monitorViews != null)
                foreach (var ts in _monitorViews.Keys)
                    if (tileStresstest == ts)
                        foreach (var view in _monitorViews[ts])
                            l.Add(view.GetMonitorResultCache());
            return l;
        }

        /// <summary>
        ///     Used in stresstest started eventhandling.
        /// </summary>
        private void StartMonitors() {
            if (_monitorViews != null) {
                int runningMonitors = 0;
                int monitorBefore = 0;
                foreach (TileStresstest ts in _monitorViews.Keys) {
                    if (ts.AdvancedTileStresstest.MonitorBefore > monitorBefore && ts.BasicTileStresstest.Monitors.Length != 0)
                        monitorBefore = ts.AdvancedTileStresstest.MonitorBefore;

                    foreach (MonitorView monitorView in _monitorViews[ts])
                        if (monitorView != null && !monitorView.IsDisposed)
                            try {
                                monitorView.Start();

                                if (_tileStresstestsWithDbIds.ContainsKey(ts))
                                    monitorView.GetMonitorResultCache().MonitorConfigurationId =
                                    _resultsHelper.SetMonitor(_tileStresstestsWithDbIds[ts], monitorView.Monitor.ToString(), monitorView.Monitor.MonitorSource.ToString(),
                                    monitorView.GetConnectionString(), monitorView.Configuration, monitorView.GetMonitorResultCache().Headers);

                                distributedStresstestControl.AppendMessages(monitorView.Text + " is started.");
                                ++runningMonitors;
                            } catch (Exception e) {
                                LogWrapper.LogByLevel(monitorView.Text + " is not started.\n" + e, LogLevel.Error);
                                distributedStresstestControl.AppendMessages(monitorView.Text + " is not started.");

                                try { monitorView.Stop(); } catch { }
                            }
                }

                if (runningMonitors != 0 && monitorBefore != 0) {
                    int countdownTime = monitorBefore * 60000;
                    _monitorBeforeCountDown = new Countdown(countdownTime, 5000);
                    _monitorBeforeCountDown.Tick += monitorBeforeCountDown_Tick;
                    _monitorBeforeCountDown.Stopped += monitorBeforeCountDown_Stopped;
                    _monitorBeforeCountDown.Start();
                } else MonitorBeforeDone();
            } else MonitorBeforeDone();
        }

        private void monitorBeforeCountDown_Tick(object sender, EventArgs e) {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                int countdowntime = _monitorBeforeCountDown == null ? 0 : _monitorBeforeCountDown.CountdownTime;
                var ts = new TimeSpan(countdowntime * TimeSpan.TicksPerMillisecond);
                distributedStresstestControl.AppendMessages("The test will start in " + ts.ToShortFormattedString() +
                                                            ", monitoring first.");

                int runningMonitors = 0;
                foreach (TileStresstest tileStresstest in _monitorViews.Keys)
                    foreach (MonitorView view in _monitorViews[tileStresstest])
                        if (view != null && !view.IsDisposed)
                            runningMonitors++;

                if (runningMonitors == 0) {
                    if (_monitorBeforeCountDown != null) _monitorBeforeCountDown.Stop();
                    distributedStresstestControl.AppendMessages("All monitors were manually closed.");
                }

#if EnableBetaFeature
                WriteMonitorRestConfig();
                WriteMonitorRestProgress();
#endif
            }, null);
        }

        private void monitorBeforeCountDown_Stopped(object sender, EventArgs e) {
            if (_monitorBeforeCountDown != null) {
                _monitorBeforeCountDown.Dispose();
                _monitorBeforeCountDown = null;
            }

#if EnableBetaFeature
            WriteMonitorRestConfig();
            WriteMonitorRestProgress();
#endif

            MonitorBeforeDone();
        }
        private void MonitorBeforeDone() {
            try { _distributedTestCore.Start(); } catch (Exception ex) { HandleInitializeOrStartException(ex); }
        }

        private void monitorAfterCountdown_Tick(object sender, EventArgs e) {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                var monitorAfterCountDown = sender as Countdown;
                var ts = new TimeSpan(monitorAfterCountDown.CountdownTime * TimeSpan.TicksPerMillisecond);
                distributedStresstestControl.AppendMessages("Monitoring after the test is finished: " +
                                                            ts.ToShortFormattedString() + ".");

                int runningMonitors = 0;
                foreach (TileStresstest tileStresstest in _monitorViews.Keys)
                    foreach (MonitorView view in _monitorViews[tileStresstest])
                        if (view != null && !view.IsDisposed)
                            ++runningMonitors;

                if (runningMonitors == 0) {
                    monitorAfterCountDown.Stop();
                    distributedStresstestControl.AppendMessages("All monitors were manually closed.");
                }

#if EnableBetaFeature
                WriteMonitorRestConfig();
                WriteMonitorRestProgress();
#endif
            }, null);
        }
        private void monitorAfterCountdown_Stopped(object sender, EventArgs e) {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate { StopMonitorsUpdateDetailedResultsAndSetMode(); }, null);

            var monitorAfterCountdown = sender as Countdown;
            monitorAfterCountdown.Dispose();
            monitorAfterCountdown = null;

#if EnableBetaFeature
            WriteMonitorRestConfig();
            WriteMonitorRestProgress();
#endif
        }

        /// <summary>
        ///     Only used in stop
        /// </summary>
        private void StopMonitorsUpdateDetailedResultsAndSetMode() {
            //Same view for multiple tilestresstests.
            var stoppedMonitorViews = new List<MonitorView>();
            if (_monitorViews != null)
                foreach (TileStresstest ts in _monitorViews.Keys)
                    foreach (MonitorView view in _monitorViews[ts])
                        if (view != null && !view.IsDisposed && !stoppedMonitorViews.Contains(view)) {
                            stoppedMonitorViews.Add(view);
                            view.Stop();
                            distributedStresstestControl.AppendMessages(view.Text + " is stopped.");

                            try { _resultsHelper.SetMonitorResults(view.GetMonitorResultCache()); } catch (Exception e) {
                                LogWrapper.LogByLevel(view.Text + ": Failed adding results to the database.\n" + e, LogLevel.Error);
                            }
                        }

            stoppedMonitorViews = null;

            SetMode(DistributedTestMode.Edit, true);

            //Update the detailed results in the gui if any.
            RefreshDetailedResults();
        }
        #endregion

        #region REST
        private void WriteMonitorRestConfig() {
            try {
                var monitorConfigCache = new ConverterCollection();
                foreach (TileStresstest key in _monitorViews.Keys)
                    foreach (MonitorView view in _monitorViews[key])
                        Converter.SetMonitorConfig(monitorConfigCache, _distributedTest.ToString(), view.Monitor);

                Converter.WriteToFile(monitorConfigCache, "MonitorConfig");
            } catch {
            }
        }
        private void WriteMonitorRestProgress() {
            try {
                var monitorProgressCache = new ConverterCollection();
                int monitorCount = 0;

                if (_monitorViews != null)
                    foreach (TileStresstest key in _monitorViews.Keys)
                        foreach (MonitorView view in _monitorViews[key]) {
                            ++monitorCount;
                            Converter.SetMonitorProgress(monitorProgressCache, _distributedTest.ToString(), view.Monitor,
                                                         view.GetMonitorResultCache().Headers, view.GetMonitorValues());
                        }
                if (monitorCount != 0) Converter.WriteToFile(monitorProgressCache, "MonitorProgress");
            } catch { }
        }
        private void WriteRestProgress() {
            try {
                var testProgressCache = new ConverterCollection();
                if (_distributedTestCore != null && !_distributedTestCore.IsDisposed) {
                    foreach (TileStresstest tileStresstest in _distributedTestCore.TestProgressMessages.Keys) {
                        var tpm = _distributedTestCore.TestProgressMessages[tileStresstest];
                        foreach (var metrics in tpm.StresstestMetricsCache.GetConcurrencyMetrics()) {
                            Converter.SetTestProgress(testProgressCache, _distributedTest.ToString(), "Tile " + (tileStresstest.Parent as Tile).Index + " Stresstest " +
                                                      tileStresstest.Index + " " + tileStresstest.BasicTileStresstest.Connection.Label, metrics, tpm.RunStateChange, tpm.StresstestStatus);
                        }
                    }

                }
                Converter.WriteToFile(testProgressCache, "TestProgress");
            } catch {
            }
        }

        #endregion
    }
}
