using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerItem : MenuItemMaster
{
    public override void Select()
    {
        //Do nothing right now
    }
    public override void Deselect()
    {
        //Do nothing
    }
    public override void UpState()
    {
        MenuController.Instance.ChangeCurrentState("Play");
    }
    public override void DownState()
    {
        MenuController.Instance.ChangeCurrentState("Options");
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
