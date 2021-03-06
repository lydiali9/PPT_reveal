#include "stdafx.h"
#include <iostream>

#include "opencv2/core/core.hpp"
#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "OpenCVFaceRecognition.h"

using namespace std;
using namespace cv;

/** Function Headers */
bool detectAndDisplay(Mat frame);

/** Global variables */
String face_cascade_name = "C:\\Tools\\OpenCV\\opencv\\sources\\data\\haarcascades\\haarcascade_frontalcatface.xml";
String eyes_cascade_name = "C:\\Tools\\OpenCV\\opencv\\sources\\data\\haarcascades\\haarcascade_eye.xml";
CascadeClassifier face_cascade;
CascadeClassifier eyes_cascade;
string window_name = "Capture - Face detection";
RNG rng(12345);

/** @function detectAndDisplay */
bool detectAndDisplay(Mat frame)
{
	std::vector<Rect> faces;
	Mat frame_gray;
	Mat crop;
	bool is_defet = false;
	std::vector<Rect> eyes;

	cvtColor(frame, frame_gray, CV_BGR2GRAY);
	equalizeHist(frame_gray, frame_gray);

	//-- Detect faces
	face_cascade.detectMultiScale(frame_gray, faces, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));

	for (size_t i = 0; i < faces.size(); i++)
	{
		Point center(faces[i].x + faces[i].width*0.5, faces[i].y + faces[i].height*0.5);
		ellipse(frame, center, Size(faces[i].width*0.5, faces[i].height*0.5), 0, 0, 360, Scalar(255, 0, 255), 4, 8, 0);

		Mat faceROI = frame_gray(faces[i]);

		//-- In each face, detect eyes
		eyes_cascade.detectMultiScale(faceROI, eyes, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));

		for (size_t j = 0; j < eyes.size(); j++)
		{
			Point center(faces[i].x + eyes[j].x + eyes[j].width*0.5, faces[i].y + eyes[j].y + eyes[j].height*0.5);
			int radius = cvRound((eyes[j].width + eyes[j].height)*0.25);
			circle(frame, center, radius, Scalar(255, 0, 0), 4, 8, 0);

			if (eyes.size() != 0) {
				is_defet = true;
			}
		}
	}
	//imshow("detected", crop);
	//-- Show what you got
	//if (is_defet) {  
	//imshow(window_name, frame);
	//}
	//imshow(window_name, frame);
	return is_defet;
}

bool DetectFace::detectFaces(string iamge)
{
	Mat frame;

	//-- 1. Load the cascades
	if (!face_cascade.load(face_cascade_name)) { printf("--(!)Error loading\n"); return -1; };
	if (!eyes_cascade.load(eyes_cascade_name)) { printf("--(!)Error loading\n"); return -1; };

	// Read the image file
	//frame = imread("C:/Users/lli508/Desktop/photo/openCV/Cognitive-Face-Windows/Data/detection3.jpg");
	frame = imread(iamge);
	// Apply the classifier to the frame
	if (!frame.empty()) {
		return detectAndDisplay(frame);
	}
	else {
		printf(" --(!) No captured frame -- Break!");
		//return 0;
	}
	//waitKey(0);
}
