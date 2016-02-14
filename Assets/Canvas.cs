﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {
	public RawImage image;
	private Texture2D texture;
	private Color paint_color;
	private int ball_x;
	private int ball_y;
	private int width = 1024;
	private int height = 768;

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
	}

	// Update is called once per frame
	void Update () {
		DrawRound(50);
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
			texture.SetPixel( x+x0, y+y0, paint_color );
			texture.SetPixel( y+x0, x+y0, paint_color );
			texture.SetPixel( -x+x0, y+y0, paint_color );
			texture.SetPixel( -y+x0, x+y0, paint_color );
			texture.SetPixel( -x+x0, -y+y0, paint_color );
			texture.SetPixel( -y+x0, -x+y0, paint_color );
			texture.SetPixel( x+x0, -y+y0, paint_color );
			texture.SetPixel( y+x0, -x+y0, paint_color );
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
}
