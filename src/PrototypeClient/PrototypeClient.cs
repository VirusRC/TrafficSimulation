using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrototypeClient
{
    class PrototypeClient
    {
        public static void Main(string[] Args)
        {
            NamedPipeClientStream pipeClient =
                new NamedPipeClientStream("ULBUM453-88944", "IncommingPipe",
                    PipeDirection.InOut, PipeOptions.None,
                    TokenImpersonationLevel.Impersonation);

            Console.WriteLine("Connecting to server...\n");
            pipeClient.Connect();

            StreamString ss = new StreamString(pipeClient);
            string tmp = string.Empty;
            while (!tmp.Contains("end"))
            {
                tmp = Console.ReadLine();
                ss.WriteString(tmp);

                Thread.Sleep(250);
            }
            
            Console.WriteLine("Message sent!");
            Console.ReadLine();


            pipeClient.Close();

            


        }
    }
}
