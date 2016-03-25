using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    private SpriteRenderer default_ball;
    private SpriteRenderer p1_ball;
    private SpriteRenderer p2_ball;

    public GameObject canvas;
    public GameObject manager;

    public int id;

    private Color[] player_color;

    // Use this for initialization
    void Start() {
        // get game objects
        canvas = GameObject.Find("Canvas");
        manager = GameObject.Find("Manager");

        default_ball = this.gameObject.GetComponent<SpriteRenderer>();
        p1_ball = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        p2_ball = this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        player_color = new Color[3];
        player_color[0] = Color.clear;
        player_color[1] = new Color(.453125F, .796875F, 1.0F, 0.5F);
        player_color[2] = new Color(1.0F, .5859375F, .89453125F, 0.5F);

        // tell manager color
        manager.SendMessage("SetColors", player_color);

        Player(0);
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

    void SetID(int new_id)
    {
        id = new_id;
    }

	// switch player
	void Player (int id) {
        //Debug.Log("Player switched to " + id);
		// switch apperance of ball
		default_ball.enabled = false;
		p1_ball.enabled = false;
		p2_ball.enabled = false;

		switch(id) {
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

		// switch paint color
		canvas.SendMessage("SetColor", player_color[id]);
	}
}
