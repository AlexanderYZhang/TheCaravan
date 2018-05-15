using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {


	public static Inventory instance;

    void Awake() {
		if (instance != null) {
			Debug.LogWarning("More than one instance of Inventory found");
			return;
		}
		instance = this;
	}

	public int wood;
	public int stone;
    public int energy;
	public int turrets;
    public TurretData[] turretData;

    [System.Serializable]
    public class TurretData {
        public string turretType;
        public int wood;
        public int stone;
        public int energy;
        public GameObject prefab;
        public Material seeThrough;
        public Material seeThroughError;
        public Material primary;
    };

    public Text woodText;
    public Text stoneText;

    void Start() {
        woodText.text = "x " + wood.ToString();
        stoneText.text = "x " + stone.ToString();
    }
	
	public void AddWood(int quantity) {
		wood += quantity;
		woodText.text = "x " + wood.ToString();
	}

    public void AddStone(int quantity)
    {
		stone += quantity;
		stoneText.text = "x " + stone.ToString();
    }

    public bool EnoughForTurret(int turretCode) {
        TurretData data = turretData[turretCode];
        if (wood - data.wood >= 0 && stone - data.stone >= 0 && energy - data.energy >= 0) {
            return true;
        }
        return false;
    }

    public TurretData GetTurretData(int turretCode) {
        return turretData[turretCode];
    }

    public void AddTurret(int turretCode, int quantity)
    {
        TurretData data = turretData[turretCode];

        wood -= data.wood * quantity;
        stone -= data.stone * quantity;
        energy -= data.energy * quantity;

        woodText.text = "x " + wood.ToString();
        stoneText.text = "x " + stone.ToString();

        print("Adding Turret");

        turrets += quantity;
    }

	
}
