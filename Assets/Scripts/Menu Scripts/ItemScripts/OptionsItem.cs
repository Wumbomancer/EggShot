using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsItem : MenuItemMaster
{
    public override void Select()
    {
        MenuController.Instance.ChangeCurrentState("Sound");
        MenuController.Instance.ChangeCamera(2);
    }
    public override void Deselect()
    {
        //Do nothing
    }
    public override void UpState()
    {
        MenuController.Instance.ChangeCurrentState("Multiplayer");
    }
    public override void DownState()
    {
        MenuController.Instance.ChangeCurrentState("Exit");
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
