using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Collections.Generic;

public class SocketTest : MonoBehaviour {
    Communicator c;

    // text
    public GameObject DebugText;
    UnityEngine.UI.Text debugText;
    protected string[] infoFromText;
    protected string[] infoTester;
    protected Vector3 singlePointPointerForDebug;

    // cursor
    public GameObject Cursor;

    // calibration
    protected Vector3[] calibrationPoints = new Vector3[4];
    protected int calibrationPointSet = 0;
    protected Vector3[] calibrationDefaults = { new Vector3(-4f, -3f, 0), new Vector3(4f, 3f, 0), new Vector3(-4f, -3f, 0), new Vector3(4f, -3f, 0) };
    protected bool calibrationComplete = false;

    // points
    protected Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();

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
        // request vicon info fro socket
        // raw message will be set to dtext variable
        Thread tid = new Thread(new ThreadStart(c.GetInfo));
        tid.Start();

        // get coordinate from dtext
        // parse dtext into dictionary
        // split dtext for analyze
        infoFromText = dtext.Split(',');
        if (infoFromText.Length >= 4)
        {
            // test if valid
            infoTester = infoFromText[0].Split(':');
            if(infoTester.Length == 2)
            {
                if (infoTester[0].Equals(infoTester[1])){
                    ctext = infoFromText[0] + " - (" + infoFromText[1] + ", " + infoFromText[2] + ", " + infoFromText[3] + ")";
                    // get vector (for debug)
                    singlePointPointerForDebug = new Vector3(float.Parse(infoFromText[1]), float.Parse(infoFromText[2]), float.Parse(infoFromText[3]));
                    // get vector (for dictionary)
                    points[infoFromText[0]] = new Vector3(float.Parse(infoFromText[1]), float.Parse(infoFromText[2]), float.Parse(infoFromText[3]));
                }
            }

        } else
        {
            ctext = "unable to get information from socket";
        }

        // generate debg text
        string dictext = "";
        foreach(KeyValuePair<string, Vector3> entry in points)
        {
            dictext += entry.Key + " - (" + entry.Value.x + ", " + entry.Value.y + ", " + entry.Value.z + ") \n";
        }

        // display info
        debugText.text = dictext;

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
        float x_ori = calibrationDefaults[0].x;
        float x_delta = calibrationDefaults[1].x;

        float xs_ori = calibrationPoints[0].x;
        float xs_delta = calibrationPoints[1].x;

        float x_scur = rawCoordinate.x;

        float x_rate = (x_scur - xs_ori) / (xs_delta - xs_ori);
        float x_cur = x_ori + x_rate * (x_delta - x_ori);

        float y_ori = calibrationDefaults[0].y;
        float y_delta = calibrationDefaults[1].y;

        float ys_ori = calibrationPoints[0].y;
        float ys_delta = calibrationPoints[1].y;

        float y_scur = rawCoordinate.y;

        float y_rate = (y_scur - ys_ori) / (ys_delta - ys_ori);
        float y_cur = y_ori + y_rate * (y_delta - y_ori);

        Vector3 output = new Vector3(x_cur, y_cur, 0);
        return output;
    }

    void setCalibrationPoint()
    {
        if (calibrationPointSet < 2)
        {
            calibrationPoints[calibrationPointSet] = singlePointPointerForDebug;
            // prepare for next calibration
            calibrationPointSet += 1;
            moveCursor(calibrationDefaults[calibrationPointSet]);
        }
        if (calibrationPointSet == 2){
            // finish
            calibrationComplete = true;
        }
        for (int i = 0; i < 2; i++)
            Debug.Log(calibrationPoints[i]);
    }
}

public class Communicator
{
    Socket host;
    Socket client;

    public SocketTest st;

    //bool connected = false;

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