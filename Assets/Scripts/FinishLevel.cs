using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour {

    // Use this for initialization


    private void OnTriggerEnter(Collider other)
    {
        EndLevel();
    }

    public void EndLevel()
    {
        Debug.Log("You Did It");
    }
}
