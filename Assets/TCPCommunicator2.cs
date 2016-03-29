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

    private int i = 0;
    private int counter = 0;

    // Use this for initialization
    void Start () {
        

    }

    // Update is called once per frame
    void Update () {
	    if(counter < 1000)
        {
            if (counter % 100 == 0)
            {
                if (i < 4)
                {
                    Debug.Log("connect:"+host[i]);
                    try
                    {
                        tcp[i] = new TcpClient(host[i], port+i);
                        tcp[i].Client.ReceiveTimeout = 1000;
                        tcp[i].Connect(host[i], port+i);
                        str[i] = tcp[i].GetStream();
                    } catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                i++;

            }
        }
        counter++;
	}

    public void controlSpray(int dir, int id) // yama 0321
    {
        String mes = dir.ToString();
        byte[] umsg = Encoding.UTF8.GetBytes(mes);
        //byte[] umsg = Encoding.UTF8.GetBytes("1");
        //tcp.Send(umsg, umsg.Length);
        //tcp.
        if (str[id] != null)
        {
            str[id].Write(umsg, 0, umsg.Length);
        }
        Debug.Log("id = "+ id +", dir = " + mes);
        //str[1].Write(umsg, 0, umsg.Length);
        //Debug.Log("dir = " + mes);
    }

    void controlBaketsu(int id)
    {
        String mes = 1.ToString();
        byte[] umsg = Encoding.UTF8.GetBytes(mes);
        str[id].Write(umsg, 0, umsg.Length);
        Debug.Log("id = " + id);
    }
}
