using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Environment;

namespace EmotionRecognition.Models
{
    public class EmotionTableInterpreter
    {
        public class EmoCollValue
        {
            public float emoWeight;
            public int detecCnt;
            public int finalPercentage;

            public EmoCollValue(float i_emoWeight, int i_detecCnt, int i_finalPercentage = 0)
            {
                emoWeight = i_emoWeight;
                detecCnt = i_detecCnt;
                finalPercentage = i_finalPercentage;
            }

            public void incDetecCnt()
            {
                this.detecCnt++;
            }
        }

        private const string FACES_FOUND_COUNT = "FacesFoundCount";
        private const string SINGLE_PIC_OUTPUT = "SinglePicOutput";
        private const string HIGHEST_EMO_INDEX = "HighestEmotionIndex";
        private const string EMO_WEIGHT_ARRAY = "EmotionWeightArray";
        private const string NO_EMO_DETECED = "No Emotion Deteced!";

        private string[] EmotionDef = { "Angry", "Disgust", "Fear", "Happy", "Sad", "Surprise", "Neutral" };
        private const string patternArray = @"(?<" + SINGLE_PIC_OUTPUT + @">\(ar+ay\(\[" +
                                                "(?<" + HIGHEST_EMO_INDEX + @">[0-6])([,][ ]*[0-6])*\][, dtype=int[32|64]*]*\)[,][ ]*" +
                                             "(?<" + EMO_WEIGHT_ARRAY + @">ar+ay\(\["+
                                                "(?<" + EMO_WEIGHT_ARRAY + @"0>[0-9]*[.]*[0-9]*[e]*[-|+]*[0-9]*)[,][ ]*" +
                                                "(?<" + EMO_WEIGHT_ARRAY + @"1>[0-9]*[.]*[0-9]*[e]*[-|+]*[0-9]*)[,][ ]*" +
                                                "(?<" + EMO_WEIGHT_ARRAY + @"2>[0-9]*[.]*[0-9]*[e]*[-|+]*[0-9]*)[,][ ]*" +
                                                "(?<" + EMO_WEIGHT_ARRAY + @"3>[0-9]*[.]*[0-9]*[e]*[-|+]*[0-9]*)[,][ ]*" +
                                                "(?<" + EMO_WEIGHT_ARRAY + @"4>[0-9]*[.]*[0-9]*[e]*[-|+]*[0-9]*)[,][ ]*" +
                                                "(?<" + EMO_WEIGHT_ARRAY + @"5>[0-9]*[.]*[0-9]*[e]*[-|+]*[0-9]*)[,][ ]*" +
                                                "(?<" + EMO_WEIGHT_ARRAY + @"6>[0-9]*[.]*[0-9]*[e]*[-|+]*[0-9]*)))";
        private const string patternFaceFoundCount = @"(Faces found:  (?<" + FACES_FOUND_COUNT + @">[0-9]*)){1}";

        private Dictionary<string, EmoCollValue> hdEmoCollection = new Dictionary<string, EmoCollValue>(); // heighest deteced Emotions Collection

        private int rxOutHeighestEmoIdx;
        private float rxOutHeighestEmoWeight;

        private string netOutput;
        private bool severalFacesFound = false;
        private bool noFacesFound = false;
        private int noFaceFoundCnt = 0;

        private Match a_match;
        private MatchCollection a_matchCollection;

        public EmotionTableInterpreter(Process neuralNetProcess, ref ReturnObject i_returnObject)
        {
            neuralNetProcess.Start();
            netOutput = neuralNetProcess.StandardOutput.ReadToEnd();
            //1. Versuch
            netOutput = netOutput.Replace(Environment.NewLine, "");
            //2. Versuch
            //netOutput = Regex.Replace(netOutput, @"\r\n?|\n", "");
			netOutput = netOutput.Replace(@"\n", "");
			netOutput = netOutput.Replace(@"\r", "");
			netOutput = netOutput.Replace(@"\t", "");
			netOutput = netOutput.Replace(@"\v", "");
			netOutput = netOutput.Replace("@\f", "");
            a_matchCollection = Regex.Matches(netOutput, patternFaceFoundCount);
            if (a_matchCollection.Count > 0)
            {
                foreach (Match singlePic in a_matchCollection)
                {
                    if (Convert.ToInt64(singlePic.Groups[FACES_FOUND_COUNT].Value) > 1)
                    {
                        severalFacesFound = true;
                        break;
                    }
                    
                    if(Convert.ToInt64(singlePic.Groups[FACES_FOUND_COUNT].Value) == 0)
                    {
                        noFacesFound = true;
                        noFaceFoundCnt++;
                    }
                }
            }
            if (!severalFacesFound)
            {
                if (a_matchCollection.Count > noFaceFoundCnt)
                {
                    a_matchCollection = Regex.Matches(netOutput, patternArray);
                    if (a_matchCollection.Count > 0)
                    {
                        foreach (Match singlePic in a_matchCollection)
                        {
                            Evaluate(singlePic.Groups[SINGLE_PIC_OUTPUT].Value);
                        }
                    }
                    KeyValuePair<string, EmoCollValue> resultEmo = GetEmotion();
                    //Console.WriteLine("Deteced Emotion:\n" +
                    //    "\t" + resultEmo.Key + " | " + resultEmo.Value.finalPercentage + "%");
                    i_returnObject = new ReturnObject(resultEmo.Key, resultEmo.Value.finalPercentage, ReturnObject.Type.FaceDetected);
                } else if(/*noFacesFound &&*/ a_matchCollection.Count <= noFaceFoundCnt)
                {
                    i_returnObject = new ReturnObject(NO_EMO_DETECED, 0, ReturnObject.Type.NoFaceDetected);
                }
            } else if(severalFacesFound)
            {
                i_returnObject = new ReturnObject(NO_EMO_DETECED, 0, ReturnObject.Type.MoreThanOneFaceDetected);
            }
            
        }

        private KeyValuePair<string, EmoCollValue> GetEmotion()
        {
            KeyValuePair<string, EmoCollValue> maxEmo = new KeyValuePair<string, EmoCollValue>();
            int sumDetecCnt = 0;
            if (hdEmoCollection.Count == 1)
            {
                maxEmo = hdEmoCollection.ElementAt(0);
                sumDetecCnt = maxEmo.Value.detecCnt;
            }
            else if (hdEmoCollection.Count > 1)
            {
                maxEmo = hdEmoCollection.ElementAt(0);
                foreach (KeyValuePair<string, EmoCollValue> emoEle in hdEmoCollection)
                {
                    sumDetecCnt += emoEle.Value.detecCnt;
                    if (emoEle.Value.detecCnt >= maxEmo.Value.detecCnt)
                    {// emoEle wurden gleich oft oder öfter als maxEmo erkannt
                        if (emoEle.Value.detecCnt == maxEmo.Value.detecCnt)
                        {// emoEle wurde gleich oft wie maxEmo erkannt
                            if (emoEle.Value.emoWeight >= maxEmo.Value.emoWeight)
                            {// emoEle hat die gleiche oder eine höhere Gewichtung als maxEmo
                                if (emoEle.Value.emoWeight > maxEmo.Value.emoWeight)
                                {// emoEle hat eine höhere Gewichtung als maxEmo
                                    maxEmo = emoEle; // emoEle wird zum neuen maxEmo
                                }
                                else
                                {
                                    if (maxEmo.Key == EmotionDef.ElementAt(6) && emoEle.Key != EmotionDef.ElementAt(6))
                                    {// maxEmo ist "Neutral" und emoEle ist nicht "Neutral"
                                        maxEmo = emoEle; // emoEle wird zum neuen maxEmo
                                    }
                                }
                            }
                        }
                        else
                        {
                            maxEmo = emoEle;
                        }
                    }
                }
            }
            else
            {
                maxEmo = new KeyValuePair<string, EmoCollValue>(NO_EMO_DETECED, new EmoCollValue(0, 0));
            }

            if (!maxEmo.Key.Equals(NO_EMO_DETECED))
            {
                KeyValuePair<string, EmoCollValue> emoReturn = new KeyValuePair<string, EmoCollValue>(maxEmo.Key, new EmoCollValue(0, 0, maxEmo.Value.detecCnt * 100 / sumDetecCnt));
                maxEmo = emoReturn;
            }

            return (maxEmo);
        }

        private float TransformE(string i_val)
        {
            int eVal = i_val.IndexOf('e', 0, i_val.Length - 1);
            if (eVal != -1)
            {
                string[] weight = i_val.Split('e');
                double emoWeightDouble = float.Parse(weight[0], CultureInfo.InvariantCulture.NumberFormat) * Math.Pow(10, float.Parse(weight[1]));
                return ((float)emoWeightDouble);
            }
            return (float.Parse(i_val, CultureInfo.InvariantCulture.NumberFormat));
        }

        private void Evaluate(string singlePicOutput)
        {
            a_match = Regex.Match(singlePicOutput, patternArray);

            if (a_match.Success)
            {
                int.TryParse(a_match.Groups[HIGHEST_EMO_INDEX].Value, out rxOutHeighestEmoIdx);
                rxOutHeighestEmoWeight = TransformE(a_match.Groups[EMO_WEIGHT_ARRAY + rxOutHeighestEmoIdx.ToString()].Value);
            }

            if (hdEmoCollection.ContainsKey(EmotionDef.ElementAt(rxOutHeighestEmoIdx)))
            {
                hdEmoCollection[EmotionDef.ElementAt(rxOutHeighestEmoIdx)].emoWeight += rxOutHeighestEmoWeight;
                hdEmoCollection[EmotionDef.ElementAt(rxOutHeighestEmoIdx)].incDetecCnt();
            }
            else
            {
                EmoCollValue val = new EmoCollValue(rxOutHeighestEmoWeight, 1);
                hdEmoCollection.Add(EmotionDef.ElementAt(rxOutHeighestEmoIdx), val);
            }
        }
    }
}
