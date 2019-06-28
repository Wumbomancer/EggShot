using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class TheEggScript : MonoBehaviourPunCallbacks {

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float maxBrakeTorque;
    public float boostPower;
    public float rotateSpeed;
    public ParticleSystem Exhast;
    public ParticleSystem Glow;
    public ParticleSystem Afterburn;
    public float spinTime;
    public WheelCollider LFWheel;
    public WheelCollider RFWheel;
    public WheelCollider LRWheel;
    public WheelCollider RRWheel;
    public static GameObject LocalPlayerInstance;

    //All bools and flags
    public bool isDrivable;
    public bool touchingGround;
    public bool[] prevFrames = new bool[3];
    public bool startedBraking;




    private void Awake()
    {
        if (photonView.IsMine)
        {
            TheEggScript.LocalPlayerInstance = this.gameObject;
            FindObjectOfType<CameraControl>().SetNewCar(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        isDrivable = true;
        InputHandler.LoadInputs();
        
        RRWheel.ConfigureVehicleSubsteps(5, 25, 25);

        GetComponent<Rigidbody>().centerOfMass = new Vector3(-1f, -1.5f, 0);
        StartCoroutine(Eggshot());
        if(photonView.IsMine)
        {
            FindObjectOfType<CameraControl>().SetNewCar(this.gameObject);
        }

    }

    // Update is called once per frame
    void FixedUpdate () {
		if(isDrivable && photonView.IsMine)
        {
            GetDriving();
            BoostCheck();
            RotateControl();
         




           

        }
        
	}

    public IEnumerator Eggshot()
    {
        while(!Input.GetButtonDown("Y_1"))
        {
            yield return null;
        }
        GameObject TempCar = new GameObject();

        transform.Find("TheEgg").transform.SetParent(TempCar.transform);
        TempCar.transform.DetachChildren();
        Destroy(TempCar);
        GetComponentInChildren<MeshCollider>().gameObject.SetActive(false);
        FindObjectOfType<CameraControl>().CameraState = CameraControl.CameraPositions.DetachedEgg;
        FindObjectOfType<DetachedEgg>().Eggtime = true;
        isDrivable = false;
        GetComponent<Rigidbody>().freezeRotation = false;
    }

    public void BoostCheck()
    {
        bool boost;
        boost = Input.GetButton(SavedInputs.boost);
    
        if (boost)
        {
            Vector3 boostForce = new Vector3(0, 0, -boostPower);
            GetComponent<Rigidbody>().AddRelativeForce(boostForce, ForceMode.Force);

            AudioManager.instance.Play("Boost");
            Exhast.Play();
            Glow.Play();
            Afterburn.Play();
            
            
        }
        
        if(!boost)
        {

            AudioManager.instance.Stop("Boost");
            Exhast.Stop();
            Glow.Play();
            Afterburn.Play();
            
        }
    }

    public void GetDriving()
    {
        float motor;
        float steering;
        float braking;
        bool ebrake;

        ebrake = Input.GetButton("RB_1");
        
        motor = maxMotorTorque * Input.GetAxis(SavedInputs.gas);
        steering = maxSteeringAngle * Input.GetAxis("L_XAxis_1")/(Mathf.Sqrt(GetComponent<Rigidbody>().velocity.magnitude + 100f)-9);
        braking = 0;

        if (ebrake)
        {
            if(startedBraking == false)
            {
                /*WheelFrictionCurve newWheel = new WheelFrictionCurve
                {
                    asymptoteSlip = .2f,
                    asymptoteValue = .2f,
                    extremumSlip = .2f,
                    extremumValue = .2f
                };
                LFWheel.sidewaysFriction = newWheel;
                LRWheel.sidewaysFriction = newWheel;
                RFWheel.sidewaysFriction = newWheel;
                RRWheel.sidewaysFriction = newWheel;*/
                startedBraking = true;
                RaycastHit hit;
                Physics.Raycast(this.transform.position, Vector3.down, out hit);
                hit.collider.material.dynamicFriction = .1f;
                Debug.Log("Casting Ray"); // You should draw this bitch
            }
            
            
            motor = 0;
            braking = 1000;
            GetComponent<Rigidbody>().freezeRotation = true;
            
        }
        else
        {
            if(startedBraking)
            {
             /*   WheelFrictionCurve newWheel = new WheelFrictionCurve
                {
                    asymptoteSlip = .6f,
                    asymptoteValue = .6f,
                    extremumSlip = .6f,
                    extremumValue = .6f
                };
                LFWheel.sidewaysFriction = newWheel;
                LRWheel.sidewaysFriction = newWheel;
                RFWheel.sidewaysFriction = newWheel;
                RRWheel.sidewaysFriction = newWheel;
                startedBraking = false;*/
                
            }
            braking = 0;
            GetComponent<Rigidbody>().freezeRotation = false;
            

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
    }

    public void RotateControl()
    {
        GetComponent<Rigidbody>().freezeRotation = false;
        if (!RRWheel.isGrounded && !RFWheel.isGrounded && !LRWheel.isGrounded && !LFWheel.isGrounded && !touchingGround)
        {
            if (!Input.GetButton(SavedInputs.rotateControl) && (Input.GetAxis("L_XAxis_1") != 0 || Input.GetAxis("L_YAxis_1") != 0))
            {
                Vector3 yRotate = new Vector3(0, rotateSpeed * spinTime * Input.GetAxis("L_XAxis_1"), 0);
                transform.Rotate(yRotate);
                Vector3 xRotate = new Vector3(rotateSpeed * spinTime * Input.GetAxis("L_YAxis_1"), 0, 0);
                transform.Rotate(xRotate);
                UpdateSpin();
            }

            else if (Input.GetAxis("L_XAxis_1") != 0 || Input.GetAxis("L_YAxis_1") != 0)
            {
                Vector3 zRotate = new Vector3(0, 0, rotateSpeed * spinTime * Input.GetAxis("L_XAxis_1"));
                transform.Rotate(zRotate);
                Vector3 xRotate = new Vector3(rotateSpeed * spinTime * Input.GetAxis("L_YAxis_1"), 0, 0);
                transform.Rotate(xRotate);
                UpdateSpin();
            }

            //Checking to see if we should reset the spin rotation variable



            GetComponent<Rigidbody>().freezeRotation = true;
            prevFrames[2] = prevFrames[1];
            prevFrames[1] = prevFrames[0];
            prevFrames[0] = (Input.GetAxis("L_XAxis_1") != 0 || Input.GetAxis("L_YAxis_1") != 0);
        }
    }

    public void UpdateSpin()
    {
        spinTime += Time.deltaTime * 3.4f;
        if (spinTime > 1)
            spinTime = 1;

        if (prevFrames[0] == false && prevFrames[1] == false && prevFrames[2] == false)
        {
            spinTime = 0;
        }


    }

    public void OnCollisionEnter()
    {
        touchingGround = true;
    }

    public void OnCollisionExit()
    {
        touchingGround = false;
    }


}


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
    public bool braking;
}
