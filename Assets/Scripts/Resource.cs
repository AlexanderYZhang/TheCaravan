using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceStats))]
public class Resource : Interactable {
	ResourceStats resourceStats;
	PlayerManager playerManager;

	void Start () {
		resourceStats = GetComponent<ResourceStats>();
		playerManager = PlayerManager.instance;
	}
	
	public override void Interact() {
		base.Interact();
		
		CharacterGather playerGather = playerManager.player.GetComponent<CharacterGather>();
		if (playerGather != null) {
			playerGather.Gather(resourceStats);
		}
	}
}
