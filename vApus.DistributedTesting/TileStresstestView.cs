﻿/*
 * Copyright 2009 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using RandomUtils;
using RandomUtils.Log;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using vApus.Monitor;
using vApus.Results;
using vApus.SolutionTree;
using vApus.Stresstest;
using vApus.Util;

namespace vApus.DistributedTesting {
    public partial class TileStresstestView : BaseSolutionComponentView {
        public event EventHandler<vApus.Stresstest.TestInitializedEventArgs> TestInitialized;

        #region Fields
        /// <summary>
        ///     Lock break, continue, push message
        /// </summary>
        private readonly object _lock = new object();

        private readonly Stresstest.Stresstest _stresstest;
        private string _tileStresstestIndex;

        /// <summary>
        ///     In seconds how fast the stresstest progress will be updated.
        /// </summary>
        private const int PROGRESSUPDATEDELAY = 5;
        /// <summary>
        ///     Countdown for the update.
        /// </summary>
        private int _progressCountDown;

        private bool _canUpdateMetrics = false; //Can only be updated when a run is busy.
        private bool _simplifiedMetricsReturned = false; //Only send a warning to the user once.

        private StresstestCore _stresstestCore;
        private StresstestResult _stresstestResult;
        /// <summary>
        ///     Caching the results to visualize in the stresstestcontrol.
        /// </summary>
        private FastStresstestMetricsCache _stresstestMetricsCache;
        private StresstestStatus _stresstestStatus;

        private ResultsHelper _resultsHelper = new ResultsHelper();

        /// <summary>
        ///     Don't send push messages anymore if it is finished (stop on form closing);
        /// </summary>
        private bool _finishedSent;
        #endregion

        #region Properties
        /// <summary>
        ///     Store to identify the right stresstest.
        /// </summary>
        public string TileStresstestIndex {
            get { return _tileStresstestIndex; }
            set { _tileStresstestIndex = value; }
        }

        public RunSynchronization RunSynchronization { get; set; }
        public int MaxRerunsBreakOnLast { get; set; }
        public StresstestResult StresstestResult {
            get { return _stresstestResult; }
        }

        /// <summary>
        /// For adding results to the database.
        /// </summary>
        public int StresstestIdInDb {
            get { return _resultsHelper.StresstestId; }
            set { _resultsHelper.StresstestId = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        ///     Designer time constructor
        /// </summary>
        public TileStresstestView() { InitializeComponent(); }
        public TileStresstestView(SolutionComponent solutionComponent)
            : base(solutionComponent) {
            Solution.RegisterForCancelFormClosing(this);
            _stresstest = SolutionComponent as Stresstest.Stresstest;

            InitializeComponent();
            if (IsHandleCreated)
                SetGui();
            else
                HandleCreated += StresstestProjectView_HandleCreated;
        }
        #endregion

        #region Functions

        #region Set the Gui
        private void StresstestProjectView_HandleCreated(object sender, EventArgs e) {
            SetGui();
        }

        private void SetGui() {
            Text = SolutionComponent.ToString();
            // fastResultsControl.ResultsHelper = _resultsHelper;
        }

        public override void Refresh() {
            base.Refresh();
            SetGui();
        }
        #endregion

        #region Start
        public void ConnectToExistingDatabase(string host, int port, string databaseName, string user, string password) {
            try {
                _resultsHelper.ConnectToExistingDatabase(host, port, databaseName, user, password);
            } catch {
                throw new Exception("MAKE SURE THAT YOU DO NOT TARGET 'localhost', '127.0.0.1', '0:0:0:0:0:0:0:1' or '::1' IN 'Options' > 'Saving Test Results' and that the connection limit (max_connections setting in my.ini) is set high enough!\nA connection to the results server could not be made!");
            }
        }
        /// <summary>
        ///     Thread safe
        /// </summary>
        public void InitializeTest() {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                Cursor = Cursors.WaitCursor;
                btnStop.Enabled = true;
                try { LocalMonitor.StartMonitoring(PROGRESSUPDATEDELAY * 1000); } catch { fastResultsControl.AddEvent("Could not initialize the local monitor, something is wrong with your WMI.", Level.Error); }
                tmrProgress.Interval = PROGRESSUPDATEDELAY * 1000;
                tmrProgress.Start();

                fastResultsControl.SetStresstestInitialized();
                _stresstestResult = null;
                _stresstestMetricsCache = new FastStresstestMetricsCache();
                fastResultsControl.SetConfigurationControls(_stresstest);

                _progressCountDown = PROGRESSUPDATEDELAY - 1;
                try {
                    _stresstestCore = new StresstestCore(_stresstest);
                    _stresstestCore.WaitWhenInitializedTheFirstRun = true;
                    _stresstestCore.ResultsHelper = _resultsHelper;
                    _stresstestCore.RunSynchronization = RunSynchronization;
                    _stresstestCore.MaxRerunsBreakOnLast = MaxRerunsBreakOnLast;
                    _stresstestCore.StresstestStarted += _stresstestCore_StresstestStarted;
                    _stresstestCore.ConcurrencyStarted += _stresstestCore_ConcurrentUsersStarted;
                    _stresstestCore.ConcurrencyStopped += _stresstestCore_ConcurrencyStopped;
                    _stresstestCore.RunInitializedFirstTime += _stresstestCore_RunInitializedFirstTime;
                    _stresstestCore.RunStarted += _stresstestCore_RunStarted;
                    _stresstestCore.RunDoneOnce += _stresstestCore_RunDoneOnce;
                    _stresstestCore.RerunDone += _stresstestCore_RerunDone;
                    _stresstestCore.RunStopped += _stresstestCore_RunStopped;
                    _stresstestCore.Message += _stresstestCore_Message;

                    _stresstestCore.TestInitialized += _stresstestCore_TestInitialized;
                    ThreadPool.QueueUserWorkItem((state) => { _stresstestCore.InitializeTest(); }, null);
                } catch (Exception ex) {
                    Stop(ex);

                    if (TestInitialized != null) TestInitialized(this, new Stresstest.TestInitializedEventArgs(ex));
                }
            }, null);
        }

        private void _stresstestCore_TestInitialized(object sender, Stresstest.TestInitializedEventArgs e) {
            _stresstestCore.TestInitialized -= _stresstestCore_TestInitialized;
            SynchronizationContextWrapper.SynchronizationContext.Send((state) => {
                if (e.Exception == null) {
                    try {
                        fastResultsControl.SetClientMonitoring(_stresstestCore.BusyThreadCount, LocalMonitor.CPUUsage,
                                                              (int)LocalMonitor.MemoryUsage, (int)LocalMonitor.TotalVisibleMemory,
                                                              LocalMonitor.Nic, LocalMonitor.NicBandwidth,
                                                              LocalMonitor.NicSent, LocalMonitor.NicReceived);
                    } catch { } //Exception on false WMI. 
                } else {
                    Stop(e.Exception);
                }
                Cursor = Cursors.Default;

                if (TestInitialized != null) TestInitialized(this, e);
            }, null);
        }

        /// <summary>
        ///     Thread safe
        /// </summary>
        public void StartTest() {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                if (_stresstestCore == null) return;

                _stresstestStatus = StresstestStatus.Busy;

                //The stresstest threadpool is blocking so we run this on another thread.
                var stresstestThread = new Thread(() => {
                    Exception ex = null;
                    try {
                        _stresstestStatus = _stresstestCore.ExecuteStresstest();
                        _stresstestResult = _stresstestCore.StresstestResult;
                    } catch (Exception e) { ex = e; } finally {
                        if (_stresstestCore != null && !_stresstestCore.IsDisposed)
                            SynchronizationContextWrapper.SynchronizationContext.Send(delegate {
                                Stop(ex);
                            }, null);
                    }
                });

                stresstestThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                stresstestThread.IsBackground = true;
                stresstestThread.Start();
            }, null);
        }
        #endregion

        /// <summary>
        ///     Thread safe
        /// </summary>
        public void Break() {
            lock (_lock)
                if (_stresstestStatus == StresstestStatus.Busy)
                    _stresstestCore.Break();

        }

        /// <summary>
        ///     Thread safe
        /// </summary>
        /// <param name="continueCounter">Every time the execution is paused the continue counter is incremented by one.</param>
        public void Continue(int continueCounter) {
            lock (_lock)
                if (_stresstestStatus == StresstestStatus.Busy)
                    _stresstestCore.Continue(continueCounter);
        }

        /// <summary>
        ///     Thread safe, Keeping the shared run for a divided tile stresstest in sync.
        /// </summary>
        public void ContinueDivided() {
            lock (_lock)
                if (_stresstestStatus == StresstestStatus.Busy)
                    _stresstestCore.ContinueDivided();
        }

        #region Progress
        private void tmrProgressDelayCountDown_Tick(object sender, EventArgs e) { fastResultsControl.SetCountDownProgressDelay(_progressCountDown--); }
        private void tmrProgress_Tick(object sender, ElapsedEventArgs e) {
            try {
                fastResultsControl.SetClientMonitoring(
                    _stresstestCore == null ? 0 : _stresstestCore.BusyThreadCount, LocalMonitor.CPUUsage, (int)LocalMonitor.MemoryUsage,
                    (int)LocalMonitor.TotalVisibleMemory, LocalMonitor.Nic, LocalMonitor.NicBandwidth, LocalMonitor.NicSent, LocalMonitor.NicReceived);
            } catch { } //Exception on false WMI. 

            if (_canUpdateMetrics) {
                fastResultsControl.UpdateFastConcurrencyResults(_stresstestMetricsCache.GetConcurrencyMetrics(), true, _stresstestMetricsCache.SimplifiedMetrics);
                List<StresstestMetrics> runMetrics = _stresstestMetricsCache.GetRunMetrics();
                fastResultsControl.UpdateFastRunResults(runMetrics, false, _stresstestMetricsCache.SimplifiedMetrics);

                //Set rerunning
                fastResultsControl.SetRerunning(runMetrics.Count == 0 ? false : runMetrics[runMetrics.Count - 1].RerunCount != 0);

                if (_stresstestMetricsCache.SimplifiedMetrics && !_simplifiedMetricsReturned) {
                    _simplifiedMetricsReturned = true;
                    fastResultsControl.AddEvent("It takes too long to calculate the fast results, therefore they are simplified!", Level.Warning);
                }

            }
            _progressCountDown = PROGRESSUPDATEDELAY;

            SendPushMessage(RunStateChange.None, false, false);
        }

        private void _stresstestCore_StresstestStarted(object sender, StresstestResultEventArgs e) {
            _simplifiedMetricsReturned = false;
            _stresstestResult = e.StresstestResult;
            fastResultsControl.SetStresstestStarted(e.StresstestResult.StartedAt);
        }

        private void _stresstestCore_ConcurrentUsersStarted(object sender, ConcurrencyResultEventArgs e) {
            _progressCountDown = PROGRESSUPDATEDELAY;
            StopProgressDelayCountDown();
            //Update the metrics.
            fastResultsControl.UpdateFastConcurrencyResults(_stresstestMetricsCache.AddOrUpdate(e.Result), true, _stresstestMetricsCache.SimplifiedMetrics);
            fastResultsControl.SetRerunning(false);
        }
        private void _stresstestCore_ConcurrencyStopped(object sender, ConcurrencyResultEventArgs e) { SendPushMessage(RunStateChange.None, false, true); }

        private void _stresstestCore_RunInitializedFirstTime(object sender, RunResultEventArgs e) {
            StopProgressDelayCountDown();

            fastResultsControl.UpdateFastRunResults(_stresstestMetricsCache.AddOrUpdate(e.Result), true, _stresstestMetricsCache.SimplifiedMetrics);
            fastResultsControl.UpdateFastConcurrencyResults(_stresstestMetricsCache.GetConcurrencyMetrics(), false, _stresstestMetricsCache.SimplifiedMetrics);

            SendPushMessage(RunStateChange.ToRunInitializedFirstTime, false, false);

            fastResultsControl.SetRerunning(false);

            _progressCountDown = PROGRESSUPDATEDELAY;
            tmrProgress.Stop();
            fastResultsControl.SetCountDownProgressDelay(_progressCountDown--);
            tmrProgressDelayCountDown.Start();
            tmrProgress.Start();
        }

        private void _stresstestCore_RunStarted(object sender, RunResultEventArgs e) { _canUpdateMetrics = true; }
        private void _stresstestCore_RunDoneOnce(object sender, EventArgs e) { SendPushMessage(RunStateChange.ToRunDoneOnce, false, false); }
        private void _stresstestCore_RerunDone(object sender, EventArgs e) { SendPushMessage(RunStateChange.ToRerunDone, false, false); }
        private void _stresstestCore_RunStopped(object sender, RunResultEventArgs e) {
            _canUpdateMetrics = false;
            SendPushMessage(RunStateChange.None, true, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="runStateChange"></param>
        private void SendPushMessage(RunStateChange runStateChange, bool runFinished, bool concurrencyFinished) {
            if (!_finishedSent) {
                var estimatedRuntimeLeft = FastStresstestMetricsHelper.GetEstimatedRuntimeLeft(_stresstestResult, _stresstest.Concurrencies.Length, _stresstest.Runs);
                var events = new List<EventPanelEvent>();
                try { events = fastResultsControl.GetEvents(); } catch (Exception ex) {
                    Loggers.Log(Level.Error, "Failed getting events.", ex);
                }
                SlaveSideCommunicationHandler.SendPushMessage(_tileStresstestIndex, _stresstestMetricsCache, _stresstestStatus, fastResultsControl.StresstestStartedAt,
                    fastResultsControl.MeasuredRuntime, estimatedRuntimeLeft, _stresstestCore, events, runStateChange, runFinished, concurrencyFinished);
                if (_stresstestStatus != StresstestStatus.Busy) _finishedSent = true;
            }
        }

        /// <summary>
        ///     Refreshes the messages from the StresstestCore for a selected node and refreshes the listed results.
        /// </summary>
        private void _stresstestCore_Message(object sender, MessageEventArgs e) {
            if (e.Color == Color.Empty) fastResultsControl.AddEvent(e.Message, e.LogLevel);
            else fastResultsControl.AddEvent(e.Message, e.Color, e.LogLevel);
        }
        #endregion

        #region Stop
        private void TileStresstestView_FormClosing(object sender, FormClosingEventArgs e) {
            if (!btnStop.Enabled || MessageBox.Show("Are you sure you want to close a running test?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                StopStresstest();

                tmrProgress.Stop();
                StopProgressDelayCountDown();


                if (_stresstestCore != null && !_stresstestCore.IsDisposed) {
                    _stresstestCore.Dispose();
                    _stresstestCore = null;
                }

                SendPushMessage(RunStateChange.None, false, false);
            } else {
                Solution.ExplicitCancelFormClosing = true;
                e.Cancel = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e) {
            PerformStopClick();
        }

        /// <summary>
        ///     To stop the test from the slave side communication handler.
        /// </summary>
        public void PerformStopClick() {
            if (btnStop.Enabled) {
                int busyThreadCount = -1;
                if (_stresstestCore != null) {
                    busyThreadCount = _stresstestCore.BusyThreadCount;
                    _stresstestCore.Cancel(); // Can only be cancelled once, calling multiple times is not a problem.
                }

                //Nasty, but last resort.
                ThreadPool.QueueUserWorkItem((state) => {
                    for (int i = 0; i != 10001; i++) {
                        if (_stresstestCore == null) break;
                        Thread.Sleep(1);
                    }
                    if (_stresstestCore != null || busyThreadCount == 0)
                        SynchronizationContextWrapper.SynchronizationContext.Send((x) => {
                            Stop();
                        }, null);
                });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ex">The exception if failed.</param>
        private void Stop(Exception ex = null) {
            Cursor = Cursors.WaitCursor;
            try {
                StopStresstest();

                tmrProgress.Stop();
                StopProgressDelayCountDown();

                btnStop.Enabled = false;
                if (ex == null)
                    fastResultsControl.SetStresstestStopped(_stresstestStatus);
                else {
                    _stresstestStatus = StresstestStatus.Error;
                    fastResultsControl.SetStresstestStopped(_stresstestStatus, ex);
                }

                if (_stresstestCore != null && !_stresstestCore.IsDisposed) {
                    _stresstestCore.Dispose();
                    _stresstestCore = null;
                }

                SendPushMessage(RunStateChange.None, false, false);
            } catch (Exception eeee) {
                MessageBox.Show(eeee.ToString());
                Loggers.Log(Level.Error, "Failed stopping the test.", eeee);
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        ///     Only used in stop
        /// </summary>
        private void StopStresstest() {
            if (_stresstestCore != null && !_stresstestCore.IsDisposed) {
                try {
                    fastResultsControl.SetClientMonitoring(_stresstestCore.BusyThreadCount, LocalMonitor.CPUUsage,
                                                          (int)LocalMonitor.MemoryUsage,
                                                          (int)LocalMonitor.TotalVisibleMemory, LocalMonitor.Nic, LocalMonitor.NicBandwidth,
                                                          LocalMonitor.NicSent, LocalMonitor.NicReceived);
                } catch { } //Exception on false WMI. 

                _stresstestMetricsCache.AllowSimplifiedMetrics = false;
                fastResultsControl.UpdateFastConcurrencyResults(_stresstestMetricsCache.GetConcurrencyMetrics(), true, _stresstestMetricsCache.SimplifiedMetrics);
                fastResultsControl.UpdateFastRunResults(_stresstestMetricsCache.GetRunMetrics(), false, _stresstestMetricsCache.SimplifiedMetrics);

                fastResultsControl.SetRerunning(false);

                // Can only be cancelled once, calling multiple times is not a problem.
                if (_stresstestCore != null && !_stresstestCore.IsDisposed)
                    try {
                        _stresstestCore.Cancel();
                    } catch (Exception ex) {
                        Loggers.Log(Level.Error, "Failed cancelling the test.", ex);
                    }
            }

            fastResultsControl.SetStresstestStopped();
            _stresstestResult = null;
            _canUpdateMetrics = false;
        }

        private void StopProgressDelayCountDown() {
            try {
                tmrProgressDelayCountDown.Stop();
                if (fastResultsControl != null && !fastResultsControl.IsDisposed)
                    fastResultsControl.SetCountDownProgressDelay(-1);
            } catch {
                //Don't care.
            }
        }
        #endregion

        #endregion
    }
}