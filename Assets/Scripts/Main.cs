using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	public GameObject[] ball;
	private Rigidbody2D[] ball_rb;
    private int n_ball;
    private Color[] colors;

	public GameObject canvas;

    // for smasher debugging
    public GameObject smasher;
    private bool smasher_controlled_by_mouse = false;

	// Use this for initialization
	void Start () {

        // set framerate
        Application.targetFrameRate = 120;

        // active display
        Debug.Log("Display connected:" + Display.displays.Length);
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }

        ResetStage();
	}

    void ResetStage()
    {
        // init variables
        int length = 8;
        ball = new GameObject[length];
        ball_rb = new Rigidbody2D[length];
        n_ball = 0;

    }
	
	// Update is called once per frame
	void Update () {
	
		// Debug Mode
		// the force
		int force = 100;
		if (Input.GetKeyDown(KeyCode.UpArrow)){
            for(int i = 0; i < n_ball; i++)
                ball_rb[i].AddForce(new Vector2(0, force));
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
            for(int i = 0; i < n_ball; i++)
                ball_rb[i].AddForce(new Vector2(0, -force));
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
            for(int i = 0; i < n_ball; i++)
                ball_rb[i].AddForce(new Vector2(-force, 0));
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
            for(int i = 0; i < n_ball; i++)
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
        
        if(n_ball > 0)
        {
        }
        
        n_ball++;
    }
}
