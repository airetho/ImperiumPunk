using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class loading_fade : MonoBehaviour
{
    private bool faded = false;
    public float duration = 0.4f;
    public GameObject loading_canvas;
    public Image loading_bar_fill;


   
    public void Update() {
        
        if (building_maker.buildings_built == true && loading_bar_fill.fillAmount < 0.9f){
            
            loading_bar_fill.fillAmount += 0.0025f;

        } else if (loading_bar_fill.fillAmount < 0.3f) {
            loading_bar_fill.fillAmount += 0.005f;
        }

        
        if (road_maker.roads_built & building_maker.buildings_built == true) {
            loading_bar_fill.fillAmount = 1f;
            var canvas_group = GetComponent<CanvasGroup>();
            StartCoroutine(do_fade(canvas_group, canvas_group.alpha, 0));
        }

    }

    public IEnumerator do_fade(CanvasGroup canvas_group, float start, float end) 
    {

        float counter = 0f;

        while(counter < duration)
        {
            counter += Time.deltaTime;
            canvas_group.alpha = Mathf.Lerp(start, end, counter / duration);
            faded = !faded;
            yield return null;
        }

        if (counter >= duration){
            loading_canvas.SetActive(false);
        }
    }  
}

