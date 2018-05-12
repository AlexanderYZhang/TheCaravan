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
    };

    public Text woodText;
    public Text stoneText;

	
	public void AddWood(int quantity) {
		wood += quantity;
		woodText.text = "x " + quantity.ToString();
	}

    public void AddStone(int quantity)
    {
		stone += quantity;
		stoneText.text = "x " + quantity.ToString();
    }

    public bool EnoughForTurret(int turretCode) {
        TurretData cost = turretData[turretCode];
        if (wood - cost.wood >= 0 && stone - cost.stone >= 0 && energy - cost.energy >= 0) {
            return true;
        }
        return false;
    }

    public GameObject getTurretObject(int turretCode) {
        return turretData[turretCode].prefab;
    }

    public void AddTurret(int quantity)
    {
		turrets += quantity;
    }

	
}
