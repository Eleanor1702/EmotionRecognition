namespace EmotionRecognition.Models
{
    public class ReturnObject
    {
        public bool FaceDetected;
        public string Emotion;
        public int Percentage;

        public ReturnObject(string i_Emotion, int i_Percentage, bool iEmoDetected)
        {
            FaceDetected = iEmoDetected;
            if (iEmoDetected)
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
            return FaceDetected;
        }
    }
}
