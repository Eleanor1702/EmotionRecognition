namespace EmotionRecognition.Models
{
    public class ReturnObject
    {
        public Type FaceDetected;
        public string Emotion;
        public int Percentage;

        /// <summary>
        /// <param>FaceDetected</param>At least in one pictrue a single face was detected -> Return Object contains detected emotion with percentage
        /// <param>NoFaceDetected</param>Not a single face was detected in all the supplied pictures -> Return Object contains "No detected emotion" with zero percentage
        /// <param>MoreThanOneFaceDetected</param>In one of the given pictrues more than one face was detected -> Return Object contains "No detected emotion" with zero percentage
        /// <param>Exception</param>NN failed -> Return Object contains "Exception thrown!" with zero percentage
        /// </summary>
        public enum Type { FaceDetected, NoFaceDetected, MoreThanOneFaceDetected, Exception };
        

        public ReturnObject(string i_Emotion, int i_Percentage, Type iEmoDetected)
        {
            FaceDetected = iEmoDetected;
            if (FaceDetected == Type.FaceDetected)
            {
                Emotion = i_Emotion;
                Percentage = i_Percentage;
            }
            else
            {
                Emotion = "No Emotion Detected!";
                Percentage = 0;
            }
        }

        public bool getFaceDetected() {
            switch (FaceDetected)
            {
                case Type.FaceDetected: return true;

                default:
                    return false;
            }

            
        }
    }
}
