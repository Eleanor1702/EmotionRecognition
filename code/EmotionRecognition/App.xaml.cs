using System;
using System.Windows;

namespace EmotionRecognition {

    //Allow DebugMode through CMD argument
    static class global {
        public static bool Debug = false;
    }

    /// <summary> Interaction logic for App.xaml </summary>
    public partial class App : Application {

        //Main Function
        [STAThread]
        public static void Main(string[] args) {

            if (args.Length > 0) {
                if (args[0] == "--Debug") {
                    //Set to true
                    global.Debug = true;
                }
            }

            var application = new App();
            application.InitializeComponent();
            application.Run(); 
        }
    }   
}
