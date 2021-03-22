using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevelItem : MenuItemMaster
{
    public override void Select()
    {
        MenuController.Instance.ChangeCurrentState("Sound");
        MenuController.Instance.ChangeCamera(2);
    }
    public override void Deselect()
    {
        MenuController.Instance.ChangeCurrentState("Sound");
        MenuController.Instance.ChangeCamera(2);
    }
    public override void UpState()
    {
        MenuController.Instance.ChangeCurrentState("Rotate Control");
    }
    public override void DownState()
    {
        MenuController.Instance.ChangeCurrentState("Preview");
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
