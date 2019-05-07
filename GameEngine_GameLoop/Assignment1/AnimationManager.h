#ifndef ANIMATIONMANAGER_H
#define ANIMATIONMANAGER_H

#include <iostream>
#include <string>
#include "Monobehaviour.h"
#include "Singleton.h"

using namespace std;

class AnimationManager : public Singleton<AnimationManager>, public Monobehaviour
{
public:
	void Start();
	void Update();
	void Shutdown();

private:

};
#endif //VISUALMANAGER_H

