using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

    public class GameManager : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefab;
    public GameObject playerCamera;
    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("We Loading bby");
        if (PhotonNetwork.IsMasterClient)
        {
            
            LoadArena();
        }
    }


    #endregion


    #region Private Methods

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
            Debug.LogError("You are not the Master");
        Debug.LogFormat("Loading Level 1");
        PhotonNetwork.LoadLevel("Level1");
    }

    #endregion

    #region Public Methods

    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.Log("Missing playerPrefab, Check GameManager");
        }
        else
        {
            if (TheEggScript.LocalPlayerInstance == null && FindObjectOfType<SceneInfo>().spawnCar)
            {
                Debug.Log("Instatiating Player");
                //                FindObjectOfType<Canvas>().worldCamera = FindObjectOfType<MainCameraScript>().gameObject.GetComponent<Camera>();
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);

            }
            else
            {
                Debug.Log("Scene Load Ignored");
            }
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
