﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {
	public GameObject p1_wall;
	public GameObject p2_wall;
    public GameObject manager;

    // textures
	public RawImage image;
    public RawImage normal;
	private Texture2D texture;
    private Texture2D normal_texture;

	private Color paint_color;
    private Color[] colors;
	private int[] ball_x;
	private int[] ball_y;
    private int n_ball = 0;
    private int[] last_ball_x;
    private int[] last_ball_y;
	private int width = 1920;
	private int height = 1080;

    /* yama 0323 
	private int default_range = 50;
	private int range;
    */
    public int default_range = 50;
    public int range;

    private int itemcount = 0;

    protected int[] scores;
    public GameObject P1Display;
    public GameObject P2Display;
    public GameObject ref_ball;
    private GameObject[] ball;
    protected bool animationPlaying = false;
    protected float animateCounter = 0;
    protected float P1DisplayTargetScaleX;
    protected float P2DisplayTargetScaleX;
    protected float maximumBar = 192;

    // 線の補間に使用
    private float temp_x;   // yama 0316
    private float temp_y;   // yama 0316
    private float dis_sum = 0;

	// Use this for initialization
	void Start () {
        // init variables
        int length = 8;
        ball_x = new int[length];
        ball_y = new int[length];
        last_ball_x = new int[length];
        last_ball_y = new int[length];
        ball = new GameObject[length]; 
        last_ball_x[0] = 0;
        last_ball_y[0] = 0;

		// init texture
		texture = image.texture as Texture2D;
        normal_texture = normal.texture as Texture2D;

		colors = new Color[width*height];
		for (int i =0; i < width*height; ++i)
        {
			colors[i] = Color.clear;
        }

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                normal_texture.SetPixel(x, y, Color.clear);
            }
        }

		texture.SetPixels(0, 0, width, height, colors);
        //normal_texture.SetPixels(0, 0, width, height, colors);

		// init properties
		paint_color = Color.clear;
		ball_x[0] = width / 2;
		ball_y[0] = height / 2;
		range = default_range;

        // init ball
        AddNewBall();
	}

	// Update is called once per frame
	void Update () {
        for (int i = 0; i < n_ball; i++)
        {
            DrawRound(i, range);
        }
        //Debug.Log("range:" + range);
		//DrawRound(range);

        /*
        // tanaka 0322
		if (range > default_range)
        {
            itemcount++;

            if (itemcount > 100)
            {
                //range -= 5;
                range = default_range;
                itemcount = 0;
            }
		}
        */

		// receive touch
		if (Input.touchCount > 0){
			Debug.Log(Input.GetTouch(0).position.x + ", "+ Input.GetTouch(0).position.y);
		}

		if (Input.GetMouseButton(0)){
            // deploy wall
            DeployWall(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        }

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            AddNewBall();
        }
        /*else  // yama 0318 未完成
        {
            temp_x = Input.mousePosition.x;
            temp_y = Input.mousePosition.y;
        }
        Debug.Log("temp_x:"+ temp_x +", temp_y:"+ temp_y);
        
        if (dis_sum > 4)
        {
            dis_sum -= 0.05f;
        }else if(dis_sum < 0){
            dis_sum = 0;
        }
        */

        // get score
        if (Input.GetKeyDown(KeyCode.F5))
        {
            StartScoreShow();
        }

        // show animation
        if (animationPlaying)
        {
            float scale_1 = P1DisplayTargetScaleX * animateCounter * animateCounter * animateCounter * animateCounter;
            float scale_2 = P2DisplayTargetScaleX * animateCounter * animateCounter * animateCounter * animateCounter;

            P1Display.transform.localScale = new Vector3(scale_1, 110, 1);
            P2Display.transform.localScale = new Vector3(scale_2, 110, 1);

            animateCounter += (float)0.02;
            if (animateCounter >= 1)
            {
                animationPlaying = false;
            }
        }
	}

    void AddNewBall()
    {
        ball[n_ball] = (GameObject)GameObject.Instantiate(ref_ball, Vector3.zero, Quaternion.identity);
        ball[n_ball].SendMessage("SetID", n_ball, SendMessageOptions.RequireReceiver);
        manager.SendMessage("AddNewBall", ball[n_ball]);

        n_ball++;
    }
    
	void SetColor (Color new_color){
		paint_color = new_color;
	}

	void SetCoordinate(float[] xynid) {
        float rate_x = (float)(xynid[0] / 9.6);
        float rate_y = (float)(xynid[1] / 5.4);

        int id = (int)xynid[2];

        ball_x[id] = (int)(rate_x * width / 2 + (width / 2));
        ball_y[id] = (int)(rate_y * height / 2 + (height / 2));

		//Debug.Log(ball_x + ", " + ball_y);
	}

    // this is an implementation of ecalipse painting
    void DrawRoundAt(int id, int pos_x, int pos_y, int radius, bool splash)
    {
		int x0 = pos_x;
		int y0 = pos_y;
		int x = radius;
		int y = 0;
		int decisionOver2 = 1 - x;

        // prevent glitch
        bool not_moving = false;
        if (x0 == last_ball_x[id] && y0 == last_ball_y[id])
        {
            not_moving = true;
        }

        // paint circle
		while (y <= x){
			int i;
            
			for (i = -x+x0; i < x+x0; ++i) {
				texture.SetPixel( i, y+y0, paint_color );
				texture.SetPixel( i, -y+y0, paint_color );
                if (!not_moving && !splash)
                {
                    normal_texture.SetPixel( i, y+y0,  GetNormalColor(id, x0, y0, i, y + y0, radius));
                    normal_texture.SetPixel( i, -y+y0,  GetNormalColor(id, x0, y0, i, -y + y0, radius));
                }
			}
            
			for (i = -y+x0; i < y+x0; ++i) {
				texture.SetPixel( i, x+y0, paint_color );
				texture.SetPixel( i, -x+y0, paint_color );
                if (!not_moving && !splash)
                {
                    normal_texture.SetPixel(i, x + y0, GetNormalColor(id, x0, y0, i, x + y0, radius));
                    normal_texture.SetPixel(i, -x + y0, GetNormalColor(id, x0, y0, i, -x + y0, radius));
                }
			}

			y++;
			if (decisionOver2 <= 0){
				decisionOver2 += 2 * y + 1;
			} else {
				x--;
				decisionOver2 += 2 * (y - x) + 1;
			}
		}


        // record ball posisition
        if (!splash)
        {
            last_ball_x[id] = ball_x[id];
            last_ball_y[id] = ball_y[id];
        }

		texture.Apply(false);
        normal_texture.Apply(false);
    }

	void DrawRound (int id, int radius) {
        DrawRoundAt(id, ball_x[id], ball_y[id], radius, false);
	}

    Color GetNormalColor(int id, int ball_x, int ball_y, int pos_x, int pos_y, int radius)
    {
        float grayscale;
        float percentage;
        if (Random.value > 0.01f)
        {
            // caculate normal color
            float distance = Mathf.Abs((last_ball_y[id] - ball_y) * pos_x - (last_ball_x[id] - ball_x) * pos_y + last_ball_x[id] * ball_y - last_ball_y[id] * ball_x) / Mathf.Sqrt((last_ball_y[id] - ball_y) * (last_ball_y[id] - ball_y) + (last_ball_x[id] - ball_x) * (last_ball_x[id] - ball_x));
            percentage = (float)(distance / radius);
            grayscale = 1.0f - percentage * 0.4f;
        } else
        {
            // add noise
            percentage = 0.5f + 0.5f * Random.value;
            grayscale = Random.value;
        }
        Color result = new Color(grayscale, grayscale, grayscale, 1 - percentage);
        return result;
    }

    Color GetNormalColorCentral(int x0, int y0, int radius, int pos_x, int pos_y)
    {
        float distance = Mathf.Sqrt((pos_x - x0) * (pos_x - x0) + (pos_y - y0) * (pos_y - y0));
        float percentage = (float)(distance / radius);
        float grayscale = 0 + percentage * 1;
        Color result = new Color(grayscale, grayscale, grayscale, percentage);
        return result;
    }

    void DoSprinkle(Vector3 position) // yama 0317 Baketsu Gimmick
    {
        float[] xy0 = { position.x, position.y };
        float rate_x = (float)(xy0[0] / 9.6);
        float rate_y = (float)(xy0[1] / 5.4);

        int x0 = (int)(rate_x * width / 2 + (width / 2));
        int y0 = (int)(rate_y * height / 2 + (height / 2));
        int x = 200;
        int y = 0;
        int decisionOver2 = 1 - x;
        
        /*
        for(int i = 0; i < 3600; i++)
        {
            float temp_x = x0 * Mathf.Cos(0.1f) + y0 * Mathf.Sin(0.1f);
            float temp_y = x0 * Mathf.Sin(0.1f) - y0 * Mathf.Cos(0.1f);
            x0 += (int)temp_x;
            y0 += (int)temp_y;
            texture.SetPixel(x0, y0, paint_color);
        }
        */

        
        while (y <= x)
        {
        
        int i; 
        
        for (i = -x + x0; i < x + x0; ++i)
            {
                texture.SetPixel(i, y + y0, paint_color);
                texture.SetPixel(i, -y + y0, paint_color);
            }

            for (i = -y + x0; i < y + x0; ++i)
            {
                texture.SetPixel(i, x + y0, paint_color);
                texture.SetPixel(i, -x + y0, paint_color);
            }

            y++;
            if (decisionOver2 <= 0)
            {
                decisionOver2 += 2 * y + 1;
            }
            else {
                x--;
                decisionOver2 += 2 * (y - x) + 1;
            }
        }
        
        texture.Apply(false);
    }

    void DoSpray(Vector3 position) // yama 0318 Spray Gimmick
    {
        float[] xy0 = { position.x, position.y };
        float rate_x = (float)(xy0[0] / 9.6);
        float rate_y = (float)(xy0[1] / 5.4);

        int x = (int)(rate_x * width / 2 + (width / 2));
        int y = (int)(rate_y * height / 2 + (height / 2)) + 50;

        for (int i = 1; i < 100; i++)
        {
            for (int j = 0; j < (i * 2 - 1); j++)
            {
                texture.SetPixel(x + j - (i * 2 - 1) / 2, y + i * 3, paint_color);
                texture.SetPixel(x + j - (i * 2 - 1) / 2, y + 1 + i * 3, paint_color);
                texture.SetPixel(x + j - (i * 2 - 1) / 2, y + 2 + i * 3, paint_color);
            }
        }

        //Debug.Log("Spray OK");
    }

	void DoExplode () {
		range = 100;
	}

    void DeployWall (Vector2 mouse_position) {
        float distance = ((mouse_position.x - temp_x) * (mouse_position.x - temp_x) + (mouse_position.y - temp_y) * (mouse_position.y - temp_y)); // yamamo 0316 -->
        
        dis_sum += distance;
        if (dis_sum < 4)
        {

            if (mouse_position.x < 0)
            {
                if (distance < 0.01)
                {
                    Instantiate(p1_wall, mouse_position, Quaternion.identity);
                }
                else
                {
                    int max = (int)(distance / 0.01);
                    Debug.Log(max);

                    for (int i = 0; i < max; i++)
                    {
                        Vector2 move = new Vector2((temp_x + i * (mouse_position.x - temp_x) / max), (temp_y + i * (mouse_position.y - temp_y) / max));
                        Instantiate(p1_wall, move, Quaternion.identity);
                    }
                }
            }
            else
            {
                if (distance < 0.01)
                {
                    Instantiate(p2_wall, mouse_position, Quaternion.identity);
                }
                else
                {
                    int max = (int)(distance / 0.01);
                    Debug.Log(max);

                    for (int i = 0; i < max; i++)
                    {
                        Vector2 move = new Vector2((temp_x + i * (mouse_position.x - temp_x) / max), (temp_y + i * (mouse_position.y - temp_y) / max));
                        Instantiate(p2_wall, move, Quaternion.identity);
                    }
                }
            }
        }
        /*
        else
        {
            dis_sum = 0;
        }
        */
        temp_x = mouse_position.x;
        temp_y = mouse_position.y;  // <--
        
        /* // yama 0316
        if (mouse_position.x < 0)
            Instantiate(p1_wall, mouse_position, Quaternion.identity);
            

        if (mouse_position.x > 0)
			Instantiate(p2_wall, mouse_position, Quaternion.identity);
            */
	}

    void DoBig(int size)
    {
        range = size;
    }

    void SetColors(Color[] received_colors)
    {
        colors = received_colors;
    }

    void GetScore () {
        scores = new int[2];
        scores[0] = 0;
        scores[1] = 0;
        Color[] canvas_colors = texture.GetPixels(0, 0, width, height);
        int counter = 10;
        for (int i = 0; i < width * height; ++i) {
            if (!canvas_colors[i].Equals(Color.clear))
            {
                if (counter > 0)
                {
                    Debug.Log(canvas_colors[i].r);
                    counter--;
                }
                if (canvas_colors[i].r <= 0.5) {
                    scores[0]++;
                } else { 
                    scores[1]++;
                }

            }
        }
    }

    void StartScoreShow()
    {
        foreach(GameObject the_ball in ball)
        {
            the_ball.SetActive(false);
        }
        GetScore();

        int sum = scores[0] + scores[1];
        float rate_1 = (float)scores[0] / sum;
        float rate_2 = (float)scores[1] / sum;

        P1DisplayTargetScaleX = maximumBar * rate_1;
        P2DisplayTargetScaleX = maximumBar * rate_2;

        animationPlaying = true;

    }
}
