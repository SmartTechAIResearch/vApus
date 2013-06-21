﻿/*
 * Copyright 2010 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using vApus.SolutionTree;
using vApus.Util;

namespace vApus.Stresstest {
    [ContextMenu(new[] { "Activate_Click" }, new[] { "Edit" })]
    [Hotkeys(new[] { "Activate_Click" }, new[] { Keys.Enter })]
    [DisplayName("Connection Proxy Code"), Serializable]
    public class ConnectionProxyCode : BaseItem, ISerializable {
        #region Fields

        private const string DEFAULTCODE =
            @"/*
    ConnectionProxy generated by vApus
    vApus is Copyrighted by Sizing Servers Lab
    University College of West-Flanders, Department GKG
    
    Note: If you want to edit this connection proxy code you need to know what you are doing. You might break your stresstest.
*/

// The following line is used to add references when compiling, you can edit this here or in the references tab page. Please use the 'Browse...' button for dlls that are not in the GAC.
// dllreferences:System.dll;System.Data.dll;vApus.Util.dll;vApus.Stresstest.dll;

#region Preprocessors
    //
    // e.g. #define NOTMUCHUSEDFEATURE
    //
    // Further in code:
    // #if NOTMUCHUSEDFEATURE
    // do stuff...
    // #endif
#endregion // Preprocessors

#region Usings
    using System;
    using System.Data;
    using System.Diagnostics;
    // Contains the StringTree class.
    using vApus.Util;
    
    //
    // Here you can put your own usings under the default ones.
    //
#endregion // Usings

namespace vApus.Stresstest {
    public class ConnectionProxy : IConnectionProxy {
        
        #region Fields
            Stopwatch _stopwatch = new Stopwatch();
            bool _isDisposed;
            // Please do not edit the following lines (RuleSetFields).
            // -- RuleSetFields --
            // -- RuleSetFields --
            
            //
            // Here you can put labels, for instance, for the index values of the String Tree nodes (eg. int ip = 0).
            //
        #endregion // Fields
        
        #region Properties
            public bool IsConnectionOpen {
                get {
                    //
                    // ...
                    //
                }
            }
            public bool IsDisposed {
                get {
                    return _isDisposed;
                }
            }
        #endregion // Properties
        
        public ConnectionProxy() {
            
        }
        
        #region Functions
            public void TestConnection(out string error) {
                error = null;
                try {
                    OpenConnection();
                    if (IsConnectionOpen) {
                        CloseConnection();
                    }
                    else {
                      	error = " + "\"Could not open the connection.\"" + @";
                    }
                }
                catch (Exception ex) {
                    error = ex.ToString();
                }
            }
            public void OpenConnection() {
                if (!IsConnectionOpen) {
                    //
                    // ...
                    //
                }
            }
            public void CloseConnection() {
                if (IsConnectionOpen) {
                    //
                    // ...
                    //
                }
            }
            public void SendAndReceive(StringTree parameterizedLogEntry, out DateTime sentAt, out TimeSpan timeToLastByte, out Exception exception) {
                exception = null;
                
                //
                // parameterizedLogEntry is the log entry parsed to a String Tree using the Log Rule Set.
                // String Tree is a simple class: it can have either a value (string Value { get; }) or childs who are also String Trees (e.g. StringTree foo = parameterizedLogEntry[n] where n is an integer value).
                // You can get the combined value of the childs using the function CombineValues() (not shallow, returns a string), this uses the child delimiters in the rule set to glue these values together. 
                //
                
                //
                // Initiate other stuff here.
                // (eg. Fields or functions from the free coding section.)
                //
                
                if (_isDisposed) {
                    sentAt = DateTime.Now;
                    timeToLastByte = new TimeSpan();
                    return;
                }
                try {
                    sentAt = DateTime.Now;
                    _stopwatch.Start();
                    try {
                        //
                        // Send the request to the server here.
                        //
                    }
                    catch (Exception ex) {
                        exception = ex;
                    }
                    finally {
                        //
                        // Close stuff if needed.
                        //
                    }
                }
                catch {
                    //
                    // Always throw the exception, if any, if 'stuff' cannot be closed.
                    // vApus will handle it as a connection problem.
                    //
                    
                    throw;
                }
                finally {
                    _stopwatch.Stop();
                    timeToLastByte = _stopwatch.Elapsed;
                    _stopwatch.Reset();
                }
            }
            public void Dispose() {
                if (!_isDisposed) {
                    _isDisposed = true;
                    // A call to CloseConnection() is not necessary, if the connection proxy pool is being disposed it will get called.
                    
                    //
                    // ...
                    //
                }
            }
        #endregion // Functions
        
        #region Free Coding
            //
            // Anything you want...
            //
        #endregion // Free Coding
    } // ConnectionProxy
} // vApus.Stresstest";

        private string _code = DEFAULTCODE;

        #endregion

        #region Properties

        //Never use this in a distributed test.
        public ConnectionProxyRuleSet ConnectionProxyRuleSet {
            get { return Parent[0] as ConnectionProxyRuleSet; }
        }

        [SavableCloneable]
        public string Code {
            get { return _code; }
            set {
                if (!string.IsNullOrEmpty(value))
                    _code = value;
            }
        }

        #endregion

        #region Constructor

        public ConnectionProxyCode() {
        }

        /// <summary>
        ///     Only for sending from master to slave.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public ConnectionProxyCode(SerializationInfo info, StreamingContext ctxt) {
            SerializationReader sr;
            using (sr = SerializationReader.GetReader(info)) {
                _code = sr.ReadString();
            }
            sr = null;
            //Not pretty, but helps against mem saturation.
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
                sw.Write(_code);
                sw.AddToInfo(info);
            }
            sw = null;
            //Not pretty, but helps against mem saturation.
            GC.Collect();
        }

        public string BuildConnectionProxyClass(ConnectionProxyRuleSet connectionProxyRuleSet, string connectionString) {
            string[] split = _code.Split(new[] { "// -- RuleSetFields --" }, StringSplitOptions.None);
            split[1] = BuildRuleSetFields(connectionProxyRuleSet, connectionString);

            string connectionProxyClass = split[0].Trim() + "\n" + split[1].Trim() + "\n\n" + split[2].Trim();
            return connectionProxyClass;
        }

        private string BuildRuleSetFields(ConnectionProxyRuleSet connectionProxyRuleSet, string connectionString = "") {
            var sb = new StringBuilder();

            sb.AppendLine("// -- RuleSetFields --");
            List<string> splitInput = string.IsNullOrEmpty(connectionString)
                                          ? null
                                          : new List<string>(
                                                connectionString.Split(new[] { connectionProxyRuleSet.ChildDelimiter },
                                                                       StringSplitOptions.None));

            for (int i = 0; i < connectionProxyRuleSet.Count; i++) {
                var syntaxItem = connectionProxyRuleSet[i] as ConnectionProxySyntaxItem;
                var rule = (syntaxItem.Count != 0 && syntaxItem[0] is Rule) ? syntaxItem[0] as Rule : null;
                Type valueType = (rule == null) ? typeof(string) : Rule.GetType(rule.ValueType);

                string name = syntaxItem.GetType().Name;
                name = name[0].ToString().ToLower() + name.Substring(1);

                bool defaultValueUsed = false;
                string part = splitInput == null || i >= splitInput.Count ? null : splitInput[i];
                if (part == null && syntaxItem.DefaultValue.Length != 0) { //Default to.
                    part = syntaxItem.DefaultValue;
                    defaultValueUsed = true;
                }

                if (part == null) {
                    if (valueType == typeof(string))
                        sb.AppendFormat("{0} _{1}{2} = string.Empty;", valueType, name, i);
                    else
                        sb.AppendFormat("{0} _{1}{2};", valueType, name, i);
                } else {
                    if (valueType == typeof(string))
                        sb.AppendFormat("{0} _{1}{2} = \"{3}\";", valueType, name, i, part);
                    else
                        sb.AppendFormat("{0} _{1}{2} = {3};", valueType, name, i, part.ToLower());
                }
                sb.AppendLine(defaultValueUsed ? " // " + syntaxItem.Label + " [The default value for this syntax item is used if no value is provided in the connection.]" : " // " + syntaxItem.Label);
            }
            sb.AppendLine("// -- RuleSetFields --");
            return sb.ToString();
        }

        public override void Activate() {
            SolutionComponentViewManager.Show(this);
        }

        #endregion
    }
}