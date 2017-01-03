#pragma once

#pragma region INCLUDES

#include <iostream>
#include <tuple>
#include <vector>
#include <algorithm>
#include "HelperClass.h"

using namespace std;
#pragma endregion

#pragma region DEFINES

#ifdef IPCCOMPONENT_EXPORTS
#define IPCCOMPONENT_API __declspec(dllexport) 
#else
#define IPCCOMPONENT_API __declspec(dllimport)
#endif

#define MAXMESSLENGTH 512

//disable save method warnings
#pragma warning(disable:4996)
#pragma endregion

#pragma region INTERFACES

extern "C" IPCCOMPONENT_API int server_InitPipeConfiguration(char* tmpNetworkHostName, char* tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);
//starts the multithreading process for the pipes on server (every pipe should run in a different thread)
extern "C" IPCCOMPONENT_API int server_StartPipeServer();
extern "C" IPCCOMPONENT_API int server_ResetPipe();

extern "C" IPCCOMPONENT_API int client_InitPipeConfiguration(char* tmpNetworkHostName, char* tmpClientPipeName, char* tmpClientName);
extern "C" IPCCOMPONENT_API int client_ClientConnectToServerPipe();
extern "C" IPCCOMPONENT_API int client_ClientSendMessage(char* tmpMessage);
#pragma endregion

#pragma region SERVER CONFIGURATION
/*
Contains the configuration for the named pipes used for client and server
The default values for the configuration constructor was chosen to fit the requirements at start of project
*/
class ServerConfiguration
{
public:
	ServerConfiguration();
	ServerConfiguration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);
	~ServerConfiguration();

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
	string serverNetworkHostName = ".";

	//default max. 3 pipes for the server can be created
	int serverPipeMaxInstances = 3;

	//defines the buffer size for input/output buffer in bytes
	int serverOutBufferSize = 255;
	int serverInBufferSize = 255;
};

ServerConfiguration::ServerConfiguration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
{
	this->serverNetworkHostName = tmpNetworkHostName;
	this->serverPipeName = tmpServerPipeName;
	this->serverPipeMaxInstances = tmpServerPipeMaxInstances;
	this->serverOutBufferSize = tmpServerOutBufferSize;
	this->serverInBufferSize = tmpServerInBufferSize;
}

ServerConfiguration::ServerConfiguration()
{
}

ServerConfiguration::~ServerConfiguration()
{
}

#pragma endregion

#pragma region SERVER
class Server
{
public:
	static Server* server_GetInstance();

	//default or user set values of Configuration class are used for pipe init
	static int ServerInitConfiguration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);

	//terminates the currently running pipe server instance 
	static int ServerReset();

	//starts the multithreaded listening process of the different pipes (every pipe runs in own thread)
	static int ServerStartPipes();

	//checks for valid serverPipe, and serverConfiguration
	static int CheckValidServerConfig();

	//stores available client connection IDs
	static vector<int> availableConnections;

	//stores listing of clientName and connection ID
	static vector<tuple<string, int>> clientConnectionList;

	//stores the data received of a client according to the given connection ID used as index
	static vector<vector<string>> dataStorage;

protected:
	Server() {}

private:
	static Server *server_Instance;

	ServerConfiguration *serverConfiguration = nullptr;

};
#pragma endregion

#pragma region CLIENT CONFIGURATION

class ClientConfiguration
{
public:
	ClientConfiguration();
	ClientConfiguration(string tmpClientNetworkHostName, string tmpClientPipeName, string tmpClientName);
	~ClientConfiguration();

	void Set_ClientPipeName(string tmpClientPipeName);
	string Get_ClientPipeName();

	void Set_ClientNetworkHostName(string tmpNetworkHostName);
	string Get_NetworkHostName();

	void Set_ClientName(string tmpClientName);
	string Get_ClientName();

private:

	string clientNetworkHostName = ".";

	string clientPipeName = "ServerPipe";

	//used for unique identification on server (server returns error code if client with name is already connected)
	string clientName = "ClientName";

};

ClientConfiguration::ClientConfiguration(string tmpClientNetworkHostName, string tmpClientPipeName, string tmpClientName)
{
	this->clientNetworkHostName = tmpClientNetworkHostName;
	this->clientPipeName = tmpClientPipeName;
	this->clientName = tmpClientName;
}

ClientConfiguration::ClientConfiguration()
{
}

ClientConfiguration::~ClientConfiguration()
{
}

#pragma endregion

#pragma region CLIENT

class Client
{
public:
	static Client* client_GetInstance();

	//creates and assigns client config to client instance
	static int ClientInitConfiguration(string tmpClientNetworkHostName, string tmpClientPipeName, string tmpClientName);

	//creates pipe handle with the given configuration and tries to connect to given server pipe
	static int ClientConnectToServerPipe();

	//sends a given message to the configured server pipe
	static int ClientSendMessage(string tmpMessageContent);
	
protected:
	Client() {}

private:
	static Client *client_Instance;

	ClientConfiguration *clientConfiguration = nullptr;

	HANDLE clientPipeHandle = nullptr;
};

#pragma endregion

