using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
	public GameObject player;
	public GameObject[] Tile;
	public GameObject[] Trees;
	public GameObject[] Protrusions;
	public GameObject[] Rocks;
	
	public int width;
	public int height;
	public float samplingScale;
	private float offsetX;
	private float offsetY;
	private Transform tileHolder;
	private Transform treeHolder;
	private Transform protrusionHolder;
	private Transform resourceHolder;
	private Vector3 position;
	private Vector3 scale;
	private int[,] board;
	private int[,] protrusionBoard;

	private enum TerrainItem {
		Nothing = 0,
		Tree = 1,
		Protrusion = 2, 
		Misc = 4, 
		Resource = 8
	};

	// Use this for initialization
	void Start () {
		int heightCoord = height;
		int widthCoord = width;
		offsetX = Random.Range(0, 9999f);
		offsetY = Random.Range(0, 9999f);
		
		Random.InitState((int)System.DateTime.Now.Ticks);
		InitBoard();

		tileHolder = new GameObject("Board").transform;
		treeHolder = new GameObject("Trees").transform;
		protrusionHolder = new GameObject("Protrusions").transform;
		resourceHolder = new GameObject("Resources").transform;

		position = new Vector3(0, 0, 0);
		scale = new Vector3(0, 0, 0);

		
		for (int r = 0; r < width; r++) {
			for (int c = 0; c < height; c++) {
				float offset = 0f;
				if (protrusionBoard[r,c] != (int) TerrainItem.Nothing) {
					offset = protrusionPlacement(r, c);
				}

				if (board[r,c] == (int) TerrainItem.Tree) {
					treePlacement(3 * r, 3 * c, offset);
				}

				if (board[r,c] == (int) TerrainItem.Resource) {
					rockPlacement(3 * r, 3 * c);
				}

				// position.Set(3 * r, 0, 3 * c);
				// GameObject tileToInstantiate = Instantiate(Tile[randomTile(r,c)%2], position, Quaternion.identity);
				// tileToInstantiate.transform.SetParent(tileHolder.gameObject.transform);
			}
		}

		player.transform.position = new Vector3(width*3/2, 1.5f, height*3/2);
	}
	
	private void InitBoard() {
		int numRocks = 50, i = 0;
		board = new int[height, width];
		protrusionBoard = new int[height, width];

		int[] distribution = {0,0,0,0,0,0,0,0,0,0,0};
		for (int x = 0; x < height; x++) {
			for (int y = 0; y < width; y++) {
				int tileIndex = randomTile(x,y);
				float randomTree = Random.Range(0, 2f);
				
				distribution[tileIndex]++;
				
				board[x,y] = (int)TerrainItem.Nothing;
				protrusionBoard[x,y] = (int)TerrainItem.Nothing;
				if ((tileIndex <= 2 || tileIndex >= 8) && randomTree > 1.9f) {
					board[x,y] = (int) TerrainItem.Tree;
				}	

				if (tileIndex >= 7) {
					protrusionBoard[x,y] = (int) TerrainItem.Protrusion;
				}
			}
		}

		while(i < numRocks) {
			i++;
			int randX = (int) Random.Range(0, height);
			int randY = (int) Random.Range(0, width);

			if (protrusionBoard[randX, randY] == (int)TerrainItem.Nothing &&
				board[randX, randY] == (int)TerrainItem.Nothing) {
					board[randX, randY] = (int)TerrainItem.Resource;
			}
		}

		for (i = 0; i < distribution.Length; i++) {
			print(distribution[i]);
		}
	}

	private void rockPlacement(int r, int c) {
		int randRock = (int) Random.Range(0, 1f);
		position.Set(r, 0, c);
		GameObject rockToInstantiate = Instantiate(Rocks[0], position, Quaternion.identity);
		rockToInstantiate.transform.SetParent(resourceHolder.gameObject.transform);
	}

	private float protrusionPlacement(int r, int c) {
		int tileIndex = randomTile(r, c);
		float offset = 0f;
		position.Set(r*3, 0, c*3);
		GameObject protrusionToInstantiate;
		switch (tileIndex) {
			case 7: 
				protrusionToInstantiate = Instantiate(Protrusions[0], position, Quaternion.identity);
				offset = 2.5f;
				break;
			case 8: 
				protrusionToInstantiate = Instantiate(Protrusions[1], position, Quaternion.identity);
				offset = 2.7f;
				break;
			case 9:
				protrusionToInstantiate = Instantiate(Protrusions[2], position, Quaternion.identity);
				offset = 4.8f;
				break;
			case 10:
				protrusionToInstantiate = Instantiate(Protrusions[2], position, Quaternion.identity);
				offset = 4.8f;
				break;
			default:
				protrusionToInstantiate = new GameObject("empty");
				break;
		}
		protrusionToInstantiate.transform.SetParent(protrusionHolder.gameObject.transform);
		return offset;
	}

	private void treePlacement(int r, int c, float offset) {
		float scaleFactor = 0f;
		float randomTree = Random.Range(0f, 2f);
		GameObject treeToInstantiate;

		randomTree = Random.Range(0,3);

		if (randomTree < 1) {
			scaleFactor =  3 + Random.Range(0, .75f);

			position.Set(r, .75f + offset, c);
			scale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[0], position, Quaternion.identity);
		} else if (randomTree >= 1 && randomTree < 2) {
			scaleFactor = 3 + Random.Range(0, .75f);

			position.Set(r, .3f + offset, c);
			scale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[1], position, Quaternion.identity);
		} else {
			scaleFactor = 3 + Random.Range(0, .75f);

			position.Set(r, .3f + offset, c);
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
