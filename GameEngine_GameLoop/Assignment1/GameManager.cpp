#include "GameManager.h"

//start the render thread
thread render(GameManager::Render);
thread input(GameManager::InputHandler);

//get the instnce of game world
GameWorld* gameWorld = &Singleton<GameWorld>::getInstance();

GameManager::GameManager()
{
}

GameManager::~GameManager()
{
}

void GameManager::InputHandler()
{
	while (true)
	{
		cin.clear();
		gameWorld->input = _getch();
	}
}

//this method will render the gameboard to the screen
void GameManager::Render()
{
	//system("cls");
	////render previous array 
	//for (int row = 0; row < 12; row++)
	//{
	//	for (int col = 0; col < 37; col++)
	//	{
	//		cout << gameWorld->gameboard[row][col];
	//		//cout << gameWorld->bufferboard[row][col];
	//	}
	//	cout << endl;
	//}
}

void GameManager::Start()
{	

}

void GameManager::Update()
{
	gameWorld->Update();
}

void GameManager::Shutdown()
{
	render.join();
	input.join();
}
