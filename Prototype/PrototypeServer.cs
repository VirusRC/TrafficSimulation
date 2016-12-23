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
        public static void Main()
        {
            Thread server = new Thread(serverThread);
            server.Start();
            Console.WriteLine("\nServer started.");
            int i = 1;
            while (i > 0)
            {
                if (server != null)
                {
                    if (server.Join(250))
                    {
                        server = null;
                        i--;    // decrement the thread watch count
                    }
                }
            }
            Console.WriteLine("\nServer threads exhausted, exiting.");
            Console.ReadLine();
        }


        private static void serverThread()
        {
            // Creating a new pipeServer object.
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("IncommingPipe", PipeDirection.In, 1);

            //Waiting for a client to connect
            pipeServer.WaitForConnection();
            try
            {
                // Read the request from the client. Once the client has written to the pipe its security token will be available.
                StreamString ss = new StreamString(pipeServer);

                while (true)
                {
                    string clientstring = ss.ReadString();
                    if (clientstring != string.Empty && clientstring != null)
                    {
                        Console.WriteLine(clientstring);
                    }
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
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
