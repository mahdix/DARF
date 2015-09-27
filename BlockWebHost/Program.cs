using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using DCRF.Primitive;

namespace BlockWebHost
{
    static class Program
    {
        static Form1 form1 = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
               
            form1 = new Form1();
            Application.Run(form1);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //TODO: fix
            //form1.ProcessRequest("LogWebEvent", LogType.Exception, "CRITICAL THREAD EXCEPTION OCCURED: "+ GetExceptionString(e.Exception));
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO: fix
            //form1.ProcessRequest("LogWebEvent", LogType.Exception, "CRITICAL APPLICATION EXCEPTION OCCURED: " + GetExceptionString(e.ExceptionObject as Exception));
        }

        private static string GetExceptionString(Exception exc)
        {
            string text = "Message: " + exc.Message;

            try
            {
                text += "\r\n";
                text += "Source: " + exc.Source;
                text += "\r\n";
                text += "Target Site: " + exc.TargetSite.ToString();
                text += "\r\n";
                text += "Stack trace: " + exc.StackTrace;
                text += "\r\n";


                if (exc.InnerException != null)
                {
                    text += "(InnerException:\r\n";
                    text += GetExceptionString(exc.InnerException);
                    text += ")\r\n";
                }
            }
            catch (Exception ex)
            {
                text += "\r\nTwice Exception:" + ex.Message;
            }

            return text;
        }


    }
}
