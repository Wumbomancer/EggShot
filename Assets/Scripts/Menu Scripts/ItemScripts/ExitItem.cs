using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitItem : MenuItemMaster
{ 
    public override void Select()
    {
        Application.Quit();
    }
    public override void Deselect()
    {
        //Do nothing
    }
    public override void UpState()
    {
        MenuController.Instance.ChangeCurrentState("Options");
    }
    public override void DownState()
    {
        //Do nothing
    }
    public override void RightState()
    {
        //Do nothing
    }
    public override void LeftState()
    {
        //Do nothing
    }
}
