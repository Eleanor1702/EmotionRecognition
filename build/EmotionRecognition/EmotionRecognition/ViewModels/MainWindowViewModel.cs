using System;
using System.Windows;
using System.Windows.Media.Imaging;
using EmotionRecognition.Models;

namespace EmotionRecognition.ViewModels {

    //This is our Controller/ViewModel
    public class MainWindowViewModel {

        private GameClass game;
        private VideoClass videoClass;

        public MainWindowViewModel() {
            this.game = new GameClass();
        }

        //Change Content on MainWindow through UI thread (Only UI thread allowed to change MainWindow Content!!)
        private void updateUserMsg(string msg) {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                //Update Label "UserMessage" Content
                MainWindow.main.UserMessage.Content = msg;
            }));
        }

        //Change Content on MainWindow through UI thread (Only UI thread allowed to change MainWindow Content!!)
        private void updateEmotion(string emotion) {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                //Get generated Emoji and update Emoji Div Content
                MainWindow.main.Emoji.Source = new BitmapImage(new Uri("Images/" + emotion + ".png", UriKind.Relative));
            }));
        }

        //Change Content on MainWindow through UI thread (Only UI thread allowed to change MainWindow Content!!)
        private void refreshPoints() {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                //update Points
                MainWindow.main.Points.Content = game.Points;
            }));
        }

        //Main function of Game Logic
        public void run() {
            //Will be a HUGE while loop

            //Random Emotion Generator in a simple way
            string[] emotions = new string[] { "Laughing", "Thinking" };
            Random random = new Random();
            int index = random.Next(0, emotions.Length);
            string emotion = emotions[index];

            //wait for 2 Seconds to recognize User
            System.Threading.Thread.Sleep(2000);

            updateUserMsg("Willkommen zum Spiel! Viel Spaß!");

            System.Threading.Thread.Sleep(2000);

            updateEmotion(emotion);
            updateUserMsg("Versuchen Sie diesen Emoji nachzumachen");

            System.Threading.Thread.Sleep(2000);

            //Announce registering the emotion of user in 5 seconds
            for (int i = 5; i > 0; i--) {
                updateUserMsg("Emotion wird analysiert in: " + i);

                System.Threading.Thread.Sleep(1000);
            }

            //Wait message while processing
            updateUserMsg("Bild wird analysiert...");

            //this try and catch serves handling the exception of a missing user during Gameplay
            try {
                //Get random generated Emoji from Emotiongenerator in Services. Usually captured Image should be passed here as well
                bool result = game.CompareEmotion(emotion);

                System.Threading.Thread.Sleep(3000);

                //Rate the results and update UI
                if (result == true) {
                    updateUserMsg("Jawohl! Gut gemacht!");
                    refreshPoints();
                } else {
                    updateUserMsg("Leider falsch! Nächster Versuch..");
                    refreshPoints();
                }

            } catch(UserMissingException) {
                updateUserMsg("Spiel wurde abgebrochen!");

                //reset game
                game.ResetGame();

                //reset points
                refreshPoints();
            }
        }
    }
}
