using System;
using System.Windows;
using System.Windows.Media.Imaging;
using EmotionRecognition.Models;
using EmotionRecognition.Services;

namespace EmotionRecognition.ViewModels {

	//This is our Controller/ViewModel
	public class MainWindowViewModel {

        private GameClass game;

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

		//clear Content of Emoji and Points after Game end
		private void resetGame() {
			Application.Current.Dispatcher.Invoke(new Action(() => {
				MainWindow.main.Emoji.Source = null;
				MainWindow.main.Points.Content = null;
			}));
		}

        //Main function of Game Logic
        public void run(bool userExist) {

			//Main Loop while User Exist
			while(userExist) {

				System.Threading.Thread.Sleep(3000);
				updateUserMsg("Versuchen Sie diesen Emoji nachzumachen");

				//Random Emotion Generator
				string emotion = RandomEmotionGenerator.generate();
				updateEmotion(emotion);

				System.Threading.Thread.Sleep(3000);

				//Announce registering the emotion of user in 5 seconds
				for (int i = 5; i > 0; i--)
				{
					updateUserMsg("Emotion wird analysiert in: " + i);
					System.Threading.Thread.Sleep(1000);
				}

				//capture Image
				BitmapSource img = null;
				Application.Current.Dispatcher.Invoke(new Action(() => {
					img = BitmapConverter.ConvertToBitmapSource(MainWindow.main.webCameraControl.GetCurrentImage());
				}));

				//Wait message while processing
				updateUserMsg("Bild wird analysiert...");

				//this try and catch serves handling the exception of a missing user during Gameplay
				try
				{
					//Get random generated Emoji from Emotiongenerator in Services. Usually captured Image should be passed here as well
					bool result = game.CompareEmotion(img, emotion);

					System.Threading.Thread.Sleep(3000);

					//Rate the results and update UI
					if (result == true)
					{
						updateUserMsg("Jawohl! Gut gemacht!");
						refreshPoints();
						System.Threading.Thread.Sleep(2000);
					}
					else
					{
						updateUserMsg("Leider falsch! Nächster Versuch..");
						refreshPoints();
						System.Threading.Thread.Sleep(2000);
					}

				}
				catch (UserMissingException)
				{

					updateUserMsg("Spiel wurde abgebrochen! Bis zum nächsten mal!");
					System.Threading.Thread.Sleep(5000);

					//reset game
					resetGame();

					//call main Loop to recognize user
					userExist = false;
				}
			}

			recognizeUser();
		}

		public void recognizeUser(){
			while(true){

				updateUserMsg("Gib uns 5 sekunden um dich zu erkenenn");
				System.Threading.Thread.Sleep(5000);

				//send video instance to GameStart to check if user Exist
				bool userExist = false;
				Application.Current.Dispatcher.Invoke(new Action(() => {
					userExist = game.StartGame(BitmapConverter.ConvertToBitmapSource(MainWindow.main.webCameraControl.GetCurrentImage()));
				}));

				if (userExist){
					updateUserMsg("Willkommen zum Spiel! Viel Spaß!");
					run(userExist);
				}
			}
		}
	}
}
