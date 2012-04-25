﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace vApus.Util
{
    public partial class DisableFirewallAutoUpdatePanel : Panel
    {
        public enum Status
        {
            AllDisabled = 0,
            WindowsFirewallEnabled = 1,
            WindowsAutoUpdateEnabled = 2,
            AllEnabled = 3
        }
        private Status _status;
        private delegate void DisableThemDel();
        private DisableThemDel _disableThemCallback;
        private ActiveObject _activeObject = new ActiveObject();

        public Status __Status
        {
            get { return _status; }
        }

        public DisableFirewallAutoUpdatePanel()
        {
            InitializeComponent();
            _disableThemCallback = DisableThemCallback;
            _activeObject.OnResult += new EventHandler<ActiveObject.OnResultEventArgs>(_activeObject_OnResult);
            this.HandleCreated += new EventHandler(DisableFirewallAutoUpdatePanel_HandleCreated);
        }

        private void DisableFirewallAutoUpdatePanel_HandleCreated(object sender, EventArgs e)
        {
            CheckStatus();
        }
        public Status CheckStatus()
        {
            _status = Status.AllDisabled;

            EvaluateValue((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\SharedAccess\Parameters\FirewallPolicy\DomainProfile", "EnableFirewall", 0), 0, Status.WindowsFirewallEnabled);
            EvaluateValue((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\SharedAccess\Parameters\FirewallPolicy\PublicProfile", "EnableFirewall", 0), 0, Status.WindowsFirewallEnabled);
            EvaluateValue((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\SharedAccess\Parameters\FirewallPolicy\StandardProfile", "EnableFirewall", 0), 0, Status.WindowsFirewallEnabled);

            EvaluateValue((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", "AUOptions", 0), 0, Status.WindowsAutoUpdateEnabled);
            EvaluateValue((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", "IncludeRecommendedUpdates", 0), 0, Status.WindowsAutoUpdateEnabled);
            EvaluateValue((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", "ElevateNonAdmins", 1), 1, Status.WindowsAutoUpdateEnabled);

            if (Handle != null)
                SetGui();
            return _status;
        }
        private void EvaluateValue(int value, int validValue, Status append)
        {
            if (value != validValue)
                _status |= append;
        }
        private void SetGui()
        {
            kvpFirewall.BackColor = kvpWindowsAutoUpdate.BackColor = Color.LightGreen;
            kvpFirewall.Value = kvpWindowsAutoUpdate.Value = "Off";
            btnDisableThem.Enabled = false;

            switch (_status)
            {
                case Status.WindowsFirewallEnabled:
                    kvpFirewall.BackColor = Color.Orange;
                    kvpFirewall.Value = "On";
                    btnDisableThem.Enabled = true;
                    break;
                case Status.WindowsAutoUpdateEnabled:
                    kvpWindowsAutoUpdate.BackColor = Color.Orange;
                    kvpWindowsAutoUpdate.Value = "On";
                    btnDisableThem.Enabled = true;
                    break;
                case Status.AllEnabled:
                    kvpFirewall.BackColor = kvpWindowsAutoUpdate.BackColor = Color.Orange;
                    kvpFirewall.Value = kvpWindowsAutoUpdate.Value = "On";
                    btnDisableThem.Enabled = true;
                    break;
            }
        }
        private void btnDisableThem_Click(object sender, EventArgs e)
        {
            btnDisableThem.Enabled = false;
            btnDisableThem.Text = "Wait...";
            _activeObject.Send(_disableThemCallback);
        }
        private void DisableThemCallback()
        {
            //Disabling Windows Firewall
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\SharedAccess\Parameters\FirewallPolicy\DomainProfile", "EnableFirewall", 0, RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\SharedAccess\Parameters\FirewallPolicy\PublicProfile", "EnableFirewall", 0, RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\SharedAccess\Parameters\FirewallPolicy\StandardProfile", "EnableFirewall", 0, RegistryValueKind.DWord);

            //Restarting the process
            StartProcess("NET", "STOP MpsSvc");
            StartProcess("NET", "START MpsSvc");

            //Disabling Auto Update
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", "AUOptions", 0, RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", "IncludeRecommendedUpdates", 0, RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", "ElevateNonAdmins", 1, RegistryValueKind.DWord);

            //Restarting the process
            StartProcess("NET", "STOP wuauserv");
            StartProcess("NET", "START wuauserv");
        }
        private void _activeObject_OnResult(object sender, ActiveObject.OnResultEventArgs e)
        {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate 
            {
                btnDisableThem.Text = "Disable Them";
                CheckStatus();
            });
        }
        private void StartProcess(string process, string arguments)
        {
            Process p = null;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(process, arguments);
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p = Process.Start(startInfo);
                p.WaitForExit();
            }
            catch { }
            if (p != null)
                try { p.Dispose(); }
                catch { }
            p = null;
        }
        public override string ToString()
        {
            return "Windows Firewall / Auto Update";
        }
    }
}
