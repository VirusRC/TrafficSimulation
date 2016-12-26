#pragma once

#pragma region INCLUDES

#include <string>
#include <windows.h>
#include "HelperClass.h"

#ifdef IPCCOMPONENT_EXPORTS
#define IPCCOMPONENT_API __declspec(dllexport) 
#else
#define IPCCOMPONENT_API __declspec(dllimport)
#endif

using namespace std;
#pragma endregion

#pragma region DEFINES

#define MAXMESSLENGTH 1024

#pragma endregion

#pragma region INTERFACES

extern "C" IPCCOMPONENT_API int server_InitPipe(char* tmpNetworkHostName, char* tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);

#pragma endregion

#pragma region CONFIGURATION
/*
Contains the configuration for the named pipes used for client and server
The default values for the configuration constructor was chosen to fit the requirements at start of project
*/
class Configuration
{
public:
	Configuration();
	Configuration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);
	Configuration(string tmpNetworkHostName, string tmpServerPipeName);
	~Configuration();

	void Set_ServerPipeName(string tmpServerPipeName);
	string Get_ServerPipeName();

	void Set_ServerNetworkHostName(string tmpNetworkHostName);
	string Get_NetworkHostName();

	void Set_ServerPipeMaxInstances(int tmpPipeMaxInstances);
	int Get_ServerPipeMaxInstances();

	void Set_ServerOutBufferSize(int tmpOutBufferSize);
	int Get_ServerOutBufferSize();

	void Set_ServerInBufferSize(int tmpInBufferSize);
	int Get_ServerInBufferSize();

private:
	string serverPipeName = "ServerPipe";

	//"." defines that local hostname should be used
	string networkHostName = ".";

	//default max. 3 pipes for the server can be created
	int serverPipeMaxInstances = 3;

	//defines the buffer size for input/output buffer in bytes
	int serverOutBufferSize = 255;
	int serverInBufferSize = 255;
};

Configuration::Configuration(string tmpNetworkHostName, string tmpServerPipeName)
{
	this->networkHostName = tmpNetworkHostName;
	this->serverPipeName = tmpServerPipeName;
}

Configuration::Configuration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
{
	this->networkHostName = tmpNetworkHostName;
	this->serverPipeName = tmpServerPipeName;
	this->serverPipeMaxInstances = tmpServerPipeMaxInstances;
	this->serverOutBufferSize = tmpServerOutBufferSize;
	this->serverInBufferSize = tmpServerInBufferSize;
}

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

