using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerBox1 : MonoBehaviour {

	

    public void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<MainCameraScript>().movetoOptions = false;
        FindObjectOfType<MainCameraScript>().movetoMain = false;
        FindObjectOfType<MainCameraScript>().movetoControls = false;
        //Debug.Log("Im Hit");
    }
}
