using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControlItem : MenuItemMaster
{
    public override void Select()
    {
        //Call Control manager to change controls
    }
    public override void Deselect()
    {
        MenuController.Instance.ChangeCurrentState("Sound");
        MenuController.Instance.ChangeCamera(2);
    }
    public override void UpState()
    {
        MenuController.Instance.ChangeCurrentState("Brake");
    }
    public override void DownState()
    {
        MenuController.Instance.ChangeCurrentState("Restart Level");
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
