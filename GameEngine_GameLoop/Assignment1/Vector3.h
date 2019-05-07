#ifndef VECTOR3_H
#define VECTOR3_H
#include <iostream>
#include <string>
#include "MemoryManager.h"
#include "Singleton.h"

using namespace std;

class Vector3
{
protected:
	float x, y, z; // elements of vector3


public:
	//default constructor [1, 1, 1]
	Vector3()
		: x(float (1)), y(float (1)), z(float (1))
	{
		
	}

	Vector3(float inputX,float inputY,float inputZ)
		: x(inputX), y(inputY), z(inputZ)
	{

	}

	void* operator new(size_t);
	void operator delete(void*);

	inline void* operator new[] (size_t size)
	{
		return MemoryManager::getInstance().allocate(size);
	}

	inline void operator delete [] (void* arrayDelete)
	{
		MemoryManager::getInstance().free(arrayDelete);
	}

	Vector3 add_Vectors(Vector3 a, Vector3 b);	 // add two vectors

	Vector3 sub_Vectors(Vector3 a, Vector3 b);	// subtract two vectors

	Vector3 scale(Vector3 vec, float mul);		//scale a vector by a float value

	float dot(Vector3 a, Vector3 b);

	Vector3 normalize(Vector3 vec);

	float length(Vector3 vec);

};

inline void Vector3::operator delete (void* ptrDelete)
{
	MemoryManager::getInstance().free(ptrDelete);
}

inline void* Vector3::operator new (size_t size)
{
	return MemoryManager::getInstance().allocate(size);
}
#endif

