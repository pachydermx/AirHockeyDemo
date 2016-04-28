using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	public GameObject[] ball;
	private Rigidbody2D[] ball_rb;
    private int n_ball;
    public Color[] colors;

    public GameObject start_scene;

	public GameObject canvas;
    public GameObject itembox1;
    public GameObject itembox2;

    //tanaka 0426
    public GameObject bucket1;
    public GameObject bucket2;
    public GameObject spray1;
    public GameObject spray2;

    // timer
    public float stage_duration = 30;
    public float set_duration = 10;
    public float remaining_time;

    // for smasher debugging
    public GameObject smasher;
    private bool smasher_controlled_by_mouse = false;

    public Color[] available_colors = {
        new Color(1.0f, 0.176470588235f, 0.176470588235f, 0.5f),
        new Color(1.0f, 0.823529411765f, 0.0f, 0.5f),
        new Color(0.0941176470588f, 1.0f, 0.0f, 0.5f),
        new Color(0.211764705882f, 0.0f, 1.0f, 0.5f),
        new Color(0.988235294118f, 0.0f, 1.0f, 0.5f),
        new Color(.453125F, .796875F, 1.0F, 0.5F),
        new Color(1.0F, .5859375F, .89453125F, 0.5F)
    };

    // timer
    private Timer timer;

    private GameObject npa1;
    private GameObject npa2;

    private Animation npanim1;
    private Animation npanim2;

    //public GameObject cover;

	// Use this for initialization
	void Start () {
        /*
        // init variables
        int length = 8;
        ball = new GameObject[length];
        ball_rb = new Rigidbody2D[length];
        */
        // get objects
        timer = GameObject.Find("Timer").GetComponent<Timer>();

        //tanaka 0418
	    npa1 = GameObject.Find("SketchBook1");
	    npa2 = GameObject.Find("SketchBook2");

        // set framerate
        Application.targetFrameRate = 120;

        // active display
        Debug.Log("Display connected:" + Display.displays.Length);
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }

        //tanaka 0426
        //ball[0].SetActive(false);
	}

    void ResetStage(bool new_stage)
    {
        // init variables
        int length = 8;
        ball = new GameObject[length];
        ball_rb = new Rigidbody2D[length];
        n_ball = 0;

        // switch scene
        start_scene.SetActive(false);

        itembox1.SetActive(true);

        itembox1.SendMessage("reset");


        // reset timer
        if (new_stage)
        {
            // reset timer
            remaining_time = set_duration + stage_duration;
            CancelInvoke("TimeDecrease");
            InvokeRepeating("TimeDecrease", 0.0f, 1.0f);
            // pick colors
            colors = PickColors();
            SetColors();
            // set score display
            GameObject.Find("P1Display").transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = colors[1];
            GameObject.Find("P2Display").transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = colors[2];
        }

        // reset cover
        if (new_stage)
        {
            //cover.SetActive(true);
        }
    }

    void TimeDecrease()
    {
        // count down
        if (remaining_time <= 5)
        {
            timer.ShowText(remaining_time.ToString("#."), true, remaining_time);
        } else if (Mathf.Abs(remaining_time - stage_duration) < 1)
        // game start
        {
            timer.ShowText("START", true, remaining_time);
            //cover.SetActive(false);
            canvas.SendMessage("KickOff");

            //tanaka 0426
            //ball[0].SetActive(true);

            //tanaka 0418
            npa1.SetActive(false); 
            npa2.SetActive(false);
        } else if (remaining_time == stage_duration + set_duration) { 
        // set
            timer.ShowText("SET", true, remaining_time);

            //tanaka 0420
            npa1.SetActive(true);
            npa2.SetActive(true);

            npa1.SendMessage("ResetNewPage");
            npa2.SendMessage("ResetNewPage");

            //tanaka 0426
            itembox1.SetActive(true);
            itembox2.SetActive(true);
            bucket1.SetActive(true);
            bucket2.SetActive(true);
            spray1.SetActive(true);
            spray2.SetActive(true);
        }
        else if (remaining_time == stage_duration + 3)
        {
            npa1.SendMessage("NewPage");
            npa2.SendMessage("NewPage");
        }
        else
        // normal
        {
            timer.ShowText("", false, remaining_time);
        }
        remaining_time -= 1.0f;
        if (remaining_time < 0)
        {
            CancelInvoke("TimeDecrease");
            canvas.SendMessage("StartScoreShow");

            //tanaka 0426
            itembox1.SetActive(false);
            itembox2.SetActive(false);
            bucket1.SetActive(false);
            bucket2.SetActive(false);
            spray1.SetActive(false);
            spray2.SetActive(false);
        }
    }

    Color[] PickColors()
    {
        Color[] result = new Color[3];
        int id1 = Random.Range(0, available_colors.Length);
        int id2 = Random.Range(0, available_colors.Length);
        while (id1.Equals(id2))
        {
            id2 = Random.Range(0, available_colors.Length);
        }
        result[0] = Color.clear;
        result[1] = available_colors[id1];
        result[2] = available_colors[id2];

        return result;
    }
	
	// Update is called once per frame
	void Update () {
	
		// Debug Mode
		// the force
		int force = 100;
		if (Input.GetKeyDown(KeyCode.UpArrow)){
            for(int i = 0; i < n_ball; i++)
                if(ball_rb[i] != null)
                    ball_rb[i].AddForce(new Vector2(0, force));
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
            for(int i = 0; i < n_ball; i++)
                if(ball_rb[i] != null)
                    ball_rb[i].AddForce(new Vector2(0, -force));
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
            for(int i = 0; i < n_ball; i++)
                if(ball_rb[i] != null)
                    ball_rb[i].AddForce(new Vector2(-force, 0));
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
            for(int i = 0; i < n_ball; i++)
                if(ball_rb[i] != null)
                    ball_rb[i].AddForce(new Vector2(force, 0));
		}

	    if (Input.GetKeyDown(KeyCode.Alpha0))
	    {
            for(int i = 0; i < n_ball; i++)
                if(ball_rb[i] != null)
                    ball_rb[i].velocity = new Vector2(0, 0);
	    }

		// ball sw
		if (Input.GetKeyDown(KeyCode.Alpha1)){
            for(int i = 0; i < n_ball; i++)
                if (ball[i] != null)
                {
                    ball[i].SendMessage("Player", 1);
                }
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
            for(int i = 0; i < n_ball; i++)
                if (ball[i] != null)
                {
                    ball[i].SendMessage("Player", 2);
                }
		}

		// test draw
		if (Input.GetKeyDown(KeyCode.T)){
			canvas.SendMessage("DrawRound", 100);
			canvas.SendMessage("DrawRound", 120);
		}

        // get score
        if (Input.GetKeyDown(KeyCode.P))
        {
            canvas.SendMessage("GetScore");
        }

		// explode
		if (Input.GetKeyDown(KeyCode.Space)) {
			canvas.SendMessage("DoExplode");
		}

        // smasher debug
        if (Input.GetKeyDown(KeyCode.S))
        {
            smasher.SetActive(true);
            smasher_controlled_by_mouse = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ball[0].SendMessage("Set_Flag", 1);
            ball[0].SendMessage("TouchCheck", 1);
        }

        if (smasher_controlled_by_mouse)
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse_position.z = 0;
            smasher.SendMessage("Move", mouse_position);
        }
    }

    /*
	void Player(int player_id){
		ball.SendMessage("Player", player_id);
	}

	void Accelerate(Vector2 direction){
		ball_rb.AddForce(direction * 10);
	}
    */

    void SetColors()
    {
        canvas.SendMessage("SetColors", colors);
    }
	
    void AddNewBall(GameObject new_ball)
    {
        ball[n_ball] = new_ball;
        ball_rb[n_ball] = new_ball.GetComponent<Rigidbody2D>();

        new_ball.GetComponent<Ball>().player_color = colors;
        
        n_ball++;
    }
}
