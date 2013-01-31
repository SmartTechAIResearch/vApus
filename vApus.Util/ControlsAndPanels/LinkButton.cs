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
using System.Drawing;
using System.Windows.Forms;

namespace vApus.Util
{
    public class LinkButton : LinkLabel
    {
        [Description("Use this rather than LinkClicked, Click or KeyDown.")]
        public event EventHandler ActiveChanged;

        private bool _active;

        public LinkButton()
        {
            TextAlign = ContentAlignment.TopCenter;
            Padding = new Padding(3, 4, 3, 3);
            AutoSize = true;
            SetStateInGui();
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    SetStateInGui();
                }
            }
        }

        [Description(
            "Must be set to true or false for all LinkButtons in the Parent. This behaviour is only applied when clicking or pushing the enter key on the control."
            )]
        public bool RadioButtonBehavior { get; set; }

        private void SetStateInGui()
        {
            if (_active)
            {
                BorderStyle = BorderStyle.FixedSingle;
                LinkColor = ActiveLinkColor = VisitedLinkColor = ForeColor = Color.Black;
                Font = new Font(Font, FontStyle.Bold);
                LinkBehavior = LinkBehavior.NeverUnderline;
            }
            else
            {
                BorderStyle = BorderStyle.None;
                LinkColor = ActiveLinkColor = VisitedLinkColor = ForeColor = Color.Blue;
                Font = new Font(Font, FontStyle.Regular);
                LinkBehavior = LinkBehavior.AlwaysUnderline;
            }
        }
        public void PerformClick()
        {
            OnClick(new EventArgs());
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Activate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Enter) Activate();
        }

        private void Activate()
        {
            if (RadioButtonBehavior)
            {
                if (Parent != null)
                {
                    var otherActiveLinkButtons = new List<LinkButton>();
                    foreach (Control ctrl in Parent.Controls)
                        if (ctrl != this && ctrl is LinkButton)
                        {
                            var lbtn = ctrl as LinkButton;
                            if (lbtn.Active) otherActiveLinkButtons.Add(lbtn);
                        }

                    if (Active)
                    {
                        if (otherActiveLinkButtons.Count != 0) Active = false;
                    }
                    else
                    {
                        Active = true;
                        foreach (LinkButton lbtn in otherActiveLinkButtons) lbtn.Active = false;
                    }
                }
            }
            else Active = !Active;

            if (ActiveChanged != null) ActiveChanged(this, null);
        }
    }
}