using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {

	public float damage;					//How much damage to do to the enemy

	//If the collider's Game Object has the tag of "Enemy", subtract damage from its health and call the trigger on its animation controller to start the impact animation
	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Enemy") {
			Debug.Log ("Hit Enemy");
			coll.gameObject.GetComponent<EnemyController> ().enemyHealth -= damage;
			coll.gameObject.GetComponent<EnemyController> ().animator.SetTrigger ("tookDamage");
		}
	}
}
