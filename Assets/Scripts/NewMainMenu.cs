using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;




public class NewMainMenu : MonoBehaviourPunCallbacks { 

    public enum MenuSelect { PLAY, MULTI, OPTIONS, EXIT};
    
    public GameObject PlaySelected;
    public GameObject MultiSelected;
    public GameObject OptionsSelected;
    public GameObject ExitSelected;
    
    public Camera Camera;
    public GameObject CamPos1;
    public GameObject CamPos2;
    public GameObject CamPos3;

    public Material HighlightMaterial;
    public Material NonHighlightMaterial;

    public bool isActive;
    public bool inTransition;

    private float timeDelay;
    private MenuSelect MenuState = MenuSelect.PLAY;

    private IEnumerator coroutine;

    // Use this for initialization
    void Start () {
        isActive = true;
        SelectPlay();
        timeDelay = 0f;
        InputHandler.LoadInputs();
        FindObjectOfType<AudioManager>().Play("HappyMainSong");

    }
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            float XInput = Input.GetAxis("L_XAxis_1");
            float YInput = Input.GetAxis("L_YAxis_1");
           
            if (MenuState == MenuSelect.PLAY)
            {
                if (YInput > 0 && timeDelay > .2f)
                {
                    SelectMulti();
                    timeDelay = 0f;
                    MenuState = MenuSelect.MULTI;
                    //AudioManager.instance.Stop("MenuPing");
                    //AudioManager.instance.Play("MenuPing");
                }
            }
            else if (MenuState == MenuSelect.MULTI)
            {
                if (XInput > 0 && timeDelay > .2f)
                {
                    SelectOptions();
                    timeDelay = 0f;
                    MenuState = MenuSelect.OPTIONS;
                    //AudioManager.instance.Stop("MenuPing");
                    //AudioManager.instance.Play("MenuPing");
                }
                else if (YInput < 0 && timeDelay > .2f) {
                    SelectPlay();
                    timeDelay = 0f;
                    MenuState = MenuSelect.PLAY;
                    //AudioManager.instance.Stop("MenuPing");
                    //AudioManager.instance.Play("MenuPing");
                }
            }
            else if (MenuState == MenuSelect.OPTIONS)
            {
                if (XInput > 0 && timeDelay > .2f)
                {
                    SelectExit();
                    timeDelay = 0f;
                    MenuState = MenuSelect.EXIT;
                    //AudioManager.instance.Stop("MenuPing");
                    //AudioManager.instance.Play("MenuPing");
                }
                else if (XInput < 0 && timeDelay > .2f) {
                    SelectMulti();
                    timeDelay = 0f;
                    MenuState = MenuSelect.MULTI;
                    //AudioManager.instance.Stop("MenuPing");
                    //AudioManager.instance.Play("MenuPing");
                }

            }
            else if (MenuState == MenuSelect.EXIT)
            {
                if (XInput < 0 && timeDelay > .2f)
                {
                    SelectOptions();
                    timeDelay = 0f;
                    MenuState = MenuSelect.OPTIONS;
                    //AudioManager.instance.Stop("MenuPing");
                    //AudioManager.instance.Play("MenuPing");
                }
            }
            
            

            //Select Menu Option: Run corresponding program
            if (Input.GetButtonDown("A_1") || Input.GetButtonDown("Submit"))
            {
                if (MenuState == MenuSelect.PLAY)
                    FindObjectOfType<Launcher>().Connect();
                if (MenuState == MenuSelect.OPTIONS)
                {
                    FindObjectOfType<MainCameraScript>().ClearFlags();
                    FindObjectOfType<MainCameraScript>().movetoOptions = true;
                    FindObjectOfType<MainCameraScript>().timeDelay = 0f;
                    isActive = false;
                    FindObjectOfType<OptionsMenu>().isActive = true;
                    
                }
                if (MenuState == MenuSelect.EXIT)
                    Exit();
            }

            timeDelay += Time.deltaTime;
        }
        
    }

    public void SelectPlay()
    {
        PlaySelected.GetComponent<MeshRenderer>().material = HighlightMaterial;
        MultiSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        OptionsSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        ExitSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
    }
    public void SelectMulti()
    {
        PlaySelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        MultiSelected.GetComponent<MeshRenderer>().material = HighlightMaterial;
        OptionsSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        ExitSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
    }

    public void SelectOptions()
    {
        PlaySelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        MultiSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        OptionsSelected.GetComponent<MeshRenderer>().material = HighlightMaterial;
        ExitSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
    }

    public void SelectExit()
    {
        PlaySelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        MultiSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        OptionsSelected.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
        ExitSelected.GetComponent<MeshRenderer>().material = HighlightMaterial;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

   

    public void Exit()
    {
        Application.Quit();
    }
}
