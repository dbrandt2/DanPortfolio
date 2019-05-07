#ifndef PHYSICSMANAGER_H
#define PHYSICSMANAGER_H

#include <iostream>
#include <string>
#include "Monobehaviour.h"
#include "Singleton.h"

using namespace std;

class PhysicsManager : public Singleton<PhysicsManager>, public Monobehaviour
{
public:
	void Start();
	void Update();
	void Shutdown();

private:

};
#endif //PHYSICSMANAGER_H
