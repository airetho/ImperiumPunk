using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resource_ui_select : MonoBehaviour
{
    public Transform resource_ui;
    public string resource_ui_state = "closed";
    public float increment; 

    // Start is called before the first frame update
    public void switching_ui_state()
    {
        if (resource_ui_state == "closed")
        {
            resource_ui_state = "open";

            var y = Screen.width / increment;
            resource_ui.position += Vector3.left * y;
            
        } else if (resource_ui_state == "open")
        {
            resource_ui_state = "closed";
            var y = Screen.width / increment;
            resource_ui.position += Vector3.right * y;
        } 
    }
}        