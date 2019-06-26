using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinbox : MonoBehaviour {


    public bool boxtrigger;

    public void OnTriggerEnter(Collider other)
    {
        boxtrigger = false;
    }

    public void OnTriggerExit(Collider other)
    {
        boxtrigger = true;
    }



}
