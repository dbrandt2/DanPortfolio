#ifndef GAMEWORLD_H
#define GAMEWORLD_H
#include "Monobehaviour.h"
#include "Singleton.h"
#include "RenderManager.h"
#include <string>
#include <thread>
#include <conio.h>
#include <chrono>

class GameWorld : public Singleton<GameWorld>, public Monobehaviour
{
public:
	GameWorld();
	~GameWorld();

	void Start();
	void Update();
	void Shutdown();

	//mehtods for updating the buffer board
	void UpdateBufferboard();
	void UpdateEnemies();
	void CheckEnemyBoarders(int col);
	void UpdatePlayer();
	void UpdateShots();

	char input;
	char** gameboard; //2d array to store gameboard
	char** bufferboard; // 2d array used to update gameboard

	//game characters information
	int pos = 13; // start position is the center of the screen
	float playerSpeed = (1000 / 6); //six cells per second
	int playerRow = 10;
	int bulletCount = 0;

	int enemyCellsPerSec = 1;
	float enemySpeed = (1000 / enemyCellsPerSec); // this will be increased everytime the enemies reach the left edge of bufferboard
	int enemyRow = 3; // keeps track of the row the enemies are on
	int enemyDirection = 1; //1 = right : -1 = left


};
#endif //GAMEWORLD_H

