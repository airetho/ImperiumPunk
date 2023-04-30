using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;



public class open_menu : MonoBehaviour
{
    public GameObject level_textbox;
    public string state = "closed";
    public string type = null;
    public static bool tutorial_achievement = false;

    private AudioSource source;
    

    void Start() {
        source = GetComponent<AudioSource>();

        //If Tutorial Menu
        if (type == "tutorial") {
            //Boot up for 1-level players.
            string user_save_data = Application.persistentDataPath + "/user_data.json";
            string user_data_string = File.ReadAllText(user_save_data);
            JObject data_file = JObject.Parse(user_data_string);

            int level = (int)data_file["user_level"];
                
            //Add Level
            if (level < 2) {
                state = "open";
                level_textbox.SetActive(true);
            }
        }
    }

   
    public void open_level_textbox_canvas()
    {
        source.Play();

        if (state == "closed") {
            state = "open";
            level_textbox.SetActive(true);
            if (type == "tutorial") {
                tutorial_achievement = true;
            }

        } else if (type != "tutorial" ){
            state = "closed";
            level_textbox.SetActive(false);
        }
        
    }

    public void open_canvas()
    {
        source = GetComponent<AudioSource>();
        source.Play();
        level_textbox.SetActive(true);
    }
}