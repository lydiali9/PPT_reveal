// OpenCV_Test.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include <iostream>

#include <opencv2/core/core.hpp>
#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"

using namespace std;
using namespace cv;

string face_cascade_name = "C:\\Tools\\OpenCV\\opencv\\sources\\data\\haarcascades\\haarcascade_frontalface_alt2.xml";
CascadeClassifier face_cascade;
string window_name = "Face Recognizer";

void detectAndDisplay(Mat frame);

int main(int argc, char** argv) {
	Mat image;
	//image = imread(argv[1]);
	image = imread("C:\\Users\\lli508\\Desktop\\photo\\openCV\\6.jpg");

	//validate if photo has exsits.

	if (!face_cascade.load(face_cascade_name)) {
		printf("[error] cannot load face cascade \n"); //无法加载级联分类器文件！
		return -1;
	}

	detectAndDisplay(image);

	waitKey(0);
}

void detectAndDisplay(Mat frame) {
	std::vector<Rect> faces;
	Mat frame_gray;

	cvtColor(frame, frame_gray, CV_BGR2GRAY);
	equalizeHist(frame_gray, frame_gray);

	face_cascade.detectMultiScale(frame_gray, faces, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));

	for (int i = 0; i < faces.size(); i++) {
		Point center(faces[i].x + faces[i].width*0.5, faces[i].y + faces[i].height*0.5);
		ellipse(frame, center, Size(faces[i].width*0.5, faces[i].height*0.5), 0, 0, 360, Scalar(255, 0, 255), 4, 8, 0);
	}

	imshow(window_name, frame);
}
