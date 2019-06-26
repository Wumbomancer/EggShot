using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class InputHandler {

    public static SavedInputs currentInput = new SavedInputs();

    public static void SaveInputs()
    {
        //Debug.Log("Saving Player Inputs");
        
        PlayerPrefs.SetString("boost", SavedInputs.boost);
        PlayerPrefs.SetString("gas", SavedInputs.gas);
        PlayerPrefs.SetString("brake", SavedInputs.brake);
        PlayerPrefs.SetString("rotate control", SavedInputs.rotateControl);
        PlayerPrefs.SetString("restart level", SavedInputs.restartLevel);
        PlayerPrefs.SetFloat("sound level", SavedInputs.soundLevel);
        PlayerPrefs.SetString("eggshot", SavedInputs.eggshot);
        PlayerPrefs.Save();
        
    }
	
    public static void LoadInputs()
    {
        if(File.Exists(Application.persistentDataPath + "/playerAlteredInputs.gd"))
        {
            
            SavedInputs.boost = PlayerPrefs.GetString("boost");
            SavedInputs.gas = PlayerPrefs.GetString("gas");
            SavedInputs.brake = PlayerPrefs.GetString("brake");
            SavedInputs.rotateControl = PlayerPrefs.GetString("rotate control");
            SavedInputs.restartLevel = PlayerPrefs.GetString("restart level");
            SavedInputs.eggshot = PlayerPrefs.GetString("eggshot");
            SavedInputs.soundLevel = PlayerPrefs.GetFloat("sound level");
           
        }
        else
        {
            Debug.Log("No Player Input Data. Creating Starting Inputs");
            SavedInputs.boost = "X_1";
            SavedInputs.gas = "TriggersR_1";
            SavedInputs.brake = "TriggersL_1";
            SavedInputs.rotateControl = "RB_1";
            SavedInputs.restartLevel = "B_1";
            SavedInputs.soundLevel = 4f;
            SavedInputs.eggshot = "Y_1";
            SavedInputs.current = new SavedInputs();
            SaveInputs();
            currentInput = SavedInputs.current;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerAlteredInputs.gd");
            bf.Serialize(file, InputHandler.currentInput);
            file.Close();
        }
    }
}

