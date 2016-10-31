#pragma once
#include <stdexcept>
using namespace std;

namespace DetectFace
{
	extern "C" { __declspec(dllexport) bool detectFaces(string image); }
}