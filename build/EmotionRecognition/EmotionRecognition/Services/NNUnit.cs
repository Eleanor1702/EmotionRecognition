using EmotionRecognition.Models;
using System;
using System.Windows.Media.Imaging;

namespace EmotionRecognition.Services {

    //This Class presents the NN Unit. The Code presented here is just an example.

    public class NNUnit {

		public ReturnObject analyse(BitmapSource img) {
            //CheckUserExist for up to date user Existence
            //first 2 parameters should be provided by NNUnit through a function


            //ReturnObject tst = new ReturnObject("Angry", 30, CheckUserExist(img));


            PrepareModel prepareModel = new PrepareModel(); //Auruf : EmotionRecognition_NNProcessComb.exe camera_roll.py Images/
            ReturnObject returnObject = prepareModel.GetReturnObject();




            return returnObject;
        }

		public bool CheckUserExist(BitmapSource img) {
			//this random is taking the functionality of NNUnit recognizing a user
			bool[] boolValue = new bool[] { true, false };
			Random random = new Random();
			int index = random.Next(0, boolValue.Length);
			bool userRecognized = boolValue[index];

			//if user recognized by NNUnit, a true is returned to allow GameStart in MainWindow & update NNResult in analyse
			return userRecognized;
		}
    }
}
