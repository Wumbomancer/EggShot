using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsMenu : MonoBehaviour {

    public enum GameState { BOOST, GAS, BRAKE, ROTATECONTROL, RESTARTLEVEL, PREVIEW, BACK };

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
    public GameObject TriggersL;
    public GameObject TriggersR;
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

    private Dictionary<string, GameObject> controlNameDict;
    private LinkedListNode<GameState> CurrentState;
    private LinkedList<GameState> list;
    private IEnumerator coroutine;
    private bool coroutineActive;

    // Use this for initialization
    void Start () {
        coroutineActive = true;
        isActive = false;
        timeDelay = 0f;

        //Tracking state using a doubly linked list
        PopulateGameObjectDLL();
        //Creating dictionary to deal with GameObject aliases
        PopulateControlDict();

        SetOnStart();

    }
	
	// Update is called once per frame
    void Update () {
        RunState();
        timeDelay += Time.deltaTime;
    }

    private void RunState() {
        if (isActive && timeDelay > .15f)
        {
            if (Input.GetButtonDown("B_1"))
                Return();
            switch (CurrentState.Value)
            {
                case GameState.BOOST: GameStateCoRout(0, null, Boost, Gas);return;
                case GameState.GAS: GameStateCoRout(1, Boost, Gas, Brake);return;
                case GameState.BRAKE: GameStateCoRout(2, Gas, Brake, RT);return;
                case GameState.ROTATECONTROL: GameStateCoRout(3, Brake, RT, RL);return;
                case GameState.RESTARTLEVEL: GameStateCoRout(4, RT, RL, Preview);return;
                case GameState.PREVIEW: GameStateCoRout(-1, RL, Preview, Back);return;
                case GameState.BACK: GameStateCoRout(-2, Preview, Back, null);return;

                default:return;
            }
           
        }
    }

    private void GameStateCoRout(int buttonInput, GameObject prevField, GameObject currField, GameObject nextField )
    {
        if (Input.GetButtonDown("A_1") && buttonInput > -1)
            { coroutine = ButtonInput(buttonInput); StartCoroutine(coroutine); isActive = false; }
        else if (Input.GetButtonDown("A_1") && buttonInput == -1)
            TestTrack();
        else if (Input.GetButtonDown("A_1") && buttonInput == -2)
            Return();
        
        if (Input.GetAxis("L_YAxis_1") > 0 && nextField != null)
        {
            CurrentState = CurrentState.Next ;
            //print(CurrentState.Value);
            currField.GetComponent<MeshRenderer>().material = NonHighlightMaterial;

            timeDelay = 0f;
            nextField.GetComponent<MeshRenderer>().material = HighlightMaterial;
        }

        if (Input.GetAxis("L_YAxis_1") < 0 && prevField != null)
        {
            CurrentState = CurrentState.Previous;
            //print(CurrentState.Value);
            currField.GetComponent<MeshRenderer>().material = NonHighlightMaterial;

            timeDelay = 0f;
            prevField.GetComponent<MeshRenderer>().material = HighlightMaterial;
        }
    }

    private void Return()
    {
        isActive = false;
        FindObjectOfType<MainCameraScript>().ClearFlags();
        FindObjectOfType<MainCameraScript>().movetoOptions = true;
        FindObjectOfType<MainCameraScript>().timeDelay = 0f;
        FindObjectOfType<OptionsMenu>().timeDelay = -.1f;
        FindObjectOfType<OptionsMenu>().isActive = true;
    }

    IEnumerator ButtonInput(int input)
    {
        if (coroutineActive)
        {
           // Debug.Log("Starting Coroutine");
            yield return new WaitForSeconds(.1f);
           // Debug.Log("Finished Waiting");
           
            while (!Input.GetButtonDown("A_1") && !Input.GetButtonDown("B_1") && !Input.GetButtonDown("Y_1") && !Input.GetButtonDown("X_1")
                && !Input.GetButtonDown("LB_1") && !Input.GetButtonDown("RB_1") && !(Input.GetAxis("TriggersL_1") > 0) && !(Input.GetAxis("TriggersR_1") < 0))
                yield return null;

            ControlToAlter(input);
            
           // Debug.Log("Finish Coroutine");
            InputHandler.SaveInputs();
            isActive = true;
        }
        else
            coroutineActive = true;
    }

    private void ControlToAlter(int input)
    {
        foreach (string i in controlNameDict.Keys)
        {
            string buttonName = i + "_1";
            if (Input.GetButtonDown(buttonName))
            {
                AlterControl(buttonName, controlNameDict[i], input);
                break;
            }
            else if (buttonName == "TriggersL_1")
            {
                if (Input.GetAxis("TriggersL_1") > 0)
                {
                    AlterControl(buttonName, controlNameDict[i], input);
                }
            }
            else if(buttonName == "TriggersR_1")
            { 
                if(Input.GetAxis("TriggersR_1") < 0)
                {
                    AlterControl(buttonName, controlNameDict[i], input);
                }
            }
        } 
    }

    private void AlterControl(string ctrl, GameObject buttonObject, int input)
    {

        switch (input)
        {
            case 0: CheckForOverlap(ctrl, Position1, SavedInputs.boost, buttonObject); break;
            case 1: CheckForOverlap(ctrl, Position2, SavedInputs.gas, buttonObject); break;
            case 2: CheckForOverlap(ctrl, Position3, SavedInputs.brake, buttonObject); break;
            case 3: CheckForOverlap(ctrl, Position4, SavedInputs.rotateControl, buttonObject); break;
            case 4: CheckForOverlap(ctrl, Position5, SavedInputs.restartLevel, buttonObject); break;
            default: break;
        }
    }

    private void CheckForOverlap(string ctrl, Transform position, string savedInput, GameObject buttonObject)
    {
        SharingSpace(position).transform.position = ButtonWaitArea.position;
        savedInput = ctrl; buttonObject.transform.position = position.position;
    }

    private GameObject SharingSpace(Transform position)
    {
        print("SharingSpace");
        foreach (GameObject buttonGraphic in controlNameDict.Values)
        {
            print(buttonGraphic.ToString());
            print(position.position);
            if (Vector3.Distance(position.position, buttonGraphic.transform.position) < 0.5)
            {
                print("Shared space control: "+buttonGraphic.ToString());
                return buttonGraphic;
            }
        }
      
        return ButtonWaitArea.gameObject;
    }

    private void TestTrack()
    {
        Debug.Log("Moving from controls to preview");
        FindObjectOfType<MainCameraScript>().movetoPreview = true;
        FindObjectOfType<MainCameraScript>().timeDelay = 0f;
        FindObjectOfType<MainMenuCarTestTrack>().CarSpawn();
         
        
        isActive = false;
    }

    private void SetOnStart()
    {
        InputHandler.LoadInputs();
        CheckInputControl(SavedInputs.boost, Position1);
        CheckInputControl(SavedInputs.gas, Position2);
        CheckInputControl(SavedInputs.brake, Position3);
        CheckInputControl(SavedInputs.rotateControl, Position4);
        CheckInputControl(SavedInputs.restartLevel, Position5);
    }

    private void CheckInputControl(string input, Transform panelPosition)
    {
        string button = input.Remove(input.Length - 2);
        //print(button);
        GameObject buttonGraphic = controlNameDict[button.Trim()];
        //print(buttonGraphic.ToString());
        buttonGraphic.transform.position = panelPosition.position;
    }
    
    private void PopulateControlDict()
    {
        controlNameDict = new Dictionary<string, GameObject>
        {
            { "A", A },
            { "B", B },
            { "X", X },
            { "Y", Y },
            { "LB", LB },
            { "RB", RB },
            { "TriggersL", TriggersL },
            { "TriggersR", TriggersR }
        };
    }

    private void PopulateGameObjectDLL()
    {
        list = new LinkedList<GameState>();
        foreach (GameState state in (GameState[])System.Enum.GetValues(typeof(GameState)))
        {

            LinkedListNode<GameState> node = new LinkedListNode<GameState>(state);
            list.AddLast(new LinkedListNode<GameState>(state));

        }
        CurrentState = list.First;
        Boost.GetComponent<MeshRenderer>().material = HighlightMaterial;
    }
    


}
