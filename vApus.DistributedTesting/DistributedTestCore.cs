﻿/*
 * Copyright 2010 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using vApus.REST.Convert;
using vApus.Results;
using vApus.Stresstest;
using vApus.Util;

namespace vApus.DistributedTesting {
    /// <summary>
    /// Handles all messages coming from MasterSideCommunicationHandler and does run sync magic.
    /// Only one test can run at the same time, if this is called and another test (stresstest core or distributed test core) exists (not disposed) an argument out of range exception will be thrown.
    /// </summary>
    public class DistributedTestCore {
        /// <summary>
        ///     Messages to the user on the message textbox
        /// </summary>
        public event EventHandler<MessageEventArgs> Message;
        /// <summary>
        /// Get the TileStresstestMessage from eg var tpms = _distributedTestCore.TileStresstestMessages; var tpm = tpms[e.TileStresstest].
        /// </summary>
        public event EventHandler OnTestProgressMessageReceivedDelayed;
        public event EventHandler<ListeningErrorEventArgs> OnListeningError;
        public event EventHandler<TestFinishedEventArgs> OnFinished;

        #region Fields
        private readonly DistributedTest _distributedTest;
        // For adding and getting results.
        private Dictionary<TileStresstest, int> _tileStresstestsWithDbIds;

        private readonly object _lock = new object();

        private volatile string[] _cancelled = new string[] { };
        private object _communicationLock = new object();
        private volatile string[] _failed = new string[] { };

        //Invoke only once
        private bool _finishedHandled;
        private bool _hasResults;
        private bool _isDisposed;

        private volatile string[] _ok = new string[] { };

        private volatile string[] _runDoneOnce = new string[] { };
        private volatile string[] _runInitialized = new string[] { };
        private volatile ConcurrentDictionary<string, RerunCounter> _breakOnLastReruns = new ConcurrentDictionary<string, RerunCounter>();
        private Stopwatch _sw = new Stopwatch();

        /// <summary>
        ///     The messages pushed from the slaves.
        /// </summary>
        private Dictionary<TileStresstest, Dictionary<string, TestProgressMessage>> _testProgressMessages = new Dictionary<TileStresstest, Dictionary<string, TestProgressMessage>>();
        /// <summary>
        /// To know when a combined run is finished.
        /// </summary>
        private Dictionary<TileStresstest, List<string>> _dividedRunInitializedOrDoneOnce = new Dictionary<TileStresstest, List<string>>();

        private object _testProgressMessagesLock = new object();
        private Dictionary<TileStresstest, TileStresstest> _usedTileStresstests = new Dictionary<TileStresstest, TileStresstest>(); //the divided stresstests and the originals
        private int _totalTestCount = 0; //This way we do not need a lock to get the count.

        private ResultsHelper _resultsHelper;

        private System.Timers.Timer _tmrOnInvokeTestProgressMessageReceivedDelayed = new System.Timers.Timer(1000);
        #endregion

        #region Properties
        /// <summary>
        /// Key = Divided Stresstest, Value = original
        /// </summary>
        public Dictionary<TileStresstest, TileStresstest> UsedTileStresstests { get { return _usedTileStresstests; } }

        /// <returns>-1 if not found or the first Id if the given is a divided tile stresstest.</returns>
        public int GetDbId(TileStresstest tileStresstest) {
            string tileStresstestToString = ReaderAndCombiner.GetCombinedStresstestToString(tileStresstest.ToString());
            foreach (TileStresstest ts in _tileStresstestsWithDbIds.Keys)
                if (tileStresstestToString == ReaderAndCombiner.GetCombinedStresstestToString(ts.ToString()))
                    return _tileStresstestsWithDbIds[ts];
            return -1;
        }

        public bool IsDisposed { get { return _isDisposed; } }

        public int OK { get { return _ok.Length; } }

        public int Cancelled { get { return _cancelled.Length; } }

        public int Failed { get { return _failed.Length; } }

        /// <summary>
        ///     The stresstests that are not finished.
        /// </summary>
        public int Running { get { return _totalTestCount - Finished; } }

        public int Finished { get { return OK + Cancelled + Failed; } }

        /// <summary>
        ///     To check wheter you can close the distributed test view without a warning or not.
        /// </summary>
        public bool HasResults { get { return _hasResults; } }
        #endregion

        #region Con-/Destructor
        /// <summary>
        /// Handles all messages coming from MasterSideCommunicationHandler and does run sync magic.
        /// Only one test can run at the same time, if this is called and another test (stresstest core or distributed test core) exists (not disposed) an argument out of range exception will be thrown.
        /// </summary>
        /// <param name="distributedTest"></param>
        /// <param name="resultsHelper"></param>
        public DistributedTestCore(DistributedTest distributedTest, ResultsHelper resultsHelper) {
            ObjectRegistrar.MaxRegistered = 1;
            ObjectRegistrar.Register(this);

            _distributedTest = distributedTest;
            _resultsHelper = resultsHelper;
            MasterSideCommunicationHandler.ListeningError += _masterCommunication_ListeningError;
            MasterSideCommunicationHandler.TestInitialized += MasterSideCommunicationHandler_TestInitialized;
            MasterSideCommunicationHandler.OnTestProgressMessageReceived += _masterCommunication_OnTestProgressMessageReceived;

            _tmrOnInvokeTestProgressMessageReceivedDelayed.Elapsed += _tmrOnInvokeTestProgressMessageReceivedDelayed_Elapsed;

#if EnableBetaFeature
            WriteRestConfig();
#endif
        }
        ~DistributedTestCore() {
            Dispose();
            GC.Collect();
        }
        #endregion

        #region Functions

        #region Event Handling
        private void InvokeMessage(string message, LogLevel logLevel = LogLevel.Info) { InvokeMessage(message, Color.Empty, logLevel); }
        private void InvokeMessage(string message, Color color, LogLevel logLevel = LogLevel.Info) {
            LogWrapper.LogByLevel(message, logLevel);
            if (Message != null) {
                if (logLevel == LogLevel.Error) {
                    string[] split = message.Split(new[] { '\n', '\r' }, StringSplitOptions.None);
                    message = split[0] + "\n\nSee " +
                              Path.Combine(Logger.DEFAULT_LOCATION, DateTime.Now.ToString("dd-MM-yyyy") + " " + LogWrapper.Default.Logger.Name + ".txt");
                }
                SynchronizationContextWrapper.SynchronizationContext.Send(delegate { Message(this, new MessageEventArgs(message, color, logLevel)); }, null);
            }
        }

        private void InvokeOnTestProgressMessageReceivedDelayed(TileStresstest tileStresstest) {
            if (OnTestProgressMessageReceivedDelayed != null && _tmrOnInvokeTestProgressMessageReceivedDelayed != null)
                try {
                    _tmrOnInvokeTestProgressMessageReceivedDelayed.Stop();
                    _tmrOnInvokeTestProgressMessageReceivedDelayed.Start();
                } catch { }
        }
        private void _tmrOnInvokeTestProgressMessageReceivedDelayed_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (!_isDisposed && _tmrOnInvokeTestProgressMessageReceivedDelayed != null)
                try {
                    _tmrOnInvokeTestProgressMessageReceivedDelayed.Stop();
                    SynchronizationContextWrapper.SynchronizationContext.Send(delegate { OnTestProgressMessageReceivedDelayed(this, null); }, null);
                } catch { }
        }

        private void InvokeOnListeningError(ListeningErrorEventArgs listeningErrorEventArgs) {
            LogWrapper.LogByLevel(listeningErrorEventArgs, LogLevel.Error);
            if (OnListeningError != null)
                SynchronizationContextWrapper.SynchronizationContext.Send(delegate { OnListeningError(this, listeningErrorEventArgs); }, null);
        }

        private void InvokeOnFinished() {
            if (OnFinished != null)
                SynchronizationContextWrapper.SynchronizationContext.Send(
                    delegate { OnFinished(this, new TestFinishedEventArgs(OK, Cancelled, Failed)); }, null);
        }
        #endregion

        #region Init
        /// <summary>
        ///     Connects + sends tests
        /// </summary>
        /// <returns>Stresstests and the IDs in the db</returns>
        public void Initialize(out bool notACleanDivision) {
            InvokeMessage("Initializing the Test.");

            Connect(out notACleanDivision);
            SetvApusInstancesAndStresstestsInDb();
            SendAndReceiveInitializeTest();
        }
        private void Connect(out bool notACleanDivision) {
            InvokeMessage("Connecting slaves...");
            _sw.Start();

            MasterSideCommunicationHandler.Init();

            _usedTileStresstests = DivideEtImpera.DivideTileStresstestsOverSlaves(_distributedTest, out notACleanDivision);
            _totalTestCount = _usedTileStresstests.Count;

            foreach (var dividedTileStresstest in _usedTileStresstests.Keys) {
                Exception exception = null;
                int slaveCount = dividedTileStresstest.BasicTileStresstest.SlaveIndices.Length;

                string tileStresstestIndex = dividedTileStresstest.TileStresstestIndex;

                var slave = dividedTileStresstest.BasicTileStresstest.Slaves[0];
                int processID;
                MasterSideCommunicationHandler.ConnectSlave(slave.IP, slave.Port, out processID, out exception);

                if (exception == null) {
                    lock (_testProgressMessagesLock) {
                        var original = _usedTileStresstests[dividedTileStresstest];
                        if (!_testProgressMessages.ContainsKey(original))
                            _testProgressMessages.Add(original, new Dictionary<string, TestProgressMessage>());

                        _testProgressMessages[original].Add(tileStresstestIndex, new TestProgressMessage());
                    }
                    InvokeMessage(string.Format("|->Connected {0} - {1}", dividedTileStresstest.Parent, dividedTileStresstest));
                } else {
                    Dispose();
                    var ex = new Exception(string.Format("Could not connect to one of the slaves ({0} - {1})!{2}{3}", dividedTileStresstest.Parent, dividedTileStresstest, Environment.NewLine, exception));
                    string message = ex.Message + "\n" + ex.StackTrace + "\n\nSee " +
                                     Path.Combine(Logger.DEFAULT_LOCATION, DateTime.Now.ToString("dd-MM-yyyy") + " " + LogWrapper.Default.Logger.Name + ".txt");
                    InvokeMessage(message, LogLevel.Error);
                    throw ex;
                }
            }
            if (_totalTestCount == 0) {
                var ex = new Exception("Please use at least one test!");
                string message = ex.Message + "\n" + ex.StackTrace + "\n\nSee " +
                   Path.Combine(Logger.DEFAULT_LOCATION, DateTime.Now.ToString("dd-MM-yyyy") + " " + LogWrapper.Default.Logger.Name + ".txt");
                InvokeMessage(message, LogLevel.Warning);
                throw ex;
            }
            _sw.Stop();
            InvokeMessage(string.Format(" ...Connected slaves in {0}", _sw.Elapsed.ToLongFormattedString()));
            _sw.Reset();
            // Thread.Sleep(10000);
        }
        private void SetvApusInstancesAndStresstestsInDb() {
            _tileStresstestsWithDbIds = new Dictionary<TileStresstest, int>(_usedTileStresstests.Count);
            foreach (TileStresstest ts in _usedTileStresstests.Keys) {
                var slave = ts.BasicTileStresstest.Slaves[0];
                _resultsHelper.SetvApusInstance(slave.HostName, slave.IP, slave.Port, string.Empty, string.Empty, false);
                int id = _resultsHelper.SetStresstest(ts.ToString(), _distributedTest.RunSynchronization.ToString(), ts.BasicTileStresstest.Connection.ToString(), ts.BasicTileStresstest.ConnectionProxy,
                          ts.BasicTileStresstest.Connection.ConnectionString, ts.AdvancedTileStresstest.Log.ToString(), ts.AdvancedTileStresstest.LogRuleSet, ts.AdvancedTileStresstest.Concurrencies,
                          ts.AdvancedTileStresstest.Runs, ts.AdvancedTileStresstest.MinimumDelay, ts.AdvancedTileStresstest.MaximumDelay, ts.AdvancedTileStresstest.Shuffle, ts.AdvancedTileStresstest.Distribute.ToString(),
                          ts.AdvancedTileStresstest.MonitorBefore, ts.AdvancedTileStresstest.MonitorAfter);
                _tileStresstestsWithDbIds.Add(ts, id);
            }

            _resultsHelper.SetvApusInstance(Dns.GetHostName(), NamedObjectRegistrar.Get<string>("IP"), NamedObjectRegistrar.Get<int>("Port"),
                    NamedObjectRegistrar.Get<string>("vApusVersion") ?? string.Empty, NamedObjectRegistrar.Get<string>("vApusChannel") ?? string.Empty,
                    true);
        }
        private void SendAndReceiveInitializeTest() {
            InvokeMessage("Initializing tests on slaves [Please, be patient]...");
            _sw.Start();
            List<int> stresstestIdsInDb = new List<int>(_usedTileStresstests.Count);
            foreach (var ts in _usedTileStresstests.Keys)
                stresstestIdsInDb.Add(_tileStresstestsWithDbIds.ContainsKey(ts) ? _tileStresstestsWithDbIds[ts] : 0);

            Exception exception = MasterSideCommunicationHandler.InitializeTests(_usedTileStresstests, stresstestIdsInDb, _resultsHelper.DatabaseName, _distributedTest.RunSynchronization, _distributedTest.MaxRerunsBreakOnLast);
            if (exception != null) {
                var ex = new Exception("Could not initialize one or more tests!\n" + exception);
                InvokeMessage(ex.ToString(), LogLevel.Error);
                throw ex;
            }

            _sw.Stop();
            InvokeMessage(string.Format(" ...Test initialized in {0}", _sw.Elapsed.ToLongFormattedString()));
            _sw.Reset();
        }
        private void MasterSideCommunicationHandler_TestInitialized(object sender, TestInitializedEventArgs e) {
            lock (_lock)
                if (e.Exception == null) {
                    InvokeMessage(string.Format("|->Initialized {0} - {1}", e.TileStresstest.Parent, e.TileStresstest));
                } else {
                    var ex =
                        new Exception(string.Format("Could not initialize {0} - {1}.{2}{3}", e.TileStresstest.Parent,
                                                    e.TileStresstest, Environment.NewLine, e.Exception));
                    InvokeMessage(ex.ToString(), LogLevel.Error);
                }
        }

        public void Start() {
            InvokeMessage("Starting the test...");
            Exception exception;
            MasterSideCommunicationHandler.StartTest(out exception);

            if (exception == null) {
                InvokeMessage(" ...Started!", Color.LightGreen);
            } else {
                Dispose();
                InvokeMessage(exception.ToString(), LogLevel.Error);
                throw exception;
            }

            if (exception != null) {
                Dispose();
                var ex =
                    new Exception(string.Format("Could not start the Distributed Test.{0}{1}", Environment.NewLine,
                                                exception.ToString()));
                InvokeMessage(ex.ToString(), LogLevel.Error);
                throw ex;
            }
        }
        #endregion

        #region Progress
        private void _masterCommunication_ListeningError(object sender, ListeningErrorEventArgs e) {
            if (_isDisposed)
                return;

            lock (_communicationLock) {
                try {
                    if (!_isDisposed && Finished < _totalTestCount) {
#warning Allow multiple slaves for work distribution
                        foreach (TileStresstest tileStresstest in _usedTileStresstests.Keys)
                            if (tileStresstest.BasicTileStresstest.Slaves[0].IP == e.SlaveIP &&
                                tileStresstest.BasicTileStresstest.Slaves[0].Port == e.SlavePort)
                                _failed = AddUniqueToStringArray(_failed, tileStresstest.TileStresstestIndex);

                        InvokeOnListeningError(e);

                        Stop();
                        //The test cannot be valid, therefore stop testing. It is stopped in the gui also, but stop it here explicitely for when the gui fails.

                        if (Finished == _totalTestCount)
                            HandleFinished();
                    }
                } catch {
                }
            }
        }

        /// <summary>
        /// All the heavy lifting happens here (run sync).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _masterCommunication_OnTestProgressMessageReceived(object sender, TestProgressMessageReceivedEventArgs e) {
            if (_isDisposed)
                return;

            lock (_communicationLock) {
                try {
                    _hasResults = true;

                    TestProgressMessage tpm = e.TestProgressMessage;
                    TileStresstest originalTileStresstest = DivideEtImpera.GetOriginalTileStresstest(tpm.TileStresstestIndex, _usedTileStresstests);
                    RunStateChange combinedRunStateChanged = DivideEtImpera.PreProcessTestProgressMessage(originalTileStresstest, tpm, _testProgressMessages, _usedTileStresstests, _dividedRunInitializedOrDoneOnce);

                    bool okCancelError = true;
                    switch (tpm.StresstestStatus) {
                        case StresstestStatus.Busy:
                            okCancelError = false;
                            break;
                        case StresstestStatus.Ok:
                            _ok = AddUniqueToStringArray(_ok, tpm.TileStresstestIndex);
                            break;
                        case StresstestStatus.Cancelled:
                            _cancelled = AddUniqueToStringArray(_cancelled, tpm.TileStresstestIndex);
                            break;
                        case StresstestStatus.Error:
                            _failed = AddUniqueToStringArray(_failed, tpm.TileStresstestIndex);
                            break;
                    }

                    //Check if it is needed to do anything
                    if (Running != 0 && Cancelled == 0 && Failed == 0) {
                        //Threat this as stopped.
                        if (okCancelError) _runDoneOnce = AddUniqueToStringArray(_runDoneOnce, tpm.TileStresstestIndex);

                        switch (_distributedTest.RunSynchronization) {
                            case RunSynchronization.None:
                                break;

                            //Send Break, wait for all stopped, send continue, wait for all started, send continue
                            case RunSynchronization.BreakOnFirstFinished:
                                if (tpm.RunStateChange == RunStateChange.ToRunInitializedFirstTime) {
                                    _runInitialized = AddUniqueToStringArray(_runInitialized, tpm.TileStresstestIndex);
                                    if (_runInitialized.Length == Running) {
                                        _runInitialized = new string[] { };
                                        MasterSideCommunicationHandler.SendContinue();
                                    }
                                } else if (tpm.RunStateChange == RunStateChange.ToRunDoneOnce) {
                                    _runDoneOnce = AddUniqueToStringArray(_runDoneOnce, tpm.TileStresstestIndex);
                                    if (_runDoneOnce.Length == _totalTestCount) {
                                        _runDoneOnce = new string[] { };
                                        //Increment the index here to be able to continue to the next run.
                                        MasterSideCommunicationHandler.SendContinue();
                                    } else if (_runDoneOnce.Length == 1) {
                                        //Break the tests that are still in the previous run
                                        MasterSideCommunicationHandler.SendBreak();
                                    }
                                }
                                break;

                            //Wait for all stopped, send continue, wait for all started, send continue
                            case RunSynchronization.BreakOnLastFinished:
                                if (tpm.RunStateChange == RunStateChange.ToRunInitializedFirstTime) {
                                    _runInitialized = AddUniqueToStringArray(_runInitialized, tpm.TileStresstestIndex);
                                    if (_runInitialized.Length == Running) {
                                        _runInitialized = new string[] { };
                                        MasterSideCommunicationHandler.SendContinue();
                                    }
                                } else if (tpm.RunStateChange == RunStateChange.ToRunDoneOnce) {
                                    _runDoneOnce = AddUniqueToStringArray(_runDoneOnce, tpm.TileStresstestIndex);
                                    if (_runDoneOnce.Length == Running) {
                                        _runDoneOnce = new string[] { };
                                        _breakOnLastReruns = new ConcurrentDictionary<string, RerunCounter>();
                                        MasterSideCommunicationHandler.SendBreak();
                                    }
                                } else if (tpm.RunStateChange == RunStateChange.ToRerunDone) {
                                    if (IncrementReruns(tpm.TileStresstestIndex) == _distributedTest.MaxRerunsBreakOnLast) {
                                        _runDoneOnce = new string[] { };
                                        _breakOnLastReruns = new ConcurrentDictionary<string, RerunCounter>();
                                        MasterSideCommunicationHandler.SendBreak();
                                    }
                                }
                                break;
                        }
                    }
                    //Take merging divided stresstest loads into account, do not put the test progress message here as is.
                    InvokeOnTestProgressMessageReceivedDelayed(originalTileStresstest);

                    if (Cancelled != 0 || Failed != 0) Stop(); //Test is invalid stop the test.
                    if (Finished == _totalTestCount) HandleFinished();
                } catch (Exception ex) {
                    LogWrapper.LogByLevel("Something went wrong when handling test progress in the distributed test core.\n" + ex, LogLevel.Error);
                }
            }
        }
        private int IncrementReruns(string tileStresstestIndex) {
            if (!_breakOnLastReruns.ContainsKey(tileStresstestIndex))
                _breakOnLastReruns.TryAdd(tileStresstestIndex, new RerunCounter());

            return _breakOnLastReruns[tileStresstestIndex].Increment();
        }

        /// <summary>
        ///     Use GetTestProgressMessage(TileStresstest) if you do not need all test progress messages, that is way faster. 
        ///     If you do need them all, put the return value in a new Dictionary, this to avoid recalcullations.
        ///     Key= index of the tile stresstest. Value = pushed progress message from slave.
        ///     Divided progress is combined.
        /// </summary>
        public Dictionary<TileStresstest, TestProgressMessage> GetAllTestProgressMessages() {
            lock (_testProgressMessagesLock) {
                var testProgressMessages = new Dictionary<TileStresstest, TestProgressMessage>(_testProgressMessages.Count);
                foreach (var ts in _testProgressMessages.Keys) {
                    var tpm = GetTestProgressMessage(ts);
                    if (tpm.TileStresstestIndex != null)
                        testProgressMessages.Add(ts, tpm);
                }
                return testProgressMessages;
            }
        }
        /// <summary>
        ///     If the test progress message is not found a new one with TileStresstestIndex == null is returned.
        ///     Divided progress is combined.
        /// </summary>
        /// <param name="tileStresstest"></param>
        /// <returns></returns>
        public TestProgressMessage GetTestProgressMessage(TileStresstest tileStresstest) {
            lock (_testProgressMessagesLock) {
                if (_testProgressMessages.ContainsKey(tileStresstest))
                    //_testProgressMessages[tileStresstest].Values = dictionary of divided stresstest results, for when the same test is divided over 2 or more slaves.
                    return DivideEtImpera.GetMergedTestProgressMessage(tileStresstest, _testProgressMessages[tileStresstest].Values);

                return new TestProgressMessage();
            }
        }
        #endregion

        #region Finished
        /// <summary>
        ///     Stop the distributed test.
        /// </summary>
        public void Stop() { MasterSideCommunicationHandler.StopTest(); }
        private void HandleFinished() {
            if (!_finishedHandled) {
                _finishedHandled = true;

                MasterSideCommunicationHandler.ListeningError -= _masterCommunication_ListeningError;
                MasterSideCommunicationHandler.TestInitialized -= MasterSideCommunicationHandler_TestInitialized;
                MasterSideCommunicationHandler.OnTestProgressMessageReceived -= _masterCommunication_OnTestProgressMessageReceived;

                ObjectRegistrar.Unregister(this);
                InvokeOnFinished();
            }
        }

        public void Dispose() {
            if (!_isDisposed) {
                try {
                    _isDisposed = true;

                    MasterSideCommunicationHandler.ListeningError -= _masterCommunication_ListeningError;
                    MasterSideCommunicationHandler.TestInitialized -= MasterSideCommunicationHandler_TestInitialized;
                    MasterSideCommunicationHandler.OnTestProgressMessageReceived -= _masterCommunication_OnTestProgressMessageReceived;

                    _communicationLock = null;
                    _testProgressMessagesLock = null;

                    _hasResults = false;
                    _usedTileStresstests = null;
                    _totalTestCount = 0;
                    _testProgressMessages = null;
                    _sw = null;

                    _ok = null;
                    _cancelled = null;
                    _failed = null;

                    _runInitialized = null;
                    _runDoneOnce = null;

                    _tmrOnInvokeTestProgressMessageReceivedDelayed = null;
                } catch {
                }

                ObjectRegistrar.Unregister(this);
            }
        }
        #endregion

        #region Other
        private string[] AddUniqueToStringArray(string[] arr, string item) {
            foreach (string s in arr)
                if (s == item)
                    return arr;

            var newArr = new string[arr.Length + 1];
            for (int index = 0; index != arr.Length; index++)
                newArr[index] = arr[index];
            newArr[arr.Length] = item;

            return newArr;
        }
        private void WriteRestConfig() {
            try {
                Converter.ClearWrittenFiles();

                var testConfigCache = new ConverterCollection();
                var distributedTestCache = Converter.AddSubCache(_distributedTest.ToString(), testConfigCache);

                foreach (Tile tile in _distributedTest.Tiles)
                    foreach (TileStresstest tileStresstest in tile)
                        if (tileStresstest.Use)
                            Converter.SetTestConfig(distributedTestCache,
                                                    _distributedTest.RunSynchronization.ToString(),
                                                    "Tile " + (tileStresstest.Parent as Tile).Index + " Stresstest " +
                                                    tileStresstest.Index + " " +
                                                    tileStresstest.BasicTileStresstest.Connection.Label,
                                                    tileStresstest.BasicTileStresstest.Connection,
                                                    tileStresstest.BasicTileStresstest.ConnectionProxy,
                                                    tileStresstest.BasicTileStresstest.Monitors,
                                                    tileStresstest.BasicTileStresstest.Slaves.Length == 0
                                                        ? string.Empty
                                                        : tileStresstest.BasicTileStresstest.Slaves[0].ToString(),
                                                    tileStresstest.AdvancedTileStresstest.Log,
                                                    tileStresstest.AdvancedTileStresstest.LogRuleSet,
                                                    tileStresstest.AdvancedTileStresstest.Concurrencies,
                                                    tileStresstest.AdvancedTileStresstest.Runs,
                                                    tileStresstest.AdvancedTileStresstest.MinimumDelay,
                                                    tileStresstest.AdvancedTileStresstest.MaximumDelay,
                                                    tileStresstest.AdvancedTileStresstest.Shuffle,
                                                    tileStresstest.AdvancedTileStresstest.Distribute,
                                                    tileStresstest.AdvancedTileStresstest.MonitorBefore,
                                                    tileStresstest.AdvancedTileStresstest.MonitorAfter);

                Converter.WriteToFile(testConfigCache, "TestConfig");
            } catch {
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// For break on last max reruns functionality, Initial Value == 0 --> incremented on every run after the first (for a certain tile stresstest)
        /// </summary>
        private class RerunCounter {
            private int _counter;
            public int Counter { get { return _counter; } }
            /// <summary>
            /// Increments and returns the counter thread-safe.
            /// </summary>
            /// <returns></returns>
            public int Increment() { return Interlocked.Increment(ref _counter); }
        }
    }
}