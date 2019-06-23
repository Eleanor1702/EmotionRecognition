using System.Diagnostics;
using System;

namespace EmotionRecognition
{
    public class PrepareModel
    {
        private Process neuralNetProcess;
        private string pathToScript;
        private string pathToPicture;
        private string pathToCmd;
        private ProcessStartInfo startInfo;

        public PrepareModel(string picture, string script)
        {
            pathToPicture = picture;
            pathToScript = script;
            neuralNetProcess = new Process();
            startInfo = new ProcessStartInfo()
            {
                FileName = pathToCmd,
                Arguments = "python3 " + pathToScript,//Command to be processed at Process start up. Wie sollen die Bilder eingebunden werden?
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            neuralNetProcess.StartInfo = startInfo;
            
        }
        public void StartETI()
        {
            try
            { 
                
            }
            catch(Exception ex)
            {
                //Was will GUI haben?
            }
        }
    }
}