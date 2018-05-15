using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour {
	public static TerrainGenerator instance;
	public GameObject[] Trees;
	public GameObject[] Protrusions;
	public GameObject[] Rocks;
	public Material material;
	public GameObject goal;
    public int[,] board { get; private set;}
    public int[,] protrusionBoard { get; private set;}
	
	public int width;
	public int height;
	public float samplingScale;
	public float scale;

	private float offsetX;
	private float offsetY;
	private Transform treeHolder;
	private Transform protrusionHolder;
	private Transform resourceHolder;
	private GameObject goalInstance;

	PlayerManager playerManager;

	private enum TerrainItem {
		Nothing = 0,
		Tree = 1,
		Protrusion = 2, 
		Misc = 4, 
		Resource = 8
	};

	void Awake() {
		instance = this;
	}
	void Start() {
		playerManager = PlayerManager.instance;
		GenerateMap();
	}

	public void GenerateMap () {
		int heightCoord = height;
		int widthCoord = width;

        Random.InitState((int)System.DateTime.Now.Ticks);

        offsetX = Random.Range(0, 9999f);
		offsetY = Random.Range(0, 9999f);
		
		InitBoardArray();
		PlaceGoal();

		treeHolder = new GameObject("Trees").transform;
		protrusionHolder = new GameObject("Protrusions").transform;
		resourceHolder = new GameObject("Resources").transform;
		
		for (int r = 0; r < width; r++) {
			for (int c = 0; c < height; c++) {
				float offset = 0f;
				if (protrusionBoard[r,c] != (int) TerrainItem.Nothing) {
					offset = protrusionPlacement(r, c);
				}

				if (board[r,c] == (int) TerrainItem.Tree) {
					treePlacement(r, c, offset);
				}

				if (board[r,c] == (int) TerrainItem.Resource) {
					rockPlacement(r, c);
				}
			}
		}

		// GameObject meshObject = new GameObject("Terrain");
		// MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
		// MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
		// MeshCollider meshCollider = meshObject.AddComponent<MeshCollider>();
		// NavMeshSourceTag navMesh = meshObject.AddComponent<NavMeshSourceTag>();
        // meshFilter.mesh = CreateMesh(width * scale, height * scale);
		// meshRenderer.material = material;
		// meshCollider.sharedMesh = meshFilter.mesh;
		// meshObject.transform.position = new Vector3(scale * ((width - 1) / 2), 0, scale * ((height - 1) / 2));
		// 8 is the ground layer
		// meshObject.layer = 8;
		playerManager.player.transform.position = new Vector3(2, 1, 2);
		playerManager.car.transform.position = new Vector3(25, 0, 15);
	}
	
	public void DestroyMap() {
		Debug.Log("destruction"); 
		if (treeHolder != null) 
		{
			Destroy(treeHolder.gameObject);
		}
        if (protrusionHolder != null)
        {
            Destroy(protrusionHolder.gameObject);
        }
        if (resourceHolder != null)
        {
            Destroy(resourceHolder.gameObject);
        }
		if (goalInstance != null) {
			Destroy(goalInstance);
		}
		playerManager.player.GetComponent<CharController>().SetFocus(null);
        playerManager.player.GetComponent<PlayerMotor>().StopMoveToPoint();
	}

	private void PlaceGoal() {
		int corner = (int) Random.Range(0,3);
		int range = 20;
		int lowerX = 0, lowerY = 0;
		switch (corner) {
			case 0:
				lowerX = 0;
				lowerY = height - range;
				break;
			case 1:
				lowerX = width - range;
				lowerY = 0;
				break;
			case 2:
				lowerX = width - range;
				lowerY = height - range;
				break;
		}

		clearArea(lowerX, lowerY, range);
		Vector3 position = new Vector3((lowerX + range/2) * 3, 0, (lowerY + range/2) * 3);
        goalInstance = Instantiate(goal, position, Quaternion.identity);
	}

	private void clearArea(int lowerX, int lowerY, int range) {
		for (int x = lowerX; x < lowerX + range; x++) {
			for (int y = lowerY; y < lowerY + range; y++) {
				board[x,y] = (int) TerrainItem.Nothing;
				protrusionBoard[x,y] = (int) TerrainItem.Nothing;
			}
		}
	}

	private void InitBoardArray() {
		int numRocks = 50, i = 0;
		board = new int[height, width];
		protrusionBoard = new int[height, width];

		int[] distribution = {0,0,0,0,0,0,0,0,0,0,0};
		for (int x = 2; x < height; x++) {
			for (int y = 2; y < width; y++) {
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

		//Clear spawn area
		clearArea(0, 0, 30);
	}

	private void rockPlacement(int r, int c) {
		int randRock = (int) Random.Range(0, 1f);
        Vector3 position = new Vector3(r * scale, 0, c * scale);

		GameObject rockToInstantiate = Instantiate(Rocks[0], position, Quaternion.identity);
		rockToInstantiate.transform.SetParent(resourceHolder.gameObject.transform);
	}

	private float protrusionPlacement(int r, int c) {
		int tileIndex = randomTile(r, c);
		float offset = 0f;
        Vector3 position = new Vector3(0, 0, 0);

        position.Set(r * scale, 0, c * scale);
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
        Vector3 treeScale = new Vector3(0, 0, 0);
        Vector3 position = new Vector3(r * scale, 0, c * scale);

        randomTree = Random.Range(0,3);

		if (randomTree < 1) {
			scaleFactor =  1 + Random.Range(0, .75f);

			position.Set(position.x, offset, position.z);
			treeScale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[0], position, Quaternion.identity);
		} else if (randomTree >= 1 && randomTree < 2) {
			scaleFactor = 1 + Random.Range(0, .75f);

			position.Set(position.x, offset, position.z);
            treeScale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[1], position, Quaternion.identity);
		} else {
			scaleFactor = 1 + Random.Range(0, .75f);

			position.Set(position.x, offset, position.z);
            treeScale.Set(scaleFactor, scaleFactor, scaleFactor); 

			treeToInstantiate = Instantiate(Trees[2], position, Quaternion.identity);
		}
		treeToInstantiate.transform.SetParent(treeHolder);
		treeToInstantiate.transform.localScale = treeScale;
    }

	public int randomTile(int x, int y) {
		float coordX = ((float)x / width) * samplingScale + offsetX;
		float coordY = ((float)y / height) * samplingScale + offsetY;

		float sample = Mathf.PerlinNoise(coordX, coordY);
		sample = Mathf.Floor(Mathf.Abs(sample) * 100.0f);

		int index = (int)sample / 10;
		return index;
	}

    Mesh CreateMesh(float width, float height)
    {
        Mesh m = new Mesh();
		float topLeftX = (width - 1) / 2;
		float topLeftZ = (width - 1) / 2;
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
			new Vector3(-topLeftX, 0, topLeftZ),
			new Vector3(topLeftX, 0, topLeftZ),
            new Vector3(-topLeftX, 0, -topLeftZ),
    		new Vector3(topLeftX, 0, -topLeftZ),
     	};
        m.uv = new Vector2[] {
        	new Vector2 (0, 0),
        	new Vector2 (0, 1),
        	new Vector2(1, 1),
        	new Vector2 (1, 0)
     	};
        m.triangles = new int[] { 0, 3, 2, 3, 0, 1};
        m.RecalculateNormals();

        return m;
    }

}
