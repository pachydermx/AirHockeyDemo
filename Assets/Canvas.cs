using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas : MonoBehaviour {
	public RawImage image;
	private Texture2D texture;

	// Use this for initialization
	void Start () {
		texture = image.texture as Texture2D;

		int width = 1024;
		int height = 768;

		Color[] colors = new Color[width*height];
		for (int i =0; i < width*height; ++i) 
			colors[i] = Color.clear;

		texture.SetPixels(0, 0, width, height, colors);
		texture.Apply(true);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DrawRect () {

		int width = 102;
		int height = 76;

		Color[] colors = new Color[width*height];
		for (int i =0; i < width*height; ++i) 
			colors[i] = Color.red;

		texture.SetPixels(0, 0, width, height, colors);
		texture.Apply(true);

	}
}
