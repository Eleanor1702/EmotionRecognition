using NUnit.Framework;
using System;
using EmotionRecognition.Models;

namespace NunitTests
{
    [TestFixture()]
    public class ReturnObjectTest
    {
        private ReturnObject actual_Object;


        [SetUp]
        public void SetUp()
        {

        }
        //Test, if ReturnObject changes inputs.
        [Test()]
        [TestCase("Happy", 83, ReturnObject.Type.FaceDetected, "Happy", 83, ReturnObject.Type.FaceDetected)]
        [TestCase("Disgust", 83, ReturnObject.Type.FaceDetected, "Disgust", 83, ReturnObject.Type.FaceDetected)]
        [TestCase("Fear", 83, ReturnObject.Type.FaceDetected, "Fear", 83, ReturnObject.Type.FaceDetected)]
        [TestCase("Neutral", 40, ReturnObject.Type.FaceDetected, "Neutral", 40, ReturnObject.Type.FaceDetected)]
        [TestCase("Surprised", 83, ReturnObject.Type.FaceDetected, "Surprised", 83, ReturnObject.Type.FaceDetected)]
        [TestCase("Test", int.MinValue, ReturnObject.Type.FaceDetected, "Test", int.MinValue, ReturnObject.Type.FaceDetected)]
        public void TestConstructorCorrectValues(string expected_emotion, int expected_percentage, ReturnObject.Type expected_enum, string actual_emotion, int actual_percentage, ReturnObject.Type actual_enum)
        {
            //Arrange
            //Arrangement of the Test is made in the TestCase-Attribute above.
            //Act
            actual_Object = new ReturnObject(actual_emotion, actual_percentage, actual_enum);

            //Assert
            Assert.AreEqual(expected_emotion, actual_Object.Emotion);
            Assert.AreEqual(expected_percentage, actual_Object.Percentage);
            Assert.AreEqual(expected_enum, actual_Object.FaceDetected);
        }

        //Test the Errors
        [Test()]
        [TestCase("Happy", 83, ReturnObject.Type.MoreThanOneFaceDetected, "Happy", 83, ReturnObject.Type.FaceDetected)]//should always fail
        [TestCase("No Emotion Detected!", 0, ReturnObject.Type.NoFaceDetected, "No Emotion Detected", 0, ReturnObject.Type.NoFaceDetected)]
        [TestCase("Fear", 83, ReturnObject.Type.FaceDetected, "Fear", 83, ReturnObject.Type.FaceDetected)]
        [TestCase("Neutral", 40, ReturnObject.Type.FaceDetected, "Neutral", 40, ReturnObject.Type.NoFaceDetected)]//should always fail
        [TestCase("No Emotion Detected!", 0, ReturnObject.Type.MoreThanOneFaceDetected, "Surprised", 20, ReturnObject.Type.MoreThanOneFaceDetected)]
        public void TestConstructorIncorrectValues(string expected_emotion, int expected_percentage, ReturnObject.Type expected_enum, string actual_emotion, int actual_percentage, ReturnObject.Type actual_enum)
        {
            //Arrange
            //Arrangement of the Test is made in the TestCase-Attribute above.
            //Act
            actual_Object = new ReturnObject(actual_emotion, actual_percentage, actual_enum);

            //Assert
            Assert.AreEqual(expected_emotion, actual_Object.Emotion);
            Assert.AreEqual(expected_percentage, actual_Object.Percentage);
            Assert.AreEqual(expected_enum, actual_Object.FaceDetected);
        }

    }
}
