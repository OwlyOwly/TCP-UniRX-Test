using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UniRx;

public class TCPTestClient : MonoBehaviour
{
    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    #endregion

    private string currentCommand;

    private void Start()
    {
        ConnectToTcpServer();

        var clickStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0));

        clickStream.Last()
            .Subscribe(_ => PrepareExplosionCommand());

    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient("localhost", 8052);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        Debug.Log("server message received as: " + serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    private void SendCommandToServer(string command)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(command);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Command sent");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void PrepareSwitchOnCommand()
    {
        SendCommandToServer("ON");
    }

    public void PrepareSwitchOffCommand()
    {
        SendCommandToServer("OFF");
    }

    public void PrepareExplosionCommand()
    {
        SendCommandToServer("BOOM");
    }
}