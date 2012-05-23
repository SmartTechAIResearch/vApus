﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using vApus.SolutionTree;

namespace vApus.DistributedTesting
{
    public class Slave : LabeledBaseItem
    {
        #region Fields
        private bool _use = true;
        private int _port = 1337;
        #endregion

        #region Properties
        [SavableCloneable]
        public bool Use
        {
            get { return _use; }
            set { _use = value; }
        }
        [SavableCloneable]
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        #endregion

        #region Constructor
        public Slave()
        {
            ShowInGui = false;
        }
        #endregion

        #region Functions
        public override string ToString()
        {
            object parent = Parent;
            if (parent != null)
                return parent.ToString() + " - " + _port;

            return base.ToString();
        }
        #endregion
    }
}
