using NUnit.Framework;
using System.Diagnostics;
using EmotionRecognition.Models;
using System;
using System.IO;

namespace NunitTests
{
    class Program
    {
        static void Main()
        {
            PrepareModelTests test = new PrepareModelTests();
        }

    }
    [TestFixture()]
    public class PrepareModelTests
    {
        private PrepareModel actual_prepareModel;
        private string expected_PathToScript;
        private string expected_PathToPicture;
        private Process expected_Process;

        [SetUp]
        public void SetUp()
        {
            expected_Process = new Process();
            expected_Process.StartInfo.FileName = "Python.exe";
            expected_Process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            expected_Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            expected_Process.StartInfo.UseShellExecute = false;
            expected_Process.StartInfo.RedirectStandardOutput = true;
            
        }
        [TearDown]
        public void TearDown()
        {
            Test
        }

        //Da der Konstruktor von PrepareModel vor allem aus der Deklaration des verwendeten Prozesses besteht,
        //wird nur getestet, ob der Prozess wie erwartet vorbereitet wird.
        [Test]
        public void TestConstructor()
        {
            //Arrange
            expected_PathToPicture = "Images/";
            expected_PathToScript = "camera_roll.py";
            
            //Act
            ProcessStartInfo expected_Info = expected_Process.StartInfo;
            expected_Process.StartInfo.Arguments = expected_PathToScript + " " + expected_PathToPicture;
            actual_prepareModel = new PrepareModel();
            ProcessStartInfo actual_ProcessInfo = actual_prepareModel.neuralNetProcess.StartInfo;

            //Assert
            Assert.AreEqual(expected_Info.Arguments , actual_ProcessInfo.Arguments); //Only Addition made by User.
        }

        //Test, ob auch wirklich das ReturnObject zurückgegeben wird. ReturnObject selbst wird nicht getestet.
        [Test]
        public void GetReturnObjectTest()
        {
            //Arrange
            string emotion = "Surprised";
            int percentage = 60;
            ReturnObject.Type enumType = ReturnObject.Type.FaceDetected;
            actual_prepareModel = new PrepareModel();

            //Act
            ReturnObject expected = actual_prepareModel.returnObject;
            //Change the returnObject in actual_prepareModel
            actual_prepareModel.returnObject = new ReturnObject(emotion, percentage, enumType);
            ReturnObject actual = actual_prepareModel.GetReturnObject();
            
            //Assert
            Assert.AreNotEqual(expected.Emotion, actual.Emotion);
            Assert.AreNotEqual(expected.Percentage, actual.Percentage);
            Assert.AreNotEqual(expected.FaceDetected, actual.FaceDetected);
            
        }

        //Test, ob der ETI gestartet wird. Nicht nutzbar auf MacOS.
        //[Test]
        //public void StartETITest()
        //{
        //    //Arrange
        //    actual_prepareModel = new PrepareModel();

        //    //Act
        //    actual_prepareModel.StartETI();
        //    int i = 0;
        //    while(actual_prepareModel.interpreter == null)
        //    { i++; }
        //    //Assert
        //    Assert.IsNotNull(actual_prepareModel.interpreter);
        //    Assert.GreaterOrEqual(10000000, i);
        //}
    }
}
