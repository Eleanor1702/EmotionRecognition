#!/usr/bin/env python
import cv2
import numpy as np
from os import listdir
import predict
import sys
import time


#definition of a function which gets pictures from a folder
#z = input("Input vor CascadeClassifier?: ")
# load OpenCV pre-trained cascade classifier for face detection
face_cascade = cv2.CascadeClassifier('model/haarcascade_frontalface_default.xml')
#y = input("Input am Anfamg?: ")
# variables
#face_locations = []
#faces_preds = []
#process_every_n_frames = 20
#path = r'C:/Users/lazyj/Desktop/Studium/Laborpraktikum/emotionrecognition-NN_1.1/emotionrecognition-NN/TestImages'
path = sys.argv[1]
imageList = listdir(path)
#print("Pfad wurde gelesen " + path)
image_count = 0
#picsList = loadImage(path)


# colors
white = (255, 255, 255)
black = (0, 0, 0)
class_labels = ['Angry', 'Disgust', 'Fear', 'Happy', 'Sad', 'Surprise', 'Neutral']
emotion_colors = [(54, 67, 244), (136, 150, 0), (76, 39, 156), (80, 175, 76), (0, 152, 255), (59, 235, 255), (243, 150, 33)]

for image in imageList:
	frame = cv2.imread(path + image, cv2.IMREAD_UNCHANGED)
	frame_resized = cv2.resize(frame, (0, 0), fx=0.25, fy=0.25)
	frame_resized_gray = cv2.cvtColor(frame_resized, cv2.COLOR_BGR2GRAY)
	#print("Resize hat funktioniert")
	# Find all the faces in the current frame of video
	faces = face_cascade.detectMultiScale(frame_resized_gray, 1.1, 5)
	print('Faces found: ', len(faces))
	if len(faces) == 1: 
		faces_preds = []
		for (x, y, w, h) in faces:
			face = frame_resized_gray[y:y+h, x:x+w]
			face_input_48 = cv2.resize(face, (48, 48)).reshape(1, 48*48)
			face_input_48 = np.array(face_input_48, dtype=np.float32)
			# face_input_48 = np.divide(face_input_48, 255)
			face_input_128 = cv2.resize(face, (128, 128)).reshape(1, 128*128)
			face_input_128 = np.array(face_input_128, dtype=np.float32)
				# face_input_128 = np.divide(face_input_128, 255)
				# pred_probas = predict.predictEmotion(face_input_intensity)
				# np.set_printoptions(precision=3, suppress=True)
			prediction = predict.predict_emotion_probas(face_input_128, face_input_48)
			print(prediction)
			faces_preds.append(prediction)


#x = input("Input:")


