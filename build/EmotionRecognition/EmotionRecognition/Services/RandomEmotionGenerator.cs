using System;
using EmotionRecognition.Data;

namespace EmotionRecognition.Services
{
    public class RandomEmotionGenerator
    {
        private EmotionEnum _actualEmotion;

        public RandomEmotionGenerator()
        {
            //Initial emotion creating
            CreateRandomEmotion();
        }

        /// <summary>
        /// Creates a random Emotion
        /// </summary>
        /// <returns></returns>
        public void CreateRandomEmotion()
        {
            Random random = new Random();
            int enumCount = Enum.GetNames(typeof(EmotionEnum)).Length;
            int randomNumber = random.Next(0, enumCount+1);

            _actualEmotion = (EmotionEnum) randomNumber;
        }

        /// <summary>
        /// Gets the actual random emotion
        /// For new random emotion call CreateRandomEmotion
        /// </summary>
        /// <returns></returns>
        public EmotionEnum GetActualRandomEmotion() => _actualEmotion;


    }
}
