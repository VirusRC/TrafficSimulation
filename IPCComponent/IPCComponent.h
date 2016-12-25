#pragma once

#ifdef IPCComponent_EXPORTS
#define IPCComponent_API __declspec(dllexport) 
#else
#define IPCComponent_API __declspec(dllimport) 
#endif

#pragma region INCLUDES

#include <string>
#include <windows.h>
#include "HelperClass.h"

using namespace std;
#pragma endregion

#pragma region DEFINES

#define MAXMESSLENGTH 1024

#pragma endregion

#pragma region INTERFACES



#pragma endregion

#pragma region CONFIGURATION

class Configuration
{
public:
	Configuration();
	~Configuration();

	void Set_ServerPipeName(string tmpServerPipeName);
	string Get_ServerPipeName();

	void Set_ServerNetworkHostName(string tmpNetworkHostName);
	string Get_NetworkHostName();

private:
	string serverPipeName = "ServerPipe";

	//"." defines that local hostname should be used
	string networkHostName = ".";

};

Configuration::Configuration()
{
}

Configuration::~Configuration()
{
}

#pragma endregion


#pragma region SERVER

class Server
{
public:
	Server();
	~Server();

	//default or user set values of Configuration class are used for pipe init
	int ServerInitPipe(Configuration tmpConfiguration);

private:
	HANDLE serverPipe = nullptr;

	DWORD serverRead = NULL;

	char serverBuffer[MAXMESSLENGTH];
};

Server::Server()
{
}

Server::~Server()
{
}

#pragma endregion

#pragma region CLIENT

class Client
{
public:
	Client();
	~Client();

private:

};

Client::Client()
{
}

Client::~Client()
{
}

#pragma endregion

