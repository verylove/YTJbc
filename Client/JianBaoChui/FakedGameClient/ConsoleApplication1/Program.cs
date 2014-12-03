﻿using System;

using System.Net.Sockets;

using System.Text;



namespace ConsoleApplication1
{

    class Program
    {

        static void Main(string[] args)
        {

            TcpListener serverSocket = new TcpListener(8888);

            int requestCount = 0;

            TcpClient clientSocket = default(TcpClient);

            serverSocket.Start();

            Console.WriteLine(" >> Server Started");

            clientSocket = serverSocket.AcceptTcpClient();

            Console.WriteLine(" >> Accept connection from client");

            requestCount = 0;



            while ((true))
            {

                try
                {

                    requestCount = requestCount + 1;

                    NetworkStream networkStream = clientSocket.GetStream();

                    byte[] bytesFrom = new byte[10025];

                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);

                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    Console.WriteLine(" >> Data from client - " + dataFromClient);

                    string serverResponse = "Server response " + Convert.ToString(requestCount);

                    Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);

                    networkStream.Write(sendBytes, 0, sendBytes.Length);

                    networkStream.Flush();

                    Console.WriteLine(" >> " + serverResponse);

                }

                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());

                }

            }



            clientSocket.Close();

            serverSocket.Stop();

            Console.WriteLine(" >> exit");

            Console.ReadLine();

        }

    }

}

