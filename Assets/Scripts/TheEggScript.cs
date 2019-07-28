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
    public Transform centerOfMass;
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

    public float prevRotation;
    public float currentRotation;




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
        
        RRWheel.ConfigureVehicleSubsteps(20, 25, 50);

        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
        StartCoroutine(Eggshot());
        if(photonView.IsMine || !FindObjectOfType<SceneInfo>().multiplayer)
        {
            FindObjectOfType<CameraControl>().SetNewCar(this.gameObject);
        }

    }

    // Update is called once per frame
    void FixedUpdate () {
		if((isDrivable && photonView.IsMine) || !FindObjectOfType<SceneInfo>().multiplayer)
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
            Glow.Stop();
            Afterburn.Stop();
            
        }
    }

    public void GetDriving()
    {
        float motor;
        float steering;
        float braking;
        bool ebrake;



        ebrake = Input.GetButton("LB_1");
       
        if (WheelsGroundedCheck() && ebrake == false)
        {
            motor = maxMotorTorque * Input.GetAxis(SavedInputs.gas);
            GetComponent<Rigidbody>().AddRelativeForce(0, 250000, 0);
        }
        else
        {
            motor = 0;
        }
        steering = maxSteeringAngle * Input.GetAxis("L_XAxis_1") /(.15f*Mathf.Pow(GetComponent<Rigidbody>().velocity.magnitude,.8f));
        braking = 0;

        if (ebrake)
        {
            if(startedBraking)
            {
                GetComponent<Rigidbody>().AddRelativeForce(0, 150000, 0, ForceMode.Impulse);
            }
            if(WheelsGroundedCheck())
            {
                /*prevRotation = currentRotation;
                currentRotation = this.transform.rotation.y;
                Vector3 tempVec = new Vector3(GetComponent<Rigidbody>().rotation.x, (currentRotation - prevRotation) / 2f, GetComponent<Rigidbody>().rotation.z);
                GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(tempVec));*/
                
            }
            startedBraking = false;
            
            
        }
        else
        {
            startedBraking = true;
            braking = 0;
            
            

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

    public bool WheelsGroundedCheck()
    {
        if(!RRWheel.isGrounded && !RFWheel.isGrounded && !LRWheel.isGrounded && !LFWheel.isGrounded)
        {
            return false;
        }
        return true;
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
