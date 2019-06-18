using System;

namespace EmotionRecognition.Services {

	public class RandomEmotionGenerator {



		private static string[] emotions = new string[] { "Angry", "Disgust", "Fear", "Happy", "Neutral", "Sad", "Surprise" };

		//return a random Emotion
        public static string generate() {
			Random random = new Random();
			int index = random.Next(0, emotions.Length);
			
			return emotions[index];
		}
    }
}
