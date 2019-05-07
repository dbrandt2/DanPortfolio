#include <iostream>
#include <string>
#include <chrono>
#include <thread>
#include <conio.h>
#include <thread>
#include "PhysicsManager.h"
#include "RenderManager.h"
#include "SoundManager.h"
#include "FileSystemManager.h"
#include "MemoryManager.h"
#include "TextureManager.h"
#include "AnimationManager.h"
#include "Monobehaviour.h"
#include "Vector3.h"
#include "GameWorld.h"
#include "GameManager.h"

using namespace std;

int main()
{
	//initialize game world
	GameWorld();

	//array of pointers to managers
	Monobehaviour** managers = new Monobehaviour*[8];

	float milliSecondsFrameSpeed = 66.6; //66.6 = 15 fps 

	//add managers to array
	managers[0] = &MemoryManager::getInstance();
	managers[1] = &FileSystemManager::getInstance();
	managers[2] = &AnimationManager::getInstance();
	managers[3] = &PhysicsManager::getInstance();
	managers[4] = &RenderManager::getInstance();
	managers[5] = &SoundManager::getInstance();
	managers[6] = &TextureManager::getInstance();
	managers[7] = &GameManager::getInstance();

	//Start each manager
	for (int i = 0; i <= 7; i++)
	{
		managers[i]->Start();
	}

	//update each manager 
	for (int i = 0; i <= 7; i++)
	{
		managers[i]->Update();
	}

	//Game Loop 
	while (true)
	{
		//frames per seconds
		auto currTime = chrono::high_resolution_clock::now();
		managers[7]->Update();
		auto endTime = chrono::high_resolution_clock::now();
		auto realTime = int((milliSecondsFrameSpeed - chrono::duration_cast<chrono::milliseconds>(endTime - currTime).count()));
		this_thread::sleep_for(chrono::milliseconds(realTime));
	}

	//shutdown each manager in reverse order
	for (int i = 7; i >= 0; i--)
	{
		managers[i]->Shutdown();
	}

	char c;
	cin.get();
	return 0;
}

