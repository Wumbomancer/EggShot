using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    Rigidbody rb;

    RaycastHit LeftFront;
    RaycastHit RightFront;
    RaycastHit LeftRear;
    RaycastHit RightRear;

    public Transform LeftFrontPos;
    public Transform RightFrontPos;
    public Transform LeftRearPos;
    public Transform RightRearPos;

    public float springConstant;
    public float maxSpringDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RaycastCalculate();

    }


    private bool RaycastCalculate()
    {

        bool lftemp = Physics.Raycast(LeftFrontPos.position, Vector3.down, out LeftFront, maxSpringDistance);
        Debug.DrawRay(LeftFrontPos.position, Vector3.down* maxSpringDistance, Color.green);
        bool rftemp = Physics.Raycast(LeftRearPos.position, Vector3.down, out LeftRear, maxSpringDistance);
        Debug.DrawRay(LeftRearPos.position, Vector3.down * maxSpringDistance, Color.green);
        bool lrtemp = Physics.Raycast(RightFrontPos.position, Vector3.down, out RightFront, maxSpringDistance);
        Debug.DrawRay(RightFrontPos.position, Vector3.down * maxSpringDistance, Color.green);
        bool rrtemp = Physics.Raycast(RightRearPos.position, Vector3.down, out RightRear, maxSpringDistance);
        Debug.DrawRay(RightRearPos.position, Vector3.down * maxSpringDistance, Color.green);
        Debug.Log(springConstant * Mathf.Abs(LeftFront.distance / maxSpringDistance - 1f));

        //Calculations

        float avgForce = (springConstant * Mathf.Abs(LeftFront.distance / maxSpringDistance - 1f) + springConstant * Mathf.Abs(RightFront.distance / maxSpringDistance - 1f) + springConstant * Mathf.Abs(LeftRear.distance / maxSpringDistance - 1f) + springConstant * Mathf.Abs(RightRear.distance / maxSpringDistance - 1f)) / 4f;


        if (lftemp)
            rb.AddForceAtPosition(transform.up * avgForce, LeftFrontPos.position);
        //else
          //  rb.AddForceAtPosition((transform.up) * -.05f * springConstant, LeftFrontPos.position, ForceMode.Impulse);
        if (rftemp)
            rb.AddForceAtPosition(transform.up * avgForce, RightFrontPos.position);
        //else
          //  rb.AddForceAtPosition((transform.up) * -.05f * springConstant, RightFrontPos.position, ForceMode.Impulse);
        if (lrtemp)
            rb.AddForceAtPosition((transform.up) * avgForce, LeftRearPos.position);
        //else
          //  rb.AddForceAtPosition((transform.up) * -.05f * springConstant, LeftRearPos.position, ForceMode.Impulse);
        if (rrtemp)
            rb.AddForceAtPosition(transform.up * avgForce, RightRearPos.position);
        //else
          //  rb.AddForceAtPosition(transform.up * -.05f * springConstant, RightRearPos.position, ForceMode.Impulse);


        return true;
    }

    private void ApplyWheelForces(Transform forcePos)
    {
        rb.AddForceAtPosition(transform.TransformDirection(transform.up)*springConstant, forcePos.position, ForceMode.VelocityChange);
    }
}
