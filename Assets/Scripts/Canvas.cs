using UnityEngine;
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
    // for normal map
    private Color[] ncolors;
	private int ball_x;
	private int ball_y;
	private int width = 1920;
	private int height = 1080;

    private Color normal_color;
    private Color normal_color2;

	private int default_range = 50;
	private int range;

    protected int[] scores;
    public GameObject P1Display;
    public GameObject P2Display;
    public GameObject ball;
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
		// init texture
		texture = image.texture as Texture2D;
        normal_texture = normal.texture as Texture2D;

        normal_color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        normal_color2 = new Color(0.0f, 0.0f, 0.0f, 1.0f);


        Color default_normal = normal_color2;

		colors = new Color[width*height];
		ncolors = new Color[width*height];

		for (int i =0; i < width*height; ++i)
        {
			colors[i] = Color.red;
            ncolors[i] = default_normal;
        }


		texture.SetPixels(0, 0, width, height, colors);
        normal_texture.SetPixels(0, 0, width, height, ncolors);


        normal_texture.SetPixels(0, 0, width, height, ncolors);

		// init properties
		paint_color = Color.clear;
		ball_x = width / 2;
		ball_y = height / 2;
		range = default_range;

        normal_texture.Apply(true);
	}

	// Update is called once per frame
	void Update () {
		DrawRound(range);

		if (range > default_range) {
			range -= 5;
		}

		// receive touch
		if (Input.touchCount > 0){
			Debug.Log(Input.GetTouch(0).position.x + ", "+ Input.GetTouch(0).position.y);
		}

		if (Input.GetMouseButton(0)){
            // deploy wall
            DeployWall(Camera.main.ScreenToWorldPoint(Input.mousePosition));

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
    
	void SetColor (Color new_color){
		paint_color = new_color;
	}

	void SetCoordinate(float[] xy) {
        /*
		ball_x = (int)((xy[0]/10.5) * width + width / 2);
		ball_y = (int)((xy[1]/7.8) * height + height / 2);
        */
        float rate_x = (float)(xy[0] / 9.6);
        float rate_y = (float)(xy[1] / 5.4);

        ball_x = (int)(rate_x * width / 2 + (width / 2));
        ball_y = (int)(rate_y * height / 2 + (height / 2));

		//Debug.Log(ball_x + ", " + ball_y);
	}

    // this is an implementation of ecalipse painting
	void DrawRound (int radius) {
		int x0 = ball_x;
		int y0 = ball_y;
		int x = radius;
		int y = 0;
		int decisionOver2 = 1 - x;

		while (y <= x){
			int i;
            
			for (i = -x+x0; i < x+x0; ++i) {
                //colors[(width * ( y + y0 )) + i] = paint_color;
                //colors[(width * ( -y + y0 )) + i] = paint_color;
				texture.SetPixel( i, y+y0, paint_color );
				texture.SetPixel( i, -y+y0, paint_color );
				normal_texture.SetPixel( i, y+y0, normal_color );
				normal_texture.SetPixel( i, -y+y0, normal_color );
			}
            
			for (i = -y+x0; i < y+x0; ++i) {
                //colors[(width * ( x + y0 )) + i] = paint_color;
                //colors[(width * ( -x + y0 )) + i] = paint_color;
				texture.SetPixel( i, x+y0, paint_color );
				texture.SetPixel( i, -x+y0, paint_color );
				normal_texture.SetPixel( i, x+y0, normal_color2 );
				normal_texture.SetPixel( i, -x+y0, normal_color2 );
			}

			y++;
			if (decisionOver2 <= 0){
				decisionOver2 += 2 * y + 1;
			} else {
				x--;
				decisionOver2 += 2 * (y - x) + 1;
			}
		}

        //texture.SetPixels(0, 0, width, height, colors);

		texture.Apply(false);
        normal_texture.Apply(false);
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
		range = 200;
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
        ball.SetActive(false);
        GetScore();

        int sum = scores[0] + scores[1];
        float rate_1 = (float)scores[0] / sum;
        float rate_2 = (float)scores[1] / sum;

        P1DisplayTargetScaleX = maximumBar * rate_1;
        P2DisplayTargetScaleX = maximumBar * rate_2;

        animationPlaying = true;

    }
}
