﻿using RandomUtils.Log;
/*
 * Copyright 2009 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using vApus.SolutionTree;
using vApus.Util;

namespace vApus.Stresstest {
    /// <summary>
    /// Contains log entries.
    /// </summary>
    [DisplayName("User Action"), Serializable]
    public class UserAction : LabeledBaseItem, ISerializable {

        #region Fields
        private int _occurance = 1;
        private bool _useDelay = true;
        [field: NonSerialized]
        private List<string> _logEntryStringsAsImported = new List<string>();
        //These indices are stored here, this must be updated if something happens to a user action in the log.
        private List<int> _linkedToUserActionIndices = new List<int>();
        private int _linkColorRGB = -1;
        #endregion

        #region Properties
        [ReadOnly(true)]
        [SavableCloneable]
        [Description("How many times this user action occures in the log. Action and Log Entry Distribution in the stresstest determines how this value will be used.")]
        public int Occurance {
            get { return _occurance; }
            set {
                if (_occurance < 0)
                    throw new ArgumentOutOfRangeException("occurance");
                _occurance = value;
            }
        }

        [ReadOnly(true)]
        [SavableCloneable]
        [Description("To pin this user action in place.")]
        public bool Pinned { get; set; }

        [ReadOnly(true)]
        [SavableCloneable]
        [Description("When true the determined delay (stresstest properties) will take place after this user action."), DisplayName("Use Delay")]
        public bool UseDelay {
            get { return _useDelay; }
            set { _useDelay = value; }
        }

        [ReadOnly(true)]
        [SavableCloneable]
        [DisplayName("Log Entry Strings as Imported")]
        public List<string> LogEntryStringsAsImported {
            get { return _logEntryStringsAsImported; }
            set { _logEntryStringsAsImported = value; }
        }

        /// <summary>
        /// One-based indices
        /// </summary>
        [ReadOnly(true)]
        [SavableCloneable]
        public List<int> LinkedToUserActionIndices {
            get { return _linkedToUserActionIndices; }
            set { _linkedToUserActionIndices = value; }
        }
        public UserAction[] LinkedToUserActions {
            get {
                var l = new List<UserAction>(_linkedToUserActionIndices.Count);
                try {
                    var log = this.Parent as Log;

                    foreach (int i in _linkedToUserActionIndices)
                        l.Add(log[i - 1] as UserAction);

                } catch (Exception ex) {
                    Loggers.Log(Level.Warning, "Failed linking user actions", ex);
                }
                return l.ToArray();
            }
        }

        [ReadOnly(true)]
        [SavableCloneable]
        public int LinkColorRGB {
            get { return _linkColorRGB; }
            set { _linkColorRGB = value; }
        }
        public Color LinkColor {
            get {
                if (_linkColorRGB != -1) {
                    int red = _linkColorRGB >> 16;
                    int green = (_linkColorRGB >> 8) & 0x00FF;
                    int blue = _linkColorRGB & 0x0000FF;
                    return Color.FromArgb(red, green, blue);
                }
                return Color.Transparent;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Contains log entries.
        /// </summary>
        public UserAction() {
            ShowInGui = false;
        }
        /// <summary>
        /// Contains log entries.
        /// </summary>
        /// <param name="label"></param>
        public UserAction(string label)
            : this() {
            Label = label;
        }
        public UserAction(SerializationInfo info, StreamingContext ctxt) {
            SerializationReader sr;
            using (sr = SerializationReader.GetReader(info)) {
                ShowInGui = false;
                Label = sr.ReadString();
                _occurance = sr.ReadInt32();
                Pinned = sr.ReadBoolean();
                _useDelay = sr.ReadBoolean();
                _linkedToUserActionIndices = sr.ReadCollection<int>(_linkedToUserActionIndices) as List<int>;

                AddRangeWithoutInvokingEvent(sr.ReadCollection<BaseItem>(new List<BaseItem>()));
            }
            sr = null;
        }
        #endregion

        #region Functions
        /// <summary>
        /// </summary>
        /// <param name="beginTokenDelimiter">Needed to dermine parameter tokens</param>
        /// <param name="endTokenDelimiter">Needed to dermine parameter tokens</param>
        /// <param name="containsTokens">Does the log contain the tokens? If not getting the structure can be simpler/faster.</param>
        /// <param name="chosenNextValueParametersForLScope">Can be an empty hash set but may not be null, used to store all these values for the right scope.</param>
        /// <returns></returns>
        internal StringTree[] GetParameterizedStructure(Dictionary<string, BaseParameter> parameterTokens, HashSet<BaseParameter> chosenNextValueParametersForLScope) {
            var parameterizedStructure = new StringTree[Count];

            HashSet<BaseParameter> chosenNextValueParametersForUAScope = parameterTokens == null ? null : new HashSet<BaseParameter>();

            if (parameterTokens == null && Count > 1)
                Parallel.For(0, parameterizedStructure.Length, (i) => {
                    parameterizedStructure[i] = (this[i] as LogEntry).GetParameterizedStructure(parameterTokens, chosenNextValueParametersForLScope, chosenNextValueParametersForUAScope);
                });
            else
                for (int i = 0; i != parameterizedStructure.Length; i++)
                    parameterizedStructure[i] = (this[i] as LogEntry).GetParameterizedStructure(parameterTokens, chosenNextValueParametersForLScope, chosenNextValueParametersForUAScope);
            return parameterizedStructure;
        }

        public void AddToLink(UserAction userAction, int[] linkColorToChooseFrom, bool invokeSolutionComponentChanched = true) {
            var log = this.Parent as Log;

            bool canDetermineColor = _linkedToUserActionIndices.Count == 0;

            //Add linked user actions to this link, because indices can change, re-add the already linked user actions
            var toLink = new List<UserAction>();
            toLink.AddRange(LinkedToUserActions);
            toLink.Add(userAction);
            toLink.AddRange(userAction.LinkedToUserActions);

            log.RemoveRangeWithoutInvokingEvent(toLink);

            _linkedToUserActionIndices.Clear();
            userAction.LinkedToUserActionIndices.Clear();

            //Determine the link color.
            if (canDetermineColor) {
                var exclude = new List<int>();
                foreach (UserAction ua in log)
                    if (ua != this && ua.LinkedToUserActionIndices.Count != 0)
                        exclude.Add(ua.LinkColorRGB);

                foreach (int c in linkColorToChooseFrom)
                    if (!exclude.Contains(c)) {
                        _linkColorRGB = c;
                        break;
                    }
            }

            //Re-add the useractions the log.
            int index = this.Index;
            if (_linkedToUserActionIndices.Count != 0)
                index = _linkedToUserActionIndices[_linkedToUserActionIndices.Count - 1];

            foreach (var ua in toLink) {
                if (index < log.Count)
                    log.InsertWithoutInvokingEvent(index, ua);
                else
                    log.AddWithoutInvokingEvent(ua);
                ua.Pinned = Pinned;
                _linkedToUserActionIndices.Add(ua.Index);

                ua.LinkColorRGB = _linkColorRGB;
                ++index;
            }

            if (invokeSolutionComponentChanched)
                log.InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
        }
        public void RemoveFromLink(UserAction userAction) {
            if (_linkedToUserActionIndices.Remove(userAction.Index)) {
                userAction.LinkColorRGB = -1;
                //Put the unlinked useraction behind this linked group if need be.
                if (_linkedToUserActionIndices.Count != 0) {
                    var log = this.Parent as Log;

                    var arr = LinkedToUserActions;

                    log.RemoveWithoutInvokingEvent(userAction);

                    _linkedToUserActionIndices.Clear();
                    foreach (var ua in arr)
                        _linkedToUserActionIndices.Add(ua.Index);

                    int lastIndex = _linkedToUserActionIndices[_linkedToUserActionIndices.Count - 1];
                    if (lastIndex < log.Count)
                        log.InsertWithoutInvokingEvent(lastIndex, userAction);
                    else
                        log.AddWithoutInvokingEvent(userAction);
                }

                InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
            }
        }
        public void MergeLinked() {
            var log = this.Parent as Log;
            var l = new List<int>(_linkedToUserActionIndices.Count + 1);
            l.AddRange(_linkedToUserActionIndices);
            l.Add(this.Index);
            l.Sort();

            var toMerge = new List<UserAction>(l.Count);
            foreach (int i in l) toMerge.Add(log[i - 1] as UserAction);

            var merged = toMerge[0];
            for (int i = 1; i != toMerge.Count; i++) {
                var ua = toMerge[i] as UserAction;
                merged.AddRangeWithoutInvokingEvent(ua);
                merged.LogEntryStringsAsImported.AddRange(ua.LogEntryStringsAsImported);
                log.RemoveWithoutInvokingEvent(ua);
            }

            //Update the linked indices for the other user actions.
            for (int i = merged.Index; i != log.Count; i++) {
                var ua = log[i] as UserAction;
                var linkedIndices = ua.LinkedToUserActionIndices.ToArray();
                for (int j = 0; j != linkedIndices.Length; j++)
                    ua.LinkedToUserActionIndices[j] = linkedIndices[j] - merged.LinkedToUserActionIndices.Count;
            }

            merged.LinkedToUserActionIndices.Clear();
            log.InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
        }
        /// <summary>
        /// This user action will find its own parent log, more convinient but not so fast.
        /// </summary>
        /// <param name="linkUserAction"></param>
        /// <returns></returns>
        public bool IsLinked(out UserAction linkUserAction) {
            return IsLinked(Parent as Log, out linkUserAction);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log">The log where this user action resides in.</param>
        /// <param name="linkUserAction"></param>
        /// <returns></returns>
        public bool IsLinked(Log log, out UserAction linkUserAction) {
            linkUserAction = null;
            if (LinkedToUserActionIndices.Count == 0) {
                int index = Index;
                for (int i = index - 2; i != -1; i--) {
                    var ua = log[i] as UserAction;
                    if (ua.LinkedToUserActionIndices.Contains(index)) {
                        linkUserAction = ua;
                        return true;
                    }
                }
            } else {
                linkUserAction = this;
                return true;
            }
            return false;
        }
        public void Split() {
            var log = this.Parent as Log;
            int index = this.Index - 1;
            var l = new List<UserAction>(this.Count);
            int i = 0;
            foreach (LogEntry logEntry in this) {
                var ua = new UserAction(logEntry.LogEntryString.Length < 101 ? logEntry.LogEntryString : logEntry.LogEntryString.Substring(0, 100) + "...");
                ua.AddWithoutInvokingEvent(logEntry);

                if (i < this.LogEntryStringsAsImported.Count)
                    ua.LogEntryStringsAsImported.Add(this.LogEntryStringsAsImported[i]);

                l.Add(ua);
                ++i;
            }

            //Update the linked indices for the other user actions.
            int add = l.Count - 1;
            for (int j = index + 1; j != log.Count; j++) {
                var ua = log[j] as UserAction;
                var linkedIndices = ua.LinkedToUserActionIndices.ToArray();
                for (int k = 0; k != linkedIndices.Length; k++)
                    ua.LinkedToUserActionIndices[k] = linkedIndices[k] + add;
            }

            log.InserRangeWithoutInvokingEvent(index, l);
            log.RemoveWithoutInvokingEvent(this);

            log.InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logRuleSet"></param>
        /// <param name="applyRuleSet">Not needed in a distributed test</param>
        /// <param name="cloneLabelAndLogEntryStringByRef">Set to true to leverage memory usage, should only be used in a distributed test otherwise strange things will happen.</param>
        /// <param name="copyLogEntryAsImported">Not needed in a distributed test</param>
        /// <param name="copyLinkedUserActionIndices">Needed in a distributed test</param>
        /// <returns></returns>
        public UserAction Clone(LogRuleSet logRuleSet, bool applyRuleSet, bool cloneLabelAndLogEntryStringByRef, bool copyLogEntryAsImported, bool copyLinkedUserActionIndices) {
            UserAction userAction = new UserAction();
            if (cloneLabelAndLogEntryStringByRef)
                SetLogEntryStringByRef(userAction, ref _label);
            else
                userAction.Label = _label;

            userAction.SetParent(Parent);
            userAction.Occurance = _occurance;
            userAction.Pinned = Pinned;
            userAction.UseDelay = _useDelay;

            if (copyLogEntryAsImported) {
                var arr = new string[_logEntryStringsAsImported.Count];
                _logEntryStringsAsImported.CopyTo(arr);
                userAction.LogEntryStringsAsImported = new List<string>(arr);
            }

            if (copyLinkedUserActionIndices) {
                foreach (int i in _linkedToUserActionIndices) userAction._linkedToUserActionIndices.Add(i);
                userAction._linkColorRGB = _linkColorRGB;
            }

            foreach (LogEntry logEntry in this)
                userAction.AddWithoutInvokingEvent(logEntry.Clone(logRuleSet, applyRuleSet, cloneLabelAndLogEntryStringByRef));

            return userAction;
        }
        private void SetLogEntryStringByRef(UserAction userAction, ref string label) {
            userAction._label = label;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            SerializationWriter sw;
            using (sw = SerializationWriter.GetWriter()) {
                sw.Write(Label);
                sw.Write(_occurance);
                sw.Write(Pinned);
                sw.Write(_useDelay);
                sw.Write(_linkedToUserActionIndices);

                sw.Write(this);
                sw.AddToInfo(info);
            }
            sw = null;
        }
        #endregion
    }
}
