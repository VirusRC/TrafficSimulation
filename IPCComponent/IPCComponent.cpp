// IPCComponent.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "IPCComponent.h"

DWORD WINAPI InstanceThread(LPVOID lpvParam);

#pragma region INTERFACES
IPCCOMPONENT_API int server_InitPipeConfiguration(char* tmpNetworkHostName, char* tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
{
	return Server::ServerInitConfiguration(string(tmpNetworkHostName), string(tmpServerPipeName), tmpServerPipeMaxInstances, tmpServerOutBufferSize, tmpServerInBufferSize);
}

IPCCOMPONENT_API int server_StartPipeServer()
{
	return Server::ServerStartPipes();
}

IPCCOMPONENT_API int server_ResetPipe()
{
	return Server::ServerReset();
}

IPCCOMPONENT_API int client_InitPipeConfiguration(char* tmpNetworkHostName, char* tmpServerPipeName)
{
	return Client::ClientInitConfiguration(string(tmpNetworkHostName), string(tmpServerPipeName));
}

IPCCOMPONENT_API int client_ClientConnectToServerPipe()
{
	return Client::ClientConnectToServerPipe();
}

IPCCOMPONENT_API int client_ClientSendMessage(char* tmpMessage)
{
	return Client::ClientSendMessage(string(tmpMessage));
}

#pragma endregion

#pragma region SERVER
Server* Server::server_Instance = 0;

Server* Server::server_GetInstance()
{
	if (server_Instance == 0)
		server_Instance = new Server();
	return server_Instance;
}

int Server::ServerInitConfiguration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
{
	try
	{
		Server *serverInstance = Server::server_GetInstance();
		serverInstance->serverConfiguration = new ServerConfiguration(tmpNetworkHostName, tmpServerPipeName, tmpServerPipeMaxInstances, tmpServerOutBufferSize, tmpServerInBufferSize);
	}
	catch (...)
	{
		return -1;
	}
	return 0;
}

int Server::ServerReset()
{
	try
	{
		Server *serverInstance = Server::server_GetInstance();

		//TODO: force termination of the listening processes of the pipes

		//next GetInstace() will give new/default serverInstance
		serverInstance = 0;
	}
	catch (...)
	{

	}
	return 0;
}

int Server::CheckValidServerConfig()
{
	Server *serverInstance = Server::server_GetInstance();

	if (serverInstance->serverConfiguration == nullptr)
	{
		return -1;
	}

	return 0;
}

//detailed information to the process after this function is called can be found here: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365588(v=vs.85).aspx
int Server::ServerStartPipes()
{
	Server *serverInstance = Server::server_GetInstance();

	BOOL   fConnected = FALSE;
	DWORD  dwThreadId = 0;
	HANDLE hPipe = INVALID_HANDLE_VALUE, hThread = NULL;
	wchar_t* wstrPipeName = nullptr;	

	//creating pipe name
	try
	{
		string tmpPipeName = "\\\\" + serverInstance->serverConfiguration->Get_NetworkHostName() + "\\pipe\\" + serverInstance->serverConfiguration->Get_ServerPipeName();
		wstrPipeName = Helper::s2wct(tmpPipeName);
		
	}
	catch (...)
	{
		return -1;
	}

	for (;;)
	{
		hPipe = CreateNamedPipe(
			wstrPipeName,             // pipe name 
			PIPE_ACCESS_DUPLEX,       // read/write access 
			PIPE_TYPE_MESSAGE |       // message type pipe 
			PIPE_READMODE_MESSAGE |   // message-read mode 
			PIPE_WAIT,                // blocking mode 
			PIPE_UNLIMITED_INSTANCES, // max. instances  
			serverInstance->serverConfiguration->Get_ServerOutBufferSize(),                  // output buffer size 
			serverInstance->serverConfiguration->Get_ServerInBufferSize(),                  // input buffer size 
			0,                        // client time-out 
			NULL);                    // default security attribute 

		if (hPipe == INVALID_HANDLE_VALUE)
		{
			return -2;
		}

		cout << "Waiting for client to connect!" << endl;

		//waiting for client to connect
		fConnected = ConnectNamedPipe(hPipe, NULL) ?
			TRUE : (GetLastError() == ERROR_PIPE_CONNECTED);

		cout << "Client connected! YEAH" << endl;

		if (fConnected)
		{
			printf("Client connected, creating a processing thread.\n");

			// Create a thread for this client. 
			hThread = CreateThread(
				NULL,              // no security attribute 
				0,                 // default stack size 
				InstanceThread,    // thread proc
				(LPVOID)hPipe,    // thread parameter 
				0,                 // not suspended 
				&dwThreadId);      // returns thread ID 

			if (hThread == NULL)
			{
				return -3;
			}
			else
			{
				CloseHandle(hThread);
			}
		}
		else
		{
			// The client could not connect, so close the pipe. 
			CloseHandle(hPipe);
			cout << "Client handle thread closed!" << endl;
		}

	}
	return 0;
}

DWORD WINAPI InstanceThread(LPVOID lpvParam)
{
	HANDLE hHeap = GetProcessHeap();
	TCHAR* pchRequest = (TCHAR*)HeapAlloc(hHeap, 0, MAXMESSLENGTH * sizeof(TCHAR));
	TCHAR* pchReply = (TCHAR*)HeapAlloc(hHeap, 0, MAXMESSLENGTH * sizeof(TCHAR));
	char* c_szText = new char[MAXMESSLENGTH];

	DWORD cbBytesRead = 0, cbReplyBytes = 0, cbWritten = 0;
	BOOL fSuccess = FALSE;
	HANDLE hPipe = NULL;

	if (lpvParam == NULL)
	{
		if (pchReply != NULL) HeapFree(hHeap, 0, pchReply);
		if (pchRequest != NULL) HeapFree(hHeap, 0, pchRequest);
		return (DWORD)-1;
	}

	if (pchRequest == NULL)
	{
		if (pchReply != NULL) HeapFree(hHeap, 0, pchReply);
		return (DWORD)-1;
	}

	if (pchReply == NULL)
	{
		if (pchRequest != NULL) HeapFree(hHeap, 0, pchRequest);
		return (DWORD)-1;
	}

	// The thread's parameter is a handle to a pipe object instance. 
	hPipe = (HANDLE)lpvParam;

	cout << "Waiting for a message to read!" << endl;

	// Loop until done reading
	while (1)
	{
		fSuccess = ReadFile(
			hPipe,        // handle to pipe 
			pchRequest,    // buffer to receive data 
			MAXMESSLENGTH * sizeof(TCHAR), // size of buffer 
			&cbBytesRead, // number of bytes read 
			NULL);        // not overlapped I/O 

		if (fSuccess && cbBytesRead != 0)
		{
			wcstombs(c_szText, pchRequest, wcslen(pchRequest) + 1);



			cout << c_szText << endl;
			//break;
		}
	}

	// Flush the pipe to allow the client to read the pipe's contents 
	// before disconnecting. Then disconnect the pipe, and close the 
	// handle to this pipe instance. 

	FlushFileBuffers(hPipe);
	DisconnectNamedPipe(hPipe);
	CloseHandle(hPipe);

	HeapFree(hHeap, 0, pchRequest);
	HeapFree(hHeap, 0, pchReply);

	printf("InstanceThread exitting.\n");
	return 0;
}
#pragma endregion

#pragma region CLIENT
Client* Client::client_Instance = 0;

Client* Client::client_GetInstance()
{
	if (client_Instance == 0)
		client_Instance = new Client();
	return client_Instance;
}

int Client::ClientInitConfiguration(string tmpClientNetworkHostName, string tmpClientPipeName)
{
	try
	{
		Client* client_Instance = Client::client_GetInstance();
		client_Instance->clientConfiguration = new ClientConfiguration(tmpClientNetworkHostName, tmpClientPipeName);
	}
	catch (...)
	{
		return -1;
	}
	return 0;
}

/*
Creates Pipe handle with given client configuration and tries to connect to this pipe
If no pipe instance on the server should be available 10 reconnects will be tried before exiting with code -4
*/
int Client::ClientConnectToServerPipe()
{
	Client* client_Instance = Client::client_GetInstance();
	HANDLE tmpPipeHandle = nullptr;
	int cntConnectionAttempts = 0;
	DWORD dwMode = NULL;
	BOOL fSuccess = FALSE;

	//check for existing client configuration
	if (client_Instance->clientConfiguration == nullptr)
	{
		return -1;
	}

	string tmpPipeName = "\\\\" + client_Instance->clientConfiguration->Get_NetworkHostName() + "\\pipe\\" + client_Instance->clientConfiguration->Get_ClientPipeName();
	wchar_t* wstrPipeName = Helper::s2wct(tmpPipeName);

	while (true)
	{
		try
		{
			tmpPipeHandle = CreateFile(
				wstrPipeName,   // pipe name 
				GENERIC_READ |  // read and write access 
				GENERIC_WRITE,
				0,              // no sharing 
				NULL,           // default security attributes
				OPEN_EXISTING,  // opens existing pipe 
				0,              // default attributes 
				NULL);          // no template file 
		}
		catch (...)
		{
			return -2;
		}

		//if the pipe handle is valid the connection loop can be terminated
		if (tmpPipeHandle != INVALID_HANDLE_VALUE)
		{
			client_Instance->clientPipeHandle = tmpPipeHandle;
			break;
		}

		if (GetLastError() != ERROR_PIPE_BUSY)
		{
			return -3;
		}

		//after 10 connection attempts to the server every 3 seconds the connection failed with exit code -4
		if (!WaitNamedPipe(wstrPipeName, 3000))
		{
			if (cntConnectionAttempts >= 10)
			{
				return -4;
			}
		}

		cntConnectionAttempts++;
	}

	dwMode = PIPE_READMODE_MESSAGE;
	fSuccess = SetNamedPipeHandleState(
		client_Instance->clientPipeHandle,    // pipe handle 
		&dwMode,  // new pipe mode 
		NULL,     // don't set maximum bytes 
		NULL);    // don't set maximum time 

	if (!fSuccess)
	{
		return -5;
	}



	cout << "ClientConnectToServerPipe returned with 0" << endl;
	return 0;
}

int Client::ClientSendMessage(string tmpMessageContent)
{
	Client* client_Instance = Client::client_GetInstance();
	BOOL fSuccess = false;
	wchar_t* wstrPipeName = Helper::s2wct(tmpMessageContent);

	DWORD  cbToWrite, cbWritten = NULL;

	if (client_Instance->clientPipeHandle == nullptr)
	{
		return -1;
	}

	try
	{
		cbToWrite = (lstrlen(wstrPipeName) + 1) * sizeof(TCHAR);

		fSuccess = WriteFile(
			client_Instance->clientPipeHandle,                  // pipe handle 
			wstrPipeName,             // message 
			cbToWrite,              // message length 
			&cbWritten,             // bytes written 
			NULL);                  // not overlapped 
	}
	catch (...)
	{
		return -2;
	}

	if (!fSuccess)
	{
		return -3;
	}
	else
	{
		return 0;
	}
}

#pragma endregion

#pragma region SERVER CONFIGURATION
void ServerConfiguration::Set_ServerPipeName(string tmpServerPipeName)
{
	serverPipeName = tmpServerPipeName;
}

string ServerConfiguration::Get_ServerPipeName()
{
	return serverPipeName;
}

void ServerConfiguration::Set_ServerNetworkHostName(string tmpNetworkHostName)
{
	serverNetworkHostName = tmpNetworkHostName;
}

string ServerConfiguration::Get_NetworkHostName()
{
	return serverNetworkHostName;
}

void ServerConfiguration::Set_ServerPipeMaxInstances(int tmpPipeMaxInstances)
{
	serverPipeMaxInstances = tmpPipeMaxInstances;
}

int ServerConfiguration::Get_ServerPipeMaxInstances()
{
	return serverPipeMaxInstances;
}

void ServerConfiguration::Set_ServerOutBufferSize(int tmpOutBufferSize)
{
	serverOutBufferSize = tmpOutBufferSize;
}

int ServerConfiguration::Get_ServerOutBufferSize()
{
	return serverOutBufferSize;
}

void ServerConfiguration::Set_ServerInBufferSize(int tmpInBufferSize)
{
	serverInBufferSize = tmpInBufferSize;
}

int ServerConfiguration::Get_ServerInBufferSize()
{
	return serverInBufferSize;
}

#pragma endregion

#pragma region CLIENT CONFIGURATION
void ClientConfiguration::Set_ClientPipeName(string tmpClientPipeName)
{
	clientPipeName = tmpClientPipeName;
}

string ClientConfiguration::Get_ClientPipeName()
{
	return clientPipeName;
}

void ClientConfiguration::Set_ClientNetworkHostName(string tmpNetworkHostName)
{
	clientNetworkHostName = tmpNetworkHostName;
}

string ClientConfiguration::Get_NetworkHostName()
{
	return clientNetworkHostName;
}
#pragma endregion
