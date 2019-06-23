using System;
using System.Windows;

namespace EmotionRecognition {

    /// <summary> Interaction logic for App.xaml </summary>
    public partial class App : Application {

        //Main Function
        [STAThread]
        public static void Main() {
            var application = new App();
            application.InitializeComponent();
            application.Run(); 
        }
    }   
}
