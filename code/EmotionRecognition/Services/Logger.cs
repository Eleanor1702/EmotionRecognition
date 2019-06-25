using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nocksoft.IO.ConfigFiles;

namespace EmotionRecognition.Services
{
    class Logger
    {
        private static string logFileName = "log-File.ini";
        private static string logSection = "log-at-";
        private static string logKey = "Info";
        private INIFile logFile;

        public Logger()
        {
            logFile = new INIFile(logFileName, true);
        }

        public void Log (string logContent)
        {
            logFile.SetValue(logSection + DateTime.Now, logKey, logContent);
        }

    }
}
