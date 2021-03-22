using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsItem : MenuItemMaster
{
    public override void Select()
    {
        MenuController.Instance.ChangeCurrentState("Boost");
        MenuController.Instance.ChangeCamera(3);
    }
    public override void Deselect()
    {
        MenuController.Instance.ChangeCurrentState("Play");
        MenuController.Instance.ChangeCamera(1);
    }
    public override void UpState()
    {
        MenuController.Instance.ChangeCurrentState("Sound");
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
