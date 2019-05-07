#ifndef IMEMORYMANAGER_H
#define IMEMORYMANAGER_H
#include <iostream>
#include <string>

using namespace std;

class IMemoryManager
{
public:
	 virtual void* allocate(size_t) = 0;
	 virtual void free(void*) = 0;
};
#endif //IMEMORYMANAGER_H
