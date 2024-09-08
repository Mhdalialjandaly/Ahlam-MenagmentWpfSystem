using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EyeClinic.Core
{
    public static class Global
    {
        private static Dictionary<string, object> Dictionary { get; set; }

        public static void AddValue(string key, object item) {
            Dictionary ??= new Dictionary<string, object>();

            if (Dictionary.TryGetValue(key, out object value)) {
                Dictionary.Remove(key);
            }

            Dictionary.Add(key, item);
        }

        public static void RemoveValue(string key) {
            if (Dictionary == null)
                return;

            if (Dictionary.TryGetValue(key, out object value)) {
                Dictionary.Remove(key);
            }
        }

        public static object GetValue(string key) {
            if (Dictionary == null)
                return null;

            Dictionary.TryGetValue(key, out object item);

            return item;
        }


        public static string DateFormat = "dd/MM/yyyy";
        public static string DeviceName { get; set; }

        public static string TestImageDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Orders", "PDFFiles");

        public static string MedicalReportPath = @"D:\Eye Clinic System\MedicalReports";
        public static string MedicalReportTemplatePath = @"D:\Eye Clinic System\MedicalReports\Templates";
        public static string[] OperationWithSessionCodes;
    }

    public static class GlobalKeys
    {
        public const string LoggedUser = nameof(LoggedUser);
        public const string SelectedPrinter = nameof(SelectedPrinter);
    }

}
