﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;

namespace vApus.Results {
    /// <summary>
    /// Do not forget the prepare for rerun function.
    /// </summary>
    public class RunResult {
        #region Properties
        public int Run { get; private set; }

        /// <summary>
        ///     For break on last run sync.
        /// </summary>
        public int RerunCount { get; private set; }

        public DateTime StartedAt { get; internal set; }
        public DateTime StoppedAt { get; set; }

        /// <summary>
        ///     Dont forget to set this.
        /// </summary>
        public VirtualUserResult[] VirtualUserResults { get; internal set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Do not forget the prepare for rerun function.
        /// </summary>
        /// <param name="run"></param>
        /// <param name="concurrentUsers"></param>
        public RunResult(int run, int concurrentUsers) {
            StartedAt = DateTime.Now;
            StoppedAt = DateTime.MinValue;
            Run = run;
            VirtualUserResults = new VirtualUserResult[concurrentUsers];
        }
        #endregion

        #region Functions
        /// <summary>
        ///     Only used for break on last run sync. Do this before the rerun is executed.
        /// </summary>
        public void PrepareForRerun() {
            ++RerunCount;
            foreach (VirtualUserResult result in VirtualUserResults)
                result.PrepareForRerun();
        }
        #endregion
    }
}