﻿/*
 * Copyright 2017 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using IntelliLock.Licensing;
using RandomUtils.Log;
using System;
using System.IO;
using System.Windows.Forms;

namespace vApus.Gui {
    public static class LicenseChecker {
        public static event EventHandler<LicenseCheckEventArgs> LicenseCheckFinished;

        public enum __Status {
            CheckingLicense = 0,
            Licensed = 1,
            NotLicensed = 2
        }

        private static readonly string[] LicenseStatusses = {
            "License not checked", "Licensed", "Evaluation mode", "Evaluation expired", "License file not found", "Hardware did not match the license",
            "Invalid signature", "Server validation failed", "Deactivated", "Reactivated", "Floating license users exceeded",
            "Floating license server error", "Full version expired", "Floating license server timeout"
        };

        private static string _licenseFile = Path.Combine(Application.StartupPath, "license.license");

        public static __Status Status { get; private set; }
        public static string StatusMessage { get; private set; }

        static LicenseChecker() {
            EvaluationMonitor.LicenseCheckFinished += EvaluationMonitor_LicenseCheckFinished;
            if (File.Exists(_licenseFile)) {
                CheckCurrentLicense();
            }
            else {
                Status = __Status.NotLicensed;
                StatusMessage = "No license file found. vApus will not run without a valid license.";
                if (LicenseCheckFinished != null) LicenseCheckFinished(null, new LicenseCheckEventArgs(Status, StatusMessage));
            }
        }
        /// <summary>
        /// Explicitely activate the license file, if any, located in the vApus dir.
        /// </summary>
        public static void ActivateLicense() {
            Status = __Status.CheckingLicense;
            StatusMessage = "Checking license...";
            if (File.Exists(_licenseFile)) {
                EvaluationMonitor.LoadLicense(_licenseFile);
            }
            else {
                Status = __Status.NotLicensed;
                StatusMessage = "No license file found. vApus will not run without a valid license.";
                if (LicenseCheckFinished != null) LicenseCheckFinished(null, new LicenseCheckEventArgs(Status, StatusMessage));
            }
        }

        private static void CheckCurrentLicense() {
            try {
                if (EvaluationMonitor.CurrentLicense == null) {
                    Status = __Status.NotLicensed;
                    StatusMessage = "No license file found. vApus will not run without a valid license.";
                    if (LicenseCheckFinished != null) LicenseCheckFinished(null, new LicenseCheckEventArgs(Status, StatusMessage));

                    //Not possible that you get this status. vApus is not allowed to run whithout a license.
                    //This can be an expired license. In that case vApus will run for 10 minutes.
                }
                else {

                    StatusMessage = "License status: ";

                    if (EvaluationMonitor.CurrentLicense.ExpirationDate_Enabled) {
                        if (EvaluationMonitor.CurrentLicense.ExpirationDate < DateTime.Now) {
                            Status = __Status.NotLicensed;
                            StatusMessage += "Expired. vApus will not run without a valid license.\n\n";
                        }
                        else {
                            if (EvaluationMonitor.CurrentLicense.LicenseStatus == LicenseStatus.Licensed) {
                                Status = __Status.NotLicensed;
                                StatusMessage += LicenseStatusses[(int)LicenseStatus.Licensed] + "\n\n";
                            }
                            else {
                                Status = __Status.NotLicensed;
                                StatusMessage += LicenseStatusses[(int)EvaluationMonitor.CurrentLicense.LicenseStatus] + ". vApus will not run without a valid license.\n\n";
                            }
                        }
                        StatusMessage += "License valid until " + EvaluationMonitor.CurrentLicense.ExpirationDate + "\n\n";
                    }
                    else {
                        if (EvaluationMonitor.CurrentLicense.LicenseStatus == LicenseStatus.Licensed) {
                            Status = __Status.NotLicensed;
                            StatusMessage += LicenseStatusses[(int)LicenseStatus.Licensed] + "\n\n";
                        }
                        else {
                            Status = __Status.NotLicensed;
                            StatusMessage += LicenseStatusses[(int)EvaluationMonitor.CurrentLicense.LicenseStatus] + ". vApus will not run without a valid license.\n\n";
                        }
                    }

                    if (EvaluationMonitor.CurrentLicense.LicenseInformation.Count != 0) {
                        StatusMessage += "Licensed to\n";
                        for (int i = 0; i != EvaluationMonitor.CurrentLicense.LicenseInformation.Count; i++)
                            StatusMessage += "  " + EvaluationMonitor.CurrentLicense.LicenseInformation.GetByIndex(i) + "\n";
                    }
                }
            }
            catch (Exception ex) {
                Status = __Status.NotLicensed;
                StatusMessage = "Checking license failed! vApus will not run without a valid license.";
                Loggers.Log(Level.Error, StatusMessage, ex);
            }

            if (LicenseCheckFinished != null) LicenseCheckFinished(null, new LicenseCheckEventArgs(Status, StatusMessage));
        }

        private static void EvaluationMonitor_LicenseCheckFinished() {
            CheckCurrentLicense();
        }

        public class LicenseCheckEventArgs : EventArgs {
            public __Status Status { get; private set; }
            public string StatusMessage { get; private set; }

            internal LicenseCheckEventArgs(__Status status, string statusMessage) {
                Status = status;
                StatusMessage = statusMessage;
            }
        }

    }
}
