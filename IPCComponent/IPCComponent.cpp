// IPCComponent.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "IPCComponent.h"


#pragma region SERVER
int Server::ServerInitPipe(Configuration tmpConfiguration)
{
	//Creating the pipename as LPCWSTR as required from Windows API CreateNamedPipe-function
	string tmpPipeName = "\\\\" + tmpConfiguration.Get_NetworkHostName() + "\\pipe\\" + tmpConfiguration.Get_ServerPipeName();
	wstring stemp = Helper::s2ws(tmpPipeName);
	LPCWSTR pipeName = stemp.c_str();

	serverPipe = CreateNamedPipe(pipeName,
		PIPE_ACCESS_DUPLEX | PIPE_TYPE_BYTE | PIPE_READMODE_BYTE,   // FILE_FLAG_FIRST_PIPE_INSTANCE is not needed but forces CreateNamedPipe(..) to fail if the pipe already exists...
		PIPE_WAIT,
		1,
		1024 * 16,
		1024 * 16,
		NMPWAIT_USE_DEFAULT_WAIT,
		NULL);



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

#pragma endregion
