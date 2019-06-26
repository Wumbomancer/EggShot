using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopboxtrigger : MonoBehaviour {

    public GameObject Car;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Rigidbody>().gameObject == Car)
        {
            FindObjectOfType<CameraControl>().CameraState = CameraControl.CameraPositions.LoopBehavior;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Rigidbody>().gameObject == Car)
            FindObjectOfType<CameraControl>().CameraState = CameraControl.CameraPositions.Default;
    }

}
