﻿/*
 * Copyright 2009 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;

namespace vApus.DistributedTesting
{
    /// <summary>
    /// </summary>
    public class IPChangedEventArgs : EventArgs
    {
        public readonly string IP;
        /// <summary>
        /// </summary>
        /// <param name="ip"></param>
        public IPChangedEventArgs(string ip)
        {
            IP = ip;
        }
    }
    public class ListeningErrorEventArgs : EventArgs
    {
        public readonly Exception Exception;
        public readonly string SlaveIP;
        public readonly int SlavePort;

        public ListeningErrorEventArgs(string slaveIP, int slavePort, Exception exception)
        {
            Exception = exception;
            SlaveIP = slaveIP;
            SlavePort = slavePort;
        }

        public override string ToString()
        {
            return "Listening error occured for slave " + SlaveIP + ":" + SlavePort + " threw following exception: " + Exception;
        }
    }
    public class TestProgressMessageReceivedEventArgs : EventArgs
    {
        public readonly TileStresstest TileStresstest;
        public readonly TestProgressMessage TestProgressMessage;
        public TestProgressMessageReceivedEventArgs(TestProgressMessage testProgressMessage)
            : this(null, testProgressMessage)
        { }
        public TestProgressMessageReceivedEventArgs(TileStresstest tileStresstest, TestProgressMessage testProgressMessage)
        {
            TileStresstest = tileStresstest;
            TestProgressMessage = testProgressMessage;
        }
    }
    public class FinishedEventArgs : EventArgs
    {
        public readonly int OK, Cancelled, Error;
        public FinishedEventArgs(int ok, int cancelled, int error)
        {
            OK = ok;
            Cancelled = cancelled;
            Error = error;
        }
    }
    public class ResultsMessageReceivedEventArgs : EventArgs
    {
        public readonly ResultsMessage ResultsMessage;
        public ResultsMessageReceivedEventArgs(ResultsMessage resultsMessage)
        {
            ResultsMessage = resultsMessage;
        }
    }
}
