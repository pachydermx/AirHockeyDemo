using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	public GameObject[] ball;
	private Rigidbody2D[] ball_rb;
    private int n_ball;
    private Color[] colors;

    public GameObject start_scene;

	public GameObject canvas;
    public GameObject itembox;

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

	// Use this for initialization
	void Start () {
        /*
        // init variables
        int length = 8;
        ball = new GameObject[length];
        ball_rb = new Rigidbody2D[length];
        */

        // set framerate
        Application.targetFrameRate = 120;

        // active display
        Debug.Log("Display connected:" + Display.displays.Length);
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }

	}

    void ResetStage()
    {

        // init variables
        int length = 8;
        ball = new GameObject[length];
        ball_rb = new Rigidbody2D[length];
        n_ball = 0;

        // switch scene
        start_scene.SetActive(false);

        itembox.SetActive(true);
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

		// ball sw
		if (Input.GetKeyDown(KeyCode.Alpha1)){
            for(int i = 0; i < n_ball; i++)
                ball[i].SendMessage("Player", 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
            for(int i = 0; i < n_ball; i++)
                ball[i].SendMessage("Player", 2);
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

    void SetColors(Color[] received_colors)
    {
        colors = received_colors;
        // tell canvas
        canvas.SendMessage("SetColors", colors);
    }
	
    void AddNewBall(GameObject new_ball)
    {
        ball[n_ball] = new_ball;
        ball_rb[n_ball] = new_ball.GetComponent<Rigidbody2D>();

        // pick colors
        Color[] paint_color = PickColors();
        new_ball.GetComponent<Ball>().player_color = paint_color;
        
        if(n_ball > 0)
        {
        }
        
        n_ball++;
    }
}
