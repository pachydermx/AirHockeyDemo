﻿using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TCPCommunicator2 : MonoBehaviour {
    /*
    string[] host = new String[4] { "192.168.1.190",
                                    "192.168.1.191",
                                    "192.168.1.192",
                                    "192.168.1.193"};
                                    */

    string[] host = new String[4]
    {
        "localhost",
        "localhost",
        "localhost",
        "localhost"
    };
    int port = 10001;
    static TcpClient[] tcp = new TcpClient[4];
    static NetworkStream[] str = new NetworkStream[4];
    Thread thread;

    bool msg = false;

    int fuel = 50;

    static int max = 4;

    private int i = 0;
    private int counter = 0;

    private Thread tid;
    private string tmessage = "";
    private Exception tException;

    // Use this for initialization
    void Start () {
        

    }

    // Update is called once per frame
    void Update () {
	    if(counter < 1000)
        {
            if (counter % 100 == 0)
            {
                tid = new Thread(new ThreadStart(init_connection));
                tid.Start();
            }
        counter++;
        }

        if (tmessage.Length > 0)
        {
            Debug.Log(tmessage);
            tmessage = "";
        }
        if (tException != null)
        {
            Debug.LogException(tException);
        }
	}

    public void init_connection()
    {
                //if (i < 4)
                if(i == 2)
                {
                    int tport = port + i;
                    tmessage = "Connecting Peer " + i + "(" + host[i] + ", " + tport + ")";
                    try
                    {
                        tcp[i] = new TcpClient(host[i], port+i);
                        tcp[i].Client.ReceiveTimeout = 100;
                        tcp[i].Connect(host[i], tport);
                        //str[i] = tcp[i].GetStream();
                        tmessage += "... OK";
                    } catch (Exception e)
                    {
                        tmessage += "... ERROR";
                        tException = e;
                    }
                }
                i++;
        
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
            Debug.Log("id = " + id + ", dir = " + mes);
        }
        
        //str[1].Write(umsg, 0, umsg.Length);
        //Debug.Log("dir = " + mes);
    }

    void controlBaketsu(int id)
    {
        String mes = "1";
        byte[] umsg = Encoding.UTF8.GetBytes(mes);
        str[id].Write(umsg, 0, umsg.Length);
        Debug.Log("id = " + id);
    }
}
