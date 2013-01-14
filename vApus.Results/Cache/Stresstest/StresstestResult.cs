﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */

using System;
using System.Collections.Generic;

namespace vApus.Results
{
    public class StresstestResult
    {
        public StresstestResult()
        {
            StartedAt = DateTime.Now;
            StoppedAt = DateTime.MinValue;
            ConcurrencyResults = new List<ConcurrencyResult>();
        }

        public DateTime StartedAt { get; private set; }
        public DateTime StoppedAt { get; internal set; }

        public List<ConcurrencyResult> ConcurrencyResults { get; private set; }
    }
}