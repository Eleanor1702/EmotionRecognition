using EmotionRecognition.Services;

namespace EmotionRecognition.Models {

    public class GameClass {

        private NNUnit nnUnit;
        private int points;

        public GameClass() {
            this.nnUnit = new NNUnit();
            this.points = 0;
        }

        /// <summary> Get and Set Points </summary>
        public int Points {
            get { return points; }
        }

        //for the sake of the example the parameters types are string
        public bool CompareEmotion(string randomEmotion) {
            //For example calling the func in NN Unit for analysing
            //NN analyse throws an object of NNResult where for example(EmotionName, percentage or points) are saved.
            NNResult result = nnUnit.analyse();

            //check if user DOESNT exist
            if(result.UserExist == false) {
                throw new UserMissingException();
            }
            //Compare our randomEmotion and the result
            if (randomEmotion == result.EmotionName) {
                //save points
                points += result.Percentage;

                //true for SUCCESS
                return true;
            }

            //false for FAILURE
            return false;
        }


        /// <summary> Resets the GameMode </summary>
        public void ResetGame() {

            //save user & points to Leaderboard when available

            //reset points
            points = 0;

            //show leaderboard for 10 seconds maybe?
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
