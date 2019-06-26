using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour {

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float maxBrakeTorque;
    public float boostPower;
    public float rotateSpeed;
    public bool boost;
    public bool drivable;
    public float spinTime;
    public float old;
    public bool[] prevFrames = new bool[3];
    
    
    
    

    public WheelCollider WheelConfigure1;
    public WheelCollider WheelConfigure2;
    public WheelCollider WheelConfigure3;
    public WheelCollider WheelConfigure4;

    public ParticleSystem Exhast;

    private Vector3 CenterOfMass = new Vector3(0f, .1f, 0f);


    void Start()
    {
        drivable = true;
        WheelConfigure1.ConfigureVehicleSubsteps(1f, 12, 13);
        WheelConfigure2.ConfigureVehicleSubsteps(1f, 12, 13);
        WheelConfigure3.ConfigureVehicleSubsteps(1f, 12, 13);
        WheelConfigure4.ConfigureVehicleSubsteps(1f, 12, 13);
        InputHandler.LoadInputs();
        prevFrames = new bool[3];

        //GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
    }

    public void FixedUpdate()
    {

        float motor = 0; ;
        float steering = 0; ;
        float braking = 0; ;
        bool boost = false;
        if (drivable)
        {

           
        motor = maxMotorTorque * Input.GetAxis(SavedInputs.gas);
        steering = maxSteeringAngle * Input.GetAxis("L_XAxis_1");
        braking = maxBrakeTorque* Input.GetAxis(SavedInputs.brake);
        boost = Input.GetButton(SavedInputs.boost);
        }
        if (boost)
        {
            Vector3 boostForce = new Vector3(0, 0, -boostPower);
            GetComponent<Rigidbody>().AddRelativeForce(boostForce,ForceMode.Force);

            FindObjectOfType<AudioManager>().Play("Boost");
                Exhast.Play();
            
        }
        
        if(!boost)
        {

            FindObjectOfType<AudioManager>().Stop("Boost");
            Exhast.Stop();
            
        }
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (axleInfo.braking)
            {
                axleInfo.leftWheel.brakeTorque = braking;
                axleInfo.rightWheel.brakeTorque = braking;
            }
        }

        //Checking to see if we are on the ground with our wheels
        GetComponent<Rigidbody>().freezeRotation = false;
        if(FindObjectOfType<Spinbox>().boxtrigger)
        {
            // Checking Inputs to see if we should rotate in the Air
            if(!Input.GetButton(SavedInputs.rotateControl) && (Input.GetAxis("L_XAxis_1") != 0 || Input.GetAxis("L_YAxis_1") != 0))
            {
                Vector3 yRotate = new Vector3(0, rotateSpeed * spinTime * Input.GetAxis("L_XAxis_1"), 0);
                transform.Rotate(yRotate);
                Vector3 xRotate = new Vector3(rotateSpeed *spinTime* Input.GetAxis("L_YAxis_1"), 0, 0);
                transform.Rotate(xRotate);
                UpdateSpin();
            }

            else if(Input.GetAxis("L_XAxis_1") != 0 || Input.GetAxis("L_YAxis_1") != 0)
            {
                Vector3 zRotate = new Vector3(0, 0, rotateSpeed * spinTime * Input.GetAxis("L_XAxis_1"));
                transform.Rotate(zRotate);
                Vector3 xRotate = new Vector3(rotateSpeed * spinTime * Input.GetAxis("L_YAxis_1"), 0, 0);
                transform.Rotate(xRotate);
                UpdateSpin();
            }
            
            //Checking to see if we should reset the spin rotation variable
            
            
            
            GetComponent<Rigidbody>().freezeRotation = true;
                
            
        }
        
        
            prevFrames[2] = prevFrames[1];
            prevFrames[1] = prevFrames[0];
            prevFrames[0] = (Input.GetAxis("L_XAxis_1") != 0 || Input.GetAxis("L_YAxis_1") != 0);
        

    }

    public void SetDriving(bool drive)
    {
        drivable = drive;
    }
    
    public void UpdateSpin()
    {
        spinTime += Time.deltaTime * 2.4f;
        if (spinTime > 1)
            spinTime = 1;

        if (prevFrames[0] == false && prevFrames[1] == false /*&& prevFrames[2] == false*/)
        {
            spinTime = 0;
        }


    }
    
}

