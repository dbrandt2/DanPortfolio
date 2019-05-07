#ifndef MONOBEHAVIOR_H
#define MONOBEHAVIOR_H

#include <iostream>
#include<string>

using namespace std;

class Monobehaviour
{
public:
	
	virtual void Start() = 0;
	virtual void Update() = 0;
	virtual void Shutdown() = 0;

};
#endif //MONOBEHAVIOR_H

