using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDiagonalRise : MonoBehaviour {

   
    public bool startMove = false;
    public bool firsttime = true;
    public float speedCoef;

    public Vector3 movementVector;

    IEnumerator coroutine;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(startMove)
        {
            TransferPlatform();
        }
	}

    public void OnTriggerEnter()
    {
        if (firsttime)
        {
            startMove = true;
            firsttime = false;
        }
    }

    public void TransferPlatform()
    {
        //yield return new WaitForSeconds(1f);

        if(transform.position.y < 480)
        {
            transform.Translate(movementVector * Time.deltaTime * speedCoef);
        }
    }
}
