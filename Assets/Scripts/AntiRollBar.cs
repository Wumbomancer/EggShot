using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour {

    public WheelCollider WheelL;
    public WheelCollider WheelR;
    public float AntiRoll;

    private WheelHit hit;
    private float travelL = 1f;
    private float travelR = 1f;
    private bool groundedL;
    private bool groundedR;
    private float antiRollForce;



// Update is called once per frame
void FixedUpdate() {
        groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        antiRollForce = (travelL - travelR) * AntiRoll;

        if(groundedL)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * -antiRollForce, WheelL.transform.position);
        }

        if(groundedR)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * antiRollForce, WheelR.transform.position);
        }
    }
}
