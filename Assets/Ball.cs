using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	private SpriteRenderer default_ball;
	private SpriteRenderer p1_ball;
	private SpriteRenderer p2_ball;

	// Use this for initialization
	void Start () {
		default_ball = this.gameObject.GetComponent<SpriteRenderer>();
		p1_ball = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
		p2_ball = this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

		Player(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Player (int id) {
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
	}
}
