#include "GameWorld.h"

//startTime for increasing enemy speed
auto startTime = chrono::high_resolution_clock::now();
auto playerStartTime = chrono::high_resolution_clock::now();

GameWorld::GameWorld()
{
	gameboard = new char*[12];

	//initialize the buffer and the boarders
	for (int row = 0; row < 12; row++)
	{
		gameboard[row] = new char[37];
		for (int col = 0; col < 37; col++)
		{
			if (row == 0 || row == 11) // draw top and bottom boarder
			{
				gameboard[row][col] = '#';
			}
			else if (col == 0 || col == 36) // draw left and right boarder
			{
				gameboard[row][col] = '#';
			}
			else if (col == pos && row == playerRow) // draw player
			{
				gameboard[row][col] = '^';
			}
			else if ((col == 4 || col == 8 || col == 12 || col == 16 || col == 20 || col == 24 || col == 28 || col == 32) && row == enemyRow) // draw enemies
			{
				gameboard[row][col] = 'M';
			}
			else // draw empty
			{
				gameboard[row][col] = ' ';
			}
		}
	}

	////allocate bufferboard
	bufferboard = new char*[12];
	for (int row = 0; row < 12; row++)
	{
		bufferboard[row] = new char[37];
	}
}


GameWorld::~GameWorld()
{

}


void GameWorld::Start()
{
	
}

//handle input on a seperate thread
void GameWorld::Update()
{
	switch (input)
	{
	case 'a':
		if (pos >= 2)
		{
			pos--;
		}
		break;

	case 'd':
		if (pos <= 34)
		{
			pos++;
		}
		break;

	case ' ':
		if (bulletCount < 3)
		{
			bulletCount++;
			bufferboard[playerRow - 1][pos] = '*';
		}
			break;
	}

	////allocate bufferboard
	//bufferboard = new char*[12];
	//for (int row = 0; row < 12; row++)
	//{
	//	bufferboard[row] = new char[37];
	//}

	//copy gameboard into bufferboard using memcpy
	memcpy(&bufferboard, &gameboard, sizeof(gameboard));

	//update bufferboard
	UpdateBufferboard();

	//copy buffer back into gameboard
	//memcpy(&gameboard, &bufferboard, sizeof(bufferboard));
}

//clean up gameboard memory
void GameWorld::Shutdown()
{
	delete[] bufferboard[0];
	delete[] bufferboard;
}

void GameWorld::UpdateBufferboard()
{
	auto currTime = chrono::high_resolution_clock::now();
	auto time = chrono::duration_cast<chrono::milliseconds>(currTime - startTime).count();
	//update enemies
	if (time >= enemySpeed)
	{
		startTime = chrono::high_resolution_clock::now();
		UpdateEnemies();
	}

	//update player
	auto playerCurrTime = chrono::high_resolution_clock::now();
	auto playerTime = chrono::duration_cast<chrono::milliseconds>(playerCurrTime - playerStartTime).count();
	if (playerTime >= playerSpeed)
	{
		playerStartTime = chrono::high_resolution_clock::now();
		UpdatePlayer();
		UpdateShots();
	}

}

//loop through buffer and update enemies as needed
void GameWorld::UpdateEnemies()
{
	if (enemyDirection == 1)
	{
		for (int col = 37; col > 0; col--)
		{
			if (bufferboard[enemyRow][col] == 'M') //if it is an M make it a W
			{
				if (bufferboard[enemyRow][col + 1] == '#')
				{
					enemyDirection = -1;
					bufferboard[enemyRow][col - 1] = 'W';
				}
				else
				{
					bufferboard[enemyRow][col + 1] = 'W';
					bufferboard[enemyRow][col] = ' ';
				}
			}
			else if (bufferboard[enemyRow][col] == 'W') //if it is a W make it a M
			{
				if (bufferboard[enemyRow][col + 1] == '#')
				{
					enemyDirection = -1;
					bufferboard[enemyRow][col - 1] = 'M';
				}
				else
				{
					bufferboard[enemyRow][col + 1] = 'M';
					bufferboard[enemyRow][col] = ' ';
				}
			}
		}
	}
	else if (enemyDirection == -1)
	{
		for (int col = 0; col < 37; col++)
		{
			if (bufferboard[enemyRow][col] == 'M') // if it is an M make it a W
			{
				if (bufferboard[enemyRow][col - 1] == '#')
				{
					enemyDirection = 1;
					for (int col = 0; col < 37; col++)
					{
						if (bufferboard[enemyRow][col] == 'M') // turn all enemies around and drop a level
						{
							bufferboard[enemyRow + 1][col + 1] = 'W'; //col + 1 
							bufferboard[enemyRow][col] = ' ';
						}
					}
					enemyRow++;
					enemyCellsPerSec++;
					enemySpeed = 1000 / enemyCellsPerSec;
				}
				else
				{
					bufferboard[enemyRow][col - 1] = 'W';
					bufferboard[enemyRow][col] = ' ';
				}
			}
			else if (bufferboard[enemyRow][col] == 'W') // if it is a W make it an M
			{
				if (bufferboard[enemyRow][col - 1] == '#')
				{
					enemyDirection = 1;
					for (int col = 0; col < 37; col++)
					{
						if (bufferboard[enemyRow][col] == 'W') // turn all enamies around and drop a level
						{
							bufferboard[enemyRow + 1][col + 1] = 'W'; //col + 1
							bufferboard[enemyRow][col] = ' ';
						}
					}
					enemyRow++;
					enemyCellsPerSec++;
					enemySpeed = 1000 / enemyCellsPerSec;
				}
				else
				{
					bufferboard[enemyRow][col - 1] = 'M';
					bufferboard[enemyRow][col] = ' ';
				}
			}
		}
	}
	//if (enemyDirection == 1)
	//{
	//	for (int col = 37; col > 0; col--)
	//	{
	//		if (bufferboard[enemyRow][col] == 'M' || bufferboard[enemyRow][col] == 'W')
	//		{
	//			if (bufferboard[enemyRow][col + 1] == '#')
	//			{
	//				enemyDirection = -1;
	//				bufferboard[enemyRow][col - 1] = 'M';
	//			}
	//			else
	//			{
	//				bufferboard[enemyRow][col + 1] = 'W';
	//				bufferboard[enemyRow][col] = ' ';
	//			}
	//		}
	//	}
	//}
	//else if (enemyDirection == -1)
	//{
	//	for (int col = 0; col < 37; col++)
	//	{
	//		if (bufferboard[enemyRow][col] == 'M' || bufferboard[enemyRow][col] == 'W')
	//		{
	//			if (bufferboard[enemyRow][col - 1] == '#')
	//			{
	//				enemyDirection = 1;
	//				enemyRow++;
	//				bufferboard[enemyRow][col + 1] = 'M'; // turn around and drop a level
	//				bufferboard[enemyRow - 1][col] = ' ';
	//			}
	//			else
	//			{
	//				bufferboard[enemyRow][col - 1] = 'W';
	//				bufferboard[enemyRow][col] = ' ';
	//			}
	//		}
	//	}
	//}
	}

void GameWorld::CheckEnemyBoarders(int col)
{
	if (col + 1 == 36) //reached right boarder, turn around
	{
		enemyDirection = -1; 
	}
	else if (col - 1 == 0) //reached left boarder, turn around drop a row and speed up
	{
		enemyDirection = 1;
		//enemyRow++;
		enemyCellsPerSec++;
		enemySpeed = 1000/ enemyCellsPerSec;
	}
}

void GameWorld::UpdatePlayer()
{
	//Handle the input and update player position in the bufferboard
	for (int col = 0; col < 37; col++)
	{
		if (col == 0 || col == 36)
		{
			bufferboard[playerRow][col] = '#';
		}
		else
		{
			bufferboard[playerRow][col] = ' ';
		}
		bufferboard[playerRow][pos] = '^';
	}

	//TEST to see if the bufferboard is being updated properly (not double buffering)
	system("cls");
	for (int i = 0; i < 12; i++)
	{
		for (int j = 0; j < 37; j++)
		{
			//cout << bufferboard[i][j];
			cout << gameboard[i][j];
		}
		cout << "\n";
	}
	cout << "";
}

void GameWorld::UpdateShots()
{
	for (int row = 0; row < 12; row++)
	{
		for (int col = 0; col < 37; col++)
		{
			if (bufferboard[row][col] == '*')
			{
				bufferboard[row][col] = ' ';
				bufferboard[row - 1][col] = '*';
				if (bufferboard[row - 1][col] == 'M' || bufferboard[row - 1][col] == 'W')
				{
					bufferboard[row - 1][col] = ' ';
					bufferboard[row][col] = ' ';
					bulletCount--;
				}
				else if (row - 1 == 0)
				{
					bufferboard[row - 1][col] = '#';
					bulletCount--;
				}

				//reset bullet count if needed
				if (bulletCount <= 0)
				{
					bulletCount = 0;
				}
			}
		}
	}
}
