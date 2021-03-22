using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MenuItemMaster :MonoBehaviour
{
    public string itemName;
    public GameObject objectReference;
    public Material hoverMaterial;
    public Material notHoverMaterial;
    private bool isHover;

    public bool IsHover
    {
        set
        {
            isHover = value;
            if(isHover == true)
            {
                GetComponent<MeshRenderer>().material = hoverMaterial;
                transform.localScale = transform.localScale * 1.2f;
            }
            else
            {
                GetComponent<MeshRenderer>().material = notHoverMaterial;
                transform.localScale = transform.localScale / 1.2f;
            }  
        }
        get
        {
            return isHover;
        }
    }

    abstract public void Select();
    abstract public void Deselect();
    abstract public void LeftState();
    abstract public void RightState();
    abstract public void DownState();
    abstract public void UpState();
    

    






}
