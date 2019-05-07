#ifndef RENDERMANAGER_H
#define RENDERMANAGER_H

#include <iostream>
#include <string>
#include "Monobehaviour.h"
#include "Singleton.h"

class RenderManager : public Singleton<RenderManager>, public Monobehaviour
{
public:
	void Start();
	void Update();
	void Shutdown();

private:
};
#endif 

