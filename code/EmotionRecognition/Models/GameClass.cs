using EmotionRecognition.Services;

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
        public bool CompareEmotion(string randomEmotion) {
            //NN analyse throws an object with EmotionName, percentage and user Recognition.
            ReturnObject result = nnUnit.analyse();

            //check if NNUnit threw an Exception (During Game)
            switch (result.FaceDetected) {
                case ReturnObject.Type.NoFaceDetected:
                    throw new UserMissingException();
                case ReturnObject.Type.MoreThanOneFaceDetected:
                    throw new MoreThanOneUserException();
                case ReturnObject.Type.Exception:
                    throw new UnknownException();
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
		public ReturnObject.Type TryToRecognizeUser() {
            return nnUnit.CheckUserExist();
		}

        //reset Game
        public void resetGame() {
            this.points = 0;
        }
    }
}
