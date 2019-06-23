using EmotionRecognition.Models;

namespace EmotionRecognition.Services {

    //This Class presents the NN Unit. The Code presented here is just an example.

    public class NNUnit {

		public ReturnObject analyse() {

            //Call: camera_roll.py Images/
            PrepareModel prepareModel = new PrepareModel();
            ReturnObject returnObject = prepareModel.GetReturnObject();

            return returnObject;
        }

        //called to handle User Exceptions
		public ReturnObject.Type CheckUserExist() {

            return analyse().FaceDetected;
		}
    }
}
