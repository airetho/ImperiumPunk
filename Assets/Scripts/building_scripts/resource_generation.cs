using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class resource_generation : MonoBehaviour
{

    public int max_resource = 3;
    public int current_resource = 1;
    public string resource_type;

    public float max_time = 65; 
    public float current_time = 65; 
    public TextMeshProUGUI  timer;
    public TextMeshProUGUI  resources;

    // Update is called once per frame
    void Update()
    {
        resources.text = "[" + current_resource.ToString() + "]";
        if (current_time > 0)
        {
            current_time -= Time.deltaTime;

            int ui_time = (int)Mathf.Round(current_time);
            //Take Time and Convert it into Readable Data
            int seconds = ui_time % 60;
            int minutes = (int)Mathf.Floor(ui_time / 60); 
            int hours = (int)Mathf.Floor(minutes / 60); 

            timer.text = string.Format("{0:d2}:{1:d2}:{2:d2}", hours, minutes % 60, seconds);

        } else if (current_resource <= max_resource) {
            current_resource += 1;
            current_time = max_time;
        }

        /// ---- TODO: Add UI element showing resources are full ---
    }
}
