using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public bool isRunning;								//Bool to indicate whether the player is moving or not
	public bool hasDied;								//Bool to indicate whether the player has died or not

	public float speed = 10.0f;							//Speed at which the player moves
	public float playerHealth;							//The player's current health
	public float playerMaxHealth;						//The player's maximum health
	public float playerDamage;							//The amount of damage the player does to enemies

	public GameObject playerHealthBar;					//Reference to the player's health bar
	public GameObject weapon;							//Reference to the player's weapon

	public Animator playerAnimatorController;			//Reference to the player's animator controller

	void Start () {
		//Initializes the playerAnimatorController reference
		playerAnimatorController = GetComponent<Animator> ();
		//Sets the player's current health to maximum
		playerHealth = playerMaxHealth;
		//Sets the damage variable present on the weapon Game Object to be equal to the playerDamage variable
		weapon.GetComponent<DamageScript> ().damage = playerDamage;
		//Disables the collider on the weapon Game Object
		weapon.GetComponent<CapsuleCollider> ().enabled = true;
		//Locks the cursor to the center of the screen
		Cursor.lockState = CursorLockMode.Locked;

	}

	void Update () {
		//Sets the bool inside the animator controller, isRunning, to be equal to the boolean isRunning inside of this class
		playerAnimatorController.SetBool ("isRunning", isRunning);
		//If Fire1 is pressed, begin the BasicAttack coroutine
		if (Input.GetButtonDown ("Fire1")) {
			StartCoroutine (BasicAttack ());
		}
		//If Jump is pressed, begin the Backflip coroutine
		if (Input.GetButtonDown ("Jump")) {
			StartCoroutine (Backflip ());
		}
		//If Escape is pressed and the cursor is not locked, lock the cursor
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (Cursor.lockState != CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		//If the player has not died, call the MovePlayer function
		if (hasDied == false) {
			MovePlayer ();
		}
		//Call the CalculateHealth function
		CalculateHealth ();

		//If the player's health is less than or equal to 0, set hasDied to true and begin the PlayerDeath coroutine
		if (playerHealth <= 0) {
			hasDied = true;
			StartCoroutine (PlayerDeath ());
		}
	}

	//Takes the player's current health and divides it by the player's max health to calculate the percentage of remaining health as calcHealth
	//Then, calls the SetHealthBar function with calcHealth as a parameter
	void CalculateHealth () {
		float calcHealth = playerHealth / playerMaxHealth;
		//If calcHealth is less than or equal to zero, calcHealth is equal to zero
		if (calcHealth <= 0) {
			calcHealth = 0;
		}

		SetHealthBar (calcHealth);
	}

	//Transforms the localScale of the player's health bar to represent the percentage of remaining health the player has
	void SetHealthBar (float myHealth) {
		if (playerHealthBar != null) {
			playerHealthBar.transform.localScale = new Vector3 (myHealth, playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
		}
	}

	//Moves the player based on input from the vertical and horizontal axes
	void MovePlayer () {
		//Stores the input from the vertical axis multiplied by the speed variable as translation
		float translation = Input.GetAxis ("Vertical") * speed;
		//Stores the input from the horizontal axis multiplied by the speed variable as straffe
		float straffe = Input.GetAxis ("Horizontal") * speed;
		//Multiplies translation and straffe by Time.deltaTime
		translation *= Time.deltaTime;
		straffe *= Time.deltaTime;
		//Determines whether or not the player is moving based on the values of translation and straffe
		if (straffe != 0 || translation != 0) {
			isRunning = true;
		} else {
			isRunning = false;
		}
		//Translates the player based on straffe on the x axis and translation on the z axis
		transform.Translate (straffe, 0, translation);
	}
		
	//Coroutine to handle the player's attack mechanic
	IEnumerator BasicAttack() {
		//Calls the trigger in the player's animation controller telling it to start the attack animation
		playerAnimatorController.SetTrigger ("basicAttacking");
		//Enables the player's weapon's collider
		//weapon.GetComponent<CapsuleCollider> ().enabled = true;
		//Waits for the animation to play through
		yield return new WaitForSeconds (.25f);
		//Disables the player's weapon's collider
		//weapon.GetComponent<CapsuleCollider> ().enabled = false;
	}

	//Coroutine to handle the player's sick backflip
	IEnumerator Backflip () {
		//Calls the trigger in the player's animation controller telling it to do a sick a backflip
		playerAnimatorController.SetTrigger ("backflip");
		//Waits for the sick backflip to play through
		yield return new WaitForSeconds (1);
	}

	//Coroutine to handle the player's death
	IEnumerator PlayerDeath () {
		//Calls the trigger in the player's animation controller tellig it to start the death animation
		playerAnimatorController.SetTrigger ("died");
		//Waits for the player the die dramatically
		yield return new WaitForSeconds (5);
		//Reloads the scene
		SceneManager.LoadScene ("DemoScene");
	}

}
