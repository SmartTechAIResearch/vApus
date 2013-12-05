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
using System.Windows.Forms;
using vApus.SolutionTree;
using vApus.Util;

namespace vApus.Stresstest {
    [ContextMenu(new[] { "Add_Click", "Import_Click", "Clear_Click", "Paste_Click" },
        new[] { "Add Text Parameter", "Import Parameter(s)", "Clear", "Paste" })]
    [Hotkeys(new[] { "Add_Click", "Paste_Click" }, new[] { Keys.Insert, (Keys.Control | Keys.V) })]
    [DisplayName("Text Parameters")]
    [Serializable]
    public class TextParameters : BaseItem, ISerializable {
        public TextParameters() { }
        public TextParameters(SerializationInfo info, StreamingContext ctxt) {
            SerializationReader sr;
            using (sr = SerializationReader.GetReader(info)) {
                ShowInGui = false;
                AddRangeWithoutInvokingEvent(sr.ReadCollection<BaseItem>(new List<BaseItem>()));
            }
            sr = null;
        }

        private void Add_Click(object sender, EventArgs e) { Add(new TextParameter()); }
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            SerializationWriter sw;
            using (sw = SerializationWriter.GetWriter()) {
                sw.Write(this);
                sw.AddToInfo(info);
            }
            sw = null;
        }
    }
}