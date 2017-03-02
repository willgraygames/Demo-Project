using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject pauseMenu;			//Reference to the pause menu Game Object
	public GameObject spawner;				//Reference to the spawner Game Object
	public GameObject enemy;				//Reference to the enemy Game Object
	public GameObject rain;					//Reference to the rain Game Object
	public GameObject snow;					//Reference to the snow Game Object

	void Start () {
		//Initializes the pauseMenu Game Object and sets it to inactive
		pauseMenu = GameObject.FindGameObjectWithTag ("PauseMenu");

		pauseMenu.SetActive (false);
	}
		
	void Update () {
		//Calls the PauseMenu function to dictate when the pause menu appears
		PauseMenu ();
		//If the Q key is pressed, create a new enemy at the spawner's location
		if (Input.GetKeyDown (KeyCode.Q)) {
			Instantiate (enemy, new Vector3 (spawner.transform.position.x, spawner.transform.position.y, spawner.transform.position.z), spawner.transform.rotation);
		}
		//If the E key is pressed, toggle the rain Game Object on and off
		if (Input.GetKeyDown (KeyCode.E)) {
			if (rain.activeInHierarchy == true) {
				rain.SetActive (false);
			} else {
				rain.SetActive (true);
			}
		}
		//If the R key is pressed, toggle the snow Game Object on and off
		if (Input.GetKeyDown (KeyCode.R)) {
			if (snow.activeInHierarchy == true) {
				snow.SetActive (false);
			} else {
				snow.SetActive (true);
			}
		}
	}

	//Function to dictate when the pause menu will be active
	void PauseMenu () {
		//If the escape key is pressed and the timescale is not 0, set it to 0, otherwise, check that it is 0 and that the pause menu is active and set it to 1
		if (Input.GetButtonDown ("Cancel") && Time.timeScale != 0) {
			Time.timeScale = 0;
		} else if (Input.GetButtonDown("Cancel") && Time.timeScale == 0 && pauseMenu.activeInHierarchy == true) {
			Time.timeScale = 1;
		}
		//If the time scale is 0, activate the pause menu, otherwise deactivate it
		if (Time.timeScale == 0) {
			pauseMenu.SetActive (true);
		} else {
			pauseMenu.SetActive (false);
		}
	}
}
