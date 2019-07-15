using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuCarTestTrack : MonoBehaviour {

    private bool isActive;
    private bool spawnCar;
    public GameObject playerPrefab;
    public GameObject carCameraPrefab;
    public GameObject menuCameraPrefab;
    public GameObject mainCameraScript;
    public Transform respawnPosition;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive)
        {
            //be able to drive
            if(spawnCar)
            {
                Debug.Log("Spawning Player");
                Instantiate(playerPrefab, respawnPosition.position,Quaternion.identity);
                
                Debug.Log("Spawning Camera");
                Destroy(FindObjectOfType<Camera>().gameObject);
                Instantiate(carCameraPrefab, respawnPosition.position,Quaternion.identity);
                Debug.Log("Disabling Main Menu Script");
                mainCameraScript.SetActive(false);
                //GetComponent<NewMainMenu>().enabled = false;
                spawnCar = false;
            }
            if(Input.GetButtonDown("Back_1"))
            {
                Destroy(FindObjectOfType<TheEggScript>().gameObject);
                Destroy(FindObjectOfType<CameraControl>().gameObject);
                mainCameraScript.SetActive(true);
                GameObject tempObj = Instantiate(menuCameraPrefab, FindObjectOfType<MainCameraScript>().transform);

                mainCameraScript.GetComponent<MainCameraScript>().Camera = tempObj.GetComponent<Camera>();
                FindObjectOfType<NewMainMenu>().isActive = true;
            }
        }
	}

    public void CarSpawn()
    {
        spawnCar = true;
        isActive = true;
    }
}
