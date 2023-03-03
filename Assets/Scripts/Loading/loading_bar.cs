using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loading_bar : MonoBehaviour
{

    public Image loading_bar_fill;
    
    // Update is called once per frame
    void Update()
    {
        if (loading_bar_fill.fillAmount < 0.3f){
            
            loading_bar_fill.fillAmount += 0.0025f;

        }
    }
}
