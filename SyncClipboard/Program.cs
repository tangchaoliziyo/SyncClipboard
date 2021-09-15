﻿using System;
using System.Threading;
using System.Windows.Forms;
using SyncClipboard.Utility;
namespace SyncClipboard
{
    internal static class Program
    {
        public static string SoftName = "SyncClipboard";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Log.Write("[Program] started");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Mutex mutex = new Mutex(false, SoftName, out bool creetedNew);
            if (creetedNew)
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                // handle UI exceptions
                Application.ThreadException += Application_ThreadException;
                // handle non-UI exceptions
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.ApplicationExit += Application_ApplicationExit;

                Global.StartUp();

                Application.Run();
            }
            else
            {
                MessageBox.Show("SyncClipboard已经运行！", SoftName);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Write("程序错误:" + e.Exception.Message);
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Write("程序错误:" + e.ExceptionObject.ToString());
        }
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Global.EndUp();
            Application.ApplicationExit -= Application_ApplicationExit;
            Application.ThreadException -= Application_ThreadException;
            Log.Write("[Program] exited");
        }
    }
}
