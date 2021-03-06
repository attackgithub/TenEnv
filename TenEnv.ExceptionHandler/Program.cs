﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TenEnv.ExceptionHandler
{
    static class Program
    {
        public const string IssuesPage = "https://github.com/feel-the-dz3n/TenEnv/issues";
        public const string IssuesPageNew = "https://github.com/feel-the-dz3n/TenEnv/issues/new";

        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception ex)
            {
                return "Can't decode program's output: " + ex.Message + $" ({ex.GetType().Name})";
            }
        }

        public static string ReadFile(string file)
        {

            try
            {
                using (StreamReader r = new StreamReader(file))
                    return $"File '{file}':\r\n{r.ReadToEnd()}";
            }
            catch (Exception ex)
            {
                return $"Can't read file '{file}': {ex.Message} ({ex.GetType().Name})";
            }
        }

        // public const string AngryText = "Something went wrong with Base64 decoder, so let's think that this program crashed too.\r\n\r\n\r\nStupid script kiddies. >:|";
        public static string DecodedText = "";
        public static string[] Args = { };

        private static void AppendDecoded(string something)
        {
            if (DecodedText.Length != 0)
                DecodedText += Environment.NewLine + Environment.NewLine;

            DecodedText += something;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Args = args;

            if (Args.Length <= 0)
            {
                Process.Start(IssuesPage);
                Environment.Exit(0);
            }

            foreach (var arg in Args)
            {
                if (arg.StartsWith("base64:"))
                    AppendDecoded(Base64Decode(arg.Remove(0, "base64:".Length)));
                else if (arg.StartsWith("txt:"))
                    AppendDecoded(ReadFile(arg.Remove(0, "txt:".Length)));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(DecodedText));
        }
    }
}
