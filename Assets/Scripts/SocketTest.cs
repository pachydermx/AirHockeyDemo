﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.Diagnostics; // yama 0316

using System.Collections.Generic;


public class SocketTest : MonoBehaviour {
    Communicator c;
    Thread tid;

    // text
    public GameObject DebugText;
    UnityEngine.UI.Text debugText;
    protected string[] rawInputs;
    protected string[] infoFromText;
    protected string[] infoTester;

    // cursor
    public GameObject Cursor;
    public GameObject TestCursor;
    public GameObject Canvas;
    public GameObject p1_wall;
    public GameObject p2_wall;
    protected Dictionary<string, GameObject> TestCursorList = new Dictionary<string, GameObject>();

    // real game object
    public GameObject Smasher;
    public GameObject Baketsu1; // yama 0317
    public GameObject Baketsu2; // yama 0328
    public GameObject Spray1; // yama 0328
    public GameObject Spray2; // yama 0328
    public GameObject manager; // yama 0321
    public GameObject Itembox; // yama 0323

    // calibration
    protected Vector3[] calibrationPoints = new Vector3[4];
    protected int calibrationPointSet = 0;
    protected Vector3[] calibrationDefaults = { new Vector3(-4f, -3f, 0), new Vector3(4f, 3f, 0), new Vector3(-4f, -3f, 0), new Vector3(4f, -3f, 0) };
    protected bool calibrationComplete = false;

    // points
    protected Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();

    // debug info
    public string dtext = "";
    protected string ctext;
    protected Dictionary<string, int> counter = new Dictionary<string, int>();
    protected bool debug = true;

    // level
    protected float touchLevel;
    protected bool levelSet = false;

    protected System.Diagnostics.Process vicon; // yama 0316

    void Start()
    {
        // config server
        c = new Communicator();

        // config communicator
        c.st = this;
        debugText = DebugText.GetComponent<UnityEngine.UI.Text>();

        c.initServer();

        // start calibration
        moveCursor(calibrationDefaults[calibrationPointSet]);

        // request vicon info from socket
        // raw message will be set to dtext variable
        tid = new Thread(new ThreadStart(c.GetInfo));
        tid.Start();

        // execute vicon client
        vicon = System.Diagnostics.Process.Start(Application.dataPath + "/ViconClient.exe");
        //Process vicon; // yama 0316
        //vicon = Process.Start("/ViconClient.exe"); //yama 0316

        //Baketsu.SetActive(false); // yama 0325 試験的にコメントアウト

    }

    void Update()
    {
        c.clock = 100;

        // split dtext
        rawInputs = dtext.Split('&');
        foreach(string rawInput in rawInputs)
        {
            // get coordinate from dtext
            // parse dtext into dictionary
            // split dtext for analyze
            infoFromText = rawInput.Split(',');
            if (infoFromText.Length >= 4)
            {
                // test if valid
                infoTester = infoFromText[0].Split(':');
                if (infoTester.Length == 2)
                {
                    if (infoTester[0].Equals(infoTester[1]))
                    {
                        string name = infoFromText[0];
                        Vector3 position = new Vector3(float.Parse(infoFromText[1]), float.Parse(infoFromText[2]), float.Parse(infoFromText[3]));
                        // get vector (for dictionary)
                        points[infoFromText[0]] = position;

                        // create cursors
                        if (!TestCursorList.ContainsKey(name))
                        {
                            if (name.Contains("baketsu1")) // 0317 yama <--
                            {
                                //Debug.Log("OK");
                                Baketsu1.SetActive(true);
                            }else if (name.Contains("baketsu2"))
                            {
                                Baketsu2.SetActive(true);
                            }
                            else if (name.Contains("Spray1"))
                            {
                                Spray1.SetActive(true);
                            }
                            else if (name.Contains("Spray2"))
                            {
                                Spray2.SetActive(true);
                            }
                            else
                            {
                                // create 
                                GameObject newCursor = (GameObject)Instantiate(TestCursor, Vector3.zero, Quaternion.identity);
                                // set parent
                                newCursor.transform.parent = Canvas.transform;
                                TestCursorList.Add(name, newCursor);

                                // create counter
                                counter.Add(name, 0);
                            } // -->

                            /*
                            // create 
                            GameObject newCursor = (GameObject)Instantiate(TestCursor, Vector3.zero, Quaternion.identity);
                            // set parent
                            newCursor.transform.parent = Canvas.transform;
                            TestCursorList.Add(name, newCursor);

                            // create counter
                            counter.Add(name, 0);
                            */
                        }
                        
                        //counter[name] += 1; // yama 0317
                    }
                }
            }
        }


        // generate debg text
        string dictext = "";
        foreach(KeyValuePair<string, Vector3> entry in points)
        {
            dictext += entry.Key + " - (" + entry.Value.x + ", " + entry.Value.y + ", " + entry.Value.z + ") \n";
        }
        /*
        foreach(KeyValuePair<string, int> entry in counter)
        {
            dictext += entry.Key + " - " + entry.Value + "\n";
        }
        */
        // display info
        debugText.text = dictext;

        // do calibrate
        if (Input.GetKeyDown(KeyCode.F14))
        {
            setCalibrationPoint();
        }
        if (Input.GetKeyDown(KeyCode.F15))
        {
            setlevel();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            loadSetting();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            saveSetting();
        }

        // move cursor for debug
        if (calibrationComplete)
        {
            foreach(KeyValuePair<string, Vector3> entry in points)
            {
                moveTestCursor(entry.Key, getRealCoordinate(entry.Value));
            }
            Smasher.SendMessage("Move", getRealCoordinate(points["Smasher1:Smasher1"]));
            //Baketsu1.transform.position = getRealCoordinate(points["baketsu1:baketsu1"]); // yama 0328
            //Baketsu2.transform.position = getRealCoordinate(points["baketsu2:baketsu2"]); // yama 0328
            Spray1.transform.position = getRealCoordinate(points["Spray1:Spray1"]); // yama 0318 // yama 0328
            Spray2.transform.position = getRealCoordinate(points["Spray2:Spray2"]); // yama 0328
        }
        if (levelSet)
        {
            if(points["Pen1:Pen1"].z < touchLevel)
            {
                deployWall(getRealCoordinate(points["Pen1:Pen1"]));
            }
            if(points["Pen2:Pen2"].z < touchLevel)
            {
                deployWall(getRealCoordinate(points["Pen2:Pen2"]));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q)) { // yama 0316 finish viconclient
            vicon.CloseMainWindow();
            vicon.Close();
        }
    }

    void deployWall(Vector3 position)
    {
        if(position.x > 0)
        {
            GameObject nw = (GameObject)Instantiate(p1_wall, Vector3.zero, Quaternion.identity);
            nw.transform.parent = Canvas.transform;
            nw.transform.position = position;

        } else
        {
            GameObject nw = (GameObject)Instantiate(p2_wall, Vector3.zero, Quaternion.identity);
            nw.transform.parent = Canvas.transform;
            nw.transform.position = position;
        }
    }

    void moveCursor(Vector3 newPosition)
    {
        Cursor.transform.position = newPosition;
    }

    void moveTestCursor(string name, Vector3 newPositon)
    {
        if (TestCursorList.ContainsKey(name))
        {
            TestCursorList[name].transform.position = newPositon;
        } else
        {
            //Debug.Log("Cannot found " + name);  // yama 0317 
        }

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
            calibrationPoints[calibrationPointSet] = points["Smasher1:Smasher1"];
            // prepare for next calibration
            calibrationPointSet += 1;
            moveCursor(calibrationDefaults[calibrationPointSet]);
        }
        if (calibrationPointSet == 2){
            // finish
            calibrationComplete = true;
            Cursor.SetActive(false);

            Smasher.SetActive(true);
        }
        for (int i = 0; i < 2; i++) 
            Debug.Log(calibrationPoints[i]); 
    }

    void setlevel()
    {
        touchLevel = points["Pen1:Pen1"].z;
        levelSet = true;
    }

    void saveSetting()
    {
        Debug.Log("Saving Calibration"); 
        if (calibrationComplete && levelSet)
        {
            PlayerPrefs.SetFloat("cp0x", calibrationPoints[0].x);
            PlayerPrefs.SetFloat("cp0y", calibrationPoints[0].y);
            PlayerPrefs.SetFloat("cp0z", calibrationPoints[0].z);

            PlayerPrefs.SetFloat("cp1x", calibrationPoints[1].x);
            PlayerPrefs.SetFloat("cp1y", calibrationPoints[1].y);
            PlayerPrefs.SetFloat("cp1z", calibrationPoints[1].z);

            PlayerPrefs.SetFloat("level", touchLevel);

            PlayerPrefs.SetInt("set", 1);
            PlayerPrefs.Save();
            Debug.Log("Calibration Saved"); 
        } else
        {
            Debug.LogError("Calibration is not complete");
        }
    }

    void loadSetting()
    {
        Debug.Log("Loading Calibration");
        if (PlayerPrefs.HasKey("set"))
        {
            calibrationPoints[0] = new Vector3(PlayerPrefs.GetFloat("cp0x"), PlayerPrefs.GetFloat("cp0y"), PlayerPrefs.GetFloat("cp0z"));
            calibrationPoints[1] = new Vector3(PlayerPrefs.GetFloat("cp1x"), PlayerPrefs.GetFloat("cp1y"), PlayerPrefs.GetFloat("cp1z"));

            touchLevel = PlayerPrefs.GetFloat("level");

            Cursor.SetActive(false);
            calibrationComplete = true;
            levelSet = true;

            // config objects
            Smasher.SetActive(true);

            Debug.Log("Calibration Loaded."); 
        } else
        {
            Debug.LogError("Save is not found"); 
        }
    }

    /*
    void sendB2position() // yama 0317
    {
        //Debug.Log("manager OK");
        Canvas.SendMessage("DoSprinkle", Baketsu.transform.position);
    }
    */

    void sendPosition(String name) // yama 0318 GimmickDiscrimination
    {
        if (name.Contains("Spray1")) // yama 0321
        {
            //Debug.Log("Gimmick name:"+Spray1.name);
            Spray1.SendMessage("stopFlag", 1);
            Canvas.GetComponent<Canvas>().DoSpray(Spray1.transform.position, 1);
            //Canvas.SendMessage("DoSpray", Spray1.transform.position);
        }
        else if (name.Contains("Spray2"))
        {
            Spray2.SendMessage("stopFlag", 1);
            Canvas.GetComponent<Canvas>().DoSpray(Spray2.transform.position, 2);
            //Canvas.SendMessage("DoSpray", Spray2.transform.position);
        }
        else if (name.Contains("Baketsu1"))
        {
            Canvas.SendMessage("DoSprinkle", Baketsu1.transform.position);
            manager.SendMessage("controlBaketsu", 2);
        }else if (name.Contains("Baketsu2"))
        {
            Canvas.SendMessage("DoSprinkle", Baketsu2.transform.position);
            manager.SendMessage("controlBaketsu", 3);
        }
        else if (name.Contains("ItemBox"))
        {
            Itembox.SendMessage("ItemUse");
        }
    }
}

public class Communicator
{
    Socket host;
    Socket client;
    bool receiving = false;

    public int clock = 100;

    public SocketTest st;

	// Use this for initialization
	public void initServer () {
        host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        host.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8889));
        host.Listen(100);

        //host.Close();
	
	}
	
	// Update is called once per frame
	public void GetInfo () {
        while (true)
        {

            if (!receiving)
            {
                receiving = true;
                try
                {
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
                    //Debug.Log(recvText);
                    st.dtext = recvText;
                } catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }

                clock--;
                if(clock < 0)
                {
                    break;
                }

                receiving = false;
            }

        }
	}
}

