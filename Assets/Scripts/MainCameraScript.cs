using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {


    public Camera Camera;
    public GameObject CamPos1;
    public GameObject CamPos2;
    public GameObject CamPos3;
    public GameObject CarCamPos;
    public Transform Car;

    public Quaternion rotationtarget = Quaternion.identity;
    public float timeDelay;
    public bool movetoOptions;
    public bool movetoMain;
    public bool movetoControls;
    public bool movetoPreview;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(movetoControls)
        {
            MoveToControls();
            
            //Debug.Log("Moving to Controls");
        }
        if(movetoMain)
        {
            MoveToMain();
            //Debug.Log("Moving to Main");
        }
        if(movetoOptions)
        {
            MoveToOptions();
            //Debug.Log("Moving to Options");
        }
       
        if(movetoPreview)
        {
            MoveToPreview();
        }
        timeDelay += Time.deltaTime;
    }

    public void MoveToMain()
    {
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, CamPos1.transform.position, (.15f * Mathf.Pow(timeDelay, 2f)));

        SetQuaternion(CamPos1.transform.eulerAngles);
        rotationtarget = Quaternion.Lerp(Camera.transform.rotation, rotationtarget, (.15f * Mathf.Pow(timeDelay, 2f)));
        Camera.transform.rotation = rotationtarget;
    }

    public void MoveToControls()
    {
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, CamPos3.transform.position, (.15f * Mathf.Pow(timeDelay, 2f)));

        SetQuaternion(CamPos3.transform.eulerAngles);
        rotationtarget = Quaternion.Lerp(Camera.transform.rotation, rotationtarget, (.15f * Mathf.Pow(timeDelay, 2f)));
        Camera.transform.rotation = rotationtarget;
    }

    public void MoveToOptions()
    {
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, CamPos2.transform.position, (.15f * Mathf.Pow(timeDelay, 2f)));

        SetQuaternion(CamPos2.transform.eulerAngles);
        rotationtarget = Quaternion.Lerp(Camera.transform.rotation, rotationtarget, (.15f * Mathf.Pow(timeDelay, 2f)));
        Camera.transform.rotation = rotationtarget;
    }

    public void SetQuaternion(Vector3 angle)
    {
        rotationtarget = Quaternion.Euler(angle);
    }

    public void MoveToPreview()
    {
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, CarCamPos.transform.position, .1f);
        Camera.transform.LookAt(Car);
    }

    public void ClearFlags()
    {
        movetoControls = false;
        movetoMain = false;
        movetoOptions = false;
        movetoPreview = false;
    }

    
    
}
