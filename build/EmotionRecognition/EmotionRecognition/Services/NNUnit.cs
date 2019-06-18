using EmotionRecognition.Models;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace EmotionRecognition.Services {

    //This Class presents the NN Unit. The Code presented here is just an example.

    public class NNUnit {

		public ReturnObject analyse(BitmapSource img) {

            //save bitmap pic in folder
            saveImage(img);

            //Call: EmotionRecognition_NNProcessComb.exe camera_roll.py Images/
            PrepareModel prepareModel = new PrepareModel();
            ReturnObject returnObject = prepareModel.GetReturnObject();

            return returnObject;
        }

        //This Function should be called to check if User exist... And handle exceptions
		public ReturnObject.Type CheckUserExist(BitmapSource bitPic) {

            return analyse(bitPic).FaceDetected;
		}

        //sava image in Folder to be used
        private void saveImage(BitmapSource bitPic) {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitPic));
            encoder.QualityLevel = 100;

            //relative Path
            string filepath = "Images\\capture.jpg";
            FileStream fstream = new FileStream(filepath, FileMode.Create);
            encoder.Save(fstream);
            fstream.Close();
        }
    }
}
