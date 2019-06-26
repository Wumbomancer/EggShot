using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MenuSounds : MonoBehaviour {

    public AudioMixer MasterVolume;
	// Use this for initialization
	void Start () {
        LoadStartSounds();
        //AudioManager.instance.Play("MainTheme");
        //AudioManager.instance.Play("WelcomeToEggshot");
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LoadStartSounds()
    {
        InputHandler.LoadInputs();
        MasterVolume.SetFloat("volume", SavedInputs.soundLevel);        
    }
}
