using EmotionRecognition.Services;
using System.Windows.Media.Imaging;

namespace EmotionRecognition.Models {

    public class GameClass {

        private NNUnit nnUnit;
        private int points;

        public GameClass() {
            this.nnUnit = new NNUnit();
            this.points = 0;
        }

        //Get and Set Points
        public int Points {
            get { return points; }
        }

        //for the sake of the example the parameters types are string
        public bool CompareEmotion(BitmapSource img , string randomEmotion) {
            //For example calling the func in NN Unit for analysing
            //NN analyse throws an object of NNResult where for example(EmotionName, percentage or points) are saved.
            ReturnObject result = nnUnit.analyse(img);

            //check if user DOESNT exist (During Game)
            if(result.FaceDetected == ReturnObject.Type.NoFaceDetected) {
                throw new UserMissingException();
            }

            //Compare our randomEmotion and the result
            if (randomEmotion == result.Emotion) {
                //save points
                points += result.Percentage;

                //true for SUCCESS
                return true;
            }

            //false for FAILURE
            return false;
        }

		//check if user Exist to Start Game
		public bool StartGame(BitmapSource img) {
			if (nnUnit.CheckUserExist(img)) {
				return true;
			}

			return false;
		}
    }
}
