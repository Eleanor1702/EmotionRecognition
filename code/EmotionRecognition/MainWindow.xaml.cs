using EmotionRecognition.ViewModels;
using System.Windows;
using WebEye.Controls.Wpf;
using System.Collections.Generic;
using System;

namespace EmotionRecognition {

    /// <summary> NO PROCESS RELATED CODE ALLOWED </summary>
    public partial class MainWindow : Window {

        //allows MainWindow to be edited from other Classes, in our Case (MainProcessor)
        public static MainWindow main;

        public MainWindow() {
			
            //Initialize View (GUI) Components
            InitializeComponent();

            if (global.Debug) {
                DebugModeLabel.Content = "Debug Mode Output";
            }

			//Creating a thread to allow initializing Game logic at the same time as UI Component (this thread is only responsible for logic!!)
			//IsBackground = true makes sure that thread is terminated when Window is also terminated
			System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Start)) { IsBackground = true };
            thread.Start();

            System.Threading.Thread carouselThread = new System.Threading.Thread(new System.Threading.ThreadStart(rotateTipps)) { IsBackground = true };
            carouselThread.Start();
            main = this;
		}

        //This Function initializes our Controller (Mainprocessor) to start the System
        public void Start() {
            MainWindowViewModel MainProcessor = new MainWindowViewModel();
			
			//start actual program
            MainProcessor.recognizeUser();
        }

        public void rotateTipps() {
            int currentTipp = 0;
            string[] tipps = {"Falls du eine Brille trägst, nimm diese bitte ab!", "Komm nah ran, die Kamera beißt nicht!",
                              "Stell sicher, dass du der einzige bist, der zu sehen ist!", "Drück die Emotion so deutlich wie möglich aus!"};

            while (true) {
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    //Update Label "UserMessage" Content
                    Tipps.Text = tipps[currentTipp];
                }));

                System.Threading.Thread.Sleep(8000);

                currentTipp = (currentTipp + 1) % tipps.Length;
            }
        }

		//This function is called in MainWindom.xaml while loading to get first recognized device as Camera
		//The Camera Control which we are using is from here: https://www.codeproject.com/Articles/330177/Yet-another-Web-Camera-control
		private void OnWindowLoaded(object sender, RoutedEventArgs e) {
			var cameras = new List<WebCameraId>(webCameraControl.GetVideoCaptureDevices());
			webCameraControl.StartCapture(cameras[0]);
		}
	}
}
