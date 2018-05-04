using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

	public Text woodText;
	public Text stoneText;

	void OnResourceChange(string resource, int quantity) {
		if (string.Equals(resource, "wood")) {
			woodText.text = "x " + quantity.ToString();
		} else if (string.Equals(resource,"stone")) {
			stoneText.text = "x " + quantity.ToString();
		}
	}
}
