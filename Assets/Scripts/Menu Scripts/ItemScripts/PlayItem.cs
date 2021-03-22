using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayItem : MenuItemMaster
{
    public override void Select()
    {
        //Launch the level selector
    }
    public override void UpState()
    {
        //Do nothing
    }
    public override void DownState()
    {
        MenuController.Instance.ChangeCurrentState("Multiplayer");
    }
    public override void RightState()
    {
        //do nothing
    }
    public override void LeftState()
    {
        //Do nothing
    }
    public override void Deselect()
    {
        //Do nothing
    }
}
