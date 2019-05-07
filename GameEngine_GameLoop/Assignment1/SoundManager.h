#ifndef SOUNDMANAGER_H
#define SOUNDMANAGER_H

#include <iostream>
#include <string>
#include "Monobehaviour.h"
#include "Singleton.h"

using namespace std;

class SoundManager : public Singleton<SoundManager>, public Monobehaviour
{
public:
	void Start();
	void Update();
	void Shutdown();

private:
};
#endif //SOUNDMANAGER_H

