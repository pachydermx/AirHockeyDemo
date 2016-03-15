using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class UDPCommunicator : MonoBehaviour {

    string host = "192.168.1.190";
    int port = 10001;
    static UdpClient udp;
    Thread thread;

    bool msg = false;

    int fuel = 50;

	// Use this for initialization
	void Start () {
        udp = new UdpClient(port);
        udp.Client.ReceiveTimeout = 1000;

        udp.Connect(host, port);

        //thread = new Thread(new ThreadStart(Message));
        //thread.Start();
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.O))
        {
            test();
        }
        
        if (Input.GetKeyUp(KeyCode.O))
        {
            Debug.Log("false");
            msg = false;
        }
        fuel = 50;
	}

    void test()
    {
        byte[] umsg = Encoding.UTF8.GetBytes("hello");
        udp.Send(umsg, umsg.Length);
        Debug.Log("MSG");

    }

    void Message()
    {
        while (true)
        {
            try
            {
                /*
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] msg = Encoding.UTF8.GetBytes("hello");
                udp.Send(msg, msg.Length, anyIP);
                */
                IPEndPoint remoteEP = null;
                //byte[] data = udp.Receive(ref remoteEP);
                //string text = Encoding.UTF8.GetString(data);

                //Debug.Log(msg);
                if (msg)
                {
                }
                byte[] sdata = Encoding.UTF8.GetBytes("hello\n");
                udp.Send(sdata, sdata.Length, remoteEP);
                Debug.Log("Send Message");
            } catch (Exception err)
            {
                //
            }
            if (fuel < 0)
            {
                break;
            }
            fuel--;
            //Debug.Log("Send Message");

        }
    }
}
