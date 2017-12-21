using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Sharp_Client
{
    class Program
    {
        // Emil -----
        Thread send, receive; // The threads that handle the respective aspects of the connection
        Socket socket;        // The socket that is used to create the connection

        bool connected = false; // Looping purposes

        static void Main(string[] args)
        {
            Program prog = new Program(); // Have to instantiate the class to access the global variabes in the Main function

            prog.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Setup the socket
            prog.socket.Blocking = true; // Wait for send / receive functions to actually send or receive something before continuing.

            prog.socket.Connect(IPAddress.Parse("127.0.0.1"), 5000); // Attach socket to given IP and Port
            prog.connected = true; // If it goes down here successfully we know it's got a connection; we're connected!

            // 'Instantiate' threads
            prog.send = new Thread(prog.sendData);          
            prog.receive = new Thread(prog.receiveData);

            Console.WriteLine("Connected to server - write something!\nWrite 'exit' to shutdown gracefully.");

            // Start threads
            prog.send.Start();
            prog.receive.Start();

        }

        void receiveData()
        {
            // Boas -----
        }

        void sendData()
        {
            // Andrej -----
        }



    }
}
