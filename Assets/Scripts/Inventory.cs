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
	public int turrets;

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

    public void AddTurret(int quantity)
    {
		turrets += quantity;
    }

	
}
