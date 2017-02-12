// IPCComponent.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "IPCComponent.h"

DWORD WINAPI InstanceThread(LPVOID lpvParam);
int AuthorizeClientAtServer(HANDLE pipeHandle);

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

IPCCOMPONENT_API int server_RequestClientConnectionID(char* tmpClientConnectionName)
{
	return Server::RequestClientConnectionID(string(tmpClientConnectionName));
}

IPCCOMPONENT_API char* __stdcall server_RequestClientData(int tmpClientConnectionID)
{
	return Server::RequestClientData(tmpClientConnectionID);
}

IPCCOMPONENT_API int client_InitPipeConfiguration(char* tmpNetworkHostName, char* tmpClientPipeName, char* tmpClientName)
{
	return Client::ClientInitConfiguration(string(tmpNetworkHostName), string(tmpClientPipeName), string(tmpClientName));
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
vector<int> Server::availableConnections;
vector<tuple<string, int>> Server::clientConnectionList;
vector<list<string>> Server::dataStorage;

Server* Server::server_GetInstance()
{
	if (server_Instance == 0)
	{
		server_Instance = new Server();
		clientConnectionList.clear();
		dataStorage.clear();
		availableConnections.clear();

		//reset the information of the server
		availableConnections.clear();
		for (int i = 0; i < PIPE_UNLIMITED_INSTANCES; i++)
		{
			availableConnections.push_back(i);
		}

		for (int i = 0; i < PIPE_UNLIMITED_INSTANCES; i++)
		{
			list<string> tmp;
			dataStorage.push_back(tmp);
		}
	}

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

	SECURITY_ATTRIBUTES m_pSecAttrib;
	SECURITY_DESCRIPTOR* m_pSecDesc;

	m_pSecDesc = (SECURITY_DESCRIPTOR*)LocalAlloc(LPTR,
		SECURITY_DESCRIPTOR_MIN_LENGTH);

	InitializeSecurityDescriptor(m_pSecDesc,
		SECURITY_DESCRIPTOR_REVISION);


	SetSecurityDescriptorDacl(m_pSecDesc, TRUE, (PACL)NULL, FALSE);

	m_pSecAttrib.nLength = sizeof(SECURITY_ATTRIBUTES);
	m_pSecAttrib.bInheritHandle = TRUE;
	m_pSecAttrib.lpSecurityDescriptor = m_pSecDesc;

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
			NMPWAIT_USE_DEFAULT_WAIT,                        // client time-out 
			&m_pSecAttrib);

		if (hPipe == INVALID_HANDLE_VALUE)
		{
			return -2;
		}

		cout << "Waiting for client to connect!" << endl;

		//waiting for client to connect
		fConnected = ConnectNamedPipe(hPipe, NULL) ?
			TRUE : (GetLastError() == ERROR_PIPE_CONNECTED);

		if (fConnected)
		{
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

	DWORD cbBytesRead = 0;
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

	//getting saving index for the client´s connection session or error code
	int returnAuthorize = AuthorizeClientAtServer(hPipe);

	if (returnAuthorize < 0)
	{
		return returnAuthorize;
	}

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

			//TODO: define break statement which gets sent from client to terminate connection
			if (c_szText == "END")
			{
				break;
			}

			Server::dataStorage.at(returnAuthorize).push_back(c_szText);
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

	return 0;
}

/*
Registers a client at the server.
This is required for the custom data storage of the received data of a client.
Returns the saving index which will be used for the client
*/
int AuthorizeClientAtServer(HANDLE pipeHandle)
{
	HANDLE hHeap = GetProcessHeap();
	TCHAR* pchRequest = (TCHAR*)HeapAlloc(hHeap, 0, MAXMESSLENGTH * sizeof(TCHAR));
	char* c_szText = new char[MAXMESSLENGTH];

	DWORD cbBytesRead = 0;
	BOOL fSuccess = FALSE;
	int connectionID = MININT;

	fSuccess = ReadFile(
		pipeHandle,        // handle to pipe 
		pchRequest,    // buffer to receive data 
		MAXMESSLENGTH * sizeof(TCHAR), // size of buffer 
		&cbBytesRead, // number of bytes read 
		NULL);        // not overlapped I/O 

	if (fSuccess && cbBytesRead != 0)
	{
		wcstombs(c_szText, pchRequest, wcslen(pchRequest) + 1);

		string str(c_szText);
		//check if name already exists
		if (std::find_if(Server::clientConnectionList.begin(), Server::clientConnectionList.end(), [str](tuple<string, int> const& t) {return std::get<string>(t) == str; }) == Server::clientConnectionList.end())
		{
			try
			{
				connectionID = Server::availableConnections.back();
				Server::availableConnections.pop_back();
			}
			catch (...)
			{
				return -3;
			}

			if (connectionID != MININT)
			{
				Server::clientConnectionList.push_back(make_tuple(c_szText, connectionID));

				return connectionID;
			}
			else
			{
				return -4;
			}
		}
		else
		{
			return -2;
		}
	}
	else
	{
		return -1;
	}
}

/*
Looks up the connection ID for a given client connection name.
This connection ID is required to request data from the server of a certain client connection.
Return values can be the found connection ID or if < 0 error codes
*/
int Server::RequestClientConnectionID(string tmpClientConnectionName)
{
	Server *serverInstance = Server::server_GetInstance();

	//using find if to get iterator of searched item in tuple vector
	auto it = std::find_if(Server::clientConnectionList.begin(), Server::clientConnectionList.end(), [tmpClientConnectionName](tuple<string, int> const& t) {return std::get<string>(t) == tmpClientConnectionName; });

	//check if entry for given client connection name exists
	if (it != Server::clientConnectionList.end())
	{
		try
		{
			//return the connection ID of the found entry tuple
			return std::get<int>(*it);
		}
		catch (...)
		{
			return -2;
		}
	}
	//no entry with given client connection name found
	else
	{
		return -1;
	}
}

/*
Looks up the oldest entry of the requested client´s data
*/
char* Server::RequestClientData(int tmpClientConnectionID)
{
	Server *serverInstance = Server::server_GetInstance();

	if (serverInstance == nullptr)
	{
		return createReturnString("Server Instance not found.");
	}

	list<string> clientData;
	//get datavector for requested client
	if (tmpClientConnectionID < 0)
	{
		return createReturnString("Client connection ID not found.");
	}

	clientData = serverInstance->dataStorage.at(tmpClientConnectionID);

	if (clientData.size() <= 0)
	{
		return createReturnString("No new data available for client.");
	}
	//retrieving the first data element
	string resultString = clientData.front();

	//remove the queried element
	clientData.pop_front();

	//reinsert changed local dataset
	serverInstance->dataStorage.at(tmpClientConnectionID) = clientData;

	return createReturnString(resultString);
}

/*
Correctly formats a given string to return it in the c-interface
*/
char* Server::createReturnString(string tmpString)
{
	const char* convStr = tmpString.c_str();
	ULONG ulSize = strlen(convStr) + sizeof(char);

	char* pszReturn = NULL;
	pszReturn = (char*)::CoTaskMemAlloc(ulSize);
	strcpy(pszReturn, convStr);

	return pszReturn;
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

int Client::ClientInitConfiguration(string tmpClientNetworkHostName, string tmpClientPipeName, string tmpClientName)
{
	try
	{
		Client* client_Instance = Client::client_GetInstance();
		client_Instance->clientConfiguration = new ClientConfiguration(tmpClientNetworkHostName, tmpClientPipeName, tmpClientName);
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

		// Break if the pipe handle is valid. 
		if (tmpPipeHandle != INVALID_HANDLE_VALUE)
		{
			client_Instance->clientPipeHandle = tmpPipeHandle;
			break;
		}

		// Exit if an error other than ERROR_PIPE_BUSY occurs. 
		if (GetLastError() != ERROR_PIPE_BUSY)
		{
			return -3;
		}

		// All pipe instances are busy, so wait for sometime.
		if (!WaitNamedPipe(wstrPipeName, NMPWAIT_USE_DEFAULT_WAIT))
		{
		}
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

	//sending (hopefully) unique client name to server for authentification
	if (ClientSendMessage(client_Instance->clientConfiguration->Get_ClientName()) != 0)
	{
		return -6;
	}

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

void ClientConfiguration::Set_ClientName(string tmpClientName)
{
	clientName = tmpClientName;
}

string ClientConfiguration::Get_ClientName()
{
	return clientName;
}
#pragma endregion
