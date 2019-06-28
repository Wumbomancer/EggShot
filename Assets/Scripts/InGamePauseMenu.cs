using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class InGamePauseMenu : MonoBehaviour {

    public enum MenuState { Resume, Options, Exit};
    public GameObject canvas;
    public GameObject Resume;
    public GameObject Options;
    public GameObject Exit;
    public bool exitMenu;

    public Material Highlighted;
    public Material NonHighlighted;

    public MenuState menustate;
    
    public void Start()
    {
        menustate = MenuState.Resume;
        Resume.GetComponent<MeshRenderer>().material = Highlighted;
        StartCoroutine(OperatePauseMenu());
        
    }

    

    public IEnumerator OperatePauseMenu()
    {

        while (!Input.GetButtonDown("Start_1"))
        {
            
            yield return null;
        }
        StartCoroutine(MovingInMenu(menustate));
        exitMenu = false;
        StartCoroutine(InputCheck());
        canvas.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        FindObjectOfType<LightBulbIllumination>().reset = true;
        while (!exitMenu)
            yield return null;
        canvas.gameObject.SetActive(false);
        ExitMenu();
    }

    public IEnumerator MovingInMenu(MenuState State)
    {
        
        while (Input.GetAxis("L_YAxis_1") == 0)
        {
            yield return null;
        }
        float input = Input.GetAxis("L_YAxis_1");
        if (input < 0)
            MoveUp(State);
        else
        {
            MoveDown(State);
        }
        Highlight(menustate);

        yield return new WaitForSeconds(.2f);
        StartCoroutine(MovingInMenu(menustate));
        
    }

    public IEnumerator InputCheck()
    {
        while (!Input.GetButtonDown("A_1"))
        {
            yield return null;

        }
        Debug.Log("Action");

        Action();
        yield return new WaitForFixedUpdate();
        StartCoroutine(InputCheck());
        
    }

    public void MoveUp(MenuState State)
    {
        if (State == MenuState.Exit)
            menustate = MenuState.Options;
        if (State == MenuState.Options)
            menustate = MenuState.Resume;

    }

    public void MoveDown(MenuState State)
    {
        if (State == MenuState.Resume)
            menustate = MenuState.Options;
        if (State == MenuState.Options)
            menustate = MenuState.Exit;

    }

    public void Highlight(MenuState State)
    {
        switch (State)
        {
            case MenuState.Resume:
                Resume.GetComponent<MeshRenderer>().material = Highlighted;
                Options.GetComponent<MeshRenderer>().material = NonHighlighted;
                Exit.GetComponent<MeshRenderer>().material = NonHighlighted;
                break;
            case MenuState.Options:
                Resume.GetComponent<MeshRenderer>().material = NonHighlighted;
                Options.GetComponent<MeshRenderer>().material = Highlighted;
                Exit.GetComponent<MeshRenderer>().material = NonHighlighted;
                break;
            case MenuState.Exit:
                Resume.GetComponent<MeshRenderer>().material = NonHighlighted;
                Options.GetComponent<MeshRenderer>().material = NonHighlighted;
                Exit.GetComponent<MeshRenderer>().material = Highlighted;
                break;
            default:
                break;
        }
    }

    public void Action()
    {
        switch (menustate)
        {
            case MenuState.Resume: exitMenu = true;
                break;
            case MenuState.Options: 
                break;
            case MenuState.Exit:
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("MainMenu");
                break;
            default:
                break;
        }
    }

    public void ExitMenu()
    {
        StopAllCoroutines();
        StartCoroutine(OperatePauseMenu());
    }
}
