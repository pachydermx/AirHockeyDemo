using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TCPCommunicator2 : MonoBehaviour {
    string[] host = new String[4] { "192.168.1.190",
                                    "192.168.1.191",
                                    "192.168.1.192",
                                    "192.168.1.193"};
    int port = 10001;
    static TcpClient[] tcp = new TcpClient[4];
    static NetworkStream[] str = new NetworkStream[4];
    Thread thread;

    bool msg = false;

    int fuel = 50;

    static int max = 4;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < max; i++)
        {
            tcp[i] = new TcpClient(host[i], port+i);
            tcp[i].Client.ReceiveTimeout = 1000;
            tcp[i].Connect(host[i], port+i);
            str[i] = tcp[i].GetStream();
            Debug.Log("connect:"+host[i]);
        }

        

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void controlSpray(int dir, int id) // yama 0321
    {
        String mes = dir.ToString();
        byte[] umsg = Encoding.UTF8.GetBytes(mes);
        //byte[] umsg = Encoding.UTF8.GetBytes("1");
        //tcp.Send(umsg, umsg.Length);
        //tcp.
        str[id].Write(umsg, 0, umsg.Length);
        Debug.Log("id = "+ id +", dir = " + mes);
        //str[1].Write(umsg, 0, umsg.Length);
        //Debug.Log("dir = " + mes);
    }
}
