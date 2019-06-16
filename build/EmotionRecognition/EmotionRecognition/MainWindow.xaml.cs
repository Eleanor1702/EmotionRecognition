using EmotionRecognition.ViewModels;
using System.Windows;
using WebEye.Controls.Wpf;
using System.Collections.Generic;
using System.Threading;
using System;

namespace EmotionRecognition
{

	/// <summary> NO PROCESS RELATED CODE ALLOWED </summary>
	public partial class MainWindow : Window {

        //allows MainWindow to be edited from other Classes, in our Case (MainProcessor)
        public static MainWindow main;

        public MainWindow() {
			
            //Initialize View (GUI) Components
            InitializeComponent();

			//Creating a thread to allow initializing Game logic at the same time as UI Component (this thread is only responsible for logic!!)
			//IsBackground = true makes sure that thread is terminated when Window is also terminated
			System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Start)) { IsBackground = true };
            thread.Start();

            main = this;
		}

        //This Function initializes our Controller (Mainprocessor) to start the System
        public void Start() {
            MainWindowViewModel MainProcessor = new MainWindowViewModel();

            MainProcessor.run();
        }

		//This function is called in MainWindom.xaml while loading to get first recognized device as Camera
		//The Camera Control which we are using is from here: https://www.codeproject.com/Articles/330177/Yet-another-Web-Camera-control
		private void OnWindowLoaded(object sender, RoutedEventArgs e) {
			var cameras = new List<WebCameraId>(webCameraControl.GetVideoCaptureDevices());
			webCameraControl.StartCapture(cameras[0]);
		}
	}
}
