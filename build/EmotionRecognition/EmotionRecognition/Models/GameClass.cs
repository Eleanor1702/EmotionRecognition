using System.ComponentModel;
using System.Runtime.CompilerServices;
using EmotionRecognition.Annotations;
using EmotionRecognition.Services;

namespace EmotionRecognition.Models
{
    public class GameClass : INotifyPropertyChanged
    {
        private RandomEmotionGenerator _randomEmotionGenerator;
        public RandomEmotionGenerator RandomEmotionGenerator
        {
            get => _randomEmotionGenerator;
            set
            {
                if (Equals(value, _randomEmotionGenerator)) return;
                _randomEmotionGenerator = value;
                OnPropertyChanged();
            }
        }

        private int _points;
        public int Points
        {
            get { return _points; }
            set
            {
                if (value == _points) return;
                _points = value;
                OnPropertyChanged();
            }
        }

        public GameClass()
        {
            RandomEmotionGenerator = new RandomEmotionGenerator();
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
            _points = 0;
        }

        


        #region PropertyChanged
        /// <summary>
        /// This is our property change functionality.
        /// With that we recognize property changes from the ViewModel in the View
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
