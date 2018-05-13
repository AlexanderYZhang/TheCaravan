using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour {

	public int width;
	public int height;
	public int scale;

	public float offsetX;
	public float offsetY;

	void Start() {
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = GenerateTexture();
		offsetX = Random.Range(0, 9999f);
		offsetY = Random.Range(0, 9999f);
	}

	Texture2D GenerateTexture() {
		Texture2D texture = new Texture2D(width, height);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Color color = calculateColor(x,y);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
		return texture;
	}

	public Color calculateColor(int x, int y) {
		float coordX = ((float)x / width) * scale + offsetX;
		float coordY = ((float)y / height) * scale + offsetY;
		print(coordX);
		print(coordY);
		float sample = Mathf.PerlinNoise(coordX, coordY);
		return new Color(sample, sample, sample);
	}
}
