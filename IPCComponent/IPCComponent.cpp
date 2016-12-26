// IPCComponent.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "IPCComponent.h"

#pragma region INTERFACES

IPCCOMPONENT_API int server_InitPipe(char* tmpNetworkHostName, char* tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
{
	try
	{
		Configuration *defaultConfig = new Configuration(string(tmpNetworkHostName), string(tmpServerPipeName), tmpServerPipeMaxInstances, tmpServerOutBufferSize, tmpServerInBufferSize);
		Server *serverInstance = new Server();

		return serverInstance->ServerInitPipe(*defaultConfig);
	}
	catch (...)
	{
		return -1;
	}
}

#pragma endregion

#pragma region SERVER
int Server::ServerInitPipe(Configuration tmpConfiguration)
{
	LPCWSTR pipeName = nullptr;

	//Creating the pipename as LPCWSTR as required from Windows API CreateNamedPipe-function
	try
	{
		string tmpPipeName = "\\\\" + tmpConfiguration.Get_NetworkHostName() + "\\pipe\\" + tmpConfiguration.Get_ServerPipeName();
		wstring stemp = Helper::s2ws(tmpPipeName);
		pipeName = stemp.c_str();
	}
	catch (...)
	{
		return -1;
	}

	try
	{
		//detailed information about CreateNamedPipe and the parameters: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365150(v=vs.85).aspx
		serverPipe = CreateNamedPipe(pipeName,
			PIPE_ACCESS_INBOUND | PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE, //flags define that the pipes can be used to send data !!!from client to server ONLY!!!, data is treated as byte messages, and data is read as byte messages
			PIPE_WAIT,
			tmpConfiguration.Get_ServerPipeMaxInstances(),
			tmpConfiguration.Get_ServerOutBufferSize(),
			tmpConfiguration.Get_ServerInBufferSize(),
			NMPWAIT_USE_DEFAULT_WAIT,
			NULL);
	}
	catch (...)
	{
		return -2;
	}

	return 0;
}
#pragma endregion

#pragma region CLIENT



#pragma endregion

#pragma region CONFIGURATION
void Configuration::Set_ServerPipeName(string tmpServerPipeName)
{
	serverPipeName = tmpServerPipeName;
}

string Configuration::Get_ServerPipeName()
{
	return serverPipeName;
}

void Configuration::Set_ServerNetworkHostName(string tmpNetworkHostName)
{
	networkHostName = tmpNetworkHostName;
}

string Configuration::Get_NetworkHostName()
{
	return networkHostName;
}

void Configuration::Set_ServerPipeMaxInstances(int tmpPipeMaxInstances)
{
	serverPipeMaxInstances = tmpPipeMaxInstances;
}

int Configuration::Get_ServerPipeMaxInstances()
{
	return serverPipeMaxInstances;
}

void Configuration::Set_ServerOutBufferSize(int tmpOutBufferSize)
{
	serverOutBufferSize = tmpOutBufferSize;
}

int Configuration::Get_ServerOutBufferSize()
{
	return serverOutBufferSize;
}

void Configuration::Set_ServerInBufferSize(int tmpInBufferSize)
{
	serverInBufferSize = tmpInBufferSize;
}

int Configuration::Get_ServerInBufferSize()
{
	return serverInBufferSize;
}

#pragma endregion
