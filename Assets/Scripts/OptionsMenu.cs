using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour {

    public bool isActive;
    public GameObject[] SoundObjects;
    public enum GameState { SOUND, CONTROLS};

    public GameObject Sound;
   
    public GameObject Controls;
    
    public GameObject CurrentObject;
    public int CurrentObjectNumber;

    public Camera Camera1;
    public GameObject CamPos1;
    public GameObject CamPos2;
    public GameObject CamPos3;

    public Material HighlightedMaterial;
    public Material NonHighlightedMaterial;

    public AudioMixer MasterVolume;

    
    

    GameState CurrentState = GameState.SOUND;

    public float timeDelay;

    // Use this for initialization
    void Start () {
        timeDelay = 0f;
        isActive = false;
        SetSound();
        

    }
	
	// Update is called once per frame
	void Update () {
        if (isActive && timeDelay > .15f)
        {
            if (CurrentState == GameState.SOUND)
            {

                Sound.GetComponent<MeshRenderer>().material = HighlightedMaterial;
                if (Input.GetAxis("L_XAxis_1") > 0 & timeDelay > .2f)
                    UpSound();
                if (Input.GetAxis("L_XAxis_1") < 0 & timeDelay > .2f)
                    DownSound();
                if (Input.GetAxis("L_YAxis_1") > 0)
                {
                    CurrentState = GameState.CONTROLS;

                    Controls.GetComponent<MeshRenderer>().material = HighlightedMaterial;

                    Sound.GetComponent<MeshRenderer>().material = NonHighlightedMaterial;
                }

            }

            else if (CurrentState == GameState.CONTROLS)
            {

                Controls.GetComponent<MeshRenderer>().material = HighlightedMaterial;
                if (Input.GetButtonDown("A_1"))
                    ToControls();
                if (Input.GetAxis("L_YAxis_1") < 0)
                {
                    CurrentState = GameState.SOUND;

                    Sound.GetComponent<MeshRenderer>().material = HighlightedMaterial;

                    Controls.GetComponent<MeshRenderer>().material = NonHighlightedMaterial;
                }

            }
            if (Input.GetButtonDown("B_1"))
                
                ToMainMenu();
            
        }
        else
        {
            
            CurrentState = GameState.SOUND;
            
        }
        timeDelay += Time.deltaTime;
    }

    public void UpSound()
    {
        if (CurrentObjectNumber == 9)
            return;

        CurrentObjectNumber += 1;
        CurrentObject = SoundObjects[CurrentObjectNumber];
        
        CurrentObject.SetActive(true);
        timeDelay = 0f;
        SavedInputs.soundLevel = ((CurrentObjectNumber - 9f) * (80f / 9f));
        MasterVolume.SetFloat("volume", ((CurrentObjectNumber - 9f) * (80f / 9f)));
        InputHandler.SaveInputs();
    }

    public void DownSound()
    {
        if (CurrentObjectNumber == 0)
            return;
        
        CurrentObject = SoundObjects[CurrentObjectNumber];

        CurrentObject.SetActive(false);
        CurrentObjectNumber -= 1;
        timeDelay = 0f;
        SavedInputs.soundLevel = ((CurrentObjectNumber - 9f) * (80f / 9f));
        MasterVolume.SetFloat("volume", ((CurrentObjectNumber - 9f) * (80f / 9f)));
        InputHandler.SaveInputs();
    }

    public void ToggleActive()
    {
        isActive = !isActive;
    }

    public void ToControls()
    {
        

        FindObjectOfType<ControlsMenu>().isActive = true;
        isActive = false;
        FindObjectOfType<MainCameraScript>().movetoControls = true;
        FindObjectOfType<MainCameraScript>().timeDelay = 0f;
        Sound.GetComponent<MeshRenderer>().material = NonHighlightedMaterial;
        Controls.GetComponent<MeshRenderer>().material = NonHighlightedMaterial;
        //Dont touch the time delay here. It delays the controls menu
        //from activating, so that it doesnt take the input from this Update.
        //No Touchy
        FindObjectOfType<ControlsMenu>().timeDelay = -0.1f;
        
       

    }

    public void ToMainMenu()
    {
        

        isActive = false;
        Debug.Log("Moving to Main Menu");
        FindObjectOfType<MainCameraScript>().ClearFlags();
        FindObjectOfType<NewMainMenu>().isActive = true;
        FindObjectOfType<MainCameraScript>().timeDelay = 0f;
        FindObjectOfType<MainCameraScript>().movetoMain = true;
        Sound.GetComponent<MeshRenderer>().material = NonHighlightedMaterial;
        Controls.GetComponent<MeshRenderer>().material = NonHighlightedMaterial;
        
        
    }

    public void SetSound()
    {

        int soundIndex = (int)(SavedInputs.soundLevel*(9f/80f)+9f);

        for (int i = 0; i < soundIndex; i++)
        {
            UpSound();
        }
    }
}
