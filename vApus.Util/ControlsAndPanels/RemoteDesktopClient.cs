﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */

using AxMSTSCLib;
using MSTSCLib;
using System;
using System.Drawing;
using System.Windows.Forms;
using IMsTscAxEvents_OnFatalErrorEventHandler = AxMSTSCLib.IMsTscAxEvents_OnFatalErrorEventHandler;
using IMsTscAxEvents_OnLogonErrorEventHandler = AxMSTSCLib.IMsTscAxEvents_OnLogonErrorEventHandler;

namespace vApus.Util {
    public partial class RemoteDesktopClient : UserControl {
        public RemoteDesktopClient() {
            InitializeComponent();
        }

        #region Functions

        public void ShowRemoteDesktop(string hostName, string ip, string userName, string password, string domain) {
            TabPage tp = DoesRdcExist(ip);
            AxMsTscAxNotSafeForScripting rdc = null;
            if (tp == null) {
                tp = new TabPage(hostName);
                tp.Padding = new Padding(0);
                rdc = new AxMsTscAxNotSafeForScripting();
                rdc.OnLogonError += new IMsTscAxEvents_OnLogonErrorEventHandler(rdc_OnLogonError);
                rdc.OnFatalError += new IMsTscAxEvents_OnFatalErrorEventHandler(rdc_OnFatalError);

                rdc.Dock = DockStyle.Fill;
                tp.Controls.Add(rdc);
                tc.TabPages.Add(tp);
            } else {
                rdc = tp.Controls[0] as AxMsTscAxNotSafeForScripting;
            }

            try {
                if (rdc.Connected.ToString() != "1") {
                    rdc.Server = ip;
                    rdc.Domain = domain;
                    rdc.UserName = userName;

                    rdc.DesktopWidth = 1280;
                    rdc.DesktopHeight = 800;

                    var comObject = (IMsRdpClient)rdc.GetOcx();
                    comObject.AdvancedSettings2.ClearTextPassword = password;

                    rdc.Connect();
                }
            } catch {
                tp.Text += " - Connection Failed";
            }

            tp.Select();
        }

        private void rdc_OnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e) {
            var rdc = sender as AxMsTscAxNotSafeForScripting;
            var tp = rdc.Parent as TabPage;
            tp.Text += " - Connection Failed";

            if (RdpException != null)
                RdpException(this, new RdpExceptionEventArgs(rdc.Server, e.errorCode));
        }

        private void rdc_OnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e) {
            var rdc = sender as AxMsTscAxNotSafeForScripting;
            var tp = rdc.Parent as TabPage;
            tp.Text += " - Connection Failed";

            if (RdpException != null)
                RdpException(this, new RdpExceptionEventArgs(rdc.Server, e.lError));
        }

        public TabPage DoesRdcExist(string ip) {
            foreach (TabPage tp in tc.TabPages) {
                var rdp = tp.Controls[0] as AxMsTscAxNotSafeForScripting;
                if (rdp.Server == ip)
                    return tp;
            }
            return null;
        }

        public void ClearRemoteDesktops() {
            try {
                foreach (TabPage tp in tc.TabPages) {
                    var rdp = tp.Controls[0] as AxMsTscAxNotSafeForScripting;
                    try {
                        if (rdp.Connected.ToString() == "1")
                            rdp.Disconnect();
                    } catch {
                    }
                }
                tc.TabPages.Clear();
            } catch {
            }
        }

        private void tc_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                TabPage tp = GetTabPage(e.Location);
                if (tp != null) {
                    RemoveRemoteDesktop(tp);

                    if (tc.TabPages.Count == 0 && AllConnectionsClosed != null)
                        AllConnectionsClosed(this, null);
                }
            }
        }

        private TabPage GetTabPage(Point mouseLocation) {
            for (int i = 0; i != tc.Controls.Count; i++)
                if (tc.GetTabRect(i).Contains(mouseLocation))
                    return tc.TabPages[i];
            return null;
        }

        private void RemoveRemoteDesktop(TabPage tabPage) {
            var rdp = tabPage.Controls[0] as AxMsTscAxNotSafeForScripting;
            try {
                if (rdp.Connected.ToString() == "1")
                    rdp.Disconnect();
            } catch {
            }
            tc.TabPages.Remove(tabPage);
        }

        #endregion

        public event EventHandler<RdpExceptionEventArgs> RdpException;
        public event EventHandler AllConnectionsClosed;

        public class RdpExceptionEventArgs : EventArgs {
            public readonly int ErrorCode;
            public readonly string IP;

            public RdpExceptionEventArgs(string ip, int errorCode) {
                IP = ip;
                ErrorCode = errorCode;
            }
        }
    }
}