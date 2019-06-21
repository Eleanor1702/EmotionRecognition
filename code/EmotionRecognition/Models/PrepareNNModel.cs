using System;
using System.Diagnostics;
using System.IO;

namespace EmotionRecognition.Models
{
    public class PrepareModel
    {
        private Process neuralNetProcess;
        private string pathToCmd = @"C:\Windows\System32\cmd.exe";
        private ProcessStartInfo startInfo;
        private ReturnObject returnObject = new ReturnObject("Default", 999, ReturnObject.Type.NoFaceDetected);

        public PrepareModel(/*string pathToScript, string pathToPicture*/)
        {
            //Console.WriteLine("PreareModel called");
            //Console.WriteLine("\twith:\n\t" + pathToScript + "\n\t" + pathToPicture + "\n\t" + Directory.GetCurrentDirectory());
            neuralNetProcess = new Process();
            neuralNetProcess.StartInfo.FileName = "Python.exe";
            neuralNetProcess.StartInfo.Arguments = @"../../NeuronalNets/camera_roll.py" + " " + @"../../Images/";
            neuralNetProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            neuralNetProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            neuralNetProcess.StartInfo.UseShellExecute = false;
            neuralNetProcess.StartInfo.RedirectStandardOutput = true;
            //Console.WriteLine(startInfo);
            StartEmoRecTableInterpreter();

        }
        public void StartEmoRecTableInterpreter()
        {
            try
            {
                EmotionTableInterpreter interpreter = new EmotionTableInterpreter(neuralNetProcess, ref returnObject);
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
