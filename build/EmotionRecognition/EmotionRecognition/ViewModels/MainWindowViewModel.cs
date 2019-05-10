using System.ComponentModel;
using System.Runtime.CompilerServices;
using EmotionRecognition.Annotations;
using EmotionRecognition.Models;

namespace EmotionRecognition.ViewModels
{
    //This is our Controller/ViewModel
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private GameClass _gameClass;
        public GameClass GameClass
        {
            get => _gameClass;
            set
            {
                if (Equals(value, _gameClass)) return;
                _gameClass = value;
                OnPropertyChanged();
            }
        }

        private VideoClass _videoClass;
        public VideoClass VideoClass
        {
            get => _videoClass;
            set
            {
                if (Equals(value, _videoClass)) return;
                _videoClass = value;
                OnPropertyChanged();
            }
        }


        public MainWindowViewModel()
        {
            GameClass = new GameClass();
            VideoClass = new VideoClass();

            
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
