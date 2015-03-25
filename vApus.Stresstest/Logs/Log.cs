﻿/*
 * Copyright 2009 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */

using RandomUtils.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Runtime.Serialization;
using System.Windows.Forms;
using vApus.SolutionTree;
using vApus.Util;

namespace vApus.Stresstest {
    /// <summary>
    /// Contains UserActions that contain LogEntries.
    /// </summary>
    [Serializable]
    [ContextMenu(new[] { "Activate_Click", "EditPlainText_Click", "Remove_Click", "Export_Click", "ExportLogAndUsedParameters_Click", "Copy_Click", "Cut_Click", "Duplicate_Click" },
        new[] { "Edit/Import", "Edit Plain Text", "Remove", "Export Data Structure", "Export log and Used Parameter Data Structures", "Copy", "Cut", "Duplicate" })]
    [Hotkeys(new[] { "Activate_Click", "Remove_Click", "Copy_Click", "Cut_Click", "Duplicate_Click" },
        new[] { Keys.Enter, Keys.Delete, (Keys.Control | Keys.C), (Keys.Control | Keys.X), (Keys.Control | Keys.D) })]
    public class Log : LabeledBaseItem, ISerializable {

        /// <summary>
        ///     This event is used in a control, this makes sure that trying to serialize the control where this event is used will not happen.
        ///     This is asynchronously invoked, invoke to the gui where needed.
        /// </summary>
        [field: NonSerialized] //This makes sure that trying to serialize the control where this event is used will not happen.
        internal event EventHandler<LexicalResultsChangedEventArgs> LexicalResultChanged;

        #region Fields
        private static readonly object _lock = new object();

        private LexicalResult _lexicalResult;
        private LogRuleSet _logRuleSet;

        private static Parameters _parameters;

        private int _preferredTokenDelimiterIndex;

        //Capture web traffic settings
        private string[] _allow = new string[] { }, _deny = new string[] { };
        #endregion

        #region Properties
        [SavableCloneable, PropertyControl(1)]
        [DisplayName("Log Rule Set"), Description("You must define a rule set to validate if the log file(s) are correctly formated to be able to stresstest.")]
        public LogRuleSet LogRuleSet {
            get {
                if (Solution.ActiveSolution != null && (_logRuleSet.IsEmpty || _logRuleSet.Parent == null))
                    _logRuleSet = GetNextOrEmptyChild(typeof(LogRuleSet), Solution.ActiveSolution.GetSolutionComponent(typeof(LogRuleSets))) as LogRuleSet;

                return _logRuleSet;
            }
            set {
                if (value == null)
                    return;
                _logRuleSet = value;
            }
        }

        /// <summary>
        ///     Used for getting the right token delimiters, this value is updated if the tokens are manually redetermined.
        ///     Set: if it is outside boundaries this will be corrected by going to the last or first possible index.
        /// </summary>
        [SavableCloneable]
        public int PreferredTokenDelimiterIndex {
            get { return _preferredTokenDelimiterIndex; }
            set {
                if (value < 0)
                    value = LogEntry.MaxTokenDelimiterIndex;
                else if (value > LogEntry.MaxTokenDelimiterIndex)
                    value = 0;

                _preferredTokenDelimiterIndex = value;
            }
        }

        public LexicalResult LexicalResult {
            get { return _lexicalResult; }
        }

        //All the settings for capturing web traffic.
        [SavableCloneable]
        public bool UseAllow { get; set; }
        [SavableCloneable]
        public string[] Allow { get { return _allow; } set { _allow = value; } }
        [SavableCloneable]
        public bool AllowIncludeReferer { get; set; }
        [SavableCloneable]
        public bool UseDeny { get; set; }
        [SavableCloneable]
        public string[] Deny { get { return _deny; } set { _deny = value; } }

        /// <summary>
        /// </summary>
        internal static Parameters Parameters {
            set {
                _parameters = value;
                LogEntry.Parameters = _parameters;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Contains UserActions that contain LogEntries.
        /// </summary>
        public Log() {
            if (Solution.ActiveSolution == null) {
                Solution.ActiveSolutionChanged += Solution_ActiveSolutionChanged;
            } else {
                LogRuleSet = GetNextOrEmptyChild(typeof(LogRuleSet), Solution.ActiveSolution.GetSolutionComponent(typeof(LogRuleSets))) as LogRuleSet;
                _parameters = Solution.ActiveSolution.GetSolutionComponent(typeof(Parameters)) as Parameters;
            }
        }

        /// <summary>
        ///     Only for sending from master to slave. (Synchronization)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public Log(SerializationInfo info, StreamingContext ctxt) {
            SerializationReader sr;
            using (sr = SerializationReader.GetReader(info)) {
                ShowInGui = false;
                Label = sr.ReadString();
                _logRuleSet = sr.ReadObject() as LogRuleSet;
                _preferredTokenDelimiterIndex = sr.ReadInt32();
                _parameters = sr.ReadObject() as Parameters;

                AddRangeWithoutInvokingEvent(sr.ReadCollection<BaseItem>(new List<BaseItem>()));
            }
            sr = null;
            //Not pretty, but helps against mem saturation.
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        #endregion

        #region Functions
        /// <summary>
        ///     Only for sending from master to slave.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            SerializationWriter sw;
            using (sw = SerializationWriter.GetWriter()) {
                sw.Write(Label);
                sw.WriteObject(LogRuleSet);
                sw.Write(_preferredTokenDelimiterIndex);
                sw.WriteObject(_parameters);

                sw.Write(this);
                sw.AddToInfo(info);
            }
            sw = null;
            //Not pretty, but helps against mem saturation.
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void Solution_ActiveSolutionChanged(object sender, ActiveSolutionChangedEventArgs e) {
            Solution.ActiveSolutionChanged -= Solution_ActiveSolutionChanged;
            LogRuleSet = GetNextOrEmptyChild(typeof(LogRuleSet), Solution.ActiveSolution.GetSolutionComponent(typeof(LogRuleSets))) as LogRuleSet;
            Parameters = Solution.ActiveSolution.GetSolutionComponent(typeof(Parameters)) as Parameters;
        }

        /// <summary>
        ///     Get the log entries even if they are in a user action.
        ///     Threadsafe.
        /// </summary>
        /// <returns></returns>
        public LogEntry[] GetAllLogEntries() {
            lock (_lock) {
                int count = GetTotalLogEntryCount();
                var arr = new LogEntry[count];

                int index = 0;
                foreach (UserAction item in this)
                    foreach (LogEntry childItem in item)
                        arr[index++] = childItem;

                return arr;
            }
        }

        public int GetTotalLogEntryCount() {
            int count = 0;
            foreach (UserAction item in this)
                count += item.Count;
            return count;
        }

        private void ExportLogAndUsedParameters_Click(object sender, EventArgs e) {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Zip Files (*.zip) | *.zip";
            sfd.Title = "Export log and used parameters to...";
            sfd.FileName = Label.ReplaceInvalidWindowsFilenameChars('_');
            if (sfd.ShowDialog() == DialogResult.OK) {
                Package package = null;

                try {
                    package = Package.Open(sfd.FileName, FileMode.Create, FileAccess.ReadWrite);

                    var uri = new Uri("/" + Name, UriKind.Relative);
                    var part = package.CreatePart(uri, string.Empty, CompressionOption.Maximum);
                    using (var sw = new StreamWriter(part.GetStream(FileMode.Create, FileAccess.Write)))
                        GetXmlStructure().Save(sw);

                    //Get the parameters used in the log
                    string begin, end;
                    bool logEntryContainsTokens;
                    GetParameterTokenDelimiters(out begin, out end, out logEntryContainsTokens, false);

                    var usedParameters = new List<BaseParameter>();
                    var allParameterTokens = GetParameterTokens(begin, end);

                    foreach (UserAction userAction in this)
                        foreach (LogEntry logEntry in userAction)
                            foreach (string token in allParameterTokens.Keys) {
                                var parameter = allParameterTokens[token];
                                if (!usedParameters.Contains(parameter) && logEntry.LogEntryString.Contains(token))
                                    usedParameters.Add(allParameterTokens[token]);
                            }

                    //Save thenm to the package.
                    foreach (var parameter in usedParameters) {
                        uri = new Uri("/" + parameter.Name.Replace(' ', '_') + "_0" + parameter.Index, UriKind.Relative);
                        part = package.CreatePart(uri, string.Empty, CompressionOption.Maximum);
                        using (var sw = new StreamWriter(part.GetStream(FileMode.Create, FileAccess.Write)))
                            parameter.GetXmlStructure().Save(sw);
                    }

                    package.Flush();
                } catch (Exception ex) {
                    Loggers.Log(Level.Error, "Failed to export the log + parameters", ex, new object[] { sender, e });
                }

                try {
                    if (package != null)
                        package.Close();
                } catch {
                    //Ignrore. Not important.
                }
            }
        }
        private void EditPlainText_Click(object sender, EventArgs e) { SolutionComponentViewManager.Show(this, typeof(PlaintTextLogView)); }

        /// <summary>
        ///     This will apply the ruleset (lexing).
        ///     The lexed log entry will be filled in for the log entries.
        /// </summary>
        public void ApplyLogRuleSet() {
            var logEntriesWithErrors = new List<LogEntry>();
            foreach (var logEntry in GetAllLogEntries()) {
                try {
                    logEntry.ApplyLogRuleSet(LogRuleSet);
                    if (logEntry.LexicalResult == LexicalResult.Error)
                        logEntriesWithErrors.Add(logEntry);
                } catch (Exception ex) {
                    Loggers.Log(Level.Error, "Failed to apply log ruleset.", ex);
                }
            }

            _lexicalResult = (logEntriesWithErrors.Count == 0) ? LexicalResult.OK : LexicalResult.Error;
            var logEntriesWithErrorsArr = logEntriesWithErrors.ToArray();

            if (_lexicalResult == LexicalResult.OK) {
#if EnableBetaFeature
                bool successfullyParallized = SetParallelExecutions();
#else
            //#warning Parallel executions temp not available
            bool successfullyParallized = true;
#endif

                if (!successfullyParallized) {
                    string message = this + ": Could not determine the begin- and end timestamps for one or more log entries in the different user actions, are they correctly formatted?";
                    Loggers.Log(Level.Error, message);
                }
            }

            if (LexicalResultChanged != null)
                LexicalResultChanged(this, new LexicalResultsChangedEventArgs(logEntriesWithErrorsArr));
        }

        /// <summary>
        /// Webpage --> all statics in parrallel after analyzing the dom. Offset based on  the begin request time. In the acual test this will be the connect time. --> needs to be fixed in the connection proxy.
        /// </summary>
        /// <returns></returns>
        private bool SetParallelExecutions() {
            int connectedIndex = (int)LogRuleSet.ClientConnectedTimestampIndex - 1;//One-based
            if (connectedIndex == -1) return true;

            int sentIndex = (int)LogRuleSet.SentRequestTimestampIndex - 1;//One-based
            if (sentIndex == -1) return true;

            foreach (UserAction ua in this) {
                DateTime firstConnected = DateTime.MinValue;
                foreach (LogEntry le in ua) {
                    DateTime connected, sent;
                    if (!DateTime.TryParse(le.LexedLogEntry[connectedIndex].Value, out connected)) return false;
                    if (!DateTime.TryParse(le.LexedLogEntry[sentIndex].Value, out sent)) return false;

                    if (firstConnected == DateTime.MinValue) {
                        firstConnected = connected;
                    } else {
                        le.ExecuteInParallel = true;

                        le.FirstEntryConnectedToThisConnectedInMs = (int)(connected - firstConnected).TotalMilliseconds;
                        le.ConnectedToSentRequestOffsetInMs = (int)(sent - connected).TotalMilliseconds;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="beginTokenDelimiter"></param>
        /// <param name="endTokenDelimiter"></param>
        /// <param name="logEntryContainsTokens">True if one of the delimiters is in a log entry string.</param>
        public void GetParameterTokenDelimiters(out string beginTokenDelimiter, out string endTokenDelimiter, out bool logEntryContainsTokens, bool autoNextOnLogEntryContainsTokens) {
            beginTokenDelimiter = string.Empty;
            endTokenDelimiter = string.Empty;
            logEntryContainsTokens = false;

            string b, e;
            bool bln;
            int tokenIndex = -1;

            foreach (LogEntry logEntry in GetAllLogEntries()) {
                tokenIndex = logEntry.GetParameterTokenDelimiters(autoNextOnLogEntryContainsTokens, out b, out e, out bln, _preferredTokenDelimiterIndex);

                if (tokenIndex >= _preferredTokenDelimiterIndex) {
                    beginTokenDelimiter = b;
                    endTokenDelimiter = e;
                    if (bln) logEntryContainsTokens = true;

                    _preferredTokenDelimiterIndex = tokenIndex;
                }
            }
        }
        private Dictionary<string, BaseParameter> GetParameterTokens(string beginTokenDelimiter, string endTokenDelimiter) {

            var scopeIdentifiers = new[] { ASTNode.LOG_PARAMETER_SCOPE, ASTNode.USER_ACTION_PARAMETER_SCOPE, ASTNode.LOG_ENTRY_PARAMETER_SCOPE, ASTNode.LEAF_NODE_PARAMETER_SCOPE, ASTNode.ALWAYS_PARAMETER_SCOPE };

            var parameterTokens = new Dictionary<string, BaseParameter>();

            int i;
            foreach (string scopeIdentifier in scopeIdentifiers) {
                i = 1;
                foreach (BaseParameter parameter in _parameters.GetAllParameters())
                    parameterTokens.Add(beginTokenDelimiter + scopeIdentifier + (i++) + endTokenDelimiter, parameter);
            }

            return parameterTokens;
        }

        /// <summary>
        ///     Get a list of string trees, these are used in the connection proxy code.
        /// </summary>
        /// <returns></returns>
        public StringTree[] GetParameterizedStructure(out bool hasParameters) {
            var parameterizedStructure = new List<StringTree>(Count);

            string b, e;
            bool logEntryContainsTokens;
            GetParameterTokenDelimiters(out b, out e, out logEntryContainsTokens, false);

            HashSet<BaseParameter> chosenNextValueParametersForLScope = logEntryContainsTokens ? new HashSet<BaseParameter>() : null;

            Dictionary<string, BaseParameter> parameterTokens = logEntryContainsTokens ? GetParameterTokens(b, e) : null;

            foreach (UserAction userAction in this)
                parameterizedStructure.AddRange(userAction.GetParameterizedStructure(parameterTokens, chosenNextValueParametersForLScope));

            hasParameters = parameterTokens != null;

            return parameterizedStructure.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="oldAndNewIndices"></param>
        /// <param name="oldAndNewBeginTokenDelimiter"></param>
        /// <param name="oldAndNewEndTokenDelimiter"></param>
        public void SynchronizeTokens(Dictionary<BaseParameter, KeyValuePair<int, int>> oldAndNewIndices, KeyValuePair<string, string> oldAndNewBeginTokenDelimiter,
            KeyValuePair<string, string> oldAndNewEndTokenDelimiter) {
            //Synchronize only if needed.
            if (oldAndNewIndices.Count == 0)
                return;

            var oldAndNewTokens = new Dictionary<string, string>();

            var scopeIdentifiers = new[] { ASTNode.LOG_PARAMETER_SCOPE, ASTNode.USER_ACTION_PARAMETER_SCOPE, 
                ASTNode.LOG_ENTRY_PARAMETER_SCOPE, ASTNode.LEAF_NODE_PARAMETER_SCOPE, ASTNode.ALWAYS_PARAMETER_SCOPE };

            foreach (string scopeIdentifier in scopeIdentifiers)
                foreach (BaseParameter parameter in oldAndNewIndices.Keys) {
                    KeyValuePair<int, int> kvp = oldAndNewIndices[parameter];
                    string oldToken = oldAndNewBeginTokenDelimiter.Key + scopeIdentifier + kvp.Key + oldAndNewEndTokenDelimiter.Key;
                    string newToken = oldAndNewBeginTokenDelimiter.Value + scopeIdentifier + kvp.Value + oldAndNewEndTokenDelimiter.Value;

                    oldAndNewTokens.Add(oldToken, newToken);
                }

            foreach (LogEntry entry in GetAllLogEntries())
                foreach (string oldToken in oldAndNewTokens.Keys)
                    entry.LogEntryString = entry.LogEntryString.Replace(oldToken, oldAndNewTokens[oldToken]);

            InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cloneChildren"></param>
        /// <param name="applyRuleSet">Not needed in a distributed test</param>
        /// <param name="cloneLabelAndLogEntryStringByRef">Set to true to leverage memory usage, should only be used in a distributed test otherwise strange things will happen.</param>
        /// <param name="copyLogEntriesAsImported">Not needed in a distributed test</param>
        /// <returns></returns>
        public Log Clone(bool cloneChildren = true, bool applyRuleSet = true, bool cloneLabelAndLogEntryStringByRef = false, bool copyLogEntriesAsImported = true) {
            var log = new Log();
            log.Parent = Parent;
            log.Label = Label;
            log._logRuleSet = LogRuleSet;
            log._lexicalResult = _lexicalResult;
            log.PreferredTokenDelimiterIndex = _preferredTokenDelimiterIndex;

            if (cloneChildren)
                foreach (UserAction userAction in this)
                    log.AddWithoutInvokingEvent(userAction.Clone(log._logRuleSet, applyRuleSet, cloneLabelAndLogEntryStringByRef, copyLogEntriesAsImported, true));

            return log;
        }

        public override BaseSolutionComponentView Activate() {
            if ((Count > 499 || GetTotalLogEntryCount() > 4999) &&
                MessageBox.Show("This is a large log! Do you want to use the plain text editor?\nYou will loose most functionality, but vApus will stay responsive and memory usage within boundaries.", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                return SolutionComponentViewManager.Show(this, typeof(PlaintTextLogView));
            }
            return SolutionComponentViewManager.Show(this);
        }

        public new void Dispose() {
            _parameters = null;
            base.Dispose();
        }
        #endregion

        public class LexicalResultsChangedEventArgs : EventArgs {
            public LogEntry[] LogEntriesWithErrors { get; private set; }
            public LexicalResultsChangedEventArgs(LogEntry[] logEntriesWithErrors) {
                LogEntriesWithErrors = logEntriesWithErrors;
            }
        }
    }
}