#ifndef GAMEMANAGER_H
#define GAMEMANAGER_H
#include "Singleton.h"
#include "Monobehaviour.h"
#include "GameWorld.h"
#include <thread>
#include <functional>

using namespace std;

class GameManager : public Singleton<GameManager>, public Monobehaviour
{
public:
	GameManager();
	~GameManager();


	thread render;
	thread input;
	static void Render();
	static void InputHandler();
	void Start();
	void Update();
	void Shutdown();

};
#endif //GAMEMANAGER_H

