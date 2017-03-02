using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageScript : MonoBehaviour {

	public float damage;						//How much damage to do to the player

	//If the collider's Game Object has the tag of "Player", subtract damage from the player's health
	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<PlayerController> ().playerHealth -= damage;
		}
	}
}
