using EmotionRecognition.Models;

namespace EmotionRecognition.Services {

    /// <summary> This Class presents the NN Unit. The Code presented here is just an example </summary>

    class NNUnit {

        /// <summary> This Function should take an Image as Parameter from Video Class </summary>
        public NNResult analyse() {
            NNResult tst = new NNResult("Laughing", 80, true);
            return tst;
        }
    }
}
