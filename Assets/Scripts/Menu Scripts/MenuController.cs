using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private static MenuController _instance;
    public static MenuController Instance { get { return _instance; } }
    private Dictionary<string,MenuItemMaster> menuItems = new Dictionary<string, MenuItemMaster>();
    private string currentState;
    private bool menuHoldForInputs = false;
    private Camera mainCamera;
    public Transform[] cameraLocations;

    private void Awake()
    {
        _instance = this;
        /*
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }*/
    }

    private void Start()
    {
        MenuItemMaster[] items = FindObjectsOfType<MenuItemMaster>();
        
        
        foreach (MenuItemMaster item in items)
        {
            menuItems.Add(item.itemName, item);
            

        }
        currentState = "Play";
        menuItems[currentState].IsHover = true;

        mainCamera = FindObjectOfType<Camera>();
        //ChangeCamera(1);

        //StartCoroutine(RuntimeMenu());
    }


    

    public void ChangeCurrentState(string nextState)
    {
        menuItems[currentState].IsHover = false;
        menuItems[nextState].IsHover = true;
        currentState = nextState;
        Debug.Log("The current state is " + currentState);
    }

    public void ActivateCurrentState()
    {
        menuItems[currentState].Select();
    }

    public void ChangeCamera(int position)
    {
        if (position == 1)
        {
            mainCamera.transform.eulerAngles = new Vector3(-12.081f, -77.64f, -.02f);
            mainCamera.transform.position = new Vector3(-326.6f, -62.8f, -91.5f);
        }
        if (position == 2)
        {
            mainCamera.transform.position = new Vector3(68f, 194f, 202f);
            mainCamera.transform.eulerAngles = new Vector3(6.202f, -23.141f, 1.234f);
        }
        if(position == 3)
        {
            mainCamera.transform.position = new Vector3(275f, -181f, 146f);
            mainCamera.transform.eulerAngles = new Vector3(-37.143f, 109.449f, -6.982f);
        }
        
    }

    IEnumerator RuntimeMenu()
    {
        yield return new WaitForEndOfFrame();
        Menustart:
        yield return new WaitUntil(() => Input.GetAxis("L_YAxis_1") != 0 || Input.GetButtonDown("A_1") || Input.GetButtonDown("B_1"));
        if(Input.GetAxis("L_YAxis_1") < 0)
        {
            //Move up
            menuItems[currentState].UpState();
        }
        if(Input.GetAxis("L_YAxis_1") > 0)
        {
            //Move down
            menuItems[currentState].DownState();
        }
        if(Input.GetButtonDown("A_1"))
        {
            //Select Item
            menuItems[currentState].Select();
        }
        else if(Input.GetButtonDown("B_1"))
        {
            //Go Back
            menuItems[currentState].Deselect();
        }
        yield return new WaitUntil(() => menuHoldForInputs == false && Input.GetAxis("L_YAxis_1") == 0);
        goto Menustart;
    }

    

}
