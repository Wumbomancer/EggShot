using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCar : MonoBehaviour {

    public GameObject Car;
    public IEnumerator coroutine;

    public bool touching;

    /* public void FixedUpdate()
     {
         if (touching)
         { 
             coroutine = RightSelf();
             StartCoroutine(coroutine);
         }
     }

     public void OnTriggerEnter(Collision other)
     {
         touching = true;
        // coroutine = RightSelf();
        // StartCoroutine(coroutine);
     }

     public void OnTriggerExit(Collision other)
     {
         touching = false;
     }


     IEnumerator RightSelf()
     {
         yield return new WaitForSeconds(1.5f);
         if (touching)
         {
             Car.GetComponent<Rigidbody>().AddForce(Vector3.up*10, ForceMode.VelocityChange);
             Car.transform.rotation = new Quaternion(0, Car.transform.rotation.y, 0,0);
             touching = false;
         }
     }*/
}
