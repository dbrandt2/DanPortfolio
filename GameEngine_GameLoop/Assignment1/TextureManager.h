#ifndef MONOBEHAVIOUR_H
#define MONOBEHAVIOUR_H

#include <iostream>
#include <string>
#include "Monobehaviour.h"
#include "Singleton.h"

class TextureManager : public Singleton<TextureManager>, public Monobehaviour
{
public:
	void Start();
	void Update();
	void Shutdown();

private:

};
#endif //MONOBEHAVIOUR_H

