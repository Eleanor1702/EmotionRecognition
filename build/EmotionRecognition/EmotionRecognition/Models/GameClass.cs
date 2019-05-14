using System.ComponentModel;
using System.Runtime.CompilerServices;
using EmotionRecognition.Annotations;
using EmotionRecognition.Services;

namespace EmotionRecognition.Models
{
    public class GameClass : INotifyPropertyChanged
    {
        private RandomEmotionGenerator RandomEmotionGenerator;

        private int points;

        public int Points
        {
            get { return points; }
            set
            {
                if (value == points) return;
                points = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Compares the two emotions (input and generated)
        /// </summary>
        /// <param name="randomEmotion"></param>
        /// <param name="userEmotion"></param>
        /// <returns></returns>
        private int EmotionComparison(object randomEmotion, object userEmotion)
        {
            if (randomEmotion == userEmotion)
                return 10;
            
            else
                return 0;
        }


        /// <summary>
        /// Resets the GameMode
        /// </summary>
        public void ResetGame()
        {
            points = 0;
        }








        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
