using EmotionRecognition.ViewModels;
using System;
using System.Windows;

namespace EmotionRecognition {

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
    }
}
