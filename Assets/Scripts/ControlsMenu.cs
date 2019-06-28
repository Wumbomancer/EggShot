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

    public GameState CurrentState;
    private IEnumerator coroutine;
    private bool coroutineActive;


    // Use this for initialization
    void Start () {
        isActive = false;
        timeDelay = 0f;
        CurrentState = GameState.BOOST;
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
                    GameStateCoRout(0, null, Boost, Gas);
                    return;
                case GameState.GAS:
                    GameStateCoRout(1, Boost, Gas, Brake);
                    return;
                case GameState.BRAKE:
                    GameStateCoRout(2, Gas, Brake, RT);
                    return;
                case GameState.ROTATECONTROL:
                    GameStateCoRout(3, Brake, RT, RL);
                    return;
                case GameState.RESTARTLEVEL:
                    GameStateCoRout(4, RT, RL, Preview);
                    return;
                case GameState.PREVIEW:
                    GameStateCoRout(-1, RL, Preview, Back);        
                    return;
                case GameState.BACK:
                    GameStateCoRout(-2, Preview, Back, null);
                    return;

                default:
                    return;
            }   

        }
            
        timeDelay += Time.deltaTime;
    }

    public void GameStateCoRout(int buttonInput, GameObject prevField, GameObject currField, GameObject nextField )
    {
        if (Input.GetButtonDown("A_1") && buttonInput > -1)
            { coroutine = ButtonInput(buttonInput); StartCoroutine(coroutine); isActive = false; }
        else if (Input.GetButtonDown("A_1") && buttonInput == -1)
            TestTrack();
        else if (Input.GetButtonDown("A_1") && buttonInput == -2)
            Return();
        
        if (Input.GetAxis("L_YAxis_1") > 0 && nextField != null)
        {
            CurrentState = GameState.BRAKE;
            currField.GetComponent<MeshRenderer>().material = NonHighlightMaterial;

            timeDelay = 0f;
            nextField.GetComponent<MeshRenderer>().material = HighlightMaterial;
        }

        if (Input.GetAxis("L_YAxis_1") < 0 && prevField != null)
        {
            CurrentState = GameState.BOOST;
            currField.GetComponent<MeshRenderer>().material = NonHighlightMaterial;

            timeDelay = 0f;
            prevField.GetComponent<MeshRenderer>().material = HighlightMaterial;

        }
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


            ControlToAlter(input);
            

            Debug.Log("Finish Coroutine");
            InputHandler.SaveInputs();
            isActive = true;
        }
        else
            coroutineActive = true;
    }

    public void ControlToAlter(int input)
    {

        if (Input.GetButtonDown("A_1"))
        {
            AlterControl("A_1", A, input);
        }
        else if (Input.GetButtonDown("B_1"))
        {
            AlterControl("B_1", B, input);
        }
        else if (Input.GetButtonDown("Y_1"))
        {
            AlterControl("Y_1", Y, input);
        }
        else if (Input.GetButtonDown("X_1"))
        {
            AlterControl("X_1", X, input);
        }
        else if (Input.GetButtonDown("RB_1"))
        {
            AlterControl("RB_1", RB, input);
        }
        else if (Input.GetButtonDown("LB_1"))
        {
            AlterControl("LB_1", LB, input);
        }
        else if (Input.GetAxis("TriggersR_1") < 0)
        {
            AlterControl("TriggersR_1", RTrigger, input);
        }
        else if (Input.GetAxis("TriggersL_1") > 0)
        {
            AlterControl("TriggersL_1", LTrigger, input);
        }
    }

    public void AlterControl(string ctrl, GameObject buttonObject, int input)
    {
        CheckForOverlap(input);
        switch (input)
        {
            case 0: SavedInputs.boost = ctrl; buttonObject.transform.position = Position1.position; break;
            case 1: SavedInputs.gas = ctrl; buttonObject.transform.position = Position2.position; break;
            case 2: SavedInputs.brake = ctrl; buttonObject.transform.position = Position3.position; break;
            case 3: SavedInputs.rotateControl = ctrl; buttonObject.transform.position = Position4.position; break;
            case 4: SavedInputs.restartLevel = ctrl; buttonObject.transform.position = Position5.position; break;
            default: break;
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

    public void TestTrack()
    {
        FindObjectOfType<MainCameraScript>().movetoPreview = true;
        FindObjectOfType<MainCameraScript>().timeDelay = 0f;
        isActive = false;
        Car.SetActive(true);
    }

}
