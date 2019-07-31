using System.Collections;
using UnityEngine;
using Photon.Pun;


public class CameraControl : MonoBehaviourPun {

    public enum CameraPositions { LoopBehavior, ControlBehavior, Default, DetachedEgg };

    public GameObject CameraTargetObject;
    
    public float smoothness;
    
    public GameObject CameraDesiredPosition;
    public GameObject DefaultPosition;
    public GameObject DetachedEgg;
    public float loopRotation;
    public float prevLoopRotation;
    public Transform DetachedCameraPosition;
   
    
    public float maxCameraHeight;
    public float maxCameraTilt;


    
    public Vector3 rotationOffset;
    public Vector3 offset;
    private Vector3 prevCarPos;
    private Vector3 currentCarPos;

    public CameraPositions CameraState = CameraPositions.Default;

    private void Awake()
    {
        
    }

    void Start () {
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //foreach (GameObject player in players)
        //{

        //    if (player.GetPhotonView().IsMine || !FindObjectOfType<SceneInfo>().multiplayer)
        //    {
        //        Debug.Log("CameraGrab");
        //        SetNewCar(player);
        //    }

        //}
        CameraTargetObject = null;
        CameraState = CameraPositions.ControlBehavior;
        //prevCarPos = Car.transform.position;
        //currentCarPos = prevCarPos;
        DefaultPosition = null;
        
        
    }

    private void Update()
    {
       
    }

    void FixedUpdate ()
    {
        if (CameraTargetObject != null)
        {
            //prevCarPos = currentCarPos;
            //currentCarPos = Car.transform.position;
            //prevLoopRotation = loopRotation;
            //loopRotation = Car.transform.rotation.x;
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
        else { Debug.Log("Target not set"); }

    }

    public void SetCameraTarget ( GameObject target )
    {
        //if (target.GetPhotonView().IsMine || !FindObjectOfType<SceneInfo>().multiplayer)
        //{
            CameraTargetObject = target;
            Debug.Log("Camera is pointing at: " + CameraTargetObject);

            CameraDesiredPosition = CameraTargetObject.transform.GetChild(target.transform.childCount - 2).gameObject;
            DefaultPosition = CameraTargetObject.transform.GetChild(target.transform.childCount - 1).gameObject;
            Debug.Log(CameraDesiredPosition);
        //}
        CameraDesiredPosition.transform.position = CameraTargetObject.transform.position + offset;
        DefaultPosition.transform.position = CameraTargetObject.transform.position + offset;
    }

    private void DefaultCameraBehavior()
    {
        CameraState = CameraPositions.Default;
        CameraBehavior(CameraTargetObject, CameraDesiredPosition.transform, 0);
    }

    public void LoopCameraBehavior()
    {
        CameraState = CameraPositions.LoopBehavior;
        CameraBehavior(CameraTargetObject, DefaultPosition.transform, 1);
    }

    private void ControlCameraBehavior()
    {
        CameraState = CameraPositions.ControlBehavior;
        CameraBehavior(CameraTargetObject, CameraDesiredPosition.transform, 0);
      
        if (Input.GetAxis("R_XAxis_1") != 0)
        {
            CameraDesiredPosition.transform.RotateAround(CameraTargetObject.transform.position, Vector3.up, Input.GetAxis("R_XAxis_1") * Time.deltaTime * -100);
        }
        if (Input.GetAxis("R_YAxis_1") != 0)
        {

            if (CameraDesiredPosition.transform.position.y - (CameraTargetObject.transform.position + offset).y <= 10)
            {
                CameraDesiredPosition.transform.position += new Vector3(0, Input.GetAxis("R_YAxis_1") * Time.deltaTime * 100, 0);

            }
            else { Debug.Log("Camera Y-limit reached"); }
        }
        else
        {
            CameraBehavior(CameraTargetObject, DefaultPosition.transform, 0);
        }
    }


    private void DetachedEggCloseUpCameraBehavior()
    {
        //Mathf.Pow((float)(FindObjectOfType<DetachedEgg>().SecondsSinceDetached() * 1.4), 2)
        //Vector3 smoothPosition = Vector3.Lerp(transform.position, DetachedCameraPosition.position,.8f);
        //transform.position = smoothPosition;
        //transform.LookAt(DetachedEgg.transform.position + DetachedEgg.transform.TransformPoint(-2,0,0));
        //transform.Rotate(rotationOffset);
        //transform.SetParent(DetachedEgg.transform);
        transform.SetParent(DetachedEgg.transform);
        CameraBehavior(DetachedEgg, DetachedCameraPosition, 2);
    }

    private void DetachedEggCameraBehavior()
    {
        if (CameraDesiredPosition.transform.IsChildOf(CameraTargetObject.transform))
        {
            GameObject TempCamera = new GameObject();

            CameraDesiredPosition.transform.SetParent(TempCamera.transform);
            TempCamera.transform.DetachChildren();
            Destroy(TempCamera);
        }
       // cameraBehavior(DetachedEgg, DetachedCameraPosition, 0);
    }
    public void CameraBehavior(GameObject lookAtObject, Transform cameraOrigin, int behavior) {
        //DefaultCameraBehavior
        if (behavior == 0)
        {
            transform.position = SmoothReposition(transform.position, cameraOrigin.position);
            transform.LookAt(lookAtObject.transform);
        }
        //LoopCameraBehavior
        else if (behavior == 1)
        {
            transform.position = SmoothReposition(transform.position, cameraOrigin.position);
            transform.LookAt(lookAtObject.transform, lookAtObject.transform.up);
        }
        //DetachedEggCloseUpCameraBehavior
        else if (behavior == 2)
        {
            transform.position = Reposition(transform.position, cameraOrigin.position, .2f);
            transform.eulerAngles = Reposition(transform.eulerAngles, cameraOrigin.eulerAngles, .2f);
        }
        //DetachedEggCameraBehavior
        else if (behavior == 3) {

        }

        transform.Rotate(rotationOffset);
    }
    public Vector3 SmoothReposition(Vector3 start, Vector3 end) {
        return Reposition(start, end, Time.deltaTime * smoothness);
    }

    public Vector3 Reposition(Vector3 start, Vector3 end, float scale) {
         return Vector3.Lerp(start, end, scale);
    }
}
