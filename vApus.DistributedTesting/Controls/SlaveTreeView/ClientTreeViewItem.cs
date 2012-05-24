﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using vApus.SolutionTree;
using vApus.Util;

namespace vApus.DistributedTesting
{
    [ToolboxItem(false)]
    public partial class ClientTreeViewItem : UserControl, ITreeViewItem
    {
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int LockWindowUpdate(int hWnd);

        #region Events
        /// <summary>
        /// Call unfocus for the other items in the panel.
        /// </summary>
        public event EventHandler AfterSelect;
        public event EventHandler DuplicateClicked;
        public event EventHandler DeleteClicked;
        public event EventHandler HostNameAndIPSet;
        #endregion

        #region Fields
        private Client _client = new Client();
        /// <summary>
        /// Check if the ctrl key is pressed.
        /// </summary>
        private bool _ctrl;

        /// <summary>
        /// To set the host name and ip
        /// </summary>
        private ActiveObject _activeObject = new ActiveObject();
        private delegate void SetHostNameAndIPDel(out string ip, out string hostname, out  bool online);
        private SetHostNameAndIPDel _SetHostNameAndIPDel;

        private int _chosenImageIndex = 2;
        #endregion

        #region Properties
        public Client Client
        {
            get { return _client; }
        }
        private int UsedSlaveCount
        {
            get
            {
                int count = 0;
                foreach (Slave slave in _client)
                    if (slave.Use)
                        ++count;
                return count;
            }
        }
        #endregion

        #region Constructors
        public ClientTreeViewItem()
        {
            InitializeComponent();

            _SetHostNameAndIPDel = SetHostNameAndIP_Callback;
            _activeObject.OnResult += new EventHandler<ActiveObject.OnResultEventArgs>(_activeObject_OnResult);
        }
        public ClientTreeViewItem(Client client)
            : this()
        {
            _client = client;
            RefreshGui();

            chk.CheckedChanged -= chk_CheckedChanged;
            chk.Checked = _client.Use;
            chk.CheckedChanged += chk_CheckedChanged;

            lblClient.Text = txtClient.Visible ? "Host Name or IP:" : _client.ToString() + " (#" + UsedSlaveCount + "/" + _client.Count + ")";

            txtClient.Text = (_client.HostName == string.Empty) ? _client.IP : _client.HostName;


            //To check if the use has changed of the child controls.
            SolutionComponent.SolutionComponentChanged += new EventHandler<SolutionComponentChangedEventArgs>(SolutionComponent_SolutionComponentChanged);
        }
        #endregion

        #region Functions
        private void SolutionComponent_SolutionComponentChanged(object sender, SolutionComponentChangedEventArgs e)
        {
            //To set if the client is used or not.
            if (sender is Slave)
            {
                Slave slave = sender as Slave;
                if (_client.Contains(slave))
                {
                    _client.Use = false;
                    foreach (Slave sl in _client)
                        if (sl.Use)
                        {
                            _client.Use = true;
                            break;
                        }
                    if (chk.Checked != _client.Use)
                    {
                        chk.CheckedChanged -= chk_CheckedChanged;
                        chk.Checked = _client.Use;
                        chk.CheckedChanged += chk_CheckedChanged;
                    }
                }
            }
        }
        private void _Enter(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.Control;
            SetVisibleControls();

            if (AfterSelect != null)
                AfterSelect(this, null);
        }
        public void Unfocus()
        {
            this.BackColor = Color.Transparent;
            SetVisibleControls();
        }
        private void txtClient_Leave(object sender, EventArgs e)
        {
            SetHostNameAndIP();
            SetVisibleControls();
        }
        private void _MouseEnter(object sender, EventArgs e)
        {
            SetVisibleControls();
        }
        private void _MouseLeave(object sender, EventArgs e)
        {
            SetVisibleControls();
        }
        public void SetVisibleControls(bool visible)
        {
            txtClient.Visible = picDuplicate.Visible = picDelete.Visible = visible;
            lblClient.Text = visible ? "Host Name or IP:" : _client.ToString() + " (#" + UsedSlaveCount + "/" + _client.Count + ")";
        }
        public void SetVisibleControls()
        {
            if (this.BackColor == SystemColors.Control)
                SetVisibleControls(true);
            else
                SetVisibleControls(ClientRectangle.Contains(PointToClient(Cursor.Position)));
        }

        public void RefreshGui()
        {
            lblClient.Text = txtClient.Visible ? "Host Name or IP:" : _client.ToString() + " (#" + UsedSlaveCount + "/" + _client.Count + ")";

            _client.Use = UsedSlaveCount != 0;
            if (_client.Use != chk.Checked)
            {
                chk.CheckedChanged -= chk_CheckedChanged;
                chk.Checked = _client.Use;
                chk.CheckedChanged += chk_CheckedChanged;
            }
        }
        private void _KeyUp(object sender, KeyEventArgs e)
        {
            if (sender == txtClient && e.KeyCode == Keys.Enter)
                SetHostNameAndIP();

            if (e.KeyCode == Keys.ControlKey)
                _ctrl = false;
            else if (_ctrl)
                if (e.KeyCode == Keys.R && DeleteClicked != null)
                    DeleteClicked(this, null);
                else if (e.KeyCode == Keys.D && DuplicateClicked != null)
                    DuplicateClicked(this, null);
                else if (e.KeyCode == Keys.U)
                    chk.Checked = !chk.Checked;
            if (e.KeyCode == Keys.F5)
                SetHostNameAndIP();
        }
        /// <summary>
        /// IP or Host Name can be filled in in txtclient.
        /// </summary>
        /// <returns>False if it was already busy doing it.</returns>
        public bool SetHostNameAndIP()
        {
            //Make sure this can not happen multiple times at the same time.
            if (!this.Enabled)
                return false;

            this.Enabled = false;

            tmrRotateOnlineOffline.Start();

            picOnlineOffline.Image = imageList.Images[2];

            txtClient.Text = txtClient.Text.Trim().ToLower();

            string ip = string.Empty;
            string hostname = string.Empty;
            bool online = false;

            _activeObject.Send(_SetHostNameAndIPDel, ip, hostname, online);

            return true;
        }
        private void SetHostNameAndIP_Callback(out string ip, out string hostname, out  bool online)
        {
            online = false;
            ip = string.Empty;
            hostname = string.Empty;
            IPAddress address;
            if (IPAddress.TryParse(txtClient.Text, out address))
            {
                ip = address.ToString();
                try
                {
                    hostname = Dns.GetHostByAddress(address).HostName;
                    if (hostname == null) hostname = string.Empty;
                    online = true;
                }
                catch { }

            }
            else
            {
                hostname = txtClient.Text;
                IPAddress[] addresses = { };
                try
                {
                    if (hostname.Length != 0)
                        addresses = Dns.GetHostByName(hostname).AddressList;
                }
                catch { }

                if (addresses != null && addresses.Length != 0)
                {
                    ip = addresses[0].ToString();
                    online = true;
                }
            }
        }
        private void _activeObject_OnResult(object sender, ActiveObject.OnResultEventArgs e)
        {
            SynchronizationContextWrapper.SynchronizationContext.Send(delegate
            {
                string ip = e.Arguments[0] as string;
                string hostname = e.Arguments[1] as string;
                bool online = (bool)e.Arguments[2];
                bool changed = false;

                if (_client.IP != ip || _client.HostName != hostname)
                    changed = true;

                _client.IP = ip;
                _client.HostName = hostname;

                tmrRotateOnlineOffline.Stop();
                if (online)
                {
                    picOnlineOffline.Image = imageList.Images[1];
                    toolTip.SetToolTip(picOnlineOffline, "Online <f5>");
                }
                else
                {
                    picOnlineOffline.Image = imageList.Images[0];
                    toolTip.SetToolTip(picOnlineOffline, "Offline <f5>");
                }

                if (changed)
                    _client.InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);

                if (HostNameAndIPSet != null)
                    HostNameAndIPSet(this, null);

                this.Enabled = true;
            }, null);
        }
        private void tmrRotateOnlineOffline_Tick(object sender, EventArgs e)
        {
            //Rotate it to visualize it is refreshing
            _chosenImageIndex = _chosenImageIndex == 2 ? 3 : 2;
            picOnlineOffline.Image = imageList.Images[_chosenImageIndex];
        }

        private void _KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                _ctrl = true;
        }
        private void picDuplicate_Click(object sender, EventArgs e)
        {
            if (DuplicateClicked != null)
                DuplicateClicked(this, null);
        }
        private void picDelete_Click(object sender, EventArgs e)
        {
            if (DeleteClicked != null)
                DeleteClicked(this, null);
        }
        private void picOnlineOffline_Click(object sender, EventArgs e)
        {
            SetHostNameAndIP();
        }
        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            if (_client.Use != chk.Checked)
            {
                _client.Use = chk.Checked;
                foreach (TileStresstest ts in _client)
                    ts.Use = _client.Use;

                _client.InvokeSolutionComponentChangedEvent(SolutionComponentChangedEventArgs.DoneAction.Edited);
            }
        }
        #endregion


        public void SetDistributedTestMode(DistributedTestMode distributedTestMode)
        {
        }

        public DistributedTestMode DistributedTestMode
        {
            get { throw new NotImplementedException(); }
        }
    }
}
