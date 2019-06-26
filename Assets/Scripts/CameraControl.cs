using System.Collections;
using UnityEngine;
using Photon.Pun;


public class CameraControl : MonoBehaviourPun {

    public enum CameraPositions { LoopBehavior, ControlBehavior, Default, DetachedEgg };

    public Transform Car;
    public Vector3 offset;
    public float smoothness;
    public Vector3 rotationOffset;
    public GameObject CameraDesiredPosition;
    public GameObject CameraDesiredParentPosition;
    public GameObject DetachedEgg;
    public float loopRotation;
    public float prevLoopRotation;
    public Transform DetachedCameraPosition;
    
    public float maxCameraHeight;
    public float maxCameraTilt;

    private Vector3 prevCarPos;
    private Vector3 currentCarPos;

    public CameraPositions CameraState = CameraPositions.Default;

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {

            if (player.GetPhotonView().IsMine)
            {
                SetNewCar(player);
            }
        }
    }

    void Start () {
        
        CameraState = CameraPositions.Default;
        DefaultSetup();
        prevCarPos = Car.transform.position;
        currentCarPos = Car.transform.position;
        //if(Car == null)
        Debug.Log("Player");
        
        
    }

	
	void FixedUpdate () {
        prevCarPos = currentCarPos;
        currentCarPos = Car.transform.position;
        prevLoopRotation = loopRotation;
        loopRotation = Car.rotation.x;
        //CameraDesiredPosition.transform.position += (currentCarPos - prevCarPos);
        switch (CameraState)
        {
            case CameraPositions.Default:
                DefaultCameraBehavior();
                break;
            case CameraPositions.LoopBehavior:
                LoopCameraBehavior();
                break;
            case CameraPositions.ControlBehavior:
                ControlCameraBehavior();
                break;
            case CameraPositions.DetachedEgg:
                DetachedEggCameraBehavior();
                break;
            default:
                break;
                
        }



    }

    public void SetNewCar ( GameObject NewCar )
    {
        if (NewCar.GetPhotonView().IsMine)
        {
            Car = NewCar.transform;
            Debug.Log("Thats Mine");

            CameraDesiredPosition = NewCar.transform.GetChild(NewCar.transform.childCount - 1).gameObject;
        }
    }

    public void RotateAroundCar()
    {
        
        


    }

    public void DefaultCameraBehavior()
    {
        /*if(CameraDesiredPosition.transform.IsChildOf(Car))
        {
            GameObject TempCamera = new GameObject();

            CameraDesiredPosition.transform.SetParent(TempCamera.transform);
            TempCamera.transform.DetachChildren();
            Destroy(TempCamera);
        }*/
        Vector3 smoothPosition = Vector3.Lerp(transform.position, new Vector3(CameraDesiredPosition.transform.position.x, CameraDesiredPosition.transform.position.y, CameraDesiredPosition.transform.position.z), Time.deltaTime * smoothness);
        transform.position = smoothPosition;

        transform.LookAt(Car);
        transform.Rotate(rotationOffset);
        
    }

    public void LoopCameraBehavior()
    {
        Vector3 smoothPosition = Vector3.Lerp(transform.position, new Vector3(CameraDesiredParentPosition.transform.position.x, CameraDesiredParentPosition.transform.position.y, CameraDesiredParentPosition.transform.position.z), Time.deltaTime * smoothness);
        transform.position = smoothPosition;

        //CameraDesiredPosition.transform.SetParent(Car);
        
        
        transform.LookAt(Car, Car.up);
        transform.Rotate(rotationOffset);

        //CameraDesiredPosition.transform.RotateAround(Car.position, Car.up, (prevLoopRotation - loopRotation) );
    }

    public void ControlCameraBehavior()
    {
        Vector3 smoothPosition = Vector3.Lerp(transform.position, new Vector3(CameraDesiredPosition.transform.position.x, CameraDesiredPosition.transform.position.y, CameraDesiredPosition.transform.position.z), Time.deltaTime * smoothness);
        transform.position = smoothPosition;

        transform.LookAt(Car);
        transform.Rotate(rotationOffset);
        if (Input.GetAxis("R_XAxis_1") != 0)
        {
            CameraDesiredPosition.transform.RotateAround(Car.transform.position, Vector3.up, Input.GetAxis("R_XAxis_1") * Time.deltaTime * -100);
        }
        if (Input.GetAxis("R_YAxis_1") != 0)
        {
            CameraDesiredPosition.transform.position += new Vector3(0, Input.GetAxis("R_YAxis_1") * Time.deltaTime * 10, 0);
        }
    }

    public void DefaultSetup()
    {
        CameraDesiredPosition.transform.position = Car.position + offset;
    }

    public void DetachedEggCloseUpCameraBehavior()
    {
        //Mathf.Pow((float)(FindObjectOfType<DetachedEgg>().SecondsSinceDetached() * 1.4), 2)
        //Vector3 smoothPosition = Vector3.Lerp(transform.position, DetachedCameraPosition.position,.8f);
        //transform.position = smoothPosition;
        //transform.LookAt(DetachedEgg.transform.position + DetachedEgg.transform.TransformPoint(-2,0,0));
        //transform.Rotate(rotationOffset);
        //transform.SetParent(DetachedEgg.transform);
        transform.SetParent(DetachedEgg.transform);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, DetachedCameraPosition.position,.2f);
        transform.position = smoothPosition;
        Vector3 smoothAngles = Vector3.Lerp(transform.eulerAngles, DetachedCameraPosition.eulerAngles, .2f);
        transform.eulerAngles = smoothAngles;
    }

    public void DetachedEggCameraBehavior()
    {
        if (CameraDesiredPosition.transform.IsChildOf(Car))
        {
            GameObject TempCamera = new GameObject();

            CameraDesiredPosition.transform.SetParent(TempCamera.transform);
            TempCamera.transform.DetachChildren();
            Destroy(TempCamera);
        }
        Vector3 smoothPosition = Vector3.Lerp(transform.position, new Vector3(DetachedCameraPosition.transform.position.x, DetachedCameraPosition.transform.position.y, DetachedCameraPosition.transform.position.z), Time.deltaTime * smoothness);
        transform.position = smoothPosition;

        transform.LookAt(DetachedEgg.transform);
        transform.Rotate(rotationOffset);
    }
}
