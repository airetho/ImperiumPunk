using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class level_textbox_script : MonoBehaviour
{
    public GameObject level_textbox_canvas;

    public string state = "closed";

   
    public void open_level_textbox_canvas()
    {

        if (state == "closed") {
            state = "open";
            level_textbox_canvas.SetActive(true);
        } else {
            state = "closed";
            level_textbox_canvas.SetActive(false);
        }
        
    }
}