using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RespawnControl : MonoBehaviourPunCallbacks {

    /* public GameObject MainCar;
     public Camera MainCamera;
     public Transform Spawn;
     public GameObject Menu;*/

    public Transform spawnPoint;
    public GameObject Car;
    public GameObject playerPrefab;

    private void Start()
    {
        Car = null;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {

            if (player.GetPhotonView().IsMine)
            {
                Car = player;
            }
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown(SavedInputs.restartLevel))
            Respawn();
    }

    public void Respawn()
    {
        Destroy(Car);
        Debug.Log("Instatiating Player");
        Car =  PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);
        FindObjectOfType<CameraControl>().SetCameraTarget(Car);
        FindObjectOfType<CameraControl>().transform.position = spawnPoint.position;

    }
            

            


    }
