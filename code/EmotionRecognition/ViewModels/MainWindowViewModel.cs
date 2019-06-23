using System;
using System.IO;
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
                MainWindow.main.Emoji.Source = new BitmapImage(new Uri("Emoji/" + emotion + ".png", UriKind.Relative));
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

            //reset points in game
            this.game.resetGame();
		}

        //sava image in Folder to be used
        private void saveImage(BitmapSource bitPic) {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitPic));
            encoder.QualityLevel = 100;

            //relative Path
            string filepath = @"..\..\Images\capture.jpg";
            FileStream fstream = new FileStream(filepath, FileMode.Create);
            encoder.Save(fstream);
            fstream.Close();
        }

        //Main function of Game Logic
        public void run() {

			//Main Loop
			while(true) {

				System.Threading.Thread.Sleep(3000);
				updateUserMsg("Versuchen Sie diesen Emoji nachzumachen");

				//Random Emotion Generator
				string emotion = RandomEmotionGenerator.generate();
				updateEmotion(emotion);

				System.Threading.Thread.Sleep(3000);

				//Announce registering the emotion of user in 5 seconds
				for (int i = 5; i > 0; i--) {
					updateUserMsg("Emotion wird analysiert in: " + i);
					System.Threading.Thread.Sleep(1000);
				}

				//capture Image + save
				BitmapSource img = null;
				Application.Current.Dispatcher.Invoke(new Action(() => {
					img = BitmapConverter.ConvertToBitmapSource(MainWindow.main.webCameraControl.GetCurrentImage());
                    saveImage(img);
                }));

                System.Threading.Thread.Sleep(100);

                //Wait message while processing
                updateUserMsg("Bild wird analysiert...");

				//this try and catch serves handling the exception of a missing user during Gameplay
				try {
                    //Get random generated Emoji from Emotiongenerator in Services. Usually captured Image should be passed here as well
                    bool result = game.CompareEmotion(emotion);

                    //Rate the results and update UI
                    if (result) {
						updateUserMsg("Jawohl! Gut gemacht!");
						refreshPoints();
						System.Threading.Thread.Sleep(2000);
					} else {
						updateUserMsg("Leider falsch! Nächster Versuch..");
						refreshPoints();
						System.Threading.Thread.Sleep(2000);
					}

				} catch (UserMissingException) {

					updateUserMsg("Spiel wurde abgebrochen! Bis zum nächsten mal!");
					System.Threading.Thread.Sleep(5000);

					//reset game
					resetGame();

                    //exit while loop
                    break;

				} catch (MoreThanOneUserException) {

                    updateUserMsg("Wir erkennen mehr als eine Person. Die Runde wurde abgebrochen!");
                    System.Threading.Thread.Sleep(5000);

                } catch (UnknownException) {

                    updateUserMsg("Unbekannter Fehler ist aufgetreten! Die Runde wurde abgebrochen!");
                    System.Threading.Thread.Sleep(5000);

                }
			}

			recognizeUser();
		}

		public void recognizeUser(){
			while(true){

				updateUserMsg("Gib uns paar sekunden um dich zu erkennen");
				System.Threading.Thread.Sleep(3000);

                //send video instance to NNUnit to check if user exist
                ReturnObject.Type recognizedUserType = ReturnObject.Type.Exception;

                Application.Current.Dispatcher.Invoke(new Action(() => {
                    BitmapSource Userimg = BitmapConverter.ConvertToBitmapSource(MainWindow.main.webCameraControl.GetCurrentImage());
                    saveImage(Userimg);
				}));

                System.Threading.Thread.Sleep(100);
                updateUserMsg("Bild wird analysiert....");

                recognizedUserType = game.TryToRecognizeUser();

                switch (recognizedUserType) {
                    case ReturnObject.Type.FaceDetected:
                        updateUserMsg("Willkommen zum Spiel! Viel Spaß!");
                        run();
                        break;
                    case ReturnObject.Type.MoreThanOneFaceDetected:
                        updateUserMsg("Wir erkennen mehr als eine Person. Das Spiel konnte nicht gestartet werden!");
                        break;
                    case ReturnObject.Type.Exception:
                        updateUserMsg("Ein Fehler ist aufgetreten! Bitte wenden Sie sich an einen Mitarbeiter!");
                        break;
                    case ReturnObject.Type.NoFaceDetected:
                        updateUserMsg("Kein Gesicht ist erkannt. Lass uns nochmal probieren!");
                        System.Threading.Thread.Sleep(3000);
                        break;
                }
			}
		}
	}
}
