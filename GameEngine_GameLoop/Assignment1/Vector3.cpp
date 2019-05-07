#include "Vector3.h"
#include <cmath>

//inline void* Vector3::operator new (size_t size)
//{
//	return MemoryManager::getInstance().allocate(size);
//}

//inline void Vector3::operator delete (void* ptrDelete)
//{
//	MemoryManager::getInstance().free(ptrDelete);
//}


//add two vectors
Vector3 Vector3::add_Vectors(Vector3 a, Vector3 b)
{
	return Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
}

//subtract two vectors 
Vector3 Vector3::sub_Vectors(Vector3 a, Vector3 b)
{
	return Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
}

//return the length of the vector
float Vector3::length(Vector3 vec)
{
	return sqrt((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z));
}

//normailize the vector
Vector3 Vector3::normalize(Vector3 vec)
{
	Vector3 retVec;
	float len = length(vec);

	if (len != 0)
	{
		retVec.x = vec.x / len;
		retVec.y = vec.y / len;
		retVec.z = vec.z / len;
	}

	return retVec;
}

//return the dot product of two vectors 
float Vector3::dot(Vector3 a, Vector3 b)
{
	return (a.x * b.x) + (a.y + b.y) + (a.z + b.z);
}

//scale a vector
Vector3 Vector3::scale(Vector3 vec, float mul)
{
	return Vector3(vec.x * mul, vec.y * mul, vec.z * mul);
}
