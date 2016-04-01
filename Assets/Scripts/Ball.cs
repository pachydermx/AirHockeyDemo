using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    public SpriteRenderer default_ball;
    public SpriteRenderer p1_ball;
    public SpriteRenderer p2_ball;

    public GameObject canvas;
    public GameObject manager;

    public int id;

    public Color[] player_color;

    public int paint_id;

    public int default_pid = 0;

    SpriteRenderer MainSpriteRenderer;
    public Sprite BombPack;

    // Use this for initialization
    void Start() {
        //MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>(); //0328 tanaka icon

        // get game objects
        canvas = GameObject.Find("Canvas");
        manager = GameObject.Find("Manager");

        default_ball = this.gameObject.GetComponent<SpriteRenderer>();
        p1_ball = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        p2_ball = this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        Player(default_pid);
    }

    // Update is called once per frame
    void Update() {
        // set coordinate
        //Debug.Log(transform.position);
        float[] xy = new float[3];
        xy[0] = (transform.position.x);
        xy[1] = (transform.position.y);
        xy[2] = (float)id;
        canvas.SendMessage("SetCoordinate", xy);
    }

    void changePack()
    {
        MainSpriteRenderer.sprite = BombPack;
    }

    void SetID(int new_id)
    {
        id = new_id;
    }

	// switch player
	void Player (int pid) {
        //Debug.Log("Player switched to " + id);
		// switch apperance of ball
		default_ball.enabled = false;
		p1_ball.enabled = false;
		p2_ball.enabled = false;

		switch(pid) {
		case 1:
			p1_ball.enabled = true;
			break;
		case 2:
			p2_ball.enabled = true;
			break;
		default:
			default_ball.enabled = true;
			break;
		}


            paint_id = pid;

		// switch paint color
		canvas.SendMessage("SetColor", new float[] { player_color[pid].r, player_color[pid].g, player_color[pid].b, player_color[pid].a, id});
	}

}
