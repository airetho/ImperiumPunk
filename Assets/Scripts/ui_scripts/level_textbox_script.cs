using UnityEngine;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;


public class level_textbox_script : MonoBehaviour
{
    public GameObject[] level_textbox;

    public string state = "closed";

    private AudioSource sources;
    

   
    public void open_level_textbox_canvas()
    {
        sources = GetComponent<AudioSource>();
        sources.Play();
        //Used to keep the player from building "buildings" out of order.
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        string user_data_string = File.ReadAllText(user_save_data);
        JObject data_file = JObject.Parse(user_data_string);

        int level = (int)data_file["user_level"] - 1;

        if (state == "closed") {
        
            state = "open";
            level_textbox[level].SetActive(true);
        } else {
            
            state = "closed";
            level_textbox[level].SetActive(false);
        }
    }
}