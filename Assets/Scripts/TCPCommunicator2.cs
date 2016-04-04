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

    /*
    string[] host = new String[4]
    {
        "localhost",
        "localhost",
        "localhost",
        "localhost"
    };
    */
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

    private int[] wait = new int[2];

    // setting
    public bool connect_async = true;

    // Use this for initialization
    void Start ()
    {
        wait[0] = 0;
        wait[1] = 0;

    }

    // Update is called once per frame
    void Update () {
	    if(counter < 1000)
        {
            if (counter % 100 == 0)
            {
                if (connect_async)
                {
                    init_connection_async();
                }
                else
                {
                    tid = new Thread(new ThreadStart(init_connection));
                    tid.Start();
                }
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
            tException = null;
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            close_connection();
        }

        if (wait[0] > 0)
        {
            wait[0]--;
        }
        if (wait[1] > 0)
        {
            wait[1]--;
        }
        if (wait[0] == 1)
        {
            controlSpray(1, 1);
        }
        if (wait[1] == 1)
        {
            controlSpray(1, 2);
        }

	}

    public void init_connection()
    {
            if (i < 4)
            {
                int tport = port + i;
                tmessage = "Connecting Peer " + i + " (" + host[i] + ", " + tport + ")";
                try
                {
                    tcp[i] = new TcpClient();
                    tcp[i].Client.ReceiveTimeout = 100;
                    tcp[i].Connect(host[i], tport);
                    tmessage += "... OK";
                } catch (Exception e)
                {
                    tmessage += "... ERROR";
                    tException = e;
                }
            }
            i++;
        
    }

    void init_connection_async()
    {
        if (i < 4)
        {
            int tport = port + i;
            tmessage = "Connecting Peer " + i + " (" + host[i] + ", " + tport + ")";
            try
            {
                tcp[i] = new TcpClient(AddressFamily.InterNetwork);
                tcp[i].Client.ReceiveTimeout = 100;
                tcp[i].BeginConnect(host[i], tport, connect_callback, tcp[i]);
            } catch (Exception e)
            {
                tmessage += "... NOT ABLE TO ESTABLISH";
                tException = e;
            }
        }
        i++;
    }

    public void connect_callback(IAsyncResult ar)
    {
        TcpClient client = (TcpClient)ar.AsyncState;
        if (ar.IsCompleted && client.Connected)
        {
            tmessage += "... OK";
        }
        else
        {
            tmessage += "... ERROR";
        }
    }

    public void controlSpray(int dir, int id) // yama 0321
    {
        if (tcp[id - 1] != null)
        {
            if (dir == 0)
            {
                wait[id - 1] = 100;
            }
            if (tcp[id - 1].Connected)
            {
                String mes = dir.ToString();
                byte[] umsg = Encoding.UTF8.GetBytes(mes + "\n");
                tcp[id - 1].Client.Send(umsg);
            }
        }
    }

    void controlBaketsu(int id)
    {
        
        String mes = "1";
        byte[] umsg = Encoding.UTF8.GetBytes(mes);
        //str[id].Write(umsg, 0, umsg.Length);
        tcp[id].Client.Send(umsg);
        Debug.Log("id = " + id);
        
    }

    void close_connection()
    {
        for (int i = 0; i < tcp.Length; i++)
        {
            if (tcp[i] != null)
            {
                Debug.Log("Disconnected with peer " + i);
                tcp[i].Close();
            }
        }
    }

    void OnApplicationQuit()
    {
        close_connection();
    }
}
