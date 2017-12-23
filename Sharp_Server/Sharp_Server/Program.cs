using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Sharp_Server
{
    class Program
    {
        Thread send;

        Socket sck, acc; // 2 sockets
        bool clientConnected = false; // connection loop

        // Martin
        public static void Main()
        {
            Program self = new Program(); // Create object to reference variables

            self.sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // IPv4 adress, Stream not Datagram, TCP not UDP
            self.sck.Blocking = true; // waits for response

            self.sck.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000)); // Accept IP on port # 5000
            self.sck.Listen(0); // Listen for clients, 0 no queue (Not handling multiple clients)

            Console.WriteLine("Waiting for Client connection.");
            self.acc = self.sck.Accept(); // for some reason has to Accept into a new socket before it wants to work
            Console.WriteLine("Client connected.");

            self.clientConnected = true; // enable loops

            self.send = new Thread(self.sendMessage);
            self.send.Start(); // start send thread

        }

        void sendMessage()
        {
            while (clientConnected)
            {
                try
                {
                    string text = Console.ReadLine();   // Take input from user into a string
                    byte[] info = Encoding.ASCII.GetBytes(text);    // convert string

                    acc.Send(info, 0, info.Length, SocketFlags.None);   // Send everything from beginning to end (0 - info.Length)

                    if (text == "exit") // Want to exit?
                    {
                        clientConnected = false;   // Stop loop and sockets
                        acc.Close();
                        sck.Close();

                        send.Join(); // Stop thread

                        Environment.Exit(0); // Close program
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("Something went wrong"); // Avoid crashing the program
                }
            }
        }
    }
}
