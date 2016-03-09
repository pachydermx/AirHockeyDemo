using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Net;
using System.Net.Sockets;
using System.Threading;

public class SocketTest : MonoBehaviour {
    Communicator c;

    // text
    public GameObject DebugText;
    UnityEngine.UI.Text debugText;
    protected string[] infoFromText;
    protected Vector3 singlePointPointerForDebug;

    // cursor
    public GameObject Cursor;

    // calibration
    protected Vector3[] calibrationPoints = new Vector3[4];
    protected int calibrationPointSet = 0;
    protected Vector3[] calibrationDefaults = { new Vector3(4f, 3f, 0), new Vector3(-4f, 3f, 0), new Vector3(-4f, -3f, 0), new Vector3(4f, -3f, 0) };
    protected bool calibrationComplete = false;

    // debug info
    public string dtext;
    protected string ctext;

    void Start()
    {
        // config server
        c = new Communicator();
        c.initServer();

        // config communicator
        c.st = this;
        debugText = DebugText.GetComponent<UnityEngine.UI.Text>();

        // start calibration
        moveCursor(calibrationDefaults[calibrationPointSet]);

        // config debug
        singlePointPointerForDebug = new Vector3();
    }

    void Update()
    {
        // request info
        Thread tid = new Thread(new ThreadStart(c.GetInfo));
        tid.Start();

        // get coordinate from dtext
        infoFromText = dtext.Split(',');
        if (infoFromText.Length >= 4)
        {
            ctext = "(" + infoFromText[1] + ", " + infoFromText[2] + ", " + infoFromText[3] + ")";
            // get vector
            singlePointPointerForDebug = new Vector3(float.Parse(infoFromText[1]), float.Parse(infoFromText[2]), float.Parse(infoFromText[3]));
        } else
        {
            ctext = "unable to get information from socket";
        }

        // display info
        debugText.text = ctext;

        // do calibrate
        if (Input.GetKeyDown(KeyCode.F14))
        {
            setCalibrationPoint();
        }

        // move cursor for debug
        if (calibrationComplete)
        {
            moveCursor(getRealCoordinate(singlePointPointerForDebug));
        }
    }

    void moveCursor(Vector3 newPosition)
    {
        Cursor.transform.position = newPosition;
    }

    Vector3 getRealCoordinate(Vector3 rawCoordinate)
    {
        Vector3 output = new Vector3();
        return output;
    }

    void setCalibrationPoint()
    {
        if (calibrationPointSet < 4)
        {
            calibrationPoints[calibrationPointSet] = singlePointPointerForDebug;
            // prepare for next calibration
            calibrationPointSet += 1;
            moveCursor(calibrationDefaults[calibrationPointSet]);
        } else
        {
            // finish
            calibrationComplete = true;
        }
        for (int i = 0; i < 4; i++)
            Debug.Log(calibrationPoints[i]);
    }
}

public class Communicator
{
    Socket host;
    Socket client;

    public SocketTest st;

    bool connected = false;

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
            Debug.Log("connected");
            client.Send(System.Text.Encoding.UTF8.GetBytes("hello"));

        }
        byte[] buffer = new byte[1024];
        int bytesRec = client.Receive(buffer);
        string recvText = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRec);

        // debug
        st.dtext = recvText;

        /*
        if (host.Connected)
        {
            Debug.Log("connected 2");
            host.Shutdown(SocketShutdown.Both);
        }
        */
	}
}