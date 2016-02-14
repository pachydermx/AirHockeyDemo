using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {
	public RawImage image;
	private Texture2D texture;

	int timer = 300;

	// Use this for initialization
	void Start () {
		texture = image.texture as Texture2D;

		int width = 1024;
		int height = 768;

		Color[] colors = new Color[width*height];
		for (int i =0; i < width*height; ++i) 
			colors[i] = Color.clear;

		texture.SetPixels(0, 0, width, height, colors);
		texture.Apply(false);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0){
			DrawRound(100);
			timer--;
		}
	}

	void DrawRect () {

		int width = 102;
		int height = 76;

		Color[] colors = new Color[width*height];
		for (int i =0; i < width*height; ++i) 
			colors[i] = Color.red;

		for (int i = 0; i < width; ++i)
			colors[GetIndex(width, height, i, 20)] = Color.blue;

		texture.SetPixels(0, 0, width, height, colors);
		texture.Apply(true);

	}

	void DrawRound (int radius) {
		int x0 = radius / 2;
		int y0 = radius / 2;
		int x = radius;
		int y = 0;
		int decisionOver2 = 1 - x;

		Color[] colors = new Color[radius*radius*8];
		/*
		for (int i =0; i < radius*radius*4; ++i) 
			colors[i] = Color.clear;
		*/

		Color paint_color = Color.red;

		while (y <= x){
			colors[GetIndex(radius, radius, x+x0, y+y0)] = paint_color;
			colors[GetIndex(radius, radius, y+x0, x+y0)] = paint_color;
			colors[GetIndex(radius, radius, -x+x0, y+y0)] = paint_color;
			colors[GetIndex(radius, radius, -y+x0, x+y0)] = paint_color;
			colors[GetIndex(radius, radius, -x+x0, -y+y0)] = paint_color;
			colors[GetIndex(radius, radius, -y+x0, -x+y0)] = paint_color;
			colors[GetIndex(radius, radius, x+x0, -y+y0)] = paint_color;
			colors[GetIndex(radius, radius, y+x0, -x+y0)] = paint_color;
			y++;
			if (decisionOver2 <= 0){
				decisionOver2 += 2 * y + 1;
			} else {
				x--;
				decisionOver2 += 2 * (y - x) + 1;
			}
		}

		texture.SetPixels(timer + 200, timer + 200, radius, radius, colors);
		texture.Apply(false);
	}

	int GetIndex (int width, int height, int x, int y){
		int index = width * y + x;
		if (index < width * height && index >= 0){
			return width * y + x;
		} else {
			Debug.Log(x + ", " + y);
			return 0;
		}
	}
}
