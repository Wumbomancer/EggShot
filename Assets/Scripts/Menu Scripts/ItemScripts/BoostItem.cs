using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostItem : MenuItemMaster
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
        //Do nothing
    }
    public override void DownState()
    {
        MenuController.Instance.ChangeCurrentState("Gas");
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
