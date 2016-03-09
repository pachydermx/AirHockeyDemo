using UnityEngine;
using System.Collections;

using System.Net;
using System.Net.Sockets;
using System.Threading;

public class SocketTest : MonoBehaviour {
    Communicator c;

    void Start()
    {
        c = new Communicator();
        c.initServer();
    }

    void Update()
    {
        c.GetInfo();

    }
}

public class Communicator
{
    Socket host;
    Socket client;

	// Use this for initialization
	public void initServer () {
        host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        host.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8889));
        host.Listen(100);

        //host.Close();
	
	}
	
	// Update is called once per frame
	public void GetInfo () {
        if (host.Poll(0, SelectMode.SelectRead))
        {
            client = host.Accept();
            Debug.Log("connected 1");
            client.Send(System.Text.Encoding.UTF8.GetBytes("hello"));

            byte[] buffer = new byte[1024];
            int bytesRec = client.Receive(buffer);
            Debug.Log(System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRec));
        }

        if (host.Connected)
        {
            Debug.Log("connected 2");
            host.Shutdown(SocketShutdown.Both);
        }
	}

}