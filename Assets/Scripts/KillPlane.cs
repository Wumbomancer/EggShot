using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class KillPlane : MonoBehaviour {

    public Camera CarCam;
    public GameObject Winner;
    public Transform Spawn;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        this.GetComponent<RespawnControl>().Respawn();
    }
}
