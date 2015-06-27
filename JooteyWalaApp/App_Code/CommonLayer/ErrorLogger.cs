using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Collections.Generic;
using System.Text;
using System.IO;

using NSpring.Logging;

/// <summary>
/// Summary description for ErrorLog1
/// </summary>

    public class ErrorLogger
    {

        Logger logEx; //= Logger.CreateFileLogger("c:\\temp\\nspring.log", "{ts}{z}  [{ln:1w}]  {msg}");
        string sCurrentDay= DateTime.Now.Day.ToString();
        string sCurrentMonth = DateTime.Now.Month.ToString();
        string sCurrentYear = DateTime.Now.Year.ToString();

        public string LogFilePath
        {
            get
            {
                return Path.Combine(HttpContext.Current.Server.MapPath("ErrorLog/"), sCurrentDay + "-" + sCurrentMonth + "-" + sCurrentYear + "-" + "Exception.log");
            }
        }
        public ErrorLogger()
        {
            //logEx = Logger.CreateFileLogger(Path.Combine(LogFolder, "Exception.log"), "{ts}  [{ln:1w}]  {msg}");
            //logEx = Logger.CreateFileLogger(AppConf.ExceptionPath, "{ts}  [{ln:1w}]  {msg}");
            //logEx.IsBufferingEnabled = true;
            //logEx.BufferSize = 1000;
            //logEx.Open();

        }
        public void Log(string msg)
        {
            logEx.Log(Level.Exception, msg);
            //logEx.Flush();
        }
        public void Log(StringBuilder msg)
        {
            logEx.Log(Level.Exception, msg.ToString());
            //logEx.Flush();
        }

        public Logger GetExceptionLogger()
        {
            return logEx;
        }

        public Logger Open()
        {
            if (logEx == null)
                logEx = Logger.CreateFileLogger(LogFilePath, "{ts}  [{ln:1w}]  {msg}");

            logEx.IsBufferingEnabled = true;
            logEx.BufferSize = 1000;
            logEx.Open();

            return logEx;
        }

        public void Close()
        {
            logEx.Flush();
            logEx.Close();
        }
        public void Flush()
        {
            logEx.Flush();
        }
    }

