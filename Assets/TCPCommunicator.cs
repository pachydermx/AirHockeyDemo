using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TCPCommunicator : MonoBehaviour {
    string host = "192.168.1.190";
    int port = 10001;
    static TcpClient tcp;
    Thread thread;

    bool msg = false;

    int fuel = 50;

    // Use this for initialization
    void Start () {
        tcp = new TcpClient(host, port);
        tcp.Client.ReceiveTimeout = 1000;

        tcp.Connect(host, port);

    }

    // Update is called once per frame
    void Update () {
	
	}

    void controlSpray(int dir) // yama 0321
    {
        String mes = dir.ToString();
        byte[] umsg = Encoding.UTF8.GetBytes(mes);
        //byte[] umsg = Encoding.UTF8.GetBytes("1");
        //tcp.Send(umsg, umsg.Length);
        //tcp.
        NetworkStream stream = tcp.GetStream();
        stream.Write(umsg, 0, umsg.Length);
        Debug.Log("dir = " + mes);
    }
}
