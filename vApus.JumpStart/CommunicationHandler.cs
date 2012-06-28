﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * Technical University Kortrijk, Department GKG
 *  
 * Author(s):
 *    Vandroemme Dieter
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using vApus.JumpStartStructures;
using vApus.Util;

namespace vApus.JumpStart
{
    public static class CommunicationHandler
    {
        [ThreadStatic]
        private static HandleJumpStartWorkItem _handleJumpStartWorkItem;

        #region Message Handling

        public static Message<Key> HandleMessage(SocketWrapper receiver, Message<Key> message)
        {
            try
            {
                switch (message.Key)
                {
                    case Key.JumpStart:
                        return HandleJumpStart(message);
                    case Key.Kill:
                        return HandleKill(message);
                }
            }
            catch { }
            return message;
        }
        private static Message<Key> HandleJumpStart(Message<Key> message)
        {
            JumpStartMessage jumpStartMessage = (JumpStartMessage)message.Content;
            string[] ports = jumpStartMessage.Port.Split(',');
            string[] processorAffinity = jumpStartMessage.ProcessorAffinity.Split(',');

            AutoResetEvent waithandle = new AutoResetEvent(false);
            int j = 0;
            for (int i = 0; i != ports.Length; i++)
            {
                Thread t = new Thread(delegate(object state)
                {
                    _handleJumpStartWorkItem = new HandleJumpStartWorkItem();
                    _handleJumpStartWorkItem.HandleJumpStart(jumpStartMessage.IP, int.Parse(ports[(int)state]), processorAffinity[(int)state]);
                    if (Interlocked.Increment(ref j) == ports.Length)
                        waithandle.Set();
                });
                t.IsBackground = true;
                t.Start(i);
            }

            waithandle.WaitOne();

            //Wait until the vApusses are ready to accept communication from the master.
            Thread.Sleep(20000);

            return message;
        }


        private static Message<Key> HandleKill(Message<Key> message)
        {
            KillMessage killMessage = (KillMessage)message.Content;
            Kill(killMessage.ExcludeProcessID);
            return message;
        }
        private static void Kill(int excludeProcessID)
        {
            Process[] processes = Process.GetProcessesByName("vApus");
            Parallel.ForEach(processes, delegate(Process p)
            {
                if (excludeProcessID == -1 || p.Id != excludeProcessID)
                    KillProcess(p);
            });
        }
        private static void KillProcess(Process p)
        {
            try
            {
                if (!p.HasExited)
                {
                    p.Kill();
                    p.WaitForExit(10000);
                }
            }
            catch { }
        }
        #endregion

        private class HandleJumpStartWorkItem
        {
            public void HandleJumpStart(string ip, int port, string processorAffinity)
            {
                Process process = new Process();
                try
                {
                    string vApusLocation = Path.Combine(Application.StartupPath, "vApus.exe");

                    if (processorAffinity.Length == 0)
                        process.StartInfo = new ProcessStartInfo(vApusLocation, "-ipp " + ip + ":" + port);
                    else
                        process.StartInfo = new ProcessStartInfo(vApusLocation, "-ipp " + ip + ":" + port + " -pa " + processorAffinity);

                    process.Start();
                    if (!process.WaitForInputIdle(10000))
                        throw new TimeoutException("The process did not start.");
                }
                catch
                {
                    try
                    {
                        if (!process.HasExited)
                            process.Kill();
                    }
                    catch { }
                    process = null;
                }
            }

        }
    }
}