using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prototype
{
    class PrototypeServer
    {
		private static int numThreads = 4;

		public static void Main()
		{
			int i;
			Thread[] servers = new Thread[numThreads];

			Console.WriteLine("\n*** Named pipe server stream with impersonation example ***\n");
			Console.WriteLine("Waiting for client connect...\n");
			for(i = 0; i < numThreads; i++)
			{
				servers[i] = new Thread(serverThread);
				servers[i].Start();
			}
			Thread.Sleep(250);
			while(i > 0)
			{
				for(int j = 0; j < numThreads; j++)
				{
					if(servers[j] != null)
					{
						if(servers[j].Join(250))
						{
							Console.WriteLine("Server thread[{0}] finished.", servers[j].ManagedThreadId);
							servers[j] = null;
							i--;    // decrement the thread watch count
						}
					}
				}
			}
			Console.WriteLine("\nServer threads exhausted, exiting.");
		}


		private static void serverThread()
        {
            // Creating a new pipeServer object.
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1);
            int threadId = Thread.CurrentThread.ManagedThreadId;

            //Waiting for a client to connect
            pipeServer.WaitForConnection();
            try
            {
                // Read the request from the client. Once the client has written to the pipe its security token will be available.
                StreamString ss = new StreamString(pipeServer);

                // Verify our identity to the connected client using a string that the client anticipates.
                ss.WriteString("I am the one true server!");
                string filename = ss.ReadString();

                // Read in the contents of the file while impersonating the client.
                ReadFileToStream fileReader = new ReadFileToStream(ss, filename);

                // Display the name of the user we are impersonating.
                Console.WriteLine("Reading file: {0} on thread[{1}] as user: {2}.",
                    filename, threadId, pipeServer.GetImpersonationUserName());
                pipeServer.RunAsClient(fileReader.Start);
            }
            catch (IOException ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
            pipeServer.Close();
        }
    }

    // Contains the method executed in the context of the impersonated user
    public class ReadFileToStream
    {
        private string fn;
        private StreamString ss;

        public ReadFileToStream(StreamString str, string filename)
        {
            fn = filename;
            ss = str;
        }

        public void Start()
        {
            string contents = File.ReadAllText(fn);
            ss.WriteString(contents);
        }
    }
}
