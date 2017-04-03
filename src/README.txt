The prototype implementations for named pipes IPC using C# work.
Due to the fact that one of the hosts is based on Linux OS it would be required that Mono supports named pipes, which it does not at 25.12.2016.
It makes no sense to create different components for the hosts with the same OS, 
therefore a C++ dll using named pipes will be created for the IPC via all different OSs.
>!<Most important knowledge which was gathered during the prototype implementation is: 
- Hosts have to be in the same "Homenetwork" to communicate with each other via namedpipes. 
- Error if this is not the case: Unknown user or bad passwort at runtime.