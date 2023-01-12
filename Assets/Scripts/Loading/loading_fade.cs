using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class loading_fade : MonoBehaviour
{
    private bool faded = false;
    public float duration = 0.4f;


   
    public void Update() {
        
        
        if (road_maker.roads_built & building_maker.buildings_built == true) {
            var canvas_group = GetComponent<CanvasGroup>();
            StartCoroutine(do_fade(canvas_group, canvas_group.alpha, 0));
            faded = !faded;
        }
    }

    public IEnumerator do_fade(CanvasGroup canvas_group, float start, float end) 
    {

        

        float counter = 0f;

        while(counter < duration)
        {
            counter += Time.deltaTime;
            canvas_group.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }

    }
    
}

