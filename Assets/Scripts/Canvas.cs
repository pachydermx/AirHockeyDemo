using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {
    public GameObject wall;
    public GameObject manager;

    public AudioClip split_sound;
    public float split_sound_volume = 1.0f;
    public AudioClip spray_sound;
    public float spray_sound_volume = 0.3f;
    public AudioClip explode_sound;
    public float explode_sound_volume = 1.0f;
    AudioSource bucket_sound;
    public AudioClip bucketclip;



    // textures
	public RawImage image;
    public RawImage normal;
	private Texture2D texture;
    private Texture2D normal_texture;

	private Color[] paint_color;
    public Color[] colors;
	private int[] ball_x;
	private int[] ball_y;
    private int n_ball;
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


    // yama 0325 分裂時の方向
    private float angle = 150;

    // settings
    public bool kick_off_when_start = true;
    private float ink_ratio = 0.8f;

    // debug
    public int counter = 10;

    // paint
    private Vector3[] last_paint_position;

	// Use this for initialization
	void Start () {
        // init variables
        last_paint_position = new Vector3[3];
		// init texture
		texture = image.texture as Texture2D;
        normal_texture = normal.texture as Texture2D;

        RefreshCanvas();
        ResetStage();

        // debug
        //Debug.Log(Vector3.Angle(new Vector3(1, 0, 0), new Vector3(-1, 1, 0)));
	}

	// Update is called once per frame
	void Update () {
        for (int i = 0; i < n_ball; i++)
        {
            if(paint_color[i].a > 0 && ball[i] != null)
            {
                DrawRound(i, range);
            }
        }
        // apply texture
        texture.Apply(false);
        normal_texture.Apply(false);
        //Debug.Log("range:" + range);
		//DrawRound(range);

        // tanaka 0324
		if (range >= 200)
        {
            //range -= 5;
            range = default_range;
		}else if(range > default_range)
        {
            itemcount++;
            if(itemcount >= 100)
            {
                DoBig(default_range);
                /*
                GameObject box = GameObject.Find("ItemBox1");
                ItemBoX s = box.GetComponent<ItemBoX>();
                //ball[0].transform.localScale = new Vector3(ball[0].transform.localScale.x / 1.5f, ball[0].transform.localScale.y / 1.5f, 1);
                ball[0].transform.localScale = new Vector3(s.p_scale.x , s.p_scale.y, 1);
                */
                itemcount = 0;
            }
        }
        

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

        if (Input.GetKeyDown(KeyCode.F6))
        {
            ResetStage();
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

    /*
    void AddNewBall()
    {   
        // instantiate
        ball[n_ball] = (GameObject)GameObject.Instantiate(ref_ball, new Vector3(0, 0, -1), Quaternion.identity);
        // config
        ball[n_ball].SendMessage("SetID", n_ball, SendMessageOptions.RequireReceiver);
        paint_color[n_ball] = Color.clear;
        ball[n_ball].gameObject.name = "Ball_" + n_ball;
        manager.SendMessage("AddNewBall", ball[n_ball]);

        if (n_ball > 0)
        {
            ball[n_ball].gameObject.GetComponent<ColliderPack>().enabled = true;

        }
        
        if (n_ball == 0) // yama 0325 初期パックの設定
        {
            ball[n_ball] = (GameObject)GameObject.Instantiate(ref_ball, new Vector3(0, 0, -1), Quaternion.identity);
            GameObject box = GameObject.Find("ItemBox");
            box.SendMessage("setBallOriginal", ball[n_ball]);

            ball[n_ball].gameObject.GetComponent<ColliderPack>().enabled = true; // yama 0325 爆発使用
        }
        else // yama 0325 複製パックの設定
        {
            ball[n_ball] = (GameObject)GameObject.Instantiate(ref_ball, new Vector3(ball[0].transform.position.x, ball[0].transform.position.y, -1), Quaternion.identity);

            ball[n_ball].gameObject.GetComponent<ColliderPack>().enabled = true; // yama 0325 爆発使用
            ball[n_ball].gameObject.GetComponent<CloneDelete>().enabled = true; // yama 0325 複製削除

            float s_x = ball[0].GetComponent<Rigidbody2D>().velocity.x;
            float s_y = ball[0].GetComponent<Rigidbody2D>().velocity.y;

            ball[0].GetComponent<Rigidbody2D>().velocity = new Vector2(s_x * Mathf.Cos(angle) + s_y * Mathf.Sin(angle), s_x * (-Mathf.Sin(angle)) + s_y * Mathf.Cos(angle));
            ball[n_ball].GetComponent<Rigidbody2D>().velocity = new Vector2(s_x * Mathf.Cos(-angle) + s_y * Mathf.Sin(-angle), s_x * (-Mathf.Sin(-angle)) + s_y * Mathf.Cos(-angle));

            // yama 0325 パックの色付け（できてません）
            //Debug.Log("p_id:"+ ball[0].GetComponent<Ball>().paint_id);
            //int id = ball[0].GetComponent<Ball>().paint_id;  
            //ball[n_ball].SendMessage("Player", id);
            //ball[n_ball].SendMessage("Player", 1);
            //paint_color[n_ball] = paint_color[0];
        }


        // kick off
        if (kick_off_when_start)
        {
            float power = 150;
            float direction = Random.value * 2 * Mathf.PI;
            Vector2 kick_off = new Vector2(power * Mathf.Cos(direction), power * Mathf.Sin(direction));
            ball[n_ball].GetComponent<Rigidbody2D>().AddForce(kick_off);
        }

        n_ball++;
    }
    */

    void AddNewBall()
    {
        if (n_ball == 0) // yama 0325 初期パックの設定
        {
            ball[n_ball] = (GameObject)GameObject.Instantiate(ref_ball, new Vector3(0, 0, -1), Quaternion.identity);
            GameObject box = GameObject.Find("ItemBox1");
            box.SendMessage("setBallOriginal", ball[n_ball]);

            ball[n_ball].gameObject.GetComponent<ColliderPack>().enabled = true; // yama 0325 爆発使用
        }
        else // yama 0325 複製パックの設定
        {
            ball[n_ball] = (GameObject)GameObject.Instantiate(ref_ball, new Vector3(ball[0].transform.position.x, ball[0].transform.position.y, -1), Quaternion.identity);

            ball[n_ball].gameObject.GetComponent<ColliderPack>().enabled = true; // yama 0325 爆発使用
            ball[n_ball].gameObject.GetComponent<CloneDelete>().enabled = true; // yama 0325 複製削除

            float s_x = ball[0].GetComponent<Rigidbody2D>().velocity.x;
            float s_y = ball[0].GetComponent<Rigidbody2D>().velocity.y;

            ball[0].GetComponent<Rigidbody2D>().velocity = new Vector2(s_x * Mathf.Cos(angle) + s_y * Mathf.Sin(angle), s_x * (-Mathf.Sin(angle)) + s_y * Mathf.Cos(angle));
            ball[n_ball].GetComponent<Rigidbody2D>().velocity = new Vector2(s_x * Mathf.Cos(-angle) + s_y * Mathf.Sin(-angle), s_x * (-Mathf.Sin(-angle)) + s_y * Mathf.Cos(-angle));

            // yama 0325 パックの色付け（できてません）
            Debug.Log("p_id:"+ ball[0].GetComponent<Ball>().paint_id);
            int id = ball[0].GetComponent<Ball>().paint_id;  
            //ball[n_ball].SendMessage("Player", id);
            //paint_color[n_ball] = paint_color[0];

            ball[n_ball].GetComponent<Ball>().default_pid = id;
        }

        ball[n_ball].SendMessage("SetID", n_ball, SendMessageOptions.RequireReceiver);
        //paint_color[n_ball] = Color.clear;
        ball[n_ball].gameObject.name = "Ball_" + n_ball;
        manager.SendMessage("AddNewBall", ball[n_ball]);

        // kick off
        if (kick_off_when_start)
        {
            KickOff();
        }

        n_ball++;

        // sound effect
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(split_sound, split_sound_volume);
    }

    void KickOff()
    {
        float power = 150;
        float direction = Random.value * 2 * Mathf.PI;
        Vector2 kick_off = new Vector2(power * Mathf.Cos(direction), power * Mathf.Sin(direction));
        ball[n_ball - 1].GetComponent<Rigidbody2D>().AddForce(kick_off);
    }


    void SetColor (float[] rgbanid){
		paint_color[(int)rgbanid[4]] = new Color(rgbanid[0], rgbanid[1], rgbanid[2], rgbanid[3]);
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
        // prevent glitch
        bool not_moving = false;
        if (pos_x == last_ball_x[id] && pos_y == last_ball_y[id])
        {
            not_moving = true;
        }

        // paint
        for (int x = -radius; x < radius; x++)
        {
            int height = (int)Mathf.Sqrt(radius * radius - x * x);
            int inside_height = -1;
            if (Mathf.Abs(x)/(float)radius < ink_ratio)
            {
                inside_height = (int) Mathf.Sqrt(radius*radius*ink_ratio*ink_ratio - x*x);
            }

            for (int y = -height; y < height; y++)
            {
                Color origColor = texture.GetPixel(pos_x + x, pos_y + y);
                if (!CompareColors(origColor, paint_color[id]))
                {
                    if (Mathf.Abs(y) < inside_height)
                    {
                        // color
                        texture.SetPixel(pos_x + x, pos_y + y, paint_color[id]);
                    }
                    else
                    {
                        texture.SetPixel(pos_x + x, pos_y + y, new Color(paint_color[id].r, paint_color[id].g, paint_color[id].b, paint_color[id].a * 0.5f));
                    }
                } else if (Mathf.Abs(y) < inside_height)
                {
                        texture.SetPixel(pos_x + x, pos_y + y, paint_color[id]);
                    
                }
                // random dots
                /*
                if (Random.value < 0.01)
                {
                    texture.SetPixel( pos_x + x, pos_y + y, new Color(1.0f, 1.0f, 1.0f, 0.5f));
                }
                */
                // normal map
                /*
                if (!not_moving && !splash)
                {
                    normal_texture.SetPixel(pos_x + x, pos_y + y, GetNormalColor(id, pos_x, pos_y, pos_x + x, pos_y + y, radius));
                } else if (splash)
                {
                    normal_texture.SetPixel( pos_x + x, pos_y + y,  GetNormalColorCentral(pos_x, pos_y, radius, pos_x + x, pos_y + y) );
                }
                */
            }
        }

        // record ball posisition
        if (!splash)
        {
            last_ball_x[id] = ball_x[id];
            last_ball_y[id] = ball_y[id];
        }
    }

    // legacy algorithm
    void DrawRoundAt2(int id, int pos_x, int pos_y, int radius, bool splash)
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
				texture.SetPixel( i, y+y0, paint_color[id] );
				texture.SetPixel( i, -y+y0, paint_color[id] );
                if (!not_moving && !splash)
                {
                    normal_texture.SetPixel( i, y+y0,  GetNormalColor(id, x0, y0, i, y + y0, radius));
                    normal_texture.SetPixel( i, -y+y0,  GetNormalColor(id, x0, y0, i, -y + y0, radius));
                } else if (splash)
                {
                    normal_texture.SetPixel( i, y+y0,  GetNormalColorCentral(x0, y0, radius, i, y+y0));
                    normal_texture.SetPixel( i, -y+y0,  GetNormalColorCentral(x0, y0, radius, i, -y+y0));
                }
			}
            
			for (i = -y+x0; i < y+x0; ++i) {
				texture.SetPixel( i, x+y0, paint_color[id] );
				texture.SetPixel( i, -x+y0, paint_color[id] );
                if (!not_moving && !splash)
                {
                    normal_texture.SetPixel(i, x + y0, GetNormalColor(id, x0, y0, i, x + y0, radius));
                    normal_texture.SetPixel(i, -x + y0, GetNormalColor(id, x0, y0, i, -x + y0, radius));
                } else if (splash)
                {
                    normal_texture.SetPixel( i, x+y0,  GetNormalColorCentral(x0, y0, radius, i, x+y0));
                    normal_texture.SetPixel( i, -x+y0,  GetNormalColorCentral(x0, y0, radius, i, -x+y0));
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

    }

	void DrawRound (int id, int radius) {
        int radius_noised = (int)(radius - 15 + 30 * Random.value);
        DrawRoundAt(id, ball_x[id], ball_y[id], radius_noised, false);
        if (Random.value < 0.2)
        {
            int splash_rad = 80;
            int ink_x = (int)(ball_x[id] + Random.value * splash_rad * 2) - splash_rad;
            int ink_y = (int)(ball_y[id] + Random.value * splash_rad * 2) - splash_rad;
            int rad = (int)(Random.value * 30);
            DrawRoundAt(id, ink_x, ink_y, rad, true);
        }
	}

    Color GetNormalColor(int id, int ball_x, int ball_y, int pos_x, int pos_y, int radius)
    {
        float grayscale;
        float percentage;
        // caculate normal color
        float distance = Mathf.Abs((last_ball_y[id] - ball_y) * pos_x - (last_ball_x[id] - ball_x) * pos_y + last_ball_x[id] * ball_y - last_ball_y[id] * ball_x) / Mathf.Sqrt((last_ball_y[id] - ball_y) * (last_ball_y[id] - ball_y) + (last_ball_x[id] - ball_x) * (last_ball_x[id] - ball_x));
        percentage = (float)(distance / radius);
        grayscale = 1.0f - percentage * 0.4f;
        if (Random.value > 0.7f)
        { 
            // add noise
            percentage = 0.5f + 0.5f * Random.value;
            grayscale += Random.value - 0.5f;
        }
        Color result = new Color(grayscale, grayscale, grayscale, 1 - percentage);
        return result;
    }

    Color GetNormalColorCentral(int x0, int y0, int radius, int pos_x, int pos_y)
    {
        float distance = Mathf.Sqrt((pos_x - x0) * (pos_x - x0) + (pos_y - y0) * (pos_y - y0));
        float percentage = (float)(distance / radius);
        float grayscale = 1.0f - percentage * 0.3f;
        if (Random.value > 0.7f)
        { 
            // add noise
            percentage = 0.5f + 0.5f * Random.value;
            grayscale += Random.value - 0.5f;
        }
        Color original = normal_texture.GetPixel(x0, y0);
        if (original.a > 0.1)
        {
            float original_grayscale = original.r;
            grayscale = (grayscale + original_grayscale) / 2;
        }
        Color result = new Color(grayscale, grayscale, grayscale, percentage);
        return result;
    }

    void DoSprinkle(Vector3 position) // yama 0317 Baketsu Gimmick
    {
        bucket_sound = GetComponent<AudioSource>();
        bucket_sound.clip = bucketclip;
        bucket_sound.PlayOneShot(bucketclip);

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

        
        DrawRoundAt(0, x0, y0, 200, true);
    }

    public void DoSpray(Vector3 position, int id) // yama 0318 Spray Gimmick
    {
        // paint_color id not working for second ball
        float[] xy0 = { position.x, position.y };
        float rate_x = (float)(xy0[0] / 9.6);
        float rate_y = (float)(xy0[1] / 5.4);

        int x = (int)(rate_x * width / 2 + (width / 2));
        //int y = (int)(rate_y * height / 2 + (height / 2)) + 50;

        if (id == 1)
        {
            int y = (int)(rate_y * height / 2 + (height / 2)) + 50;
            for (int i = 1; i < 100; i++)
            {
                for (int j = 0; j < (i * 2 - 1); j++)
                {
                    texture.SetPixel(x + j - (i * 2 - 1) / 2, y + i * 3, paint_color[0]);
                    texture.SetPixel(x + j - (i * 2 - 1) / 2, y + 1 + i * 3, paint_color[0]);
                    texture.SetPixel(x + j - (i * 2 - 1) / 2, y + 2 + i * 3, paint_color[0]);
                }
            }
        }
        else if(id == 2)
        {
            int y = (int)(rate_y * height / 2 + (height / 2)) - 50;
            for (int i = 1; i < 100; i++)
            {
                for (int j = 0; j < (i * 2 - 1); j++)
                {
                    texture.SetPixel(x + j - (i * 2 - 1) / 2, y - (i * 3), paint_color[0]);
                    texture.SetPixel(x + j - (i * 2 - 1) / 2, y - (1 + i * 3), paint_color[0]);
                    texture.SetPixel(x + j - (i * 2 - 1) / 2, y - (2 + i * 3), paint_color[0]);
                }
            }
        }

        //Debug.Log("Color");
        // sound effect
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(spray_sound, spray_sound_volume);
    }

	void DoExplode () {
		range = 200;
        // sound effect
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(explode_sound, explode_sound_volume);
	}

    void DeployWall (Vector2 mouse_position) {
        int id;
        if (mouse_position.x < 0)
        {
            id = 1;
        } else
        {
            id = 2;
        }
        // calc
        Vector3 current_position = new Vector3(mouse_position.x, mouse_position.y, -1);
        Vector3 size;
        Quaternion point_to;
        Vector3 delta = last_paint_position[id] - current_position;
        //Debug.Log(delta);
        if (delta.magnitude < 1)
        {
            // orthant iii, iv
            if (delta.y > 0) {
                point_to = Quaternion.Euler(new Vector3(0, 0, Vector3.Angle(delta.normalized, new Vector3(1, 0, 0))));
            } else if (delta.x < 0)
            // orthant i
            {
                point_to = Quaternion.Euler(new Vector3(0, 0, Vector3.Angle(delta, new Vector3(0, 1, 0)) + 90));
            } else
            // orthant ii
            {
                point_to = Quaternion.Euler(new Vector3(0, 0, 90 - Vector3.Angle(delta, new Vector3(0, 1, 0)) + 0));
            }
            size = new Vector3(delta.magnitude * 10, 1, 1);
        } else
        {
            size = new Vector3(1, 1, 1);
            point_to = Quaternion.identity;
        }
        // deploy
        GameObject nw = (GameObject)Instantiate(wall, current_position, point_to);
        nw.transform.localScale = size;
        GameObject nwp = nw.transform.GetChild(0).gameObject;
        nwp.GetComponent<SpriteRenderer>().color = colors[id];
        nw.GetComponent<ColliderMessenager>().player_id = id;
        // log
        last_paint_position[id] = current_position;
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
        Color[] paint_color = manager.GetComponent<Main>().colors;
        for (int i = 0; i < width * height; ++i) {
            if (!canvas_colors[i].Equals(Color.clear))
            {
                if (CompareColors(canvas_colors[i], paint_color[1])) {
                    scores[0]++;
                } else { 
                    scores[1]++;
                }

            }
        }
        Debug.Log("Score " + scores[0] + " : " + scores[1]);
    }

    bool CompareColors(Color color1, Color color2)
    {
        float delta_r = Mathf.Abs(color1.r - color2.r);
        float delta_g = Mathf.Abs(color1.g - color2.g);
        float delta_b = Mathf.Abs(color1.b - color2.b);
        float delta = delta_r + delta_g + delta_b;
        if (delta < 0.1)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void StartScoreShow()
    {
        GetScore();

        ClearStage(false);

        int sum = scores[0] + scores[1];
        float rate_1 = (float)scores[0] / sum;
        float rate_2 = (float)scores[1] / sum;

        P1DisplayTargetScaleX = maximumBar * rate_1;
        P2DisplayTargetScaleX = maximumBar * rate_2;

        animationPlaying = true;

    }

    void RefreshCanvas()
    {
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

    }

    void ClearStage(bool will_set_timer)
    {
        // init variables
        int length = 8;
        ball_x = new int[length];
        ball_y = new int[length];
        last_ball_x = new int[length];
        last_ball_y = new int[length];
        paint_color = new Color[length];
        last_ball_x[0] = 0;
        last_ball_y[0] = 0;
        // clean canvas
        RefreshCanvas();

		// init properties
		ball_x[0] = width / 2;
		ball_y[0] = height / 2;
		range = default_range;

        // clean balls
        for (int i = 0; i < n_ball; i++)
        {
            Destroy(ball[i]);
        }
        manager.SendMessage("ResetStage", will_set_timer);
    }

    void ResetStage()
    {
        ClearStage(true);
        // reset score animation
        P1Display.transform.localScale = new Vector3(1, 110, 1);
        P2Display.transform.localScale = new Vector3(1, 110, 1);
        animateCounter = 0;
        // start game
        int length = 8;
        ball = new GameObject[length];
        n_ball = 0;
        AddNewBall();
    }
}
