using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionRecognition_NNProcessComb
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program.cs called");
            PrepareModel prepareModel = new PrepareModel(args[0], args[1]); //Auruf : EmotionRecognition_NNProcessComb.exe camera_roll.py Images/
            ReturnObject returnObject = prepareModel.GetReturnObject();


            Console.WriteLine("\n\n\nAusgabe ReturnObject ! GUI SEITE !");
            Console.WriteLine("Face detected: " + Convert.ToString(returnObject.FaceDeteced));
            Console.WriteLine("Emotion: " + returnObject.Emotion);
            Console.WriteLine("Percentage: " + returnObject.Percentage.ToString());

        }
    }
}
