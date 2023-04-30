using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

public class resource_generation : MonoBehaviour
{

    public int max_resource = 0;
    public int current_resource = 0;
    public int last_resource = 1000000000;
    public int building_id = -1;

    public string resource_type;

    public double max_time = 0; 
    public double current_time = 0; 
    public double elapsed_time = 0; 
    public TextMeshProUGUI  timer;
    public TextMeshProUGUI  resources;
    
    public int building_index = -1;


    IEnumerator Start() {
        while(elapsed_time == 0) {
            yield return null;
        }
        
        //Debug.Log("Old Current Time: " + current_time);
        current_time -= elapsed_time; 
        //Debug.Log("New Current Time: " + current_time);

        //Loops through to see how many resources the user has.
        for (int i = 1; i < max_resource; i ++) {
           
            if (current_time < 0) {
                //Debug.Log("Added---");
                current_resource += 1;
                current_time += max_time;
            } 
        }
    }


    // Update is called once per frame
    void Update()
    {

        resources.text = "[" + current_resource.ToString() + "]";
        if (current_time > 0)
        {
            current_time -= Time.deltaTime;

            int ui_time = (int)Mathf.Round((float)current_time);
            //Take Time and Convert it into Readable Data
            int seconds = ui_time % 60;
            int minutes = (int)Mathf.Floor(ui_time / 60); 
            int hours = (int)Mathf.Floor(minutes / 60); 

            timer.text = string.Format("{0:d2}:{1:d2}:{2:d2}", hours, minutes % 60, seconds);

        } else if (current_resource <= max_resource) {
            current_resource += 1;
            current_time = max_time;
        }


        //Debug.Log("Current Resources: " + current_resource + " Building_id: " + building_id);
        //Debug.Log("Last Resources: " + last_resource);

        //Update Building Data
        if ((last_resource != current_resource) && (building_id != -1)) {
            //Debug.Log("---------HERE-------------: " + building_id);
            string user_save_data = Application.persistentDataPath + "/user_data.json";
            JObject user_data = JObject.Parse(File.ReadAllText(@user_save_data));

            JArray building_obj = (JArray)user_data["building_obj"];
            user_data["building_obj"][building_id]["storage"] = current_resource;

            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
        }

        last_resource = current_resource;

        /// ---- TODO: Add UI element showing resources are full --- ////
    }
}
