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

        private Logger logger = new Logger();

        public MainWindowViewModel() {
            this.game = new GameClass();
        }

        //Change Content on MainWindow through UI thread (Only UI thread allowed to change MainWindow Content!!)
        private void updateUserMsg(string msg) {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                //Update Label "UserMessage" Content
                MainWindow.main.UserMessage.Text = msg;
            }));
        }

        //Change Content on MainWindow through UI thread (Only UI thread allowed to change MainWindow Content!!)
        private void updateEmotion(string emotion) {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                //Get generated Emoji and update Emoji Div Content
                MainWindow.main.Emoji.Source = new BitmapImage(new Uri("Emoji/" + emotion + ".png", UriKind.Relative));
                //Set Emoji Name
                MainWindow.main.EmojiName.Content = emotion;
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
                MainWindow.main.EmojiName.Content = null;
				MainWindow.main.Points.Content = null;

                //DebugMode !!TEST
                MainWindow.main.DebugModeOutput.Text = null;
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

        private void getDebugModeOutput(ReturnObject obj) {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                //update Debug Mode Output
                MainWindow.main.DebugModeOutput.Text = "ReturnObject: " + obj.FaceDetected + "\n Emotion: " + obj.Emotion;
            }));
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

                    //Update Debug Mode Output !!Just for Test, afterward DELETE!!!
                    getDebugModeOutput(game.testObj);

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
                    logger.Log("GUI says: No ones there anymore. Stop the game.");
					updateUserMsg("Spiel wurde abgebrochen! Bis zum nächsten mal!");

                    //Update Debug Mode Output !!Just for Test, afterward DELETE!!!
                    getDebugModeOutput(game.testObj);

                    System.Threading.Thread.Sleep(5000);

					//reset game
					resetGame();

                    //exit while loop
                    break;

				} catch (MoreThanOneUserException) {
                    logger.Log("GUI says: More than one face detected.");
                    updateUserMsg("Wir erkennen mehr als eine Person. Die Runde wurde abgebrochen!");

                    //Update Debug Mode Output !!Just for Test, afterward DELETE!!!
                    getDebugModeOutput(game.testObj);

                    System.Threading.Thread.Sleep(5000);

                } catch (UnknownException) {

                    updateUserMsg("Unbekannter Fehler ist aufgetreten! Die Runde wurde abgebrochen!");

                    //Update Debug Mode Output !!Just for Test, afterward DELETE!!!
                    getDebugModeOutput(game.testObj);

                    System.Threading.Thread.Sleep(5000);
                }
			}

			recognizeUser();
		}

		public void recognizeUser(){
			while(true){

                updateUserMsg("Na, Lust eine Runde zu spielen?" + "\n" + "Es darf nur eine Person in der Kamera zu sehen sein..");
				
				System.Threading.Thread.Sleep(1000);

                //send video instance to NNUnit to check if user exist
                ReturnObject.Type recognizedUserType = ReturnObject.Type.Exception;

                Application.Current.Dispatcher.Invoke(new Action(() => {
                    BitmapSource Userimg = BitmapConverter.ConvertToBitmapSource(MainWindow.main.webCameraControl.GetCurrentImage());
                    saveImage(Userimg);
				}));

                System.Threading.Thread.Sleep(100);

                //recognizedUserType = game.TryToRecognizeUser();

                //DebugModeVersion TEST!! DELETE Afterward
                recognizedUserType = game.TryToRecognizeUser().FaceDetected;
                getDebugModeOutput(game.testObj);

                switch (recognizedUserType) {
                    case ReturnObject.Type.FaceDetected:
                        updateUserMsg("Willkommen zum Spiel! Viel Spaß!");
                        run();
                        break;
                    case ReturnObject.Type.Exception:
                        logger.Log("GUI says: ReturnObject threw exception!");
                        updateUserMsg("Ein Fehler ist aufgetreten! Bitte wenden Sie sich an einen Mitarbeiter!");
                        System.Threading.Thread.Sleep(3000);
                        break;
                }
			}
		}
	}
}
