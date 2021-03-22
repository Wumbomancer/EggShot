using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundItem : MenuItemMaster
{
    public override void Select()
    {
        //Allow Sound Adjustment
    }
    public override void Deselect()
    {
        MenuController.Instance.ChangeCurrentState("Play");
        MenuController.Instance.ChangeCamera(1);
    }
    public override void UpState()
    {
        //Do nothing
    }
    public override void DownState()
    {
        MenuController.Instance.ChangeCurrentState("Controls");
    }
    public override void RightState()
    {
        //Same as select. Allow sound adjustment.
    }
    public override void LeftState()
    {
        //Do nothing
    }
}
