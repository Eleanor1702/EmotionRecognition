using System;
using System.Diagnostics;
using System.IO;

namespace EmotionRecognition.Models
{
    public class PrepareModel
    {
        public Process neuralNetProcess;
        //private string pathToCmd = @"C:\Windows\System32\cmd.exe";
        //private ProcessStartInfo startInfo;
        public ReturnObject returnObject = new ReturnObject("Default", 999, ReturnObject.Type.NoFaceDetected);
        public EmotionTableInterpreter interpreter = null;

        public PrepareModel()
        {
            //Console.WriteLine("PreareModel called");
            //Console.WriteLine("\twith:\n\t" + pathToScript + "\n\t" + pathToPicture + "\n\t" + Directory.GetCurrentDirectory());
            neuralNetProcess = new Process();
            neuralNetProcess.StartInfo.FileName = "Python.exe";
            neuralNetProcess.StartInfo.Arguments = "camera_roll.py" + " " + "Images/";
            neuralNetProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            neuralNetProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            neuralNetProcess.StartInfo.UseShellExecute = false;
            neuralNetProcess.StartInfo.RedirectStandardOutput = true;
            //Console.WriteLine(startInfo);
            //StartETI(); -> Zu Testzwecken wird der ETI später aufgerufen.

        }
        public void StartETI()
        {
            try
            {
                interpreter = new EmotionTableInterpreter(neuralNetProcess, ref returnObject);
            }
            catch (Exception ex)
            {
                returnObject.FaceDetected = ReturnObject.Type.Exception;
                returnObject.Emotion = "Exception thrown!";
                returnObject.Percentage = 0;
            }
        }

        public ReturnObject GetReturnObject()
        {
            while (returnObject.Emotion == "Default" && returnObject.Percentage == 999)
            {
                ;
            }
            return returnObject;
        }
    }
}
