#ifndef FILESYSTEMMANAGER_H
#define FILESYSTEMMANAGER_H

#include <iostream>
#include <string>
#include "Monobehaviour.h"
#include "Singleton.h"

using namespace std;

class FileSystemManager : public Singleton<FileSystemManager>, public Monobehaviour
{
public:
	void Start();
	void Update();
	void Shutdown();

private: 
};

#endif //FILESYSTEMMANAGER_H
