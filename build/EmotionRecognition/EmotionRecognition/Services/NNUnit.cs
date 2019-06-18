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

        //This Function should be called to check if User exist... And handle exception if 
		public bool CheckUserExist(BitmapSource img) {
            PrepareModel prepareModel = new PrepareModel(); //Auruf : EmotionRecognition_NNProcessComb.exe camera_roll.py Images/
            ReturnObject returnObject = prepareModel.GetReturnObject();

            //This line uncommented when both parts connected!!!!
            //bool userRecognized = returnObject.getFaceDetected();

            //this random is taking the functionality of "NNUnit recognizing a user"
            bool[] boolValue = new bool[] { true, false };
			Random random = new Random();
			int index = random.Next(0, boolValue.Length);
			bool userRecognized = boolValue[index];

			//if user recognized by NNUnit, a true is returned to allow GameStart in MainWindow & update NNResult in analyse
			return userRecognized;
		}
    }
}
