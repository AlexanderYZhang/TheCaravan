using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
	public GameObject player;
	public GameObject[] Tile;
	public GameObject[] Trees;
	public GameObject[] protrusions;
	public GameObject[] Rocks;
	
	public int width;
	public int height;
	public float samplingScale;
	private float offsetX;
	private float offsetY;
	private Transform tileHolder;
	private Transform treeHolder;
	private Transform protrusionHolder;
	private Vector3 position;
	private Vector3 scale;
	private int[,] board;
	private int[,] treeBoard;

	private enum TerrainItem {
		Nothing = 0,
		Tree = 1,
		Protrusion = 2, 
		Misc = 4, 
		Resource = 8
	};

	// Use this for initialization
	void Start () {
		int heightCoord = height * 3;
		int widthCoord = width * 3;

		Random.InitState((int)System.DateTime.Now.Ticks);
		board = new int[heightCoord, widthCoord];
		treeBoard = new int[heightCoord, widthCoord];
		InitBoard(board);
		InitBoard(treeBoard);

		tileHolder = new GameObject("Board").transform;
		treeHolder = new GameObject("Trees").transform;
		protrusionHolder = new GameObject("Protrusions").transform;

		position = new Vector3(0, 0, 0);
		scale = new Vector3(0, 0, 0);
		offsetX = Random.Range(0, 9999f);
		offsetY = Random.Range(0, 9999f);
		
		int[] distribution = {0,0,0,0,0,0,0,0,0,0,0};
		for (int r = 0; r < widthCoord; r++) {
			for (int c = 0; c < heightCoord; c++) {
				int tileIndex = randomTile(r, c);
				distribution[tileIndex]++;

				if (r % 3 == 0 && c % 3 == 0) {
					position.Set(r, 0, c);
					GameObject tileToInstantiate = Instantiate(Tile[tileIndex%2], position, Quaternion.identity);
					tileToInstantiate.transform.SetParent(tileHolder.gameObject.transform);
				}

				if (tileIndex <= 2 || tileIndex >= 8) {
					treePlacement(r, c);
				}

				if (tileIndex >= 7) {
					protrusionPlacement(r, c, tileIndex);
				}
			}
		}

		player.transform.position = new Vector3(width*3/2, 1.5f, height*3/2);
		for (int i = 0; i < distribution.Length; i++) {
			print(distribution[i]);
		}
	}
	
	private void InitBoard(int[,] board) {
		for (int x = 0; x < height * 3; x++) {
			for (int y = 0; y < width * 3; y++) {
				board[x,y] = (int)TerrainItem.Nothing;
			}
		}
	}
	private void protrusionPlacement(int r, int c, int tileIndex) {
		position.Set(r, 0, c);
		GameObject protrusionToInstantiate;
		switch (tileIndex) {
			case 7: 
				protrusionToInstantiate = Instantiate(protrusions[0], position, Quaternion.identity);
				break;
			case 8: 
				protrusionToInstantiate = Instantiate(protrusions[1], position, Quaternion.identity);
				break;
			case 9:
				protrusionToInstantiate = Instantiate(protrusions[2], position, Quaternion.identity);
				break;
			case 10:
				protrusionToInstantiate = Instantiate(protrusions[2], position, Quaternion.identity);
				break;
			default:
				protrusionToInstantiate = new GameObject("empty");
				break;
		}
		protrusionToInstantiate.transform.SetParent(protrusionHolder.gameObject.transform);
	}

	private void treePlacement(int r, int c) {
		float scaleFactor = 0f;
		float randomTree = Random.Range(0f, 2f);
		GameObject treeToInstantiate;

		if (randomTree < 1.9f) {
			return;
		}

		randomTree = Random.Range(0,3);

		if (randomTree < 1) {
			scaleFactor = 1 + Random.Range(0, .75f);

			position.Set(r, .75f, c);
			scale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[0], position, Quaternion.identity);
		} else if (randomTree >= 1 && randomTree < 2) {
			scaleFactor = 1 + Random.Range(0, .75f);

			position.Set(r, .3f, c);
			scale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[1], position, Quaternion.identity);
		} else {
			scaleFactor = 1 + Random.Range(0, .75f);

			position.Set(r, .3f, c);
			scale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[2], position, Quaternion.identity);
		}
		treeToInstantiate.transform.SetParent(treeHolder);
		treeToInstantiate.transform.localScale = scale;
	}

	public int randomTile(int x, int y) {
		float coordX = ((float)x / width) * samplingScale + offsetX;
		float coordY = ((float)y / height) * samplingScale + offsetY;

		float sample = Mathf.PerlinNoise(coordX, coordY);
		sample = Mathf.Floor(Mathf.Abs(sample) * 100.0f);

		int index = (int)sample / 10;
		return index;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
