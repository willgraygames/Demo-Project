using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	//public variables
	public bool isRunning;						//Bool to indicate whether the enemy is moving or not

	public int moveSpeed;						//The speed at which the enemy moves
	public int rotationSpeed;					//The speed at which the enemy rotates

	public float enemyHealth;					//The enemy's current health
	public float enemyMaxHealth;				//The enemy's maximum health
	public float enemyDamage;					//The amount of damage the enemy does to the player
	public float sightDistance;					//The distance away from the player that the enemy can "see" the player
	public float attackDistance;				//The distance away from the player that the enemy can attack the player

	public GameObject enemyHealthBar;			//Reference to the enemy's health bar
	public GameObject gameManager;				//Reference to the game manager
	public GameObject sword;					//Reference to the enemy's sword

	public Animator animator;					//Reference to the enemy's animator controller

	//private variables
	bool noticed;								//Bool to indicate when the player gets noticed by the enemy
	bool hasNoticed;							//Bool to indicate whether the player has been noticed by the enemy

	float distanceFromPlayer;					//The distance from the enemy to the player

	Vector3 currPosition;						//The current position of the enemy
	Vector3 lastPosition;						//The position of the enemy in the last frame

	GameObject player;							//Reference to the player's Game Object

	Transform target;							//Reference to the target's transform

	void Start () {
		//Initializes the gameManager variable
		gameManager = GameObject.FindGameObjectWithTag ("GameController");
		//Initializes the player variable
		player = GameObject.FindGameObjectWithTag ("Player");
		//Sets the enemy's health to maximum
		enemyHealth = enemyMaxHealth;
		//Sets the damage variable on the sword Game Object to be equal to the enemyDamage variable
		sword.GetComponent<PlayerDamageScript> ().damage = enemyDamage;
		//Sets noticed and hasNoticed to false
		noticed = false;
		hasNoticed = false;

		//Sets the target to the player's transform
		target = player.transform;

		//Starts the NoticePlayer Coroutine 
		StartCoroutine (NoticePlayer ());
	}

	void Update () {
		//Calculates the distance from the enemy to the player
		distanceFromPlayer = Vector3.Distance (target.position, transform.position);

		//Calls the CalculateHealth function
		CalculateHealth ();

		//If the enemy's health is less than or equal to zero, kill the enemy
		if (enemyHealth <= 0) {
			StartCoroutine (EnemyDeath ());
		}

		//If the enemy has not noticed the player, cycle through some idle animations
		if (noticed != true) {
			int idlePick = Random.Range (0, 3);
			animator.SetInteger ("Idle", idlePick);
		}

		//If the player is still alive, call the MoveEnemy function. If the enemy is within range to attack, start the Attack coroutine
		if (player.GetComponent<PlayerController> ().hasDied == false) {
			MoveEnemy ();
			if (distanceFromPlayer < attackDistance) {
				StartCoroutine (Attack ());
			}
		}
	}

	//Takes the enemy's current health and divides it by the enemy's max health to calculate the percentage of remaining health as calcHealth
	//Then, calls the SetHealthBar function with calcHealth as a parameter
	void CalculateHealth () {
		float calcHealth = enemyHealth / enemyMaxHealth;
		if (calcHealth <= 0) {
			calcHealth = 0;
		}
		SetHealthBar (calcHealth);
	}
	 
	//Transforms the localScale of the enemy's health bar to represent the percentage of remaining health the enemy has
	void SetHealthBar (float myHealth) {
		if (enemyHealthBar != null) {
			enemyHealthBar.transform.localScale = new Vector3 (myHealth, enemyHealthBar.transform.localScale.y, enemyHealthBar.transform.localScale.z);
		}
	}

	//Moves the enemy towards the player
	void MoveEnemy() {
		//Sets the currPosition variable to the enemy's current position
		currPosition = transform.position;

		//If the player is within the enemy's sight distance, the enemy will notice the player
		if (distanceFromPlayer < sightDistance) {
			noticed = true;
		}

		//If the enemy has noticed the player, it will puruse the player based on its moveSpeed and rotationSpeed variables
		if (hasNoticed == true) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotationSpeed * Time.deltaTime);
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
		}

		//If currPosition is equal to lastPosition, then the enemy is moving. If not, then the enemy is not moving
		if (currPosition != lastPosition) {
			isRunning = true;
		} else {
			isRunning = false;
		}

		//Sets the enemy's animation controller booleans to be equal to the booleans of this class
		animator.SetBool ("isRunning", isRunning);
		animator.SetBool ("hasNoticed", hasNoticed);

		//Sets lastPosition as equal to currPosition at the end of the Update function so it will always be one frame behind currPosition
		lastPosition = currPosition;
	}

	//Coroutine to handle the enemy's death
	IEnumerator EnemyDeath() {
		//Sets hasNoticed as false so the enemy will not pursue the player while dying
		hasNoticed = false;
		//Calls the trigger to start the death animation
		animator.SetTrigger ("Dead");
		//Randomly picks one of two death animations
		int deaths = Random.Range (0, 4);
		animator.SetInteger ("whichDeath", deaths);
		//Freezes the rotation of the enemy's rigidbody
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		//Waits for the enemy to die dramatically
		yield return new WaitForSeconds (5);
		//Destroys the enemy's Game Object
		Destroy (this.gameObject);
	}

	//Coroutine to handle the enemy noticing the player
	IEnumerator NoticePlayer() {
		//Waits until the enemy has noticed the player
		yield return new WaitUntil (() => noticed == true);
		//Calls the trigger on the enemy's animation controller to start the enrage animation
		animator.SetTrigger ("noticePlayer");
		//Waits for the enrage animation to play
		yield return new WaitForSeconds (2);
		//Sets hasNoticed to true to indicate the player has been noticed
		hasNoticed = true;
	}

	//Coroutine to handle the enemy attacking the player
	IEnumerator Attack () {
		//Temporarily sets hasNoticed to false so the enemy does not pursue the player while attacking
		hasNoticed = false;
		//Calls the trigger on the enmy's animation controller to start the attack animation
		animator.SetTrigger ("Attack");
		//Waits for the attack animation to play
		yield return new WaitForSeconds (1.4f);
		//Resets hasNoticed back to true
		hasNoticed = true;
	}
}
