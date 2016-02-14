using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	public GameObject ball;
	private Rigidbody2D ball_rb;

	public GameObject canvas;

	// Use this for initialization
	void Start () {
		ball_rb = ball.GetComponent<Rigidbody2D>();
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
			canvas.SendMessage("DrawRect");
		}
	}
	
}
