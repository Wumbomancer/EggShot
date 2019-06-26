using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {

    public enum GameState { BOOST, GAS, BRAKE, ROTATECONTROL, RESTARTLEVEL, PREVIEW, BACK};

    public GameObject Boost;
    public GameObject Gas;
    public GameObject Brake;
    public GameObject RT;
    public GameObject RL;
    public GameObject Preview;
    public GameObject Back;
    public GameObject X;
    public GameObject Y;
    public GameObject A;
    public GameObject B;
    public GameObject LB;
    public GameObject RB;
    public GameObject LTrigger;
    public GameObject RTrigger;
    public GameObject Car;
    public Transform Position1;
    public Transform Position2;
    public Transform Position3;
    public Transform Position4;
    public Transform Position5;
    public Transform ButtonWaitArea;

    public Camera Camera1;
    public GameObject CamPos1;
    public GameObject CamPos2;
    public GameObject CamPos3;

    public Material HighlightMaterial;
    public Material NonHighlightMaterial;

    public bool isActive;
    public float timeDelay;
    public bool inTransition;

    public GameState CurrentState = GameState.BOOST;

    private IEnumerator coroutine;
    private bool coroutineActive;


    // Use this for initialization
    void Start () {
        isActive = false;
        timeDelay = 0f;
        
        Boost.GetComponent<MeshRenderer>().material = HighlightMaterial;
        
        coroutineActive = true;
        SetOnStart();
    }
	
	// Update is called once per frame
	void Update () {
		if(isActive  && timeDelay > .15f)
        {
            if(Input.GetButtonDown("B_1"))
                Return();
            switch (CurrentState)
            {
                case GameState.BOOST:
                    if (Input.GetButtonDown("A_1"))
                    { coroutine = ButtonInput(0); StartCoroutine(coroutine); isActive = false; }
                    if (Input.GetAxis("L_YAxis_1") > 0)
                    {
                        CurrentState = GameState.GAS;
                        
                        Boost.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        timeDelay = 0f;
                        Gas.GetComponent<MeshRenderer>().material = HighlightMaterial;
                    }
                    return;
                case GameState.GAS:
                    if (Input.GetButtonDown("A_1"))
                    { coroutine = ButtonInput(1); StartCoroutine(coroutine); isActive = false; }
                    if (Input.GetAxis("L_YAxis_1") > 0)
                    {
                        CurrentState = GameState.BRAKE;
                        Gas.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        Brake.GetComponent<MeshRenderer>().material = HighlightMaterial;
                    }

                    if (Input.GetAxis("L_YAxis_1") < 0)
                    {
                        CurrentState = GameState.BOOST;
                        Gas.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        Boost.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }
                    return;
                case GameState.BRAKE:
                    if (Input.GetButtonDown("A_1"))
                    { coroutine = ButtonInput(2); StartCoroutine(coroutine); isActive = false; }
                    if (Input.GetAxis("L_YAxis_1") > 0)
                    {
                        CurrentState = GameState.ROTATECONTROL;
                        Brake.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        RT.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }

                    if (Input.GetAxis("L_YAxis_1") < 0)
                    {
                        CurrentState = GameState.GAS;
                        Brake.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        Gas.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }
                    return;
                case GameState.ROTATECONTROL:
                    if (Input.GetButtonDown("A_1"))
                    { coroutine = ButtonInput(3); StartCoroutine(coroutine); isActive = false; }
                    if (Input.GetAxis("L_YAxis_1") > 0)
                    {
                        CurrentState = GameState.RESTARTLEVEL;
                        RT.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        RL.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }

                    if (Input.GetAxis("L_YAxis_1") < 0)
                    {
                        CurrentState = GameState.BRAKE;
                        RT.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        Brake.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }
                    return;
                case GameState.RESTARTLEVEL:

                    if (Input.GetButtonDown("A_1"))
                    { coroutine = ButtonInput(4); StartCoroutine(coroutine); isActive = false; }
                    if (Input.GetAxis("L_YAxis_1") > 0)
                    {
                        CurrentState = GameState.PREVIEW;
                        RL.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        timeDelay = 0f;
                        Preview.GetComponent<MeshRenderer>().material = HighlightMaterial;
                    }

                    if (Input.GetAxis("L_YAxis_1") < 0)
                    {
                        CurrentState = GameState.ROTATECONTROL;
                        RL.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        timeDelay = 0f;
                        RT.GetComponent<MeshRenderer>().material = HighlightMaterial;
                    }
                    return;

                case GameState.PREVIEW:

                    if (Input.GetButtonDown("A_1"))
                        TestTrack();
                    if (Input.GetAxis("L_XAxis_1") > 0)
                    {
                        CurrentState = GameState.BACK;
                        Preview.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        Back.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }

                    if (Input.GetAxis("L_YAxis_1") < 0)
                    {
                        CurrentState = GameState.RESTARTLEVEL;
                        Preview.GetComponent<MeshRenderer>().material = NonHighlightMaterial;

                        timeDelay = 0f;
                        RL.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }
                    return;
                case GameState.BACK:


                    if (Input.GetButtonDown("A_1"))
                        Return();
                    if (Input.GetAxis("L_XAxis_1") < 0)
                    {
                        CurrentState = GameState.PREVIEW;
                        Back.GetComponent<MeshRenderer>().material = NonHighlightMaterial;
                        
                        timeDelay = 0f;
                        Preview.GetComponent<MeshRenderer>().material = HighlightMaterial;
                        
                    }
                    return;

                default:
                    return;
            }
            


        }
        
        
            
        
        timeDelay += Time.deltaTime;
	}

    public void Return()
    {
        FindObjectOfType<MainCameraScript>().ClearFlags();
        FindObjectOfType<MainCameraScript>().movetoOptions = true;
        FindObjectOfType<MainCameraScript>().timeDelay = 0f;
        FindObjectOfType<OptionsMenu>().timeDelay = -.1f;

        isActive = false;
        FindObjectOfType<OptionsMenu>().isActive = true;
        

    }

    public void TestTrack()
    {
        FindObjectOfType<MainCameraScript>().movetoPreview = true;
        FindObjectOfType<MainCameraScript>().timeDelay = 0f;
        isActive = false;
        Car.SetActive(true);
    }

    IEnumerator ButtonInput(int input)
    {
        if (coroutineActive)
        {
            Debug.Log("Starting Coroutine");
            yield return new WaitForSeconds(.1f);
            Debug.Log("Finished Waiting");
            while (!Input.GetButtonDown("A_1") && !Input.GetButtonDown("B_1") && !Input.GetButtonDown("Y_1") && !Input.GetButtonDown("X_1")
                && !Input.GetButtonDown("LB_1") && !Input.GetButtonDown("RB_1") && !(Input.GetAxis("TriggersL_1") > 0) && !(Input.GetAxis("TriggersR_1") < 0))
                yield return null;


            AlterControls(input);
            

            Debug.Log("Finish Coroutine");
            InputHandler.SaveInputs();
            isActive = true;
        }
        else
            coroutineActive = true;
    }

    public void AlterControls(int input)
    {

        if (Input.GetButtonDown("A_1"))
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "A_1"; A.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "A_1"; A.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "A_1"; A.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "A_1"; A.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "A_1"; A.transform.position = Position5.position; break;
                default: break;
            }


        }
        else if (Input.GetButtonDown("B_1"))
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "B_1"; B.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "B_1"; B.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "B_1"; B.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "B_1"; B.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "B_1"; B.transform.position = Position5.position; break;
                default: break;
            }
        }
        else if (Input.GetButtonDown("Y_1"))
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "Y_1"; Y.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "Y_1"; Y.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "Y_1"; Y.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "Y_1"; Y.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "Y_1"; Y.transform.position = Position5.position; break;
                default: break;
            }
        }
        else if (Input.GetButtonDown("X_1"))
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "X_1"; X.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "X_1"; X.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "X_1"; X.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "X_1"; X.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "X_1"; X.transform.position = Position5.position; break;
                default: break;
            }
        }
        else if (Input.GetButtonDown("RB_1"))
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "RB_1"; RB.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "RB_1"; RB.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "RB_1"; RB.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "RB_1"; RB.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "RB_1"; RB.transform.position = Position5.position; break;
                default: break;
            }
        }
        else if (Input.GetButtonDown("LB_1"))
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "LB_1"; LB.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "LB_1"; LB.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "LB_1"; LB.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "LB_1"; LB.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "LB_1"; LB.transform.position = Position5.position; break;
                default: break;
            }
        }
        else if (Input.GetAxis("TriggersR_1") < 0)
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "TriggersR_1"; RTrigger.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "TriggersR_1"; RTrigger.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "TriggersR_1"; RTrigger.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "TriggersR_1"; RTrigger.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "TriggersR_1"; RTrigger.transform.position = Position5.position; break;
                default: break;
            }
        }
        else if (Input.GetAxis("TriggersL_1") > 0)
        {
            CheckForOverlap(input);
            switch (input)
            {
                case 0: SavedInputs.boost = "TriggersL_1"; LTrigger.transform.position = Position1.position; break;
                case 1: SavedInputs.gas = "TriggersL_1"; LTrigger.transform.position = Position2.position; break;
                case 2: SavedInputs.brake = "TriggersL_1"; LTrigger.transform.position = Position3.position; break;
                case 3: SavedInputs.rotateControl = "TriggersL_1"; LTrigger.transform.position = Position4.position; break;
                case 4: SavedInputs.restartLevel = "TriggersL_1"; LTrigger.transform.position = Position5.position; break;
                default: break;
            }
        }
    }

    public void SetOnStart()
    {
        InputHandler.LoadInputs();
        if(SavedInputs.boost == "A_1")
            A.transform.position = Position1.position;
        else if(SavedInputs.boost == "B_1")
            B.transform.position = Position1.position;
        else if (SavedInputs.boost == "X_1")
            X.transform.position = Position1.position;
        else if (SavedInputs.boost == "Y_1")
            Y.transform.position = Position1.position;
        else if (SavedInputs.boost == "LB_1")
            LB.transform.position = Position1.position;
        else if (SavedInputs.boost == "RB_1")
            RB.transform.position = Position1.position;
        else if (SavedInputs.boost == "TriggersR_1")
            RTrigger.transform.position = Position1.position;
        else if (SavedInputs.boost == "TriggersL_1")
            LTrigger.transform.position = Position1.position;

        if (SavedInputs.gas == "A_1")
            A.transform.position = Position2.position;
        else if (SavedInputs.gas == "B_1")
            B.transform.position = Position2.position;
        else if (SavedInputs.gas == "X_1")
            X.transform.position = Position2.position;
        else if (SavedInputs.gas == "Y_1")
            Y.transform.position = Position2.position;
        else if (SavedInputs.gas == "LB_1")
            LB.transform.position = Position2.position;
        else if (SavedInputs.gas == "RB_1")
            RB.transform.position = Position2.position;
        else if (SavedInputs.gas == "TriggersR_1")
            RTrigger.transform.position = Position2.position;
        else if (SavedInputs.gas == "TriggersL_1")
            LTrigger.transform.position = Position2.position;

        if (SavedInputs.brake == "A_1")
            A.transform.position = Position3.position;
        else if (SavedInputs.brake == "B_1")
            B.transform.position = Position3.position;
        else if (SavedInputs.brake == "X_1")
            X.transform.position = Position3.position;
        else if (SavedInputs.brake == "Y_1")
            Y.transform.position = Position3.position;
        else if (SavedInputs.brake == "LB_1")
            LB.transform.position = Position3.position;
        else if (SavedInputs.brake == "RB_1")
            RB.transform.position = Position3.position;
        else if (SavedInputs.brake == "TriggersR_1")
            RTrigger.transform.position = Position3.position;
        else if (SavedInputs.brake == "TriggersL_1")
            LTrigger.transform.position = Position3.position;

        if (SavedInputs.rotateControl == "A_1")
            A.transform.position = Position4.position;
        else if (SavedInputs.rotateControl == "B_1")
            B.transform.position = Position4.position;
        else if (SavedInputs.rotateControl == "X_1")
            X.transform.position = Position4.position;
        else if (SavedInputs.rotateControl == "Y_1")
            Y.transform.position = Position4.position;
        else if (SavedInputs.rotateControl == "LB_1")
            LB.transform.position = Position4.position;
        else if (SavedInputs.rotateControl == "RB_1")
            RB.transform.position = Position4.position;
        else if (SavedInputs.rotateControl == "TriggersR_1")
            RTrigger.transform.position = Position4.position;
        else if (SavedInputs.rotateControl == "TriggersL_1")
            LTrigger.transform.position = Position4.position;

        if (SavedInputs.restartLevel == "A_1")
            A.transform.position = Position5.position;
        else if (SavedInputs.restartLevel == "B_1")
            B.transform.position = Position5.position;
        else if (SavedInputs.restartLevel == "X_1")
            X.transform.position = Position5.position;
        else if (SavedInputs.restartLevel == "Y_1")
            Y.transform.position = Position5.position;
        else if (SavedInputs.restartLevel == "LB_1")
            LB.transform.position = Position5.position;
        else if (SavedInputs.restartLevel == "RB_1")
            RB.transform.position = Position5.position;
        else if (SavedInputs.restartLevel == "TriggersR_1")
            RTrigger.transform.position = Position5.position;
        else if (SavedInputs.restartLevel == "TriggersL_1")
            LTrigger.transform.position = Position5.position;
    }

    public void CheckForOverlap(int input)
    {
        switch (input)
        {
            case 0: SharingSpace(Position1).transform.position = ButtonWaitArea.position;
                return;
            case 1: SharingSpace(Position2).transform.position = ButtonWaitArea.position;
                return;
            case 2: SharingSpace(Position3).transform.position = ButtonWaitArea.position;
                return;
            case 3: SharingSpace(Position4).transform.position = ButtonWaitArea.position;
                return;
            case 4: SharingSpace(Position5).transform.position = ButtonWaitArea.position;
                return;
            default: return;




        }
    }

    public GameObject SharingSpace(Transform position)
    {
        Debug.Log("SharingSpace");
        if (position.position == A.transform.position)
            return A;
        if( position.position == B.transform.position)
            return B;
        if (position.position == X.transform.position)
            return X;
        if (position.position == Y.transform.position)
            return Y;
        if (position.position == LTrigger.transform.position)
            return LTrigger;
        if (position.position == RTrigger.transform.position)
            return RTrigger;
        if (position.position == LB.transform.position)
            return LB;
        if (position.position == RB.transform.position)
            return RB;
        return ButtonWaitArea.gameObject;
    }
            
        
    
}
