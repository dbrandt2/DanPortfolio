#ifndef SINGLETON_H
#define SINGLETON_H

#include <iostream>
#include <string>

using namespace std;

template <typename T>
class Singleton
{
protected:
	Singleton() = default;
	~Singleton() = default;

public:
	static T& getInstance()
	{
		static T instance; //instantiate first time 
		return instance;
	}

	Singleton (T &&) = delete; //Move constructor
	Singleton (T const&) = delete; //Copy constructor
	void operator=(T const&) = delete; // assignment opperator
	void operator=(T &&) = delete; // Move assignment opperator

};
#endif //SINGLETON_H

