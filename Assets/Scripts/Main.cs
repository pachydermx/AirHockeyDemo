using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	public GameObject ball;
	private Rigidbody2D ball_rb;
    private Color[] colors;

	public GameObject canvas;

	// Use this for initialization
	void Start () {
		ball_rb = ball.GetComponent<Rigidbody2D>();

        // set framerate
        Application.targetFrameRate = 120;
	}
	
	// Update is called once per frame
	void Update () {
	
		// Debug Mode
		// the force
		int force = 100;
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			ball_rb.AddForce(new Vector2(0, force));
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			ball_rb.AddForce(new Vector2(0, -force));
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			ball_rb.AddForce(new Vector2(-force, 0));
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			ball_rb.AddForce(new Vector2(force, 0));
		}

		// ball sw
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			ball.SendMessage("Player", 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			ball.SendMessage("Player", 2);
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
	}

	void Player(int player_id){
		ball.SendMessage("Player", player_id);
	}

	void Accelerate(Vector2 direction){
		ball_rb.AddForce(direction * 10);
	}

    void SetColors(Color[] received_colors)
    {
        colors = received_colors;
        // tell canvas
        canvas.SendMessage("SetColors", colors);
    }
	
}
