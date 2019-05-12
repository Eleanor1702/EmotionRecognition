namespace EmotionRecognition.Models {

    /// <summary> This Class presents the result of NN Unit </summary>

    class NNResult {
        private string emotionName;
        private int percentage;
        private bool userExist;

        public NNResult(string name, int percent, bool exist) {
            this.emotionName = name;
            this.percentage = percent;
            this.userExist = exist;
        }

        public string EmotionName {
            get { return this.emotionName; }
        }

        public int Percentage {
            get { return this.percentage; }
        }

        //This field helps handling an Exception later during the game
        public bool UserExist {
            get { return this.userExist; }
        }
    }
}
