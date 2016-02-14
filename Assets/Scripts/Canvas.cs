using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {
	public GameObject p1_wall;
	public GameObject p2_wall;

	public RawImage image;
	private Texture2D texture;
	private Color paint_color;
	private int ball_x;
	private int ball_y;
	private int width = 1024;
	private int height = 768;

	private int default_range = 50;
	private int range;

	// Use this for initialization
	void Start () {
		// init texture
		texture = image.texture as Texture2D;

		Color[] colors = new Color[width*height];
		for (int i =0; i < width*height; ++i) 
			colors[i] = Color.clear;

		texture.SetPixels(0, 0, width, height, colors);

		// init properties
		paint_color = Color.clear;
		ball_x = width / 2;
		ball_y = height / 2;
		range = default_range;
	}

	// Update is called once per frame
	void Update () {
		DrawRound(range);

		if (range > default_range) {
			range -= 10;
		}

		// receive touch
		if (Input.touchCount > 0){
			Debug.Log(Input.GetTouch(0).position.x + ", "+ Input.GetTouch(0).position.y);
		}

		if (Input.GetMouseButton(0)){
			// deploy wall
			DeployWall(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	void SetColor (Color new_color){
		paint_color = new_color;
	}

	void SetCoordinate(float[] xy) {
		ball_x = (int)((xy[0]/10.5) * width + width / 2);
		ball_y = (int)((xy[1]/7.8) * height + height / 2);
		//Debug.Log(ball_x + ", " + ball_y);
	}

	void DrawRound (int radius) {
		int x0 = ball_x;
		int y0 = ball_y;
		int x = radius;
		int y = 0;
		int decisionOver2 = 1 - x;

		while (y <= x){
			int i;
			for (i = -x+x0; i < x+x0; ++i) {
				texture.SetPixel( i, y+y0, paint_color );
				texture.SetPixel( i, -y+y0, paint_color );
			}

			for (i = -y+x0; i < y+x0; ++i) {
				texture.SetPixel( i, x+y0, paint_color );
				texture.SetPixel( i, -x+y0, paint_color );
			}

			y++;
			if (decisionOver2 <= 0){
				decisionOver2 += 2 * y + 1;
			} else {
				x--;
				decisionOver2 += 2 * (y - x) + 1;
			}
		}

		texture.Apply(false);
	}

	void DoExplode () {
		range = 100;
	}

	void DeployWall (Vector2 mouse_position) {
		if (mouse_position.x < 0)
			Instantiate(p1_wall, mouse_position, Quaternion.identity);

		if (mouse_position.x > 0)
			Instantiate(p2_wall, mouse_position, Quaternion.identity);
	}
}
