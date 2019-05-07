#ifndef MEMORYMANAGER_H
#define MEMORYMANAGER_H

#include <iostream>
#include <string>
#include <sys/types.h>
#include "Monobehaviour.h"
#include "Singleton.h"
#include "IMemoryManager.h"

using namespace std;

class MemoryManager : public Singleton<MemoryManager>, public IMemoryManager, public Monobehaviour
{
	struct FreeStore
	{
		FreeStore *next;
	};

	void expandPoolSize();
	void cleanUp();
	FreeStore* freeStoreHead;

public:

	//constructor
	MemoryManager()
	{
		freeStoreHead = nullptr;
		expandPoolSize();
	} 

	virtual ~MemoryManager()
	{
		cleanUp();
	}

	virtual void* allocate(size_t);
	virtual void free(void*);
	
	void Start();
	void Update();
	void Shutdown();

};

inline void* MemoryManager::allocate(size_t size)
{
	if (0 == freeStoreHead)
	{
		expandPoolSize();
	}

	FreeStore* head = freeStoreHead;
	freeStoreHead = head->next;
	return head;
}

inline void MemoryManager::free(void* deleted)
{
	FreeStore* head = static_cast <FreeStore*> (deleted);
	head->next = freeStoreHead;
	freeStoreHead = head;
}

#endif //MEMORYMANAGER_H

