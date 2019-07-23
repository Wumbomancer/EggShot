using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFan : MonoBehaviour {

    private bool isActive;
    private GameObject collisionObject;
    

    private void Start()
    {
        isActive = false;
        collisionObject = null;
        
    }
    // Update is called once per frame
    void FixedUpdate () {
		//on collision, add force up. limit speed of vehicle while in air
        if(isActive)
        {
            collisionObject.GetComponentInParent<Rigidbody>().AddForce(0,200,0,ForceMode.Acceleration);
            
        }
	}

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collisionObject = collision.gameObject;
            isActive = true;
            
        }
        
    }
    private void OnTriggerExit(Collider collision)
    {
        isActive = false;

    }
}
