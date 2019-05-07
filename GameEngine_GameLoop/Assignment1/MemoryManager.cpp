#include "MemoryManager.h"
#include "Vector3.h"

#define POOLSIZE 32
//expand the memory pool size 
void MemoryManager::expandPoolSize()
{
	size_t size = (sizeof(Vector3) > sizeof(FreeStore*)) ? sizeof(Vector3) : sizeof(FreeStore*);

	FreeStore* head = reinterpret_cast <FreeStore*> (new char[size]);
	freeStoreHead = head;

	for (int i = 0; i < POOLSIZE; i++)
	{
		head->next = reinterpret_cast <FreeStore*> (new char[size]);
	}

	head->next = 0;
}

void MemoryManager::cleanUp()
{
	FreeStore* nextPtr = freeStoreHead;
	for (; nextPtr; nextPtr = freeStoreHead)
	{
		freeStoreHead = freeStoreHead->next;
		delete[] nextPtr;
	}
}

void MemoryManager::Start()
{
	//cout << "MemoryManager::Start()" << endl;
}

void MemoryManager::Update()
{
	//cout << "MemoryManager::Update()" << endl;
}

void MemoryManager::Shutdown()
{
	//cout << "MemoryManager::Shutdown()" << endl;
}