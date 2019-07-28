using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DrivingUI : MonoBehaviourPunCallbacks {

    public GameObject FirstNumber;
    public GameObject SecondNumber;
    public GameObject Car;
    public Transform PositionFirstNumber;
    public Transform PositionSecondNumber;
    public GameObject[] NumbersLookup;


	void Start () {
        Debug.Log("Filling Numbers Lookup Table");
        NumbersLookup[0] = Resources.Load("Numbers/Number0") as GameObject;
        NumbersLookup[1] = Resources.Load("Numbers/Number1") as GameObject;
        NumbersLookup[2] = Resources.Load("Numbers/Number2") as GameObject;
        NumbersLookup[3] = Resources.Load("Numbers/Number3") as GameObject;
        NumbersLookup[4] = Resources.Load("Numbers/Number4") as GameObject;
        NumbersLookup[5] = Resources.Load("Numbers/Number5") as GameObject;
        NumbersLookup[6] = Resources.Load("Numbers/Number6") as GameObject;
        NumbersLookup[7] = Resources.Load("Numbers/Number7") as GameObject;
        NumbersLookup[8] = Resources.Load("Numbers/Number8") as GameObject;
        NumbersLookup[9] = Resources.Load("Numbers/Number9") as GameObject;
        Debug.Log("Filled Numbers Lookup Table");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {

            if (player.GetPhotonView().IsMine || !FindObjectOfType<SceneInfo>().multiplayer)
            {
                Debug.Log("CameraGrab");
                Car = player;
            }

        }
    }

    // Update is called once per frame
    void Update () {
		

        float speed = Car.GetComponent<Rigidbody>().velocity.magnitude/3f;
        Debug.Log(speed);
        float first = Mathf.Floor(speed / 10f);
        float second = speed - 10f;
        while(second >= 0)
        {
            second -= 10f;
        }
        second += 10f;

        if(FirstNumber != NumbersLookup[(int)first])
        {
            Destroy(FirstNumber);
            FirstNumber = Instantiate(NumbersLookup[(int)first],PositionFirstNumber);
        }

        if(SecondNumber != NumbersLookup[(int)second])
        {
            Destroy(SecondNumber);
            SecondNumber = Instantiate(NumbersLookup[(int)second],PositionSecondNumber);
        }

	}
    
}
