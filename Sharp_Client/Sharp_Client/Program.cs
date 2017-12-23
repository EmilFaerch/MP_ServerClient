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

        // Boas -----
        void receiveData()
        {
            while (connected)
            {
                try // Added for more graceful exit
                {
                    if (socket.Connected) // Stops crashes when exiting... sometimes
                    {
                        byte[] data = new byte[1024]; // create byte array
                        int receivedData = socket.Receive(data); // int holds amount of characters in array: "1234" = 4

                        if (receivedData > 0) // if received something
                        {
                            Array.Resize(ref data, receivedData); // avoid whitespace by resizing byte array to only hold the amount of characters received

                            string msg = Encoding.ASCII.GetString(data); // convert from ASCII numbers to string

                            if (msg == "exit") // if server exited
                            {
                                // stop connection
                                connected = false;
                                socket.Close();
                            }

                            Console.WriteLine("Server: " + msg); // display received message, exit or not
                        }
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong.");
                }
            }
        }

        // Andrej -----
        void sendData()
        {
            while (connected) // While connected to the server
            {
                if (socket.Connected)
                {
                    string text = Console.ReadLine(); // Read the user input from the console
                    byte[] msg = Encoding.ASCII.GetBytes(text); // convert string to an array of bytes

                    socket.Send(msg, 0, msg.Length, SocketFlags.None); // Send message to the server with first index 0 and last index is total length of message

                    if (text == "exit") // If we wish to exit
                    {
                        connected = false; // Disable loop ...
                        socket.Close();     // ... close the socket ...

                        Environment.Exit(0); // ... aand terminate the program
                    }
                }
            }
        }



    }
}
